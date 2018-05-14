using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IT_F18.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace IT_F18.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IHostingEnvironment environment;
        private readonly DatabaseContext context;
        private readonly string UploadPath;
        private readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private Random random;

        public GalleryController(IHostingEnvironment environment, DatabaseContext context)
        {
            this.environment = environment;
            this.context = context;
            this.UploadPath = Path.Combine(environment.WebRootPath, "images", "uploads");
            this.random = new Random();
        }

        // GET: Gallery
        public async Task<IActionResult> Index()
        {
            return View(await context.Gallery.ToListAsync());
        }

        // GET: Gallery/Details/5
        public async Task<IActionResult> List()
        {
            return View(await context.Gallery.ToListAsync());
        }

        // GET: Gallery/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(List<IFormFile> Files)
        {
            long Size = Files.Sum(f => f.Length);
            
            foreach (var formFile in Files)
            {
                if (formFile.Length > 0)
                {
                    string filePath = Path.Combine(UploadPath, string.Concat(GetRandomString(32), formFile.FileName.Substring(formFile.FileName.IndexOf('.'))));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    context.Add(new GalleryViewModel() { ImagePath = filePath.Replace(UploadPath, "").Substring(1) });
                    await context.SaveChangesAsync();
                }
            }
            
            return RedirectToAction("Index");
        }

        // GET: Gallery/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var galleryViewModel = await context.Gallery.SingleOrDefaultAsync(m => m.ID == id);
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
            var model = context.Gallery.SingleOrDefault(m => m.ID == id);
            
            string filePath = Path.Combine(UploadPath, model.ImagePath);
            System.IO.File.Delete(filePath);

            context.Gallery.Remove(model);
            await context.SaveChangesAsync();

            return RedirectToAction("List");
        }

        private bool GalleryViewModelExists(int id)
        {
            return context.Gallery.Any(e => e.ID == id);
        }

        private string GetRandomString(int length)
        {
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
