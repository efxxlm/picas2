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
                    strTipoSolicitud = await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal);
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
                return await _context.DisponibilidadPresupuestal.Where( d => d.DisponibilidadPresupuestalId == id) 
                                    .Include( r => r.DisponibilidadPresupuestalProyecto )
                                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(string pCodigoEstadoSolicitud) {

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
                    strTipoSolicitud = await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_de_Solicitud);
                }

                DisponibilidadPresupuestalGrilla disponibilidadPresupuestalGrilla = new DisponibilidadPresupuestalGrilla
                {

                    FechaSolicitud = DisponibilidadPresupuestal.FechaSolicitud.ToString("yyyy/MM/dd"),
                    EstadoRegistro = strEstadoRegistro,
                    TipoSolicitud = strTipoSolicitud,
                    DisponibilidadPresupuestalId = DisponibilidadPresupuestal.DisponibilidadPresupuestalId,
                    NumeroSolicitud = DisponibilidadPresupuestal.NumeroSolicitud
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
                estadosdisponibles.Add(new EstadosDisponibilidad{DominioId=estado.DominioId,NombreEstado=estado.Nombre,DisponibilidadPresupuestal=await this.GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(estado.Codigo)});
            }
            return estadosdisponibles;
        }

        public async Task<Respuesta> SetCancelDisponibilidadPresupuestal(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion)
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
            /*busco usuario Juridico*/
            var usuarioJuridico = _context.UsuarioPerfil.Where(x=>x.PerfilId==(int)EnumeratorPerfil.Juridica).Include(y=>y.Usuario).FirstOrDefault();
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Con_validacion_presupuestal;
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = pUsuarioModificacion;
                DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();
                //
                //guardar el tema de platas
                //
                
                Dictionary<int, List<decimal>> fuente =new Dictionary<int, List<decimal>>();
                var contratacionproyecto = DisponibilidadCancelar.Contratacion.ContratacionProyecto;
                foreach(var contratpro in contratacionproyecto)
                {
                    var proyectoAportante=contratpro.ContratacionProyectoAportante;
                    foreach(var proAport in proyectoAportante)
                    {
                        var fuentes=proAport.CofinanciacionAportante.FuenteFinanciacion;
                        foreach(var fuent in fuentes)
                        {
                            List<decimal> valores = new List<decimal>();
                            valores.Add(fuent.ValorFuente);
                            valores.Add(proAport.ValorAporte);
                            fuente.Add(fuent.FuenteFinanciacionId, valores);
                        }                        
                    }
                }
                foreach(var f in fuente)
                {
                    GestionFuenteFinanciacion gf = new GestionFuenteFinanciacion();
                    gf.UsuarioCreacion = pUsuarioModificacion;
                    gf.FechaCreacion = DateTime.Now;
                    gf.Eliminado = false;
                    gf.FuenteFinanciacionId = f.Key;
                    gf.SaldoActual = f.Value[0]- f.Value[1];
                    gf.NuevoSaldo = f.Value[0] - f.Value[1];
                    gf.ValorSolicitado = f.Value[1];
                    _context.GestionFuenteFinanciacion.Add(gf);
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

        private string ReemplazarDatosDDP(string contenido, DisponibilidadPresupuestal disponibilidad)
        {
            return contenido;
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
                //envio correo a juridica
                var usuarioJuridico = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Juridica).Include(y => y.Usuario).FirstOrDefault();
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DisponibilidadPresupuestalGenerada);
                string template = TemplateRecoveryPassword.Contenido;
                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioJuridico.Usuario.Email, "SDP Devuelto por validación presupuestal", template, pSentender, pPassword, pMailServer, pMailPort);
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
                //envio correo a juridica
                var usuarioJuridico = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Juridica).Include(y => y.Usuario).FirstOrDefault();
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DisponibilidadPresupuestalGenerada);
                string template = TemplateRecoveryPassword.Contenido;
                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioJuridico.Usuario.Email, "SDP rechazado por validación presupuestal", template, pSentender, pPassword, pMailServer, pMailPort);
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
        public async Task<Respuesta> SetValidarValidacionDDP(int id,string usuariomod, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
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
                
                var valoresSolicitados= _context.GestionFuenteFinanciacion.Where(x => x.FuenteFinanciacionId == pDisponibilidadPresObservacion.FuenteFinanciacionId).Sum(x => x.ValorSolicitado);
                var fuente = _context.FuenteFinanciacion.Find(pDisponibilidadPresObservacion.FuenteFinanciacionId);
                pDisponibilidadPresObservacion.SaldoActual = fuente.ValorFuente - valoresSolicitados;
                pDisponibilidadPresObservacion.NuevoSaldo = pDisponibilidadPresObservacion.SaldoActual - pDisponibilidadPresObservacion.ValorSolicitado;
                int estado = (int)EnumeratorEstadoGestionFuenteFinanciacion.Solicitado;
                pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                pDisponibilidadPresObservacion.EstadoCodigo= estado.ToString();
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
                Data = _context.GestionFuenteFinanciacion.Where(x=>x.DisponibilidadPresupuestalProyectoId==pIdDisponibilidadPresupuestalProyecto).Include(x=>x.FuenteFinanciacion).Include(x=>x.DisponibilidadPresupuestalProyecto).ThenInclude(x=>x.Proyecto).ToList()
            };
        }
        

    }
}