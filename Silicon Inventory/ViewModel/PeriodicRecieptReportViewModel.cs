using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Silicon_Inventory.Model;
using System.Windows.Input;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Documents;
using Silicon_Inventory.View;

namespace Silicon_Inventory.ViewModel
{
    public class PeriodicRecieptReportViewModel : BaseViewModel
    {
        ObservableCollection<Warehouse> _warehouse = new ObservableCollection<Warehouse>();
        ObservableCollection<StockData> stockData = new ObservableCollection<StockData>();
        ObservableCollection<PeriodicRecieptReportModel> _ShowingReport = new ObservableCollection<PeriodicRecieptReportModel>();
        ObservableCollection<ReceiptVoucher> AllRecieptVoucher = new ObservableCollection<ReceiptVoucher>();
        Warehouse _wareHouseName { get; set; }
        DateTime From = new DateTime();
        DateTime To = new DateTime();

        public int itemOrder = 0;
        public int slOrder = 0;
        public int rcvOrder = 0;
        public string _FdateTxbx { get; set; }
        public string _FmonthTxbx { get; set; }
        public string _FyearTxbx { get; set; }
        public string _TdateTxbx { get; set; }
        public string _TmonthTxbx { get; set; }
        public string _TyearTxbx { get; set; }
        public bool _IsdetailChkChecked {get;set;}
        public bool _IssummaryChkChecked { get; set; }
        public bool _IsprintEnable { get; set; }
        public bool _dchkEnabled { get; set; }
        public bool _schkEnabled { get; set; }

        public string _Error { get; set; }
        public string _ErrorColor { get; set; }

        public string red = "#E53E3E", green = "#18CB51 ", black = "#000000", blue = "#0C84F6";

        public ICommand Updater { get; set; }
        public PeriodicRecieptReportViewModel()
        {
            Updater = new UpdaterForRecieptReport(this);
            schkEnabled = true;
            dchkEnabled = true;
            IsprintEnable = false;
            warehouse = StaticPageForAllData.WareHouse;
            stockData = StaticPageForAllData.StockData;
            AllRecieptVoucher = StaticPageForAllData.AllReceiptVoucher;
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

                if(wareHouseName == null)
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
                if (From > DateTime.Now || To > DateTime.Now)
                {
                    error = 5;
                }

            }
            catch (Exception ex)
            {
                error = 2;
                exTxt = ex.Message;
            }
           
            if(error == 1)
            {
                IsprintEnable = false;
                sendMsg("Enter Valid Date.", 1);
            }
            else if (error == 2)
            {
                IsprintEnable = false;
                sendMsg("Fill up all data", 0);
            }
            else if(error ==  4)
            {
                IsprintEnable = false;
                sendMsg("Select Warehouse", 1);
            }
            else if (error == 5)
            {
                IsprintEnable = false;
                sendMsg("Future date is not valid", 1);
            }
            else if (error == 0 && wareHouseName != null)
            {
                sendMsg("", 2);
                From = new DateTime(fy, fm, fd);
                To = new DateTime(ty, tm, td);
                ShowList();
            }

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

        public void ShowList()
        {
            ObservableCollection<PeriodicRecieptReportModel> rrReport = new ObservableCollection<PeriodicRecieptReportModel>();
            int sl = 1;
            for(int i = 0; i < AllRecieptVoucher.Count; i++)
            {
                DateTime thisVoucherDate = DateTime.ParseExact(AllRecieptVoucher[i].RRDate, "dd/M/yyyy",null);
                if((thisVoucherDate >= From && thisVoucherDate <= To) && AllRecieptVoucher[i].warehouse == wareHouseName.wareHouseName && AllRecieptVoucher[i].isPrinted == 1)
                {
                    PeriodicRecieptReportModel newRR = new PeriodicRecieptReportModel();

                    newRR.SL = sl;
                    newRR.RRVoucherID = AllRecieptVoucher[i].RRVoucherID;
                    newRR.RRDate = AllRecieptVoucher[i].RRDate;
                    newRR.warehouse = AllRecieptVoucher[i].warehouse;
                    newRR.wareHouseID = AllRecieptVoucher[i].wareHouseID;
                    newRR.itemNumber = AllRecieptVoucher[i].itemNumber;
                    newRR.description = AllRecieptVoucher[i].description;
                    newRR.RCVQuantity = AllRecieptVoucher[i].RCVQuantity;
                    for(int j = 0; j < stockData.Count; j++)
                    {
                        if (AllRecieptVoucher[i].itemNumber == stockData[j].itemNO)
                        {
                            newRR.unit = stockData[j].unit;
                            break;
                        }
                    }               
                    sl++;
                    rrReport.Add(newRR);

                }
            }
            
            ShowingReport = rrReport;
            IsprintEnable = true;
        }

        public void SortSL ( int order)
        {
            if(order == 0)
            {
                List<PeriodicRecieptReportModel> spr = new List<PeriodicRecieptReportModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr = spr.OrderByDescending(o => o.SL).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                slOrder = 1;
            }
            else if(order == 1)
            {
                List<PeriodicRecieptReportModel> spr = new List<PeriodicRecieptReportModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr = spr.OrderBy(o => o.SL).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                slOrder = 0;
            }
        }

        public void SortRcvQuentity(int order)
        {
            if (order == 0)
            {
                List<PeriodicRecieptReportModel> spr = new List<PeriodicRecieptReportModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr = spr.OrderByDescending(o => o.RCVQuantity).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                rcvOrder = 1;
            }
            else if (order == 1)
            {
                List<PeriodicRecieptReportModel> spr = new List<PeriodicRecieptReportModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr = spr.OrderBy(o => o.RCVQuantity).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                rcvOrder = 0;
            }
        }

        public void SortItemName(int itemorder)
        {
            if(itemorder == 0)
            {
                List<PeriodicRecieptReportModel> spr = new List<PeriodicRecieptReportModel>();
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
            else if(itemorder == 1)
            {
                List<PeriodicRecieptReportModel> spr = new List<PeriodicRecieptReportModel>();
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

        public void printInPrinter()
        {
            int fd = 0, fm = 0, fy = 0, td = 0, tm = 0, ty = 0;
            fd = int.Parse(FdateTxbx);
            fm = int.Parse(FmonthTxbx);
            fy = int.Parse(FyearTxbx);

            td = int.Parse(TdateTxbx);
            tm = int.Parse(TmonthTxbx);
            ty = int.Parse(TyearTxbx);
            ShowingReport[0].startDate = ""+fd + "/" + fm + "/" + fy;
            ShowingReport[0].endDate = "" + td + "/" + tm + "/" + ty;
            for(int i = 0; i < ShowingReport.Count; i++)
            {
                ShowingReport[i].SL = i+1;
            }


            StaticPageForAllData.printPeriodicRecieptReport = ShowingReport;
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
                PrintPeriodicRecieptReport print = new PrintPeriodicRecieptReport();
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

            if (myPrintDialog.ShowDialog() == true)
            {
                pd.PrintDocument(document.DocumentPaginator, "My Document");
            }
            //up = new UpdateViewCommand(viewModel);
        }


        public ObservableCollection<Warehouse> warehouse { get { return _warehouse; } set { _warehouse = value; OnPropertyChanged(nameof(warehouse)); } }
        public ObservableCollection<PeriodicRecieptReportModel> ShowingReport { get { return _ShowingReport; } set { _ShowingReport = value; OnPropertyChanged(nameof(ShowingReport)); } }
        public string FdateTxbx { get { return _FdateTxbx; } set { _FdateTxbx = value; checkDate(); OnPropertyChanged(nameof(FdateTxbx)); } }
        public string FmonthTxbx { get { return _FmonthTxbx; } set { _FmonthTxbx = value; checkDate(); OnPropertyChanged(nameof(FmonthTxbx)); } }
        public string FyearTxbx { get { return _FyearTxbx; } set { _FyearTxbx = value; checkDate(); OnPropertyChanged(nameof(FyearTxbx)); } }

        public string TdateTxbx { get { return _TdateTxbx; } set { _TdateTxbx = value; checkDate(); OnPropertyChanged(nameof(TdateTxbx)); } }
        public string TmonthTxbx { get { return _TmonthTxbx; } set { _TmonthTxbx = value; checkDate(); OnPropertyChanged(nameof(TmonthTxbx)); } }
        public string TyearTxbx { get { return _TyearTxbx; } set { _TyearTxbx = value; checkDate(); OnPropertyChanged(nameof(TyearTxbx)); } }

        public bool IsprintEnable { get { return _IsprintEnable; } set { _IsprintEnable = value; OnPropertyChanged(nameof(IsprintEnable)); } }
        public bool dchkEnabled { get { return _dchkEnabled; } set {  _dchkEnabled = value; OnPropertyChanged(nameof(dchkEnabled)); } }
        public bool schkEnabled { get { return _schkEnabled; } set { _schkEnabled = value; OnPropertyChanged(nameof(schkEnabled)); } }


        public string Error { get { return _Error; } set { _Error = value; OnPropertyChanged(nameof(Error)); } }
        public string ErrorColor { get { return _ErrorColor; } set { _ErrorColor = value; OnPropertyChanged(nameof(ErrorColor)); } }

        public Warehouse wareHouseName { get { return _wareHouseName; } set { _wareHouseName = value; checkDate();  OnPropertyChanged(nameof(wareHouseName)); } }
    }
    class UpdaterForRecieptReport : ICommand
    {
        #region ICommand Members  
        public static string txtbx;
        private PeriodicRecieptReportViewModel viewModel;
        public UpdaterForRecieptReport(PeriodicRecieptReportViewModel view)
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
            if (parameter.ToString() == "in")
            {
                viewModel.SortItemName(viewModel.itemOrder);
            }
            else if (parameter.ToString() == "rcv")
            {
                viewModel.SortRcvQuentity(viewModel.rcvOrder);
            }
            else if (parameter.ToString() == "print")
            {
                viewModel.printInPrinter();
            }

        }



        #endregion
    }
}
