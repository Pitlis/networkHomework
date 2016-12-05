using Core;
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

		private void UpdateEmails(object sender, RoutedEventArgs e)
		{
			try
			{
				Container.Core.UpdateEmailList();
				emailList.ItemsSource = Container.Core.data.Emails;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void SendMessage(object sender, RoutedEventArgs e)
		{
			try
			{
				if (Container.Core.data.User != null)
				{
					SendEmail sendForm = new SendEmail();
					sendForm.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (Container.Core.data.Emails != null && Container.Core.data.User != null)
			{
				emailList.ItemsSource = Container.Core.data.Emails;
			}
		}

		private void emailList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Email email = (Email)e.AddedItems[0];
			txtFrom.Text = email.From;
			txtDate.Text = email.Date;
			txtSubject.Text = email.Subject;
			txtTest.Text = email.Text;
		}
	}
}
