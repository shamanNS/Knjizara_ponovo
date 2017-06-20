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
    public class GenreController : Controller
    {
        private IRepository<Book> bookRepo = new BookRepository();
        private IRepository<Genre> genreRepo = new GenreRepository();

        private BookstoreContext db = new BookstoreContext();

        public ActionResult Index()
        {
            var genres = db.Genres.ToList();
            //var genres = genreRepo.GetAll();

            //Index + Create Partial experiment:
            ViewBag.CreateModel = new Genre();

            // kraj experimenta

            return View(genres);
        }

 
        public ActionResult Details(int id)
        {
            //Genre genre = genreRepo.GetById(id);
            //var books = bookRepo.GetAll().Where(b => b.Genre.Id == genre.Id).ToList();

            /* var genre = db.Genres.Include( g => g.Books).Where(g => g.Id == id).SingleOrDefault();
             db.Entry(genre).Collection<Book>( g => g.Books).Load();
             */

            var genre = db.Genres.Find(id);
            genre.Books = db.Books.Where(b => b.Genre.Id == id).ToList();
            return View(genre);
        }


        public ActionResult Create()
        {
            
            return View(/*new Genre()*/);
        }


        [HttpPost]
        public ActionResult Create(Genre genre)
        {
            if (ModelState.IsValid)
            {
                //var existingGenre = genreRepo.GetAll().Where(g => g.Name.ToLower() == genre.Name.ToLower()).SingleOrDefault();
                //if (existingGenre != null)
                //{
                //    ViewBag.message = string.Format("Već postoji žanr sa imenom: {0}", genre.Name);
                //    return View(genre);
                //}

                //if (genreRepo.Create(genre))
                //{
                //    return RedirectToAction("Index");
                //}
                var existingGenre = db.Genres.Where(g => g.Name.ToLower() == genre.Name.ToLower()).SingleOrDefault();
                if (existingGenre != null)
                {
                    ViewBag.message = string.Format("Već postoji žanr sa imenom: {0}", genre.Name);
                    return View(genre);
                }
                db.Genres.Add(genre);
                db.SaveChanges();
                 return RedirectToAction("Index");
                
            }
            // Index + Create Partial experiment:
            ViewBag.CreateModel = genre;
            var genres = db.Genres.ToList();
            return View("Index", genres);
            // kraj experimenta

            // return View(genre);
        }


        public ActionResult Edit(int id)
        {
            //var genre = genreRepo.GetById(id);
            var genre = db.Genres.Find(id);
            return View(genre);
        }


        [HttpPost]
        public ActionResult Edit(Genre genre)
        {
            if (ModelState.IsValid)
            {
                //genreRepo.Update(genre);
                db.Entry(genre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(genre);
        }


        public ActionResult Delete(int id)
        {
            var genre = db.Genres.Find(id);
            db.Genres.Remove(genre);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{

        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
