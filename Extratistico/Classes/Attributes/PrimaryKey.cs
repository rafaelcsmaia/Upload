using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Extratistico.Classes.Attributes
{
    public class PrimaryKey : Attribute
    {
        public bool AutoIncrement { get; set; }
    }
}
