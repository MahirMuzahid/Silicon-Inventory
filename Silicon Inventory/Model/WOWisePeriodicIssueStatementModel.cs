using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicon_Inventory.Model
{
    public class WOWisePeriodicIssueStatementModel
    {
        public int SL { get; set; }
        public string itemNumber { get; set; }
        public string description { get; set; }
        public string warehouseName { get; set; }
        public string unit { set; get; }
        public string startDate { set; get; }
        public string endDate { set; get; }
        public string startItem { set; get; }
        public string endItem { set; get; }
        public int issueQnty { set; get; }
        public int retQnty { get; set; }
        public int netIssueQnty { get; set; }
        public string contructor { get; set; }
        public string workorder { get; set; }

    }
}
