using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SALESCenterLivingKB.Models
{
    public partial class Pricelist
    {
        public string Summary { get { return PricelistName + "(" + Mandant + ")"; } }
        public string Mandant
        {
            get
            {
                if (PricelistClientID == "1")
                    return "Living Kitzbühel";
                //if (PricelistClientID == "2")
                //    return "SOLIDUS";

                return "";
            }
        }
    }
}