using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TecWebProject.Models
{
    public class ModelSites
    {
        public Site Site { get; set; }
        public List<Site> Sites { get; set; }
        public List<Site> SitesCatalogos { get; set; }
    }
}