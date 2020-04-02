using SellerCodeManagerWPF.Controllers;
using SellerCodeManagerWPF.Models;
using SellerCodeManagerWPF.Views;
using System;

namespace SellerCodeManagerWPF.ViewModels
{
    internal class UserInformationViewModel : ViewModelBase
    {
        private static readonly QRCodeSeller currentSeller;

        static UserInformationViewModel()
        {
            currentSeller = QRCodeClient.GetCurrentUserSellerInformation();
        }

        public string Name => currentSeller.Name;

        public string Role => JupiterClient.IsCA ? "超级管理员" : (JupiterClient.IsMA ? "管理员" : "标准用户");

        public string Rate { get; private set; }

        public string TotalCommission { get; private set; }

        public DependencyCommand ChangePasswordCommand { get; set; }
        public void ChangePassword(object parameter)
        {
            var dig = new ChangePasswordDialog
            {
                DataContext = new ChangePasswordViewModel()
            };
            dig.ShowDialog();
        }

        public Uri Avatar => string.IsNullOrWhiteSpace(JupiterClient.UserInformation.Avator) ? new Uri("https://jupiter.clarabeautynism.com/defaultAvatar.png") : new Uri(JupiterClient.UserInformation.Avator);

        public UserInformationViewModel()
        {
            ChangePasswordCommand = new DependencyCommand(ChangePassword, DependencyCommand.AlwaysCan);
            Rate = "当前佣金比例：" + (QRCodeClient.GetCurrentUserSellerRate() * 100).ToString() + "%";
            TotalCommission = "当前累计应得佣金：" + (QRCodeClient.GetTotalCommission()).ToString();
        }
    }
}
