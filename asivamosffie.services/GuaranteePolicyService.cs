using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class GuaranteePolicyService : IGuaranteePolicyService
    {
        private readonly ICommonService _commonService;

        private readonly devAsiVamosFFIEContext _context;

        public GuaranteePolicyService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        #region Opt
        public async Task<List<VGestionarGarantiasPolizas>> ListGrillaContratoGarantiaPolizaOptz(string pEstadoCodigo)
        {
            if (string.IsNullOrEmpty(pEstadoCodigo))
                return await _context.VGestionarGarantiasPolizas.OrderByDescending(r => r.ContratoPolizaId).ToListAsync();
            else
                return await _context.VGestionarGarantiasPolizas.Where(v => v.EstadoPolizaCodigo == pEstadoCodigo).OrderByDescending(r => r.ContratoPolizaId).ToListAsync();
        }

        public async Task<Contrato> GetContratoByContratoId(int pContratoId)
        {
            return await _context.Contrato
                                        .Where(c => c.ContratoId == pContratoId)
                                        .Include(c => c.Contratacion).ThenInclude(d => d.DisponibilidadPresupuestal)
                                        .Include(c => c.Contratacion).ThenInclude(c => c.Contratista)
                                        .Include(p => p.ContratoPoliza).ThenInclude(p => p.PolizaGarantiaActualizacion)
                                        .Include(p => p.ContratoPoliza).ThenInclude(p => p.PolizaObservacion).ThenInclude(u => u.ResponsableAprobacion)
                                        .Include(p => p.ContratoPoliza).ThenInclude(p => p.PolizaGarantia)
                                        .Include(p => p.ContratoPoliza).ThenInclude(p => p.PolizaListaChequeo)
                                        .FirstOrDefaultAsync();
        }

        public async Task<Respuesta> CreateEditContratoPoliza(Contrato pContrato)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Contrato_Poliza, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                foreach (var ContratoPoliza in pContrato.ContratoPoliza)
                {
                    if (ContratoPoliza.ContratoPolizaId > 0)
                    {
                        await _context.Set<ContratoPoliza>()
                                 .Where(c => c.ContratoPolizaId == pContrato.ContratoPoliza.FirstOrDefault().ContratoPolizaId)
                                 .UpdateAsync(c => new ContratoPoliza
                                 {
                                     EstadoPolizaCodigo = ContratoPoliza.EstadoPolizaCodigo,
                                     UsuarioModificacion = pContrato.UsuarioCreacion,
                                     FechaModificacion = DateTime.Now,
                                     RegistroCompleto = ValidarRegistroCompletoContratoPoliza(ContratoPoliza),
                                     NombreAseguradora = ContratoPoliza.NombreAseguradora,
                                     NumeroPoliza = ContratoPoliza.NumeroPoliza,
                                     NumeroCertificado = ContratoPoliza.NumeroCertificado,
                                     FechaExpedicion = ContratoPoliza.FechaExpedicion
                                 });
                    }
                    else
                    {
                        ContratoPoliza.FechaCreacion = DateTime.Now;
                        ContratoPoliza.Eliminado = false;
                        _context.ContratoPoliza.Add(ContratoPoliza);
                    }

                    CreateEditPolizaGarantia(ContratoPoliza.PolizaGarantia, pContrato.UsuarioCreacion);
                    CreateEditPolizaObservacion(ContratoPoliza.PolizaObservacion, pContrato.UsuarioCreacion);
                    CreateEditPolizaListaChequeo(ContratoPoliza.PolizaListaChequeo, pContrato.UsuarioCreacion);
                }
                return
                       new Respuesta
                       {
                           IsSuccessful = true,
                           IsException = false,
                           IsValidation = false,
                           Code = ConstantMessagesContratoPoliza.OperacionExitosa,
                           Message =
                           await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                                                           (int)enumeratorMenu.GestionarGarantias,
                                                                                           ConstantMessagesContratoPoliza.OperacionExitosa,
                                                                                           idAccion,
                                                                                           pContrato.UsuarioCreacion,
                                                                                           ConstantCommonMessages.GuaranteePolicies.CREAR_EDITAR
                                                                                       )
                       };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsValidation = false,
                    Code = ConstantMessagesContratoPoliza.ErrorInterno,
                    Message =
                           await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                                                           (int)enumeratorMenu.GestionarGarantias,
                                                                                           ConstantMessagesContratoPoliza.ErrorInterno,
                                                                                           idAccion,
                                                                                           pContrato.UsuarioCreacion,
                                                                                           ConstantCommonMessages.GuaranteePolicies.ERROR
                                                                                       )
                };
            }
        }

        private void CreateEditPolizaObservacion(ICollection<PolizaObservacion> pListPolizaObservacion, string pAuthor)
        {
            foreach (var PolizaObservacion in pListPolizaObservacion)
            {
                if (PolizaObservacion.PolizaObservacionId == 0)
                {
                    PolizaObservacion.UsuarioCreacion = pAuthor;
                    PolizaObservacion.FechaCreacion = DateTime.Now;
                    PolizaObservacion.Eliminado = false;
                    PolizaObservacion.RegistroCompleto = ValidarRegistroCompletoPolizaObservacion(PolizaObservacion);
                    _context.PolizaObservacion.Add(PolizaObservacion);
                }
                else
                {
                    _context.Set<PolizaObservacion>()
                            .Where(p => p.PolizaObservacionId == PolizaObservacion.PolizaObservacionId)
                            .Update(p => new PolizaObservacion
                            {
                                UsuarioModificacion = pAuthor,
                                FechaModificacion = DateTime.Now,
                                RegistroCompleto = ValidarRegistroCompletoPolizaObservacion(PolizaObservacion),
                                Observacion = PolizaObservacion.Observacion,
                                FechaRevision = PolizaObservacion.FechaRevision,
                                EstadoRevisionCodigo = PolizaObservacion.EstadoRevisionCodigo,
                                FechaAprobacion = PolizaObservacion.FechaAprobacion,
                                ResponsableAprobacionId = PolizaObservacion.ResponsableAprobacionId
                            });
                }
            }
        }

        private void CreateEditPolizaListaChequeo(ICollection<PolizaListaChequeo> pListpolizaListaChequeo, string usuarioCreacion)
        {
            foreach (var PolizaListaChequeo in pListpolizaListaChequeo)
            {
                if (PolizaListaChequeo.PolizaListaChequeoId > 0)
                {
                    _context.Set<PolizaListaChequeo>()
                            .Where(p => p.PolizaListaChequeoId == PolizaListaChequeo.PolizaListaChequeoId)
                            .Update(p => new PolizaListaChequeo
                            {
                                UsuarioModificacion = usuarioCreacion,
                                FechaModificacion = DateTime.Now,
                                RegistroCompleto = ValidarRegistroCompletoListaChequeo(PolizaListaChequeo),

                                CumpleDatosAseguradoBeneficiario = PolizaListaChequeo.CumpleDatosAseguradoBeneficiario,
                                CumpleDatosBeneficiarioGarantiaBancaria = PolizaListaChequeo.CumpleDatosBeneficiarioGarantiaBancaria,
                                CumpleDatosTomadorAfianzado = PolizaListaChequeo.CumpleDatosTomadorAfianzado,
                                TieneReciboPagoDatosRequeridos = PolizaListaChequeo.TieneReciboPagoDatosRequeridos,
                                TieneCondicionesGeneralesPoliza = PolizaListaChequeo.TieneCondicionesGeneralesPoliza
                            });
                }
                else
                {
                    PolizaListaChequeo.FechaCreacion = DateTime.Now;
                    PolizaListaChequeo.Eliminado = false;
                    PolizaListaChequeo.RegistroCompleto = ValidarRegistroCompletoListaChequeo(PolizaListaChequeo);
                    _context.PolizaListaChequeo.Add(PolizaListaChequeo);
                }
            }
        }

        private void CreateEditPolizaGarantia(ICollection<PolizaGarantia> pListPolizaGarantia, string pAuthor)
        {
            foreach (var PolizaGarantia in pListPolizaGarantia)
            {
                if (PolizaGarantia.PolizaGarantiaId > 0)
                {
                    _context.Set<PolizaGarantia>()
                            .Where(p => p.PolizaGarantiaId == PolizaGarantia.PolizaGarantiaId)
                            .Update(p => new PolizaGarantia
                            {
                                UsuarioModificacion = pAuthor,
                                FechaModificacion = DateTime.Now,
                                RegistroCompleto = ValidarRegistroCompletoPolizaGarantia(PolizaGarantia),

                                TipoGarantiaCodigo = PolizaGarantia.TipoGarantiaCodigo,
                                EsIncluidaPoliza = PolizaGarantia.EsIncluidaPoliza,
                                ValorAmparo = PolizaGarantia.ValorAmparo,
                                VigenciaAmparo = PolizaGarantia.VigenciaAmparo,
                                Vigencia = PolizaGarantia.Vigencia
                            });
                }
                else
                {
                    PolizaGarantia.UsuarioCreacion = pAuthor;
                    PolizaGarantia.FechaCreacion = DateTime.Now;
                    PolizaGarantia.Eliminado = false;
                    PolizaGarantia.RegistroCompleto = ValidarRegistroCompletoPolizaGarantia(PolizaGarantia);
                    _context.PolizaGarantia.Add(PolizaGarantia);
                }
            }
        }

        public bool ValidarRegistroCompletoContratoPoliza(ContratoPoliza contratoPoliza)
        {
            if (
                   string.IsNullOrEmpty(contratoPoliza.NombreAseguradora.ToString())
                || string.IsNullOrEmpty(contratoPoliza.NumeroPoliza.ToString())
                || string.IsNullOrEmpty(contratoPoliza.NumeroCertificado.ToString())
                || string.IsNullOrEmpty(contratoPoliza.FechaExpedicion.ToString())
                || string.IsNullOrEmpty(contratoPoliza.EstadoPolizaCodigo.ToString())
                )
                return false;

            if (contratoPoliza.PolizaListaChequeo.Count() == 0)
                return false;

            foreach (var PolizaListaChequeo in contratoPoliza.PolizaListaChequeo)
            {
                if (!ValidarRegistroCompletoListaChequeo(PolizaListaChequeo))
                    return false;
            }
            if (contratoPoliza.PolizaGarantia.Count() == 0)
                return false;

            foreach (var PolizaGarantia in contratoPoliza.PolizaGarantia)
            {
                if (!ValidarRegistroCompletoPolizaGarantia(PolizaGarantia))
                    return false;
            }

            if (contratoPoliza.PolizaObservacion.Count() == 0)
                return false;

            foreach (var PolizaObservacion in contratoPoliza.PolizaObservacion)
            {
                if (!ValidarRegistroCompletoPolizaObservacion(PolizaObservacion))
                    return false;
            }

            return true;
        }

        private bool ValidarRegistroCompletoListaChequeo(PolizaListaChequeo pPolizaListaChequeo)
        {
            if (
                  !pPolizaListaChequeo.CumpleDatosAseguradoBeneficiario.HasValue
               || !pPolizaListaChequeo.CumpleDatosBeneficiarioGarantiaBancaria.HasValue
               || !pPolizaListaChequeo.CumpleDatosTomadorAfianzado.HasValue
               || !pPolizaListaChequeo.TieneReciboPagoDatosRequeridos.HasValue
               || !pPolizaListaChequeo.TieneCondicionesGeneralesPoliza.HasValue
                ) return false;

            if (
                  pPolizaListaChequeo.CumpleDatosAseguradoBeneficiario == false
               || pPolizaListaChequeo.CumpleDatosBeneficiarioGarantiaBancaria == false
               || pPolizaListaChequeo.CumpleDatosTomadorAfianzado == false
               || pPolizaListaChequeo.TieneReciboPagoDatosRequeridos == false
               || pPolizaListaChequeo.TieneCondicionesGeneralesPoliza == false
                ) return false;
            return true;
        }

        private bool ValidarRegistroCompletoPolizaGarantia(PolizaGarantia polizaGarantia)
        {
            if (
                string.IsNullOrEmpty(polizaGarantia.TipoGarantiaCodigo)
                || polizaGarantia.ValorAmparo == null
                || !polizaGarantia.VigenciaAmparo.HasValue
                || !polizaGarantia.Vigencia.HasValue
                ) return false;
            return true;
        }

        private bool ValidarRegistroCompletoPolizaObservacion(PolizaObservacion polizaObservacion)
        {
            if (polizaObservacion.EstadoRevisionCodigo == ConstanCodigoEstadoRevisionPoliza.Devuelta)
                return false;

            if (!polizaObservacion.FechaAprobacion.HasValue
                || polizaObservacion.ResponsableAprobacionId == 0
                ) return false;

            return true;
        }

        public async Task<Respuesta> ChangeStatusEstadoPoliza(ContratoPoliza pContratoPoliza)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Gestion_Poliza, (int)EnumeratorTipoDominio.Acciones);

            try
            { 
                _context.Set<ContratoPoliza>()
                        .Where(p => p.ContratoPolizaId == pContratoPoliza.ContratoPolizaId)
                        .Update(p => new ContratoPoliza
                        { 
                            FechaModificacion = DateTime.Now,
                            EstadoPolizaCodigo = pContratoPoliza.EstadoPolizaCodigo 
                        });
               
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContratoPoliza.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.OperacionExitosa, idAccion, pContratoPoliza.UsuarioModificacion, ConstantCommonMessages.GuaranteePolicies.CAMBIAR_ESTADO)
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContratoPoliza.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.Error, idAccion, pContratoPoliza.UsuarioModificacion, ex.InnerException.ToString())
                };
            }

        }


        #endregion

        #region Old
        public async Task<List<PolizaGarantia>> GetListPolizaGarantiaByContratoPolizaId(int pContratoPolizaId)
        {
            return await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == pContratoPolizaId).ToListAsync();
        }

        public async Task<NotificacionMensajeGestionPoliza> GetNotificacionContratoPolizaByIdContratoId(int pContratoId)
        {
            NotificacionMensajeGestionPoliza msjNotificacion = new NotificacionMensajeGestionPoliza();

            Contrato contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            ContratoPoliza contratoPoliza = new ContratoPoliza();
            if (contrato != null)
                contratoPoliza = _context.ContratoPoliza.Where(r => r.ContratoId == pContratoId)
                                                           .OrderByDescending(r => r.ContratoPolizaId)
                                                           .FirstOrDefault();

            PolizaObservacion polizaObservacion = null;

            if (contratoPoliza != null)
            {
                List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).ToListAsync();

                contratoPoliza.PolizaGarantia = contratoPolizaGarantia;

                polizaObservacion = _context.PolizaObservacion.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).FirstOrDefault();

                List<PolizaObservacion> contratoPolizaObservacion = await _context.PolizaObservacion.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).ToListAsync();

                contratoPoliza.PolizaObservacion = contratoPolizaObservacion;

                msjNotificacion.NombreAseguradora = contratoPoliza.NombreAseguradora;
                msjNotificacion.NumeroPoliza = contratoPoliza.NumeroPoliza;

                msjNotificacion.FechaAprobacion = contratoPoliza.FechaAprobacion != null ? Convert.ToDateTime(contratoPoliza.FechaAprobacion).ToString("dd/MM/yyyy") : contratoPoliza.FechaAprobacion.ToString();

                if (polizaObservacion != null)
                {
                    msjNotificacion.EstadoRevision = polizaObservacion.EstadoRevisionCodigo;

                    msjNotificacion.FechaRevisionDateTime = polizaObservacion.FechaRevision;

                }
            }

            return msjNotificacion;
        }

        public async Task<ContratoPoliza> GetContratoPolizaByIdContratoId(int pContratoId)
        {
            Contrato contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            ContratoPoliza contratoPoliza = new ContratoPoliza();

            if (contrato != null)
                contratoPoliza = _context.ContratoPoliza.Where(r => r.ContratoId == pContratoId)
                                                        .OrderByDescending(r => r.ContratoPolizaId).FirstOrDefault();

            PolizaObservacion polizaObservacion = null;

            if (contratoPoliza != null)
            {
                contratoPoliza.ContratacionId = 0;
                List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).ToListAsync();

                contratoPoliza.PolizaGarantia = contratoPolizaGarantia;

                polizaObservacion = _context.PolizaObservacion.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).FirstOrDefault();

                List<PolizaObservacion> contratoPolizaObservacion = await _context.PolizaObservacion.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).ToListAsync();

                contratoPoliza.PolizaObservacion = contratoPolizaObservacion;

                if (contrato != null)
                {
                    contratoPoliza = _context.ContratoPoliza.Where(r => r.ContratoId == pContratoId)
                                                            .OrderByDescending(r => r.ContratoPolizaId)
                                                            .FirstOrDefault();

                    if (contratoPoliza != null)
                        contratoPoliza.ContratacionId = contrato.ContratacionId;
                }
            }

            return contratoPoliza;
        }

        public async Task<ContratoPoliza> GetContratoPolizaByIdContratoPolizaId(int pContratoPolizaId)
        {
            ContratoPoliza contratoPoliza = _context.ContratoPoliza.Where(r => r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();

            if (contratoPoliza != null)
            {
                List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == pContratoPolizaId).ToListAsync();
                contratoPoliza.PolizaGarantia = contratoPolizaGarantia;
            }

            PolizaObservacion polizaObservacion = _context.PolizaObservacion.Where(r => r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();

            if (contratoPoliza != null)
            {
                List<PolizaObservacion> contratoPolizaObservacion = await _context.PolizaObservacion.Where(r => r.ContratoPolizaId == pContratoPolizaId).ToListAsync();
                contratoPoliza.PolizaObservacion = contratoPolizaObservacion;
            }

            return contratoPoliza;
        }

        public async Task<List<PolizaObservacion>> GetListPolizaObservacionByContratoPolizaId(int pContratoPolizaId)
        {
            return await _context.PolizaObservacion.Where(r => r.ContratoPolizaId == pContratoPolizaId).ToListAsync();
        }

        public async Task<Respuesta> InsertEditPolizaGarantia(PolizaGarantia polizaGarantia)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Poliza_Garantia, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar;
            try
            {
                if (polizaGarantia != null)
                {
                    //este dato no se esta enviando por frontend, me parece que es mas facil por aca
                    //jflroez 20201124
                    //if (polizaGarantia.PolizaGarantiaId == 0)
                    var polizaGarantiaExiste = _context.PolizaGarantia.Where(x => x.TipoGarantiaCodigo == polizaGarantia.TipoGarantiaCodigo && x.ContratoPolizaId == polizaGarantia.ContratoPolizaId).FirstOrDefault();
                    if (polizaGarantiaExiste == null)
                    {
                        //Auditoria
                        strCrearEditar = "REGISTRAR POLIZA GARANTIA";
                        polizaGarantia.FechaCreacion = DateTime.Now;
                        _context.PolizaGarantia.Add(polizaGarantia);
                        _context.SaveChanges();

                    }
                    else
                    {
                        strCrearEditar = "EDIT POLIZA GARANTIA";
                        PolizaGarantia polizaGarantiaBD = null;
                        polizaGarantiaBD = polizaGarantiaExiste;

                        if (polizaGarantiaBD != null)
                        {
                            polizaGarantia.FechaModificacion = DateTime.Now;
                            polizaGarantiaBD.TipoGarantiaCodigo = polizaGarantia.TipoGarantiaCodigo;
                            polizaGarantiaBD.EsIncluidaPoliza = polizaGarantia.EsIncluidaPoliza;
                            _context.PolizaGarantia.Update(polizaGarantiaBD);

                        }
                    }

                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContratoPoliza.CreacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                                                            (int)enumeratorMenu.GestionarGarantias,
                                                                                            ConstantMessagesContratoPoliza.OperacionExitosa,
                                                                                            idAccionCrearContratoPoliza,
                                                                                            polizaGarantia.UsuarioCreacion,
                                                                                            strCrearEditar
                                                                                        )
                        };
                }
                else
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.RecursoNoEncontrado };

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.ErrorInterno, Message = ex.InnerException.ToString().Substring(0, 500) };
            }

        }

        public async Task<Respuesta> InsertEditPolizaObservacion(PolizaObservacion polizaObservacion, AppSettingsService appSettingsService)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Poliza_Observacion, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty,
            strUsuario = string.Empty;
            try
            {
                if (polizaObservacion != null)
                {
                    int ContratoPolizaId = 0;
                    if (polizaObservacion.PolizaObservacionId == 0)
                    {
                        strCrearEditar = "REGISTRAR POLIZA OBSERVACION";
                        strUsuario = polizaObservacion.UsuarioCreacion;
                        polizaObservacion.FechaCreacion = DateTime.Now;

                        _context.PolizaObservacion.Add(polizaObservacion);
                        await _context.SaveChangesAsync();
                        ContratoPolizaId = polizaObservacion.ContratoPolizaId;
                    }
                    else
                    {
                        strCrearEditar = "EDIT POLIZA OBSERVACION";
                        strUsuario = polizaObservacion.UsuarioModificacion;
                        PolizaObservacion polizaObservacionBD = _context.PolizaObservacion
                                                               .Where(r => r.PolizaObservacionId == polizaObservacion.PolizaObservacionId)
                                                               .FirstOrDefault();
                        if (polizaObservacion != null)
                        {
                            polizaObservacionBD.FechaModificacion = DateTime.Now;
                            polizaObservacionBD.Observacion = polizaObservacion.Observacion;
                            polizaObservacionBD.FechaRevision = polizaObservacion.FechaRevision;
                            polizaObservacionBD.EstadoRevisionCodigo = polizaObservacion.EstadoRevisionCodigo;
                            _context.PolizaObservacion.Update(polizaObservacionBD);
                        }
                        ContratoPolizaId = polizaObservacionBD.ContratoPolizaId;
                    }
                    Template TemplateRecoveryPassword = new Template();

                    if (polizaObservacion.EstadoRevisionCodigo == ConstanCodigoEstadoRevision.aprobada)
                        TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjFiduciariaJuridicaGestionPoliza);
                    else
                        TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjSupervisorGestionPoliza);


                    string template = TemplateRecoveryPassword.Contenido;

                    //string urlDestino = pDominio;
                    //asent/img/logo  
                    var contratopoliza = _context.ContratoPoliza.Where(x => x.ContratoPolizaId == ContratoPolizaId).
                                                                           Include(x => x.Contrato).FirstOrDefault();

                    var ListVista = ListVistaContratoGarantiaPoliza(contratopoliza.Contrato.ContratoId).Result.FirstOrDefault();

                    var fechaFirmaContrato = contratopoliza.Contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contratopoliza.Contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : "";


                    //
                    var NumeroDRP = _context.Contrato.Where(c => c.ContratacionId == contratopoliza.ContratoId)
                                                                                                  .Include(c => c.Contratacion)
                                                                                                  .ThenInclude(dp => dp.DisponibilidadPresupuestal).Select(r => r.Contratacion.DisponibilidadPresupuestal).FirstOrDefault();

                    //datos basicos generales, aplican para los 4 mensajes
                    template = template.Replace("_Tipo_Contrato_", ListVista.TipoContrato);
                    template = template.Replace("_Numero_Contrato_", ListVista.NumeroContrato);
                    template = template.Replace("_Fecha_Firma_Contrato_", ListVista.FechaFirmaContrato);
                    template = template.Replace("_Nombre_Contratista_", ListVista.NombreContratista);
                    template = template.Replace("_Valor_Contrato_", string.Format("${0:#,0}", ListVista.ValorContrato.ToString()));
                    template = template.Replace("_Plazo_", ListVista.PlazoContrato);
                    template = template.Replace("_NumeroDRP_", NumeroDRP?.FirstOrDefault()?.NumeroDrp ?? " ");
                    template = template.Replace("_LinkF_", appSettingsService.DominioFront);
                    template = template.Replace("_Estado_Revision_", _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Revision_Poliza && x.Codigo == polizaObservacion.EstadoRevisionCodigo).Select(x => x.Nombre).FirstOrDefault());
                    template = template.Replace("_Observaciones_", Helpers.Helpers.HtmlStringLimpio(polizaObservacion.Observacion));
                    template = template.Replace("_Nombre_Aseguradora_", contratopoliza.NombreAseguradora);
                    template = template.Replace("_Numero_Poliza_", contratopoliza.NumeroPoliza);

                    if (polizaObservacion.EstadoRevisionCodigo != ConstanCodigoEstadoRevision.aprobada)
                    {
                        string destinatario = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Supervisor && (bool)x.Activo && (bool)x.Usuario.Activo)
                       .Select(x => x.Usuario.Email).FirstOrDefault();//esto va a cambiar en fase 2
                        var blEnvioCorreo = Helpers.Helpers.EnviarCorreo(destinatario, "Gestión Poliza", template, appSettingsService.Sender, appSettingsService.Password, appSettingsService.MailServer, appSettingsService.MailPort);

                    }

                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContratoPoliza.OperacionExitosa,
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                                                                    (int)enumeratorMenu.GestionarGarantias,
                                                                                                    ConstantMessagesContratoPoliza.OperacionExitosa,
                                                                                                    idAccionCrearContratoPoliza,
                                                                                                    polizaObservacion.UsuarioCreacion,
                                                                                                    "REGISTRAR POLIZA OBSERVACION"
                                                                                                   )
                        };
                }
                else
                {
                    return new Respuesta
                    {
                        IsSuccessful = false,
                        IsValidation = false,
                        Data = null,
                        Code = ConstantMessagesContratoPoliza.RecursoNoEncontrado
                    };
                }

            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesContratoPoliza.ErrorInterno,
                    Message = ex.InnerException.ToString().Substring(0, 500)
                };
            }

        }

        public async Task<Respuesta> CreateEditPolizaObservacion(PolizaObservacion pPolizaObservacion, AppSettingsService appSettingsService)
        {
            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Poliza_Observacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pPolizaObservacion.PolizaObservacionId > 0)
                {
                    _context.Set<PolizaObservacion>()
                                        .Where(p => p.PolizaObservacionId == pPolizaObservacion.PolizaObservacionId)
                                                                                        .Update(r => new PolizaObservacion
                                                                                        {
                                                                                            UsuarioModificacion = pPolizaObservacion.UsuarioCreacion,
                                                                                            Observacion = pPolizaObservacion.Observacion,
                                                                                            FechaRevision = pPolizaObservacion.FechaRevision,
                                                                                            EstadoRevisionCodigo = pPolizaObservacion.EstadoRevisionCodigo,
                                                                                        });
                }
                else
                {

                    try
                    {


                        pPolizaObservacion.FechaCreacion = DateTime.Now;
                        pPolizaObservacion.Eliminado = false;
                        _context.PolizaObservacion.Add(pPolizaObservacion);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                    }
                }


                Template TemplateRecoveryPassword = new Template();

                if (pPolizaObservacion.EstadoRevisionCodigo == ConstanCodigoEstadoRevision.aprobada)
                    TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjFiduciariaJuridicaGestionPoliza);
                else
                    TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjSupervisorGestionPoliza);


                string template = TemplateRecoveryPassword.Contenido;

                //string urlDestino = pDominio;
                //asent/img/logo  
                var contratopoliza = _context.ContratoPoliza.Where(x => x.ContratoPolizaId == pPolizaObservacion.ContratoPolizaId).
                                                                       Include(x => x.Contrato).FirstOrDefault();

                var ListVista = ListVistaContratoGarantiaPoliza(contratopoliza.Contrato.ContratoId).Result.FirstOrDefault();

                var fechaFirmaContrato = contratopoliza.Contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contratopoliza.Contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : "";

                //datos basicos generales, aplican para los 4 mensajes
                template = template.Replace("_Tipo_Contrato_", ListVista.TipoContrato);
                template = template.Replace("_Numero_Contrato_", ListVista.NumeroContrato);
                template = template.Replace("_Fecha_Firma_Contrato_", ListVista.FechaFirmaContrato);
                template = template.Replace("_Nombre_Contratista_", ListVista.NombreContratista);
                template = template.Replace("_Valor_Contrato_", string.Format("${0:#,0}", ListVista.ValorContrato.ToString()));  //fomato miles .
                template = template.Replace("_Plazo_", ListVista.PlazoContrato);
                template = template.Replace("_LinkF_", appSettingsService.DominioFront);

                template = template.Replace("_Estado_Revision_", _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Revision_Poliza && x.Codigo == pPolizaObservacion.EstadoRevisionCodigo).Select(x => x.Nombre).FirstOrDefault());
                template = template.Replace("_Observaciones_", Helpers.Helpers.HtmlStringLimpio(pPolizaObservacion.Observacion));
                template = template.Replace("_Nombre_Aseguradora_", contratopoliza.NombreAseguradora);
                template = template.Replace("_Numero_Poliza_", contratopoliza.NumeroPoliza);

                if (pPolizaObservacion.EstadoRevisionCodigo != ConstanCodigoEstadoRevision.aprobada)
                {
                    string destinatario = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Supervisor && (bool)x.Activo && (bool)x.Usuario.Activo)
                   .Select(x => x.Usuario.Email).FirstOrDefault();//esto va a cambiar en fase 2
                    var blEnvioCorreo = Helpers.Helpers.EnviarCorreo(destinatario, "Gestión Poliza", template, appSettingsService.Sender, appSettingsService.Password, appSettingsService.MailServer, appSettingsService.MailPort);
                }


                return
                      new Respuesta
                      {
                          IsSuccessful = true,
                          IsException = false,
                          IsValidation = false,
                          Code = ConstantMessagesContratoPoliza.OperacionExitosa,
                          Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias,
                          ConstantMessagesContratoPoliza.OperacionExitosa,
                          idAccionCrearContratoPoliza,
                          pPolizaObservacion.UsuarioCreacion,
                          "REGISTRAR POLIZA OBSERVACION"
                          )
                      };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContratoPoliza.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                                                               (int)enumeratorMenu.GestionarGarantias,
                                                                                               ConstantMessagesContratoPoliza.Error,
                                                                                               idAccionCrearContratoPoliza,
                                                                                               pPolizaObservacion.UsuarioCreacion,
                                                                                               ex.ToString()
                                                                                              )
                };
            }


        }

        public async Task<Respuesta> EditarContratoPoliza(ContratoPoliza contratoPoliza)
        {
            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Contrato_Poliza, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (contratoPoliza != null)
                {
                    ContratoPoliza contratoPolizaBD = null;
                    contratoPolizaBD = _context.ContratoPoliza.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).FirstOrDefault();

                    bool ContratoEsDevuelto = false;

                    if (contratoPolizaBD != null)
                    {
                        Contrato contrato = _context.Contrato.Where(r => r.ContratoId == contratoPoliza.ContratoId).FirstOrDefault();

                        if (contrato != null)
                            if (contrato.EstaDevuelto != null)
                                ContratoEsDevuelto = Convert.ToBoolean(contrato.EstaDevuelto);

                        if (contratoPolizaBD.RegistroCompleto == true)
                            contratoPolizaBD.RegistroCompleto = await ValidarRegistroCompletoSeguros(contratoPoliza);

                        contratoPolizaBD.FechaModificacion = DateTime.Now;
                        contratoPolizaBD.UsuarioModificacion = contratoPoliza.UsuarioCreacion;
                        contratoPolizaBD.RegistroCompleto = ValidarRegistroCompletoContratoPoliza(contratoPoliza);

                        contratoPolizaBD.NombreAseguradora = contratoPoliza.NombreAseguradora;

                        contratoPolizaBD.NumeroPoliza = contratoPoliza.NumeroPoliza;
                        contratoPolizaBD.NumeroCertificado = contratoPoliza.NumeroCertificado;

                        contratoPolizaBD.EstadoPolizaCodigo = contratoPoliza.EstadoPolizaCodigo;
                        contratoPolizaBD.FechaExpedicion = contratoPoliza.FechaExpedicion;


                        contratoPolizaBD.IncluyeCondicionesGenerales = contratoPoliza.IncluyeCondicionesGenerales;
                        contratoPolizaBD.FechaAprobacion = contratoPoliza.FechaAprobacion;

                        LimpiarEntradasContratoPoliza(ref contratoPolizaBD);
                        _context.ContratoPoliza.Update(contratoPolizaBD);

                    }
                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContratoPoliza.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                                                            (int)enumeratorMenu.GestionarGarantias,
                                                                                            ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente,
                                                                                            idAccionCrearContratoPoliza,
                                                                                            contratoPoliza.UsuarioCreacion,
                                                                                            "EDITAR CONTRATO POLIZA"
                                                                                          )
                        };

                }
                else
                {
                    return new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.ErrorInterno, Message = ex.InnerException.ToString().Substring(0, 500) };
            }

        }

        public async Task<Respuesta> InsertContratoPoliza(ContratoPoliza contratoPoliza, AppSettingsService appSettingsService)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Contrato_Poliza, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //Valida si llega el objeto ContratoPoliza
                if (contratoPoliza == null)
                {
                    return new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesContratoPoliza.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.Error, idAccion, contratoPoliza.UsuarioCreacion, "ERROR PETICIÓN SERVICIO")
                    };
                }


                //Validar Contrato Poliza Duplicado
                if (_context.ContratoPoliza.Where(r => r.ContratoId == contratoPoliza.ContratoId).ToList().Count() > 0)
                {
                    return new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesContratoPoliza.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.ErrorDuplicarPoliza, idAccion, contratoPoliza.UsuarioCreacion, "ERROR PETICIÓN SERVICIO")
                    };
                }

                //Si Model Ok save 
                contratoPoliza.FechaCreacion = DateTime.Now;
                contratoPoliza.Eliminado = false;
                Contrato contrato = _context.Contrato.Where(r => r.ContratoId == contratoPoliza.ContratoId).FirstOrDefault();
                bool ContratoEsDevuelto = false;

                if (contrato != null)
                    if (contrato.EstaDevuelto != null)
                        ContratoEsDevuelto = Convert.ToBoolean(contrato.EstaDevuelto);

                contratoPoliza.RegistroCompleto = ValidarRegistroCompletoContratoPoliza(contratoPoliza);

                LimpiarEntradasContratoPoliza(ref contratoPoliza);
                _context.ContratoPoliza.Add(contratoPoliza);
                _context.SaveChanges();

                return await EnviarCorreosCrearPoliza(contratoPoliza, contrato, appSettingsService);
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContratoPoliza.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.Error, idAccion, contratoPoliza.UsuarioCreacion, ex.InnerException.ToString())
                };
            }
        }

        private async Task<Respuesta> EnviarCorreosCrearPoliza(ContratoPoliza contratoPoliza, Contrato contrato, AppSettingsService appSettingsService)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_Correo_Crear_Contrato_Poliza, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                string fechaFirmaContrato = string.Empty;
                string correo = string.Empty;

                VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza;

                List<Usuario> ListUsuarioCorreos = getCorreos((int)EnumeratorPerfil.Supervisor);

                int idAccionEditarContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

                objVistaContratoGarantiaPoliza = new VistaContratoGarantiaPoliza();

                List<VistaContratoGarantiaPoliza> ListVista = new List<VistaContratoGarantiaPoliza>();
                ListVista = await ListVistaContratoGarantiaPoliza(contratoPoliza.ContratacionId);

                NotificacionMensajeGestionPoliza msjNotificacion = new NotificacionMensajeGestionPoliza();
                getDataNotifMsjAseguradora(ref msjNotificacion, contratoPoliza, ref fechaFirmaContrato, ref objVistaContratoGarantiaPoliza, ListVista);


                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjSupervisorJuridicaGestionPoliza);

                string template = TemplateRecoveryPassword.Contenido;

                contrato = _context.Contrato.Where(r => r.ContratoId == objVistaContratoGarantiaPoliza.IdContrato)
                                            .Include(r => r.Contratacion)
                                                    .ThenInclude(r => r.DisponibilidadPresupuestal)
                                                                                                 .FirstOrDefault();

                fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                //datos basicos generales, aplican para los 4 mensajes
                template = template.Replace("_Tipo_Contrato_", objVistaContratoGarantiaPoliza.TipoContrato);
                template = template.Replace("_Numero_Contrato_", objVistaContratoGarantiaPoliza.NumeroContrato);
                template = template.Replace("_Fecha_Firma_Contrato_", fechaFirmaContrato); //Formato (dd/MM/aaaa)
                template = template.Replace("_Nombre_Contratista_", objVistaContratoGarantiaPoliza.NombreContratista);
                template = template.Replace("_Valor_Contrato_", string.Format("${0:#,0}", objVistaContratoGarantiaPoliza.ValorContrato.ToString()));  //fomato miles .
                template = template.Replace("_Plazo_", objVistaContratoGarantiaPoliza.PlazoContrato);
                template = template.Replace("_LinkF_", appSettingsService.DominioFront);
                template = template.Replace("_NumeroDRP_", contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault()?.NumeroDrp ?? " ");

                if (msjNotificacion != null)
                {
                    template = template.Replace("_Nombre_Aseguradora_", msjNotificacion.NombreAseguradora);
                    template = template.Replace("_Numero_Poliza_", msjNotificacion.NumeroPoliza);
                    template = template.Replace("_Fecha_Revision_", msjNotificacion.FechaRevision);
                    template = template.Replace("_Estado_Revision_", _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Revision_Poliza && x.Codigo == msjNotificacion.EstadoRevision).Select(x => x.Nombre).FirstOrDefault());
                    template = template.Replace("_Observaciones_", msjNotificacion.Observaciones);
                    template = template.Replace("_Fecha_Aprobacion_Poliza", msjNotificacion.FechaAprobacion);


                }

                bool blEnvioCorreo = true;

                foreach (var Usuario in ListUsuarioCorreos)
                {
                    if (Helpers.Helpers.EnviarCorreo(Usuario.Email, "Gestión Poliza", template, appSettingsService.Sender, appSettingsService.Password, appSettingsService.MailServer, appSettingsService.MailPort) == false)
                        blEnvioCorreo = false;
                }

                if (blEnvioCorreo)
                    return new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.CorreoEnviado };

                else
                    return new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.ErrorEnviarCorreo };

            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContratoPoliza.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.Error, idAccion, contratoPoliza.UsuarioCreacion, ex.InnerException.ToString())
                };
            }

        }

        public async Task<Respuesta> CambiarEstadoPolizaByContratoId(int pContratoId, string pCodigoNuevoEstadoPoliza, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Gestion_Poliza, (int)EnumeratorTipoDominio.Acciones);

            bool ContratoEsDevuelto = false;
            try
            {
                Contrato contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

                ContratoPoliza contratoPoliza = new ContratoPoliza();
                if (contrato != null)
                    contratoPoliza = await _commonService.GetLastContratoPolizaByContratoId(contrato.ContratoId);

                if (contratoPoliza != null)
                {
                    contratoPoliza.UsuarioModificacion = pUsuarioModifica;
                    contratoPoliza.FechaModificacion = DateTime.Now;
                    contratoPoliza.EstadoPolizaCodigo = pCodigoNuevoEstadoPoliza;

                    contrato = _context.Contrato.Where(r => r.ContratoId == contratoPoliza.ContratoId).FirstOrDefault();

                    if (contrato != null)
                        if (contrato.EstaDevuelto != null)
                            ContratoEsDevuelto = Convert.ToBoolean(contrato.EstaDevuelto);

                    contratoPoliza.RegistroCompleto = ValidarRegistroCompletoContratoPoliza(contratoPoliza);

                    if (contratoPoliza.RegistroCompleto == true)
                        contratoPoliza.RegistroCompleto = await ValidarRegistroCompletoSeguros(contratoPoliza);

                    _context.SaveChanges();

                }

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContratoPoliza.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO POLIZA")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContratoPoliza.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }


        //enviar correo estado devuelto
        public async Task<Respuesta> EnviarCorreoSupervisor(ContratoPoliza contratoPoliza, AppSettingsService settings)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_Correo_Crear_Contrato_Poliza, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                string fechaFirmaContrato = string.Empty;

                int idAccionEditarContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente, (int)EnumeratorTipoDominio.Acciones);
                VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza = new VistaContratoGarantiaPoliza();
                List<VistaContratoGarantiaPoliza> ListVista = new List<VistaContratoGarantiaPoliza>();
                ListVista = await ListVistaContratoGarantiaPoliza(0);

                NotificacionMensajeGestionPoliza msjNotificacion;
                msjNotificacion = new NotificacionMensajeGestionPoliza();

                getDataNotifMsjAseguradora(ref msjNotificacion, contratoPoliza, ref fechaFirmaContrato, ref objVistaContratoGarantiaPoliza, ListVista);


                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjSupervisorJuridicaGestionPoliza);

                string template = TemplateRecoveryPassword.Contenido;

                Contrato contrato = _context.Contrato
                                            .Where(r => r.ContratoId == objVistaContratoGarantiaPoliza.IdContrato)
                                            .Include(r => r.Contratacion)
                                                .ThenInclude(dp => dp.DisponibilidadPresupuestal)
                                                                                               .FirstOrDefault();

                fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                //datos basicos generales, aplican para los 4 mensajes
                template = template.Replace("_Tipo_Contrato_", objVistaContratoGarantiaPoliza.TipoContrato);
                template = template.Replace("_Numero_Contrato_", objVistaContratoGarantiaPoliza.NumeroContrato);
                template = template.Replace("_Fecha_Firma_Contrato_", fechaFirmaContrato); //Formato (dd/MM/aaaa)
                template = template.Replace("_Nombre_Contratista_", objVistaContratoGarantiaPoliza.NombreContratista);
                template = template.Replace("_Valor_Contrato_", string.Format("${0:#,0}", objVistaContratoGarantiaPoliza.ValorContrato.ToString()));  //fomato miles .
                template = template.Replace("_Plazo_", objVistaContratoGarantiaPoliza.PlazoContrato);
                template = template.Replace("_LinkF_", settings.DominioFront);
                template = template.Replace("_NumeroDRP_", contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault()?.NumeroDrp ?? " ");
                if (msjNotificacion != null)
                {
                    template = template.Replace("_Nombre_Aseguradora_", msjNotificacion.NombreAseguradora);
                    template = template.Replace("_Numero_Poliza_", msjNotificacion.NumeroPoliza);
                    template = template.Replace("_Fecha_Revision_", msjNotificacion.FechaRevision);
                    template = template.Replace("_Estado_Revision_", _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Revision_Poliza && x.Codigo == msjNotificacion.EstadoRevision).Select(x => x.Nombre).FirstOrDefault());
                    template = template.Replace("_Observaciones_", msjNotificacion.Observaciones);
                    template = template.Replace("_Fecha_Aprobacion_Poliza", msjNotificacion.FechaAprobacion);


                }

                List<UsuarioPerfil> lstUsuariosPerfil = new List<UsuarioPerfil>();

                lstUsuariosPerfil = _context.UsuarioPerfil.Where(r => r.Activo == true && r.PerfilId == (int)EnumeratorPerfil.Supervisor).ToList();

                List<Usuario> lstUsuarios = new List<Usuario>();
                bool blEnvioCorreo = true;
                foreach (var item in lstUsuariosPerfil)
                {
                    lstUsuarios = _context.Usuario.Where(r => r.UsuarioId == item.UsuarioId).ToList();

                    foreach (var usuario in lstUsuarios)
                    {
                        if (Helpers.Helpers.EnviarCorreo(usuario.Email, "Gestión Poliza", template, settings.Sender, settings.Password, settings.MailServer, settings.MailPort) == false)
                            blEnvioCorreo = false;
                    }
                }

                if (blEnvioCorreo)
                    return new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.CorreoEnviado };

                else
                    return new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.ErrorEnviarCorreo };

            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContratoPoliza.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.Error, idAccion, contratoPoliza.UsuarioCreacion, ex.InnerException.ToString())
                };
            }

        }

        //public async Task GetConsignationValue(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        //paquete 2: estado diferente a Aprobado
        public async Task EnviarCorreoSupervisor4dPolizaNoAprobada2(string dominioFront, string mailServer, int mailPort, bool enableSSL, string password, string sender)
        {
            Respuesta respuesta = new Respuesta();
            string fechaFirmaContrato = string.Empty;

            VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza;

            List<ContratoPoliza> lstContratoPoliza;
            //lstContratoPoliza = _context.ContratoPoliza
            //                                .Where(r => r.TipoSolicitudCodigo != ((int)EnumeratorEstadoPoliza.Con_aprobacion_de_polizas).ToString()
            //                                 && (bool)r.Eliminado == false)
            //                                                            .ToList();

            DateTime? FechaFirmaContrato_dt;
            DateTime RangoFechaConDiasHabiles;

            //foreach (ContratoPoliza contratoPoliza in lstContratoPoliza)
            //{
            //    RangoFechaConDiasHabiles = await _commonService.CalculardiasLaboralesTranscurridos(4, DateTime.Now);

            //    FechaFirmaContrato_dt = contratoPoliza?.FechaModificacion;

            //    if (FechaFirmaContrato_dt != null)

            //        if (FechaFirmaContrato_dt <= RangoFechaConDiasHabiles)
            //        {
            //            try
            //            {
            //                int idAccionEditarContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

            //                objVistaContratoGarantiaPoliza = new VistaContratoGarantiaPoliza();

            //                List<VistaContratoGarantiaPoliza> ListVista = new List<VistaContratoGarantiaPoliza>();
            //                ListVista = await ListVistaContratoGarantiaPoliza(0);

            //                NotificacionMensajeGestionPoliza msjNotificacion;
            //                msjNotificacion = new NotificacionMensajeGestionPoliza();

            //                getDataNotifMsjAseguradora(ref msjNotificacion, contratoPoliza, ref fechaFirmaContrato, ref objVistaContratoGarantiaPoliza, ListVista);


            //                bool blEnvioCorreo = false;

            //                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjFiduciariaJuridicaGestionPoliza);

            //                string template = TemplateRecoveryPassword.Contenido;

            //                Contrato contrato = _context.Contrato.Where(r => r.ContratoId == objVistaContratoGarantiaPoliza.IdContrato)
            //                    .Include(r => r.Contratacion).ThenInclude(r => r.DisponibilidadPresupuestal)
            //                                                                                          .FirstOrDefault();

            //                fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

            //                //datos basicos generales, aplican para los 4 mensajes
            //                template = template.Replace("_Tipo_Contrato_", objVistaContratoGarantiaPoliza.TipoContrato);
            //                template = template.Replace("_Numero_Contrato_", objVistaContratoGarantiaPoliza.NumeroContrato);
            //                template = template.Replace("_Fecha_Firma_Contrato_", fechaFirmaContrato); //Formato (dd/MM/aaaa)
            //                template = template.Replace("_Nombre_Contratista_", objVistaContratoGarantiaPoliza.NombreContratista);
            //                template = template.Replace("_Valor_Contrato_", string.Format("${0:#,0}", objVistaContratoGarantiaPoliza.ValorContrato.ToString()));  //fomato miles .
            //                template = template.Replace("_Plazo_", objVistaContratoGarantiaPoliza.PlazoContrato);
            //                template = template.Replace("_NumeroDRP_", contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault()?.NumeroDrp);

            //                if (msjNotificacion != null)
            //                {
            //                    template = template.Replace("_Nombre_Aseguradora_", msjNotificacion.NombreAseguradora);
            //                    template = template.Replace("_Numero_Poliza_", msjNotificacion.NumeroPoliza);
            //                }


            //                List<UsuarioPerfil> lstUsuariosPerfil = _context.UsuarioPerfil
            //                    .Where(r => r.Activo == true && r.PerfilId == (int)EnumeratorPerfil.Supervisor)
            //                    .Include(u => u.Usuario)
            //                    .ToList();

            //                lstUsuariosPerfil.ForEach(user =>
            //                {
            //                    blEnvioCorreo = Helpers.Helpers.EnviarCorreo(user.Usuario.Email, "Gestión Poliza", template, sender, password, mailServer, mailPort);

            //                });
            //                if (blEnvioCorreo)
            //                    respuesta = new Respuesta()
            //                    {
            //                        IsSuccessful = blEnvioCorreo,
            //                        IsValidation = blEnvioCorreo,
            //                        Code = ConstantMessagesContratoPoliza.CorreoEnviado,
            //                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, sender, "Gestión Pólizas")
            //                    };

            //                else
            //                    respuesta = new Respuesta()
            //                    {
            //                        IsSuccessful = blEnvioCorreo,
            //                        IsValidation = blEnvioCorreo,
            //                        Code = ConstantMessagesContratoPoliza.ErrorEnviarCorreo,
            //                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, sender, "Gestión Pólizas")
            //                    };
            //            }
            //            catch (Exception ex)
            //            {
            //                respuesta = new Respuesta()
            //                {
            //                    IsSuccessful = false,
            //                    IsValidation = false,
            //                    Code = ConstantMessagesUsuarios.ErrorGuardarCambios,
            //                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, sender, "Gestión Pólizas") + ": " + ex.ToString() + ex.InnerException
            //                };
            //            }
            //        }
            //}

        }

        //paquete 1: no tienen registro inicial contrato poliza
        public async Task EnviarCorreoSupervisor4dPolizaNoAprobada(string dominioFront, string mailServer, int mailPort, bool enableSSL, string password, string sender)
        {
            Respuesta respuesta = new Respuesta();
            string fechaFirmaContrato = string.Empty;
            DateTime? FechaFirmaContrato_dt;
            VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza;

            List<Contrato> lstContrato = _context.Contrato.Where(r => !(bool)r.Eliminado
                                                              && r.Contratacion.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.Registrados.ToString()).ToList();

            int cntPolizas = 0;
            DateTime RangoFechaConDiasHabiles;

            foreach (Contrato contrato in lstContrato)
            {
                cntPolizas = _context.ContratoPoliza.Where(r => r.ContratoId == contrato.ContratoId).Count();

                if (cntPolizas == 0)
                {
                    RangoFechaConDiasHabiles = await _commonService.CalculardiasLaboralesTranscurridos(4, DateTime.Now);

                    FechaFirmaContrato_dt = contrato?.FechaFirmaContrato;

                    if (FechaFirmaContrato_dt != null)
                        if (FechaFirmaContrato_dt <= RangoFechaConDiasHabiles)
                        {
                            ContratoPoliza contratoPoliza = new ContratoPoliza();

                            try
                            {
                                int idAccionEditarContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

                                objVistaContratoGarantiaPoliza = new VistaContratoGarantiaPoliza();

                                List<VistaContratoGarantiaPoliza> ListVista = new List<VistaContratoGarantiaPoliza>();
                                ListVista = await ListVistaContratoGarantiaPoliza(0);

                                int pIdTemplate = (int)enumeratorTemplate.MsjFiduciariaJuridicaGestionPoliza;

                                NotificacionMensajeGestionPoliza msjNotificacion;
                                msjNotificacion = new NotificacionMensajeGestionPoliza();

                                getDataNotifMsjAseguradora(ref msjNotificacion, contratoPoliza, ref fechaFirmaContrato, ref objVistaContratoGarantiaPoliza, ListVista);

                                bool blEnvioCorreo = false;

                                Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

                                string template = TemplateRecoveryPassword.Contenido;

                                fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                                //datos basicos generales, aplican para los 4 mensajes
                                template = template.Replace("_Tipo_Contrato_", objVistaContratoGarantiaPoliza.TipoContrato);
                                template = template.Replace("_Numero_Contrato_", objVistaContratoGarantiaPoliza.NumeroContrato);
                                template = template.Replace("_Fecha_Firma_Contrato_", fechaFirmaContrato);
                                template = template.Replace("_Nombre_Contratista_", objVistaContratoGarantiaPoliza.NombreContratista);
                                template = template.Replace("_Valor_Contrato_", string.Format("${0:#,0}", objVistaContratoGarantiaPoliza.ValorContrato.ToString()));  //fomato miles .
                                template = template.Replace("_Plazo_", objVistaContratoGarantiaPoliza.PlazoContrato);

                                if (msjNotificacion != null)
                                {
                                    template = template.Replace("_Nombre_Aseguradora_", msjNotificacion.NombreAseguradora);
                                    template = template.Replace("_Numero_Poliza_", msjNotificacion.NumeroPoliza);
                                    if (!string.IsNullOrEmpty(msjNotificacion.NumeroDRP))
                                        template = template.Replace("_NumeroDRP_", msjNotificacion.NumeroDRP);

                                }
                                //1   Administrador  - 
                                //2   Técnica
                                //3   Financiera - 
                                //4   Jurídica
                                //5   Administrativa - 
                                //6   Miembros Comite
                                //7   Secretario comité - 
                                //8   Supervisor 
                                List<UsuarioPerfil> lstUsuariosPerfil = _context.UsuarioPerfil
                                    .Where(r => r.Activo == true && r.PerfilId == (int)EnumeratorPerfil.Supervisor)
                                    .Include(u => u.Usuario)
                                    .ToList();

                                lstUsuariosPerfil.ForEach(user =>
                                {
                                    blEnvioCorreo = Helpers.Helpers.EnviarCorreo(user.Usuario.Email, "Gestión Poliza", template, sender, password, mailServer, mailPort);
                                });

                                if (blEnvioCorreo)
                                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.CorreoEnviado };

                                else
                                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.ErrorEnviarCorreo };

                                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, sender, "Gestión Pólizas");

                                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, sender, "Gestión Pólizas");

                            }
                            catch (Exception ex)
                            {

                                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesUsuarios.ErrorGuardarCambios };
                                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, sender, "Gestión Pólizas") + ": " + ex.ToString() + ex.InnerException;

                            }
                        }
                }
            }
        }

        private void getDataNotifMsjAseguradora(ref NotificacionMensajeGestionPoliza msjNotificacion, ContratoPoliza contratoPoliza, ref string fechaFirmaContrato, ref VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza, List<VistaContratoGarantiaPoliza> ListVista)
        {
            msjNotificacion.NombreAseguradora = contratoPoliza.NombreAseguradora;
            msjNotificacion.NumeroPoliza = contratoPoliza.NumeroPoliza;

            msjNotificacion.FechaAprobacion = contratoPoliza.FechaAprobacion != null ? Convert.ToDateTime(contratoPoliza.FechaAprobacion).ToString("dd/MM/yyyy") : contratoPoliza.FechaAprobacion.ToString();

            PolizaObservacion polizaObservacion;

            polizaObservacion = _context.PolizaObservacion.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).FirstOrDefault();

            if (polizaObservacion != null)
            {
                msjNotificacion.EstadoRevision = polizaObservacion.EstadoRevisionCodigo;

            }

            Contrato contrato = _context.Contrato.Where(r => r.ContratoId == contratoPoliza.ContratoId).FirstOrDefault();

            if (contrato != null)
            {
                objVistaContratoGarantiaPoliza = ListVista.Where(x => x.IdContrato == contrato.ContratoId).FirstOrDefault();
                fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();
            }

            Contratacion contratacion = _context.Contratacion.Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();

            DisponibilidadPresupuestal disponibilidadPresupuestal = _context.DisponibilidadPresupuestal.Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();

            if (disponibilidadPresupuestal != null)
                msjNotificacion.NumeroDRP = disponibilidadPresupuestal.NumeroDrp;
        }

        private static void LimpiarEntradasContratoPoliza(ref ContratoPoliza contratoPoliza)
        {
            contratoPoliza.DescripcionModificacion = Helpers.Helpers.CleanStringInput(contratoPoliza.DescripcionModificacion);
            contratoPoliza.NumeroPoliza = Helpers.Helpers.CleanStringInput(contratoPoliza.NumeroPoliza);
            //contratoPoliza.ResponsableAprobacion = Helpers.Helpers.CleanStringInput(contratoPoliza.ResponsableAprobacion);
            contratoPoliza.NumeroCertificado = Helpers.Helpers.CleanStringInput(contratoPoliza.NumeroCertificado);
            contratoPoliza.NombreAseguradora = Helpers.Helpers.CleanStringInput(contratoPoliza.NombreAseguradora);
        }

        public async Task<bool> ValidarRegistroCompletoSeguros(ContratoPoliza contratoPoliza)
        {
            List<PolizaGarantia> lstPolizaGarantia = await _context.PolizaGarantia
                                                              .Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId)
                                                                                                                            .ToListAsync();
            return lstPolizaGarantia.Count() > 0;
        }


        public async Task<bool> ConsultarRegistroCompletoCumple(int ContratoPolizaId)
        {
            ContratoPoliza contratoPoliza = await _context.ContratoPoliza.Where(r => r.ContratoPolizaId == ContratoPolizaId).FirstOrDefaultAsync();

            if (contratoPoliza != null)
            {

                return true;
            }
            else
                return false;
        }

        public async Task<Respuesta> AprobarContratoByIdContrato(int pIdContrato, AppSettingsService settings, string pUsuario)
        {
            int idAccionEditarContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza;
                objVistaContratoGarantiaPoliza = new VistaContratoGarantiaPoliza();

                List<VistaContratoGarantiaPoliza> ListVista = new List<VistaContratoGarantiaPoliza>();
                ListVista = await ListVistaContratoGarantiaPoliza(pIdContrato);

                objVistaContratoGarantiaPoliza = ListVista.Where(x => x.IdContrato == pIdContrato).FirstOrDefault();

                NotificacionMensajeGestionPoliza msjNotificacion;
                msjNotificacion = new NotificacionMensajeGestionPoliza();

                string fechaFirmaContrato;

                ContratoPoliza contratoPoliza = new ContratoPoliza();
                Contrato contrato = new Contrato();
                contrato = _context.Contrato.Where(r => r.ContratoId == pIdContrato).FirstOrDefault();
                if (contrato != null)
                {
                    contrato.EstaDevuelto = false;
                    _context.Update(contrato);
                    _context.SaveChanges();
                }

                contratoPoliza = _context.ContratoPoliza.Where(r => r.ContratoId == contrato.ContratoId)
                   .OrderByDescending(r => r.ContratoPolizaId).FirstOrDefault();


                contratoPoliza.UsuarioModificacion = pUsuario;
                //contratoPoliza.TipoSolicitudCodigo = ((int)EnumeratorEstadoPoliza.Con_aprobacion_de_polizas).ToString();
                _context.ContratoPoliza.Update(contratoPoliza);

                fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                getDataNotifMsjAseguradora(ref msjNotificacion, contratoPoliza, ref fechaFirmaContrato, ref objVistaContratoGarantiaPoliza, ListVista);


                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjSupervisorJuridicaGestionPoliza);

                string template = TemplateRecoveryPassword.Contenido;

                contrato = _context.Contrato.Where(r => r.ContratoId == objVistaContratoGarantiaPoliza.IdContrato).FirstOrDefault();

                fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                //datos basicos generales, aplican para los 4 mensajes
                template = template.Replace("_Tipo_Contrato_", objVistaContratoGarantiaPoliza.TipoContrato);
                template = template.Replace("_Numero_Contrato_", objVistaContratoGarantiaPoliza.NumeroContrato);
                template = template.Replace("_Fecha_Firma_Contrato_", fechaFirmaContrato); //Formato (dd/MM/aaaa)
                template = template.Replace("_Nombre_Contratista_", objVistaContratoGarantiaPoliza.NombreContratista);
                template = template.Replace("_Valor_Contrato_", string.Format("${0:#,0}", objVistaContratoGarantiaPoliza.ValorContrato.ToString()));  //fomato miles .
                template = template.Replace("_Plazo_", objVistaContratoGarantiaPoliza.PlazoContrato);
                template = template.Replace("_NumeroDRP_", msjNotificacion.NumeroDRP ?? " ");
                template = template.Replace("_LinkF_", settings.DominioFront);

                if (msjNotificacion != null)
                {
                    template = template.Replace("_Nombre_Aseguradora_", msjNotificacion.NombreAseguradora);
                    template = template.Replace("_Numero_Poliza_", msjNotificacion.NumeroPoliza);
                    template = template.Replace("_Fecha_Revision_", msjNotificacion.FechaRevision);
                    template = template.Replace("_Estado_Revision_", _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Revision_Poliza && x.Codigo == msjNotificacion.EstadoRevision).Select(x => x.Nombre).FirstOrDefault());
                    template = template.Replace("_Observaciones_", msjNotificacion.Observaciones);
                    template = template.Replace("_NumeroDRP_", msjNotificacion.NumeroDRP ?? " ");
                    template = template.Replace("_Fecha_Aprobacion_Poliza", msjNotificacion.FechaAprobacion);
                }

                //Enviar Correo  INTERVENTOR - JURIDICA - SUPERVISOR
                List<Usuario> lstUsuarios = _context.UsuarioPerfil
                                                                    .Where(r => r.Activo == true &&
                                                                                              (
                                                                                                    r.PerfilId == (int)EnumeratorPerfil.Interventor
                                                                                                || r.PerfilId == (int)EnumeratorPerfil.Juridica
                                                                                                || r.PerfilId == (int)EnumeratorPerfil.Supervisor)
                                                                                              ).Include(r => r.Usuario)
                                                                                                                    .Select(r => r.Usuario).ToList();
                bool blEnvioCorreo = true;

                foreach (var usuario in lstUsuarios)
                {
                    if (false == Helpers.Helpers.EnviarCorreo(usuario.Email, "Gestión Poliza", template, settings.Sender, settings.Password, settings.MailServer, settings.MailPort))
                        blEnvioCorreo = false;
                }

                if (blEnvioCorreo)
                    return new Respuesta()
                    {
                        IsSuccessful = blEnvioCorreo,
                        IsValidation = blEnvioCorreo,
                        Code = ConstantMessagesContratoPoliza.CorreoEnviado
                    };
                else
                    return new Respuesta()
                    {
                        IsSuccessful = blEnvioCorreo,
                        IsValidation = blEnvioCorreo,
                        Code = ConstantMessagesContratoPoliza.ErrorEnviarCorreo
                    };

            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesContratoPoliza.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.ErrorInterno, idAccionEditarContratoPoliza, pUsuario, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        private List<Usuario> getCorreos(int perfilId)
        {
            return _context.UsuarioPerfil
                                        .Where(r => r.Activo == true && r.PerfilId == perfilId)
                                        .Include(r => r.Usuario)
                                        .Select(r => r.Usuario)
                                                              .ToList();
        }

        public async Task<Respuesta> EnviarCorreoGestionPoliza(string lstMails, string pMailServer, int pMailPort, string pPassword,
           string pSentender, VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza, string fechaFirmaContrato, int pIdTemplate,
           string fronturl, NotificacionMensajeGestionPoliza objNotificacionAseguradora = null)
        {
            bool blEnvioCorreo = false;
            Respuesta respuesta = new Respuesta();
            try
            {
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

                string template = TemplateRecoveryPassword.Contenido;

                Contrato contrato = _context.Contrato
                    .Where(r => r.ContratoId == objVistaContratoGarantiaPoliza.IdContrato)
                    .Include(c => c.Contratacion)
                       .ThenInclude(dp => dp.DisponibilidadPresupuestal)
                    .FirstOrDefault();

                fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                //datos basicos generales, aplican para los 4 mensajes
                template = template.Replace("_Tipo_Contrato_", objVistaContratoGarantiaPoliza.TipoContrato);
                template = template.Replace("_Numero_Contrato_", objVistaContratoGarantiaPoliza.NumeroContrato);
                template = template.Replace("_Fecha_Firma_Contrato_", fechaFirmaContrato); //Formato (dd/MM/aaaa)
                template = template.Replace("_Nombre_Contratista_", objVistaContratoGarantiaPoliza.NombreContratista);
                template = template.Replace("_Valor_Contrato_", string.Format("${0:#,0}", objVistaContratoGarantiaPoliza.ValorContrato.ToString()));  //fomato miles .
                template = template.Replace("_Plazo_", objVistaContratoGarantiaPoliza.PlazoContrato);
                template = template.Replace("_NumeroDRP_", contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault()?.NumeroDrp ?? " ");
                template = template.Replace("_LinkF_", fronturl);

                if (objNotificacionAseguradora != null)
                {
                    template = template.Replace("_Nombre_Aseguradora_", objNotificacionAseguradora.NombreAseguradora);
                    template = template.Replace("_Numero_Poliza_", objNotificacionAseguradora.NumeroPoliza);
                    template = template.Replace("_Fecha_Revision_", objNotificacionAseguradora.FechaRevision);
                    template = template.Replace("_Estado_Revision_", _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Revision_Poliza && x.Codigo == objNotificacionAseguradora.EstadoRevision).Select(x => x.Nombre).FirstOrDefault());
                    template = template.Replace("_Observaciones_", objNotificacionAseguradora.Observaciones);
                    template = template.Replace("_Fecha_Aprobacion_Poliza", objNotificacionAseguradora.FechaAprobacion);
                }
                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(lstMails, "Gestión Poliza", template, pSentender, pPassword, pMailServer, pMailPort);

                if (blEnvioCorreo)
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.CorreoEnviado };

                else
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.ErrorEnviarCorreo };

                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, lstMails, "Gestión Pólizas");
                return respuesta;

            }
            catch (Exception ex)
            {
                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesContratoPoliza.Error };
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, lstMails, "Gestión Pólizas") + ": " + ex.ToString() + ex.InnerException;
                return respuesta;
            }

        }


        public async Task<List<GrillaContratoGarantiaPoliza>> ListGrillaContratoGarantiaPoliza()
        {
            List<GrillaContratoGarantiaPoliza> ListContratoGrilla = new List<GrillaContratoGarantiaPoliza>();

            List<Contrato> ListContratos = await _context.Contrato.Where(r => r.Contratacion.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.Registrados.ToString() && !(bool)r.Eliminado).ToListAsync();

            foreach (var contrato in ListContratos)
            {
                try
                {
                    Contratacion contratacion = null;
                    contratacion = _context.Contratacion.Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();

                    string strNumeroSolicitudContratacion = string.Empty;
                    Dominio TipoSolicitudCodigoContratacion = null;
                    string strTipoSolicitudContratacion = string.Empty;
                    string strTipoSolicitudCodigoContratacion = string.Empty;

                    if (contratacion != null)
                    {
                        strNumeroSolicitudContratacion = contratacion.NumeroSolicitud;
                        TipoSolicitudCodigoContratacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratacion.EstadoSolicitudCodigo.Trim(), (int)EnumeratorTipoDominio.Estado_Solicitud);
                        if (TipoSolicitudCodigoContratacion != null)
                        {
                            strTipoSolicitudCodigoContratacion = TipoSolicitudCodigoContratacion.Codigo;
                            strTipoSolicitudContratacion = TipoSolicitudCodigoContratacion.Nombre;
                        }
                    }

                    ContratoPoliza contratoPoliza = null;
                    contratoPoliza = await _commonService.GetLastContratoPolizaByContratoId(contrato.ContratoId);

                    int ContratoPolizaIdValor = 0;

                    string strTipoSolicitudCodigoContratoPoliza = (Convert.ToInt32(ConstanCodigoTipoSolicitud.Contratacion)).ToString();
                    string strTipoSolicitudNombreContratoPoliza = ConstanStringTipoSolicitudContratacion.contratacion; //contratacion o modif contractual - 

                    string strEstadoSolicitudCodigoContratoPoliza = ((int)EnumeratorEstadoPoliza.Sin_radicacion_polizas).ToString();
                    string strEstadoSolicitudNombreContratoPoliza = "Sin radicación de pólizas";

                    Dominio TipoSolicitudCodigoContratoPoliza;
                    Dominio EstadoSolicitudCodigoContratoPoliza;

                    string strRegistroCompletoPolizaNombre = "Incompleto";
                    bool bRegistroCompletoPoliza = false;

                    if (contratoPoliza != null)
                    {
                        if (contrato.EstaDevuelto != null)
                        {
                            if ((bool)contrato.EstaDevuelto == false)
                            {
                                if (contratoPoliza.RegistroCompleto != null)
                                {
                                    strRegistroCompletoPolizaNombre = (bool)contratoPoliza.RegistroCompleto ? "Completo" : "Incompleto";
                                    bRegistroCompletoPoliza = (bool)contratoPoliza.RegistroCompleto;
                                }
                                else
                                {
                                    strRegistroCompletoPolizaNombre = "Incompleto";
                                    bRegistroCompletoPoliza = false;
                                }
                            }
                        }
                        else
                        {
                            if (contratoPoliza.RegistroCompleto != null)
                            {
                                strRegistroCompletoPolizaNombre = (bool)contratoPoliza.RegistroCompleto ? "Completo" : "Incompleto";
                                bRegistroCompletoPoliza = (bool)contratoPoliza.RegistroCompleto;
                            }
                            else
                            {
                                strRegistroCompletoPolizaNombre = "Incompleto";
                                bRegistroCompletoPoliza = false;

                            }
                        }


                        ContratoPolizaIdValor = contratoPoliza.ContratoPolizaId;
                        //if (contratoPoliza.TipoSolicitudCodigo != null)
                        //{
                        //    TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo.Trim(), (int)EnumeratorTipoDominio.Tipo_Solicitud);

                        //    if (TipoSolicitudCodigoContratoPoliza != null)
                        //    {
                        //        strTipoSolicitudCodigoContratoPoliza = TipoSolicitudCodigoContratoPoliza.Codigo;
                        //        strTipoSolicitudNombreContratoPoliza = TipoSolicitudCodigoContratoPoliza.Nombre;
                        //    }
                        //}

                        EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.EstadoPolizaCodigo.Trim(), (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                        if (EstadoSolicitudCodigoContratoPoliza != null)
                        {
                            strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Codigo;
                            strEstadoSolicitudNombreContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;
                        }
                    }


                    GrillaContratoGarantiaPoliza contratoGrilla = new GrillaContratoGarantiaPoliza
                    {
                        ContratoId = contrato.ContratoId,
                        ContratoPolizaId = ContratoPolizaIdValor,
                        FechaFirma = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString(),
                        FechaCreacionContrato = contrato.FechaCreacion,
                        NumeroContrato = contrato.NumeroContrato,
                        NumeroSolicitudContratacion = strNumeroSolicitudContratacion,
                        //jflorez 20201124 no modelado, dejo el dato de contratos (puede ser contratacion o modificaion contractual)
                        TipoSolicitud = ConstanStringTipoSolicitudContratacion.contratacion,
                        TipoSolicitudCodigo = strTipoSolicitudCodigoContratoPoliza,
                        TipoSolicitudCodigoContratacion = strTipoSolicitudCodigoContratacion,
                        TipoSolicitudContratacion = strTipoSolicitudContratacion,
                        EstadoPoliza = strEstadoSolicitudNombreContratoPoliza,
                        EstadoPolizaCodigo = strEstadoSolicitudCodigoContratoPoliza,
                        RegistroCompleto = contrato.RegistroCompleto,
                        RegistroCompletoNombre = contrato.RegistroCompleto != true ? "Completo" : "Incompleto",
                        RegistroCompletoPoliza = bRegistroCompletoPoliza,
                        RegistroCompletoPolizaNombre = strRegistroCompletoPolizaNombre,
                    };

                    ListContratoGrilla.Add(contratoGrilla);
                }
                catch (Exception e)
                {
                    GrillaContratoGarantiaPoliza proyectoGrilla = new GrillaContratoGarantiaPoliza
                    {

                        ContratoId = contrato.ContratoId,
                        ContratoPolizaId = 0,
                        FechaFirma = e.ToString(),
                        NumeroContrato = e.InnerException.ToString(),
                        //TipoSolicitud= contratoPoliza.TipoSolicitudCodigo
                        //EstadoRegistro { get; set; }

                        //Departamento = departamento.Descripcion,
                        //Municipio = municipio.Descripcion,
                        //InstitucionEducativa = _context.InstitucionEducativaSede.Find(contrato.InstitucionEducativaId).Nombre,
                        //Sede = _context.InstitucionEducativaSede.Find(contrato.SedeId).Nombre,
                        TipoSolicitud = "ERROR",
                        FechaCreacionContrato = DateTime.Now,
                        RegistroCompleto = false,
                        RegistroCompletoNombre = "ERROR",

                    };
                    ListContratoGrilla.Add(proyectoGrilla);
                }
            }
            return ListContratoGrilla.OrderByDescending(r => r.FechaFirma).ToList();

        }

        public async Task<List<VistaContratoGarantiaPoliza>> ListVistaContratoGarantiaPoliza(int pContratoId)
        {
            List<VistaContratoGarantiaPoliza> ListContratoGrilla = new List<VistaContratoGarantiaPoliza>();

            if (pContratoId == 0)
            {
                return ListContratoGrilla;
            }
            List<Contrato> ListContratos = new List<Contrato>();


            ListContratos = await _context.Contrato
                                                .Where(r => r.ContratoId == pContratoId)
                                                .Include(r => r.ContratoPoliza).ThenInclude(R => R.PolizaGarantia)
                                                .Include(R => R.ContratoPoliza).ThenInclude(R => R.PolizaObservacion)
                                                .ToListAsync();

            foreach (var contrato in ListContratos)
            {
                try
                {
                    ContratoPoliza contratoPoliza = null;
                    contratoPoliza = await _commonService.GetLastContratoPolizaByContratoId(contrato.ContratoId);

                    Dominio TipoSolicitudCodigoContratoPoliza = null;
                    string strTipoSolicitudCodigoContratoPoliza = "Sin radicación de pólizas";
                    string strFechaFirmaContrato = string.Empty;
                    int contratacionIdValor = 0;

                    if (contratoPoliza != null)
                    {
                        //if (contratoPoliza.TipoSolicitudCodigo != null)
                        //{
                        //    TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo.Trim(), (int)EnumeratorTipoDominio.Tipo_de_Solicitud);

                        //    if (TipoSolicitudCodigoContratoPoliza != null)
                        //        strTipoSolicitudCodigoContratoPoliza = TipoSolicitudCodigoContratoPoliza.Nombre;
                        //}
                    }
                    strFechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                    Contratacion contratacion = null;
                    contratacion = await _commonService.GetContratacionByContratacionId(contrato.ContratacionId);

                    string strContratistaNombre = string.Empty;
                    string strContratistaNumeroIdentificacion = string.Empty;

                    Dominio TipoDocumentoCodigoContratista;
                    string strTipoDocumentoContratista = string.Empty;

                    Dominio TipoContratoCodigoContrato = null;

                    Contratista contratista = null;
                    decimal vlrContratoComponenteUso = 0;

                    if (contratacion != null)
                    {
                        if (contratacion.ContratistaId != null)
                            contratista = await _commonService.GetContratistaByContratistaId((Int32)contratacion.ContratistaId);

                        if (contratista != null)
                        {
                            strContratistaNombre = contratista.Nombre;

                            strContratistaNumeroIdentificacion = contratista.NumeroIdentificacion.ToString();

                            TipoDocumentoCodigoContratista = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratista.TipoIdentificacionCodigo, (int)EnumeratorTipoDominio.Tipo_Documento);

                            if (TipoDocumentoCodigoContratista != null)
                                strTipoDocumentoContratista = TipoDocumentoCodigoContratista.Nombre;
                        }
                        //jflorez -20201124 ajusto tipo dominio
                        //TipoContratoCodigoContrato = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratacion.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Solicitud);
                        TipoContratoCodigoContrato = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratacion.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Opcion_por_contratar);

                        contratacionIdValor = contratacion.ContratacionId;

                        vlrContratoComponenteUso = getSumVlrContratoComponente(contratacion.ContratacionId);

                    }

                    DisponibilidadPresupuestal disponibilidadPresupuestal = null;

                    if (contratacion != null)
                    {
                        disponibilidadPresupuestal = _context.DisponibilidadPresupuestal.Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();
                    }
                    string contratoObjeto = string.Empty;

                    Int32 plazoDias = 0, plazoMeses = 0;
                    string PlazoContratoFormat = string.Empty;

                    if (disponibilidadPresupuestal != null)
                    {
                        contratoObjeto = disponibilidadPresupuestal.Objeto;

                        if (!string.IsNullOrEmpty(disponibilidadPresupuestal.PlazoDias.ToString()))
                            plazoDias = Convert.ToInt32(disponibilidadPresupuestal.PlazoDias);

                        if (!string.IsNullOrEmpty(disponibilidadPresupuestal.PlazoMeses.ToString()))
                            plazoMeses = Convert.ToInt32(disponibilidadPresupuestal.PlazoMeses);

                        PlazoContratoFormat = plazoMeses.ToString("00") + " meses / " + plazoDias.ToString("00") + " dias";
                    }

                    string strTipoContratoCodigoContratoNombre = string.Empty;

                    if (TipoContratoCodigoContrato != null)
                        strTipoContratoCodigoContratoNombre = TipoContratoCodigoContrato.Nombre;

                    VistaContratoGarantiaPoliza proyectoGrilla = new VistaContratoGarantiaPoliza
                    {
                        ContratacionId = contratacionIdValor,
                        IdContrato = contrato.ContratoId,
                        TipoContrato = strTipoContratoCodigoContratoNombre,
                        NumeroContrato = contrato.NumeroContrato,
                        ObjetoContrato = contratoObjeto,
                        NombreContratista = strContratistaNombre,
                        TipoDocumento = strTipoDocumentoContratista,
                        PlazoMeses = disponibilidadPresupuestal.PlazoMeses,
                        PlazoDias = disponibilidadPresupuestal.PlazoDias,
                        PlazoContrato = PlazoContratoFormat,
                        //Nit  
                        NumeroIdentificacion = strContratistaNumeroIdentificacion,

                        //ValorContrato = contrato.Valor.ToString(),
                        ValorContrato = vlrContratoComponenteUso,

                        //EstadoRegistro 
                        //public bool? RegistroCompleto { get; set; }                         

                        DescripcionModificacion = "resumen", // resumen   TEMPORAL REV

                        //TipoModificacion = TipoModificacionCodigoContratoPoliza.Nombre
                        TipoModificacion = "Tipo modificacion",
                        //jflorez 20201124 ajusto el tipo de solicitud, es contratación o modificacion contractual, en este momento no existe modelo de modificaciones contractuales por lo tanto envio el string plano
                        TipoSolicitud = ConstanStringTipoSolicitudContratacion.contratacion,

                        FechaFirmaContrato = strFechaFirmaContrato,

                        //TipoSolicitud= contratoPoliza.TipoSolicitudCodigo
                        //EstadoRegistro { get; set; }

                        //InstitucionEducativa = _context.InstitucionEducativaSede.Find(contrato.InstitucionEducativaId).Nombre,
                        //Sede = _context.InstitucionEducativaSede.Find(contrato.SedeId).Nombre,

                        //Fecha = contrato.FechaCreacion != null ? Convert.ToDateTime(contrato.FechaCreacion).ToString("yyyy-MM-dd") : proyecto.FechaCreacion.ToString(),
                        //EstadoRegistro = "COMPLETO"
                    };

                    //if (!(bool)proyecto.RegistroCompleto)
                    //{
                    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    //}
                    ListContratoGrilla.Add(proyectoGrilla);
                }
                catch (Exception e)
                {
                    VistaContratoGarantiaPoliza proyectoGrilla = new VistaContratoGarantiaPoliza();
                }
            }
            return ListContratoGrilla.OrderByDescending(r => r.TipoSolicitud).ToList();

        }

        /*jflorez, ajusto la suma*/
        private decimal getSumVlrContratoComponente(int contratacionId)
        {
            return _context.ComponenteUso
                                 .Where(x => x.ComponenteAportante.ContratacionProyectoAportante.ContratacionProyecto.ContratacionId == contratacionId).
                                  Sum(x => x.ValorUso);
        }
    }

    #endregion
}
