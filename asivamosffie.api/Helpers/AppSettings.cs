namespace asivamosffie.api
{
    public class AppSettings
    {
        public string Dominio { get; set; }

        public string DominioFront { get; set; }
        
        public string MailServer { get; set; }

        public int MailPort { get; set; }

        public bool EnableSSL { get; set; }

        public string SenderName { get; set; }

        public string Sender { get; set; }

        public string Password { get; set; }

        public string Secret { get; set; }

        public string asivamosffieIssuerJwt { get; set; }

        public string asivamosffieAudienceJwt { get; set; }

        public string yearVigente { get; set; }

        public bool yearSiguienteEsVigente { get; set; }

    }
}