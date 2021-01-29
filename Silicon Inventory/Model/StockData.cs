using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicon_Inventory.Model
{
    public class StockData
    {
        public int SL { get; set; }
        public int wareHouseID { get; set; }
        public int itemID { get; set; }
        public string itemNO { get; set; }
        public int currentBalance { get; set; }
        public string description { get; set; }
        public string wareHouseName { get; set; }
        public string unit { get; set; }
        public string Response { get; set; }
    }
}
