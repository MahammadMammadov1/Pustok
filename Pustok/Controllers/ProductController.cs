using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        public ProductController(IBookRepository bookRepository,IBookService bookService)
        {
            _bookRepository = bookRepository;
            _bookService = bookService;
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


        public IActionResult AddToBasket(int bookId)
        {
            if (!_bookRepository.Table.Any(x => x.Id == bookId)) return NotFound();

            List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();

            string basketItemStr = HttpContext.Request.Cookies["BasketItems"];
            BasketItemViewModel basketItem = null;
            if (basketItemStr is not null)
            {
            
                basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemStr);
                basketItem = basketItems.FirstOrDefault(x=>x.BookId == bookId);
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
            CheckOutViewModel checkoutItem = null;

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

            return View(checkoutItemList);
        }
    }


}
