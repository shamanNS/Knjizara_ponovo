using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Knjizara_ponovo.Models;
using Knjizara_ponovo.Repository;
using Knjizara_ponovo.ViewModels;

namespace Knjizara_ponovo.Controllers
{
    public class GenreController : Controller
    {
        private IRepository<Book> bookRepo = new BookRepository();
        private IRepository<Genre> genreRepo = new GenreRepository();

        public ActionResult Index()
        {
            var genres = genreRepo.GetAll();
            return View(genres);
        }

 
        public ActionResult Details(int id)
        {
            Genre genre = genreRepo.GetById(id);
            var books = bookRepo.GetAll().Where(b => b.Genre.Id == genre.Id).ToList();
            genre.Books = books;
            return View(genre);
        }


        public ActionResult Create()
        {
            
            return View(new Genre());
        }


        [HttpPost]
        public ActionResult Create(Genre genre)
        {
            if (ModelState.IsValid)
            {
                var existingGenre = genreRepo.GetAll().Where(g => g.Name.ToLower() == genre.Name.ToLower()).SingleOrDefault();
                if (existingGenre != null)
                {
                    ViewBag.message = string.Format("Već postoji žanr sa imenom: {0}", genre.Name);
                    return View(genre);
                }

                if (genreRepo.Create(genre))
                {
                    return RedirectToAction("Index");
                }

                
            }
            return View(genre);
        }


        public ActionResult Edit(int id)
        {
            var genre = genreRepo.GetById(id);
            return View(genre);
        }


        [HttpPost]
        public ActionResult Edit(Genre genre)
        {
            if (ModelState.IsValid)
            {
                genreRepo.Update(genre);
                return RedirectToAction("Index");
            }
            return View(genre);
        }

       
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

      
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
            
        //}
    }
}
