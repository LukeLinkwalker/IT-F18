using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IT_F18.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace IT_F18.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly DatabaseContext _context;

        public AdminController(DatabaseContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Admin.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            if(!AdminAccountExists())
            {
                return RedirectToAction("Register");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("ID, Username, Password")] AdminViewModel admin)
        {
            if (!AdminAccountExists())
            {
                return RedirectToAction("Register");
            }

            if (ValidateCredentials(admin.Username, admin.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.Username)
                };

                var identity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(principal);

                return RedirectToAction("Index");
            }

            return View();
        }
        
        [AllowAnonymous]
        public IActionResult Register()
        {
            if(AdminAccountExists())
            {
                return RedirectToAction("Login");
            }

            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("ID,Username,Password,ConfirmPassword")] AdminViewModel adminViewModel)
        {
            if(AdminAccountExists())
            {
                return RedirectToAction("Login");
            }

            if(adminViewModel.Username == string.Empty)
            {
                return View();
            }

            if(adminViewModel.Password != adminViewModel.ConfirmPassword)
            {
                return View();
            }

            if (ModelState.IsValid)
            {
                adminViewModel.Username = adminViewModel.Username.ToLower();
                adminViewModel.ConfirmPassword = string.Empty;
                adminViewModel.Password = new PasswordHasher<AdminViewModel>().HashPassword(adminViewModel, adminViewModel.Password);
                _context.Add(adminViewModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Login");
        }
        
        public async Task<IActionResult> Edit()
        {
            var adminViewModel = _context.Admin.ToList().ElementAt(0);
            adminViewModel.Password = string.Empty;
            return View(adminViewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ID,Username,Password, ConfirmPassword")] AdminViewModel adminViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    adminViewModel.Username = _context.Admin.ToList().ElementAt(0).Username;
                    adminViewModel.ConfirmPassword = string.Empty;

                    var entry = _context.Admin.SingleOrDefault(m => m.Username == adminViewModel.Username);
                    entry.Password = new PasswordHasher<AdminViewModel>().HashPassword(adminViewModel, adminViewModel.Password);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        private bool ValidateCredentials(string username, string password)
        {
            AdminViewModel admin = _context.Admin.ToList().ElementAt(0);
            admin.ConfirmPassword = string.Empty;

            if (admin.Username.ToLower() == username.ToLower() &&
               PasswordVerificationResult.Success == new PasswordHasher<AdminViewModel>().VerifyHashedPassword(admin, admin.Password, password))
            {
                return true;
            }

            return false;
        }

        private bool AdminAccountExists()
        {
            if(_context.Admin.ToList().Count == 0)
            {
                return false;
            } 

            return true;
        }
    }
}
