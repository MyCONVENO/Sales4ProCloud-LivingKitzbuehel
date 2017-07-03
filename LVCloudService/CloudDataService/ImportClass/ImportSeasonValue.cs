using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.ImportClass
{
    public class ImportSeasonValue
    {
        public string SeasonValueID { get; set; }
        public string CustomerID { get; set; }
        public string SeasonID { get; set; }
        public int QtyPreOrder { get; set; }
        public int Qty { get; set; }
        public decimal ValuePreOrder { get; set; }
        public decimal Value { get; set; }
        public System.DateTime SyncDateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}