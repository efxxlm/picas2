using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.api;
using asivamosffie.services.Helpers.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace asivamosffie.services
{
   public  class ActBeginService : IActBeginService
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        private readonly IOptions<AppSettingsService> _settings;

        private readonly IDocumentService _documentService;
        public readonly IConverter _converter;

        public ActBeginService(devAsiVamosFFIEContext context, ICommonService commonService, IOptions<AppSettingsService> settings, IDocumentService documentService, IConverter converter)
        {
            _commonService = commonService;
            _context = context;
            _settings = settings;
            _documentService = documentService;
            _converter = converter;
        }

        public async Task<Respuesta> GuardarTieneObservacionesActaInicio(int pContratoId, string pObservacionesActa, string pUsuarioModificacion)
        {
            Respuesta _response = new Respuesta();

            //            ConObervacionesActa - Contrato
            Contrato contrato;
            //contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId && r.Estado == true).FirstOrDefault();
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId ).FirstOrDefault();

            int idAccionCrearActaInicio = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesActaInicio.EditadoCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                contrato.FechaModificacion = DateTime.Now;
                contrato.UsuarioModificacion = pUsuarioModificacion;
                contrato.ConObervacionesActa = true;
                contrato.Observaciones = pObservacionesActa;

                //      CAMBIAR ESTADO “Sin acta generada” a “Con acta generada”.
                //DOM 60  1   Sin acta generada
                //DOM 60  3   Con acta generada
                //contrato.EstadoActa = "3";

                _context.Contrato.Update(contrato);

                            return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesActaInicio.EditadoCorrrectamente,
                    Message =
                    await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2,
                    ConstantMessagesActaInicio.EditadoCorrrectamente, idAccionCrearActaInicio
                    , contrato.UsuarioModificacion, " GUARDAR OBSERVACION CONTRATO ACTA"
                    )
                };

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesActaInicio.ErrorInterno,Message= ex.InnerException.ToString().Substring(0, 500) };
            }
        }
        //_context.Add(contratoPoliza);
                
        public async Task<Respuesta> GuardarPlazoEjecucionFase2Construccion(int pContratoId, int pPlazoFase2PreMeses, int pPlazoFase2PreDias, string pObservacionesConsideracionesEspeciales, string pUsuarioModificacion)
        {
            Respuesta _response = new Respuesta();

            //            ConObervacionesActa - Contrato
            Contrato contrato;
            //contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId && r.Estado == true).FirstOrDefault();
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            ContratoObservacion contratoObservacion= new ContratoObservacion();

            int idAccionCrearActaInicio = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesActaInicio.EditadoCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                contrato.PlazoFase2ConstruccionDias = pPlazoFase2PreDias;
                    contrato.PlazoFase1PreMeses = pPlazoFase2PreMeses;
                _context.Contrato.Update(contrato);
                //contrato.FechaModificacion = DateTime.Now;
                //contrato.UsuarioModificacion = pUsuarioModificacion;
                //contrato.ConObervacionesActa = true;
                //contrato.Observaciones = pObservacionesConsideracionesEspeciales;

                contratoObservacion.Observaciones=pObservacionesConsideracionesEspeciales;
                contratoObservacion.ContratoId = contrato.ContratoId;
                contratoObservacion.EsActaFase2 = true;
                contratoObservacion.EsActa = true;

                contratoObservacion.FechaCreacion = DateTime.Now;
                contratoObservacion.UsuarioCreacion = pUsuarioModificacion;

                //      CAMBIAR ESTADO “Sin acta generada” a “Con acta generada”.
                //DOM 60  1   Sin acta generada
                //DOM 60  3   Con acta generada
                //contrato.EstadoActa = "3";                

                _context.Add(contratoObservacion);
                await _context.SaveChangesAsync();

                //            Plazo de ejecución fase 1 – Preconstrucción: Meses: 4 Días: 3 - PlazoFase1PreMeses - PlazoFase1PreDias - contrato

                //---- - guardar    OK
                //  Plazo de ejecución fase 2 – Construcción: Meses: xx Días: xx - PlazoFase2PreMeses - PlazoFase2PreDias - contrato

                //Observaciones o consideraciones especiales   Observaciones - contrato

                return
             new Respuesta
           {
               IsSuccessful = true,
               IsException = false,
               IsValidation = false,
               Code = ConstantMessagesActaInicio.EditadoCorrrectamente,
               Message =
               await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2,
               ConstantMessagesActaInicio.EditadoCorrrectamente, idAccionCrearActaInicio
               , contrato.UsuarioModificacion, " GUARDAR OBSERVACION CONTRATO ACTA"
               )
           };

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesActaInicio.ErrorInterno, Message = ex.InnerException.ToString().Substring(0, 500) };
            }
        }
        
        public async Task<Respuesta> GuardarCargarActaSuscritaContrato(int pContratoId, DateTime pFechaFirmaContratista, DateTime pFechaFirmaActaContratistaInterventoria
            /* archivo pdf */ , IFormFile pFile, string pDirectorioBase, string pDirectorioActaInicio, string pUsuarioModificacion
            )
        {
            //            Fecha de la firma del documento por parte del contratista de obra -FechaFirmaContratista - contrato
            //Fecha de la firma del documento por parte del contratista de interventoría -FechaFirmaActaContratistaInterventoria - contrato
                        
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Cargar_Acta_Suscrita_Contrato_Fase_2, (int)EnumeratorTipoDominio.Acciones);

            Contrato contrato;

            //contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId && r.Estado == true).FirstOrDefault();
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId ).FirstOrDefault();

            try
            {
                contrato.FechaFirmaContratista = pFechaFirmaContratista;
                contrato.FechaFirmaActaContratistaInterventoriaFase2 = pFechaFirmaActaContratistaInterventoria;
                contrato.UsuarioModificacion = pUsuarioModificacion;

                string strFilePatch = "";
                //Save Files  
                if (pFile == null)
                {
                }
                else
                {
                    if (pFile.Length > 0)
                    {
                        strFilePatch = Path.Combine(pDirectorioBase, pDirectorioActaInicio, pContratoId.ToString());
                        await _documentService.SaveFileContratacion(pFile, strFilePatch, pFile.FileName);

                    }
                }

                //Auditoria
                //contratacionOld.FechaModificacion = DateTime.Now;
                //contratacionOld.UsuarioModificacion = pContratacion.UsuarioCreacion;
                //Registros
                //contratacionOld.RutaMinuta = strFilePatch;
                //contratacionOld.RegistroCompleto = pContratacion.RegistroCompleto;
                //contratacionOld.RutaMinuta = strFilePatch + "//" + pFile.FileName;

                contrato.RutaActaSuscrita = strFilePatch + "//" + pFile.FileName;
                //contratacionOld.RegistroCompleto = ValidarCamposContratacion(contratacionOld);
                _context.Contrato.Update(contrato);

                //                notificación de alerta al interventor, al apoyo a la
                //supervisión y al supervisor

                //notificación al interventor y al apoyo a la supervisión.

                int pIdTemplate = (int)enumeratorTemplate.ActaInicioFase2ObraInterventoria;
                //perfilId = 8; //  Supervisor

                VistaGenerarActaInicioContrato actaInicio;

                //pTipoContrato = 2;
                actaInicio = await getDataActaInicioAsync(pContratoId, Convert.ToInt32( contrato.TipoContratoCodigo));

                //Contrato contrato = null;
                //contrato = _context.Contrato.Where(r => r.NumeroContrato == pActaInicio.NumeroContrato).FirstOrDefault();

                string correo = "cdaza@ivolucion.com";

                correo = getCorreos((int)EnumeratorPerfil.Supervisor);

                Task<Respuesta> result = EnviarCorreoGestionActaIncio(correo, _settings.Value.MailServer,
             _settings.Value.MailPort, _settings.Value.Password, _settings.Value.Sender,
             actaInicio, pIdTemplate);

                return
                 new Respuesta
                 {
                     IsSuccessful = true,
                     IsException = false,
                     IsValidation = false,
                     Code = ConstantGestionarActaInicioFase2.OperacionExitosa,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, ConstantGestionarActaInicioFase2.OperacionExitosa, idAccion, contrato.UsuarioModificacion, "CARGAR ACTA SUSCRITA CONTRATO")
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
                  Code = ConstantGestionarProcesosContractuales.Error,
                  Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, ConstantGestionarActaInicioFase2.Error, idAccion, contrato.UsuarioModificacion, ex.InnerException.ToString())
              };
            }
        }

        private string getCorreos(int perfilId)
        {
            //[Usuario], [UsuarioPerfil] , [Perfil]
            string lstCorreos = "";

            //            1   Administrador  - //2   Técnica
            //3   Financiera - //4   Jurídica
            //5   Administrativa - //6   Miembros Comite
            //7   Secretario comité - //8   Supervisor
            List<UsuarioPerfil> lstUsuariosPerfil = new List<UsuarioPerfil>();

            lstUsuariosPerfil = _context.UsuarioPerfil.Where(r => r.Activo == true && r.PerfilId == perfilId).ToList();

            List<Usuario> lstUsuarios = new List<Usuario>();

            foreach (var item in lstUsuariosPerfil)
            {
                lstUsuarios = _context.Usuario.Where(r => r.UsuarioId == item.UsuarioId).ToList();

                foreach (var usuario in lstUsuarios)
                {
                    lstCorreos = lstCorreos += usuario.Email + ";";
                }
            }
            return lstCorreos;
        }


        public async Task<byte[]> GetPlantillaActaInicio(int pContratoId)
        {
            if (pContratoId == 0)
            {
                return Array.Empty<byte>();
            }

            VistaGenerarActaInicioContrato actaInicio;

            int pTipoContrato = 2;

            //               --Contratacion.TipoContratacionCodigo  14 DOM, tipoidentificacion

            ////DOM 14 1   Obra            
            //pTipoContrato = 1;

            //DOM 14 2   interventoria            
            pTipoContrato = 2;
            actaInicio = await getDataActaInicioAsync(pContratoId, pTipoContrato);

            if (actaInicio == null)
            {
                return Array.Empty<byte>();
            }
            Plantilla plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Acta_inicio_obra_PDF).ToString()).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();

            //Plantilla plantilla = new Plantilla();
            //plantilla.Contenido = "";

            plantilla.Contenido = await ReemplazarDatosPlantillaActaInicio(plantilla.Contenido, actaInicio,"cdaza");
            return ConvertirPDF(plantilla);
        }

        private async Task<string> ReemplazarDatosPlantillaActaInicio(string strContenido, VistaGenerarActaInicioContrato pActaInicio, string usuario)
        {
            string str = "";
            string valor = "";

            strContenido = strContenido.Replace("_Numero_Identificacion_Entidad_Contratista_Interventoria_", pActaInicio.NumeroIdentificacionEntidadContratistaObra);
            strContenido = strContenido.Replace("_Nombre_Representante_Contratista_Interventoria_", pActaInicio.NombreRepresentanteContratistaInterventoria);
            strContenido = strContenido.Replace("_Nombre_Entidad_Contratista_Interventoria_", pActaInicio.NombreEntidadContratistaSupervisorInterventoria);

            strContenido = strContenido.Replace("_Nombre_Representante_Contratista_Obra_", pActaInicio.NombreRepresentanteContratistaObra);
            strContenido = strContenido.Replace("_Nombre_Representante_Contratista_Interventoria_", pActaInicio.NombreRepresentanteContratistaObra);

            strContenido = strContenido.Replace("_Nombre_Entidad_Contratista_Obra_", pActaInicio.NombreEntidadContratistaObra);
            strContenido = strContenido.Replace("_Nombre_Entidad_Contratista_Interventoria_", pActaInicio.NombreEntidadContratistaObra);

            strContenido = strContenido.Replace("_Numero_Identificacion_Entidad_Contratista_Obra_", pActaInicio.NumeroIdentificacionEntidadContratistaObra);
            strContenido = strContenido.Replace(" _Fecha_Prevista_Terminacion_", pActaInicio.FechaPrevistaTerminacion);
            strContenido = strContenido.Replace("_OBSERVACION_O_CONSIDERACIONES_ESPECIALES_", pActaInicio.ObservacionOConsideracionesEspeciales);
            strContenido = strContenido.Replace("_Plazo_Ejecucion_Fase_1_Preconstruccion_", "");
            strContenido = strContenido.Replace("_Plazo_Ejecucion_Fase_2_Construccion_", valor);
            strContenido = strContenido.Replace("_Valor_Actual_Contrato_", pActaInicio.ValorActualContrato);
            strContenido = strContenido.Replace("_Plazo_Inicial_Contrato_", pActaInicio.PlazoInicialContratoSupervisor);
            strContenido = strContenido.Replace("_Valor_Fase_1_preconstruccion_", pActaInicio.ValorFase1Preconstruccion);
            strContenido = strContenido.Replace("_Valor_Fase_2_Construccion_Obra_", pActaInicio.Valorfase2ConstruccionObra);
            strContenido = strContenido.Replace("_Nombre_Entidad_Contratista_Obra_", pActaInicio.NombreEntidadContratistaObra);
            strContenido = strContenido.Replace("_Valor_Inicial_Contrato_", pActaInicio.ValorInicialContrato);
            strContenido = strContenido.Replace("_Fecha_Aprobacion_Garantia_Poliza_", pActaInicio.FechaAprobacionGarantiaPoliza);
            strContenido = strContenido.Replace("_Objeto_", pActaInicio.Objeto);
            strContenido = strContenido.Replace("_Numero_DRP_", pActaInicio.NumeroDRP1);
            strContenido = strContenido.Replace("_Fecha_Generación_DRP_", pActaInicio.FechaGeneracionDRP1);
            strContenido = strContenido.Replace("_Institucion_Educativa_Llave_MEN_", pActaInicio.InstitucionEducativaLlaveMEN);
            strContenido = strContenido.Replace("_Departamento_y_Municipio_Llave_MEN_", pActaInicio.DepartamentoYMunicipioLlaveMEN);
            strContenido = strContenido.Replace("_Fecha_Acta_Inicio_", pActaInicio.FechaActaInicio);
            strContenido = strContenido.Replace("_Llave_MEN_Contrato_", pActaInicio.LlaveMENContrato);
            strContenido = strContenido.Replace("_Numero_Identificacion_Entidad_Contratista_Obra_", pActaInicio.NumeroIdentificacionEntidadContratistaObra);
            strContenido = strContenido.Replace("_Numero_Contrato_Obra_", pActaInicio.NumeroContrato);
            strContenido = strContenido.Replace("_Nombre_Entidad_Contratista_Obra_", pActaInicio.NombreEntidadContratistaObra);
            strContenido = strContenido.Replace("_Numero_Identificacion_Contratista_Interventoria_", pActaInicio.NumeroIdentificacionContratistaInterventoria);
            //strContenido = strContenido.Replace("_Nombre_Representante_Contratista_Obra_", pActaInicio.NombreRepresentanteContratistaObra);
            //strContenido = strContenido.Replace("_Nombre_Representante_Contratista_Interventoria_", valor);
            //strContenido = strContenido.Replace("_Nombre_Entidad_Contratista_Interventoria_", valor);
            //strContenido = strContenido.Replace("_Fecha_Acta_Inicio_", valor);
            //strContenido = strContenido.Replace("_Numero_Contrato_Obra_", valor);

            //datos exclusivos interventoria

            UsuarioPerfil UsuarioPerfil = _context.UsuarioPerfil.Where(y => y.Usuario.Email == usuario).Include(y => y.Perfil).FirstOrDefault();

            Perfil perfil=null;

            if (UsuarioPerfil != null) { 
             perfil = _context.Perfil.Where(y => y.PerfilId == UsuarioPerfil.PerfilId).FirstOrDefault();

        }
            if (UsuarioPerfil != null)
            {
                strContenido = strContenido.Replace("_Cargo_Usuario_", perfil.Nombre);
            }
            strContenido = strContenido.Replace("_Nombre_Supervisor_", "_Nombre_Supervisor_");          

            return strContenido;

        }

        public byte[] ConvertirPDF(Plantilla pPlantilla)
        {
            string strEncabezado = " encabezado";

            //pendiente
            //if (!string.IsNullOrEmpty(pPlantilla.Encabezado.Contenido))
            //{
                pPlantilla.Contenido = pPlantilla.Contenido.Replace("[RUTA_ICONO]", Path.Combine(Directory.GetCurrentDirectory(), "assets", "img-FFIE.png"));
            //    pPlantilla.Encabezado.Contenido = pPlantilla.Encabezado.Contenido.Replace("[RUTA_ICONO]", Path.Combine(Directory.GetCurrentDirectory(), "assets", "img-FFIE.png"));
            //    strEncabezado = Helpers.Helpers.HtmlStringLimpio(pPlantilla.Encabezado.Contenido);
            //}

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

        public async Task<Respuesta> EnviarCorreoGestionActaIncio(string lstMails, string pMailServer, int pMailPort, string pPassword, string pSentender, VistaGenerarActaInicioContrato pActaInicio,  int pIdTemplate)
        {
            bool blEnvioCorreo = false;
            Respuesta respuesta = new Respuesta();

            //Si no llega Email
            //if (string.IsNullOrEmpty(pUsuario.Email))
            //{
            //    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesUsuarios.EmailObligatorio };
            //}
            try
            {
                //Usuario usuarioSolicito = _context.Usuario.Where(r => !(bool)r.Eliminado && r.Email.ToUpper().Equals(pUsuario.Email.ToUpper())).FirstOrDefault();

                //Guardar Usuario
                //await UpdateUser(usuarioSolicito);

                //Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjSupervisorGestionPoliza);
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

                string template = TemplateRecoveryPassword.Contenido;

                //string urlDestino = pDominio;
                //asent/img/logo      

                //VistaGenerarActaInicioContrato pActaInicio = new VistaGenerarActaInicioContrato();

                int tipoContrato = 0;
                

                Contrato contrato=null;
                contrato = _context.Contrato.Where(r => r.NumeroContrato == pActaInicio.NumeroContrato).FirstOrDefault();
                //template = template.Replace("_Numero_Contrato_", objVistaContratoGarantiaPoliza.NumeroContrato);
                //objVistaContratoGarantiaPoliza.FEC

                //tipoContrato = 2;
                tipoContrato = ConstanCodigoTipoContratacion.Interventoria;

                //if (contrato != null)
                //{
                //    pActaInicio = await getDataActaInicioAsync(contrato.ContratoId, Convert.ToInt32( contrato.TipoContratoCodigo));
                //}                

                template = template.Replace("_Numero_Contrato_", pActaInicio.NumeroContrato);
                template = template.Replace("_Fecha_Aprobacion_Poliza_", pActaInicio.FechaAprobacionGarantiaPoliza);
                
                if(Convert.ToInt32(contrato.TipoContratoCodigo)==ConstanCodigoTipoContratacion.Interventoria)
                    template = template.Replace("_Obra_O_Interventoria_", "interventoría");
                else if (Convert.ToInt32(contrato.TipoContratoCodigo) == ConstanCodigoTipoContratacion.Obra)
                    template = template.Replace("_Obra_O_Interventoria_", "obra");

                template = template.Replace("_Cantidad_Proyectos_Asociados_", pActaInicio.CantidadProyectosAsociados.ToString());

                template = template.Replace("_Fecha_Acta_Inicio_", pActaInicio.FechaActaInicio);
                template = template.Replace("_Fecha_Prevista_Terminacion_", pActaInicio.FechaPrevistaTerminacion);

                //datos basicos generales, aplican para los 4 mensajes

                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(lstMails, "Gestión Acta Inicio Fase II", template, pSentender, pPassword, pMailServer, pMailPort);

                if (blEnvioCorreo)
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantGestionarActaInicioFase2.CorreoEnviado };

                else
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantGestionarActaInicioFase2.ErrorEnviarCorreo };

                //}
                //}
                //else
                //{
                //    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesContratoPoliza.CorreoNoExiste };

                //}
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, respuesta.Code, Convert.ToInt32( ConstantCodigoAcciones.Notificacion_Gestion_Poliza), lstMails, "Gestión Pólizas");
                return respuesta;


            }
            catch (Exception ex)
            {

                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesUsuarios.ErrorGuardarCambios };
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificacion_Gestion_Poliza), lstMails, "Gestión Acta Inicio Fase II") + ": " + ex.ToString() + ex.InnerException;
                return respuesta;
            }

        }

        public void replaceTags()
        {
            string str="";
            string valor="";

            str = str.Replace("_Numero_Identificacion_Entidad_Contratista_Interventoria_", valor);
            str = str.Replace("_Nombre_Representante_Contratista_Interventoria_", valor);
            str = str.Replace("_Nombre_Entidad_Contratista_Interventoria_", valor);
            str = str.Replace("_Nombre_Representante_Contratista_Obra_", valor);
            str = str.Replace("_Nombre_Entidad_Contratista_Obra _", valor);
            str = str.Replace("_Numero_Identificacion_Entidad_Contratista_Obra_", valor);
            str = str.Replace(" _Fecha_Prevista_Terminacion_", valor);
            str = str.Replace("_OBSERVACION_O_CONSIDERACIONES_ESPECIALES_", valor);
            str = str.Replace("_Plazo_Ejecucion_Fase_1_Preconstruccion_", valor);
            str = str.Replace("_Plazo_Ejecucion_Fase_2_Construccion_", valor);
            str = str.Replace("_Valor_Actual_Contrato_", valor);
            str = str.Replace("_Plazo_Inicial_Contrato_", valor);
            str = str.Replace("_Valor_Fase_1_preconstruccion_", valor);
            str = str.Replace("_Valor_Fase_2_Construccion_Obra_", valor);
            str = str.Replace("_Nombre_Entidad_Contratista_Obra_", valor);
            str = str.Replace("_Valor_Inicial_Contrato_", valor);
            str = str.Replace("_Fecha_Aprobacion_Garantia_Poliza_", valor);
            str = str.Replace("_Objeto_", valor);
            str = str.Replace("_Numero_DRP_", valor);
            str = str.Replace("_Fecha_Generación_DRP_", valor);
            str = str.Replace("_Institucion_Educativa_Llave_MEN_", valor);
            str = str.Replace("_Departamento_y_Municipio_Llave_MEN_", valor);
            str = str.Replace("_Fecha_Acta_Inicio_", valor);
            str = str.Replace("_Llave_MEN_Contrato_", valor);
            str = str.Replace("_Numero_Identificacion_Entidad_Contratista_Obra_", valor);
            str = str.Replace("_Numero_Contrato_Obra_ ", valor);
            str = str.Replace("_Nombre_Entidad_Contratista_Obra_", valor);
            str = str.Replace("_Numero_Identificacion_Contratista_Interventoria_", valor);
            str = str.Replace("_Nombre_Representante_Contratista_Obra_", valor);
            str = str.Replace("_Nombre_Representante_Contratista_Interventoria_", valor);
            str = str.Replace("_Nombre_Entidad_Contratista_Interventoria_", valor);
            str = str.Replace("_Fecha_Acta_Inicio_", valor);
            str = str.Replace("_Numero_Contrato_Obra_", valor);
            str = str.Replace("", valor);

        }

        //Task<ActionResult<VistaGenerarActaInicioContrato>> GetListVistaGenerarActaInicio(int pContratoId);
        //GetListVistaGenerarActaInicio


        public async Task<List<GrillaActaInicio>> GetListGrillaActaInicio()
        {
            //            Número del contrato de obra DisponibilidadPresupuestal? contrato - numeroContrato
            //Estado del acta - CONTRATO - EstadoActa - DOM 60
           List<GrillaActaInicio> lstActaInicio = new List<GrillaActaInicio>();
            GrillaActaInicio actaInicio = new GrillaActaInicio();
            List<Contrato> lstContratos = new List<Contrato>();
            lstContratos = _context.Contrato.Where(r => r.Eliminado == false).ToList();
            Contratacion contratacion;

            Dominio EstadoActaFase2Contrato;
            string strEstadoActaFase2Contrato="";

             foreach (var item in lstContratos)
            {
                actaInicio = new GrillaActaInicio();

                EstadoActaFase2Contrato = await _commonService.GetDominioByNombreDominioAndTipoDominio(item.EstadoActaFase2, (int)EnumeratorTipoDominio.Estado_Acta_Contrato);
                //EstadoActaFase2Contrato = await _commonService.GetDominioByNombreDominioAndTipoDominio(item.EstadoActa, (int)EnumeratorTipoDominio.Estado_Acta_Contrato);

                if (EstadoActaFase2Contrato != null)
                    strEstadoActaFase2Contrato = EstadoActaFase2Contrato.Nombre;

                actaInicio.EstadoActa = strEstadoActaFase2Contrato;
                actaInicio.ContratoId = item.ContratoId;
                actaInicio.NumeroContratoObra = item.NumeroContrato;

                contratacion = _context.Contratacion.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefault();
                //actaInicio.FechaAprobacionRequisitos = contratacion.FechaAprobacion.ToString("dd/MM/yyyy");
                actaInicio.FechaAprobacionRequisitos = contratacion.FechaAprobacion != null ? Convert.ToDateTime(contratacion.FechaAprobacion).ToString("dd/MM/yyyy") : contratacion.FechaAprobacion.ToString();
                lstActaInicio.Add(actaInicio);
            }

            return lstActaInicio;

        }

        //public async Task<VistaGenerarActaInicioContrato> GetListVistaGenerarActaInicio(int pContratoId )
        public async Task<VistaGenerarActaInicioContrato> GetListVistaGenerarActaInicio(int pContratoId)
        {
            VistaGenerarActaInicioContrato actaInicioConsolidado = new VistaGenerarActaInicioContrato();

            VistaGenerarActaInicioContrato actaInicioInterventoria = new VistaGenerarActaInicioContrato();
            VistaGenerarActaInicioContrato actaInicioObra = new VistaGenerarActaInicioContrato();
            int pTipoContrato =0;

            //               --Contratacion.TipoContratacionCodigo  14 DOM, tipoidentificacion
            
            //DOM 14 1   Obra            
            pTipoContrato = 1;
            actaInicioObra = await getDataActaInicioAsync(pContratoId, pTipoContrato);
            
            //DOM 14 2   Interventoría
            pTipoContrato = 2;
            actaInicioInterventoria = await getDataActaInicioAsync(pContratoId, pTipoContrato);

            actaInicioConsolidado = await GetDataConsolidadoActaInicioAsync(actaInicioObra, actaInicioInterventoria);

            //return actaInicioObra;
            return actaInicioConsolidado;

        }

        private async Task<VistaGenerarActaInicioContrato> GetDataConsolidadoActaInicioAsync(VistaGenerarActaInicioContrato actaInicioObra, VistaGenerarActaInicioContrato actaInicioInterventoria)
        {

            VistaGenerarActaInicioContrato actaInicioConsolidado = new VistaGenerarActaInicioContrato();

            actaInicioConsolidado = actaInicioObra;
            //interventoria
            actaInicioConsolidado.NumeroIdentificacionRepresentanteContratistaInterventoria = actaInicioInterventoria.NumeroIdentificacionRepresentanteContratistaInterventoria;// Contratista . numeroIdentificaionRepresentante
            actaInicioConsolidado.NombreRepresentanteContratistaInterventoria = actaInicioInterventoria.NombreRepresentanteContratistaInterventoria;

            //obra
            actaInicioConsolidado.NumeroIdentificacionRepresentanteContratistaObraInterventoria = actaInicioObra.NumeroIdentificacionRepresentanteContratistaObraInterventoria;
            actaInicioConsolidado.NombreRepresentanteContratistaObra = actaInicioObra.NombreRepresentanteContratistaObra;

            actaInicioConsolidado.NombreEntidadContratistaObra = actaInicioObra.NombreEntidadContratistaObra;
            actaInicioConsolidado.NombreEntidadContratistaSupervisorInterventoria = actaInicioObra.NombreEntidadContratistaSupervisorInterventoria;

            actaInicioConsolidado.ValorFase1Preconstruccion = actaInicioInterventoria.ValorFase1Preconstruccion;
            actaInicioConsolidado.Valorfase2ConstruccionObra = actaInicioObra.Valorfase2ConstruccionObra;

            actaInicioConsolidado.ValorActualContrato = actaInicioInterventoria.ValorFase1Preconstruccion;
            //actaInicioConsolidado.Valorfase2ConstruccionObra = actaInicioConsolidado.Valorfase2ConstruccionObra + actaInicioConsolidado.ValorFase1Preconstruccion;

        //    Proyecto   ????
        //             decimal? ValorObra 
        // decimal? ValorInterventoria 
        // decimal? ValorTotal 

            //ValorFase1Preconstruccion = " PENDIENTE",
            //        Valorfase2ConstruccionObra
            return actaInicioConsolidado;

        }

        private async Task<VistaGenerarActaInicioContrato> getDataActaInicioAsync(int pContratoId, int pTipoContrato)
        {
            //VistaGenerarActaInicioContrato actaInicio = new VistaGenerarActaInicioContrato();
            VistaGenerarActaInicioContrato actaInicio ;
            try
            {
                //List <Contrato> ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).Include(r => r.NumeroContrato).Include(r => r.Estado).Distinct().ToListAsync();
                Contrato contrato = _context.Contrato.Where(r => !(bool)r.Estado && r.ContratoId == pContratoId && r.TipoContratoCodigo==pTipoContrato.ToString()).FirstOrDefault();
                //cofinanciacion = _context.Cofinanciacion.Where(r => !(bool)r.Eliminado && r.CofinanciacionId == idCofinanciacion).FirstOrDefault();
                string strFechaPrevistaTerminacion = "";
                string strFechaActaInicio = "";

                string strContratoObservacion = "";

                Contratacion contratacion=null;

                ContratoObservacion contratoObservacion=null;

                if (contrato!= null)
                {
                    //contratacion = _context.Contratacion.Where(r => !(bool)r.Estado && r.ContratacionId == contrato.ContratacionId).FirstOrDefault();
                    contratacion = _context.Contratacion.Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();
                    //strFechaActaInicio = contrato.FechaActaInicioFase2.ToString("dd/MM/yyyy");

                    strFechaActaInicio = contrato.FechaActaInicioFase2 != null ? Convert.ToDateTime(contrato.FechaActaInicioFase2).ToString("dd/MM/yyyy") : contrato.FechaActaInicioFase2.ToString();                                                           
                    strFechaPrevistaTerminacion = contrato.FechaTerminacionFase2 != null ? Convert.ToDateTime(contrato.FechaTerminacionFase2).ToString("dd/MM/yyyy") : contrato.FechaTerminacionFase2.ToString();

                    contratoObservacion = _context.ContratoObservacion.Where(r => r.ContratoId == contrato.ContratoId).FirstOrDefault();
                   
                    if(contratoObservacion != null)
                    {
                        strContratoObservacion = contratoObservacion.Observaciones;
                    }
                        
                }

                string strTipoContratacion = "sin definir";
                //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                Dominio TipoContratacionCodigo;

                ContratacionProyecto contratacionProyecto=null;
                string strLlaveMENContrato="";
                string strInstitucionEducativaLlaveMEN="";
                string strDepartamentoYMunicipioLlaveMEN="";

                if (contratacion != null)
                {
                    TipoContratacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratacion.TipoContratacionCodigo, (int)EnumeratorTipoDominio.Opcion_por_contratar);
                    if (TipoContratacionCodigo != null)
                        strTipoContratacion = TipoContratacionCodigo.Nombre;
                                        
                    contratacionProyecto = _context.ContratacionProyecto.Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();

                }

                Contratista contratista;
                contratista = _context.Contratista.Where(r => (bool)r.Activo && r.ContratistaId == contratacion.ContratistaId).FirstOrDefault();

                string strTipoIdentificacionCodigoContratista = "sin definir";
                Dominio TipoIdentificacionCodigoContratista;

                //30  Tipo Documento
                //        tipoIdentificacionCodigo - contratista                   

                if (contratista != null)
                {
                    TipoIdentificacionCodigoContratista = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratista.TipoIdentificacionCodigo, (int)EnumeratorTipoDominio.Tipo_Documento);
                    if (TipoIdentificacionCodigoContratista != null)
                        strTipoIdentificacionCodigoContratista = TipoIdentificacionCodigoContratista.Nombre;
                }
                //contratista obra
                //TipoContratoCodigo 12,37
                //1   Obra

                ContratoPoliza contratoPoliza;
                contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);

                //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                string strTipoSolicitudCodigoContratoPoliza = "sin definir";
                string strEstadoSolicitudCodigoContratoPoliza = "sin definir";

                string strVigencia = "sin definir";

                //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                Dominio TipoSolicitudCodigoContratoPoliza;
                Dominio EstadoSolicitudCodigoContratoPoliza;

                if (contratoPoliza != null)
                {
                    TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Modificacion_Contrato_Poliza);
                    if (TipoSolicitudCodigoContratoPoliza != null)
                        strTipoSolicitudCodigoContratoPoliza = TipoSolicitudCodigoContratoPoliza.Nombre;

                    EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.EstadoPolizaCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                    if (EstadoSolicitudCodigoContratoPoliza != null)
                        strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

                    strVigencia = contratoPoliza.Vigencia.ToString();
                }

                DisponibilidadPresupuestal disponibilidadPresupuestal;
                disponibilidadPresupuestal = _context.DisponibilidadPresupuestal.Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();

                string strNumeroDRP1 = "sin definir";
                string strFechaGeneracionDRP = "sin definir";

                if (disponibilidadPresupuestal!=null)
                {
                    strNumeroDRP1 = disponibilidadPresupuestal.NumeroDrp;
                    //strFechaGeneracionDRP = disponibilidadPresupuestal.FechaDdp.ToString("dd/MM/yyyy");
                    strFechaGeneracionDRP = disponibilidadPresupuestal.FechaDdp != null ? Convert.ToDateTime(disponibilidadPresupuestal.FechaDdp).ToString("dd/MM/yyyy") : disponibilidadPresupuestal.FechaDdp.ToString();
                }
                //disponibilidadPresupuestal = _context.DisponibilidadPresupuestal.Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();
                //                [DisponibilidadPresupuestalProyecto] - [ProyectoId]
                //[ProyectoRequisitoTecnico][ProyectoId]


                //DisponibilidadPresupuestalProyecto disponibilidadPresupuestalProyecto
                // disponibilidadPresupuestalProyecto= _context.DisponibilidadPresupuestalProyecto.Where(r => r.ProyectoId == disponibilidadPresupuestal.id).FirstOrDefault();

                //contratacion.id

                Int32 intCantidadProyectosAsociados = 0;

                intCantidadProyectosAsociados = _context.Proyecto.Where(r => r.ProyectoId == contratacionProyecto.ProyectoId).Count();

                Proyecto proyecto;
                proyecto= _context.Proyecto.Where(r => r.ProyectoId == contratacionProyecto.ProyectoId).FirstOrDefault();
                string InstitucionEducativaId = proyecto.InstitucionEducativaId.ToString();

                proyecto.InstitucionEducativa = await _commonService.GetInstitucionEducativaById( Convert.ToInt32( InstitucionEducativaId));
                         
                                
                if (proyecto!= null)
                {
                     strLlaveMENContrato =proyecto.LlaveMen;
                     strInstitucionEducativaLlaveMEN = proyecto.InstitucionEducativa.Nombre;
                    //[LocalizacionIdMunicipio] [InstitucionEducativaId]
                     strDepartamentoYMunicipioLlaveMEN =_commonService.GetNombreDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    
                }
                //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                //VistaGenerarActaInicioContrato
                actaInicio = new VistaGenerarActaInicioContrato
                {
                    //FechaAprobacionRequisitos="[FechaAprobacionRequisitos] [contrato] FechaAprobacionRequisitos",
                    NumeroContrato = contrato.NumeroContrato,
                    VigenciaContrato = strVigencia, // "2021 PENDIENTE",
                    FechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString(),

                    NumeroDRP1 = strNumeroDRP1,

                    FechaGeneracionDRP1 = strFechaGeneracionDRP,

                    NumeroDRP2 = strNumeroDRP1,
                    FechaGeneracionDRP2 = strFechaGeneracionDRP,
                    //FechaAprobacionGarantiaPoliza = contratoPoliza.FechaAprobacion.ToString("dd/MM/yyyy"),
                    FechaAprobacionGarantiaPoliza = contratoPoliza.FechaAprobacion != null ? Convert.ToDateTime(contratoPoliza.FechaAprobacion).ToString("dd/MM/yyyy") : contratoPoliza.FechaAprobacion.ToString(),
                    Objeto = contrato.Objeto,
                    ValorInicialContrato = contrato.Valor.ToString(),
                    ValorActualContrato = " PENDIENTE",
                    ValorFase1Preconstruccion = " PENDIENTE",
                    Valorfase2ConstruccionObra = " PENDIENTE",
                    PlazoInicialContratoSupervisor = contrato.Plazo.ToString(),

                    NombreEntidadContratistaObra = contratista.Nombre,
                    NombreEntidadContratistaSupervisorInterventoria = contratista.Nombre,

                    FechaActaInicio = strFechaActaInicio,
                    FechaPrevistaTerminacion = strFechaPrevistaTerminacion,
                    ObservacionOConsideracionesEspeciales = strContratoObservacion,

                    LlaveMENContrato = strLlaveMENContrato,
                    DepartamentoYMunicipioLlaveMEN = strDepartamentoYMunicipioLlaveMEN,
                    InstitucionEducativaLlaveMEN = strInstitucionEducativaLlaveMEN,

                    CantidadProyectosAsociados = intCantidadProyectosAsociados,
                    NumeroIdentificacionRepresentanteContratistaInterventoria = contratista.RepresentanteLegalNumeroIdentificacion,

                    //RegistroCompleto = contrato.RegistroCompleto

                    //,EstadoRegistro = "COMPLETO"
                };

                //if (!(bool)proyecto.RegistroCompleto)
                //{
                //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                //}

            }
            catch (Exception e)
            {
                //VistaGenerarActaInicioContrato actaInicio = new VistaGenerarActaInicioContrato
                 actaInicio = new VistaGenerarActaInicioContrato
                {
                    //ContratoId = contrato.ContratoId,
                    //FechaFirma = e.ToString(),
                    //NumeroContrato = e.InnerException.ToString(),

                    //TipoSolicitud = "ERROR"
                    //,
                    //RegistroCompleto = false

                };
            }
            return actaInicio;
        }     
                
    }
    }
