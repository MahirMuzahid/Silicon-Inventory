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
    /// Interaction logic for MeterialReceiptView.xaml
    /// </summary>
    public partial class MeterialReceiptView : UserControl
    {
        public MeterialReceiptView()
        {
            InitializeComponent();
            DataContext = new MeterialReceiptViewModel();
            this.KeyDown += new KeyEventHandler(MeterialReceiptView_KeyDown);

        }
        void MeterialReceiptView_KeyDown(object sender, KeyEventArgs e)
        {
            if (click == 1)
            {
                cmbo7.Focus();
                click = 0;
            }
            if (SaveItemBtn.IsFocused)
            {
                click++;
            }
            
            
        }
        private void lvUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cmbo2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbo3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbo4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbo5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbo6_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbo7_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        

        private void cmbo7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                amountTxbx.Focus();
            }
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SaveItemBtn.Focus();
            }
        }
        int click = 0;
        private void SaveItemBtn_KeyDown(object sender, KeyEventArgs e)
        {          
           
        }
    }
}
