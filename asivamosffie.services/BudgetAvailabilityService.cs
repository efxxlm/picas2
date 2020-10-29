using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.api.Controllers;
using AuthorizationTest.JwtHelpers;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using Z.EntityFramework.Plus;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.IO;

namespace asivamosffie.services
{
    public class BudgetAvailabilityService : IBudgetAvailabilityService
    {
        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;
        public readonly IConverter _converter;

        public BudgetAvailabilityService(devAsiVamosFFIEContext context, ICommonService commonService, IConverter converter)
        {
            _context = context;
            _commonService = commonService;
            _converter = converter;
        }

        public async Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestal()
        {
            List<DisponibilidadPresupuestal> ListDisponibilidadPresupuestal = await _context.DisponibilidadPresupuestal.Where(r => !(bool)r.Eliminado).ToListAsync();

            List<DisponibilidadPresupuestalGrilla> ListDisponibilidadPresupuestalGrilla = new List<DisponibilidadPresupuestalGrilla>();

            foreach (var DisponibilidadPresupuestal in ListDisponibilidadPresupuestal)
            {
                string strEstadoRegistro = "";
                string strTipoSolicitud = "";

                if (string.IsNullOrEmpty(DisponibilidadPresupuestal.EstadoSolicitudCodigo))
                {
                    strEstadoRegistro = await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.EstadoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal);
                }

                if (string.IsNullOrEmpty(DisponibilidadPresupuestal.TipoSolicitudCodigo))
                {
                    strTipoSolicitud = await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_de_Solicitud);
                }

                DisponibilidadPresupuestalGrilla disponibilidadPresupuestalGrilla = new DisponibilidadPresupuestalGrilla
                {

                    FechaSolicitud = DisponibilidadPresupuestal.FechaSolicitud.ToString(),
                    EstadoRegistro = strEstadoRegistro,
                    TipoSolicitud = strTipoSolicitud,
                    DisponibilidadPresupuestalId = DisponibilidadPresupuestal.DisponibilidadPresupuestalId,
                    NumeroSolicitud = DisponibilidadPresupuestal.NumeroSolicitud
                };

                ListDisponibilidadPresupuestalGrilla.Add(disponibilidadPresupuestalGrilla);
            }

            return ListDisponibilidadPresupuestalGrilla.OrderByDescending(r => r.EstadoRegistro).ToList();
        }
         
        public async Task<DisponibilidadPresupuestal> GetDisponibilidadPresupuestalByID(int pDisponibilidadPresupuestalId)
        {
            //las tabla DisponibilidadPresupuestalProyecto no tiene campos de auditoria
            return await _context.DisponibilidadPresupuestal.Where(r => r.DisponibilidadPresupuestalId == pDisponibilidadPresupuestalId).Include(r => r.DisponibilidadPresupuestalProyecto).FirstOrDefaultAsync();

        }
         
        public async Task<DisponibilidadPresupuestal> GetBudgetAvailabilityById(int id)
        {
            try
            {
                return await _context.DisponibilidadPresupuestal.Where(d => d.DisponibilidadPresupuestalId == id)
                                    .Include(r => r.DisponibilidadPresupuestalProyecto)
                                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
         
        public async Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(string pCodigoEstadoSolicitud)
        {

            List<DisponibilidadPresupuestal> ListDisponibilidadPresupuestal = await _context.DisponibilidadPresupuestal.Where(r => !(bool)r.Eliminado && r.EstadoSolicitudCodigo.Equals(pCodigoEstadoSolicitud)).ToListAsync();

            List<DisponibilidadPresupuestalGrilla> ListDisponibilidadPresupuestalGrilla = new List<DisponibilidadPresupuestalGrilla>();

            foreach (var DisponibilidadPresupuestal in ListDisponibilidadPresupuestal)
            {
                string strEstadoRegistro = "";
                string strTipoSolicitud = "";
                if (!string.IsNullOrEmpty(DisponibilidadPresupuestal.EstadoSolicitudCodigo))
                {
                    strEstadoRegistro = await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.EstadoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal);
                }

                if (!string.IsNullOrEmpty(DisponibilidadPresupuestal.TipoSolicitudCodigo))
                {
                    strTipoSolicitud = await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal);
                }

                DateTime? fechaContrato = null;
                string numeroContrato = "";
                if (DisponibilidadPresupuestal.ContratacionId != null)
                {
                    var contrato = _context.Contrato.Find(DisponibilidadPresupuestal.ContratacionId);
                    fechaContrato = contrato != null ? contrato.FechaFirmaContrato : null;
                    numeroContrato = DisponibilidadPresupuestal.NumeroContrato == null ?
                        contrato != null ? contrato.NumeroContrato : ""
                        : DisponibilidadPresupuestal.NumeroContrato;
                }
                else
                {
                    numeroContrato = DisponibilidadPresupuestal.NumeroContrato != null ?
                         DisponibilidadPresupuestal.NumeroContrato : "";
                }
                 
                DisponibilidadPresupuestalGrilla disponibilidadPresupuestalGrilla = new DisponibilidadPresupuestalGrilla
                {

                    FechaSolicitud = DisponibilidadPresupuestal.FechaSolicitud.ToString("yyyy/MM/dd"),
                    EstadoRegistro = strEstadoRegistro,
                    TipoSolicitud = strTipoSolicitud,
                    DisponibilidadPresupuestalId = DisponibilidadPresupuestal.DisponibilidadPresupuestalId,
                    NumeroSolicitud = DisponibilidadPresupuestal.NumeroSolicitud,
                    FechaFirmaContrato = fechaContrato == null ? "" : Convert.ToDateTime(fechaContrato).ToString("yy-MM-dd"),
                    NumeroContrato = numeroContrato,

                };
                ListDisponibilidadPresupuestalGrilla.Add(disponibilidadPresupuestalGrilla);
            }
            return ListDisponibilidadPresupuestalGrilla.OrderByDescending(r => r.DisponibilidadPresupuestalId).ToList();

        }
         
        /*autor: jflorez
            descripción: objeto para entregar a front los datos ordenados de disponibilidades
        impacto: CU 3.3.3*/
        public async Task<List<EstadosDisponibilidad>> GetListGenerarDisponibilidadPresupuestal()
        {
            List<EstadosDisponibilidad> estadosdisponibles = new List<EstadosDisponibilidad>();
            var estados = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal && x.Activo == true).ToList();
            foreach (var estado in estados)
            {
                estadosdisponibles.Add(new EstadosDisponibilidad { DominioId = estado.DominioId, NombreEstado = estado.Nombre, DisponibilidadPresupuestal = await this.GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(estado.Codigo) });
            }
            return estadosdisponibles;
        }

        public async Task<Respuesta> SetCancelRegistroPresupuestal(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Con_disponibilidad_cancelada;
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion;
                DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();
                pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();
                _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.CanceladoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.CanceladoCorrrectamente, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, "CANCELAR DISPONIBILIDAD PRESUPUESTAL")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        public async Task<Respuesta> CreateDDP(int pId, string pUsuarioModificacion, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Include(x => x.Contratacion).
                ThenInclude(x => x.ContratacionProyecto).ThenInclude(x => x.ContratacionProyectoAportante).ThenInclude(x => x.CofinanciacionAportante).
                ThenInclude(x => x.FuenteFinanciacion).FirstOrDefault(x => x.DisponibilidadPresupuestalId == pId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            int consecutivo = _context.DisponibilidadPresupuestal.Where(x => x.NumeroDdp != null).Count();
            /*busco usuario Juridico*/
            var usuarioJuridico = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Juridica).Include(y => y.Usuario).FirstOrDefault();
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Con_validacion_presupuestal;
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = pUsuarioModificacion.ToUpper();
                DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();
                string tipo = "";
                if (DisponibilidadCancelar.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Tradicional)
                {
                    tipo = "PI";
                }
                else if (DisponibilidadCancelar.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo)
                {
                    tipo = "PA";
                }
                else
                {
                    tipo = "ES";
                }
                DisponibilidadCancelar.NumeroDdp = "DDP_" + tipo + "_" + consecutivo.ToString();
                //
                //guardar el tema de platas
                //

                Dictionary<int, List<decimal>> fuente = new Dictionary<int, List<decimal>>();
                //var contratacionproyecto = DisponibilidadCancelar.Contratacion.ContratacionProyecto;
                var gestionfuentes = _context.GestionFuenteFinanciacion.Where(x => x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == DisponibilidadCancelar.DisponibilidadPresupuestalId);
                foreach (var gestion in gestionfuentes)
                {
                    int estadocod = (int)EnumeratorEstadoGestionFuenteFinanciacion.Gestionado_en_DRP;
                    gestion.EstadoCodigo = estadocod.ToString();
                    gestion.FechaModificacion = DateTime.Now;
                    gestion.UsuarioModificacion = pUsuarioModificacion.ToUpper();
                    _context.GestionFuenteFinanciacion.Update(gestion);
                }

                _context.SaveChanges();
                //envio correo a juridica
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DisponibilidadPresupuestalGenerada);
                string template = TemplateRecoveryPassword.Contenido;

                //template = template.Replace("_Link_", urlDestino);                

                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioJuridico.Usuario.Email, "DDP Generado", template, pSentender, pPassword, pMailServer, pMailPort);
                if (blEnvioCorreo)
                {
                    return new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantMessagesGenerateBudget.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.OperacionExitosa, idAccion, pUsuarioModificacion, "GENERAR DDP DISPONIBILIDAD PRESUPUESTAL")
                    };
                }
                else
                {
                    return new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantMessagesGenerateBudget.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pUsuarioModificacion, "ERROR ENVIO MAIL GENERAR DDP DISPONIBILIDAD PRESUPUESTAL")
                    };
                }
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        public async Task<Respuesta> returnDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Devuelta_por_coordinacion_financiera;
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion;
                DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();

                pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();
                _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.DevueltoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.DevueltoCorrrectamente, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, "DEVOLVER DISPONIBILIDAD PRESUPUESTAL")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        public async Task<Respuesta> CreateEditarDisponibilidadPresupuestal(DisponibilidadPresupuestal DP)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = "";
            DisponibilidadPresupuestal disponibilidadPresupuestalAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(DP.DisponibilidadPresupuestalId.ToString()) || DP.DisponibilidadPresupuestalId == 0)
                {
                    //Concecutivo
                    var LastRegister = _context.DisponibilidadPresupuestal.OrderByDescending(x => x.DisponibilidadPresupuestalId).First().DisponibilidadPresupuestalId;


                    //Auditoria
                    strCrearEditar = "CREAR DISPONIBILIDAD PRESUPUESTAL";
                    DP.FechaCreacion = DateTime.Now;
                    DP.Eliminado = false;

                    //DP.NumeroDdp = ""; TODO: traer consecutivo del modulo de proyectos, DDP_PI_autoconsecutivo
                    DP.EstadoSolicitudCodigo = "4"; // Sin registrar

                    _context.DisponibilidadPresupuestal.Add(DP);
                    return respuesta = new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Data = DP,
                        Code = ConstantMessagesDisponibilidadPresupuesta.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesDisponibilidadPresupuesta.OperacionExitosa, idAccion, DP.UsuarioCreacion, strCrearEditar)
                    };

                }
                else
                {
                    strCrearEditar = "EDITAR DISPONIBILIDAD PRESUPUESTAL PROYECTO";
                    disponibilidadPresupuestalAntiguo = _context.DisponibilidadPresupuestal.Find(DP.DisponibilidadPresupuestalId);
                    //Auditoria
                    disponibilidadPresupuestalAntiguo.UsuarioModificacion = "jsorozco";
                    disponibilidadPresupuestalAntiguo.FechaModificacion = DateTime.Now;

                    //Registros
                    disponibilidadPresupuestalAntiguo.FechaSolicitud = DP.FechaSolicitud;
                    disponibilidadPresupuestalAntiguo.TipoSolicitudCodigo = DP.TipoSolicitudCodigo;
                    disponibilidadPresupuestalAntiguo.NumeroSolicitud = DP.NumeroSolicitud;
                    disponibilidadPresupuestalAntiguo.OpcionContratarCodigo = DP.OpcionContratarCodigo;
                    disponibilidadPresupuestalAntiguo.ValorSolicitud = DP.ValorSolicitud;
                    disponibilidadPresupuestalAntiguo.EstadoSolicitudCodigo = DP.EstadoSolicitudCodigo;
                    disponibilidadPresupuestalAntiguo.Objeto = DP.Objeto;
                    disponibilidadPresupuestalAntiguo.FechaDdp = DP.FechaDdp;
                    disponibilidadPresupuestalAntiguo.NumeroDdp = DP.NumeroDdp;
                    disponibilidadPresupuestalAntiguo.RutaDdp = DP.RutaDdp;
                    //disponibilidadPresupuestalAntiguo.Observacion = DP.Observacion;
                    disponibilidadPresupuestalAntiguo.Eliminado = false;

                    _context.DisponibilidadPresupuestal.Update(disponibilidadPresupuestalAntiguo);
                }

                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = disponibilidadPresupuestalAntiguo,
                    Code = ConstantMessagesDisponibilidadPresupuesta.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Cronograma, ConstantMessagesDisponibilidadPresupuesta.OperacionExitosa, idAccion, DP.UsuarioCreacion, strCrearEditar)
                };
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesDisponibilidadPresupuesta.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Cronograma, ConstantMessagesDisponibilidadPresupuesta.Error, idAccion, DP.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<List<GrillaDisponibilidadPresupuestal>> GetGridBudgetAvailability(int? DisponibilidadPresupuestalId)
        {

            List<DisponibilidadPresupuestal> ListDisponibilidadPresupuestal =
               DisponibilidadPresupuestalId != null ? await _context.DisponibilidadPresupuestal.Where(x => x.DisponibilidadPresupuestalId == DisponibilidadPresupuestalId).ToListAsync()
               : await _context.DisponibilidadPresupuestal.OrderByDescending(a => a.FechaSolicitud).ToListAsync();

            List<GrillaDisponibilidadPresupuestal> ListGrillaControlCronograma = new List<GrillaDisponibilidadPresupuestal>();

            foreach (var dp in ListDisponibilidadPresupuestal)
            {
                GrillaDisponibilidadPresupuestal DisponibilidadPresupuestalGrilla = new GrillaDisponibilidadPresupuestal
                {
                    DisponibilidadPresupuestalId = dp.DisponibilidadPresupuestalId,
                    FechaSolicitud = dp.FechaSolicitud,
                    TipoSolicitudCodigo = dp.TipoSolicitudCodigo,
                    TipoSolicitudText = dp.TipoSolicitudCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(dp.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Solicitud) : "",
                    NumeroSolicitud = dp.NumeroSolicitud,
                    OpcionPorContratarCodigo = dp.OpcionContratarCodigo,
                    OpcionPorContratarText = dp.OpcionContratarCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(dp.OpcionContratarCodigo, (int)EnumeratorTipoDominio.Opcion_Por_Contratar) : "",
                    ValorSolicitado = dp.ValorSolicitud,
                    EstadoSolicitudCodigo = dp.EstadoSolicitudCodigo,
                    EstadoSolicitudText = dp.EstadoSolicitudCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(dp.EstadoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Solicitud_Disponibilidad_Presupuestal) : "",


                };

                ListGrillaControlCronograma.Add(DisponibilidadPresupuestalGrilla);
            }

            return ListGrillaControlCronograma;
        }
         
        public async Task<byte[]> GetPDFDDP(int id, string pUsurioGenero)
        {
            if (id == 0)
            {
                return Array.Empty<byte>();
            }
            DisponibilidadPresupuestal disponibilidad = await _context.DisponibilidadPresupuestal
                .Where(r => r.DisponibilidadPresupuestalId == id).FirstOrDefaultAsync();
            //.Include(r => r.SesionComiteTema).FirstOrDefaultAsync();

            if (disponibilidad == null)
            {
                return Array.Empty<byte>();
            }
            Plantilla plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Ficha_De_DDP).ToString()).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();

            plantilla.Contenido = ReemplazarDatosDDP(plantilla.Contenido, disponibilidad);
            return ConvertirPDF(plantilla);
        }

        private byte[] ConvertirPDF(Plantilla pPlantilla)
        {
            string strEncabezado = "";
            if (!string.IsNullOrEmpty(pPlantilla.Encabezado.Contenido))
            {
                strEncabezado = Helpers.Helpers.HtmlStringLimpio(pPlantilla.Encabezado.Contenido);
            }

            var globalSettings = new GlobalSettings
            {
                ImageQuality = 1080,
                PageOffset = 0,
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings
                {
                    Top = pPlantilla.MargenArriba,
                    Left = pPlantilla.MargenIzquierda,
                    Right = pPlantilla.MargenDerecha,
                    Bottom = pPlantilla.MargenAbajo
                },
                DocumentTitle = DateTime.Now.ToString(),
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = pPlantilla.Contenido,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "pdf-styles.css") },
                HeaderSettings = { FontName = "Roboto", FontSize = 8, Center = strEncabezado, Line = false, Spacing = 18 },
                FooterSettings = { FontName = "Ariel", FontSize = 10, Center = "[page]" },
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            return _converter.Convert(pdf);
        }

        private string ReemplazarDatosDDP(string pStrContenido, DisponibilidadPresupuestal pDisponibilidad)
        {
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolderDDP).ToList();
            foreach (var place in placeholders)
            {
                switch (place.Codigo)
                {
                    case ConstanCodigoVariablesPlaceHolders.DDP_FECHA:
                        pStrContenido = pStrContenido
                            .Replace(place.Nombre, pDisponibilidad.FechaCreacion.ToString("dd/MM/yyy"));
                        break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NUMERO_SOLICITUD:
                        pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.NumeroSolicitud); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NO:
                        pStrContenido =
pStrContenido.Replace(place.Nombre, pDisponibilidad.NumeroDdp); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_RUBRO_POR_FINANCIAR:
                        pStrContenido = pStrContenido.Replace(place.Nombre, _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal
                                && r.Codigo == pDisponibilidad.TipoSolicitudCodigo).FirstOrDefault().Descripcion); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TIPO_SOLICITUD:
                        pStrContenido =
pStrContenido.Replace(place.Nombre, pDisponibilidad.TipoSolicitudCodigo != null ? _context.Dominio.Where(r => r.Codigo == pDisponibilidad.TipoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud).FirstOrDefault().Nombre : ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_OPCION_CONTRATAR: pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.OpcionContratarCodigo); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_FECHA_COMITE_TECNICO: pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.FechaCreacion.ToString("dd/MM/yyyy")); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NUMERO_COMITE: pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.NumeroSolicitud); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_OBJETO: pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.Objeto); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TABLAAPORTANTES: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TOTAL_DE_RECURSOS: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TOTAL_DE_RECURSOSLETRAS: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TABLA_PROYECTOS: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_LIMITACION_ESPECIAL: pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.LimitacionEspecial); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NOMBRE_APORTANTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_FUENTE_APORTANTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_VALOR_NUMERO: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_VALOR_LETRAS: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;

                    case ConstanCodigoVariablesPlaceHolders.DDP_LLAVE_MEN: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_INSTITUCION_EDUCATIVA: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_SEDE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_FUENTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_SALDO_ACTUAL_FUENTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_VALOR_SOLICITADO_FUENTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NUEVO_SALDO_FUENTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;


                }
            }
            return pStrContenido;
        }

        /*autor: jflorez
            descripción: return disponibilidad por validacion pres
        impacto: CU 3.3.2*/

        public async Task<Respuesta> SetReturnValidacionDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Devuelta_por_validacion_presupuestal;
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion;
                DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();

                pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();
                _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                _context.SaveChanges();
                //envio correo a tecnico
                var usuarioTecnico = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario).FirstOrDefault();
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DisponibilidadPresupuestalGenerada);
                string template = TemplateRecoveryPassword.Contenido;
                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioTecnico.Usuario.Email, "SDP Devuelto por validación presupuestal", template, pSentender, pPassword, pMailServer, pMailPort);
                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.DevueltoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.DevueltoCorrrectamente, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, "RECHAZAR DISPONIBILIDAD PRESUPUESTAL")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        /*autor: jflorez
            descripción: rechaza disponibilidad por validacion pres
        impacto: CU 3.3.2*/
        public async Task<Respuesta> SetRechazarValidacionDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Rechazada_por_validacion_presupuestal;
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion;
                DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();

                pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();
                _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                _context.SaveChanges();
                //envio correo a técnico
                var usuarioTecnico = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario).FirstOrDefault();
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DisponibilidadPresupuestalGenerada);
                string template = TemplateRecoveryPassword.Contenido;
                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioTecnico.Usuario.Email, "SDP rechazado por validación presupuestal", template, pSentender, pPassword, pMailServer, pMailPort);
                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.DevueltoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.DevueltoCorrrectamente, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, "DEVOLVER DISPONIBILIDAD PRESUPUESTAL")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        /*autor: jflorez
            descripción: valida disponibilidad por validacion pres
        impacto: CU 3.3.2*/
        public async Task<Respuesta> SetValidarValidacionDDP(int id, string usuariomod, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(id);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Con_validacion_presupuestal;
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = usuariomod;
                DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();
                /*
                pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();
                _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                _context.SaveChanges();*/
                //envio correo a juridica
                var usuarioJuridico = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Financiera).Include(y => y.Usuario).FirstOrDefault();
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DisponibilidadPresupuestalGenerada);
                string template = TemplateRecoveryPassword.Contenido;
                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioJuridico.Usuario.Email, "SDP con validación presupuestal", template, pSentender, pPassword, pMailServer, pMailPort);
                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.DevueltoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.DevueltoCorrrectamente, idAccion, usuariomod, "CON DISPONIBILIDAD PRESUPUESTAL")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, usuariomod, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        public async Task<Respuesta> CreateFinancialFundingGestion(GestionFuenteFinanciacion pDisponibilidadPresObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {

                var valoresSolicitados = _context.GestionFuenteFinanciacion.Where(x => x.FuenteFinanciacionId == pDisponibilidadPresObservacion.FuenteFinanciacionId).Sum(x => x.ValorSolicitado);
                var fuente = _context.FuenteFinanciacion.Find(pDisponibilidadPresObservacion.FuenteFinanciacionId);
                pDisponibilidadPresObservacion.SaldoActual = (decimal)fuente.ValorFuente - valoresSolicitados;
                pDisponibilidadPresObservacion.NuevoSaldo = pDisponibilidadPresObservacion.SaldoActual - pDisponibilidadPresObservacion.ValorSolicitado;
                int estado = (int)EnumeratorEstadoGestionFuenteFinanciacion.Solicitado;
                pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                pDisponibilidadPresObservacion.EstadoCodigo = estado.ToString();
                pDisponibilidadPresObservacion.Eliminado = false;
                _context.GestionFuenteFinanciacion.Add(pDisponibilidadPresObservacion);
                _context.SaveChanges();
                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.DevueltoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.OperacionExitosa, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, "SOLICITUD A FUENTE DE FINANCIACION CREADA")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        public async Task<Respuesta> DeleteFinancialFundingGestion(int pIdDisponibilidadPresObservacion, string usuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {

                var pDisponibilidadPresObservacion = _context.GestionFuenteFinanciacion.Find(pIdDisponibilidadPresObservacion);
                pDisponibilidadPresObservacion.FechaModificacion = DateTime.Now;
                pDisponibilidadPresObservacion.UsuarioModificacion = usuarioModificacion;
                pDisponibilidadPresObservacion.Eliminado = true;
                _context.SaveChanges();
                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.DevueltoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.OperacionExitosa, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, "SOLICITUD A FUENTE DE FINANCIACION ELIMINADA")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, usuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        public async Task<Respuesta> GetFinancialFundingGestionByDDPP(int pIdDisponibilidadPresupuestalProyecto, string usuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            return new Respuesta
            {
                IsSuccessful = true,
                IsException = false,
                IsValidation = false,
                Code = ConstantMessagesGenerateBudget.Error,
                Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, usuarioModificacion, "TRAER FUENTES GESTIONADAS"),
                Data = _context.GestionFuenteFinanciacion.Where(x => x.DisponibilidadPresupuestalProyectoId == pIdDisponibilidadPresupuestalProyecto).Include(x => x.FuenteFinanciacion).Include(x => x.DisponibilidadPresupuestalProyecto).ThenInclude(x => x.Proyecto).ToList()
            };
        }

        /*autor: jflorez
            descripción: listo los datos ordenados de disponibilidades
        impacto: CU 3.3.4*/
        public async Task<EstadosDisponibilidad> GetListGenerarRegistroPresupuestal()
        {
            EstadosDisponibilidad estadosdisponibles =
                 new EstadosDisponibilidad { DominioId = Convert.ToInt32(ConstanCodigoSolicitudDisponibilidadPresupuestal.Con_Disponibilidad_Presupuestal), NombreEstado = "", DisponibilidadPresupuestal = await this.GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(ConstanCodigoSolicitudDisponibilidadPresupuestal.Con_Disponibilidad_Presupuestal) };
            return estadosdisponibles;
        }
        /*autor: jflorez
           descripción: cancelo la disponibilidades
       impacto: CU 3.3.4*/
        public async Task<Respuesta> SetCancelDisponibilidadPresupuestal(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Sin_registro_presupuestal;
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion;
                DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();

                pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();
                _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.CanceladoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Registro_Presupuestal, ConstantMessagesGenerateBudget.CanceladoCorrrectamente, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, "CANCELAR DISPONIBILIDAD PRESUPUESTAL")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Registro_Presupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        /*autor: jflorez
           descripción: listo los datos ordenados de disponibilidades
       impacto: CU 3.3.4*/
        public async Task<Respuesta> CreateDRP(int pId, string pUsuarioModificacion, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Include(x => x.Contratacion).
                ThenInclude(x => x.ContratacionProyecto).ThenInclude(x => x.ContratacionProyectoAportante).ThenInclude(x => x.CofinanciacionAportante).
                ThenInclude(x => x.FuenteFinanciacion).FirstOrDefault(x => x.DisponibilidadPresupuestalId == pId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            /*busco usuario Juridico*/
            var usuarioJuridico = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Juridica).Include(y => y.Usuario).FirstOrDefault();
            int consecutivo = _context.DisponibilidadPresupuestal.Where(x => x.NumeroDrp != null).Count();
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Con_registro_presupuestal;
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = pUsuarioModificacion.ToUpper();
                DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();
                DisponibilidadCancelar.NumeroDrp = "DRP_PI_" + consecutivo.ToString();
                DisponibilidadCancelar.FechaDrp = DateTime.Now;
                //
                //guardar el tema de platas
                //
                var gestionfuentes = _context.GestionFuenteFinanciacion.Where(x => x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == DisponibilidadCancelar.DisponibilidadPresupuestalId);
                foreach (var gestion in gestionfuentes)
                {
                    gestion.EstadoCodigo = EnumeratorEstadoGestionFuenteFinanciacion.Gestionado_en_DRP.ToString();
                    gestion.FechaModificacion = DateTime.Now;
                    gestion.UsuarioModificacion = pUsuarioModificacion.ToUpper();
                    _context.GestionFuenteFinanciacion.Update(gestion);
                }
                _context.SaveChanges();
                //envio correo a juridica
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DisponibilidadPresupuestalGenerada);
                string template = TemplateRecoveryPassword.Contenido;

                //template = template.Replace("_Link_", urlDestino);                

                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioJuridico.Usuario.Email, "DRP Generada", template, pSentender, pPassword, pMailServer, pMailPort);
                if (blEnvioCorreo)
                {
                    return new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantMessagesGenerateBudget.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Registro_Presupuestal, ConstantMessagesGenerateBudget.OperacionExitosa, idAccion, pUsuarioModificacion, "GENERAR DRP REGISTRO PRESUPUESTAL")
                    };
                }
                else
                {
                    return new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantMessagesGenerateBudget.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Registro_Presupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pUsuarioModificacion, "ERROR ENVIO MAIL GENERAR DRP REGISTRO PRESUPUESTAL")
                    };
                }
            }
            catch (Exception ex)
            {
                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Registro_Presupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<byte[]> GetPDFDRP(int id, string usuarioModificacion)
        {
            if (id == 0)
            {
                return Array.Empty<byte>();
            }
            DisponibilidadPresupuestal disponibilidad = await _context.DisponibilidadPresupuestal
                .Where(r => r.DisponibilidadPresupuestalId == id).FirstOrDefaultAsync();
            //.Include(r => r.SesionComiteTema).FirstOrDefaultAsync();

            if (disponibilidad == null)
            {
                return Array.Empty<byte>();
            }
            Plantilla plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Ficha_De_DDP).ToString()).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();

            plantilla.Contenido = ReemplazarDatosDDP(plantilla.Contenido, disponibilidad);
            return ConvertirPDF(plantilla);
        }
    }
}