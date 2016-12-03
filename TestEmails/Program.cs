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
    class Program
    {
        //static void Main(string[] args)
        //{
        //    EmailSender sender = new EmailSender("smtp.gmail.com", 465, "email.test3101.2@gmail.com", "");
        //    sender.SendSslEmail("email.test3101@gmail.com", "Test subject", "Hello world!! \n By Nikita");
        //}

        

        static void Main(string[] args)
        {
            EmailReader reader = new EmailReader("pop.gmail.com", 995, "email.test3101@gmail.com", "");
            var emails = reader.GetAllEmailsSsl();
        }
    }
}
