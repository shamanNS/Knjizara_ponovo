using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Knjizara_ponovo.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "{0} must be betwen {2} and {1} characters.")]
        [Display(Name = "Genre name")]
        public string Name { get; set; }
        public List<Book> Books { get; set; }

        public Genre()
        {
            this.Books = new List<Book>();
        }
    }
}