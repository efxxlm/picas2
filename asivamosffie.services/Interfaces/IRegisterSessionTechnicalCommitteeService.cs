using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IRegisterSessionTechnicalCommitteeService
    {
        Task<List<ValidacionSolicitudesContractualesGrilla>> GetListSolicitudesContractuales();

        Task<Respuesta> SaveEditSesionComiteTema(List<SesionComiteTema> pListSesionComiteTema ,DateTime pFechaProximoComite);

        Task<List<ComiteGrilla>> GetComiteGrilla();

        Task<Sesion> GetSesionBySesionId(int pSesionId);

        Task<Respuesta> EliminarSesionComiteTema(int pSesionComiteTemaId, string pUsuarioModificacion);

        Task<Respuesta> CambiarEstadoComite(Sesion pSesion);
    }
}
