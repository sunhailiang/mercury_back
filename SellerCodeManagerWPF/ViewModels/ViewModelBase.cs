using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SellerCodeManagerWPF.ViewModels
{
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 定义一个命令
        /// </summary>
        /// <typeparam name="T1">Command Parameter Type</typeparam>
        /// <typeparam name="T2">CanExecute Parameter Type</typeparam>
        public class DependencyCommand : ICommand
        {
            protected readonly Action<object> ExecuteAction;

            protected readonly Func<object, bool> CanExecuteAction;

            public DependencyCommand(Action<object> executeAction, Func<object, bool> canExecuteAction)
            {
                ExecuteAction = executeAction;
                CanExecuteAction = canExecuteAction;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                bool? x = CanExecuteAction?.Invoke(parameter);
                if (x != null)
                {
                    return x.Value;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// 一个预置方法，使Command总是可用
            /// </summary>
            /// <param name="parameter"></param>
            /// <returns></returns>
            public static bool AlwaysCan(object parameter)
            {
                return true;
            }

            public virtual void Execute(object parameter)
            {
                try
                {
                    ExecuteAction?.Invoke(parameter);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"操作失败：{ex.Message}", "失败了", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            public void OnCanExecutedChanged(object sender, EventArgs e)
            {
                App.Current.Dispatcher.Invoke(() => CanExecuteChanged?.Invoke(sender, e));
            }
        }
    }
}
