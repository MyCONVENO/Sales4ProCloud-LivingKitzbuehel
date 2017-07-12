using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.CSVClasses
{
    public class BestandCSVMap : CsvClassMap<source_Stock>
    {        
        public BestandCSVMap()
        {
            Map(m => m.Lagernr).Index(0);
            Map(m => m.Artikelsaison).Index(1);
            Map(m => m.EAN).Index(2);
            Map(m => m.Freilagerbestand).Index(3);
            Map(m => m.FreiVerfuegbarerBestand).Index(4);
        }
    }
}