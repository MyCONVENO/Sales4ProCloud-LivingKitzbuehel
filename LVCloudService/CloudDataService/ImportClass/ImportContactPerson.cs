using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.ImportClass
{
    public class ImportContactPerson
    {
        public string Kundennr { get; set; }
        public string ASP_Nr { get; set; }
        public string ASP_Bereich { get; set; }
        public string ASP_Vorname { get; set; }
        public string ASP_Name1 { get; set; }
        public string ASP_Name2 { get; set; }
        public string ASP_Telefon { get; set; }
        public string ASP_Mobiltelefon { get; set; }
        public string ASP_Fax { get; set; }
        public string ASP_Bemerkung { get; set; }
        public string ASP_Email { get; set; }

        public string CustomerID { get; set; }
        public string ContactPersonID { get; set; }

    }
}