using System.Windows;
using Any.Email.ViewModels;

namespace Any.Email.Views
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
