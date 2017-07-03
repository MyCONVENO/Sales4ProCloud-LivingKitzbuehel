using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.ImportClass
{
    public class ImportTempStock
    {
        public string SizerunID { get; set; }
        public string EAN { get; set; }
        public int Freilagerbestand { get; set; }
        public string AssortmentID { get; set; }
        public int SizeIndex { get; set; }
    }
}