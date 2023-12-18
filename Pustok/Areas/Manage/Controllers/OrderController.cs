using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Pustok.Core.Enums;
using Pustok.Core.Models;
using Pustok.DAL;
using Pustok.Helpers;
using SignalRChat.Hubs;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="SuperAdmin,Admin")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _appDb;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(AppDbContext appdb, IHubContext<ChatHub> hubContext,UserManager<AppUser> userManager)
        {
            _appDb = appdb;
            _hubContext = hubContext;
            this._userManager = userManager;
        }

        public async Task<IActionResult> Index(int page =1)
        {
            var query  = _appDb.Orders.AsQueryable();
            //List<Order> orders = await _appDb.Orders.ToListAsync();
            PaginatedList<Order> paginatedOrder = new PaginatedList<Order>(query.Skip((page-1)*2).Take(2).ToList(),query.ToList().Count,page,2);
            return View(paginatedOrder);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Order order = await _appDb.Orders.Include(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id == id);
            if (order is null) return NotFound();

            return View(order);
        }

        public async Task<IActionResult> Accept(int id)
        {
            Order order = await _appDb.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (order is null) return NotFound();
            order.OrderStatus = OrderStatus.Accepted;

            await _appDb.SaveChangesAsync();

            if (order.AppUserId != null)
            {
                var user = await _userManager.FindByIdAsync(order.AppUserId);
                if (user != null)
                {
                    await _hubContext.Clients.Client(user.CoonectionId).SendAsync("OrderAccept");
                }
            }
                
            
            //_hubContext.Clients.Client()

            return RedirectToAction("index", "order");
        }

        public async Task<IActionResult> Pending(int id)
        {
            Order order = await _appDb.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (order is null) return NotFound();
            order.OrderStatus = OrderStatus.Pending;

            await _appDb.SaveChangesAsync();

            return RedirectToAction("index", "order");
        }

        public async Task<IActionResult> Reject(int id,string AdminComment)
        {
            Order order = await _appDb.Orders.Include(x=>x.OrderItems).FirstOrDefaultAsync(x => x.Id == id);
            if (order is null) return NotFound();
            if(AdminComment == null)
            {
                ModelState.AddModelError("AdminComment", "Must be written");
                return View("detail",order);
            }
            order.OrderStatus = OrderStatus.Rejected;
            order.AdminComment = AdminComment;

            await _appDb.SaveChangesAsync();

            if (order.AppUserId != null)
            {
                var user = await _userManager.FindByIdAsync(order.AppUserId);
                if (user != null)
                {
                    await _hubContext.Clients.Client(user.CoonectionId).SendAsync("OrderReject",AdminComment);
                }
            }

            return RedirectToAction("index", "order");
        }
    }
}
