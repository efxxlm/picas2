using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{

    public interface IDerivativeActionService
    {
        
        Task<Respuesta> CreateEditarSeguimientoActuacionDerivada(SeguimientoActuacionDerivada seguimientoActuacionDerivada);
        Task<List<GrillaTipoActuacionDerivada>> ListGrillaTipoActuacionDerivada();

        Task<SeguimientoActuacionDerivada> GetSeguimientoActuacionDerivadaById(int id);

        Task<Respuesta> CambiarEstadoControversiaActuacionDerivada(int pActuacionDerivadaId, string pCodigoEstado, string pUsuarioModifica);

        Task<Respuesta> EliminarControversiaActuacionDerivada(int pId, string pUsuario);

        //Task<List<GrillaTipoActuacionDerivada>> ListGrillaTipoActuacionDerivada2();


    }
}
