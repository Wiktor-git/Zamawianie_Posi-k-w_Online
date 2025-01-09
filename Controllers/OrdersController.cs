using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ZamawianiePosiłkowOnline.Data;
using ZamawianiePosiłkowOnline.Models;
using ZamawianiePosiłkowOnline.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ZamawianiePosiłkowOnline.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly List<ShoppingCartItem> _cartItems;
        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
            _cartItems = new List<ShoppingCartItem>();
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.Meals.ToListAsync());
        }
        public IActionResult AddToCart(int? id)
        {
            var mealToAdd = _db.Meals.Find(id);

            var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();

            var existingCartItem = cartItems.FirstOrDefault(item =>  item.CartMeal.Id == id);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity++;
            }
            else
            {
                cartItems.Add(new ShoppingCartItem
                {
                    CartMeal = mealToAdd,
                    Quantity = 1
                });
            }
            TempData["OrderInfo"] = "Dodano ";

            HttpContext.Session.Set("Cart", cartItems);
            return RedirectToAction("ViewCart");
        }
        [HttpGet]
        public IActionResult ViewCart()
        {
            var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();

            //total delivery cost
            int uniqueCityCount = cartItems.Select(m => m.CartMeal.CityAvailability).Distinct().Count();
            var cartViewModel = new ShoppingCartViewModel()
            {
                CartItems = cartItems,
                TotalPrice = cartItems.Sum(item => item.CartMeal.MealPrice * item.Quantity),
                TotalQuantity = cartItems.Sum(item => item.Quantity),
                TotalDeliveryCost = (decimal)uniqueCityCount * (decimal)20.00,
                TotalTotal = (cartItems.Sum(item => item.CartMeal.MealPrice * item.Quantity)) + ((decimal)uniqueCityCount * (decimal)20.00)
            };
            return View(cartViewModel);
        }
        public IActionResult DeleteFromCart(int? id)
        {
            var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();

            var itemToRemove = cartItems.FirstOrDefault(m => m.Id == id);

            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
            }

            HttpContext.Session.Set("Cart", cartItems);
            return RedirectToAction("ViewCart");
        }
        public IActionResult InputChange(int? id,int? inputQuantity)
        {
            var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();

            var itemToAdd = cartItems.FirstOrDefault(m => m.Id == id);

            if (itemToAdd != null && inputQuantity != null)
            {
                cartItems.Remove(itemToAdd);
                itemToAdd.Quantity = (int)inputQuantity;
                cartItems.Add(itemToAdd);
            }
            HttpContext.Session.Set("Cart", cartItems);
            return RedirectToAction("ViewCart");
        }
        public IActionResult ConfirmPurchase()
        {

            return View();
        }
        [HttpPost]
        public IActionResult ConfirmPurchase(string sposobPlatnosci, string address)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ViewCart");
            }
            //Get shopping cartItem
            var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();
            int uniqueCityCount = cartItems.Select(m => m.CartMeal.CityAvailability).Distinct().Count();
            var cartViewModel = new ShoppingCartViewModel()
            {
                CartItems = cartItems,
                TotalPrice = cartItems.Sum(item => item.CartMeal.MealPrice * item.Quantity),
                TotalQuantity = cartItems.Sum(item => item.Quantity),
                TotalDeliveryCost = (decimal)uniqueCityCount * (decimal)20.00,
                TotalTotal = (cartItems.Sum(item => item.CartMeal.MealPrice * item.Quantity)) + ((decimal)uniqueCityCount * (decimal)20.00)
            };
            //</Get shopping cartItem
            Random random = new Random();
            int minutesToAdd = random.Next(20, 61);
            DateTime deliveryTime = DateTime.Now.AddMinutes(minutesToAdd);
            Order order = new Order()

            {
                UserID = User.FindFirstValue(ClaimTypes.NameIdentifier),
                OrderDate = DateTime.Now,
                ScheduledDeliveryDate = DateTime.Now.AddMinutes(30),
                DeliveryDate = deliveryTime,
                TotalPaid = (decimal)cartViewModel.TotalTotal,
                DiscountAmmount = 0,
            };
            _db.Orders.Add(order);
            _db.SaveChanges();
            TempData["success"] = $"poprawnie spełniono zamówienie dostawa przewidywana {order.ScheduledDeliveryDate.TimeOfDay}";
            return RedirectToAction("Index");

            return View();
        }
    }
}
