using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok.Core.Models;
using Pustok.DAL;
using Pustok.Repositories.Interfaces;
using Pustok.Services;
using Pustok.ViewModels;
using PustokSliderCRUD.Models;

namespace Pustok.Controllers
{
    public class ProductController : Controller
    {

        private readonly IBookRepository _bookRepository;
        private readonly IBookService _bookService;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public ProductController(IBookRepository bookRepository,
                    IBookService bookService,
                    UserManager<AppUser> userManager,
                    AppDbContext context)
        {
            _bookRepository = bookRepository;
            _bookService = bookService;
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            Book book = await _bookService.GetAsync(id);
            ProductDetailViewModel productDetailViewModel = new ProductDetailViewModel()
            {
                Book = book,
                ReleatedBooks = await _bookService.GetAllRelatedBooksAsync(book)
            };

            return View(productDetailViewModel);
        }
        public async Task<IActionResult> GetBookModal(int id)
        {
            var book = await _bookService.GetAsync(id);

            return PartialView("_BookModalPartial", book);
        }

        //public IActionResult SetSession(string name)
        //{

        //    HttpContext.Session.SetString("Name", name);

        //    return Content("Added");
        //}

        //public IActionResult GetSession()
        //{
        //    string username= HttpContext.Session.GetString("Name");
        //    return Content(username);
        //}

        //public IActionResult SetCookie(int id) 
        //{
        //    List<int> ids = new List<int>();

        //    string idsStr = HttpContext.Request.Cookies["UserId"];

        //    if(idsStr is not null) 
        //    {
        //        ids = JsonConvert.DeserializeObject<List<int>>(idsStr);
        //    }

        //    ids.Add(id);

        //     idsStr = JsonConvert.SerializeObject(ids);   

        //    HttpContext.Response.Cookies.Append("UserId", idsStr);

        //    return Content("Added");
        //}

        //public IActionResult GetCookie()
        //{

        //    List<int> ids = new List<int>();

        //    string idsIdStr= HttpContext.Request.Cookies["UserId"];

        //    if(idsIdStr is not null)
        //    {
        //        ids = JsonConvert.DeserializeObject<List<int>>(idsIdStr);
        //    }



        //    return Json(ids);
        //}


        public async Task<IActionResult> AddToBasket(int bookId)
        {
            if (!_bookRepository.Table.Any(x => x.Id == bookId)) return NotFound();

            List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();
            BasketItemViewModel basketItem = null;
            BasketItem userBasketItem = null;
            AppUser user = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            }
            if (user == null)
            {
                string basketItemStr = HttpContext.Request.Cookies["BasketItems"];
                if (basketItemStr is not null)
                {

                    basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemStr);
                    basketItem = basketItems.FirstOrDefault(x => x.BookId == bookId);
                    if (basketItem is not null)
                    {
                        basketItem.Count++;
                    }
                    else
                    {
                        basketItem = new BasketItemViewModel
                        {
                            BookId = bookId,
                            Count = 1
                        };

                        basketItems.Add(basketItem);

                    }

                }
                else
                {
                    basketItem = new BasketItemViewModel
                    {
                        BookId = bookId,
                        Count = 1
                    };

                    basketItems.Add(basketItem);
                }



                basketItemStr = JsonConvert.SerializeObject(basketItems);

                HttpContext.Response.Cookies.Append("BasketItems", basketItemStr);
            }
            else
            {
                userBasketItem  =await _context.BasketItems.FirstOrDefaultAsync(x=>x.BookId == bookId && x.AppUserId == user.Id );
                if (userBasketItem is not null)
                {
                    userBasketItem.Count++; 
                }
                else
                {
                    userBasketItem = new BasketItem
                    {
                        BookId = bookId,
                        Count = 1,
                        AppUserId = user.Id,
                        IsDeleted = false
                    };
                    _context.BasketItems.Add(userBasketItem);
                }
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        public IActionResult GetBasketItems()
        {
            List<BasketItemViewModel> basketItemList = new List<BasketItemViewModel>();

            string basketItemListStr = HttpContext.Request.Cookies["BasketItems"];

            if(basketItemListStr is not null)
            {
                basketItemList = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemListStr);
            }

            

            return Json(basketItemList);
        }

        public async Task<IActionResult> Checkout()
        {
            List<CheckOutViewModel> checkoutItemList = new List<CheckOutViewModel>();
            List<BasketItemViewModel> basketItemList = new List<BasketItemViewModel>();
            List<BasketItem> userBasketItem = new List<BasketItem>();
            CheckOutViewModel checkoutItem = null;
            AppUser user = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            }
            if (user == null)
            {
                string basketItemListStr = HttpContext.Request.Cookies["BasketItems"];
                if (basketItemListStr != null)
                {
                    basketItemList = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemListStr);

                    foreach (var item in basketItemList)
                    {
                        checkoutItem = new CheckOutViewModel
                        {
                            Book = await _bookRepository.GetByIdAsync(x => x.Id == item.BookId),
                            Count = item.Count
                        };
                        checkoutItemList.Add(checkoutItem);
                    }
                }
            }
            else
            {
                userBasketItem =await _context.BasketItems.Include(x=>x.Book).Where(x=>x.AppUserId == user.Id).ToListAsync();
                foreach (var item in userBasketItem)
                {
                    checkoutItem = new CheckOutViewModel
                    {
                        Book = item.Book,
                        Count = item.Count
                    };
                    checkoutItemList.Add(checkoutItem);
                }
            }
            OrderViewModel orderViewModel = new OrderViewModel
            {
                CheckOutVM = checkoutItemList,
            };



            return View(orderViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(OrderViewModel orderViewModel)
		{
			List<CheckOutViewModel> checkoutItemList = new List<CheckOutViewModel>();
			List<BasketItemViewModel> basketItemList = new List<BasketItemViewModel>();
			List<BasketItem> userBasketItem = new List<BasketItem>();
			CheckOutViewModel checkoutItem = null;
			AppUser user = null;
            OrderItem orderItem = null;
			if (HttpContext.User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
			}
			Order order = new Order
            {
                FullName = orderViewModel.FullName,
                ZipCode = orderViewModel.ZipCode,
                Phone = orderViewModel.Phone,
                Email = orderViewModel.Email,
                Address = orderViewModel.Address,
                Note = orderViewModel.Note,
                Country = orderViewModel.Country,
                AppUserId = user?.Id
            };
			if (user == null)
			{
				string basketItemListStr = HttpContext.Request.Cookies["BasketItems"];
				if (basketItemListStr != null)
				{
					basketItemList = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemListStr);

					foreach (var item in basketItemList)
					{
                        Book book = _context.Books.FirstOrDefault(x => x.Id == item.BookId);
                        orderItem = new OrderItem
                        {
                            Book = book,
                            BookName = book.Name,
                            CostPrice = book.CostPrice,
                            DiscountPercent = book.DiscountedPrice,
                            SalePrice = book.SalePrice*((100-book.DiscountedPrice)/100),
                            Count = item.Count,
                            Order = order

                            
                        };
                        order.TotalProce = orderItem.SalePrice * orderItem.Count;
                        order.OrderItems.Add(orderItem);
					}
				}
			}
			else
			{
				userBasketItem = await _context.BasketItems.Include(x => x.Book).Where(x => x.AppUserId == user.Id).ToListAsync();
				foreach (var item in userBasketItem)
				{
					Book book = _context.Books.FirstOrDefault(x => x.Id == item.BookId);
					orderItem = new OrderItem
					{
						Book = book,
						BookName = book.Name,
						CostPrice = book.CostPrice,
						DiscountPercent = book.DiscountedPrice,
						SalePrice = book.SalePrice * ((100 - book.DiscountedPrice) / 100),
						Count = item.Count,
						Order = order


					};
					order.TotalProce = orderItem.SalePrice * orderItem.Count;
					order.OrderItems.Add(orderItem);
				}
			}
            await _context.Orders.AddAsync(order);
			await _context.SaveChangesAsync();

			return RedirectToAction("Index","Home");
        }
    }


}
