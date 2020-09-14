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
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace asivamosffie.services
{
    public class ManageContractualProcessesService : IManageContractualProcessesService
    {
        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        public readonly IConverter _converter;

        public ManageContractualProcessesService(IConverter converter, devAsiVamosFFIEContext context, ICommonService commonService, IDocumentService documentService)
        {
            _converter = converter;
            _context = context;
            _documentService = documentService;
            _commonService = commonService;
        }

        public async Task<Respuesta> CambiarEstadoSesionComiteSolicitud(SesionComiteSolicitud pSesionComiteSolicitud)
        {

            int idAccionCrearFuentesFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Sesion_Comite_Solicitud, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SesionComiteSolicitud sesionComiteSolicitudOld = await _context.SesionComiteSolicitud.FindAsync(pSesionComiteSolicitud.SesionComiteSolicitudId);
                sesionComiteSolicitudOld.EstadoCodigo = pSesionComiteSolicitud.EstadoCodigo;
                sesionComiteSolicitudOld.FechaModificacion = DateTime.Now;
                sesionComiteSolicitudOld.UsuarioModificacion = pSesionComiteSolicitud.UsuarioCreacion;

                //TODO :se cambia el estado tambien a la contratacion o solo a la contratacion?
                if (false)
                {
                    Contratacion contratacion = _context.Contratacion.Find(pSesionComiteSolicitud.SolicitudId);
                    contratacion.EstadoSolicitudCodigo = pSesionComiteSolicitud.EstadoCodigo;
                    contratacion.FechaModificacion = DateTime.Now;
                    contratacion.UsuarioCreacion = pSesionComiteSolicitud.UsuarioCreacion;
                }
                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = true,
                    Code = ConstantMessagesResourceControl.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesResourceControl.OperacionExitosa, idAccionCrearFuentesFinanciacion, pSesionComiteSolicitud.UsuarioCreacion, "CAMBIAR ESTADO SOLICITUD")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = false,
                    IsValidation = true,
                    Code = ConstantMessagesResourceControl.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesResourceControl.Error, idAccionCrearFuentesFinanciacion, pSesionComiteSolicitud.UsuarioCreacion, ex.InnerException.ToString())
                };
            }

        }


        public async Task<byte[]> GetDDPBySesionComiteSolicitudID(int pSesionComiteSolicitudID)
        {
            //Al modificar verificar a que tipo de solicitud corresponde el comite y hacer switch         
            SesionComiteSolicitud sesionComiteSolicitud = _context.SesionComiteSolicitud.Find(pSesionComiteSolicitudID);
            string TipoPlantilla = "";
            Contratacion contratacion = await GetContratacionByContratacionId(sesionComiteSolicitud.SolicitudId);

            if (contratacion.DisponibilidadPresupuestal.FirstOrDefault().TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo) {
                TipoPlantilla = ((int)ConstanCodigoPlantillas.DDP_Contratacion_Rubro_Administrativo).ToString();
            }
            else
            {
                TipoPlantilla = ((int)ConstanCodigoPlantillas.DDP_Contratacion_Rubro_Por_Financiar).ToString();
            }
            Plantilla Plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();

             
            Plantilla.Contenido = ReemplazarDatosPlantillaContratacion(Plantilla.Contenido, contratacion);

            return ConvertirPDF(Plantilla);
        }

        public byte[] ConvertirPDF(Plantilla pPlantilla)
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
                Orientation = Orientation.Landscape,
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
                HeaderSettings = { FontName = "Roboto", FontSize = 8, Center = strEncabezado, Line = false, Spacing = 18, Right = "Poner logo" },
                FooterSettings = { FontName = "Ariel", FontSize = 10, Center = "[page]" },
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            return _converter.Convert(pdf);
        }

        public string ReemplazarDatosPlantillaContratacion(string pPlantilla, Contratacion pContratacion)
        {

            string TipoPlantillaDDPRegistrosTablaAportante = ((int)ConstanCodigoPlantillas.DDP_Registros_Tabla_Aportante).ToString();
            string PlantillaRegistrosAportante = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaDDPRegistrosTablaAportante).Select(r => r.Contenido).FirstOrDefault();
            string TotalPlantillaRegistrosAportante = "";
            decimal TotalValorSolicitadoFuente = 0;

            string TipoPlantillaDDPProyectosContratacion = ((int)ConstanCodigoPlantillas.Registros_Proyectos_Contratacion_DDP).ToString();
            string PlantillaRegistrosProyectosContratacion = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaDDPProyectosContratacion).Select(r => r.Contenido).FirstOrDefault();
            string TotalRegistrosContratacionProyectos = "";

            List<Dominio> LisParametricas = _context.Dominio.ToList();
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativas = _context.InstitucionEducativaSede.ToList();

            //Registros Tabla Aportantes
            foreach (var ContratacionProyecto in pContratacion.ContratacionProyecto)
            {
                TotalPlantillaRegistrosAportante += TipoPlantillaDDPRegistrosTablaAportante;

                foreach (var ProyectoAportante in ContratacionProyecto.Proyecto.ProyectoAportante)
                {
                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        decimal ValorFuenteNumero = 0;

                        switch (placeholderDominio.Codigo)
                        {
                            case ConstanCodigoVariablesPlaceHolders.NOMBRE_APORTANTE:

                                string NombreAportante = "";
                                if (ProyectoAportante.Aportante.NombreAportanteId > 0)
                                {
                                    NombreAportante = LisParametricas
                                        .Where(r => r.DominioId == ProyectoAportante.Aportante.NombreAportanteId)
                                        .FirstOrDefault().Nombre;
                                }
                                TotalPlantillaRegistrosAportante = TotalPlantillaRegistrosAportante
                                    .Replace(placeholderDominio.Nombre, NombreAportante);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.FUENTE_APORTANTE:
                                string FuenteFinanciacionNombre = "";
                                foreach (var FuenteFinanciacion in ProyectoAportante.Aportante.FuenteFinanciacion)
                                {

                                    bool allDigits3 = FuenteFinanciacion.FuenteRecursosCodigo.All(char.IsDigit);

                                    if (allDigits3 && !string.IsNullOrEmpty(FuenteFinanciacion.FuenteRecursosCodigo))
                                    {
                                        FuenteFinanciacionNombre = LisParametricas
                                           .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Fuente_de_Recurso
                                           && r.Codigo == FuenteFinanciacion.FuenteRecursosCodigo
                                           ).FirstOrDefault().Nombre;
                                    }
                                }
                                TotalPlantillaRegistrosAportante = TotalPlantillaRegistrosAportante
                                     .Replace(placeholderDominio.Nombre, FuenteFinanciacionNombre);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.VALOR_NUMERO:

                                foreach (var FuenteFinanciacion in ProyectoAportante.Aportante.FuenteFinanciacion)
                                {
                                    ValorFuenteNumero += FuenteFinanciacion.ValorFuente;
                                }

                                TotalPlantillaRegistrosAportante = TotalPlantillaRegistrosAportante
                             .Replace(placeholderDominio.Nombre, string.Format("{0:#,0}", ValorFuenteNumero));
                                TotalValorSolicitadoFuente += ValorFuenteNumero;
                                break;

                            case ConstanCodigoVariablesPlaceHolders.VALOR_LETRAS:

                                TotalPlantillaRegistrosAportante = TotalPlantillaRegistrosAportante
                             .Replace(placeholderDominio.Nombre, Conversores.NumeroALetras(ValorFuenteNumero));
                                break;
                        }
                    }
                }
            } 
            //Registros Proyectos
            foreach (var ContratacionProyecto in pContratacion.ContratacionProyecto)
            {
                foreach (Dominio placeholderDominio in placeholders)
                {
                    TotalRegistrosContratacionProyectos += PlantillaRegistrosProyectosContratacion;

                    InstitucionEducativaSede Sede = ListInstitucionEducativas.Where(r=> r.InstitucionEducativaSedeId ==(int)ContratacionProyecto.Proyecto.InstitucionEducativaId).FirstOrDefault();
                    InstitucionEducativaSede institucionEducativa = ListInstitucionEducativas.Where(r => r.InstitucionEducativaSedeId == Sede.PadreId).FirstOrDefault();
                    
               
                    switch (placeholderDominio.Codigo)
                    {
                        case ConstanCodigoVariablesPlaceHolders.LLAVE_MEN:
                            TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                .Replace(placeholderDominio.Nombre, ContratacionProyecto.Proyecto.LlaveMen);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.INSTITUCION_EDUCATIVA:
                            TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                .Replace(placeholderDominio.Nombre, institucionEducativa.Nombre);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.SEDE:
                            TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                .Replace(placeholderDominio.Nombre, Sede.Nombre);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.NOMBRE_APORTANTE:
                            TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                .Replace(placeholderDominio.Nombre, ContratacionProyecto.Proyecto.ProyectoAportante
                                .FirstOrDefault().Aportante.NombreAportante);
                            break;
                             
                        case ConstanCodigoVariablesPlaceHolders.SALDO_ACTUAL_FUENTE:
                            TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                .Replace(placeholderDominio.Nombre, string
                                .Format("{0:#,0}", ContratacionProyecto.Proyecto.ProyectoAportante
                                .FirstOrDefault().Aportante.FuenteFinanciacion
                                .FirstOrDefault().GestionFuenteFinanciacion
                                .Sum(r => r.SaldoActual)));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.VALOR_SOLICITADO_FUENTE:
                            TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                .Replace(placeholderDominio.Nombre, string
                                .Format("{0:#,0}", ContratacionProyecto.Proyecto.ProyectoAportante
                                .FirstOrDefault().Aportante.FuenteFinanciacion
                                .FirstOrDefault().GestionFuenteFinanciacion
                                .Sum(r => r.ValorSolicitado)));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.NUEVO_SALDO_FUENTE:
                            TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                .Replace(placeholderDominio.Nombre, string
                                .Format("{0:#,0}", ContratacionProyecto.Proyecto.ProyectoAportante
                                .FirstOrDefault().Aportante.FuenteFinanciacion
                                .FirstOrDefault().GestionFuenteFinanciacion
                                .Sum(r => r.NuevoSaldo)));
                            break; 
                    }
                }
            }
            foreach (Dominio placeholderDominio in placeholders)
            {
                switch (placeholderDominio.Codigo)
                {
                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_PROYECTOS_CONTRATACION_DDP:
                        pPlantilla = pPlantilla
                            .Replace(placeholderDominio.Nombre, TotalRegistrosContratacionProyectos);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_APORTANTE:
                        pPlantilla = pPlantilla
                            .Replace(placeholderDominio.Nombre, TotalPlantillaRegistrosAportante);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.FECHA_EXPEDICION:
                        string FechaExpedicion = "";
                        if (!string.IsNullOrEmpty(pContratacion.FechaTramite.ToString()))
                        {
                            FechaExpedicion = ((DateTime)pContratacion.FechaTramite).ToString("yyyy-MM-dd");
                        }
                        pPlantilla = pPlantilla
                         .Replace(placeholderDominio.Nombre, FechaExpedicion);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                        pPlantilla = pPlantilla
                            .Replace(placeholderDominio.Nombre, pContratacion.NumeroSolicitud);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.NUMERO_DDP:
                        string NumeroDDP = "";
                        if (!string.IsNullOrEmpty(pContratacion.DisponibilidadPresupuestal.FirstOrDefault().NumeroDdp))
                        {
                            NumeroDDP = pContratacion.DisponibilidadPresupuestal.FirstOrDefault().NumeroDdp;
                        }
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, NumeroDDP);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.RUBRO_POR_FINANCIAR:

                        string RubroPorFinanciar = "";
                        if (!string.IsNullOrEmpty(pContratacion.DisponibilidadPresupuestal.FirstOrDefault().TipoSolicitudCodigo))
                        {
                            RubroPorFinanciar = LisParametricas
                                .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal
                                && r.Codigo == pContratacion.DisponibilidadPresupuestal.FirstOrDefault().TipoSolicitudCodigo).FirstOrDefault().Descripcion;
                        }
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, RubroPorFinanciar);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.TIPO_SOLICITUD:

                        string TipoRubroPorFinanciar = "";
                        if (!string.IsNullOrEmpty(pContratacion.DisponibilidadPresupuestal.FirstOrDefault().TipoSolicitudCodigo))
                        {
                            RubroPorFinanciar = LisParametricas
                                .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal
                                && r.Codigo == pContratacion.DisponibilidadPresupuestal.FirstOrDefault().TipoSolicitudCodigo).FirstOrDefault().Nombre;
                        }
                        pPlantilla = pPlantilla
                            .Replace(placeholderDominio.Nombre, TipoRubroPorFinanciar);
                        break;


                    case ConstanCodigoVariablesPlaceHolders.OPCION_CONTRATAR:
                        string opcionContratar = "";
                        if (!string.IsNullOrEmpty(pContratacion.TipoContratacionCodigo))
                        {
                            opcionContratar = LisParametricas
                            .Where(r => r.Codigo == pContratacion.TipoContratacionCodigo
                            && r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_Por_Contratar)
                            .FirstOrDefault().Nombre;
                        }
                        pPlantilla = pPlantilla
                            .Replace(placeholderDominio.Nombre, opcionContratar);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.OBJETO:
                        pPlantilla = pPlantilla
                            .Replace(placeholderDominio.Nombre, pContratacion.DisponibilidadPresupuestal.FirstOrDefault().Objeto);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CANTIDAD_TOTAL_NUMERO:
                        pPlantilla = pPlantilla
                            .Replace(placeholderDominio.Nombre, string.Format("{0:#,0}", TotalValorSolicitadoFuente));
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CANTIDAD_TOTAL_LETRAS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, Helpers.Conversores.NumeroALetras(TotalValorSolicitadoFuente));
                        break;
                }
            } 
            return pPlantilla;
        }

        public async Task<List<SesionComiteSolicitud>> GetListSesionComiteSolicitud()
        {
            // Estado de la sesionComiteSolicitud
            //• Recibidas sin tramitar ante Fiduciaria
            //• Enviadas a la fiduciaria
            //• Registradas por la fiduciaria

            //Se listan las que tengan con acta de sesion aprobada  

            //    List<SesionComiteSolicitud> ListSesionComiteSolicitud = await _context.SesionComiteSolicitud
            //.Where(r => r.TipoSolicitud == ConstanCodigoTipoSolicitud.Contratacion
            //&& r.EstadoDelRegistro == ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Aprobada
            //)
            //.ToListAsync(); 
            // 2   Aprobada por comité fiduciario


            List<SesionComiteSolicitud> ListSesionComiteSolicitud = await _context.SesionComiteSolicitud
                .Where(r => !(bool)r.Eliminado
                //TODO Filtrar por los otros parametros
                // && r.EstadoCodigo == ConstanCodigoEstadoSolicitudContratacion.Aprobada_comite_fiduciario
                //poner el id 7 y el id otro 
                ).ToListAsync();



            ListSesionComiteSolicitud = ListSesionComiteSolicitud.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion
            || r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Modificacion_Contractual).ToList();
            List<Dominio> ListasParametricas = _context.Dominio.ToList();

            List<Contratacion> ListContratacion = _context
                .Contratacion
                .Where(r => !(bool)r.Eliminado)
                //.Include(r => r.ContratacionProyecto)
                //.ThenInclude(r => r.Proyecto)
                //.ThenInclude(r => r.DisponibilidadPresupuestalProyecto)
                //    .ThenInclude(r => r.Proyecto) 
                .ToList();
            List<Contratista> ListContratista = _context.Contratista.ToList();

            foreach (var sesionComiteSolicitud in ListSesionComiteSolicitud)
            {
                switch (sesionComiteSolicitud.TipoSolicitudCodigo)
                {
                    case ConstanCodigoTipoSolicitud.Contratacion:
                        Contratacion contratacion = await GetContratacionByContratacionId(sesionComiteSolicitud.SolicitudId);

                        // sesionComiteSolicitud.Contratacion = contratacion;

                        sesionComiteSolicitud.EstaTramitado = false;

                        if (!string.IsNullOrEmpty(contratacion.FechaEnvioDocumentacion.ToString()))
                        {
                            sesionComiteSolicitud.EstaTramitado = true;
                        }

                        sesionComiteSolicitud.FechaSolicitud = (DateTime)contratacion.FechaTramite;

                        sesionComiteSolicitud.NumeroSolicitud = contratacion.NumeroSolicitud;

                        sesionComiteSolicitud.TipoSolicitud = ListasParametricas
                            .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud
                            && r.Codigo == ConstanCodigoTipoSolicitud.Contratacion
                            ).FirstOrDefault().Nombre;

                        if (contratacion.RegistroCompleto == null || !(bool)contratacion.RegistroCompleto)
                        {
                            sesionComiteSolicitud.EstadoRegistro = false;
                            sesionComiteSolicitud.EstadoDelRegistro = "Incompleto";
                        }
                        else
                        {
                            sesionComiteSolicitud.EstadoRegistro = true;
                            sesionComiteSolicitud.EstadoDelRegistro = "Completo";
                        }

                        break;

                    case ConstanCodigoTipoSolicitud.Modificacion_Contractual:

                        sesionComiteSolicitud.TipoSolicitud = ListasParametricas
                       .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud
                        && r.Codigo == ConstanCodigoTipoSolicitud.Modificacion_Contractual)
                       .FirstOrDefault().Nombre;
                        break;


                    default:
                        break;
                }
            }
            return ListSesionComiteSolicitud.OrderByDescending(r => r.SesionComiteSolicitudId).ToList();
        }

        public async Task<Contratacion> GetContratacionByContratacionId(int pContratacionId)
        {

            //TODO: PENDIENTE por FAber Numero comite Fiduciario Fecha Comite Fiduciario

            List<Dominio> LisParametricas = _context.Dominio.ToList();
            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();


            Contratacion contratacion = await _context.Contratacion.Where(r => r.ContratacionId == pContratacionId)
                      .Include(r => r.DisponibilidadPresupuestal)
                      .Include(r => r.Contratista)
                      .Include(r => r.ContratacionProyecto)
                          .ThenInclude(r => r.Proyecto)
                            .ThenInclude(r => r.ProyectoAportante)
                               .ThenInclude(r => r.Aportante)
                                 .ThenInclude(r => r.FuenteFinanciacion)
                                    .ThenInclude(r => r.GestionFuenteFinanciacion)
                    .Include(r => r.ContratacionProyecto)
                          .ThenInclude(r => r.Proyecto)
                              .ThenInclude(r => r.InstitucionEducativa)
                 .FirstOrDefaultAsync();

            SesionComiteSolicitud sesionComiteSolicitud = _context.SesionComiteSolicitud
                .Where(r => r.SolicitudId == contratacion.ContratacionId && r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion)
                .Include(r => r.ComiteTecnico).FirstOrDefault();

            if (!string.IsNullOrEmpty(contratacion.TipoContratacionCodigo))
            {
                contratacion.TipoContratacionCodigo = LisParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar && r.Codigo == contratacion.TipoContratacionCodigo).FirstOrDefault().Nombre;
            }
            if (sesionComiteSolicitud.ComiteTecnico.EsComiteFiduciario == null || !(bool)sesionComiteSolicitud.ComiteTecnico.EsComiteFiduciario)
            {
                sesionComiteSolicitud = null;
            }

            if (sesionComiteSolicitud != null)
            {
                if (sesionComiteSolicitud.FechaComiteFiduciario != null)
                {
                    contratacion.DisponibilidadPresupuestal.FirstOrDefault().FechaComiteFiduciario = ((DateTime)sesionComiteSolicitud.FechaComiteFiduciario).ToString("dd-MM-yy");
                }
                contratacion.DisponibilidadPresupuestal.FirstOrDefault().NumeroComiteFiduciario = sesionComiteSolicitud.ComiteTecnico.NumeroComite;
            }

            if (!string.IsNullOrEmpty(contratacion.Contratista.TipoIdentificacionCodigo))
            {
                bool allDigits = contratacion.Contratista.TipoIdentificacionCodigo.All(char.IsDigit);
                if (allDigits)
                {
                    contratacion.Contratista.TipoIdentificacionCodigo = LisParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Documento && r.Codigo == contratacion.Contratista.TipoIdentificacionCodigo).FirstOrDefault().Nombre;
                }
            }
            foreach (var ContratacionProyecto in contratacion.ContratacionProyecto)
            {

                bool allDigits = ContratacionProyecto.Proyecto.TipoIntervencionCodigo.All(char.IsDigit);
                if (allDigits)
                {
                    ContratacionProyecto.Proyecto.TipoIntervencionCodigo = LisParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && r.Codigo == ContratacionProyecto.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                }
                bool allDigits2 = ContratacionProyecto.Proyecto.InstitucionEducativa.LocalizacionIdMunicipio.All(char.IsDigit);

                if (allDigits2)
                {
                    ContratacionProyecto.Proyecto.InstitucionEducativa.Municipio = ListLocalizacion.Where(r => r.LocalizacionId == ContratacionProyecto.Proyecto.InstitucionEducativa.LocalizacionIdMunicipio).FirstOrDefault();
                    ContratacionProyecto.Proyecto.InstitucionEducativa.Departamento = ListLocalizacion.Where(r => r.LocalizacionId == ContratacionProyecto.Proyecto.InstitucionEducativa.Municipio.IdPadre).FirstOrDefault();
                }


                foreach (var ProyectoAportante in ContratacionProyecto.Proyecto.ProyectoAportante)
                {
                    if (ProyectoAportante.Aportante.TipoAportanteId > 0)
                    {
                        ProyectoAportante.Aportante.TipoAportante = LisParametricas
                            .Where(r => r.DominioId == ProyectoAportante.Aportante.TipoAportanteId)
                            .FirstOrDefault().Nombre;
                    }
                    if (ProyectoAportante.Aportante.NombreAportanteId > 0)
                    {

                        ProyectoAportante.Aportante.NombreAportante = LisParametricas
                            .Where(r => r.DominioId == ProyectoAportante.Aportante.NombreAportanteId)
                            .FirstOrDefault().Nombre;
                    }


                    foreach (var FuenteFinanciacion in ProyectoAportante.Aportante.FuenteFinanciacion)
                    {

                        bool allDigits3 = FuenteFinanciacion.FuenteRecursosCodigo.All(char.IsDigit);

                        if (allDigits3 && !string.IsNullOrEmpty(FuenteFinanciacion.FuenteRecursosCodigo))
                        {
                            FuenteFinanciacion.FuenteRecursosCodigo = LisParametricas
                                  .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Fuente_de_Recurso
                                  && r.Codigo == FuenteFinanciacion.FuenteRecursosCodigo
                                  ).FirstOrDefault().Nombre;
                        }

                    }

                }
            }


            return contratacion;
        }

        public async Task<Respuesta> RegistrarTramiteContratacion(Contratacion pContratacion, IFormFile pFile, string pDirectorioBase, string pDirectorioMinuta)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Tramite_Contratacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //Save Files  
                string strFilePatch = Path.Combine(pDirectorioBase, pDirectorioMinuta, pContratacion.ContratacionId.ToString());
                await _documentService.SaveFileContratacion(pFile, strFilePatch, pContratacion.ContratacionId);


                Contratacion contratacionOld = _context.Contratacion.Find(pContratacion.ContratacionId);
                //Auditoria
                contratacionOld.FechaModificacion = DateTime.Now;
                contratacionOld.UsuarioModificacion = pContratacion.UsuarioCreacion;
                //Registros
                contratacionOld.RutaMinuta = strFilePatch;
                contratacionOld.RegistroCompleto = pContratacion.RegistroCompleto;
                contratacionOld.FechaEnvioDocumentacion = pContratacion.FechaEnvioDocumentacion;
                contratacionOld.Observaciones = pContratacion.Observaciones;
                contratacionOld.RutaMinuta = pContratacion.RutaMinuta;

                await _context.SaveChangesAsync();

                return
                  new Respuesta
                  {
                      IsSuccessful = true,
                      IsException = false,
                      IsValidation = false,
                      Code = ConstantGestionarProcesosContractuales.OperacionExitosa,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantGestionarProcesosContractuales.OperacionExitosa, idAccion, pContratacion.UsuarioCreacion, "REGISTRAR SOLICITUD")
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
                  Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantGestionarProcesosContractuales.Error, idAccion, pContratacion.UsuarioCreacion, ex.InnerException.ToString())
              };
            }
        }


    }

}
