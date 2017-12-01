using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TecWebProject.Models;

namespace TecWebProject.Controllers
{
    public partial class SitesController
    {
        private bool IsLogado()
        {
            return Session["User"] != null;
        }

        private Usuario GetUsuarioLogado()
        {
            return (Usuario)Session["User"];
        }

        private Usuario GetUsuarioCatalogos(Usuario usuario)
        {
            return (from u in db.Usuarios.Include("Catalogos")
                    where u.Id == usuario.Id
                    select u).FirstOrDefault();
        }

        private Catalogo GetCatalogoSites(Catalogo catalogo)
        {
            return (from c in db.Catalogos.Include("Sites")
                    where c.Id == catalogo.Id
                    select c).FirstOrDefault();
        }

        private void ViewBagCatalogos()
        {
            if (!IsLogado())
                return;
            var us = GetUsuarioLogado();

            us = GetUsuarioCatalogos(us);

            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                
                new SelectListItem { Text = "Latur", Value = "1" },
                new SelectListItem { Text = "Pune", Value = "2" },
                new SelectListItem { Text = "Mumbai", Value = "3" },
                new SelectListItem { Text = "Delhi", Value = "4" },

            };

            //Assigning generic list to ViewBag
            ViewBag.Locations = ObjList;

            ViewBag.Catalogos = ObjList;

            ViewBag.Catalogos = new SelectList(us.Catalogos, "Id", "Nome");

        }

        private void ViewBagCatalogos(Site site)
        {
            if (!IsLogado())
                return;
            var us = GetUsuarioLogado();

            us = GetUsuarioCatalogos(us);

            var catalogos = new List<Catalogo>();

            foreach (Catalogo cat in us.Catalogos)
            {
                Catalogo c = GetCatalogoSites(cat);
                if (!c.Sites.Contains(site))
                    catalogos.Add(c);
            }

            ViewBag.Catalogos = new SelectList(catalogos, "Id", "Nome");

        }


        private Site GetSiteByLink(string link)
        {
            return (from s in db.Sites.Include("Catalogos")
                    where s.Link == link
                    select s).FirstOrDefault();
        }

        private Site GetSiteByIdCatalogos(int id)
        {
            return (from s in db.Sites.Include("Catalogos")
                    where s.Id == id
                    select s).FirstOrDefault();
        }


        // GET: Sites/AddToCatalog
        public ActionResult AddToCatalog(int? id)
        {
            if (!IsLogado())
                return RedirectToAction("Index", "Home");

            Site site = db.Sites.Find(id);
            ViewBagCatalogos(site);
            return View(site);
        }

        // POST:  Sites/AddToCatalog
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCatalog([Bind(Include = "Id,Nome,Acesso,Link,Catalogo")] Site site)
        {
            if (ModelState.IsValid)
            {
                Catalogo cat = GetCatalogoSites(site.Catalogo);
                site = GetSiteByIdCatalogos(site.Id);

                site.Catalogos.Add(cat);
                cat.Sites.Add(site);

                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }





    }
}