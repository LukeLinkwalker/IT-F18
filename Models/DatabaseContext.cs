using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IT_F18.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext (DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<IT_F18.Models.AboutViewModel> About { get; set; }
        public DbSet<IT_F18.Models.AdminViewModel> Admin { get; set; }
        public DbSet<IT_F18.Models.GalleryViewModel> Gallery { get; set; }
        public DbSet<IT_F18.Models.NewsletterViewModel> Newsletter { get; set; }
    }
}
