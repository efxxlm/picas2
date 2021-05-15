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
        #region Constructor
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

        #endregion

        #region Get

        public async Task<List<VProyectosBalance>> GridBalance()
        {
            return await _context.VProyectosBalance
                                 .OrderByDescending(r => r.ProyectoId)
                                 .ToListAsync();
        }

        public async Task<dynamic> GetDataByProyectoId(int pProyectoId)
        {
            String numeroContratoObra = string.Empty, numeroContratoInterventoria = string.Empty;

            List<dynamic> ProyectoAjustado = new List<dynamic>();
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            List<Dominio> TipoObraIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar).ToList();

            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            Proyecto proyecto = await _context.Proyecto.Where(r => r.ProyectoId == pProyectoId)
                                                       .Include(r => r.InstitucionEducativa)
                                                       .Include(r => r.BalanceFinanciero).ThenInclude(r => r.BalanceFinancieroTrasladoValor)
                                                       .FirstOrDefaultAsync();

            InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == proyecto.SedeId).FirstOrDefault();
            Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipio).FirstOrDefault();
            proyecto.MunicipioObj = Municipio;
            proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
            proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
            proyecto.Sede = Sede;
            List<ContratacionProyecto> ListContratacion = await _context.ContratacionProyecto
                                                        .Where(r => r.ProyectoId == pProyectoId)
                                                        .Include(r => r.Contratacion).ThenInclude(r => r.Contratista)
                                                        .Include(r => r.Contratacion).ThenInclude(r => r.Contrato)
                                                        .ToListAsync();

            ListContratacion.FirstOrDefault().Contratacion.TipoContratacionCodigo = TipoObraIntervencion.Where(r => r.Codigo == ListContratacion.FirstOrDefault().Contratacion.TipoSolicitudCodigo).Select(r => r.Nombre).FirstOrDefault();

            foreach (var item in ListContratacion)
            {
                Contrato contrato = await _context.Contrato.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefaultAsync();
                Contratacion contratacion = await _context.Contratacion.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefaultAsync();

                if (contrato != null)
                {
                    if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Obra)
                        numeroContratoObra = contrato.NumeroContrato ?? string.Empty;

                    else if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria)
                        numeroContratoInterventoria = contrato.NumeroContrato ?? string.Empty;
                }
            }
            ProyectoAjustado.Add(new
            {
                llaveMen = proyecto.LlaveMen,
                tipoIntervencion = proyecto.tipoIntervencionString,
                institucionEducativa = proyecto.InstitucionEducativa.Nombre,
                sedeEducativa = proyecto.Sede.Nombre,
                departamento = proyecto.DepartamentoObj.Descripcion,
                municipio = proyecto.MunicipioObj.Descripcion,
                numeroContratoObra = numeroContratoObra,
                numeroContratoInterventoria = numeroContratoInterventoria
            });

            return ProyectoAjustado;
        }

        public async Task<dynamic> GetOrdenGiroBy(string pTipoSolicitudCodigo, string pNumeroOrdenGiro, string pLLaveMen)
        {
            if (string.IsNullOrEmpty(pNumeroOrdenGiro))
            {
                List<VOrdenGiroXproyecto> ListOrdenGiroId =
                       _context.VOrdenGiroXproyecto
                       .Where(r => r.LlaveMen == pLLaveMen
                           && r.TipoSolicitudCodigo == pTipoSolicitudCodigo)
                       .ToList();

                List<VOrdenGiroXproyecto> vOrdenGiroXproyectosNoRepetidos = new List<VOrdenGiroXproyecto>();
                List<VOrdenGiro> VOrdenGiro = new List<VOrdenGiro>();

                List<VOrdenGiro> ListOrdenGiro = await _context.VOrdenGiro.ToListAsync();

                foreach (var item in ListOrdenGiroId)
                {
                    if (!vOrdenGiroXproyectosNoRepetidos.Any(r => r.OrdenGiroId == item.OrdenGiroId))
                    {
                        vOrdenGiroXproyectosNoRepetidos.Add(item);
                        VOrdenGiro.Add(ListOrdenGiro.Where(o => o.OrdenGiroId == item.OrdenGiroId).FirstOrDefault());
                    }
                }
                return (VOrdenGiro.Select(v => new
                {

                    v.FechaAprobacionFinanciera,
                    v.TipoSolicitud,
                    v.NumeroSolicitudOrdenGiro,
                    v.OrdenGiroId
                }).ToList());
            }
            else
            {
                List<VOrdenGiroXproyecto> ListOrdenGiroId =
                          _context.VOrdenGiroXproyecto
                          .Where(r => r.LlaveMen == pLLaveMen
                              && r.NumeroOrdenGiro == pNumeroOrdenGiro)
                          .ToList();

                List<VOrdenGiroXproyecto> vOrdenGiroXproyectosNoRepetidos = new List<VOrdenGiroXproyecto>();
                List<VOrdenGiro> VOrdenGiro = new List<VOrdenGiro>();

                List<VOrdenGiro> ListOrdenGiro = await _context.VOrdenGiro.ToListAsync();

                foreach (var item in ListOrdenGiroId)
                {
                    if (!vOrdenGiroXproyectosNoRepetidos.Any(r => r.OrdenGiroId == item.OrdenGiroId))
                    {
                        vOrdenGiroXproyectosNoRepetidos.Add(item);
                        VOrdenGiro.Add(ListOrdenGiro.Where(o => o.OrdenGiroId == item.OrdenGiroId).FirstOrDefault());
                    }
                }
                return (VOrdenGiro.Select(v =>
                new
                {
                    v.FechaAprobacionFinanciera,
                    v.TipoSolicitud,
                    v.NumeroSolicitudOrdenGiro,
                    v.OrdenGiroId
                }).ToList());
            }
        }

        public async Task<BalanceFinanciero> GetBalanceFinanciero(int pProyectoId)
        {
            return await _context.BalanceFinanciero
                                                    .Where(r => r.ProyectoId == pProyectoId)
                                                    .Include(r => r.BalanceFinancieroTrasladoValor)
                                                    .FirstOrDefaultAsync();
        }

        public async Task<List<dynamic>> GetContratoByProyectoId(int pProyectoId)
        {
            try
            {
                List<dynamic> ListContratos = new List<dynamic>();

                List<ContratacionProyecto> ListContratacion = await _context.ContratacionProyecto
                                            .Where(r => r.ProyectoId == pProyectoId)
                                            .Include(r => r.Contratacion).ThenInclude(r => r.Contratista)
                                            .Include(r => r.Contratacion).ThenInclude(r => r.Contrato)
                                            .OrderBy(r => r.Contratacion.TipoSolicitudCodigo)
                                            .ToListAsync();

                foreach (var contratacionProyecto in ListContratacion)
                {
                    Contrato contrato = await _context.Contrato
                   .Where(c => c.ContratoId == contratacionProyecto.Contratacion.Contrato
                   .FirstOrDefault().ContratoId)
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
                        tipoSolicitudCodigo = contratacionProyecto.Contratacion.TipoSolicitudCodigo
                    });
                }

                return ListContratos;
            }
            catch (Exception ex)
            {
                return new List<dynamic>();
            }
        }
        #endregion 

        #region C R U D

        #region create
        public async Task<Respuesta> CreateEditBalanceFinanciero(BalanceFinanciero pBalanceFinanciero)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Balance_Financiero, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            try
            {
                // BalanceFinanciero balanceFinanciero = _context.BalanceFinanciero.Where(r => r.ProyectoId == pBalanceFinanciero.ProyectoId).FirstOrDefault();

                if (pBalanceFinanciero.RequiereTransladoRecursos == false)
                {
                    pBalanceFinanciero.NumeroTraslado = 0;
                    pBalanceFinanciero.EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.Con_balance_validado;
                    pBalanceFinanciero.RegistroCompleto = true;
                }
                else
                {
                    pBalanceFinanciero.RegistroCompleto = await RegistroCompletoBalanceFinanciero(pBalanceFinanciero);
                    if (pBalanceFinanciero.RegistroCompleto == false)
                        pBalanceFinanciero.EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.En_proceso_de_validacion;
                    else
                        pBalanceFinanciero.EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.Con_balance_validado;
                }

                if (pBalanceFinanciero.BalanceFinancieroId == 0)
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
                                                                       NumeroTraslado = pBalanceFinanciero.NumeroTraslado > 0 ? pBalanceFinanciero.NumeroTraslado : pBalanceFinanciero.NumeroTraslado,
                                                                       RegistroCompleto = pBalanceFinanciero.RegistroCompleto,
                                                                       EstadoBalanceCodigo = pBalanceFinanciero.EstadoBalanceCodigo
                                                                   });

                    CreateEditBalanceFinancieroTrasladoValor(pBalanceFinanciero.BalanceFinancieroTrasladoValor, pBalanceFinanciero.UsuarioCreacion);
                }
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

        private void CreateEditBalanceFinancieroTrasladoValor(ICollection<BalanceFinancieroTrasladoValor> ListBalanceFinancieroTrasladoValor, string pAuthor)
        {
            foreach (var BalanceFinancieroTrasladoValor in ListBalanceFinancieroTrasladoValor)
            {
                if (BalanceFinancieroTrasladoValor.BalanceFinancieroTrasladoValorId == 0)
                {
                    BalanceFinancieroTrasladoValor.UsuarioCreacion = pAuthor;
                    BalanceFinancieroTrasladoValor.FechaCreacion = DateTime.Now;
                    BalanceFinancieroTrasladoValor.Eliminado = false;
                    BalanceFinancieroTrasladoValor.RegistroCompleto = BalanceFinancieroTrasladoValor.ValorTraslado != null;

                    _context.BalanceFinancieroTrasladoValor.Add(BalanceFinancieroTrasladoValor);

                    _context.Set<OrdenGiro>()
                            .Where(o => o.OrdenGiroId == BalanceFinancieroTrasladoValor.OrdenGiroId)
                            .Update(o => new OrdenGiro
                            {
                                ValorNetoGiroTraslado = ListBalanceFinancieroTrasladoValor.Where(r => r.OrdenGiroId == BalanceFinancieroTrasladoValor.OrdenGiroId).Sum(r => r.ValorTraslado) ?? 0,
                                TieneTraslado = true
                            }); ;
                }
                else
                {
                    _context.Set<BalanceFinancieroTrasladoValor>()
                            .Where(r => r.BalanceFinancieroTrasladoValorId == BalanceFinancieroTrasladoValor.BalanceFinancieroTrasladoValorId)
                            .Update(r => new BalanceFinancieroTrasladoValor
                            {
                                EsPreconstruccion = BalanceFinancieroTrasladoValor.EsPreconstruccion,
                                UsuarioModificacion = pAuthor,
                                FechaModificacion = DateTime.Now,
                                ValorTraslado = BalanceFinancieroTrasladoValor.ValorTraslado,
                                RegistroCompleto = BalanceFinancieroTrasladoValor.ValorTraslado != null
                            });
                }
            }
        }
        #endregion

        private async Task<bool> RegistroCompletoBalanceFinanciero(BalanceFinanciero balanceFinanciero)
        {
            BalanceFinanciero balanceFinancieroOld = await _context.BalanceFinanciero.Where(r => r.BalanceFinancieroId == balanceFinanciero.BalanceFinancieroId).FirstOrDefaultAsync();
            bool state = false;
            if (balanceFinanciero != null)
            {
                if (balanceFinanciero.RequiereTransladoRecursos == false)
                    state = true;
                else
                    state = balanceFinanciero.RegistroCompleto;
            }
            return state;
        }

        public async Task<Respuesta> ApproveBalance(int pProyectoId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Aprobar_Balance_Financiero, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                BalanceFinanciero balanceFinanciero =
                                                    _context.BalanceFinanciero
                                                    .Where(r => r.ProyectoId == pProyectoId)
                                                    .FirstOrDefault();

                _context.Set<BalanceFinanciero>()
                        .Where(b => b.ProyectoId == pProyectoId)
                        .Update(b => new BalanceFinanciero
                        {
                            FechaAprobacion = DateTime.Now,
                            EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.Con_balance_aprobado,
                            UsuarioModificacion = pUsuario,
                            FechaModificacion = DateTime.Now, 
                        });
                 
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "APROBAR BALANCE FINANCIERO")
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

        #endregion 
    }
}
