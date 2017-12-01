using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecWebProject.Models
{
    public class Site
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public enum TipoAcesso {Gratuito, Pago}

        public TipoAcesso Acesso { get; set; }

        public string Link { get; set; }

        public List<Catalogo> Catalogos { get; set; }

        [NotMapped]
        public Catalogo Catalogo { get; set; }
  
    }
}