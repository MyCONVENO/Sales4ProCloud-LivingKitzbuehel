using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.CSVClasses
{
    public class ArtikelCSVMap : CsvClassMap<source_Artikel>
    {        
        public ArtikelCSVMap()
        {
            Map(m => m.Artikelnr).Index(0);
            Map(m => m.Farbnr).Index(1);
            Map(m => m.Matchcode).Index(2);
            Map(m => m.Artikelbez1).Index(3);
            Map(m => m.Artikelbez2).Index(4);
            Map(m => m.Farbe).Index(5);
            Map(m => m.Groesse).Index(6);
            Map(m => m.EAN).Index(7);
            Map(m => m.Produktbereich).Index(8);
            Map(m => m.Produktgruppe).Index(9);
            Map(m => m.Kollektion).Index(10);
            Map(m => m.Artikelsaison).Index(11);
            Map(m => m.Groessenskala).Index(12);
            Map(m => m.lieferbareGrVon).Index(13);
            Map(m => m.lieferbareGrBis).Index(14);
            Map(m => m.Gewicht).Index(15);
        }
    }
}