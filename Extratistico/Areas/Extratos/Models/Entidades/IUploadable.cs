using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Extratistico.Classes.DataContext;
using System.Globalization;

namespace Extratistico.Areas.Extratos.Models.Entidades
{
    public abstract class IUploadable
    {
        protected NumberFormatInfo format = new NumberFormatInfo()
        {
            NumberGroupSeparator = ".",
            NumberDecimalSeparator = ","
        };

        private DataContext dataContext = new DataContext();
        private UploadLog _log = new UploadLog();
        private List<string> _data = new List<string>();

        public UploadLog log { get { return _log; } set { _log = value; } }
        public List<string> Data { get { return _data; } set { _data = value; } }

        public abstract void PreRoutineExecution(string procedureSQL);
        public abstract void PostRoutineExecution(string procedureSQL);
        public abstract List<Extrato> ProcessData(string username);

        public abstract string ExtratoToSQL(Extrato e);

        public UploadLog InsertData(string username)
        {
            foreach (var item in ProcessData(username))
            {
                log.TotalRecords += 1;
                try
                {
                    dataContext.ExecuteNonQuery(ExtratoToSQL(item));
                    dataContext.Commit();
                    log.SuccessRecords += 1;
                }
                catch (Exception e)
                {
                    log.Log.Add(new LogData()
                    {
                        Data = item.ToString(),
                        ErrorMessage = e.InnerException.Message,
                        rowNumber = log.TotalRecords
                    });
                    log.FailureRecords += 1;
                }
            }
            return log;
        }

        public void UploadData(Stream s, string username)
        {
            using (StreamReader sr = new StreamReader(s))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Data.Add(line);
                }
            }
            InsertData(username);
        }
    }
}
