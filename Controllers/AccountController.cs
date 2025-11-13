using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using ZamawianiePosiłkowOnline.Data;
using ZamawianiePosiłkowOnline.Models;
using ZamawianiePosiłkowOnline.ViewModels;

namespace ZamawianiePosiłkowOnline.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<OrdersController> _iLogger;


        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager, ApplicationDbContext db, ILogger<OrdersController> iLogger)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _iLogger = iLogger;
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult VerifyEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Users users = new Users
                    {
                        FullName = model.Name,
                        Email = model.Email,
                        UserName = model.Email,
                    };

                    var result = await _userManager.CreateAsync(users, model.Password);

                    if (result.Succeeded)
                    {
                        CreateReport("rejestracja", model.Email);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        CreateReport("próba rejestracji nie powiodła się", model.Email);
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }
                return View(model);

            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
                return RedirectToAction("View");
            }
        }
        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel { Email = username });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        CreateReport("zalogowano", model.Email);
                        return RedirectToAction("Index", "Orders");
                    }
                    else
                    {
                        CreateReport("błąd logowania", model.Email);
                        ModelState.AddModelError("", "Email albo hasło jest niepoprawne");
                        return View(model);
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
                return RedirectToAction("View");
            }

        }
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailPropertyViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(model.Email);

                    if (user == null)
                    {
                        ModelState.AddModelError("", "Podany adres e-mail nie istnieje");
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("ChangePassword", "Account", new { username = user.Email });
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
                return RedirectToAction("View");
            }

        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(model.Email);
                    if (user != null)
                    {
                        var result = await _userManager.RemovePasswordAsync(user);
                        if (result.Succeeded)
                        {
                            CreateReport("zmieniono hasło", model.Email);
                            result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                            return RedirectToAction("Login", "Account");
                        }
                        else
                        {
                            CreateReport("błąd zmiany hasła", model.Email);
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Nie znaleziono adresu e-mail");
                        return View(model);
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Coś poszło nie tak");
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
                return RedirectToAction("View");
            }
        }
        public async Task<IActionResult> Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
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
