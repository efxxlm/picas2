using AuthorizationTest.JwtHelpers;

namespace asivamosffie.model.APIModels
{
    public class ArchivoCargueRespuesta
    { 
        public string LlaveConsulta { get; set; }

        public string CantidadDeRegistros { get; set; }

        public string CantidadDeRegistrosValidos { get; set; }

        public string CantidadDeRegistrosInvalidos { get; set; }
        
        public bool? CargaValida { get; set; }

        public string Mensaje { get; set; }  
        public int? AjusteProgramacionId { get; set; } 

    }
}
