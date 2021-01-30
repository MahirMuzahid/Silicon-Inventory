using Silicon_Inventory.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Silicon_Inventory.Model
{
    public class PrintForSteackLadger
    {
        public string SL { get; set; }
        public string Date { get; set; }
        public string Ticket { get; set; }
        public string Type { get; set; }
        public string OpeningBalance { get; set; }
        public string RecieptQty { get; set; }
        public string IssueQty { get; set; }
        public string ReturnQty { get; set; }
        public string color { get; set; }
        public string ClosingBalance { get; set; }
        public string itemRange { get; set; }
        public string WorkOrderNo { get; set; }
        public string storename { get; set; }


    }
}
