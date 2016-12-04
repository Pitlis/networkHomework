﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Core
    {
        private Data data;

        string configPath;

        public Core(string configPath)
        {
            this.configPath = configPath;
            if (File.Exists(configPath))
            {
                data = LoadDataFromConfig(configPath);
            }
            else
            {
                data = new Data();
                data.Emails = new List<Email>();
            }
        }

        Data LoadDataFromConfig(string configPath)
        {
            Data configInfo;
            using (Stream stream = File.Open(configPath, FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                configInfo = (Data)bformatter.Deserialize(stream);
            }
            return configInfo;
        }

        public void UpdateEmailList()
        {
            EmailReader reader = new EmailReader(data.HostPop3, data.PortPop3, data.User, data.Password);
            if (data.UseSSL)
            {
                data.Emails = reader.GetAllEmailsSsl().ToList();
            }
            else
            {
                data.Emails = reader.GetAllEmails().ToList();
            }
        }

        public void SendMessage(string to, string subject, string message)
        {
            EmailSender sender = new EmailSender(data.HostStmp, data.PortPop3, data.User, data.Password);
            if (data.UseSSL)
            {
                sender.SendSslEmail(to, subject, message);
            }
            else
            {
                sender.SendEmail(to, subject, message);
            }
        }

        public void SaveSettings(string hostStmp, int portStpm, string hostPop3, int portPop3, string user, string password, bool useSSL)
        {
            EmailReader reader = new EmailReader(hostPop3, portPop3, user, password);
            var emails = reader.GetAllEmailsSsl();
            data.HostStmp = hostStmp;
            data.PortStmp = portStpm;
            data.HostPop3 = hostPop3;
            data.PortPop3 = portPop3;
            data.User = user;
            data.Password = password;
            data.UseSSL = useSSL;

            using (Stream stream = File.OpenWrite(configPath))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bformatter.Serialize(stream, data);
            }
        }
    }
}
