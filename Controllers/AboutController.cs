using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IT_F18.Models;
using System.Diagnostics;

namespace IT_F18.Controllers
{
    public class AboutController : Controller
    {
        private readonly DatabaseContext _context;

        public AboutController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: About
        public async Task<IActionResult> Index()
        {
            return View(await _context.About.ToListAsync());
        }

        // GET: About/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutViewModel = await _context.About
                .SingleOrDefaultAsync(m => m.ID == id);
            if (aboutViewModel == null)
            {
                return NotFound();
            }

            return View(aboutViewModel);
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

        // GET: About/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutViewModel = await _context.About.SingleOrDefaultAsync(m => m.ID == id);
            if (aboutViewModel == null)
            {
                return NotFound();
            }
            return View(aboutViewModel);
        }

        // POST: About/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Plaintext")] AboutViewModel aboutViewModel)
        {
            if (id != aboutViewModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aboutViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutViewModelExists(aboutViewModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aboutViewModel);
        }

        // GET: About/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutViewModel = await _context.About
                .SingleOrDefaultAsync(m => m.ID == id);
            if (aboutViewModel == null)
            {
                return NotFound();
            }

            return View(aboutViewModel);
        }

        // POST: About/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aboutViewModel = await _context.About.SingleOrDefaultAsync(m => m.ID == id);
            _context.About.Remove(aboutViewModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutViewModelExists(int id)
        {
            return _context.About.Any(e => e.ID == id);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
