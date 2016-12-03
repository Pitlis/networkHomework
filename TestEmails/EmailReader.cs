using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestEmails
{
    class EmailReader
    {
        string host;
        int port;
        string user;
        string password;

        public EmailReader(string host, int port, string user, string password)
        {
            this.host = host;
            this.port = port;
            this.user = user;
            this.password = password;
        }

        public IList<Email> GetAllEmailsSsl()
        {
            List<Email> emails = new List<Email>();
            try
            {
                string resp;
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
                            resp = reader.ReadLine();

                            writer.WriteLine("USER " + user);
                            writer.Flush();
                            resp = reader.ReadLine();
                            if (!resp.StartsWith("+OK"))
                            {
                                throw new Exception(resp);
                            }

                            writer.WriteLine("PASS " + password);
                            writer.Flush();
                            resp = reader.ReadLine();
                            if (!resp.StartsWith("+OK"))
                            {
                                throw new Exception("Authorization failed. Check your server settings\n" + resp);
                            }

                            writer.WriteLine("LIST");
                            writer.Flush();
                            resp = reader.ReadLine();
                            if (!resp.StartsWith("+OK"))
                            {
                                throw new Exception("Error get list of messages:\n" + resp);
                            }

                            resp = reader.ReadLine();

                            int countMessages = 0;
                            do
                            {
                                resp = reader.ReadLine();
                                countMessages++;
                            } while (resp != ".");

                            for (int mailIndex = 1; mailIndex <= countMessages; mailIndex++)
                            {
                                writer.WriteLine("RETR " + mailIndex.ToString());
                                writer.Flush();


                                emails.Add(ReadEmail(reader));
                            }
                            emails.Reverse();
                            stream.Close();
                            client.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return emails;
        }

        private void FillEmail(string response, Email email)
        {
            if (response.StartsWith("Date"))
            {
                email.Date = response.Substring(6);
            }
            if (response.StartsWith("From"))
            {
                email.From = response.Substring(6);
            }
            if (response.StartsWith("Subject"))
            {
                email.Subject = response.Substring(9);
            }
        }

        private Email ReadEmail(StreamReader reader)
        {
            Email email = new Email();
            string message = "";
            bool contentStarted = false;

            string resp = reader.ReadLine();
            do
            {
                resp = reader.ReadLine();
                FillEmail(resp, email);
                if (resp == "")
                {
                    contentStarted = true;
                    email.Text = "";
                }
                if (contentStarted)
                {
                    email.Text += resp + "\n";
                }
                message += resp + "\n";
            } while (resp != ".");

            email.Full = message;

            return email;
        }
    }
}
