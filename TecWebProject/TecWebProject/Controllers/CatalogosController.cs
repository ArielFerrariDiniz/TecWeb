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
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "Usuarios" );
            }
            return View(db.Catalogos.ToList());
        }

        // GET: Catalogos/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "Usuarios");
            }
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
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "Usuarios");
            }
            return View();
        }

        // POST: Catalogos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Categoria")] Catalogo catalogo)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "Usuarios");
            }
            if (ModelState.IsValid)
            {
                catalogo.Usuario = (Usuario)Session["User"];
                db.Catalogos.Add(catalogo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(catalogo);
        }

        // GET: Catalogos/Edit/5
        public ActionResult Edit(int? id)
        {
            var c = (from u in db.Catalogos
                           where u.Id == id
                           select u).FirstOrDefault();
            Usuario usu = (Usuario)Session["User"];
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "Usuarios");
            }
            if (usu.Id != c.Usuario.Id)
            {
                return RedirectToAction("Index", "Catalogos");
            }
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
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "Usuarios");
            }
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
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "Usuarios");
            }
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
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "Usuarios" );
            }
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
