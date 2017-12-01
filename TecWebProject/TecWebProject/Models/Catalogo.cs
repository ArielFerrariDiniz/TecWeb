using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TecWebProject.Models
{
    public class Catalogo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public virtual Usuario Usuario { get; set; }

        public List<Site> Sites { get; set; }
    }
}