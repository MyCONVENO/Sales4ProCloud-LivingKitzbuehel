using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.CSVClasses
{
    public class VertreterCSVMap : CsvClassMap<source_Vertreter>
    {        
        public VertreterCSVMap()
        {
            Map(m => m.VertreterNr).Index(0);
            Map(m => m.Name1).Index(1);
            Map(m => m.Name2).Index(2);
            Map(m => m.Name3).Index(3);
            Map(m => m.Strasse).Index(4);
            Map(m => m.Land).Index(5);
            Map(m => m.Plz).Index(6);
            Map(m => m.Ort).Index(7);
            Map(m => m.Tel).Index(8);
            Map(m => m.Fax).Index(9);
            Map(m => m.Mobil).Index(10);
            Map(m => m.Email).Index(11);
            Map(m => m.Kundennr).Index(12);
            Map(m => m.UID).Index(13);
            Map(m => m.Steuernr).Index(14);            
        }
    }
}