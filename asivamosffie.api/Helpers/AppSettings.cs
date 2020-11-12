namespace asivamosffie.api
{
    public class AppSettings
    {
        public string Dominio { get; set; }

        public string DominioFront { get; set; }
        
        public string MailServer { get; set; }

        public string RutaLogo { get; set; }
         
        public int MailPort { get; set; }

        public bool EnableSSL { get; set; }

        public string SenderName { get; set; }

        public string Sender { get; set; }

        public string Password { get; set; }

        public string Secret { get; set; }

        public string asivamosffieIssuerJwt { get; set; }

        public string asivamosffieAudienceJwt { get; set; }

        public string DirectoryBase { get; set; }

        public string DirectoryBaseCargue { get; set; }

        public string DirectoryBaseActaInicio { get; set; }

        public string DirectoryBaseProyectos { get; set; }
         
        public string YearVigente { get; set; }

        public bool YearSiguienteEsVigente { get; set; }

        public string DirectoryBaseOrdeELegibilidad { get; set; }
         
        public string DirectoryBaseContratacionMinuta { get; set; }
         
        public string DirectoryBaseRutaDocumentoContrato { get; set; }

        public string DirectoryActaSuscritaContrato { get; set; }

    }
  
}