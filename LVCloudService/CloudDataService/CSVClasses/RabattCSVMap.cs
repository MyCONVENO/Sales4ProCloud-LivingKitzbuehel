using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.CSVClasses
{
    public class RabattCSVMap : CsvClassMap<source_Rabatt>
    {        
        public RabattCSVMap()
        {
            Map(m => m.Kundennr).Index(0);
            Map(m => m.Match).Index(1);
            Map(m => m.Rabattart).Index(2);
            Map(m => m.Wert).Index(3);//.TypeConverter<WertConverter>(); ;
            Map(m => m.Berechenart).Index(4);
            Map(m => m.Wertstellung).Index(5);
            Map(m => m.Ebene).Index(6);                 
        }    
    }

    class WertConverter : ITypeConverter
    {
        public bool CanConvertFrom(Type type)
        {
            //if (type == typeof(double))
            //    return true;

            return true;
        }

        public bool CanConvertTo(Type type)
        {
            return true;
        }

        public object ConvertFromString(TypeConverterOptions options, string text)
        {
            return Convert.ToDouble(text);
        }

        public string ConvertToString(TypeConverterOptions options, object value)
        {
            return value.ToString();
        }
    }
}