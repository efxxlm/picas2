using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            return await _context.ProcesoSeleccion.ToListAsync();
        }

        public async Task<ProcesoSeleccion> GetSelectionProcessById(int id)
        {
            return await _context.ProcesoSeleccion.FindAsync(id);
        }

        public async Task<Respuesta> Insert(ProcesoSeleccion procesoSeleccion)
        {
            Respuesta _response = new Respuesta();
            try
            {
                if (procesoSeleccion != null)
                {
                    _context.Add(procesoSeleccion);
                    await _context.SaveChangesAsync();

                    return _response = new Respuesta {  IsSuccessful = true, IsValidation = false, Data = procesoSeleccion, Code = ConstantMessagesProcesoSeleccion.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta  { IsSuccessful = false, IsValidation = false,  Data = null, Code = ConstantMessagesProcesoSeleccion.RecursoNoEncontrado };
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

                };
            }
        }

        public async Task<Respuesta> Update(ProcesoSeleccion procesoSeleccion)
        {
            Respuesta _response = new Respuesta();

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

                };
            }
        }

        public ProcesoSeleccion GetObj(ProcesoSeleccion procesoSeleccion)
        {
            ProcesoSeleccion updateObj =  _context.ProcesoSeleccion.Find(procesoSeleccion.ProcesoSeleccionId);
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
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
