using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Knjizara_ponovo.Models;
using Knjizara_ponovo.Repository;
using Knjizara_ponovo.ViewModels;
using System.Data.Entity;

namespace Knjizara_ponovo.Controllers
{
    public class BookController : Controller
    {
        private IRepository<Book> bookRepo = new BookRepository();
        private IRepository<Genre> genreRepo = new GenreRepository();
        private BookstoreContext db = new BookstoreContext();

        public ActionResult Index()
        {
            //var books = bookRepo.GetAll().Where(b => b.isDeleted == false).OrderBy(b => b.Name).ToList();

            //var books = db.Books.ToList();
            //foreach (Book book in books)
            //{
            //    int id = book.GenreId;
            //    var genre = db.Genres.SingleOrDefault(g => g.Id == id);
            //    book.Genre = genre;
            //}
            var books = db.Books.Include(b => b.Genre).Where(b => b.isDeleted == false).OrderBy(b => b.Name).ToList();

            return View(books);
        }

        public ActionResult Deleted()
        {
            //var books = bookRepo.GetAll().Where(b => b.isDeleted == true).OrderBy(b => b.Name).ToList();
            var books = db.Books.Include(b => b.Genre).Where(b => b.isDeleted == true).OrderBy(b => b.Name).ToList();
            return View(books);
        }


        public ActionResult Details(int id)
        {
            var book = db.Books/*.Include(b => b.Genre)*/.SingleOrDefault(b => b.Id == id);
            return View(book);
        }


        public ActionResult Create()
        {
            BookGenreViewModel vm = new BookGenreViewModel();
            //var genres = genreRepo.GetAll();
            //vm.Genres = genres;
            vm.Genres = db.Genres.ToList();
            return View(vm);
        }


        [HttpPost]
        public ActionResult Create(BookGenreViewModel vm)
        {
            //vm.Genres = genreRepo.GetAll();
            vm.Genres = db.Genres.ToList();
            //if (ModelState.IsValid)
            //{
            //    var book = vm.Book;
            //    var genre = genreRepo.GetById(vm.SelectedGenreId);
            //    book.Genre = genre;

            //    var existingBook = bookRepo.GetAll().Where(b => b.Name.ToLower() == book.Name.ToLower()).SingleOrDefault();
            //    if (existingBook != null)
            //    {
            //        ViewBag.message = string.Format("Već postoji knjiga sa imenom: {0}", book.Name);
            //        return View(vm);
            //    }


            //    if (bookRepo.Create(book))
            //    {
            //        return RedirectToAction("Index");
            //    }  
            // }

            if (ModelState.IsValid)
            {
                var book = vm.Book;
                book.Genre = db.Genres.SingleOrDefault(g => g.Id == vm.SelectedGenreId);
                var existingBook = db.Books.Where(b => b.Name.ToLower() == book.Name.ToLower()).SingleOrDefault();
                if (existingBook != null)
                {
                    ViewBag.message = string.Format("Već postoji knjiga sa imenom: {0}", book.Name);
                    return View(vm);
                }

                return RedirectToAction("Index");
            }
            
            
                return View(vm);
        }


        public ActionResult Edit(int id)
        {

            BookGenreViewModel vm = new BookGenreViewModel();
            //var genres = genreRepo.GetAll();
            //vm.Genres = genres;
            //vm.Book = bookRepo.GetById(id);

            vm.Genres = db.Genres.ToList();
            vm.Book = db.Books.Find(id);

            //bez ovoga ne prosledi selektovanu opciju iz HTML select liste
            // kada se koristi @Html.DropDownListFor helper!
            vm.SelectedGenreId = vm.Book.Genre.Id;

            return View(vm);
        }


        [HttpPost]
        public ActionResult Edit(BookGenreViewModel vm)
        {
            //var genres = genreRepo.GetAll();
            vm.Genres = db.Genres.ToList();

            if (ModelState.IsValid)
            {
                //Book book = bookRepo.GetById(vm.Book.Id);
                //Genre genre = genreRepo.GetById(vm.SelectedGenreId);

                //book.Name = vm.Book.Name;
                //book.Price = vm.Book.Price;
                //book.Genre = genre;
                //bookRepo.Update(book);

                var book = db.Books.Find(vm.Book.Id);
                var genre = db.Genres.Find(vm.SelectedGenreId);
                book.Name = vm.Book.Name;
                book.Price = vm.Book.Price;
                book.Genre = genre;

                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vm);
        }


        public ActionResult Delete(int id)
        {
            //bookRepo.Delete(id);
            var book = db.Books.SingleOrDefault(b => b.Id == id);
            book.isDeleted = true;
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
