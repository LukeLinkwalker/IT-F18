using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IT_F18.Models
{
    public class AdminViewModel
    {
        public int ID { get; set; }

        public string Username { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string ConfirmPassword { get; set; }
    }
}
