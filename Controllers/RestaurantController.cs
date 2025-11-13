using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZamawianiePosiłkowOnline.Data;
using ZamawianiePosiłkowOnline.Models;

namespace ZamawianiePosiłkowOnline.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RestaurantController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webEnvironment;

        public RestaurantController(ApplicationDbContext context, IWebHostEnvironment webEnvironment)
        {
            _db = context;
            _webEnvironment = webEnvironment;
        }

        // GET: Cities
        public async Task<IActionResult> Index()
        {
            return View(await _db.Restaurants.ToListAsync());
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RestaurantID,RestaurantName,ImageUrl")] Restaurant city, IFormFile file)
        {
            ModelState.Remove("ImageUrl");
            if (!ModelState.IsValid)
            {
                return View(city);
            }
            string imgUploadsFolder = Path.Combine(_webEnvironment.WebRootPath, "CityImages");
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
            city.ImageUrl = filename.ToString();
            _db.Restaurants.Add(city);
            _db.SaveChanges();
            TempData["success"] = $"poprawnie dodano {city.RestaurantName}";
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var cityFromDb = _db.Restaurants.Find(id);
            if (cityFromDb == null)
            {
                return NotFound();
            }
            return View(cityFromDb);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obj = _db.Restaurants.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Restaurants.Remove(obj);
            _db.SaveChangesAsync(true);
            TempData["success"] = $"poprawnie usunięto {obj.RestaurantName}";
            return RedirectToAction("Index");
        }
    }
}
