using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using ZamawianiePosiłkowOnline.Data;
using ZamawianiePosiłkowOnline.Models;

namespace ZamawianiePosiłkowOnline.Controllers
{
    public class MealController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webEnvironment;
        public MealController(ApplicationDbContext db, IWebHostEnvironment webEnvironment)
        {
            _db = db;
            _webEnvironment = webEnvironment;

        }
        public IActionResult Index()
        {
            IEnumerable<Meal> objMealList = _db.Meals.ToList();
            return View(objMealList);
        }
        //Get
        public IActionResult Create()
        {
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Meal objMeal, IFormFile file)
        {
            ModelState.Remove("ImageUrl");
            if (!ModelState.IsValid)
            {
                return View(objMeal);
            }
            string imgUploadsFolder = Path.Combine(_webEnvironment.WebRootPath, "MealImages");
            if (!Directory.Exists(imgUploadsFolder))
            {
                Directory.CreateDirectory(imgUploadsFolder);
            }
            string filename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
            string fileSavePath = Path.Combine(imgUploadsFolder, filename);


            using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            //end of file upload now add to db
            objMeal.ImageUrl = filename.ToString();
            _db.Meals.Add(objMeal);
            _db.SaveChanges();
            TempData["success"] = $"poprawnie dodano {objMeal.Name}";
            return RedirectToAction("Index");

        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var mealFromDb = _db.Meals.Find(id);
            if (mealFromDb == null)
            {
                return NotFound();
            }
            return View(mealFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Meal obj, IFormFile file)
        {
            if (file == null) 
            {
                ModelState.Remove("file");
            } else
            {
                if (!ModelState.IsValid)
                {
                    return View(obj);
                }
                string imgUploadsFolder = Path.Combine(_webEnvironment.WebRootPath, "MealImages");
                if (!Directory.Exists(imgUploadsFolder))
                {
                    Directory.CreateDirectory(imgUploadsFolder);
                }
                string filename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
                string fileSavePath = Path.Combine(imgUploadsFolder, filename);


                using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                obj.ImageUrl = filename.ToString();
            }
            if (ModelState.IsValid)
            {
                _db.Meals.Update(obj);
                await _db.SaveChangesAsync(true);
                TempData["success"] = $"poprawnie edytowano {obj.Name}";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var mealFromDb = _db.Meals.Find(id);
            if (mealFromDb == null)
            {
                return NotFound();
            }
            return View(mealFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            //delete file
            var obj = _db.Meals.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Meals.Remove(obj);
            _db.SaveChangesAsync(true);
            TempData["success"] = $"poprawnie usunięto {obj.Name}";
            return RedirectToAction("Index");
        }
    }
}
