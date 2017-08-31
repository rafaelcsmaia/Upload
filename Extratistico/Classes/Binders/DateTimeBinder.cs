using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Extratistico.Classes.Binders
{
    public class DateTimeBinder : IModelBinder
    {
        #region Fields


        private readonly string _customFormat;

        #endregion

        #region Constructors and Destructors

        //public DateTimeBinder(string customFormat)
        //{
        //    this._customFormat = customFormat;
        //}

        #endregion

        #region Explicit Interface Methods

        object IModelBinder.BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var incomingData = bindingContext.ValueProvider.GetValue("data").AttemptedValue;
            int day = Convert.ToInt32(incomingData.Split(new char[1] { '/' })[0]);
            int month = Convert.ToInt32(incomingData.Split(new char[1] { '/' })[1]);
            int year = Convert.ToInt32(incomingData.Split(new char[1] { '/' })[2]);
            return new DateTime(year, month, day);
        }

        #endregion
    }
}