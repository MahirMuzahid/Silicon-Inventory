using Silicon_Inventory.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Silicon_Inventory.View
{
    /// <summary>
    /// Interaction logic for PrintPeriodicRecieptReport.xaml
    /// </summary>
    public partial class PrintPeriodicRecieptReport : UserControl
    {
        public PrintPeriodicRecieptReport()
        {
            InitializeComponent();
            int ticket = StaticPageForAllData.printNumber;
            if (StaticPageForAllData.printPeriodicRecieptReport != null)
            {
                storeName.Content = "Store Name: " + StaticPageForAllData.printPeriodicRecieptReport[0].warehouse;
                reportDate.Content = "Reporting Date: " + DateTime.Now.ToString("dd/M/yyyy");
                dateRange.Content ="Date Range: " +StaticPageForAllData.printPeriodicRecieptReport[0].startDate + " To " + StaticPageForAllData.printPeriodicRecieptReport[0].endDate;
                placeLineinGrid();
            }
        }
        public void placeLineinGrid()
        {
            int printLimit = 0;
            int printStart = 0;
            pageno.Content = "Page: " + StaticPageForAllData.PrintedpageNumber;
            printLimit = StaticPageForAllData.PrintedpageNumber * 30;
            printStart = printLimit - 30;
            ObservableCollection<PeriodicRecieptReportModel> showThisPage = new ObservableCollection<PeriodicRecieptReportModel>();

            int prvlineLength = 21;
            for (int i = printStart; i < StaticPageForAllData.printPeriodicRecieptReport.Count; i++)
            {
                showThisPage.Add(StaticPageForAllData.printPeriodicRecieptReport[i]);
               
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
                il5.Y2 = prvlineLength;
                il6.Y2 = prvlineLength;
                if (i == printLimit - 1)
                {
                    break;
                }

            }
            ItemList.ItemsSource = showThisPage;
            StaticPageForAllData.PrintedpageNumber++;
        }
    }
}
