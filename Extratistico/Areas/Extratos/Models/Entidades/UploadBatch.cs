using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Extratistico.Areas.Extratos.Models.Entidades
{
    public class UploadBatch
    {
        public string Type { get; set; }
        public int Records { get; set; }
        public DateTime Date { get; set; }
    }
}