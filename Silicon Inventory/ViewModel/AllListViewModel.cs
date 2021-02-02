using Silicon_Inventory.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Silicon_Inventory.ViewModel
{
    public class AllListViewModel : BaseViewModel
    {
        ObservableCollection<Contructor> _ConList = new ObservableCollection<Contructor>();
        ObservableCollection<WorkOrder> _woList = new ObservableCollection<WorkOrder>();
        ObservableCollection<Supplier> _supList = new ObservableCollection<Supplier>();
        ObservableCollection<Warehouse> _storeList = new ObservableCollection<Warehouse>();

        public string _addWindowVisibility { get; set; }
        public string _lastVisibility { get; set; }
        public ICommand Updater { get; set; }

        public string _adName { get; set; }
        public string _nextID { get; set; }
        public string _txbx1 { get; set; }
        public string _txbx2 { get; set; }
        public string _txbx3 { get; set; }
        public string _lbl1 { get; set; }
        public string _lbl2 { get; set; }
        public string _lbl3 { get; set; }

        public bool _addEnabled { get; set; }

        public bool isWoAdd { set; get; }
        public AllListViewModel()
        {
            Updater = new UpdaterForAllList(this);
            ConList = StaticPageForAllData.Contructor;
            woList = StaticPageForAllData.WorkOrder;
            supList = StaticPageForAllData.Supplier;
            storeList = StaticPageForAllData.WareHouse;
            addWindowVisibility = "Hidden";
        }

        public void addContructor()
        {
            isWoAdd = false;
            addEnabled = false;
            adName = "Add Contructor";
            int max = 0;
            for(int i = 0;  i < ConList.Count; i++)
            {
                for(int j = i; j < ConList.Count; j++)
                {
                    if (max < ConList[j].contructorID)
                    {
                        max = ConList[j].contructorID;
                    }
                }
                
            }
            max += 1; 
            nextID = "Next Contructor ID: " + max;

            lbl1 = "Name";
            lbl2 = "Address";
            lbl3 = "Phone Number";
            lastVisibility = "Visible";
            txbx3 = "";
            txbx2 = "";
            txbx1 = "";
        }
        public void addWorkOrder()
        {
            isWoAdd = false;
            addEnabled = false;
            adName = "Add Work Order";
            int max = 0;
            for (int i = 0; i < woList.Count; i++)
            {
                for (int j = i; j < woList.Count; j++)
                {
                    if (max < woList[j].workOrderID)
                    {
                        max = woList[j].workOrderID;
                    }
                }

            }
            max += 1;
            nextID = "Next Work Order ID: " + max;

            lbl1 = "Work Order No";
            lbl2 = "Work Order Date";
            lbl3 = "Contructor Name";
            lastVisibility = "Visible";
            txbx3 = "";
            txbx2 = "";
            txbx1 = "";
        }
        public void addSupplier()
        {
            isWoAdd = false;
            addEnabled = false;
            adName = "Add Supplier";
            int max = 0;
            for (int i = 0; i < supList.Count; i++)
            {
                for (int j = i; j < supList.Count; j++)
                {
                    if (max < supList[j].supplierID)
                    {
                        max = supList[j].supplierID;
                    }
                }

            }
            max += 1;
            nextID = "Next Supplier ID: " + max;

            lbl1 = "Name";
            lbl2 = "Address";
            lbl3 = "Phone Number";
            lastVisibility = "Visible";
            txbx3 = "";
            txbx2 = "";
            txbx1 = "";
        }

        public void addStore()
        {
            isWoAdd = true;

            addEnabled = false;
            adName = "Add Store";
            int max = 0;
            for (int i = 0; i < storeList.Count; i++)
            {
                for (int j = i; j < storeList.Count; j++)
                {
                    if (max < storeList[j].wareHouseID)
                    {
                        max = storeList[j].wareHouseID;
                    }
                }

            }

            max += 1;
            nextID = "Next Store ID: " + max;
            lbl1 = "Ware House Name";
            lbl2 = "";
            lbl3 = "";
            txbx3 = "";
            txbx2 = "";
            txbx1 = "";
            lastVisibility = "Hidden";
        }


        public void checkEnabled()
        {           
            if (!isWoAdd)
            {
                if (txbx1 != null && txbx1 != "" && txbx2 != null && txbx2 != "" && txbx3 != null && txbx3 != "")
                {
                    addEnabled = true;
                }
                else
                {
                    addEnabled = false;
                }
            }
            else
            {
                if (txbx1 != null && txbx1 != "")
                {
                    addEnabled = true;
                }
                else
                {
                    addEnabled = false;
                }
            }
            
        }
        public ObservableCollection<Contructor> ConList { get { return _ConList; } set { _ConList = value; OnPropertyChanged(nameof(ConList)); } }
        public ObservableCollection<WorkOrder> woList { get { return _woList; } set { _woList = value; OnPropertyChanged(nameof(woList)); } }
        public ObservableCollection<Supplier> supList { get { return _supList; } set { _supList = value; OnPropertyChanged(nameof(supList)); } }
        public ObservableCollection<Warehouse> storeList { get { return _storeList; } set { _storeList = value; OnPropertyChanged(nameof(storeList)); } }

        public string adName { get { return _adName; } set { _adName = value; OnPropertyChanged(nameof(adName)); } }
        public string nextID { get { return _nextID; } set { _nextID = value; OnPropertyChanged(nameof(nextID)); } }
        public string txbx1 { get { return _txbx1; } set { _txbx1 = value; checkEnabled(); OnPropertyChanged(nameof(txbx1)); } }
        public string txbx2 { get { return _txbx2; } set { _txbx2 = value; checkEnabled(); OnPropertyChanged(nameof(txbx2)); } }
        public string txbx3 { get { return _txbx3; } set { _txbx3 = value; checkEnabled(); OnPropertyChanged(nameof(txbx3)); } }
        public string lbl1 { get { return _lbl1; } set { _lbl1 = value; OnPropertyChanged(nameof(lbl1)); } }
        public string lbl2 { get { return _lbl2; } set { _lbl2 = value; OnPropertyChanged(nameof(lbl2)); } }
        public string lbl3 { get { return _lbl3; } set { _lbl3 = value; OnPropertyChanged(nameof(lbl3)); } }

        public bool addEnabled { get { return _addEnabled; } set { _addEnabled = value; OnPropertyChanged(nameof(addEnabled)); } }


        public string addWindowVisibility { get { return _addWindowVisibility; } set { _addWindowVisibility = value; OnPropertyChanged(nameof(addWindowVisibility)); } }
        public string lastVisibility { get { return _lastVisibility; } set { _lastVisibility = value; OnPropertyChanged(nameof(lastVisibility)); } }
    }

    class UpdaterForAllList : ICommand
    {
        #region ICommand Members  
        public static string txtbx;
        private AllListViewModel viewModel;
        public UpdaterForAllList(AllListViewModel view)
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
            if(parameter.ToString() == "exit")
            {
                viewModel.addWindowVisibility = "Hidden";
            }
            else if (parameter.ToString() == "con")
            {
                viewModel.addWindowVisibility = "Visble";
                viewModel.addContructor();
            }
            else if (parameter.ToString() == "wo")
            {
                viewModel.addWindowVisibility = "Visble";
                viewModel.addWorkOrder();
            }
            else if (parameter.ToString() == "sup")
            {
                viewModel.addWindowVisibility = "Visble";
                viewModel.addSupplier();
            }
            else if (parameter.ToString() == "store")
            {
                viewModel.addWindowVisibility = "Visble";
                viewModel.addStore();
            }
        }



        #endregion
    }
}
