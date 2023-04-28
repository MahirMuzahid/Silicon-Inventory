using Silicon_Inventory.Model;
using Silicon_Inventory.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace Silicon_Inventory.ViewModel
{
    public class WOWisePeriodicIssueStatementViewModel: BaseViewModel
    {
        #region Variable
        int sl = 1;
        public ObservableCollection<Warehouse> _warehouse { get; set; }
        public Warehouse _wareHouseName { get; set; }
        public ObservableCollection<IssueVoucher> allIssueVoucher { get; set; }
        public ObservableCollection<ReturnVoucher> allRetrunVocuher { get; set; }
        public ObservableCollection<StockData> stockData { get; set; }
        public ObservableCollection<WOWisePeriodicIssueStatementModel> _ShowingReport { get; set; }
        public ObservableCollection<Contructor> _contructor { get; set; }
        public Contructor _contructorName { get; set; }
        public Item _itemNumberEnd { get; set; }
       
        public Item _itemNumberStart { get; set; }
       
        public ObservableCollection<WorkOrder> _workorder { get; set; }
        public ObservableCollection<Item> _item { get; set; }
        
        public WorkOrder _workorderNo { get; set; }       
        public string _FdateTxbx { get; set; }
        public string _FmonthTxbx { get; set; }
        public string _FyearTxbx { get; set; }
        public string _TdateTxbx { get; set; }
        public string _TmonthTxbx { get; set; }
        public string _TyearTxbx { get; set; }

        public bool _IsprintEnable { get; set; }

        public string _Error { get; set; }
        public string _ErrorColor { get; set; }
        public int slOrder = 0, itemOrder = 0, issOrder = 0, retOrder = 0, netIss = 0;
        DateTime From = new DateTime();
        DateTime To = new DateTime();
        public string red = "#E53E3E", green = "#18CB51 ", black = "#000000", blue = "#0C84F6";

        public ICommand Updater { get; set; }
        #endregion

        #region method
        public WOWisePeriodicIssueStatementViewModel()
        {
            //Updater = new UpdaterForWOCoWiseReport(this);
            //contructor = StaticPageForAllData.Contructor;
            //warehouse = StaticPageForAllData.WareHouse;
            //workorder = StaticPageForAllData.WorkOrder;
            //item = StaticPageForAllData.Items;
            //allIssueVoucher = StaticPageForAllData.AllIssueVoucher;
            //allRetrunVocuher = StaticPageForAllData.AllReturnVoucher;
            //stockData = StaticPageForAllData.StockData;
        }
        public async Task Fresh()
        {
            StaticPageForAllData refresh = new StaticPageForAllData();
            await refresh.GetAllData().ConfigureAwait(false);

            contructor = StaticPageForAllData.Contructor;
            warehouse = StaticPageForAllData.WareHouse;
            workorder = StaticPageForAllData.WorkOrder;
            item = StaticPageForAllData.Items;
            allIssueVoucher = StaticPageForAllData.AllIssueVoucher;
            allRetrunVocuher = StaticPageForAllData.AllReturnVoucher;
            stockData = StaticPageForAllData.StockData;

            ObservableCollection<WOWisePeriodicIssueStatementModel> fresh = new ObservableCollection<WOWisePeriodicIssueStatementModel>();
            ShowingReport = fresh;
        }
        public void ShowList()
        {
            sl = 1;
            try
            {
                ObservableCollection<WOWisePeriodicIssueStatementModel> newObjectList = new ObservableCollection<WOWisePeriodicIssueStatementModel>();
                WOWisePeriodicIssueStatementModel newObj = new WOWisePeriodicIssueStatementModel();
                for (int i = 0; i < item.Count; i++)
                {
                    if (itemNumberStart == null && itemNumberEnd == null)
                    {
                        newObj = readyReport(i);
                        newObjectList.Add(newObj);
                    }
                    else if (((item[i].itemID >= itemNumberStart.itemID && item[i].itemID <= itemNumberEnd.itemID)))
                    {
                        newObj = readyReport(i);
                        newObjectList.Add(newObj);
                    }
                }
                IsprintEnable = true;
                ShowingReport = newObjectList;
            }
            catch
            {
                IsprintEnable = false;
            }
            
        }
       
        public WOWisePeriodicIssueStatementModel readyReport(int i)
        {            
            WOWisePeriodicIssueStatementModel newObj = new WOWisePeriodicIssueStatementModel();
            int isCnt = 0, retCnt = 0, totCnt = 0;
            for (int j = 0; j < allIssueVoucher.Count; j++)
            {
                DateTime thisVoucherDate = DateTime.ParseExact(allIssueVoucher[j].IssueVoucherDate, "dd/M/yyyy", null);
                if(workorderNo != null)
                {
                    if (allIssueVoucher[j].contructorName == contructorName.contructorName &&
                    allIssueVoucher[j].wareHouseName == wareHouseName.wareHouseName &&
                    allIssueVoucher[j].IsPrinted == 1 &&
                    item[i].itemNumber == allIssueVoucher[j].itemNumber &&
                    workorderNo.workOrderNo == allIssueVoucher[j].workOrderNo &&
                    (thisVoucherDate >= From && thisVoucherDate <= To))
                    {
                        isCnt = isCnt + allIssueVoucher[j].IssueQuantity;
                    }
                }
                if( workorderNo == null)
                {
                    if (allIssueVoucher[j].contructorName == contructorName.contructorName &&
                    allIssueVoucher[j].wareHouseName == wareHouseName.wareHouseName &&
                    allIssueVoucher[j].IsPrinted == 1 &&
                    item[i].itemNumber == allIssueVoucher[j].itemNumber &&
                    (thisVoucherDate >= From && thisVoucherDate <= To))
                    {
                        isCnt = isCnt + allIssueVoucher[j].IssueQuantity;
                    }
                }               
            }
            newObj.issueQnty = isCnt;
            for (int l = 0; l < allRetrunVocuher.Count; l++)
            {
                DateTime thisVoucherDate = DateTime.ParseExact(allRetrunVocuher[l].returnDate, "dd/M/yyyy", null); ;
                if (allRetrunVocuher[l].contructor == contructorName.contructorName &&
                    allRetrunVocuher[l].ret_warehouse == wareHouseName.wareHouseName &&
                    allRetrunVocuher[l].isPrinted == 1 &&
                    item[i].itemNumber == allRetrunVocuher[l].ItemNo &&
                    (thisVoucherDate >= From && thisVoucherDate <= To))
                {
                    retCnt = retCnt + allRetrunVocuher[l].quentity;
                }
            }
            newObj.retQnty = retCnt;
            for (int k = 0; k < stockData.Count; k++)
            {
                if (item[i].itemNumber == stockData[k].itemNO)
                {
                    newObj.unit = stockData[k].unit;
                    break;
                }
            }
            newObj.itemNumber = item[i].itemNumber;
            newObj.description = item[i].itemName;
            newObj.SL = sl;
            newObj.netIssueQnty = newObj.issueQnty - newObj.retQnty;
            sl++;
            return newObj;
        }

        public void sendMsg(string msg, int msgOrError)
        {
            if (msgOrError == 0)
            {
                Error = "Message: " + msg;
                ErrorColor = blue;
            }
            else if (msgOrError == 1)
            {
                Error = "Error: " + msg;
                ErrorColor = red;
            }
            else if (msgOrError == 2)
            {
                Error = "";
            }

        }

        public void checkDate()
        {
            string exTxt = "";
            int error = 0; ;
            int fd = 0, fm = 0, fy = 0, td = 0, tm = 0, ty = 0;
            try
            {
                fd = int.Parse(FdateTxbx);
                fm = int.Parse(FmonthTxbx);
                fy = int.Parse(FyearTxbx);

                td = int.Parse(TdateTxbx);
                tm = int.Parse(TmonthTxbx);
                ty = int.Parse(TyearTxbx);

                if (wareHouseName == null || contructorName == null)
                {
                    error = 3;

                }
                else if ((fd > 31 || fd < 1) || (td > 31 || td < 1))
                {
                    error = 1;
                }
                else if ((fm > 12 || fm < 1) || (tm > 12 || tm < 1))
                {
                    error = 1;
                }
                else if ((fy < 2018 || fy > int.Parse(DateTime.Now.ToString("yyyy"))) || (ty < 2018 || ty > int.Parse(DateTime.Now.ToString("yyyy"))))
                {
                    error = 1;
                }

                From = new DateTime(fy, fm, fd);
                To = new DateTime(ty, tm, td);
                if(From > DateTime.Now || To > DateTime.Now)
                {
                    error = 5;
                }
                if(From > To)
                {
                    error = 6;
                }
            }
            catch (Exception ex)
            {
                error = 2;
                exTxt = ex.Message;
            }
            
            if (error == 1)
            {
                IsprintEnable = false;
                sendMsg("Enter Valid Date.", 1);
            }
            else if (error == 2)
            {
                IsprintEnable = false;
                sendMsg("Fill up all data", 0);
            }
            else if (error == 4)
            {
                IsprintEnable = false;
                sendMsg("Select Warehouse or Contructor", 1);
            }
            else if (error == 5)
            {
                IsprintEnable = false;
                sendMsg("Future date is not valid", 1);
            }
            else if (error == 6)
            {
                IsprintEnable = false;
                sendMsg("\"From\" date can't bigger then \"To\" date ", 1);
            }
            else if (error == 0 && wareHouseName != null && contructorName != null)
            {
                sendMsg("", 2);
                From = new DateTime(fy, fm, fd);
                To = new DateTime(ty, tm, td);
                ShowList();
            }
        }

        public void SortSL(int order)
        {
            if (order == 0)
            {
                List<WOWisePeriodicIssueStatementModel> wos = new List<WOWisePeriodicIssueStatementModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderByDescending(o => o.SL).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                slOrder = 1;
            }
            else if (order == 1)
            {
                List<WOWisePeriodicIssueStatementModel> wos = new List<WOWisePeriodicIssueStatementModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderBy(o => o.SL).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                slOrder = 0;
            }
        }
        public void SortItemName(int itemorder)
        {
            if (itemorder == 0)
            {
                List<WOWisePeriodicIssueStatementModel> spr = new List<WOWisePeriodicIssueStatementModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr.Sort((x, y) => string.Compare(x.itemNumber, y.itemNumber));
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                itemOrder = 1;
            }
            else if (itemorder == 1)
            {
                List<WOWisePeriodicIssueStatementModel> spr = new List<WOWisePeriodicIssueStatementModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr.Sort((y, x) => string.Compare(x.itemNumber, y.itemNumber));
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                itemOrder = 0;
            }
        }
        public void SortIssueQuentity(int order)
        {
            if (order == 0)
            {
                List<WOWisePeriodicIssueStatementModel> wos = new List<WOWisePeriodicIssueStatementModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderByDescending(o => o.issueQnty).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                issOrder = 1;
            }
            else if (order == 1)
            {
                List<WOWisePeriodicIssueStatementModel> wos = new List<WOWisePeriodicIssueStatementModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderBy(o => o.issueQnty).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                issOrder = 0;
            }
        }
        public void SortRetQuentity(int order)
        {
            if (order == 0)
            {
                List<WOWisePeriodicIssueStatementModel> wos = new List<WOWisePeriodicIssueStatementModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderByDescending(o => o.retQnty).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                retOrder = 1;
            }
            else if (order == 1)
            {
                List<WOWisePeriodicIssueStatementModel> wos = new List<WOWisePeriodicIssueStatementModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderBy(o => o.retQnty).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                retOrder = 0;
            }
        }
        public void SortTotQuentity(int order)
        {
            if (order == 0)
            {
                List<WOWisePeriodicIssueStatementModel> wos = new List<WOWisePeriodicIssueStatementModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderByDescending(o => o.netIssueQnty).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                netIss = 1;
            }
            else if (order == 1)
            {
                List<WOWisePeriodicIssueStatementModel> wos = new List<WOWisePeriodicIssueStatementModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderBy(o => o.netIssueQnty).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                netIss = 0;
            }
        }
        #endregion

        public void printInPrinter()
        {
            int fd = 0, fm = 0, fy = 0, td = 0, tm = 0, ty = 0;
            fd = int.Parse(FdateTxbx);
            fm = int.Parse(FmonthTxbx);
            fy = int.Parse(FyearTxbx);

            td = int.Parse(TdateTxbx);
            tm = int.Parse(TmonthTxbx);
            ty = int.Parse(TyearTxbx);
            ShowingReport[0].startDate = "" + fd + "/" + fm + "/" + fy;
            ShowingReport[0].endDate = "" + td + "/" + tm + "/" + ty;
            for (int i = 0; i < ShowingReport.Count; i++)
            {
                ShowingReport[i].SL = i + 1;
            }

            ShowingReport[0].contructor = contructorName.contructorName;
            ShowingReport[0].warehouseName = wareHouseName.wareHouseName;
            if(itemNumberStart != null & itemNumberEnd != null)
            {
                ShowingReport[0].startItem = itemNumberStart.itemNumber;
                ShowingReport[0].endItem = itemNumberEnd.itemNumber;
            }
            else
            {
                ShowingReport[0].startItem = "All";
                ShowingReport[0].endItem = "All";
            }
            ShowingReport[0].warehouseName = wareHouseName.wareHouseName;
            if (workorderNo != null)
            {
                ShowingReport[0].workorder = workorderNo.workOrderNo;
            }
            else
            {
                ShowingReport[0].workorder = "All Work Order";
            }
            StaticPageForAllData.printWOWisePeriodicIssue = ShowingReport;
            StaticPageForAllData.printNumber = 3;

            if (ShowingReport.Count % 30 > 0)
            {
                StaticPageForAllData.pageNumber = (ShowingReport.Count / 30) + 1;
            }
            else
            {
                StaticPageForAllData.pageNumber = (ShowingReport.Count / 30);
            }
            StaticPageForAllData.PrintedpageNumber = 1;

            PrintDialog myPrintDialog = new PrintDialog();

            var pageSize = new Size(8.26 * 96, 11.69 * 96);
            var document = new FixedDocument();
            document.DocumentPaginator.PageSize = pageSize;
            for (int i = 0; i < StaticPageForAllData.pageNumber; i++)
            {
                WOWisePrint print = new WOWisePrint();
                // Create Fixed Page.
                var fixedPage = new FixedPage();
                fixedPage.Width = pageSize.Width;
                fixedPage.Height = pageSize.Height;

                // Add visual, measure/arrange page.
                fixedPage.Children.Add((UIElement)print);
                fixedPage.Measure(pageSize);
                fixedPage.Arrange(new Rect(new Point(), pageSize));
                fixedPage.UpdateLayout();

                // Add page to document
                var pageContent = new PageContent();
                ((IAddChild)pageContent).AddChild(fixedPage);
                document.Pages.Add(pageContent);
            }
            // Send to the printer.
            var pd = new PrintDialog();
            try
            {
                if (myPrintDialog.ShowDialog() == true)
                {
                    pd.PrintDocument(document.DocumentPaginator, "My Document");
                }
            }
            catch
            {
                sendMsg("Please Make A New File",1);
            }
            
            //up = new UpdateViewCommand(viewModel);
        }

        #region Veriable Binding
        public string FdateTxbx { get { return _FdateTxbx; } set { _FdateTxbx = value; checkDate(); OnPropertyChanged(nameof(FdateTxbx)); } }
        public string FmonthTxbx { get { return _FmonthTxbx; } set { _FmonthTxbx = value; checkDate(); OnPropertyChanged(nameof(FmonthTxbx)); } }
        public string FyearTxbx { get { return _FyearTxbx; } set { _FyearTxbx = value; checkDate(); OnPropertyChanged(nameof(FyearTxbx)); } }

        public string TdateTxbx { get { return _TdateTxbx; } set { _TdateTxbx = value; checkDate(); OnPropertyChanged(nameof(TdateTxbx)); } }
        public string TmonthTxbx { get { return _TmonthTxbx; } set { _TmonthTxbx = value; checkDate(); OnPropertyChanged(nameof(TmonthTxbx)); } }
        public string TyearTxbx { get { return _TyearTxbx; } set { _TyearTxbx = value; checkDate(); OnPropertyChanged(nameof(TyearTxbx)); } }
        public Contructor contructorName { get { return _contructorName; } set { _contructorName = value; checkDate(); OnPropertyChanged(nameof(contructorName)); } }
        public ObservableCollection<Item> item { get { return _item; } set { _item = value; OnPropertyChanged(nameof(item)); } }
        public Item itemNumberEnd { get { return _itemNumberEnd; } set { _itemNumberEnd = value; checkDate(); OnPropertyChanged(nameof(itemNumberEnd)); } }
        public Item itemNumberStart { get { return _itemNumberStart; } set { _itemNumberStart = value; checkDate(); OnPropertyChanged(nameof(itemNumberStart)); } }
        public ObservableCollection<Contructor> contructor { get { return _contructor; } set { _contructor = value; OnPropertyChanged(nameof(contructor)); } }
        public ObservableCollection<WOWisePeriodicIssueStatementModel> ShowingReport { get { return _ShowingReport; } set { _ShowingReport = value; OnPropertyChanged(nameof(ShowingReport)); } }
        public ObservableCollection<WorkOrder> workorder { get { return _workorder; } set { _workorder = value; OnPropertyChanged(nameof(workorder)); } }
        public Warehouse wareHouseName { get { return _wareHouseName; } set { _wareHouseName = value; checkDate(); OnPropertyChanged(nameof(wareHouseName)); } }
        public ObservableCollection<Warehouse> warehouse { get { return _warehouse; } set { _warehouse = value; OnPropertyChanged(nameof(warehouse)); } }
        public WorkOrder workorderNo { get { return _workorderNo; } set { _workorderNo = value; checkDate();OnPropertyChanged(nameof(workorderNo)); } }
        public string Error { get { return _Error; } set { _Error = value; OnPropertyChanged(nameof(Error)); } }
        public string ErrorColor { get { return _ErrorColor; } set { _ErrorColor = value; OnPropertyChanged(nameof(ErrorColor)); } }
        public bool IsprintEnable { get { return _IsprintEnable; } set { _IsprintEnable = value; OnPropertyChanged(nameof(IsprintEnable)); } }
        #endregion

    }
    class UpdaterForWOCoWiseReport : ICommand
    {
        #region ICommand Members  
        public static string txtbx;
        private WOWisePeriodicIssueStatementViewModel viewModel;
        public UpdaterForWOCoWiseReport(WOWisePeriodicIssueStatementViewModel view)
        {
            this.viewModel = view;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public void Execute(object parameter)
        {
            if (parameter.ToString() == "sl")
            {
                viewModel.SortSL(viewModel.slOrder);
            }
            else if (parameter.ToString() == "in")
            {
                viewModel.SortItemName(viewModel.itemOrder);
            }
            else if (parameter.ToString() == "iq")
            {
                viewModel.SortIssueQuentity(viewModel.issOrder);
            }
            else if (parameter.ToString() == "rq")
            {
                viewModel.SortRetQuentity(viewModel.retOrder);
            }
            else if (parameter.ToString() == "ni")
            {
                viewModel.SortTotQuentity(viewModel.netIss);
            }
            else if (parameter.ToString() == "print")
            {
                viewModel.printInPrinter();
            }
            else if (parameter.ToString() == "refresh")
            {
                viewModel.Fresh();
            }

        }



        #endregion
    }
}
