
namespace asivamosffie.model.APIModels
{
    public class ObservacionComentario
    {
        public int? SesionSolicitudCompromisoId { get; set; }
        public int? TemaCompromisoId { get; set; }
        public string Observacion { get; set; }
        public string Usuario { get; set; }
        public int UsuarioId { get; set; }
    }
}