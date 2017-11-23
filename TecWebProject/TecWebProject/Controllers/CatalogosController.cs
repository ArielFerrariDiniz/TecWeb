using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TecWebProject.Models;

namespace TecWebProject.Controllers
{
    public class CatalogosController : Controller
    {
        private FilmesDbContext db = new FilmesDbContext();

        // GET: Catalogos
        public ActionResult Index()
        {
            return View(db.Catalogos.ToList());
        }

        // GET: Catalogos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catalogo catalogo = db.Catalogos.Find(id);
            if (catalogo == null)
            {
                return HttpNotFound();
            }
            return View(catalogo);
        }

        // GET: Catalogos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Catalogos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Categoria")] Catalogo catalogo)
        {
            if (ModelState.IsValid)
            {
                db.Catalogos.Add(catalogo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(catalogo);
        }

        // GET: Catalogos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catalogo catalogo = db.Catalogos.Find(id);
            if (catalogo == null)
            {
                return HttpNotFound();
            }
            return View(catalogo);
        }

        // POST: Catalogos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Categoria")] Catalogo catalogo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(catalogo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(catalogo);
        }

        // GET: Catalogos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catalogo catalogo = db.Catalogos.Find(id);
            if (catalogo == null)
            {
                return HttpNotFound();
            }
            return View(catalogo);
        }

        // POST: Catalogos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Catalogo catalogo = db.Catalogos.Find(id);
            db.Catalogos.Remove(catalogo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
