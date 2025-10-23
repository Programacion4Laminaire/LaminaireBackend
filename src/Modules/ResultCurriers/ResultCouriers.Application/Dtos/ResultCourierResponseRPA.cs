using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResultCouriers.Application.Dtos
{
    public class ResultCourierResponseRPA
    {
        public string status { get; set; } = " ";
        public int id { get; set; } = 0;

  
        public string courier { get; set; } = string.Empty;
        public string requestId { get; set; } = string.Empty;

        public decimal price { get; set; } = 0;
        public string currency { get; set; } = string.Empty;
        public string service { get; set; } = string.Empty;

      
        public string eta { get; set; } = string.Empty;
        public string pickupDate { get; set; } = string.Empty;
        public string deliveryDate { get; set; } = string.Empty;
        public string deliveredBy { get; set; } = string.Empty;

      
        public string priceText { get; set; } = string.Empty;
        public string rateEstimateText { get; set; } = string.Empty;

    }
}
