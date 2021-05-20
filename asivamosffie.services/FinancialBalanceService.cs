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
        public async Task<Respuesta> ChangeStatudBalanceFinanciero(BalanceFinanciero pBalanceFinanciero)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Balance_Financiero, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                _context.Set<BalanceFinanciero>()
                        .Where(b => b.BalanceFinancieroId == pBalanceFinanciero.BalanceFinancieroId)
                        .Update(b => new BalanceFinanciero
                        {
                            EstadoBalanceCodigo = pBalanceFinanciero.EstadoBalanceCodigo,
                            UsuarioModificacion = pBalanceFinanciero.UsuarioModificacion,
                            FechaModificacion = DateTime.Now
                        });

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.OperacionExitosa, idAccion, pBalanceFinanciero.UsuarioCreacion, "CAMBIAR ESTADO BALANCE FINANCIERO")
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

        public async Task<Respuesta> ChangeStatudBalanceFinancieroTraslado(BalanceFinancieroTraslado pBalanceFinancieroTraslado)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Traslado_Balance_Financiero, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                _context.Set<BalanceFinancieroTraslado>()
                        .Where(b => b.BalanceFinancieroTrasladoId == pBalanceFinancieroTraslado.BalanceFinancieroTrasladoId)
                        .Update(b => new BalanceFinancieroTraslado
                        {
                            EstadoCodigo = pBalanceFinancieroTraslado.EstadoCodigo,
                            UsuarioModificacion = pBalanceFinancieroTraslado.UsuarioModificacion,
                            FechaModificacion = DateTime.Now
                        });

                switch (pBalanceFinancieroTraslado.EstadoCodigo)
                {
                    case ConstanCodigoEstadoBalanceFinancieroTraslado.Traslado_Aprobado:

                        GetRestaurarFuentesPorAportante(pBalanceFinancieroTraslado);
                        break;

                    case ConstanCodigoEstadoBalanceFinancieroTraslado.Anulado:

                        break;

                    case ConstanCodigoEstadoBalanceFinancieroTraslado.Notificado_a_fiduciaria:

                        break;
                }

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.OperacionExitosa, idAccion, pBalanceFinancieroTraslado.UsuarioCreacion, "CAMBIAR ESTADO BALANCE FINANCIERO TRASLADO")
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.Error, idAccion, pBalanceFinancieroTraslado.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        private void GetRestaurarFuentesPorAportante(BalanceFinancieroTraslado pBalanceFinancieroTraslado)
        {
            pBalanceFinancieroTraslado.BalanceFinancieroTrasladoValor =
                _context.BalanceFinancieroTrasladoValor
                                                       .Where(r => r.BalanceFinancieroTrasladoId == pBalanceFinancieroTraslado.BalanceFinancieroTrasladoId)
                                                       .Include(r => r.OrdenGiroDetalleTerceroCausacionAportante)
                                                       .Include(r => r.OrdenGiroDetalleDescuentoTecnicaAportante)
                                                       .Include(r => r.OrdenGiroDetalleDescuentoTecnica)
                                                       .ToList();


            foreach (var BalanceFinancieroTrasladoValor in pBalanceFinancieroTraslado.BalanceFinancieroTrasladoValor)
            {
                switch (BalanceFinancieroTrasladoValor.TipoTrasladoCodigo)
                {

                    case ConstantCodigoTipoTrasladoCodigo.Aportante_Tercero_Causacion:

                        break;

                    case ConstantCodigoTipoTrasladoCodigo.Descuento_Tercero_Causacion:

                        break;

                    case ConstantCodigoTipoTrasladoCodigo.Descuento_Direccion_Tecnica:

                        break;

                }
            }
        }

         
        //private bool GetTrasladarRecursosxAportantexFuente(int pAportanteId , int pFuenteFinanciacionId )
        //{
        //    GestionFuenteFinanciacion gestionFuenteFinanciacion
                 

        //}

        public async Task<List<VProyectosBalance>> GridBalance()
        {
            return await _context.VProyectosBalance
                                 .OrderByDescending(r => r.FechaTerminacionProyecto)
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
                                                       .Include(r => r.BalanceFinanciero)
                                                          .ThenInclude(r => r.BalanceFinancieroTraslado)
                                                              .ThenInclude(r => r.BalanceFinancieroTrasladoValor)
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
                numeroContratoInterventoria = numeroContratoInterventoria,
                proyecto.BalanceFinanciero
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
                        VOrdenGiro vOrdenGiro = (ListOrdenGiro.Where(o => o.OrdenGiroId == item.OrdenGiroId).FirstOrDefault());

                        VOrdenGiro.Add(vOrdenGiro);
                    }
                }
                return (VOrdenGiro.Select(v => new
                {
                    v.FechaPagoFiduciaria,
                    v.NumeroContrato,
                    v.FechaAprobacionFinanciera,
                    v.TipoSolicitud,
                    v.NumeroSolicitudOrdenGiro,
                    v.OrdenGiroId,
                    v.SolicitudPagoId
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
                    v.FechaPagoFiduciaria,
                    v.NumeroContrato,
                    v.FechaAprobacionFinanciera,
                    v.TipoSolicitud,
                    v.NumeroSolicitudOrdenGiro,
                    v.OrdenGiroId,
                    v.SolicitudPagoId
                }).ToList());
            }
        }

        public async Task<dynamic> GetOrdenGiroByNumeroOrdenGiro(string pTipoSolicitudCodigo, string pNumeroOrdenGiro, string pLLaveMen)
        {
            return (
               _context.OrdenGiro
                .Where(r => r.NumeroSolicitud == pNumeroOrdenGiro)
                .Include(r => r.SolicitudPago)
                .ThenInclude(r => r.Contrato)
                .ThenInclude(r => r.Contratacion).Select(v =>
             new
             {
                 FechaAprobacionFinanciera = v.SolicitudPago.FirstOrDefault().FechaRegistroCompletoValidacionFinanciera,
                 TipoSolicitud = v.SolicitudPago.FirstOrDefault().Contrato.Contratacion.TipoSolicitudCodigo,
                 NumeroSolicitudOrdenGiro = v.NumeroSolicitud,
                 v.OrdenGiroId,
                 v.SolicitudPagoId,
                 OrdenGiro = v,
                 SolicitudPago = v.SolicitudPago,
                 Contrato = v.SolicitudPago.FirstOrDefault().Contrato
             }));
        }

        public async Task<BalanceFinanciero> GetBalanceFinanciero(int pProyectoId)
        {
            BalanceFinanciero balanceFinanciero = await _context.BalanceFinanciero
                                                    .Where(r => r.ProyectoId == pProyectoId)
                                                    .Include(r => r.BalanceFinancieroTraslado)
                                                          .ThenInclude(r => r.BalanceFinancieroTrasladoValor)
                                                    .FirstOrDefaultAsync();

            foreach (var BalanceFinancieroTraslado in balanceFinanciero.BalanceFinancieroTraslado)
            {
                OrdenGiro OrdenGiro = _context.OrdenGiro
                    .Where(o => o.OrdenGiroId == BalanceFinancieroTraslado.OrdenGiroId)
                    .Include(r => r.SolicitudPago).ThenInclude(c => c.Contrato).ThenInclude(r => r.Contratacion)
                    .FirstOrDefault();

                BalanceFinancieroTraslado.NumeroContrato = OrdenGiro?.SolicitudPago?.FirstOrDefault()?.Contrato?.NumeroContrato;
                BalanceFinancieroTraslado.NumeroOrdenGiro = OrdenGiro.NumeroSolicitud;
                BalanceFinancieroTraslado.TablaDRP = GetDrpContrato(OrdenGiro.SolicitudPago.FirstOrDefault());
            }
            return balanceFinanciero;
        }

        private List<TablaDRP> GetDrpContrato(SolicitudPago SolicitudPago)
        {
            String strTipoSolicitud = SolicitudPago.Contrato.Contratacion.TipoSolicitudCodigo;
            List<TablaDRP> ListTablaDrp = new List<TablaDRP>();

            decimal ValorFacturado = SolicitudPago?.OrdenGiro?.TieneTraslado == false ? SolicitudPago?.OrdenGiro?.ValorNetoGiro ?? 0 : SolicitudPago?.OrdenGiro?.ValorNetoGiroTraslado ?? 0;

            List<VRpsPorContratacion> vRpsPorContratacion =
                                                           _context.VRpsPorContratacion
                                                           .Where(c => c.ContratacionId == SolicitudPago.Contrato.ContratacionId)
                                                           .OrderBy(C => C.ContratacionId)
                                                           .ToList();
            int Enum = 1;
            foreach (var DPR in vRpsPorContratacion)
            {
                ValorFacturado = (DPR.ValorSolicitud - ValorFacturado) > 0 ? (DPR.ValorSolicitud - ValorFacturado) : DPR.ValorSolicitud;

                ListTablaDrp.Add(new TablaDRP
                {
                    Enum = Enum,
                    NumeroDRP = DPR.NumeroDrp,
                    Valor = '$' + String.Format("{0:n0}", DPR.ValorSolicitud),
                    Saldo = '$' + String.Format("{0:n0}", ValorFacturado)
                });
                Enum++;
            }
            return ListTablaDrp;
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
                        tablaOrdenGiroValorTotal = GetTablaOrdenGiroValorTotal(contrato.SolicitudPago),
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

        private object GetTablaOrdenGiroValorTotal(ICollection<SolicitudPago> pListSolicitudPago)
        {
            List<dynamic> TablaOrdenesGiro = new List<dynamic>();

            if (pListSolicitudPago.Count() > 0)
                pListSolicitudPago = pListSolicitudPago.Where(s => s.OrdenGiro.RegistroCompletoTramitar == true).ToList();

            foreach (var SolicitudPago in pListSolicitudPago)
            {
                string NombreContratista = _context.SolicitudPago
                     .Where(r => r.SolicitudPagoId == SolicitudPago.SolicitudPagoId)
                     .Include(r => r.Contrato)
                        .ThenInclude(r => r.Contratacion)
                            .ThenInclude(r => r.Contratista)
                     .AsNoTracking()
                     .FirstOrDefault().Contrato.Contratacion.Contratista.Nombre;

                TablaOrdenesGiro.Add(
                    new
                    {
                        NumeroOrdenGiro = SolicitudPago.OrdenGiro.NumeroSolicitud,
                        Contratista = NombreContratista,
                        Facturado = SolicitudPago.OrdenGiro.ValorNetoGiro,
                        AnsAplicado = 0,
                        ReteGarantia = 0,
                        OtrosDescuentos = _context.VDescuentosOdgxFuenteFinanciacionXaportante.Where(o => o.OrdenGiroId == (int)SolicitudPago.OrdenGiroId).Sum(r => r.ValorDescuento) ?? 0,
                        ApagarAntesImpuestos = 0,
                        SolicitudId = SolicitudPago.SolicitudPagoId,
                        OrdenGiro = SolicitudPago.OrdenGiroId
                    });
            }
            return TablaOrdenesGiro;
        }
        #endregion

        #region C R U D

        #region create
        public async Task<Respuesta> ValidateCompleteBalanceFinanciero(int pBalanceFinancieroTrasladoId, bool pEstaCompleto)
        {
            await _context.Set<BalanceFinancieroTraslado>()
                      .Where(b => b.BalanceFinancieroTrasladoId == pBalanceFinancieroTrasladoId)
                      .UpdateAsync
                      (b => new BalanceFinancieroTraslado
                      {
                          RegistroCompleto = pEstaCompleto
                      });

            //Validar registro Completo BalanceFinanciero
            BalanceFinanciero balanceFinanciero = _context.BalanceFinanciero
                .Include(b => b.BalanceFinancieroTraslado)
                .Where(b => b.BalanceFinancieroTraslado
                  .Any(r => r.BalanceFinancieroTrasladoId == pBalanceFinancieroTrasladoId))
                .AsNoTracking()
                .FirstOrDefault();

            string strEstadoBalanceCodigo = balanceFinanciero.EstadoBalanceCodigo;

            bool BlRegistroCompleto = balanceFinanciero.BalanceFinancieroTraslado.All(r => r.RegistroCompleto == true);
            if (BlRegistroCompleto)
                strEstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.Con_balance_validado;


                await _context.Set<BalanceFinanciero>()
                              .Where(b => b.BalanceFinancieroId == balanceFinanciero.BalanceFinancieroId)
                              .UpdateAsync (b => new BalanceFinanciero
                                          {
                                              EstadoBalanceCodigo = strEstadoBalanceCodigo,
                                              RegistroCompleto = pEstaCompleto
                                          });

            return new Respuesta();
        }

        public async Task<Respuesta> CreateEditBalanceFinanciero(BalanceFinanciero pBalanceFinanciero)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Balance_Financiero, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            try
            {
                // BalanceFinanciero balanceFinanciero = _context.BalanceFinanciero.Where(r => r.ProyectoId == pBalanceFinanciero.ProyectoId).FirstOrDefault();

                if (pBalanceFinanciero.RequiereTransladoRecursos == false)
                {
                    pBalanceFinanciero.EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.Con_balance_validado;
                    pBalanceFinanciero.RegistroCompleto = true;
                }
                else
                {
                    //pBalanceFinanciero.RegistroCompleto = await RegistroCompletoBalanceFinanciero(pBalanceFinanciero);
                    //if (pBalanceFinanciero.RegistroCompleto == false)
                    //    pBalanceFinanciero.EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.En_proceso_de_validacion;
                    //else
                    //    pBalanceFinanciero.EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.Con_balance_validado;

                    pBalanceFinanciero.EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.Con_necesidad_de_traslado;
                }

                if (pBalanceFinanciero.BalanceFinancieroId == 0)
                {
                    pBalanceFinanciero.FechaCreacion = DateTime.Now;
                    _context.BalanceFinanciero.Add(pBalanceFinanciero);
                }
                else
                {
                    await _context.Set<BalanceFinanciero>().Where(r => r.BalanceFinancieroId == pBalanceFinanciero.BalanceFinancieroId)
                                                                   .UpdateAsync(r => new BalanceFinanciero()
                                                                   {
                                                                       FechaModificacion = DateTime.Now,
                                                                       UsuarioModificacion = pBalanceFinanciero.UsuarioCreacion,
                                                                       RequiereTransladoRecursos = pBalanceFinanciero.RequiereTransladoRecursos,
                                                                       JustificacionTrasladoAportanteFuente = pBalanceFinanciero.JustificacionTrasladoAportanteFuente,
                                                                       UrlSoporte = pBalanceFinanciero.UrlSoporte,
                                                                       RegistroCompleto = pBalanceFinanciero.RegistroCompleto,
                                                                       EstadoBalanceCodigo = pBalanceFinanciero.EstadoBalanceCodigo
                                                                   });

                    CreateEditBalanceFinancieroTraslado(pBalanceFinanciero.BalanceFinancieroTraslado, pBalanceFinanciero.UsuarioCreacion);
                }
                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.OperacionExitosa, idAccion, pBalanceFinanciero.UsuarioCreacion, !pBalanceFinanciero.FechaCreacion.HasValue ? "CREAR BALANCE FINANCIERO" : "ACTUALIZAR BALANCE FINANCIERO")
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

        private void CreateEditBalanceFinancieroTraslado(ICollection<BalanceFinancieroTraslado> ListBalanceFinancieroTraslado, string pAuthor)
        {
            foreach (var BalanceFinancieroTraslado in ListBalanceFinancieroTraslado)
            {
                if (BalanceFinancieroTraslado.BalanceFinancieroTrasladoId == 0)
                {
                    BalanceFinancieroTraslado.FechaCreacion = DateTime.Now;
                    BalanceFinancieroTraslado.UsuarioCreacion = pAuthor;
                    BalanceFinancieroTraslado.Eliminado = false;
                    BalanceFinancieroTraslado.EstadoCodigo = ConstanCodigoEstadoTraslado.Con_registro;
                    BalanceFinancieroTraslado.ValorTraslado = BalanceFinancieroTraslado.BalanceFinancieroTrasladoValor.Sum(r => r.ValorTraslado);
                    BalanceFinancieroTraslado.NumeroTraslado = _commonService.EnumeradorTrasladoBalanceFinanciero();
                    _context.BalanceFinancieroTraslado.Add(BalanceFinancieroTraslado);
                }
                else
                {
                    _context.Set<BalanceFinancieroTraslado>()
                          .Where(r => r.BalanceFinancieroTrasladoId == BalanceFinancieroTraslado.BalanceFinancieroTrasladoId)
                          .Update(r => new BalanceFinancieroTraslado
                          {
                              UsuarioModificacion = pAuthor,
                              FechaModificacion = DateTime.Now,
                              RegistroCompleto = BalanceFinancieroTraslado.RegistroCompleto,
                              ValorTraslado = BalanceFinancieroTraslado.BalanceFinancieroTrasladoValor.Sum(r => r.ValorTraslado)
                          });
                }

                CreateEditBalanceFinancieroTrasladoValor(BalanceFinancieroTraslado.BalanceFinancieroTrasladoValor, pAuthor);

                _context.Set<OrdenGiro>()
                             .Where(o => o.OrdenGiroId == BalanceFinancieroTraslado.OrdenGiroId)
                             .Update(o => new OrdenGiro
                             {
                                 ValorNetoGiroTraslado = BalanceFinancieroTraslado.BalanceFinancieroTrasladoValor.Sum(r => r.ValorTraslado) ?? 0,
                                 TieneTraslado = true
                             });
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
                    return true;
                else
                    return balanceFinanciero.RegistroCompleto;
            }
            return state;
        }

        public async Task<Respuesta> ApproveBalance(int pProyectoId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Aprobar_Balance_Financiero, (int)EnumeratorTipoDominio.Acciones);

            try
            {
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

        public async Task<dynamic> GetVerDetalleOrdenGiro(int pSolicitudPagoId)
        {
            SolicitudPago solicitudPago = _context.SolicitudPago
                .Where(r => r.SolicitudPagoId == pSolicitudPagoId)
                .Include(r => r.Contrato)
                .ThenInclude(r => r.Contratacion).FirstOrDefault();

            List<dynamic> OrdenGiro = new List<dynamic>();

            return OrdenGiro;
        }

        private string ReplaceTemplate(int pBalanceFinancieroTrasladoId)
        {
            BalanceFinancieroTraslado balanceFinancieroTraslado = _context.BalanceFinancieroTraslado
                .Where(r => r.BalanceFinancieroTrasladoId == pBalanceFinancieroTrasladoId)
                .Include(r => r.OrdenGiro)
                .Include(r => r.BalanceFinanciero)
                .ThenInclude(r => r.Proyecto)
                .FirstOrDefault();

            return string.Empty;
        }
        #endregion 
    }
}
