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
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.services
{
    public class ManageContractualProcessesService : IManageContractualProcessesService
    {
        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        public readonly IConverter _converter;
        public readonly IProjectContractingService _ProjectContractingService;

        public ManageContractualProcessesService(IProjectContractingService projectContractingService, IConverter converter, devAsiVamosFFIEContext context, ICommonService commonService, IDocumentService documentService)
        {
            _ProjectContractingService = projectContractingService;
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

                Contratacion contratacion = _context.Contratacion.Find(pSesionComiteSolicitud.SolicitudId);
                contratacion.EstadoSolicitudCodigo = pSesionComiteSolicitud.EstadoCodigo;
                contratacion.FechaModificacion = DateTime.Now;
                contratacion.UsuarioCreacion = pSesionComiteSolicitud.UsuarioCreacion;

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

        public async Task<byte[]> GetDDPBySesionComiteSolicitudID(int pSesionComiteSolicitudID, string pPatchLogo)
        {
            //Al modificar verificar a que tipo de solicitud corresponde el comite y hacer switch         
            SesionComiteSolicitud sesionComiteSolicitud = _context.SesionComiteSolicitud.Where(r => r.SesionComiteSolicitudId == pSesionComiteSolicitudID)
               .FirstOrDefault();

            string TipoPlantilla = "";
            Contratacion contratacion = await _ProjectContractingService.GetAllContratacionByContratacionId(sesionComiteSolicitud.SolicitudId);

            if (contratacion.DisponibilidadPresupuestal.FirstOrDefault().TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo)
            {
                TipoPlantilla = ((int)ConstanCodigoPlantillas.DDP_Contratacion_Rubro_Administrativo).ToString();
            }
            else
            {
                TipoPlantilla = ((int)ConstanCodigoPlantillas.DDP_Contratacion_Rubro_Por_Financiar).ToString();
            }
            Plantilla Plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();


            Plantilla.Contenido = ReemplazarDatosPlantillaContratacion(Plantilla.Contenido, contratacion, sesionComiteSolicitud);

            return ConvertirPDF(Plantilla, pPatchLogo);
        }

        public byte[] ConvertirPDF(Plantilla pPlantilla, string pPatchLogo)
        {
            string strEncabezado = "";
            if (!string.IsNullOrEmpty(pPlantilla.Encabezado.Contenido))
            {
                pPlantilla.Encabezado.Contenido = pPlantilla.Encabezado.Contenido.Replace("[RUTA_ICONO]", Path.Combine(Directory.GetCurrentDirectory(), "assets", "img-FFIE.png"));
                strEncabezado = Helpers.Helpers.HtmlStringLimpio(pPlantilla.Encabezado.Contenido);
                //strEncabezado = pPlantilla.Encabezado.Contenido;
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
            string ImageIcon = "<img src= '" + Path.Combine(Directory.GetCurrentDirectory(), "assets", "img-FFIE.png") + " >";

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = pPlantilla.Contenido,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "pdf-styles.css") },
                HeaderSettings = { FontName = "Roboto", FontSize = 8, Center = strEncabezado, Line = false, Spacing = 18, Right = "" },
                FooterSettings = { FontName = "Ariel", FontSize = 10, Center = "[page]", },
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            return _converter.Convert(pdf);
        }

        public string ReemplazarDatosPlantillaContratacion(string pPlantilla, Contratacion pContratacion, SesionComiteSolicitud pSesionComiteSolicitud)
        {


            pSesionComiteSolicitud.ComiteTecnico = _context.ComiteTecnico
                .Where(r => !(bool)r.EsComiteFiduciario && r.ComiteTecnicoId == pSesionComiteSolicitud.ComiteTecnicoId)
                .FirstOrDefault();

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
                foreach (var ProyectoAportante in ContratacionProyecto.Proyecto.ProyectoAportante.Where(r => !(bool)r.Eliminado))
                {
                    TotalPlantillaRegistrosAportante += PlantillaRegistrosAportante;
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
                                           .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion
                                           && r.Codigo == FuenteFinanciacion.FuenteRecursosCodigo
                                           ).FirstOrDefault().Nombre;
                                    }
                                    else
                                    {

                                        FuenteFinanciacionNombre = LisParametricas
                             .Where(r => r.Nombre == FuenteFinanciacion.FuenteRecursosCodigo
                             ).FirstOrDefault().Codigo;

                                        FuenteFinanciacionNombre = LisParametricas
                                           .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion
                                           && r.Codigo == FuenteFinanciacionNombre
                                           ).FirstOrDefault().Nombre;

                                    }
                                }
                                TotalPlantillaRegistrosAportante = TotalPlantillaRegistrosAportante
                                     .Replace(placeholderDominio.Nombre, FuenteFinanciacionNombre);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.VALOR_NUMERO:

                                foreach (var FuenteFinanciacion in ProyectoAportante.Aportante.FuenteFinanciacion)
                                {
                                    ValorFuenteNumero += (decimal)FuenteFinanciacion.ValorFuente;
                                }

                                TotalPlantillaRegistrosAportante = TotalPlantillaRegistrosAportante
                             .Replace(placeholderDominio.Nombre, string.Format("${0:#,0}", ValorFuenteNumero) + " pesos");
                                TotalValorSolicitadoFuente += ValorFuenteNumero;
                                break;

                            case ConstanCodigoVariablesPlaceHolders.VALOR_LETRAS:

                                decimal ValorFuenteNumero2 = 0;
                                foreach (var FuenteFinanciacion in ProyectoAportante.Aportante.FuenteFinanciacion)
                                {
                                    ValorFuenteNumero2 += (decimal)FuenteFinanciacion.ValorFuente;
                                }
                                TotalPlantillaRegistrosAportante = TotalPlantillaRegistrosAportante
                                    .Replace(placeholderDominio.Nombre,
                                    CultureInfo.CurrentCulture.TextInfo
                                    .ToTitleCase(Helpers.Conversores
                                    .NumeroALetras(ValorFuenteNumero2).ToLower()));

                                break;
                        }
                    }
                }
            }
            //Registros Proyectos
            foreach (var ContratacionProyecto in pContratacion.ContratacionProyecto.Where(r => !(bool)r.Eliminado))
            {
                InstitucionEducativaSede Sede = ListInstitucionEducativas.Where(r => r.InstitucionEducativaSedeId == (int)ContratacionProyecto.Proyecto.SedeId).FirstOrDefault();
                InstitucionEducativaSede institucionEducativa = ListInstitucionEducativas.Where(r => r.InstitucionEducativaSedeId == (int)ContratacionProyecto.Proyecto.InstitucionEducativaId).FirstOrDefault();


                TotalRegistrosContratacionProyectos += PlantillaRegistrosProyectosContratacion;
                foreach (Dominio placeholderDominio in placeholders)
                {

                    switch (placeholderDominio.Codigo)
                    {
                        case ConstanCodigoVariablesPlaceHolders.LLAVE_MEN:

                            TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                .Replace(placeholderDominio.Nombre, ContratacionProyecto.Proyecto.LlaveMen);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.INSTITUCION_EDUCATIVA:
                            string NombreInstitucionEducativa = "";
                            if (institucionEducativa != null)
                            {
                                NombreInstitucionEducativa = institucionEducativa.Nombre;
                            }
                            TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                .Replace(placeholderDominio.Nombre, NombreInstitucionEducativa);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.SEDE:
                            string NombreSede = "";
                            if (Sede != null)
                            {
                                NombreSede = Sede.Nombre;
                            }
                            TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                .Replace(placeholderDominio.Nombre, NombreSede);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.NOMBRE_APORTANTE:

                            string nombreAportante = "";

                            try
                            {
                                string strNombreAportante = string.Empty;
                                switch (ContratacionProyecto.ContratacionProyectoAportante.FirstOrDefault().CofinanciacionAportante.TipoAportanteId)
                                {

                                    case ConstanTipoAportante.Ffie:
                                        strNombreAportante = ConstanStringTipoAportante.Ffie;
                                        break;

                                    case ConstanTipoAportante.ET:

                                        if (ContratacionProyecto.ContratacionProyectoAportante.FirstOrDefault().CofinanciacionAportante.Departamento != null)
                                        {
                                            strNombreAportante = ContratacionProyecto.ContratacionProyectoAportante.FirstOrDefault().CofinanciacionAportante.Departamento.Descripcion;
                                        }
                                        else
                                        {
                                            strNombreAportante = ContratacionProyecto.ContratacionProyectoAportante.FirstOrDefault().CofinanciacionAportante.Municipio.Descripcion;
                                        }
                                        break;
                                    case ConstanTipoAportante.Tercero:
                                        strNombreAportante = ContratacionProyecto.ContratacionProyectoAportante.FirstOrDefault().CofinanciacionAportante.NombreAportante.Nombre;
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                            }
                            TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                .Replace(placeholderDominio.Nombre, nombreAportante);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.SALDO_ACTUAL_FUENTE:

                            if (ContratacionProyecto.Proyecto.ProyectoAportante
                                                  .FirstOrDefault().Aportante.FuenteFinanciacion.Count() > 0)
                            {
                                TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                    .Replace(placeholderDominio.Nombre, string
                                    .Format("{0:#,0}", ContratacionProyecto.Proyecto.ProyectoAportante
                                    .FirstOrDefault().Aportante.FuenteFinanciacion
                                    .FirstOrDefault().GestionFuenteFinanciacion
                                    .Sum(r => r.SaldoActual)));
                            }
                            else
                            {
                                TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                 .Replace(placeholderDominio.Nombre, " ");

                            }
                            break;

                        case ConstanCodigoVariablesPlaceHolders.VALOR_SOLICITADO_FUENTE:

                            if (ContratacionProyecto.Proyecto.ProyectoAportante
                                                .FirstOrDefault().Aportante.FuenteFinanciacion.Count() > 0)
                            {
                                TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                                .Replace(placeholderDominio.Nombre, string
                                                .Format("{0:#,0}", ContratacionProyecto.Proyecto.ProyectoAportante
                                                .FirstOrDefault().Aportante.FuenteFinanciacion
                                                .FirstOrDefault().GestionFuenteFinanciacion
                                                .Sum(r => r.ValorSolicitado)));
                            }
                            else
                            {
                                TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                            .Replace(placeholderDominio.Nombre, " ");
                            }
                            break;

                        case ConstanCodigoVariablesPlaceHolders.NUEVO_SALDO_FUENTE:

                            if (ContratacionProyecto.Proyecto.ProyectoAportante
                                             .FirstOrDefault().Aportante.FuenteFinanciacion.Count() > 0)
                            {
                                TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                                 .Replace(placeholderDominio.Nombre, string
                                                 .Format("{0:#,0}", ContratacionProyecto.Proyecto.ProyectoAportante
                                                 .FirstOrDefault().Aportante.FuenteFinanciacion
                                                 .FirstOrDefault().GestionFuenteFinanciacion
                                                 .Sum(r => r.NuevoSaldo)));
                            }
                            else
                            {
                                TotalRegistrosContratacionProyectos = TotalRegistrosContratacionProyectos
                                           .Replace(placeholderDominio.Nombre, " ");

                            }
                            break;
                    }
                }
            }

            //Contratacion 
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

                    case ConstanCodigoVariablesPlaceHolders.FUNCIONALIDAD_ESPECIAL:

                        string LimitacionEspecial = pContratacion.DisponibilidadPresupuestal.FirstOrDefault().LimitacionEspecial;


                        pPlantilla = pPlantilla
                            .Replace(placeholderDominio.Nombre, LimitacionEspecial);
                        break;


                    case ConstanCodigoVariablesPlaceHolders.FECHA_COMITE_TECNICO:
                        pPlantilla = pPlantilla
                            .Replace(placeholderDominio.Nombre, ((DateTime)pSesionComiteSolicitud.ComiteTecnico.FechaOrdenDia).ToString("yyyy-MM-dd"));
                        break;

                    case ConstanCodigoVariablesPlaceHolders.NUMERO_COMITE_TECNICO:
                        pPlantilla = pPlantilla
                            .Replace(placeholderDominio.Nombre, pSesionComiteSolicitud.ComiteTecnico.NumeroComite);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.NUMERO_CONTRATO:
                        try
                        {
                            pPlantilla = pPlantilla
                                          .Replace(placeholderDominio.Nombre, pContratacion.Contrato.FirstOrDefault().NumeroContrato);

                        }
                        catch (Exception)
                        {
                            pPlantilla = pPlantilla
                             .Replace(placeholderDominio.Nombre, " ");
                        }
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

                        string TipoSolicitud = "";
                        if (!string.IsNullOrEmpty(pSesionComiteSolicitud.TipoSolicitudCodigo))
                        {
                            TipoSolicitud = LisParametricas.Where(r => r.Codigo == pSesionComiteSolicitud.TipoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud).FirstOrDefault().Nombre;
                        }
                        pPlantilla = pPlantilla
                            .Replace(placeholderDominio.Nombre, TipoSolicitud);
                        break;


                    case ConstanCodigoVariablesPlaceHolders.OPCION_CONTRATAR:

                        string TipoContratacion = "";
                        if (!string.IsNullOrEmpty(pContratacion.TipoContratacionCodigo))
                        {
                            bool allDigits = pContratacion.DisponibilidadPresupuestal.FirstOrDefault().TipoSolicitudCodigo.All(char.IsDigit);
                            if (allDigits)
                            {
                                TipoContratacion = LisParametricas
                            .Where(r => r.Codigo == pContratacion.DisponibilidadPresupuestal.FirstOrDefault().TipoSolicitudCodigo
                            && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal)
                            .FirstOrDefault().Nombre;
                            }
                        }
                        pPlantilla = pPlantilla
                            .Replace(placeholderDominio.Nombre, TipoContratacion);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.OBJETO:
                        pPlantilla = pPlantilla
                            .Replace(placeholderDominio.Nombre, pContratacion.DisponibilidadPresupuestal.FirstOrDefault().Objeto);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CANTIDAD_TOTAL_NUMERO:
                        pPlantilla = pPlantilla
                            .Replace(placeholderDominio.Nombre, string.Format("${0:#,0}", TotalValorSolicitadoFuente) + " pesos");
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CANTIDAD_TOTAL_LETRAS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre,
                        CultureInfo.CurrentCulture.TextInfo
                        .ToTitleCase(Helpers.Conversores
                        .NumeroALetras(TotalValorSolicitadoFuente).ToLower()));
                        break;
                }
            }
            return pPlantilla;
        }

        public async Task<List<SesionComiteSolicitud>> GetListSesionComiteSolicitud()
        {
            try
            {

                List<ComiteTecnico> ListComiteTecnicos = _context.ComiteTecnico
                    .Where(r => (bool)r.EsComiteFiduciario && r.EstadoActaCodigo == ConstantCodigoActas.Aprobada && !(bool)r.Eliminado)
                    .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario).ToList();

                ////ListComiteTecnicos = ListComiteTecnicos.Where(r => r.EstadoActaCodigo == ConstantCodigoActas.Aprobada).ToList();

                List<Dominio> ListasParametricas = _context.Dominio.ToList();

                //Listas Contratacion 
                List<SesionComiteSolicitud> ListSesionComiteSolicitud = new List<SesionComiteSolicitud>();

                foreach (var comiteTecnico in ListComiteTecnicos)
                {
                    foreach (var sesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario.Where(
                          r => !(bool)r.Eliminado
                         && (r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion
                         || r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Modificacion_Contractual)
                        ))
                    {
                        switch (sesionComiteSolicitud.TipoSolicitudCodigo)
                        {
                            case ConstanCodigoTipoSolicitud.Contratacion:

                                Contratacion contratacion = await GetContratacionByContratacionId(sesionComiteSolicitud.SolicitudId);

                                if (contratacion.DisponibilidadPresupuestal.Count() == 0)
                                {
                                    break;
                                }
                                //Jmartinez Si llegan dos Disponibilidad presupuestal error

                                if (contratacion.DisponibilidadPresupuestal.Count() > 1)
                                {
                                    break;
                                }

                                //jflorez, pequeño ajsute porque toteaba                                    
                                if (contratacion.DisponibilidadPresupuestal.FirstOrDefault().NumeroDdp == null || string.IsNullOrEmpty(contratacion.DisponibilidadPresupuestal.FirstOrDefault().NumeroDdp))
                                {
                                    break;
                                }

                                // sesionComiteSolicitud.Contratacion = contratacion;
                                sesionComiteSolicitud.EstadoCodigo = contratacion.EstadoSolicitudCodigo;

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

                                if (contratacion.RegistroCompleto1 == null || !(bool)contratacion.RegistroCompleto1)
                                {
                                    sesionComiteSolicitud.EstadoRegistro = false;
                                    sesionComiteSolicitud.EstadoDelRegistro = "Incompleto";
                                }
                                else
                                {
                                    sesionComiteSolicitud.EstadoRegistro = true;
                                    sesionComiteSolicitud.EstadoDelRegistro = "Completo";
                                }

                                ListSesionComiteSolicitud.Add(sesionComiteSolicitud);
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

                }


                return ListSesionComiteSolicitud.OrderByDescending(r => r.SesionComiteSolicitudId).ToList();
            }
            catch (Exception ex)
            {
                return new List<SesionComiteSolicitud>();
            }

        }

        public async Task<Contratacion> GetContratacionByContratacionId(int pContratacionId)
        {
            try
            {
                List<Dominio> LisParametricas = _context.Dominio.ToList();
                List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();

                Contratacion contratacion = await _context.Contratacion.Where(r => r.ContratacionId == pContratacionId)
                          .Include(r => r.DisponibilidadPresupuestal)
                          .Include(r => r.Contratista)
                          .Include(r => r.Contrato)
                          .Include(r => r.ContratacionProyecto)
                              .ThenInclude(r => r.Proyecto)
                                .ThenInclude(r => r.ProyectoAportante)
                                   .ThenInclude(r => r.Aportante)
                                     .ThenInclude(r => r.FuenteFinanciacion)
                                        .ThenInclude(r => r.GestionFuenteFinanciacion)
                          .Include(r => r.ContratacionProyecto)
                              .ThenInclude(r => r.Proyecto)
                                  .ThenInclude(r => r.InstitucionEducativa)
                          .Include(r => r.ContratacionProyecto)
                              .ThenInclude(r => r.Proyecto)
                                   .ThenInclude(r => r.Sede)
                     .FirstOrDefaultAsync();

                List<SesionComiteSolicitud> sesionComiteSolicitud = _context.SesionComiteSolicitud
                     .Where(r => r.SolicitudId == contratacion.ContratacionId && r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion)
                     .Include(r => r.ComiteTecnico)
                     .Include(r => r.ComiteTecnicoFiduciario)
                     .ToList();

                contratacion.sesionComiteSolicitud = sesionComiteSolicitud;

                if (!string.IsNullOrEmpty(contratacion.TipoContratacionCodigo))
                {
                    contratacion.TipoContratacionCodigo = LisParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar && r.Codigo == contratacion.TipoContratacionCodigo).FirstOrDefault().Nombre;
                }

                if (contratacion.Contratista != null)
                {
                    if (!string.IsNullOrEmpty(contratacion.Contratista.TipoIdentificacionCodigo))
                    {
                        bool allDigits = contratacion.Contratista.TipoIdentificacionCodigo.All(char.IsDigit);
                        if (allDigits)
                        {
                            contratacion.Contratista.TipoIdentificacionCodigo = LisParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Documento && r.Codigo == contratacion.Contratista.TipoIdentificacionCodigo).FirstOrDefault().Nombre;
                        }
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
                            ProyectoAportante.Aportante.TipoAportanteString = LisParametricas
                                .Where(r => r.DominioId == ProyectoAportante.Aportante.TipoAportanteId)
                                .FirstOrDefault().Nombre;
                        }
                        if (ProyectoAportante.Aportante.NombreAportanteId > 0)
                        {

                            ProyectoAportante.Aportante.NombreAportanteString = LisParametricas
                                .Where(r => r.DominioId == ProyectoAportante.Aportante.NombreAportanteId)
                                .FirstOrDefault().Nombre;
                        }


                        foreach (var FuenteFinanciacion in ProyectoAportante.Aportante.FuenteFinanciacion)
                        {
                            bool allDigits3 = FuenteFinanciacion.FuenteRecursosCodigo.All(char.IsDigit);

                            if (allDigits3 && !string.IsNullOrEmpty(FuenteFinanciacion.FuenteRecursosCodigo))
                            {

                                FuenteFinanciacion.FuenteRecursosCodigo = LisParametricas
                                      .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion
                                      && r.Codigo == FuenteFinanciacion.FuenteRecursosCodigo
                                      ).FirstOrDefault().Nombre;
                            }

                        }

                    }
                }
                return contratacion;
            }
            catch (Exception ex)
            {
                return new Contratacion();
            }
        }

        public async Task<Respuesta> RegistrarTramiteContratacion(Contratacion pContratacion, IFormFile pFile, string pDirectorioBase, string pDirectorioMinuta)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Tramite_Contratacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                string strFilePatch = "";

                if (pFile == null)
                {
                }
                else
                {
                    if (pFile.Length > 0)
                    {
                        strFilePatch = Path.Combine(pDirectorioBase, pDirectorioMinuta, pContratacion.ContratacionId.ToString());
                        await _documentService.SaveFileContratacion(pFile, strFilePatch, pFile.FileName);

                    }
                }

                Contratacion contratacionOld = _context.Contratacion.Find(pContratacion.ContratacionId);
                //Auditoria
                contratacionOld.FechaModificacion = DateTime.Now;
                contratacionOld.UsuarioModificacion = pContratacion.UsuarioCreacion;
                //Registros
                contratacionOld.RutaMinuta = strFilePatch;
                contratacionOld.RegistroCompleto = pContratacion.RegistroCompleto;
                contratacionOld.FechaEnvioDocumentacion = pContratacion.FechaEnvioDocumentacion;
                contratacionOld.Observaciones = pContratacion.Observaciones;
                if (pFile != null && pFile.Length > 0)
                    contratacionOld.RutaMinuta = strFilePatch + "//" + pFile.FileName;
                contratacionOld.RegistroCompleto1 = ValidarCamposContratacion(contratacionOld);

                await _context.SaveChangesAsync();

                return
                  new Respuesta
                  {
                      IsSuccessful = true,
                      IsException = false,
                      IsValidation = false,
                      Code = ConstantGestionarProcesosContractuales.OperacionExitosa,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Procesos_Contractuales, ConstantGestionarProcesosContractuales.OperacionExitosa, idAccion, pContratacion.UsuarioCreacion, "REGISTRAR SOLICITUD")
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
                  Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Procesos_Contractuales, ConstantGestionarProcesosContractuales.Error, idAccion, pContratacion.UsuarioCreacion, ex.InnerException.ToString())
              };
            }
        }

        public static bool ValidarCamposContratacion(Contratacion pContratacion)
        {

            if (
                    !string.IsNullOrEmpty(pContratacion.TipoSolicitudCodigo)
                   || !string.IsNullOrEmpty(pContratacion.NumeroSolicitud.ToString())
                   || !string.IsNullOrEmpty(pContratacion.EstadoSolicitudCodigo)
                   || !string.IsNullOrEmpty(pContratacion.Observaciones)
                   || !string.IsNullOrEmpty(pContratacion.RutaMinuta)
                   || !string.IsNullOrEmpty(pContratacion.FechaEnvioDocumentacion.ToString())
                   || !string.IsNullOrEmpty(pContratacion.ConsideracionDescripcion.ToString())
                    )
            {
                return true;
            }
            return false;
        }
    }

}
