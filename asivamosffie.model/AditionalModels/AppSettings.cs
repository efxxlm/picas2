using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.AditionalModels
{
    public class AppSettings
    {
        public string DominioFront { get; set; }
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public bool EnableSSL { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
    }
}
