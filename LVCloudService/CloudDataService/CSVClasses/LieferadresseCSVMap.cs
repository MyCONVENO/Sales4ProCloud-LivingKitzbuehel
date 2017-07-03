using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.CSVClasses
{
    public class LieferadresseCSVMap : CsvClassMap<source_Lieferadresse>
    {
        public LieferadresseCSVMap()
        {
            Map(m => m.Kundennr).Index(0);
            Map(m => m.Lieferort).Index(1);
            Map(m => m.Matchcode).Index(2);
            Map(m => m.Name1).Index(3);
            Map(m => m.Name2).Index(4);
            Map(m => m.Name3).Index(5);
            Map(m => m.Strasse).Index(6);
            Map(m => m.Plz).Index(7);
            Map(m => m.Ort).Index(8);
            Map(m => m.Land).Index(9);
            Map(m => m.Telefon).Index(10);
            Map(m => m.Email).Index(11);
            Map(m => m.Fax).Index(12);
            Map(m => m.Bemerkung).Index(13);
            Map(m => m.ILN).Index(14);
            Map(m => m.Filiale).Index(15);
            Map(m => m.UKAdresse).Index(16);
            Map(m => m.Bundesstaat).Index(17);
            Map(m => m.Sendungsenpfaenger).Index(18);
            Map(m => m.Abladestelle).Index(19);
            Map(m => m.Warenempfaengernr).Index(20);
        }
    }
}