using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZamawianiePosiłkowOnline.Data;
using ZamawianiePosiłkowOnline.Models;
using ZamawianiePosiłkowOnline.Models;
using ZamawianiePosiłkowOnline.ViewModels;

namespace ZamawianiePosiłkowOnline.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            if(User.IsInRole("Admin"))
            {
                return View(await _db.Orders.ToListAsync());
            } else
            {
                var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return View(await _db.Orders.Where(o => o.UserID == userID).ToListAsync());
            }
        }
        public async Task<IActionResult> Detail(int? id)
        {
            OrderDetailViewModel detailsModel = new OrderDetailViewModel()
            {
                ID = 1,
                Order = await _db.Orders.FindAsync(id),
                Orders = await _db.OrderedItems.Where(o => o.OrderID == id).ToListAsync()

            };
            return View(detailsModel);

        }
    }
}
