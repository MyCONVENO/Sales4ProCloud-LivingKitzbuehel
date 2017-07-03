using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.CSVClasses
{
    public class ArtikelbeschreibungCSVMap : CsvClassMap<source_Artikelbeschreibung>
    {        
        public ArtikelbeschreibungCSVMap()
        {
            Map(m => m.Artikelnr).Index(0);
            Map(m => m.Artikelsaison).Index(1);
            Map(m => m.Artikelbeschreibung).Index(2);           
        }
    }
}