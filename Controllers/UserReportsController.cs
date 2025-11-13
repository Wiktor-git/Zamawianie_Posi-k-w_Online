using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZamawianiePosiłkowOnline.Data;

namespace ZamawianiePosiłkowOnline.Controllers
{
    [Authorize(Roles ="Admin")]

    public class UserReportsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserReportsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.UserReports.ToList());
        }

    }
}
