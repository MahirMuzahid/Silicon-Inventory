using Silicon_Inventory.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicon_Inventory.ViewModel
{
    public class AllListViewModel : BaseViewModel
    {
        ObservableCollection<Contructor> _ConList = new ObservableCollection<Contructor>();
        ObservableCollection<WorkOrder> _woList = new ObservableCollection<WorkOrder>();
        ObservableCollection<Supplier> _supList = new ObservableCollection<Supplier>();
        ObservableCollection<Warehouse> _storeList = new ObservableCollection<Warehouse>();

        public AllListViewModel()
        {
            ConList = StaticPageForAllData.Contructor;
            woList = StaticPageForAllData.WorkOrder;
            supList = StaticPageForAllData.Supplier;
            storeList = StaticPageForAllData.WareHouse;
        }

        public ObservableCollection<Contructor> ConList { get { return _ConList; } set { _ConList = value; OnPropertyChanged(nameof(ConList)); } }
        public ObservableCollection<WorkOrder> woList { get { return _woList; } set { _woList = value; OnPropertyChanged(nameof(woList)); } }
        public ObservableCollection<Supplier> supList { get { return _supList; } set { _supList = value; OnPropertyChanged(nameof(supList)); } }
        public ObservableCollection<Warehouse> storeList { get { return _storeList; } set { _storeList = value; OnPropertyChanged(nameof(storeList)); } }
    }
}
