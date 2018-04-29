using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IT_F18.Models
{
    public class GalleryViewModel
    {
        public int ID { get; set; }
        public IFormFile Image { get; set; }
    }
}