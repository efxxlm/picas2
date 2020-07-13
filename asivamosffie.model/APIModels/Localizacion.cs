using AuthorizationTest.JwtHelpers;

namespace asivamosffie.model.APIModels
{
    public class Localicacion
    { 
        public string LocalizacionId { get; set; }

        public string Descripcion { get; set; } 
        //para integrar listas de modo inverso, cuanto necesito el departamento de un muniicpio
        public string? IdPadre { get; set; }
    }
}