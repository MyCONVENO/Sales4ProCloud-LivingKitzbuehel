using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.CSVClasses
{
    public class KundeCSVMap : CsvClassMap<source_Kunde>
    {        
        public KundeCSVMap()
        {
            Map(m => m.Kundennr).Index(0);
            Map(m => m.Name1).Index(1);
            Map(m => m.Name2).Index(2);
            Map(m => m.Name3).Index(3);
            Map(m => m.Strasse).Index(4);
            Map(m => m.Plz).Index(5);
            Map(m => m.Ort).Index(6);
            Map(m => m.Land).Index(7);
            Map(m => m.Zahlungsbed).Index(8);
            Map(m => m.Zahlungsart).Index(9);
            Map(m => m.Tel).Index(10);
            Map(m => m.Email).Index(11);
            Map(m => m.Fax).Index(12);
            Map(m => m.homepage).Index(13);
            Map(m => m.Bonitaetsstufe).Index(14);
            Map(m => m.Kundentyp).Index(15);
            Map(m => m.Vertrerternr).Index(16);
            Map(m => m.UID_Nummer).Index(17);
            Map(m => m.Preislistennr).Index(18);
            Map(m => m.Mwst_KZ).Index(19);
            Map(m => m.Steuerschluessel).Index(20);
            Map(m => m.Rechnungsempfaenger).Index(21);
            Map(m => m.Bonitaet).Index(22);
            Map(m => m.EKV).Index(23);
            Map(m => m.EKV_KTO).Index(24);
            Map(m => m.Forderungen).Index(25);
        }
    }
}