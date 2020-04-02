using SellerCodeManagerWPF.Controllers;
using SellerCodeManagerWPF.Models;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SellerCodeManagerWPF.ViewModels
{
    internal class CreateSellerViewModel : ViewModelBase
    {
        private string name;
        private string phoneNumber;
        private string identityNumber;
        private double newUserRate;

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                name = value;
                BuildCodeCommand.OnCanExecutedChanged(this, null);
            }
        }

        public double CurrentUserRate { get; private set; }

        public double NewUserRate
        {
            get => newUserRate * 100;
            set
            {
                newUserRate = Math.Round(value / 100, 2);
                OnPropertyChanged(nameof(NewUserRateString));
            }
        }

        public string NewUserRateString => Math.Round(NewUserRate, 2) + "%";

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                BuildCodeCommand.OnCanExecutedChanged(this, null);
            }
        }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityNumber
        {
            get => identityNumber;
            set
            {
                identityNumber = value;
                BuildCodeCommand.OnCanExecutedChanged(this, null);
            }
        }

        /// <summary>
        /// 指示当前用户是否拥有QRCodeSellerMA Permission Code
        /// </summary>
        public bool IsMA { get; set; } = false;

        /// <summary>
        /// 构建太阳码Command
        /// </summary>
        public DependencyCommand BuildCodeCommand { get; set; }
        private async void BulidCode(object parameter)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "请选择导出的太阳码保存路径",
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                int sellerID = await QRCodeClient.CreateSeller(IdentityNumber, Name, PhoneNumber, "Basic", IsMA, newUserRate);
                string savePath = Path.Combine(folderBrowserDialog.SelectedPath, $"{Name}-{PhoneNumber}-{Guid.NewGuid()}.jpg");
                await QRCodeClient.GetQRCode(sellerID, savePath);
                //res.Save(savePath);
                //res.Dispose();
                ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe")
                {
                    Arguments = "/e,/select," + savePath
                };
                Process.Start(psi);
            }
            folderBrowserDialog?.Dispose();
        }
        private bool CanBulidCode(object parameter)
        {
            return !(string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(PhoneNumber) || string.IsNullOrWhiteSpace(IdentityNumber));
        }

        public CreateSellerViewModel()
        {
            BuildCodeCommand = new DependencyCommand(BulidCode, CanBulidCode);
            newUserRate = QRCodeClient.GetCurrentUserSellerRate();
            CurrentUserRate = NewUserRate;
        }
    }
}
