using AuthorizationTest.JwtHelpers;
using asivamosffie.model.Models;

namespace asivamosffie.model.APIModels
{
    public class ProyectoAdministracionGrilla
    {
        public int ProyectoAdminitracionId { get; set; }
        public bool Enviado { get; set; }
        public bool? Estado { get; set; }
        public ProyectoAdministrativo Proyecto { get; set; }
    }
}