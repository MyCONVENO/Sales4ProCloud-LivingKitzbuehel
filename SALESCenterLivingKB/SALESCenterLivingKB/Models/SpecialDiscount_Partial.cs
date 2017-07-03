using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SALESCenterLivingKB.Models
{
    public partial class SpecialDiscount
    {
        public string Mandant
        {
            get
            {
                if (SpecialDiscountClientID == "1")
                    return "Living Kitzbühel";
                if (SpecialDiscountClientID == "2")
                    return "SOLIDUS";

                return "";
            }
        }

        public string StartDateText
        {
            get
            {
                return StartDate.ToString("dd.MM.yyyy");
            }
        }

        public string EndDateText
        {
            get
            {
                return EndDate.ToString("dd.MM.yyyy");
            }
        }

        public string StartDateTextPlain
        {
            get
            {
                return StartDate.ToString("yyyy-MM-dd");
            }
        }

        public string EndDateTextPlain
        {
            get
            {
                return EndDate.ToString("yyyy-MM-dd");
            }
        }
    }
}