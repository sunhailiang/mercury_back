using SellerCodeManagerWPF.Controllers;
using SellerCodeManagerWPF.Views;
using System.Windows;

namespace SellerCodeManagerWPF.ViewModels
{
    internal class ChangePasswordViewModel : ViewModelBase
    {
        public DependencyCommand ChangePasswordCommand { get; set; }

        private async void ChangePassword(object parameter)
        {
            var currentWindow = parameter as ChangePasswordDialog;
            JupiterClient jupiterClient = new JupiterClient();
            var x = await jupiterClient.ChangePassword(JupiterClient.Md5(currentWindow.OldPassword.Password), JupiterClient.Md5(currentWindow.NewPassword.Password));
            if (x.Code == 200)
            {
                MessageBox.Show("修改成功", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                currentWindow.Close();
            }
            else
            {
                MessageBox.Show(x.Message, "失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ChangePasswordViewModel()
        {
            ChangePasswordCommand = new DependencyCommand(ChangePassword, DependencyCommand.AlwaysCan);
        }
    }
}