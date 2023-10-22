using FileAnalyzer.ViewModels;
using System.Threading;
using System.Windows;

namespace FileAnalyzer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            //new Thread(() =>
            //{
            //    for (int i = 0; i < 100; i++)
            //    {
            //        Dispatcher.Invoke(() => this.pgb.Value += i);
            //        Thread.Sleep(200);
            //    }
            //}).Start();
            
        }
    }
}
