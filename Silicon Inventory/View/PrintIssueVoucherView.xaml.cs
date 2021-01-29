using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Silicon_Inventory.ViewModel;
using Silicon_Inventory.Model;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace Silicon_Inventory.View
{
    /// <summary>
    /// Interaction logic for PrintIssueVoucherView.xaml
    /// </summary>
    public partial class PrintIssueVoucherView : UserControl
    {
        public PrintIssueVoucherView()
        {
            InitializeComponent();
            int ticket = StaticPageForAllData.printNumber;
            
            DataContext = new PrintViewModel();
            if(ticket == 1)
            {
                if (StaticPageForAllData.printIssueVoucher != null)
                {                   
                    date.Content = "Date : " + StaticPageForAllData.printIssueVoucher[0].IssueVoucherDate;
                    contructor.Content = "Issued To : " + StaticPageForAllData.printIssueVoucher[0].contructorName;
                    warehouse.Content = "Issued From : " + StaticPageForAllData.printIssueVoucher[0].wareHouseName;
                    wono.Content = "Work Order No. : " + StaticPageForAllData.printIssueVoucher[0].workOrderNo;
                    issvoucherID.Content = "Ticket No. : " + StaticPageForAllData.printIssueVoucher[0].IssueVoucherID;
                    condition.Content = "Condition : " + StaticPageForAllData.printIssueVoucher[0].Condition;
                    req.Content = "Requisition No. : " + StaticPageForAllData.printIssueVoucher[0].requisition;
                    whname.Content = "Store Name: " + StaticPageForAllData.printIssueVoucher[0].wareHouseName;
                    ex1.Content = "";
                    ex2.Content = "";
                    ex3.Content = "";
                    ex4.Content = "";
                    voucherName.Content = "Meterial Charge Ticket";
                    placeLineinGrid(1);
                }
            }
            if (ticket == 2)
            {
                if (StaticPageForAllData.printRecieptVoucher != null)
                {
                   
                    date.Content = "Date : " + StaticPageForAllData.printRecieptVoucher[0].RRDate;
                    contructor.Content = "Supplier : " + StaticPageForAllData.printRecieptVoucher[0].supplier;
                    warehouse.Content = "Contructor : " + StaticPageForAllData.printRecieptVoucher[0].contructor;
                    wono.Content = "Delevered To : " + StaticPageForAllData.printRecieptVoucher[0].deliveredTo;
                    issvoucherID.Content = "RR No. : " + StaticPageForAllData.printRecieptVoucher[0].RRVoucherID;
                    condition.Content =  "Condition " + StaticPageForAllData.printRecieptVoucher[0].condition;
                    req.Content = "Allocation No. : " + StaticPageForAllData.printRecieptVoucher[0].allocNumber;
                    whname.Content = "Store Name: : " + StaticPageForAllData.printRecieptVoucher[0].warehouse;
                    ex1.Content = "Checked By: " + StaticPageForAllData.printRecieptVoucher[0].checkedby; ;
                    ex2.Content = "Allocation Date: " + StaticPageForAllData.printRecieptVoucher[0].alolocDate; ;
                    ex3.Content = "Challan: " + StaticPageForAllData.printRecieptVoucher[0].challan; ;
                    ex4.Content = "Work Order No: " + StaticPageForAllData.printRecieptVoucher[0].wordOrderNo; ;
                    voucherName.Content = "Recieving Report";
                    placeLineinGrid(2);
                }
            }
            if (ticket == 3)
            {
                if (StaticPageForAllData.printReturnVoucher != null)
                {

                    date.Content = "Date : " + StaticPageForAllData.AllReturnVoucher[0].returnDate;
                    contructor.Content = "Contructor : " + StaticPageForAllData.AllReturnVoucher[0].contructor;
                    warehouse.Content = "Work Order No. : " + StaticPageForAllData.AllReturnVoucher[0].workOrderNo;
                    wono.Content = "Cause Of Return : " + StaticPageForAllData.AllReturnVoucher[0].cause;
                    issvoucherID.Content = "RR No. : " + StaticPageForAllData.AllReturnVoucher[0].retID;
                    condition.Content = "Condition " + StaticPageForAllData.AllReturnVoucher[0].condition;
                    req.Content = "Return Type : " + StaticPageForAllData.AllReturnVoucher[0].ret_type;
                    whname.Content = "Store Name: : " + StaticPageForAllData.printReturnVoucher[0].ret_warehouse;
                    ex1.Content = "";
                    ex2.Content = "";
                    ex3.Content = "";
                    ex4.Content = "";
                    voucherName.Content = "Meterial Return Report";
                    placeLineinGrid(3);
                }
            }


        }

        public void placeLineinGrid(int tn)
        {
            int printLimit = 0;
            int printStart = 0;
            pageno.Content = "Page: " + StaticPageForAllData.PrintedpageNumber;
            printLimit = StaticPageForAllData.PrintedpageNumber * 30;
            printStart = printLimit - 30;
            if (tn == 1)
            {
                ObservableCollection<IssueVoucher> showThisPage = new ObservableCollection<IssueVoucher>();
                
                int prvlineLength = 21;
                for (int i = printStart; i < StaticPageForAllData.printIssueVoucher.Count; i++)
                {
                    showThisPage.Add(StaticPageForAllData.printIssueVoucher[i]);
                    if (i == printLimit)
                    {
                        break;
                    }
                    prvlineLength = prvlineLength + 21;

                    Line line = new Line();
                    line.X1 = 0;
                    line.Y1 = prvlineLength;
                    line.X2 = 7000;
                    line.Y2 = prvlineLength;
                    line.Stroke = Brushes.Black;
                    line.StrokeThickness = 1;
                    listGrid.Children.Add(line);
                    ll.Y2 = prvlineLength;
                    rl.Y2 = prvlineLength;

                    il1.Y2 = prvlineLength;
                    il2.Y2 = prvlineLength;
                    il3.Y2 = prvlineLength;
                    il4.Y2 = prvlineLength;

                }
                ItemList.ItemsSource = showThisPage; 
                StaticPageForAllData.PrintedpageNumber++;
            }
            if (tn == 2)
            {
                int prvlineLength = 21;
                ObservableCollection<ReceiptVoucher> showThisPage = new ObservableCollection<ReceiptVoucher>();
                for (int i = 0; i < StaticPageForAllData.printRecieptVoucher.Count; i++)
                {
                    showThisPage.Add(StaticPageForAllData.printRecieptVoucher[i]);
                    showThisPage[i].IssueQuantity = StaticPageForAllData.printRecieptVoucher[i].RCVQuantity;
                    if (i == printLimit)
                    {
                        break;
                    }
                    prvlineLength = prvlineLength + 21;

                    Line line = new Line();
                    line.X1 = 0;
                    line.Y1 = prvlineLength;
                    line.X2 = 7000;
                    line.Y2 = prvlineLength;
                    line.Stroke = Brushes.Black;
                    line.StrokeThickness = 1;
                    listGrid.Children.Add(line);
                    ll.Y2 = prvlineLength;
                    rl.Y2 = prvlineLength;

                    il1.Y2 = prvlineLength;
                    il2.Y2 = prvlineLength;
                    il3.Y2 = prvlineLength;
                    il4.Y2 = prvlineLength;

                }
                ItemList.ItemsSource = showThisPage;
                StaticPageForAllData.PrintedpageNumber++;
            }
            if (tn == 3)
            {
                int prvlineLength = 21;
                ObservableCollection<ReturnVoucher> showThisPage = new ObservableCollection<ReturnVoucher>();
                for (int i = 0; i < StaticPageForAllData.printReturnVoucher.Count; i++)
                {
                    showThisPage.Add(StaticPageForAllData.printReturnVoucher[i]);
                    showThisPage[i].IssueQuantity = StaticPageForAllData.printReturnVoucher[i].quentity;
                    showThisPage[i].itemNumber = StaticPageForAllData.printReturnVoucher[i].ItemNo;
                    showThisPage[i].description = StaticPageForAllData.printReturnVoucher[i].Description;
                   
                    prvlineLength = prvlineLength + 21;

                    Line line = new Line();
                    line.X1 = 0;
                    line.Y1 = prvlineLength;
                    line.X2 = 7000;
                    line.Y2 = prvlineLength;
                    line.Stroke = Brushes.Black;
                    line.StrokeThickness = 1;
                    listGrid.Children.Add(line);
                    ll.Y2 = prvlineLength;
                    rl.Y2 = prvlineLength;

                    il1.Y2 = prvlineLength;
                    il2.Y2 = prvlineLength;
                    il3.Y2 = prvlineLength;
                    il4.Y2 = prvlineLength;
                    if (i == printLimit - 1)
                    {
                        break;
                    }

                }
                ItemList.ItemsSource = showThisPage;
                StaticPageForAllData.PrintedpageNumber++;
            }



        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
