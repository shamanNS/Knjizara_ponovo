using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Knjizara_ponovo.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string Name { get; set; }
        
        [Required]
        [Range(1.0, int.MaxValue, ErrorMessage = "{0} must be between {1} and {2}.")]
        public double Price { get; set; }
        public bool isDeleted { get; set; }

        public Genre Genre { get; set; }

        [ForeignKey("Genre")]
        public int GenreId { get; set; }

        public Book()
        {
            this.Genre = new Genre();
        }
    }
}