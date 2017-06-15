using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data;
namespace Knjizara_ponovo.Models
{
    public class BookstoreContext : DbContext
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }

        public BookstoreContext():base("name=BookstoreDbContext_EF")
        {

        }
    }
}