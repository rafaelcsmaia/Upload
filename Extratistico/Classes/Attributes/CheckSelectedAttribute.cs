using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Extratistico.Models.Entidades;

namespace Extratistico.Classes.Attributes
{
    public class CheckSelectedAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            Dictionary<Funcionalidade, bool> o = (Dictionary<Funcionalidade, bool>)value;
            return o.Count(x => x.Value == true) == 0 ? false : true;
        }
    }
}