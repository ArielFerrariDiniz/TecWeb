using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TecWebProject.Models;

namespace TecWebProject.Controllers
{
    public partial class UsuariosController
    {
        private bool IsLogado()
        {
            return Session["User"] != null;
        }
        private bool Invalido(Usuario usuario) {
            return usuario.Email == "admin@admin.com" || usuario.Nome == "admin";
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