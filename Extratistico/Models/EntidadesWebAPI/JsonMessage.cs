using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Extratistico.Models.EntidadesWebAPI
{
    public class JsonMessage
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public object JsonData { get; set; }
    }
}