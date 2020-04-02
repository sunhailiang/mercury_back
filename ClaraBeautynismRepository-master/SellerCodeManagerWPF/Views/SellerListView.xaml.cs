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
    /// SellerListView.xaml 的交互逻辑
    /// </summary>
    public partial class SellerListView : UserControl
    {
        public SellerListView()
        {
            InitializeComponent();
        }

        private void ListBox_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MainContainer.SelectedItem == null)
            {
                e.Handled = true;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var source = e.Source as MenuItem;
            source.CommandParameter = Window.GetWindow(this);
        }
    }
}
