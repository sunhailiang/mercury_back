using SellerCodeManagerWPF.Controllers;
using SellerCodeManagerWPF.Models;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace SellerCodeManagerWPF.ViewModels
{
    internal class SellerListViewModel : ViewModelBase
    {
        public QRCodeSeller[] Sellers { get; set; }

        public QRCodeSeller CurrentSelected { get; set; }

        public DependencyCommand DisableCommand { get; set; }
        private void Disable(object parameter)
        {
            CurrentSelected.IsEnable = false;
            QRCodeClient.UpdateSellerInformation(CurrentSelected);
            Sellers = QRCodeClient.GetSellerList();
            OnPropertyChanged(nameof(Sellers));
        }

        public DependencyCommand RegetCommand { get; set; }
        private async void GetQRCode(object parameter)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "请选择导出的太阳码保存路径",
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string savePath = Path.Combine(folderBrowserDialog.SelectedPath, $"{CurrentSelected.Name}-{CurrentSelected.PhoneNumber}-{CurrentSelected.SellerID}.jpg");
                await QRCodeClient.GetQRCode(CurrentSelected.SellerID, savePath);
                ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe")
                {
                    Arguments = "/e,/select," + savePath
                };
                Process.Start(psi);
            }
            folderBrowserDialog.Dispose();
        }

        public DependencyCommand GetOrdersCommand { get; set; }
        private void GetOrders(object parameter)
        {
            if (parameter is MainWindow mainWindow)
            {
                try
                {
                    mainWindow.OrderListView.RecommendId = CurrentSelected?.SellerID;
                    mainWindow.OrderListView.GetOrderCommand.Execute(this);
                    mainWindow.NavigateToOrderListView();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "查询失败", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        public DependencyCommand RefreshCommand { get; set; }
        private void Refresh(object parameter)
        {
            if (JupiterClient.IsMA)
            {
                Sellers = QRCodeClient.GetSellerList();
                OnPropertyChanged(nameof(Sellers));
            }
            else
            {
                System.Windows.MessageBox.Show("您不是管理员，不能查看其他分销员的信息", "查询失败", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public SellerListViewModel()
        {
            DisableCommand = new DependencyCommand(Disable, DependencyCommand.AlwaysCan);
            RegetCommand = new DependencyCommand(GetQRCode, DependencyCommand.AlwaysCan);
            GetOrdersCommand = new DependencyCommand(GetOrders, DependencyCommand.AlwaysCan);
            RefreshCommand = new DependencyCommand(Refresh, DependencyCommand.AlwaysCan);

            if (JupiterClient.IsMA)
            {
                Sellers = QRCodeClient.GetSellerList();
            }
        }
    }
}