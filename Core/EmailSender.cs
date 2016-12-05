using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
	class EmailSender
	{
		string host;
		int port;
		string user;
		string password;

		public EmailSender(string host, int port, string user, string password)
		{
			this.host = host;
			this.port = port;
			this.user = user;
			this.password = password;
		}

		public void SendSslEmail(string to, string subject, string message)
		{
			try
			{
				using (var client = new TcpClient())
				{
					client.Connect(host, port);
					using (var stream = client.GetStream())
					using (var sslStream = new SslStream(stream))
					{
						sslStream.AuthenticateAsClient(host);
						using (var writer = new StreamWriter(sslStream))
						using (var reader = new StreamReader(sslStream))
						{
							Send(writer, reader, to, subject, message);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public void SendEmail(string to, string subject, string message)
		{
			try
			{
				using (var client = new TcpClient())
				{
					client.Connect(host, port);
					using (var stream = client.GetStream())
					using (var writer = new StreamWriter(stream))
					using (var reader = new StreamReader(stream))
					{
						Send(writer, reader, to, subject, message);
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public void VerifyConnectionSSL()
		{
			try
			{
				using (var client = new TcpClient())
				{
					client.Connect(host, port);
					using (var stream = client.GetStream())
					using (var sslStream = new SslStream(stream))
					{
						sslStream.AuthenticateAsClient(host);
						using (var writer = new StreamWriter(sslStream))
						using (var reader = new StreamReader(sslStream))
						{
							Verify(writer, reader);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public void VerifyConnection()
		{
			try
			{
				using (var client = new TcpClient())
				{
					client.Connect(host, port);
					using (var stream = client.GetStream())
					using (var writer = new StreamWriter(stream))
					using (var reader = new StreamReader(stream))
					{
						Verify(writer, reader);
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void Verify(StreamWriter writer, StreamReader reader)
		{
			string resp;
			writer.WriteLine("HELO " + host);
			writer.Flush();
			resp = reader.ReadLine();
			if (!resp.StartsWith("220"))
			{
				throw new Exception("Error execute command HELO:\n" + resp);
			}

			writer.WriteLine("AUTH LOGIN " + Base64Encode(user));
			writer.Flush();
			resp = reader.ReadLine();
			resp = reader.ReadLine();

			writer.WriteLine(Base64Encode(password));
			writer.Flush();
			resp = reader.ReadLine();//235 - accepted
			if (!resp.StartsWith("235"))
			{
				throw new Exception("Authorization failed. Check your server settings\n" + resp);
			}
		}



		private void Send(StreamWriter writer, StreamReader reader, string to, string subject, string message)
		{
			string resp;

			writer.WriteLine("HELO " + host);
			writer.Flush();
			resp = reader.ReadLine();
			if (!resp.StartsWith("220"))
			{
				throw new Exception("Error execute command HELO:\n" + resp);
			}

			writer.WriteLine("AUTH LOGIN " + Base64Encode(user));
			writer.Flush();
			resp = reader.ReadLine();
			resp = reader.ReadLine();

			writer.WriteLine(Base64Encode(password));
			writer.Flush();
			resp = reader.ReadLine();//235 - accepted
			if (!resp.StartsWith("235"))
			{
				throw new Exception("Authorization failed. Check your server settings\n" + resp);
			}

			writer.WriteLine("MAIL FROM: <" + to + ">");
			writer.Flush();
			resp = reader.ReadLine();
			if (!resp.StartsWith("250"))
			{
				throw new Exception("Incorrect sender address.\n" + resp);
			}

			writer.WriteLine("RCPT TO: <" + to + ">");
			writer.Flush();
			resp = reader.ReadLine();
			if (!resp.StartsWith("250"))
			{
				throw new Exception("Incorrect reciever address.\n" + resp);
			}

			writer.WriteLine("DATA");
			writer.Flush();
			resp = reader.ReadLine();

			writer.WriteLine("Subject: " + subject);
			writer.WriteLine("");
			writer.WriteLine(message);
			writer.WriteLine(".");
			writer.Flush();
			resp = reader.ReadLine();
			if (!resp.StartsWith("250"))
			{
				throw new Exception("Error sending email.s\n" + resp);
			}

			writer.WriteLine("quit");
			writer.Flush();
			resp = reader.ReadLine();
		}


		private string Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}
	}
}
