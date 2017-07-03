using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.ImportClass
{
    public class ImportColor
    {
        public string ArticleID { get; set; }
        public string ColorID { get; set; }
        public string Kollektion { get; set; }
        public string Artikelnr { get; set; }
        public string Farbe { get; set; }
        public string Farbnr { get; set; }
        public string GroesseVon { get; set; }
        public string GroesseBis { get; set; }

        public DateTime DeliveryDateStart { get; set; }
    }
}