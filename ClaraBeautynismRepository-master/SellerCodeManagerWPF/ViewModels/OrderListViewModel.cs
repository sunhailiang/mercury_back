using SellerCodeManagerWPF.Controllers;
using SellerCodeManagerWPF.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SellerCodeManagerWPF.ViewModels
{
    internal class OrderListViewModel : ViewModelBase
    {
        private int? recommendId;

        public int? RecommendId
        {
            get => recommendId;
            set
            {
                OnPropertyChanged(nameof(OrderTargetText));
                recommendId = value;
            }
        }

        public string OrderTargetText { get; set; } = "查询我自己的订单";

        public DependencyCommand GetOrderCommand { get; set; }
        private async void GetOrder(object parameter)
        {
            OnPropertyChanged(nameof(RecommendId));
            try
            {
                var x = await Task.Run(() => QRCodeClient.GetOrderListBySellerID(RecommendId));
                var result = from order in x
                             orderby order.ConfirmTime descending
                             select order;
                Orders = result.ToArray();
                OnPropertyChanged(nameof(Orders));

            }
            catch (Exception)
            {
#warning 这里不应该用Exception筛选器
                MessageBox.Show($"您可能输入了错误的太阳码ID（这个不是手机号）", "失败了", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public DependencyCommand CalculateCommand { get; set; }
        private void Calculate(object parameter)
        {
            decimal result = 0;
            decimal resultCompleted = 0;
            if (Orders != null && Orders.Length > 0)
            {
                var money = from order in Orders
                            where order.OrderStatus == 101 || order.OrderStatus == 201 || order.OrderStatus == 301 || order.OrderStatus == 401 || order.OrderStatus == 402
                            select order.ActualPrice;
                foreach (var a in money.ToArray())
                {
                    result += a;
                }
                var moneyCompleted = from order in Orders
                                     where order.OrderStatus == 401 || order.OrderStatus == 402
                                     select order.ActualPrice;
                foreach (var a in moneyCompleted.ToArray())
                {
                    resultCompleted += a;
                }
            }
            MessageBox.Show($"当前用户有效订单实付金额总计{result}，已确认收货订单实付金额总计{resultCompleted}，仅供参考！", "计算结果", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public DependencyCommand OrderTargetCommand { get; set; }
        public void OrderTarget(object parameter)
        {
            RecommendId = null;
            OnPropertyChanged(nameof(RecommendId));
            GetOrder(null);
        }

        public LitemallOrder[] Orders { get; set; }

        public OrderListViewModel()
        {
            GetOrderCommand = new DependencyCommand(GetOrder, DependencyCommand.AlwaysCan);
            CalculateCommand = new DependencyCommand(Calculate, DependencyCommand.AlwaysCan);
            OrderTargetCommand = new DependencyCommand(OrderTarget, DependencyCommand.AlwaysCan);

            if (!(JupiterClient.IsCA || JupiterClient.IsMA))
            {
                GetOrder(null);
            }

        }
    }

    internal class OrderStatusCodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var statusCode = (short)value;
            switch (statusCode)
            {
                case 101:
                    return "用户下单";
                case 102:
                    return "用户取消";
                case 103:
                    return "超时取消";
                case 201:
                    return "等待发货";
                case 202:
                    return "用户退单";
                case 203:
                    return "退款成功";
                case 301:
                    return "等待收货";
                case 401:
                case 402:
                    return "交易成功";
                default:
                    return $"意外状态({statusCode})";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
