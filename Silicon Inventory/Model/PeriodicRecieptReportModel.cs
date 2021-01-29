using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicon_Inventory.Model
{
    public class PeriodicRecieptReportModel 
    {
        public int SL { get; set; }
        public int RRVoucherID { get; set; }
        public string RRDate { get; set; }
        public string warehouse { get; set; }
        public string unit { get; set; }
        public string itemNumber { get; set; }
        public string description { get; set; }
        public int RCVQuantity { get; set; }
        public int wareHouseID { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }

    }
}
