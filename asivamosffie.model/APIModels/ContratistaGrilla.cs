

using AuthorizationTest.JwtHelpers;

namespace asivamosffie.model.APIModels
{
    public class ContratistaGrilla
    {
        public int IdContratista { get; set; }
        public string Nombre { get; set; }
        public string RepresentanteLegal { get; set; }
        public string NumeroInvitacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public bool EsConsorcio { get; set; }

    }
}