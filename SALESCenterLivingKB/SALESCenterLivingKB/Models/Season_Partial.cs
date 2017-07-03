using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SALESCenterLivingKB.Models
{
    public partial class Season
    {
        public string StartDateText
        {
            get
            {
                if (!DeliveryDateStart.HasValue)
                    return string.Empty;

                return DeliveryDateStart.Value.ToString("dd.MM.yyyy");
            }
        }

        public string EndDateText
        {
            get
            {
                if (!DeliveryDateEnd.HasValue)
                    return string.Empty;

                return DeliveryDateEnd.Value.ToString("dd.MM.yyyy");
            }
        }

        public string StartDateTextPlain
        {
            get
            {
                if (!DeliveryDateStart.HasValue)
                    return string.Empty;

                return DeliveryDateStart.Value.ToString("yyyy-MM-dd");
            }
        }

        public string EndDateTextPlain
        {
            get
            {
                if (!DeliveryDateEnd.HasValue)
                    return string.Empty;

                return DeliveryDateEnd.Value.ToString("yyyy-MM-dd");
            }
        }
    }
}