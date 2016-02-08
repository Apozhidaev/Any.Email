using System.Windows;
using Mvvm;

namespace EmailApp.Views
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Main : Window, IView
    {
        public Main()
        {
            InitializeComponent();
            //DataContext = new MainViewModel();
        }
    }
}