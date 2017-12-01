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
    public partial class SitesController : Controller
    {
        private FilmesDbContext db = new FilmesDbContext();

        // GET: Sites
        public ActionResult Index()
        {
            ModelSites sc = new ModelSites();
            List<Site> sites = new List<Site>();
            List<Site> userSites = new List<Site>();
            Usuario u = GetUsuarioLogado();


            sc.Usuario = u;
            sc.Sites = db.Sites.ToList();
            sc.SitesCatalogos = sites;
            sc.UserSites = userSites;


            if (!IsLogado())
            {
                return View(sc);
                //return RedirectToAction("LogIn", "Usuarios");
            }

            u = GetUsuarioCatalogos(u);


            var catalogos = new List<Catalogo>();

            foreach (Catalogo cat in u.Catalogos)
            {
                Catalogo c = GetCatalogoSites(cat);
                catalogos.Add(c);
                sites.AddRange(c.Sites);
                userSites.AddRange(c.Sites);
            }

            var catalogosCmp = new List<Catalogo>();
            foreach (Catalogo cat in catalogos)
            {
                var list = sites.Except(cat.Sites).ToList();
                if (list.Count == 0)
                    catalogosCmp.Add(cat);
            }

            if (catalogosCmp.Count != catalogos.Count)
                sites.Clear();

            if (catalogosCmp.Count == catalogos.Count)
                userSites.Clear();


            return View(sc);
        }

        // GET: Sites/Details/5
        public ActionResult Details(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Sites.Find(id);
            Usuario usuario = GetUsuarioLogado();
            ModelSites sc = new ModelSites();
            List<Site> sites = new List<Site>();

            if (IsLogado())
            {
                usuario = GetUsuarioCatalogos(usuario);
                foreach (Catalogo cat in usuario.Catalogos)
                    sites.AddRange(GetCatalogoSites(cat).Sites);
            }

            sc.UserSites = sites;
            sc.Usuario = usuario;
            sc.Site = site;

            if (site == null)
            {
                return HttpNotFound();
            }
            return View(sc);
        }

        // GET: Sites/Create
        public ActionResult Create()
        {
            if (!IsLogado())
            {
                return RedirectToAction("LogIn", "Usuarios");
            }

            ViewBagCatalogos();
            var tst = ViewBag.Catalogos;
            return View();
        }



        // POST: Sites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Acesso,Link,Catalogo")] Site site)
        {
            if (!IsLogado())
            {
                return RedirectToAction("LogIn", "Usuarios");
            }
            if (ModelState.IsValid)
            {
                Site st = GetSiteByLink(site.Link);

                Catalogo cat = GetCatalogoSites(site.Catalogo);

                if (st == null)
                {
                    db.Sites.Add(site);
                    st = site;
                    st.Catalogos = new List<Catalogo>();
                }
                st.Catalogos.Add(cat);
                cat.Sites.Add(st);

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(site);
        }

        // GET: Sites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!IsLogado())
            {
                return RedirectToAction("LogIn", "Usuarios");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        // POST: Sites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Acesso,Link")] Site site)
        {
            if (!IsLogado())
            {
                return RedirectToAction("LogIn", "Usuarios");
            }
            if (ModelState.IsValid)
            {
                db.Entry(site).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(site);
        }

        // GET: Sites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!IsLogado())
            {
                return RedirectToAction("LogIn", "Usuarios");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ModelSites ms = new ModelSites();
            Usuario usu = GetUsuarioLogado();            
            Site site = db.Sites.Find(id);
            ms.Site = site;
            ms.Usuario = GetUsuarioCatalogos(usu);
          
            if (site == null)
            {
                return HttpNotFound();
            }

            ViewBagCatalogosComSite(site);

            return View(ms);
        }

        // POST: Sites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "Id,Nome,Acesso,Link,Catalogo")] Site site)
        {
            if (!IsLogado())
            {
                return RedirectToAction("LogIn", "Usuarios");
            }

            //Catalogo cat = GetCatalogoSites(site.Catalogo);
            //site = GetSiteByIdCatalogos(site.Id);

            //cat.Sites.Remove(site);
            //site.Catalogos.Remove(cat);

            //db.SaveChanges();
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
