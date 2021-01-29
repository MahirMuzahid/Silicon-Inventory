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
using System.Windows.Shapes;

namespace Silicon_Inventory.View
{
    /// <summary>
    /// Interaction logic for MeterialReturnView.xaml
    /// </summary>
    public partial class MeterialReturnView : UserControl
    {
        public MeterialReturnView()
        {
            InitializeComponent();
            DataContext = new MeterialReturnViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
