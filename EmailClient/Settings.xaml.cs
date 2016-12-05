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
	/// Interaction logic for Settings.xaml
	/// </summary>
	public partial class Settings : Window
	{
		public Settings()
		{
			InitializeComponent();
		}



		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int pop3Port, stmpPort;
				try
				{
					pop3Port = int.Parse(txtPortPop3.Text);
				}
				catch
				{
					throw new Exception("Incorrect POP3 port");
				}
				try
				{
					stmpPort = int.Parse(txtPortStmp.Text);
				}
				catch
				{
					throw new Exception("Incorrect SMTP port");
				}
				if (txtHostPop3.Text.Length == 0 || txtHostStmp.Text.Length == 0 || txtPass.Password.Length == 0 || txtUser.Text.Length == 0)
				{
					throw new Exception("All fields are required!");
				}

				bool successed = true;
				MessageBox.Show("Test connection. Please wait...", "Test connection", MessageBoxButton.OK, MessageBoxImage.Information);
				try
				{
					Container.Core.VerifyPop3(txtHostPop3.Text, pop3Port, txtUser.Text, txtPass.Password, checkBox.IsChecked.Value);
				}
				catch (Exception ex)
				{
					successed = false;
					MessageBox.Show(ex.Message, "Error connection to POP3 server!", MessageBoxButton.OK, MessageBoxImage.Error);
				}

				try
				{
					Container.Core.VerifyStmp(txtHostStmp.Text, stmpPort, txtUser.Text, txtPass.Password, checkBox.IsChecked.Value);
				}
				catch (Exception ex)
				{
					successed = false;
					MessageBox.Show(ex.Message, "Error connection to SMTP server!", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				if (successed)
				{
					//Container.Core.SaveSettings("smtp.gmail.com", 465, "pop.gmail.com", 995, "email.test3101@gmail.com", "OBLIVION", true);
					Container.Core.SaveSettings(txtHostStmp.Text, stmpPort, txtHostPop3.Text, pop3Port, txtUser.Text, txtPass.Password, checkBox.IsChecked.Value);
					MessageBox.Show("Success", "Test connection", MessageBoxButton.OK, MessageBoxImage.Information);
					this.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (Container.Core.data.User != null)
			{
				txtPortPop3.Text = Container.Core.data.PortPop3.ToString();
				txtPortStmp.Text = Container.Core.data.PortSmtp.ToString();
				txtHostPop3.Text = Container.Core.data.HostPop3;
				txtHostStmp.Text = Container.Core.data.HostSmtp;
				txtUser.Text = Container.Core.data.User;
				txtPass.Password = Container.Core.data.Password;
				checkBox.IsChecked = Container.Core.data.UseSSL;
			}
		}
	}
}
