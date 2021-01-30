using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicon_Inventory.Model
{
    public class StockLadgerModel
    {
        public int SL { get; set; }
        public string Date { get; set; }
        public string Ticket { get; set; }
        public string Type { get; set; }
        public string OpeningBalance { get; set; }
        public int RecieptQty { get; set; }
        public int IssueQty{ get; set; }
        public int ReturnQty { get; set; }
        public string color { get; set; }
        public int ClosingBalance { get; set; }
        public string WorkOrderNo { get; set; }
        public string itemStart { get; set; }
        public string itemEnd { get; set; }
        public string itemRange { get; set; }
        public string storeName { get; set; }
    }
}
