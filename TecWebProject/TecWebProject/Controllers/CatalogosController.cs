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
        public ActionResult Index(string nome, string categoria, string usuario, string ordenar)
        {
            var catalogos = from c in db.Catalogos.Include("Sites")
                            select c;
            if (nome != null && nome != "")
            {
                catalogos = from c in db.Catalogos.Include("Sites")
                            where c.Nome.Contains(nome)
                            select c;
                ViewBag.Filtro = nome;
            }
            else if (categoria != null && categoria != "")
            {
                catalogos = from c in db.Catalogos.Include("Sites")
                            where c.Categoria.Contains(categoria)
                            select c;
                ViewBag.Filtro = categoria;
            }
            else if (usuario != null && usuario != "")
            {
                catalogos = from c in db.Catalogos.Include("Sites")
                            where c.Usuario.Nome.Contains(usuario)
                            select c;
                ViewBag.Filtro = usuario;
            }
            if (String.IsNullOrEmpty(ordenar))
            {
                ViewBag.OrdenarNome = "nome";
                ViewBag.OrdenarSobrenome = "categoria";
                ViewBag.OrdenarUsuario = "usuario";
            }
            else
            {
                if (ordenar == "nome")
                {
                    catalogos = catalogos.OrderBy(a => a.Nome);
                    ViewBag.OrdenarNome = "nome_desc";
                    ViewBag.OrdenarSobrenome = "categoria";
                    ViewBag.OrdenarUsuario = "usuario";
                }
                else if (ordenar == "nome_desc")
                {
                    catalogos = catalogos.OrderByDescending(a => a.Nome);
                    ViewBag.OrdenarNome = "nome";
                    ViewBag.OrdenarSobrenome = "categoria";
                    ViewBag.OrdenarUsuario = "usuario";
                }
                else if (ordenar == "categoria")
                {
                    catalogos = catalogos.OrderBy(a => a.Categoria);
                    ViewBag.OrdenarSobrenome = "categoria_desc";
                    ViewBag.OrdenarNome = "nome";
                    ViewBag.OrdenarUsuario = "usuario";
                }
                else if (ordenar == "categoria_desc")
                {
                    catalogos = catalogos.OrderByDescending(a => a.Categoria);
                    ViewBag.OrdenarSobrenome = "categoria";
                    ViewBag.OrdenarNome = "nome";
                    ViewBag.OrdenarUsuario = "usuario";
                }
                else if (ordenar == "usuario")
                {
                    catalogos = catalogos.OrderBy(a => a.Usuario.Nome);
                    ViewBag.OrdenarUsuario = "usuario_desc";
                    ViewBag.OrdenarSobrenome = "categoria";
                    ViewBag.OrdenarNome = "nome";
                }
                else if (ordenar == "usuario_desc")
                {
                    catalogos = catalogos.OrderByDescending(a => a.Usuario.Nome);
                    ViewBag.OrdenarUsuario = "usuario";
                    ViewBag.OrdenarSobrenome = "categoria";
                    ViewBag.OrdenarNome = "nome";
                }
            }
            //ViewBag.Filtro = nome;
            if (!IsLogado())
                return View(catalogos.ToList());
            else {
                Usuario user = (Usuario)Session["User"];
                var cats = (from u in db.Usuarios.Include("Catalogos")
                            where u.Id == user.Id
                        select u).FirstOrDefault().Catalogos;
                return View(cats);
            }
        }

        // GET: Catalogos/Details/5
        public ActionResult Details(int? id)
        {
            if (!IsLogado())
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
            if (!IsLogado())
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
            if (!IsLogado())
            {
                return RedirectToAction("LogIn", "Usuarios");
            }

            if (ModelState.IsValid)
            {
                Usuario u = (Usuario)Session["User"];
                catalogo.Usuario = db.Usuarios.Find(u.Id);
                db.Catalogos.Add(catalogo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(catalogo);
        }

        // GET: Catalogos/Edit/5
        public ActionResult Edit(int? id)
        {
            Catalogo catalogo = db.Catalogos.Find(id);
            Usuario usu = (Usuario)Session["User"];
            if (!IsLogado())
            {
                return RedirectToAction("LogIn", "Usuarios");
            }
            if (usu.Id != catalogo.Usuario.Id)
            {
                return RedirectToAction("Index", "Catalogos");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

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
            if (!IsLogado())
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
            Catalogo catalogo = db.Catalogos.Find(id);
            Usuario usu = (Usuario)Session["User"];
            if (!IsLogado())
            {
                return RedirectToAction("LogIn", "Usuarios");
            }
            if (usu.Id != catalogo.Usuario.Id)
            {
                return RedirectToAction("Index", "Catalogos");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
            if (!IsLogado())
            {
                return RedirectToAction("LogIn", "Usuarios");
            }
            Catalogo catalogo = (from c in db.Catalogos.Include("Sites")
                                 where c.Id == id
                                 select c).FirstOrDefault();

            var removerSites = new List<Site>();
            foreach (Site site in catalogo.Sites)
            {
                Site st = (from s in db.Sites.Include("Catalogos")
                           where s.Id == site.Id
                           select s).FirstOrDefault();
                st.Catalogos.Remove(catalogo);
                if (st.Catalogos.Count == 0)
                    removerSites.Add(st);

            }
            db.Sites.RemoveRange(removerSites);
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

        private bool IsLogado()
        {
            return Session["User"] != null;
        }
    }
}
