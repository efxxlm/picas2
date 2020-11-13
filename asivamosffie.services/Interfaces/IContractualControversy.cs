using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IContractualControversy
    {
        Task<Respuesta> CreateEditarControversiaTAI(ControversiaContractual controversiaContractual);

        Task<Respuesta> CreateEditNuevaActualizacionTramite(ControversiaActuacion controversiaActuacion);

        Task<List<GrillaTipoSolicitudControversiaContractual>> ListGrillaTipoSolicitudControversiaContractual();

         Task<VistaContratoContratista> GetVistaContratoContratista(int pContratoId);

        Task<ControversiaContractual> GetControversiaContractualById(int pControversiaContractualId);

        Task<ControversiaActuacion> GetControversiaActuacionById(int id);

        Task<Respuesta> CambiarEstadoControversiaActuacion(int pControversiaActuacionId, string pNuevoCodigoEstadoAvance, string pUsuarioModifica);

        Task<Respuesta> CambiarEstadoControversiaContractual(int pControversiaContractualId, string pNuevoCodigoEstado, string pUsuarioModifica);

        Task<Respuesta> EliminarControversiaActuacion(int pControversiaActuacionId, string pUsuario);
        Task<Respuesta> EliminarControversiaContractual(int pControversiaContractualId, string pUsuario);
    }
}
