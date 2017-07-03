using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.CSVClasses
{
    public class AnsprechpartnerCSVMap : CsvClassMap<source_Ansprechpartner>
    {
        public AnsprechpartnerCSVMap()
        {
            Map(m => m.Kundennr).Index(0);
            Map(m => m.ASP_Nr).Index(1);
            Map(m => m.ASP_Bereich).Index(2);
            Map(m => m.ASP_Vorname).Index(3);
            Map(m => m.ASP_Name1).Index(4);
            Map(m => m.ASP_Name2).Index(5);
            Map(m => m.ASP_Telefon).Index(6);
            Map(m => m.ASP_Mobiltelefon).Index(7);
            Map(m => m.ASP_Fax).Index(8);
            Map(m => m.ASP_Bemerkung).Index(9);
            Map(m => m.ASP_Email).Index(10);
        }
    }
}