using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;

namespace asivamosffie.services
{
    public class RegisterFinalReportService : IRegisterFinalReportService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirementsConstructionPhaseService;

        public RegisterFinalReportService(devAsiVamosFFIEContext context, ICommonService commonService, ITechnicalRequirementsConstructionPhaseService technicalRequirementsConstructionPhaseService)
        {
            _context = context;
            _commonService = commonService;
            _technicalRequirementsConstructionPhaseService = technicalRequirementsConstructionPhaseService;
        }

        public async Task<List<VProyectosCierre>> gridRegisterFinalReport()
        {
            return await _context.VProyectosCierre.OrderByDescending(r => r.FechaTerminacionProyecto).ToListAsync();
        }

        public async Task<List<InformeFinal>> GetInformeFinalByContratacionProyectoId(int pContratacionProyectoId)
        {
            List<InformeFinal> ListInfomeFinal = await _context.InformeFinal.Where(r => r.ContratacionProyectoId == pContratacionProyectoId)
                                                        .Include(r => r.ContratacionProyecto)
                                                        .ThenInclude(r => r.Proyecto)
                                                        .Include(r => r.ContratacionProyecto)
                                                        .ThenInclude(r => r.Contratacion)
                                                        .ThenInclude(r => r.Contrato)
                                                        .ToListAsync();

                return ListInfomeFinal;
        }
        
        private bool VerificarAlertas(SeguimientoDiario pSeguimientoDiario)
        {
            bool tieneAlertas = false;

            if (
                    pSeguimientoDiario.DisponibilidadPersonal == false ||
                    pSeguimientoDiario.DisponibilidadMaterialCodigo == "2" ||
                    pSeguimientoDiario.DisponibilidadMaterialCodigo == "3" ||
                    pSeguimientoDiario.DisponibilidadEquipo == "2" ||
                    pSeguimientoDiario.DisponibilidadEquipo == "3" ||
                    pSeguimientoDiario.ProductividadCodigo == "3"

               )
            {
                tieneAlertas = true;
            }

            return tieneAlertas;
        }

        private bool VerificarRegistroCompleto(InformeFinal pInformeFinal)
        {
            bool completo = true;
            if (
                    pInformeFinal.EstadoInforme == ConstantCodigoEstadoInformeFinal.Con_informe_enviado_para_validación
               )
            {
                completo = false;
            }

            return completo;
        }

        public async Task<Respuesta> CreateEditInformeFinal(InformeFinal pInformeFinal)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Informe_Final, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            try
            {

                if (pInformeFinal.InformeFinalId == 0)
                {
                    CreateEdit = "CREAR INFORME FINAL";
                    pInformeFinal.FechaCreacion = DateTime.Now;
                    pInformeFinal.Eliminado = false;
                    pInformeFinal.RegistroCompleto = VerificarRegistroCompleto(pInformeFinal);
                    _context.InformeFinal.Add(pInformeFinal);
                }
                else
                {
                    CreateEdit = "ACTUALIZAR INFORME FINAL";
                    InformeFinal informeFinal = _context.InformeFinal.Find(pInformeFinal.InformeFinalId);

                    informeFinal.FechaModificacion = DateTime.Now;
                    informeFinal.UsuarioModificacion = pInformeFinal.UsuarioCreacion;

                    informeFinal.RegistroCompleto = VerificarRegistroCompleto(informeFinal);

                }

                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, pInformeFinal.UsuarioCreacion, CreateEdit)
                };
            }
            catch (Exception ex)
            {
                return
                  new Respuesta
                  {
                      IsSuccessful = false,
                      IsException = true,
                      IsValidation = false,
                      Code = GeneralCodes.Error,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.Error, idAccion, pInformeFinal.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<List<InformeFinalInterventoria>> GetInformeFinalListaChequeoByInformeFinalId(int pInformeFinalId)
        {
            List<InformeFinalInterventoria> ListInformeFinalChequeo = await _context.InformeFinalInterventoria.Where(r => r.InformeFinalListaChequeoId == pInformeFinalId)
                                                        .Include(r => r.InformeFinalListaChequeo)
                                                        .ToListAsync();
            return ListInformeFinalChequeo;
        }

    }
}
