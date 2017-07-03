using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.CSVClasses
{
    public class PreisCSVMap : CsvClassMap<source_Preis>
    {        
        public PreisCSVMap()
        {
            Map(m => m.Preislistenr).Index(0);
            Map(m => m.Saison).Index(1);
            Map(m => m.Waehrung).Index(2);
            Map(m => m.ArtikelnrFarbnr).Index(3);
            Map(m => m.Groesse).Index(4);
            Map(m => m.EAN).Index(5);
            Map(m => m.HEK).Index(6);
            Map(m => m.EmpfVK).Index(7);          
        }
    }
}