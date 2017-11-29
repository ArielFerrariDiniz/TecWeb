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
    public class UsuariosController : Controller
    {
        private FilmesDbContext db = new FilmesDbContext();

        // GET: Usuarios
        public ActionResult Index()
        {
            // gambiarra ?
            if (Session["Admin"] != null) // logado com admin
                return View(db.Usuarios.ToList());

            Session["User"] = null; // logoff tentando acessar area do admin
            return RedirectToAction("LogIn", "Usuarios"); // pagina login ou outra pagina avisando q nao tem direito de acesso
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        //POST: Usuarios/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Senha,Email")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                bool existente = false;
                if (GetUsuarioByEmail(usuario.Email) != null)
                    existente = true;

                if (IsAdmin(usuario))
                    ModelState.AddModelError(string.Empty, "alguma msg nao pode ser adm");
                else
                    ModelState.AddModelError(string.Empty, "alguma msg cadastro existente");

                if (existente)
                    return View(usuario);


                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Senha,Email")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
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

        // GET: Usuarios/Create
        public ActionResult LogIn()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(string email, string senha/*[Bind(Include = "Senha,Email")] Usuario usuario*/)
        {
            if (ModelState.IsValid)
            {
                var usu = Logar(email, senha);

                if (usu != null)
                {
                    // gambiarra ?             
                    Session["Admin"] = IsAdmin(usu) ? usu : null;

                    Session["User"] = usu;

                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }



        private bool IsAdmin(Usuario usuario)
        {
            return usuario.Email == "admin@admin.com" && usuario.Nome == "admin" && usuario.Senha == "admin";
        }

        private Usuario Logar(string email, string senha)
        {
            return (from u in db.Usuarios
                    where u.Email == email &&
                    u.Senha == senha
                    select u).FirstOrDefault();
        }

        private Usuario GetUsuarioByEmail(string email)
        {
            return (from u in db.Usuarios
                    where u.Email == email
                    select u).FirstOrDefault();
        }
    }
}
