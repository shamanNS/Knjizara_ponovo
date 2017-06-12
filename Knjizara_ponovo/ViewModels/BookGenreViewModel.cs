using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Knjizara_ponovo.Models;

namespace Knjizara_ponovo.ViewModels
{
    public class BookGenreViewModel
    {
        public Book Book { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public int SelectedGenreId { get; set; }
    }
}