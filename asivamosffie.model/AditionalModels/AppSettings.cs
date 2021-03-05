using asivamosffie.model.APIModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.AditionalModels
{
    public class AppSettings : AppSettingsService
    {
        public new string DominioFront { get; set; }
        public new string MailServer { get; set; }
        public new int MailPort { get; set; }
        public new bool EnableSSL { get; set; }
        public new string Sender { get; set; }
        public new string Password { get; set; }
        public string DirectoryBasePagos { get; set; }
        public string DirectoryBaseRendimientos { get; set; }

    }
}
