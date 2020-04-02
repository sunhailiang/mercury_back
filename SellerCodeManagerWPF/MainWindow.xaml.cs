using SellerCodeManagerWPF.Controllers;
using SellerCodeManagerWPF.ViewModels;
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

namespace SellerCodeManagerWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        internal OrderListViewModel OrderListView { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            OrderListView = new OrderListViewModel();
            DataContext = new { CreateSeller = new CreateSellerViewModel(), SellerList = new SellerListViewModel(), OrderList = OrderListView, UserInformation = new UserInformationViewModel() };
            if (!(JupiterClient.IsMA || JupiterClient.IsCA))
            {
                Create.Visibility = Visibility.Collapsed;
                List.Visibility = Visibility.Collapsed;
                OrderList.IsSelected = true;
            }
        }

        internal void NavigateToOrderListView()
        {
            OrderList.IsSelected = true;
        }
    }
}
