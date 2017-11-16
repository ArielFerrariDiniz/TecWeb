using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TecWebProject.Models
{
    public class Site
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public enum caracteristica {Gratuito, Pago}

        public caracteristica Acesso;

        public string Link;

        public List<Catalogo> Catalogos = new List<Catalogo>();

    }
}