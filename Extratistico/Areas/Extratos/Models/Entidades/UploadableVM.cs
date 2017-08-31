using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Extratistico.Areas.Extratos.Models.Entidades
{
    public class UploadableVM
    {
        public string[] TipoSelecionado {get;set;}
        public IEnumerable<Type> Types { get; set; }
        public IEnumerable<UploadBatch> Batches { get; set; }
    }
}