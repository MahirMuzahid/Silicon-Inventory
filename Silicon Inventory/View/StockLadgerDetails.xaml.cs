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
using System.Windows.Shapes;
using Silicon_Inventory.ViewModel;

namespace Silicon_Inventory.View
{
    /// <summary>
    /// Interaction logic for StockLadgerDetails.xaml
    /// </summary>
    public partial class StockLadgerDetails : UserControl
    {
        public StockLadgerDetails()
        {
            InitializeComponent();
            DataContext = new StockLadgerViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog print = new PrintDialog();
            StockLadgerDetails uc = new StockLadgerDetails();
            uc.DataContext = new StockLadgerViewModel();
            if (print.ShowDialog() == true)
            {
                print.PrintVisual(uc, "invoice");
            }
        }
    }
}
