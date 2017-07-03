using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.CSVClasses
{
    public class SaisonCSVMap : CsvClassMap<source_Saisonumsatz>
    {
        public SaisonCSVMap()
        {
            Map(m => m.Saison).Index(0);
            Map(m => m.Kundennr).Index(1);
            Map(m => m.MengeVO).Index(2);
            Map(m => m.UmsatzVO).Index(3);
            Map(m => m.MengeGesamt).Index(4);
            Map(m => m.UmsatzGesamt).Index(5);          
        }
    }
}