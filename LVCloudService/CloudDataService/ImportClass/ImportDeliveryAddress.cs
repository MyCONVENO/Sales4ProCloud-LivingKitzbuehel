using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.ImportClass
{
    public class ImportDeliveryAddress
    {        
        public string Kundennr { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public string Strasse { get; set; }
        public string Plz { get; set; }
        public string Ort { get; set; }
        public string Land { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Bemerkung { get; set; }
        public string CustomerID { get; set; }
        public string Warenempfaengernr { get; set; }
        public string DeliveryAddressID { get; set; }       
    }
}