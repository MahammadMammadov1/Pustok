using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Core.Enums;
using Pustok.Core.Models;
using Pustok.DAL;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="SuperAdmin,Admin")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _appDb;

        public OrderController(AppDbContext appdb)
        {
            _appDb = appdb;
        }

        public async Task<IActionResult> Index()
        {
            List<Order> orders = await _appDb.Orders.ToListAsync();

            return View(orders);
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

        public async Task<IActionResult> Reject(int id)
        {
            Order order = await _appDb.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (order is null) return NotFound();
            order.OrderStatus = OrderStatus.Rejected;

            await _appDb.SaveChangesAsync();


            return RedirectToAction("index", "order");
        }
    }
}
