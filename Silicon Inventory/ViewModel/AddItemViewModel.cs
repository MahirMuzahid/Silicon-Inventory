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
    class AddItemViewModel : BaseViewModel
    {
        ObservableCollection<Item> _itemList { get; set; }
        ObservableCollection<Item> _ShowingList { get; set; }
        public ICommand Updater { get; set; }
        ObservableCollection<StockData> stockData { get; set; }
        Item _itemName { get; set; }
        ObservableCollection<Item> allItemList { get; set; }
        
        public AddItemViewModel ()
        {
            Updater = new UpdaterForAdditem(this);
            stockData = StaticPageForAllData.StockData;
            itemList = StaticPageForAllData.Items;
            PlaceAllItem();


        }
        public int slOrder;
        public void SortSL(int order)
        {
            if (order == 0)
            {
                List<Item> spr = new List<Item>();
                for (int i = 0; i < ShowingList.Count; i++)
                {
                    spr.Add(ShowingList[i]);
                }
                spr = spr.OrderByDescending(o => o.SL).ToList();
                ShowingList.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingList.Add(spr[i]);
                }
                slOrder = 1;
            }
            else if (order == 1)
            {
                List<Item> spr = new List<Item>();
                for (int i = 0; i < ShowingList.Count; i++)
                {
                    spr.Add(ShowingList[i]);
                }
                spr = spr.OrderBy(o => o.SL).ToList();
                ShowingList.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingList.Add(spr[i]);
                }
                slOrder = 0;
            }
        }
        public int itemOrder;
        public void SortItemName(int itemorder)
        {
            if (itemorder == 0)
            {
                List<Item> spr = new List<Item>();
                for (int i = 0; i < ShowingList.Count; i++)
                {
                    spr.Add(ShowingList[i]);
                }
                spr.Sort((x, y) => string.Compare(x.itemNumber, y.itemNumber));
                ShowingList.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingList.Add(spr[i]);
                }
                itemOrder = 1;
            }
            else if (itemorder == 1)
            {
                List<Item> spr = new List<Item>();
                for (int i = 0; i < ShowingList.Count; i++)
                {
                    spr.Add(ShowingList[i]);
                }
                spr.Sort((y, x) => string.Compare(x.itemNumber, y.itemNumber));
                ShowingList.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingList.Add(spr[i]);
                }
                itemOrder = 0;
            }
        }
        public void PlaceAllItem()
        {
            int sl = 1;
            ObservableCollection<Item> SList = new ObservableCollection<Item>();
            for (int i = 0; i < itemList.Count; i++)
            {
                SList.Add(itemList[i]);
                SList[i].SL = sl;
                for (int j = 0; j < stockData.Count; j++)
                {
                    if (SList[i].itemNumber == stockData[j].itemNO)
                    {
                        SList[i].unit = stockData[j].unit;
                    }
                }
                sl++;
            }
            ShowingList = SList;
        }
        public ObservableCollection<Item> ShowingList { get { return _ShowingList; } set { _ShowingList = value; OnPropertyChanged(nameof(ShowingList)); } }
        public ObservableCollection<Item> itemList { get { return _itemList; } set { _itemList = value; OnPropertyChanged(nameof(itemList)); } }
        public Item itemName { get { return _itemName; } set { _itemName = value;
                
                
                if(itemName == null)
                {
                    ShowingList.Clear();
                    PlaceAllItem();
                }
                else
                {
                    for(int i = 0; i < itemList.Count; i++)
                    {
                        if(itemName.itemNumber == itemList[i].itemNumber)
                        {
                            ShowingList.Clear();
                            ShowingList.Add(itemName);
                            ShowingList[0].SL = 1;
                        }
                    }
                }                           
                OnPropertyChanged(nameof(itemName)); } }
    }

    class UpdaterForAdditem : ICommand
    {
        #region ICommand Members  
        public static string txtbx;
        private AddItemViewModel viewModel;
        public UpdaterForAdditem(AddItemViewModel view)
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
            else if (parameter.ToString() == "ii")
            {
                viewModel.SortItemName(viewModel.itemOrder);
            }
            

        }



        #endregion
    }
}
