using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Extratistico.Areas.Extratos.Models.Entidades
{
    public class UploadLog
    {
        public List<LogData> Log { get; set; }
        public int SuccessRecords { get; set; }
        public int FailureRecords { get; set; }
        public int TotalRecords { get; set; }

        public UploadLog()
        {
            Log = new List<LogData>();
            SuccessRecords = 0;
            FailureRecords = 0;
            TotalRecords = 0;
        }
    }
}