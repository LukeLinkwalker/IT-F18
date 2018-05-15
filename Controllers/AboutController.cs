using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IT_F18.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace IT_F18.Controllers
{
    [Authorize]
    public class AboutController : Controller
    {
        private readonly DatabaseContext _context;

        public AboutController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: About
        [AllowAnonymous]
        public IActionResult Index()
        {
            var text = _context.About.ToList();
            ViewData["AboutMeText"] = text.Last().Plaintext;
            return View();

        }

        // GET: About/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: About/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Plaintext")] AboutViewModel aboutViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aboutViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutViewModel);
        }
    }
}
