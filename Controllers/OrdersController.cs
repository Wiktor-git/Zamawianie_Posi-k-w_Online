using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ZamawianiePosiłkowOnline.Data;
using ZamawianiePosiłkowOnline.Models;
using ZamawianiePosiłkowOnline.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ZamawianiePosiłkowOnline.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly List<ShoppingCartItem> _cartItems;
        private readonly ILogger<OrdersController> _iLogger;

        public OrdersController(ApplicationDbContext db, ILogger<OrdersController> iLogger)
        {
            _db = db;
            _cartItems = new List<ShoppingCartItem>();
            this._iLogger = iLogger;
            _iLogger.LogDebug("Log jest zintegrowany w Zamówieniach");
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.Restaurants.ToListAsync());
        }
        public async Task<IActionResult> SelectedRestaurant(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var restaurant = await _db.Restaurants.FindAsync(id);
                return View(await _db.Meals.Where(x => x.CityAvailability == restaurant.RestaurantName).ToListAsync());
            }
            else 
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public IActionResult AddToCart(int? id)
        {
            try
            {
                var mealToAdd = _db.Meals.Find(id);

                var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();

                var existingCartItem = cartItems.FirstOrDefault(item => item.CartMeal.Id == id);

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
                HttpContext.Session.Set("Cart", cartItems);
                TempData["CartMessage"] = $"dodano {mealToAdd.Name} do koszyka";
                CreateReport($"Dodano {mealToAdd.Name} do koszyka", User.Identity.Name);
                int orderItemRestaurantID = _db.Restaurants.Where(r => r.RestaurantName == mealToAdd.CityAvailability).First().RestaurantID;
                return RedirectToAction("SelectedRestaurant", new { id = orderItemRestaurantID });
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
                return RedirectToAction("Index");
            }
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
            try
            {
                var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();

                var itemToRemove = cartItems.FirstOrDefault(m => m.CartMeal.Id == id);

                if (itemToRemove != null)
                {
                    cartItems.Remove(itemToRemove);
                    CreateReport($"Usunięto {itemToRemove.Id} z koszyka", User.Identity.Name);
                }

                HttpContext.Session.Set("Cart", cartItems);
                return RedirectToAction("ViewCart");
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
                return RedirectToAction("Index");
            }
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
            try
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
                    PaymentMethod = sposobPlatnosci,
                    UserAddress = address
                };
                _db.Orders.Add(order);
                _db.SaveChanges();
                foreach (var item in cartViewModel.CartItems)
                {
                    _db.OrderedItems.Add(new OrderItem()
                    {
                        OrderID = order.Id,
                        MealID = item.CartMeal.Id,
                        AmmountOrdered = item.Quantity,
                        OrderPrice = item.CartMeal.MealPrice * (decimal)item.Quantity,
                    });
                }
                _db.SaveChanges();
                TempData["success"] = $"złożono zamówienie dostawa przewidywana {order.ScheduledDeliveryDate.TimeOfDay}";
                CreateReport($"złożono zamówienie {order.Id}", User.Identity.Name);
                HttpContext.Session.Set("Cart", new List<ShoppingCartItem>());
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
                return RedirectToAction("Index");
            }
        }
        private async void CreateReport(string reportType, string userID)
        {
            await _db.UserReports.AddAsync(
                new Reports()
                {
                    ReportType = reportType,
                    UserID = userID,
                    Time = DateTime.Now,
                }
            );
            _db.SaveChanges();
        }
    }
}
