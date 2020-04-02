using SellerCodeManagerWPF.Controllers;
using SellerCodeManagerWPF.Views;
using System.Windows;
using System.Windows.Controls;

namespace SellerCodeManagerWPF.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        private readonly Window currentWindow;

        public LoginViewModel()
        {
            SignInCommand = new DependencyCommand(SignIn, DependencyCommand.AlwaysCan);
            currentWindow = new LoginView
            {
                DataContext = this
            };
            currentWindow.Show();
        }

        public string UserName { get; set; }

        public DependencyCommand SignInCommand { get; set; }

        private async void SignIn(object parameter)
        {
            PasswordBox password = parameter as PasswordBox;
            if (string.IsNullOrWhiteSpace(password.Password) || string.IsNullOrWhiteSpace(UserName))
            {
                MessageBox.Show("用户名或密码为空", "输入错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            JupiterClient jupiterClient = new JupiterClient();
            var result = await jupiterClient.SignIn(UserName, password.Password);
            if (result.Code == 200 && !string.IsNullOrWhiteSpace(result.Data))
            {
                if (!JupiterClient.IsUser)
                {
                    MessageBox.Show("您没有获得太阳码分销系统的使用许可", "未开通该服务", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }
                else
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    currentWindow.Close();
                }
            }
            else
            {
                MessageBox.Show(result.Message, "登录失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}