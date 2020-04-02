using SellerCodeManagerWPF.Controllers;
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

namespace SellerCodeManagerWPF.Views
{
    /// <summary>
    /// OrderListView.xaml 的交互逻辑
    /// </summary>
    public partial class OrderListView : UserControl
    {
        public OrderListView()
        {
            InitializeComponent();
            if (!(JupiterClient.IsCA || JupiterClient.IsMA))
            {
                IdTextBox.Visibility = Visibility.Collapsed;
            }
        }
    }
}
