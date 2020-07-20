using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asivamosffie.services
{

    public class SelectionProcessService: ISelectionProcessService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public SelectionProcessService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<ActionResult<List<ProcesoSeleccion>>> GetSelectionProcess()
        {
            try
            {
                return await _context.ProcesoSeleccion.Where(r => !(bool)r.Eliminado).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ProcesoSeleccion> GetSelectionProcessById(int id)
        {
            try
            {
                return await _context.ProcesoSeleccion.FindAsync(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Respuesta> Insert(ProcesoSeleccion procesoSeleccion)
        {
            Respuesta _response = new Respuesta();
            int AccionId = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proceso_Seleccion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (procesoSeleccion != null)
                {
                    procesoSeleccion.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                    procesoSeleccion.FechaCreacion = DateTime.Now;
                    _context.Add(procesoSeleccion);
                    await _context.SaveChangesAsync();

                    return _response = new Respuesta { IsSuccessful = true, IsValidation = false, Data = procesoSeleccion, Code = ConstantMessagesProcesoSeleccion.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta
                    {
                        IsSuccessful = false,
                        IsValidation = false,
                        Data = null,
                        Code = ConstantMessagesProcesoSeleccion.RecursoNoEncontrado,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, AccionId, procesoSeleccion.UsuarioCreacion, " ")
                    };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta
                {
                    IsSuccessful = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, AccionId, procesoSeleccion.UsuarioCreacion, " ")

                };
            }
        }

        public async Task<Respuesta> Update(ProcesoSeleccion procesoSeleccion)
        {
            Respuesta _response = new Respuesta();
            int AccionId = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proceso_Seleccion, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                ProcesoSeleccion updateObj = GetObj(procesoSeleccion);
                _context.Update(updateObj);
                await _context.SaveChangesAsync();
                return _response = new Respuesta
                {
                    IsSuccessful = true,
                    IsValidation = false,
                    Data = updateObj,
                    Code = ConstantMessagesProcessSchedule.EditadoCorrrectamente,

                };
            }
            catch (Exception ex)
            {
                return _response = new Respuesta
                {
                    IsSuccessful = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcessSchedule.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, AccionId, procesoSeleccion.UsuarioCreacion, " ")


                };
            }
        }

        public ProcesoSeleccion GetObj(ProcesoSeleccion procesoSeleccion)
        {
            ProcesoSeleccion updateObj = _context.ProcesoSeleccion.Find(procesoSeleccion.ProcesoSeleccionId);
            updateObj.NumeroProceso = procesoSeleccion.NumeroProceso;
            updateObj.Objeto = procesoSeleccion.Objeto;
            updateObj.AlcanceParticular = procesoSeleccion.AlcanceParticular;
            updateObj.Justificacion = procesoSeleccion.Justificacion;
            updateObj.CriteriosSeleccion = procesoSeleccion.CriteriosSeleccion;
            updateObj.TipoIntervencionCodigo = procesoSeleccion.TipoIntervencionCodigo;
            updateObj.TipoAlcanceCodigo = procesoSeleccion.TipoAlcanceCodigo;
            updateObj.TipoProcesoCodigo = procesoSeleccion.TipoProcesoCodigo;
            updateObj.EsDistribucionGrupos = procesoSeleccion.EsDistribucionGrupos;
            updateObj.CantGrupos = procesoSeleccion.CantGrupos;
            updateObj.ResponsableTecnicoUsuarioId = procesoSeleccion.ResponsableTecnicoUsuarioId;
            updateObj.ResponsableEstructuradorUsuarioid = procesoSeleccion.ResponsableEstructuradorUsuarioid;
            updateObj.CondicionesJuridicasHabilitantes = procesoSeleccion.CondicionesJuridicasHabilitantes;
            updateObj.CondicionesFinancierasHabilitantes = procesoSeleccion.CondicionesFinancierasHabilitantes;
            updateObj.CondicionesTecnicasHabilitantes = procesoSeleccion.CondicionesTecnicasHabilitantes;
            updateObj.CondicionesAsignacionPuntaje = procesoSeleccion.CondicionesAsignacionPuntaje;
            updateObj.CantidadCotizaciones = procesoSeleccion.CantidadCotizaciones;
            updateObj.CantidadProponentes = procesoSeleccion.CantidadProponentes;
            updateObj.EsCompleto = procesoSeleccion.EsCompleto;
            updateObj.EstadoProcesoSeleccionCodigo = procesoSeleccion.EstadoProcesoSeleccionCodigo;
            updateObj.EtapaProcesoSeleccionCodigo = procesoSeleccion.EtapaProcesoSeleccionCodigo;
            updateObj.EvaluacionDescripcion = procesoSeleccion.EvaluacionDescripcion;
            updateObj.UrlSoporteEvaluacion = procesoSeleccion.UrlSoporteEvaluacion;
            updateObj.TipoOrdenEligibilidadCodigo = procesoSeleccion.TipoOrdenEligibilidadCodigo;
            updateObj.CantGrupos = procesoSeleccion.CantGrupos;
            updateObj.FechaModificacion = DateTime.Now;

            return updateObj;

        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                ProcesoSeleccion entity = await GetSelectionProcessById(id);
                _context.ProcesoSeleccion.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

