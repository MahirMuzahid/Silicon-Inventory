using Silicon_Inventory.Model;
using Silicon_Inventory.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace Silicon_Inventory.ViewModel
{
    public class RequisitionStatement : BaseViewModel
    {
        ObservableCollection<Warehouse> _warehouse = new ObservableCollection<Warehouse>();
        ObservableCollection<StockData> stockData = new ObservableCollection<StockData>();
        ObservableCollection<IssueVoucher> _ShowingReport = new ObservableCollection<IssueVoucher>();
        ObservableCollection<ReceiptVoucher> AllRecieptVoucher = new ObservableCollection<ReceiptVoucher>();
        public bool _isReqEnabled { get; set; }
        ObservableCollection<IssueVoucher> _searchIssueVoucher { get; set; }
        ObservableCollection<IssueVoucher> _allIssueVoucher { get; set; }
        IssueVoucher _requisition { get; set; }
        Warehouse _wareHouseName { get; set; }


        public int itemOrder = 0;
        public int slOrder = 0;
        public int iqOrder = 0;
        public string _conNameLbl { get; set; }
        public string _woLbl { get; set; }
        
        public bool _IsprintEnable { get; set; }


        public string _Error { get; set; }
        public string _ErrorColor { get; set; }

        public string red = "#E53E3E", green = "#18CB51 ", black = "#000000", blue = "#0C84F6";

        public ICommand Updater { get; set; }
        public RequisitionStatement()
        {
            //conNameLbl = "";
            //woLbl = "";
            //isReqEnabled = false;
            //Updater = new UpdaterForRequisitionReport(this);
       
            //IsprintEnable = false;
            //warehouse = StaticPageForAllData.WareHouse;
            //stockData = StaticPageForAllData.StockData;
            //allIssueVoucher = StaticPageForAllData.AllIssueVoucher;
            //AllRecieptVoucher = StaticPageForAllData.AllReceiptVoucher;
                      
        }
        public async Task Fresh()
        {
            StaticPageForAllData refresh = new StaticPageForAllData();
            await refresh.GetAllData().ConfigureAwait(false);

            warehouse = StaticPageForAllData.WareHouse;
            stockData = StaticPageForAllData.StockData;
            allIssueVoucher = StaticPageForAllData.AllIssueVoucher;
            AllRecieptVoucher = StaticPageForAllData.AllReceiptVoucher;
            stockData = StaticPageForAllData.StockData;

            ObservableCollection<IssueVoucher> fresh = new ObservableCollection<IssueVoucher>();
            ShowingReport = fresh;
        }
        public void printInPrinter()
        {
            
            StaticPageForAllData.printRequisitionReport = ShowingReport;
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
                PrintRequisition print = new PrintRequisition();
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
        public void makeAllSingleSearchVoucher()
        {
            int rrID = 1001;
            bool shouldIncrement = false;
            ObservableCollection<IssueVoucher> rrV = new ObservableCollection<IssueVoucher>();
            for (int i = 0; i < allIssueVoucher.Count; i++)
            {
                if (rrID == allIssueVoucher[i].IssueVoucherID)
                {
                    if(wareHouseName.wareHouseName == allIssueVoucher[i].wareHouseName)
                    {                       
                        rrV.Add(allIssueVoucher[i]);                        
                    }
                    rrID = rrID + 1;
                    i = -1;
                }
            }

            searchIssueVoucher = rrV;
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

        public void showReport()
        {
            ObservableCollection<IssueVoucher> objList = new ObservableCollection<IssueVoucher>();
            int sl = 1;
            for (int i = 0; i < allIssueVoucher.Count; i++)
            {
                if(allIssueVoucher[i].IssueVoucherID == requisition.IssueVoucherID && allIssueVoucher[i].IsPrinted == 1)
                {
                    for (int k = 0; k < stockData.Count; k++)
                    {
                        if (allIssueVoucher[i].itemNumber == stockData[k].itemNO)
                        {
                            allIssueVoucher[i].unit = stockData[k].unit;
                            break;
                        }
                    }
                    allIssueVoucher[i].SL = sl;
                    objList.Add(allIssueVoucher[i]);
                    sl++;
                }
                
            }
            
            ShowingReport = objList;
        }
        
        public void SortSL(int order)
        {
            if (order == 0)
            {
                List<IssueVoucher> spr = new List<IssueVoucher>();
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
            else if (order == 1)
            {
                List<IssueVoucher> spr = new List<IssueVoucher>();
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

        public void SortIssQuentity(int order)
        {
            if (order == 0)
            {
                List<IssueVoucher> spr = new List<IssueVoucher>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr = spr.OrderByDescending(o => o.IssueQuantity).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                iqOrder = 1;
            }
            else if (order == 1)
            {
                List<IssueVoucher> spr = new List<IssueVoucher>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr = spr.OrderBy(o => o.IssueQuantity).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                iqOrder = 0;
            }
        }

        public void SortItemName(int itemorder)
        {
            if (itemorder == 0)
            {
                List<IssueVoucher> spr = new List<IssueVoucher>();
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
                List<IssueVoucher> spr = new List<IssueVoucher>();
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
        

        public ObservableCollection<Warehouse> warehouse { get { return _warehouse; } set { _warehouse = value; OnPropertyChanged(nameof(warehouse)); } }
        public ObservableCollection<IssueVoucher> searchIssueVoucher { get { return _searchIssueVoucher; } set { _searchIssueVoucher = value;  OnPropertyChanged(nameof(searchIssueVoucher)); } }
        public ObservableCollection<IssueVoucher> allIssueVoucher { get { return _allIssueVoucher; } set { _allIssueVoucher = value; OnPropertyChanged(nameof(allIssueVoucher)); } }
        public ObservableCollection<IssueVoucher> ShowingReport { get { return _ShowingReport; } set { _ShowingReport = value; OnPropertyChanged(nameof(ShowingReport)); } }
        public string conNameLbl { get { return _conNameLbl; } set { _conNameLbl = value;  OnPropertyChanged(nameof(conNameLbl)); } }
        public string woLbl { get { return _woLbl; } set { _woLbl = value;  OnPropertyChanged(nameof(woLbl)); } }


        public bool IsprintEnable { get { return _IsprintEnable; } set { _IsprintEnable = value; OnPropertyChanged(nameof(IsprintEnable)); } }
        public bool isReqEnabled { get { return _isReqEnabled; } set { _isReqEnabled = value; OnPropertyChanged(nameof(isReqEnabled)); } }
       
        public string Error { get { return _Error; } set { _Error = value; OnPropertyChanged(nameof(Error)); } }
        public IssueVoucher requisition { get { return _requisition; } set { _requisition = value; if (requisition != null) { IsprintEnable = true; conNameLbl = "Contructor Name: " + requisition.contructorName;
                    woLbl = "Work Worder: " + requisition.workOrderNo;
                    showReport();
                } OnPropertyChanged(nameof(requisition)); } }
        public string ErrorColor { get { return _ErrorColor; } set { _ErrorColor = value; OnPropertyChanged(nameof(ErrorColor)); } }

        public Warehouse wareHouseName { get { return _wareHouseName; } set { 
                _wareHouseName = value;

                if(wareHouseName.wareHouseName != null)
                {
                    IsprintEnable = false;
                    isReqEnabled = true;
                    conNameLbl = "";
                    woLbl = "";
                    makeAllSingleSearchVoucher();            
                }                
                OnPropertyChanged(nameof(wareHouseName)); } }
    }
    public class UpdaterForRequisitionReport : ICommand
    {
        #region ICommand Members  
        public static string txtbx;
        private RequisitionStatement viewModel;
        public UpdaterForRequisitionReport(RequisitionStatement view)
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
            else  if (parameter.ToString() == "in")
            {
                viewModel.SortItemName(viewModel.itemOrder);
            }
            else if (parameter.ToString() == "iq")
            {
                viewModel.SortIssQuentity(viewModel.iqOrder);
            }
            else if (parameter.ToString() == "print")
            {
                viewModel.printInPrinter();
            }

        }



        #endregion
    }
}
