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
using System.Globalization;
using Microsoft.Extensions.Hosting;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class FinancialBalanceService : IFinalBalanceService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirementsConstructionPhaseService;
        private readonly IRegisterValidatePaymentRequierementsService _registerValidatePaymentRequierementsService;

        public FinancialBalanceService(devAsiVamosFFIEContext context,
                                       ICommonService commonService,
                                       IRegisterValidatePaymentRequierementsService registerValidatePaymentRequierementsService)
        {
            _context = context;
            _commonService = commonService;
            _registerValidatePaymentRequierementsService = registerValidatePaymentRequierementsService;
        }

        public async Task<List<VProyectosBalance>> GridBalance()
        {
            return await _context.VProyectosBalance.OrderByDescending(r => r.ProyectoId).ToListAsync();
        }

        public async Task<dynamic> GetOrdenGiroBy(string pTipoSolicitudCodigo, string pNumeroOrdenGiro)
        { 
            if (string.IsNullOrEmpty(pNumeroOrdenGiro))
            {
                return (
                    _context.VOrdenGiro
                    .Where(v => v.TipoSolicitudCodigo == pTipoSolicitudCodigo && v.RegistroCompletoTramitar)
                    .Select(v => new
                    {
                     
                        v.FechaAprobacionFinanciera,
                
                        v.TipoSolicitud,
                        v.NumeroSolicitudOrdenGiro,
                        v.OrdenGiroId 
                    }));
            }
            else
            {
                return (
                    _context.VOrdenGiro
                    .Where(v => v.NumeroSolicitudOrdenGiro == pNumeroOrdenGiro && v.RegistroCompletoTramitar)
                    .Select(v => new
                    {
                        v.NumeroSolicitudOrdenGiro,
                        v.OrdenGiroId
                    })); 
            } 
        }

        public async Task<List<dynamic>> GetContratoByProyectoId(int pProyectoId)
        {
            try
            {
                List<dynamic> ListContratos = new List<dynamic>();

                List<ContratacionProyecto> ListContratacion = await _context.ContratacionProyecto
                                            .Where(r => r.ProyectoId == pProyectoId)
                                            .Include(r => r.Contratacion)
                                             .ThenInclude(r => r.Contratista)
                                            .Include(r => r.Contratacion)
                                             .ThenInclude(r => r.Contrato)
                                            .ToListAsync();
                ListContratacion.OrderBy(r => r.Contratacion.TipoSolicitudCodigo);

                foreach (var contratacionProyecto in ListContratacion)
                {
                    Contrato contrato = await _context.Contrato
                        .Where(c => c.ContratoId == contratacionProyecto.Contratacion.Contrato.FirstOrDefault().ContratoId)
                        .Include(c => c.ContratoConstruccion)
                        .Include(c => c.ContratoPoliza)
                        .Include(c => c.Contratacion).ThenInclude(c => c.Contratista)
                        .Include(c => c.Contratacion).ThenInclude(cp => cp.DisponibilidadPresupuestal)
                        .Include(r => r.SolicitudPago).ThenInclude(r => r.SolicitudPagoCargarFormaPago)
                        .Include(r => r.SolicitudPago).ThenInclude(r => r.OrdenGiro)
                        .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(t => t.ContratacionProyectoAportante).ThenInclude(t => t.CofinanciacionAportante).ThenInclude(t => t.FuenteFinanciacion).ThenInclude(t => t.CuentaBancaria)
                        .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(t => t.ContratacionProyectoAportante).ThenInclude(t => t.CofinanciacionAportante).ThenInclude(t => t.NombreAportante)
                        .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(t => t.ContratacionProyectoAportante).ThenInclude(t => t.CofinanciacionAportante).ThenInclude(t => t.Municipio)
                        .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(t => t.ContratacionProyectoAportante).ThenInclude(t => t.CofinanciacionAportante).ThenInclude(t => t.Departamento)
                        .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(t => t.ContratacionProyectoAportante).ThenInclude(t => t.ComponenteAportante).ThenInclude(t => t.ComponenteUso)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                    if (contrato.SolicitudPago.Count() > 0)
                        contrato.SolicitudPago = contrato.SolicitudPago.Where(s => s.Eliminado != true).ToList();

                    contrato.ValorFacturadoContrato =
                        _context.VValorFacturadoContrato
                        .Where(v => v.ContratoId == contratacionProyecto.Contratacion.Contrato.FirstOrDefault().ContratoId)
                        .ToList();

                    contrato.VContratoPagosRealizados =
                        _context.VContratoPagosRealizados
                           .Where(v => v.ContratoId == contratacionProyecto.Contratacion.Contrato.FirstOrDefault().ContratoId)
                           .ToList();

                    contrato.TablaDRP = _registerValidatePaymentRequierementsService.GetDrpContrato(contrato);

                    ListContratos.Add(new
                    {
                        contrato,
                        tipoSolicitudCodigo = contratacionProyecto.Contratacion.TipoSolicitudCodigo,
                        
                    });
                }

                return ListContratos;
            }
            catch (Exception ex)
            {
                return new List<dynamic>();
            }
        }

        public async Task<Respuesta> CreateEditBalanceFinanciero(BalanceFinanciero pBalanceFinanciero)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Balance_Financiero, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            try
            {
                BalanceFinanciero balanceFinanciero = _context.BalanceFinanciero.Where(r => r.ProyectoId == pBalanceFinanciero.ProyectoId).FirstOrDefault();

                if (pBalanceFinanciero.RequiereTransladoRecursos == false)
                {
                    pBalanceFinanciero.NumeroTraslado = 0;
                    pBalanceFinanciero.EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.Con_balance_validado;
                    pBalanceFinanciero.RegistroCompleto = true;
                }
                else
                {
                    pBalanceFinanciero.RegistroCompleto = await registroCompletoBalanceFinanciero(pBalanceFinanciero);
                    if (pBalanceFinanciero.RegistroCompleto == false)
                    {
                        pBalanceFinanciero.EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.En_proceso_de_validacion;
                    }
                    else
                    {
                        pBalanceFinanciero.EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.Con_balance_validado;
                    }
                }

                if (pBalanceFinanciero.BalanceFinancieroId == 0 && balanceFinanciero == null)
                {
                    strCrearEditar = "CREAR BALANCE FINANCIERO";
                    pBalanceFinanciero.FechaCreacion = DateTime.Now;
                    _context.BalanceFinanciero.Add(pBalanceFinanciero);
                }
                else
                {
                    strCrearEditar = "ACTUALIZAR BALANCE FINANCIERO";
                    await _context.Set<BalanceFinanciero>().Where(r => r.BalanceFinancieroId == pBalanceFinanciero.BalanceFinancieroId)
                                                                   .UpdateAsync(r => new BalanceFinanciero()
                                                                   {
                                                                       FechaModificacion = DateTime.Now,
                                                                       UsuarioModificacion = pBalanceFinanciero.UsuarioCreacion,
                                                                       RequiereTransladoRecursos = pBalanceFinanciero.RequiereTransladoRecursos,
                                                                       JustificacionTrasladoAportanteFuente = pBalanceFinanciero.JustificacionTrasladoAportanteFuente,
                                                                       UrlSoporte = pBalanceFinanciero.UrlSoporte,
                                                                       NumeroTraslado = pBalanceFinanciero.NumeroTraslado > 0 ? pBalanceFinanciero.NumeroTraslado : balanceFinanciero.NumeroTraslado,
                                                                       RegistroCompleto = pBalanceFinanciero.RegistroCompleto,
                                                                       EstadoBalanceCodigo = pBalanceFinanciero.EstadoBalanceCodigo
                                                                   });
                }


                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.OperacionExitosa, idAccion, pBalanceFinanciero.UsuarioCreacion, strCrearEditar)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.Error, idAccion, pBalanceFinanciero.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<BalanceFinanciero> GetBalanceFinanciero(int pProyectoId)
        {
            return await _context.BalanceFinanciero.Where(r => r.ProyectoId == pProyectoId).FirstOrDefaultAsync();
        }

        private async Task<bool> registroCompletoBalanceFinanciero(BalanceFinanciero balanceFinanciero)
        {
            bool state = false;
            BalanceFinanciero balanceFinancieroOld = _context.BalanceFinanciero.Where(r => r.BalanceFinancieroId == balanceFinanciero.BalanceFinancieroId).FirstOrDefault();
            
            if (balanceFinanciero != null)
            {
                if(balanceFinanciero.RequiereTransladoRecursos == false)
                {
                    state = true;
                }
            }

            return state;

        }

        public async Task<Respuesta> ApproveBalance(int pProyectoId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Aprobar_Balance_Financiero, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                BalanceFinanciero balanceFinanciero = _context.BalanceFinanciero.Where(r => r.ProyectoId == pProyectoId).FirstOrDefault();
                
                if (balanceFinanciero != null)
                {
                    balanceFinanciero.FechaAprobacion = DateTime.Now;
                    balanceFinanciero.EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.Con_balance_aprobado;
                    balanceFinanciero.UsuarioModificacion = pUsuario;
                    balanceFinanciero.FechaModificacion = DateTime.Now;
                }

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "INFORME FINAL ")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }
    }
}
