using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.ImportClass
{
    public class ImportPrice
    {
        public string PricelistID { get; set; }
        public string ColorID { get; set; }
        public string SizerunID { get; set; }
        public decimal HEK { get; set; }
        public decimal EmpfVK { get; set; }
        public string PriceID { get; set; }      
    }
}