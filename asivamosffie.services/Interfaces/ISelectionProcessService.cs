using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface ISelectionProcessService
    {
        Task<ActionResult<List<ProcesoSeleccion>>> GetSelectionProcess();
        Task<ProcesoSeleccion> GetSelectionProcessById(int id);
        Task<Respuesta> CreateEditarProcesoSeleccion(ProcesoSeleccion procesoSeleccion);
        Task<Respuesta> CreateEditarProcesoSeleccionCronograma(ProcesoSeleccionCronograma procesoSeleccionCronograma, bool esTransaccion);
        Task<ActionResult<List<ProcesoSeleccionCronograma>>> GetSelectionProcessSchedule();
        Task<ActionResult<List<GrillaControlCronograma>>> GetControlGridSchedule();
        Task<ProcesoSeleccionCronograma> GetSelectionProcessScheduleById(int id);
        Task<ActionResult<List<ProcesoSeleccionCronograma>>> GetRecordActivities(int ProcesoSeleccionId);
        Task<ActionResult<List<ProcesoSeleccionCronograma>>> GetScheduleBySelectionProcessId(int ProcesoSeleccionId);
        Task<Respuesta> CreateEditarProcesoSeleccionGrupo(ProcesoSeleccionGrupo procesoSeleccionGrupo);

        //Cotizacion
        Task<ProcesoSeleccionCotizacion> GetProcesoSeleccionCotizacionById(int id);
        Task<ActionResult<List<ProcesoSeleccionCotizacion>>> GetProcesoSeleccionCotizacion();
        Task<ActionResult<List<ProcesoSeleccionCotizacion>>> GetCotizacionByProcesoSeleccionId(int ProcesoSeleccionId);
        Task<Respuesta> CreateEditarProcesoSeleccionCotizacion(ProcesoSeleccionCotizacion procesoSeleccionCotizacion);

        //Proponente
        Task<ProcesoSeleccionCronograma> GetProcesoSeleccionProponenteById(int id);
        Task<ActionResult<List<GrillaProcesoSeleccionProponente>>> GetGridProcesoSeleccionProponente(int? procesoSeleccionId);
        Task<Respuesta> CreateEditarProcesoSeleccionProponente(ProcesoSeleccionProponente procesoSeleccionProponente);

        //Integrante
        Task<ProcesoSeleccionIntegrante> GetProcesoSeleccionIntegranteById(int id);
        Task<ActionResult<List<GrillaProcesoSeleccionIntegrante>>> GetGridProcesoSeleccionIntegrante(int? procesoSeleccionId);
        Task<Respuesta> CreateEditarProcesoSeleccionIntegrante(ProcesoSeleccionIntegrante procesoSeleccionIntegrante);
        Task<Respuesta> CreateEditarCronogramaSeguimiento(CronogramaSeguimiento cronogramaSeguimiento);
         Task<Respuesta> SetValidateCargueMasivo(IFormFile pFile, string pFilePatch, string pUsuarioCreo);
         Task<Respuesta> UploadMassiveLoadElegibilidad(string pIdDocument,int procesoSeleccionId, string pUsuarioModifico);
         Task<Respuesta> DeleteProcesoSeleccion( int pId, string pUsuarioModificacion );
         Task<Respuesta> ChangeStateProcesoSeleccion( int pId, string pUsuarioModificacion, string pCodigoEstado,string pFront, string mailServer, int mailPort, bool enableSSL, string password, string sender);
         Task<List<ProcesoSeleccionProponente>> GetProcesoSeleccionProponentes();
         Task<Respuesta> CreateContractorsFromProponent(ProcesoSeleccion pProcesoSeleccion, string pUsuarioCreo);
        Task<Respuesta> deleteProcesoSeleccionCotizacionByID(int procesoSeleccionCotizacionId, string usuarioModificacion);
        Task<Respuesta> deleteProcesoSeleccionGrupoByID(int procesoSeleccionCotizacionId, string usuarioModificacion);
        Task<Respuesta> deleteProcesoSeleccionActividadesByID(int procesoSeleccionCotizacionId, string usuarioModificacion);
        Task<List<ProcesoSeleccionObservacion>> getObservacionesProcesoSeleccionProponentes(int id);
    }
}
