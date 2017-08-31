using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Extratistico.Classes.DataContext
{
    public abstract class IRepository
    {
        protected DataContext dataContext = new DataContext();

        protected NumberFormatInfo format = new NumberFormatInfo()
        {
            NumberGroupSeparator = ",",
            NumberDecimalSeparator = "."
            //NumberGroupSeparator = ".",
            //NumberDecimalSeparator = ","
        };

        public void Commit()
        {
            dataContext.Commit();
        }

        public void RollBack()
        {
            dataContext.Rollback();
        }
    }
}