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
    public class GalleryController : Controller
    {
        private readonly DatabaseContext _context;

        public GalleryController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Gallery
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gallery.ToListAsync());
        }

        // GET: Gallery/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galleryViewModel = await _context.Gallery
                .SingleOrDefaultAsync(m => m.ID == id);
            if (galleryViewModel == null)
            {
                return NotFound();
            }

            return View(galleryViewModel);
        }

        // GET: Gallery/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gallery/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Image")] GalleryViewModel galleryViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(galleryViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(galleryViewModel);
        }

        // GET: Gallery/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galleryViewModel = await _context.Gallery.SingleOrDefaultAsync(m => m.ID == id);
            if (galleryViewModel == null)
            {
                return NotFound();
            }
            return View(galleryViewModel);
        }

        // POST: Gallery/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Image")] GalleryViewModel galleryViewModel)
        {
            if (id != galleryViewModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(galleryViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GalleryViewModelExists(galleryViewModel.ID))
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
            return View(galleryViewModel);
        }

        // GET: Gallery/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galleryViewModel = await _context.Gallery
                .SingleOrDefaultAsync(m => m.ID == id);
            if (galleryViewModel == null)
            {
                return NotFound();
            }

            return View(galleryViewModel);
        }

        // POST: Gallery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var galleryViewModel = await _context.Gallery.SingleOrDefaultAsync(m => m.ID == id);
            _context.Gallery.Remove(galleryViewModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GalleryViewModelExists(int id)
        {
            return _context.Gallery.Any(e => e.ID == id);
        }
    }
}
