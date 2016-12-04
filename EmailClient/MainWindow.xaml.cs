using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmailClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Container.Core = new Core.Core("config.bin");

            emailList.ItemsSource = new List<string>() { "item 1", "item 2", "item 3", "item 1", "item 2", "item 3", "item 1", "item 2", "item 3" };
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            Settings dialog = new Settings();
            dialog.ShowDialog();
        }
    }
}
