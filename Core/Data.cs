using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    [Serializable]
    public class Data
    {
        public string HostSmtp { get; set; }
        public int PortSmtp { get; set; }
        public string HostPop3 { get; set; }
        public int PortPop3 { get; set; }
        public bool UseSSL { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public List<Email> Emails { get; set; }
    }
}
