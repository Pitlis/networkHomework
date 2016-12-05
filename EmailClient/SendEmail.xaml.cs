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
using System.Windows.Shapes;

namespace EmailClient
{
	/// <summary>
	/// Interaction logic for SendEmail.xaml
	/// </summary>
	public partial class SendEmail : Window
	{
		public SendEmail()
		{
			InitializeComponent();
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string to = txtTo.Text;
				string subject = txtSubject.Text;
				string message = txtMessage.Text.Replace("\r\n.", "");
				if(to.Length == 0)
				{
					throw new Exception("'To' cannot be empty");
				}
				Container.Core.SendMessage(to, subject, message);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Error sending email!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
