using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IT_F18.Models;

namespace IT_F18.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly DatabaseContext _context;

        public NewsletterController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Newsletter
        public IActionResult Index()
        {
            
            return View();
        }

        // GET: Newsletter/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsletterViewModel = await _context.Newsletter
                .SingleOrDefaultAsync(m => m.ID == id);
            if (newsletterViewModel == null)
            {
                return NotFound();
            }

            return View(newsletterViewModel);
        }

        // GET: Newsletter/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Newsletter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Firstname,Email")] NewsletterViewModel newsletterViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newsletterViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newsletterViewModel);
        }

        // GET: Newsletter/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsletterViewModel = await _context.Newsletter.SingleOrDefaultAsync(m => m.ID == id);
            if (newsletterViewModel == null)
            {
                return NotFound();
            }
            return View(newsletterViewModel);
        }

        // POST: Newsletter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Firstname,Email")] NewsletterViewModel newsletterViewModel)
        {
            if (id != newsletterViewModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsletterViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsletterViewModelExists(newsletterViewModel.ID))
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
            return View(newsletterViewModel);
        }

        // GET: Newsletter/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsletterViewModel = await _context.Newsletter
                .SingleOrDefaultAsync(m => m.ID == id);
            if (newsletterViewModel == null)
            {
                return NotFound();
            }

            return View(newsletterViewModel);
        }

        // POST: Newsletter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newsletterViewModel = await _context.Newsletter.SingleOrDefaultAsync(m => m.ID == id);
            _context.Newsletter.Remove(newsletterViewModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsletterViewModelExists(int id)
        {
            return _context.Newsletter.Any(e => e.ID == id);
        }
    }
}
