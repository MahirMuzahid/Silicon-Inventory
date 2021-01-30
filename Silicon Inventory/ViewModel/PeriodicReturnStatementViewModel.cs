using Silicon_Inventory.Model;
using Silicon_Inventory.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace Silicon_Inventory.ViewModel
{
    public class PeriodicReturnStatementViewModel : BaseViewModel
    {
        public ObservableCollection<Warehouse> _warehouse { get; set; }
        public ObservableCollection<ReturnVoucher> _AllRetrunVoucher { get; set; }
        public ObservableCollection<ReturnVoucher> _ShowingReport { get; set; }
        ObservableCollection<StockData> stockData = new ObservableCollection<StockData>();

        public string _type { get; set; }
        public string _condition { get; set; }

        public Warehouse _wareHouseName { get; set; }
        public string _FdateTxbx { get; set; }
        public string _FmonthTxbx { get; set; }
        public string _FyearTxbx { get; set; }
        public string _TdateTxbx { get; set; }
        public string _TmonthTxbx { get; set; }
        public string _TyearTxbx { get; set; }
        public bool _IsprintEnable { get; set; }
        DateTime From = new DateTime();
        DateTime To = new DateTime();

        public string _Error { get; set; }
        public string _ErrorColor { get; set; }

        public string red = "#E53E3E", green = "#18CB51 ", black = "#000000", blue = "#0C84F6";

        public int slOrder = 0, inOrder = 0, rq = 0, con = 0;
        public ICommand Updater { get; set; }
        public PeriodicReturnStatementViewModel()
        {
            warehouse = StaticPageForAllData.WareHouse;
            AllRetrunVoucher = StaticPageForAllData.AllReturnVoucher;
            stockData = StaticPageForAllData.StockData;
            Updater = new UpdaterForReturnReport(this);
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

                if (wareHouseName == null)
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

            if (error == 1)
            {
                IsprintEnable = false;
                sendMsg("Enter Valid Date.", 1);
            }
            else if (error == 2)
            {
                IsprintEnable = false;
                sendMsg("Fill up all data/Enter Valid Data", 0);
            }
            else if (error == 4)
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
                IsprintEnable = true;
                ShowList();
            }

        }
        public void ShowList()
        {
            ObservableCollection<ReturnVoucher> objList = new ObservableCollection<ReturnVoucher>();
            filterTypeANdCondition();
            int sl = 1;
            for(int i  = 0; i < AllRetrunVoucher.Count; i++)
            {
                ReturnVoucher obj = new ReturnVoucher();
                DateTime thisVoucherDate = DateTime.ParseExact(AllRetrunVoucher[i].returnDate, "dd/M/yyyy", null);
                if ((thisVoucherDate >= From && thisVoucherDate <= To) && wareHouseName.wareHouseName == AllRetrunVoucher[i].ret_warehouse && 
                    AllRetrunVoucher[i].condition == conditionNew && AllRetrunVoucher[i].ret_type == typenew && AllRetrunVoucher[i].isPrinted == 1)
                {
                    obj.SL = sl;
                    obj.retID = AllRetrunVoucher[i].retID;
                    obj.returnDate = AllRetrunVoucher[i].returnDate;
                    obj.ItemNo = AllRetrunVoucher[i].ItemNo;
                    obj.Description = AllRetrunVoucher[i].Description;
                    obj.quentity = AllRetrunVoucher[i].quentity;
                    obj.contructor = AllRetrunVoucher[i].contructor;
                    obj.ret_type = AllRetrunVoucher[i].ret_type;
                    obj.condition = AllRetrunVoucher[i].condition;
                    for (int j = 0; j < stockData.Count; j++)
                    {
                        if (AllRetrunVoucher[i].ItemNo == stockData[j].itemNO)
                        {
                            obj.unit = stockData[j].unit;
                            break;
                        }
                    }
                    sl++;

                    objList.Add(obj);
                }
            }
            ShowingReport = objList;
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
        string conditionNew = "";
        string typenew = "";
        public void filterTypeANdCondition()
        {
            List<char> con = new List<char>();
            string conNew = "";
            string tynew = "";
            for (int i = condition.Length - 1; i > 0; i--)
            {
                if (condition[i] != ' ')
                {
                    con.Add(condition[i]);
                }
                else
                {
                    break;
                }
            }
            for (int j = con.Count - 1; j >= 0; j--)
            {
                conNew = conNew + con[j];
            }
            List<char> t = new List<char>();
           

            for (int i = type.Length - 1; i > 0; i--)
            {
                if (type[i] != ' ')
                {
                    t.Add(type[i]);
                }
                else
                {
                    break;
                }
            }
            for (int j = t.Count - 1; j >= 0; j--)
            {
                tynew = tynew + t[j];
            }
            conditionNew = conNew;
            typenew = tynew;


        }


        public void SortSL(int order)
        {
            if (order == 0)
            {
                List<ReturnVoucher> spr = new List<ReturnVoucher>();
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
                List<ReturnVoucher> spr = new List<ReturnVoucher>();
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

        public void SortRetQuentity(int order)
        {
            if (order == 0)
            {
                List<ReturnVoucher> spr = new List<ReturnVoucher>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr = spr.OrderByDescending(o => o.quentity).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                rq = 1;
            }
            else if (order == 1)
            {
                List<ReturnVoucher> spr = new List<ReturnVoucher>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr = spr.OrderBy(o => o.quentity).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                rq = 0;
            }
        }

        public void SortItemName(int itemorder)
        {
            if (itemorder == 0)
            {
                List<ReturnVoucher> spr = new List<ReturnVoucher>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr.Sort((x, y) => string.Compare(x.ItemNo, y.ItemNo));
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                inOrder = 1;
            }
            else if (itemorder == 1)
            {
                List<ReturnVoucher> spr = new List<ReturnVoucher>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr.Sort((y, x) => string.Compare(x.ItemNo, y.ItemNo));
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                inOrder = 0;
            }
        }
        public void SortContructorName(int itemorder)
        {
            if (itemorder == 0)
            {
                List<ReturnVoucher> spr = new List<ReturnVoucher>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr.Sort((x, y) => string.Compare(x.contructor, y.contructor));
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                con = 1;
            }
            else if (itemorder == 1)
            {
                List<ReturnVoucher> spr = new List<ReturnVoucher>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr.Sort((y, x) => string.Compare(x.contructor, y.contructor));
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                con = 0;
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
            ShowingReport[0].startDate = "" + fd + "/" + fm + "/" + fy;
            ShowingReport[0].endDate = "" + td + "/" + tm + "/" + ty;
            for (int i = 0; i < ShowingReport.Count; i++)
            {
                ShowingReport[i].SL = i + 1;
            }


            StaticPageForAllData.printPerioDicReturn = ShowingReport;
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
                PeriodicReturnPrint print = new PeriodicReturnPrint();
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

        public Warehouse wareHouseName { get { return _wareHouseName; } set { _wareHouseName = value; checkDate(); OnPropertyChanged(nameof(wareHouseName)); } }
        public ObservableCollection<Warehouse> warehouse { get { return _warehouse; } set { _warehouse = value; OnPropertyChanged(nameof(warehouse)); } }
        public ObservableCollection<ReturnVoucher> ShowingReport { get { return _ShowingReport; } set { _ShowingReport = value; OnPropertyChanged(nameof(ShowingReport)); } }
        public ObservableCollection<ReturnVoucher> AllRetrunVoucher { get { return _AllRetrunVoucher; } set { _AllRetrunVoucher = value; OnPropertyChanged(nameof(AllRetrunVoucher)); } }
        public string FdateTxbx { get { return _FdateTxbx; } set { _FdateTxbx = value; checkDate(); OnPropertyChanged(nameof(FdateTxbx)); } }
        public string FmonthTxbx { get { return _FmonthTxbx; } set { _FmonthTxbx = value; checkDate(); OnPropertyChanged(nameof(FmonthTxbx)); } }
        public string FyearTxbx { get { return _FyearTxbx; } set { _FyearTxbx = value; checkDate(); OnPropertyChanged(nameof(FyearTxbx)); } }

        public string TdateTxbx { get { return _TdateTxbx; } set { _TdateTxbx = value; checkDate(); OnPropertyChanged(nameof(TdateTxbx)); } }
        public string TmonthTxbx { get { return _TmonthTxbx; } set { _TmonthTxbx = value; checkDate(); OnPropertyChanged(nameof(TmonthTxbx)); } }
        public string TyearTxbx { get { return _TyearTxbx; } set { _TyearTxbx = value; checkDate(); OnPropertyChanged(nameof(TyearTxbx)); } }
        public bool IsprintEnable { get { return _IsprintEnable; } set { _IsprintEnable = value; OnPropertyChanged(nameof(IsprintEnable)); } }

        public string Error { get { return _Error; } set { _Error = value; OnPropertyChanged(nameof(Error)); } }
        public string ErrorColor { get { return _ErrorColor; } set { _ErrorColor = value; OnPropertyChanged(nameof(ErrorColor)); } }
        public string condition { get { return _condition; } set { _condition = value; checkDate(); OnPropertyChanged(nameof(condition)); } }
        public string type { get { return _type; } set { _type = value; checkDate(); OnPropertyChanged(nameof(type)); } }



    }
    class UpdaterForReturnReport : ICommand
    {
        #region ICommand Members  
        public static string txtbx;
        private PeriodicReturnStatementViewModel viewModel;
        public UpdaterForReturnReport(PeriodicReturnStatementViewModel view)
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
            else if (parameter.ToString() == "rq")
            {
                viewModel.SortRetQuentity(viewModel.rq);
            }
            else if (parameter.ToString() == "in")
            {
                viewModel.SortItemName(viewModel.inOrder);
            }
            else if (parameter.ToString() == "con")
            {
                viewModel.SortContructorName(viewModel.con);
            }
            else if (parameter.ToString() == "print")
            {
                viewModel.printInPrinter();
            }

        }



        #endregion
    }
}
