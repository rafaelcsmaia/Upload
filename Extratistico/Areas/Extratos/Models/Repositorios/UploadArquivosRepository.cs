using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Extratistico.Areas.Extratos.Models.Entidades;
using Extratistico.Classes.DataContext;
using System.Data;
using System.Globalization;

namespace Extratistico.Areas.Extratos.Models.Repositorios
{
    public class UploadArquivosRepository : IRepository
    {
        public List<Type> UploadableTypes()
        {
            return new List<Type>(
                Assembly.GetExecutingAssembly().GetTypes()
                .Where(myType => myType.IsSubclassOf(typeof(IUploadable)))
                .ToList());
                //.Select(c=> new SelectListItem(){ Text=c.Name, Value=c.Name})
                //);
        }

        public List<UploadBatch> UploadBatches(string username)
        {
            DataTable table = dataContext.Select(String.Format("select 'CD' as type,count(documento) as records, lote_carga as batch from extrato where username='{0}' group by lote_carga union " +
            "select 'CC' as type,count(documento) as records, lote_carga as batch from extrato_credito where username='{0}' group by lote_carga order by 2", username));
            List<UploadBatch> batches = new List<UploadBatch>();
            foreach (DataRow row in table.Rows)
            {
                batches.Add(new UploadBatch()
                {
                    Type = row["type"].ToString(),
                    Records = int.Parse(row["Records"].ToString()),
                    Date = DateTime.Parse(row["batch"].ToString())
                });
            }
            return batches;
        }
    }
}