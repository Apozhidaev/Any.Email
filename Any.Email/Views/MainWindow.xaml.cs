using System.Windows;
using Ap.Email.ViewModels;

namespace Ap.Email.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowModel();
        }
    }
}
