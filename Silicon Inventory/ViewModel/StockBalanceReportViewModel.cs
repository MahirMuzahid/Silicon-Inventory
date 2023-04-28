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
    public class StockBalanceReportViewModel: BaseViewModel
    {
        ObservableCollection<Warehouse> _warehouse = new ObservableCollection<Warehouse>();
        ObservableCollection<StockData> stockData = new ObservableCollection<StockData>();
        ObservableCollection<StockData> _ShowingReport = new ObservableCollection<StockData>();
        ObservableCollection<Item> _item = new ObservableCollection<Item>();
        ObservableCollection<ReceiptVoucher> AllRecieptVoucher = new ObservableCollection<ReceiptVoucher>();
        public Item _itemNumberEnd { get; set; }

        public Item _itemNumberStart { get; set; }
        public bool _enbaleItem { get; set; }
        public bool _enbaleItem2 { get; set; }
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
        public StockBalanceReportViewModel()
        {
            //conNameLbl = "";
            //woLbl = "";
            //enbaleItem = false;
            //enbaleItem2 = false;
            //Updater = new UpdaterForStockReport(this);

            //IsprintEnable = false;
            //item = StaticPageForAllData.Items;
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

            ObservableCollection<StockData> fresh = new ObservableCollection<StockData>();
            ShowingReport = fresh;
        }
        public void ShowReport()
        {
            int sl = 1;
            ObservableCollection<StockData> objList = new ObservableCollection<StockData>();
            for(int i = 0; i < stockData.Count; i++)
            {
                if (stockData[i].wareHouseID == wareHouseName.wareHouseID && (stockData[i].itemID >= itemNumberStart.itemID && stockData[i].itemID <= itemNumberEnd.itemID))
                {
                    for(int  k = 0; k < item.Count; k++)
                    {
                        if(stockData[i].itemNO == item[k].itemNumber)
                        {
                            stockData[i].description = item[k].itemName;
                        }
                    }
                    stockData[i].SL = sl;
                    objList.Add(stockData[i]);
                    sl++;
                }                
            }
            ShowingReport = objList;
            IsprintEnable = true;
        }
        public void printInPrinter()
        {
            ShowingReport[0].wareHouseName = wareHouseName.wareHouseName;
            StaticPageForAllData.printStockBalance = ShowingReport;
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
                PrintCurrentStockBlance print = new PrintCurrentStockBlance();
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
        public void checkData()
        {
            if(itemNumberStart != null && itemNumberEnd != null && wareHouseName != null)
            {
                ShowReport();
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

       
        
        public void SortSL(int order)
        {
            if (order == 0)
            {
                List<StockData> spr = new List<StockData>();
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
                List<StockData> spr = new List<StockData>();
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

        public void SortStQuentity(int order)
        {
            if (order == 0)
            {
                List<StockData> spr = new List<StockData>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr = spr.OrderByDescending(o => o.currentBalance).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                iqOrder = 1;
            }
            else if (order == 1)
            {
                List<StockData> spr = new List<StockData>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr = spr.OrderBy(o => o.currentBalance).ToList();
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
                List<StockData> spr = new List<StockData>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr.Sort((x, y) => string.Compare(x.itemNO, y.itemNO));
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                itemOrder = 1;
            }
            else if (itemorder == 1)
            {
                List<StockData> spr = new List<StockData>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr.Sort((y, x) => string.Compare(x.itemNO, y.itemNO));
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                itemOrder = 0;
            }
        }
        

        public ObservableCollection<Warehouse> warehouse { get { return _warehouse; } set { _warehouse = value; OnPropertyChanged(nameof(warehouse)); } }
        public ObservableCollection<IssueVoucher> searchIssueVoucher { get { return _searchIssueVoucher; } set { _searchIssueVoucher = value; OnPropertyChanged(nameof(searchIssueVoucher)); } }
        public ObservableCollection<IssueVoucher> allIssueVoucher { get { return _allIssueVoucher; } set { _allIssueVoucher = value; OnPropertyChanged(nameof(allIssueVoucher)); } }
        public ObservableCollection<StockData> ShowingReport { get { return _ShowingReport; } set { _ShowingReport = value; OnPropertyChanged(nameof(ShowingReport)); } }
        public string conNameLbl { get { return _conNameLbl; } set { _conNameLbl = value; OnPropertyChanged(nameof(conNameLbl)); } }
        public Item itemNumberEnd { get { return _itemNumberEnd; } set { _itemNumberEnd = value; checkData(); OnPropertyChanged(nameof(itemNumberEnd)); } }
        public Item itemNumberStart { get { return _itemNumberStart; } set { _itemNumberStart = value; if (enbaleItem2 == true) { checkData(); } enbaleItem2 = true; OnPropertyChanged(nameof(itemNumberStart)); } }
        public ObservableCollection<Item> item { get { return _item; } set { _item = value; OnPropertyChanged(nameof(item)); } }
        public string woLbl { get { return _woLbl; } set { _woLbl = value; OnPropertyChanged(nameof(woLbl)); } }


        public bool IsprintEnable { get { return _IsprintEnable; } set { _IsprintEnable = value; OnPropertyChanged(nameof(IsprintEnable)); } }
        public bool enbaleItem { get { return _enbaleItem; } set { _enbaleItem = value; OnPropertyChanged(nameof(enbaleItem)); } }
        public bool enbaleItem2 { get { return _enbaleItem2; } set { _enbaleItem2 = value; OnPropertyChanged(nameof(enbaleItem2)); } }

        public string Error { get { return _Error; } set { _Error = value; OnPropertyChanged(nameof(Error)); } }
        
        public string ErrorColor { get { return _ErrorColor; } set { _ErrorColor = value; OnPropertyChanged(nameof(ErrorColor)); } }

        public Warehouse wareHouseName
        {
            get { return _wareHouseName; }
            set
            {
                _wareHouseName = value;
                enbaleItem = true;
                enbaleItem2 = true;
                checkData();
                
                OnPropertyChanged(nameof(wareHouseName));
            }
        }
    }
    public class UpdaterForStockReport : ICommand
    {
        #region ICommand Members  
        public static string txtbx;
        private StockBalanceReportViewModel viewModel;
        public UpdaterForStockReport(StockBalanceReportViewModel view)
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
            else if (parameter.ToString() == "sb")
            {
                viewModel.SortStQuentity(viewModel.iqOrder);
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
