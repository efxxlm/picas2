using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Constants;
using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace asivamosffie.services
{
    public class JudicialDefenseService : IJudicialDefense
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;     
            
            private readonly IConverter _converter;

        public JudicialDefenseService(devAsiVamosFFIEContext context, ICommonService commonService, IConverter converter)
        {

            _commonService = commonService;
            _context = context;
            _converter = converter;
            //_settings = settings;
        }

        public async Task<string> GetNombreContratistaByContratoId(int pContratoId)
        {
            Contrato contrato = null;
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            Contratacion contratacion = null;
            if (contrato != null)
            {
                contratacion = await _commonService.GetContratacionByContratacionId(contrato.ContratacionId);

            }            

            Contratista contratista = null;
            if (contratacion != null)
            {
                if (contratacion.ContratistaId != null)
                    contratista = await _commonService.GetContratistaByContratistaId((Int32)contratacion.ContratistaId);

                if (contratista != null)
                {
                    return contratista.Nombre;

                }
            }
            return null;
        }

        public async Task<Respuesta> CreateOrEditDemandadoConvocado(DemandadoConvocado demandadoConvocado)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Demandado_Convocado, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            DemandadoConvocado demandadoConvocadoBD = null;
            try
            {

                if (string.IsNullOrEmpty(demandadoConvocado.DemandadoConvocadoId.ToString()) || demandadoConvocado.DemandadoConvocadoId == 0)
                {
                    //Auditoria
                    strCrearEditar = "REGISTRAR DEMANDADO CONVOCADO";
                    demandadoConvocado.FechaCreacion = DateTime.Now;
                    demandadoConvocado.UsuarioCreacion = demandadoConvocado.UsuarioCreacion;
                    //fichaEstudio.DefensaJudicialId = fichaEstudio.DefensaJudicialId;
                    demandadoConvocado.Eliminado = false;
                    _context.DemandadoConvocado.Add(demandadoConvocado);
                }
                else
                {
                    strCrearEditar = "EDIT DEMANDADO CONVOCADO";
                    demandadoConvocadoBD = _context.DemandadoConvocado.Find(demandadoConvocado.DemandadoConvocadoId);

                    //Auditoria
                    demandadoConvocadoBD.UsuarioModificacion = demandadoConvocado.UsuarioModificacion;
                    demandadoConvocadoBD.Eliminado = false;

                    //Registros
                    demandadoConvocadoBD.Nombre = demandadoConvocado.Nombre;
                    demandadoConvocadoBD.TipoIdentificacionCodigo = demandadoConvocado.TipoIdentificacionCodigo;
                    demandadoConvocadoBD.NumeroIdentificacion = demandadoConvocado.NumeroIdentificacion;
                    demandadoConvocadoBD.DefensaJudicial = demandadoConvocado.DefensaJudicial;
                    demandadoConvocadoBD.Direccion = demandadoConvocado.Direccion;
                    demandadoConvocadoBD.UsuarioModificacion = demandadoConvocado.UsuarioModificacion;
                    demandadoConvocadoBD.Email = demandadoConvocado.Email;
                    
                    _context.DemandadoConvocado.Update(demandadoConvocadoBD);

                }

                _context.SaveChanges();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = demandadoConvocado,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, demandadoConvocado.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = demandadoConvocado,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, demandadoConvocado.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        public async Task<Respuesta> CreateOrEditDefensaJudicial(DefensaJudicial defensaJudicial)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Defensa_Judicial, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty, strUsuario = string.Empty; ;

            DefensaJudicial defensaJudicialBD = null;
            try
            {

                if (string.IsNullOrEmpty(defensaJudicial.DefensaJudicialId.ToString()) || defensaJudicial.DefensaJudicialId == 0)
                {
                    //Auditoria
                    strCrearEditar = "REGISTRAR DEFENSA JUDICIAL";
                    defensaJudicial.FechaCreacion = DateTime.Now;
                    defensaJudicial.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                    //fichaEstudio.DefensaJudicialId = fichaEstudio.DefensaJudicialId;
                    defensaJudicial.Eliminado = false;
                    foreach(var defContratcionProyecto in defensaJudicial.DefensaJudicialContratacionProyecto)
                    {
                        defContratcionProyecto.UsuarioCreacion= defensaJudicial.UsuarioCreacion;
                        defContratcionProyecto.FechaCreacion = DateTime.Now;
                        defContratcionProyecto.EsCompleto = true;
                        defContratcionProyecto.Eliminado = false;
                    }
                    foreach (var defConvocado in defensaJudicial.DemandadoConvocado)
                    {
                        defConvocado.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                        defConvocado.FechaCreacion = DateTime.Now;
                        defConvocado.Eliminado = false;
                    }

                    _context.DefensaJudicial.Add(defensaJudicial);
                }
                else
                {
                    strCrearEditar = "EDIT DEFENSA JUDICIAL";
                    defensaJudicialBD = _context.DefensaJudicial.Find(defensaJudicial.DefensaJudicialId);

                    //Auditoria
                    defensaJudicialBD.UsuarioModificacion = defensaJudicial.UsuarioModificacion;
                    defensaJudicialBD.Eliminado = false;

                    //Registros
                    defensaJudicialBD.LocalizacionIdMunicipio = defensaJudicial.LocalizacionIdMunicipio;
                    defensaJudicialBD.TipoAccionCodigo = defensaJudicial.TipoAccionCodigo;
                    defensaJudicialBD.JurisdiccionCodigo = defensaJudicial.JurisdiccionCodigo;
                    defensaJudicialBD.Pretensiones = defensaJudicial.Pretensiones;
                    defensaJudicialBD.CuantiaPerjuicios = defensaJudicial.CuantiaPerjuicios;

                    defensaJudicialBD.UsuarioModificacion = defensaJudicial.UsuarioModificacion;
                    defensaJudicialBD.EsRequiereSupervisor = defensaJudicial.EsRequiereSupervisor;
                    foreach (var defContratcionProyecto in defensaJudicial.DefensaJudicialContratacionProyecto)
                    {
                        defContratcionProyecto.UsuarioModificacion = defensaJudicial.UsuarioModificacion;
                        defContratcionProyecto.FechaModificacion = DateTime.Now;
                        defContratcionProyecto.EsCompleto = true;
                        defContratcionProyecto.Eliminado = false;
                    }
                    _context.DefensaJudicial.Update(defensaJudicialBD);

                }

                _context.SaveChanges();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = defensaJudicial,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, defensaJudicial.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = defensaJudicial,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, defensaJudicial.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        public async Task<DefensaJudicial> GetVistaDatosBasicosProceso(int pDefensaJudicialId = 0)
        {
            DefensaJudicial defensaJudicial1 = new DefensaJudicial();
            try
            {
               
                //Contrato contrato = null;
                List<DefensaJudicial> ListDefensaJudicial = new List<DefensaJudicial>();

                if (pDefensaJudicialId == 0)
                {
                    ListDefensaJudicial = await _context.DefensaJudicial.Where(r => (bool)r.Eliminado == false).Distinct()
                .ToListAsync();

                }
                else
                {
                    ListDefensaJudicial = await _context.DefensaJudicial.Where(r => (bool)r.Eliminado == false
                    && r.DefensaJudicialId == pDefensaJudicialId).
                    Include(x=>x.DefensaJudicialContratacionProyecto).
                    Distinct()
                .ToListAsync();


                }
                //TipoAccionCodigo JurisdiccionCodigo TipoProcesoCodigo
                Dominio TipoAccionCodigo;

                Dominio TipoDocumentoCodigoContratista;

                string TipoAccionTmp = string.Empty;
                             
                foreach (var defensaJudicial in ListDefensaJudicial)
                {
                    TipoAccionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(defensaJudicial.TipoAccionCodigo, (int)EnumeratorTipoDominio.Tipo_accion_judicial);

                    if (TipoAccionCodigo != null)
                        defensaJudicial.TipoAccionCodigoNombre = TipoAccionCodigo.Nombre;

                    defensaJudicial.JurisdiccionCodigoNombre = "PENDIENTE";
                    defensaJudicial.TipoProcesoCodigoNombre = "PENDIENTE";

                    defensaJudicial.ContratosAsociados = "PENDIENTE";
                    defensaJudicial.FuenteProceso = "PENDIENTE";

                    //contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

                    string TipoDocumentoContratistaTmp = string.Empty;
                    string NumeroIdentificacionContratistaTmp = string.Empty;

                    string TipoIntervencionCodigoTmp = string.Empty;
                    string TipoIntervencionNombreTmp = string.Empty;
                    foreach (var contr in defensaJudicial.DefensaJudicialContratacionProyecto)
                    {
                        var contratacionProyecto = _context.ContratacionProyecto.Where(x => x.ContratacionProyectoId == contr.ContratacionProyectoId).FirstOrDefault();
                        contr.numeroContrato = _context.Contrato.Where(x => x.ContratacionId == contratacionProyecto.ContratacionId).FirstOrDefault().NumeroContrato;
                    }
                    return defensaJudicial;

                }
            }
            catch (Exception e)
            {
                defensaJudicial1 = new DefensaJudicial
                {
                    DefensaJudicialId = 0,
                    NumeroProceso = e.InnerException.ToString(),
                    FechaCreacionFormat = DateTime.Now.ToString(),
                    FuenteProceso = e.ToString(),
                    ContratosAsociados= "ERROR",
                    TipoProcesoCodigo= "ERROR",
                    TipoAccionCodigo= "ERROR",
                    JurisdiccionCodigo= "ERROR",
                    
                };

            }
            return defensaJudicial1;
        }

        public async Task<byte[]> GetPlantillaDefensaJudicial(int pContratoId)
        {
            if (pContratoId == 0)
            {
                return Array.Empty<byte>();
            }   
                 

            //if (actaInicio.ContratacionId != null)
            //{
            //    VlrFase1Preconstruccion = getSumVlrContratoComponente(Convert.ToInt32(actaInicio.ContratacionId),
            //        "1"  //ConstanCodigoFaseContrato.Preconstruccion.ToString()
            //        ).ToString();

            //}

            
            //if (actaInicio == null)
            if (pContratoId == 0)
            {
                return Array.Empty<byte>();
            }

            Contrato contrato = null;
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            Contratacion contratacion = null;
            if (contrato != null)
                contratacion = _context.Contratacion.Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();

            Plantilla plantilla = null;


            //else if (contrato.TipoContratoCodigo == ((int)ConstanCodigoTipoContratacion.Obra).ToString())

            plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Ficha_Estudio_Defensa_Judicial).ToString()).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();


            //Plantilla plantilla = new Plantilla();
            //plantilla.Contenido = "";
            if (plantilla != null)
                plantilla.Contenido = await ReemplazarDatosPlantillaDefensaJudicial(plantilla.Contenido, pContratoId, "cdaza");
            return ConvertirPDF(plantilla);
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

        private async Task<string> ReemplazarDatosPlantillaDefensaJudicial(string strContenido, int pContratoId , string usuario)
        {
            string str = "";
            string valor = "";

            Contrato contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();
            ControversiaContractual controversiaContractual = null;

            ControversiaMotivo controversiaMotivo = null;
            ControversiaActuacion controversiaActuacion = null;

            ActuacionSeguimiento actuacionSeguimiento = null;
            if (contrato != null)
            {
                controversiaContractual = _context.ControversiaContractual
                   .Where(r => r.ContratoId == contrato.ContratoId).FirstOrDefault();

            }

            if (controversiaContractual != null)
            {
                controversiaMotivo = _context.ControversiaMotivo
                   .Where(r => r.ControversiaContractualId == controversiaContractual.ControversiaContractualId).FirstOrDefault();

                controversiaActuacion = _context.ControversiaActuacion
                    .Where(r => r.ControversiaContractualId == controversiaContractual.ControversiaContractualId).FirstOrDefault();

            }

            if (controversiaActuacion != null)
            {
                actuacionSeguimiento = _context.ActuacionSeguimiento
                    .Where(r => r.ControversiaActuacionId == controversiaActuacion.ControversiaActuacionId).FirstOrDefault();
            }

            Contratacion contratacion = null;
            Contratista contratista = null;

            NovedadContractual novedadContractual = null;   //sin rel????? SolicitudId NumeroSolicitud 

            DisponibilidadPresupuestal disponibilidadPresupuestal = null;

            if (contrato != null)
            {
                contratacion = _context.Contratacion
                    .Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();
            }

            if (contratacion != null)
            {
                contratista = _context.Contratista
                    .Where(r => r.ContratistaId == contratacion.ContratistaId).FirstOrDefault();

                disponibilidadPresupuestal = _context.DisponibilidadPresupuestal
                    .Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();
            }

            DefensaJudicial defensaJudicial = _context.DefensaJudicial
                    /*.Where(r => r.ContratistaId == contratacion.ContratistaId)*/.FirstOrDefault();

            //defensaJudicial.DefensaJudicialId

            //contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId && r.Eliminado == false).FirstOrDefault();
            contrato = _context.Contrato.Where(r => r.ContratoId == 16 && r.Eliminado == false).FirstOrDefault();

            if (contrato != null)
            {
                contratacion = _context.Contratacion.Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();

                if (contratacion != null)
                {
                    contratista = _context.Contratista.Where(r => r.ContratistaId == contratacion.ContratistaId).FirstOrDefault();
                }

            }

            controversiaContractual = _context.ControversiaContractual.Where(r => r.ContratoId == 16 && r.Eliminado == false
            && r.Eliminado == false).FirstOrDefault();

            // controversiaContractual = _context.ControversiaContractual.Where(r => r.ContratoId == contrato.ContratoId && r.Eliminado == false
            //&& r.Eliminado == false).FirstOrDefault();

            string strEstadoCodigoControversia = "sin definir";
            string strTipoControversiaCodigo = "sin definir";
            string strTipoControversia = "sin definir";

            Dominio TipoControversiaCodigo;

            TipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
            if (TipoControversiaCodigo != null)
            {
                strTipoControversia = TipoControversiaCodigo.Nombre;
                strTipoControversiaCodigo = TipoControversiaCodigo.Codigo;

            }
            controversiaMotivo = _context.ControversiaMotivo.Where(r => r.ControversiaContractualId == controversiaContractual.ControversiaContractualId).FirstOrDefault();

            string controversiaMotivoSolicitudNombre = "";
            Dominio MotivoSolicitudCodigo;
            if (controversiaMotivo != null)
            {

                MotivoSolicitudCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaMotivo.MotivoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
                if (MotivoSolicitudCodigo != null)
                {
                    //strTipoControversia = MotivoSolicitudCodigo.Nombre;
                    controversiaMotivoSolicitudNombre = MotivoSolicitudCodigo.Codigo;

                }

            }
            //string TipoProcesoCodigoText = ProcesoSeleccion.TipoProcesoCodigo != null ?
            //    await _commonService.GetNombreDominioByCodigoAndTipoDominio(ProcesoSeleccion.TipoProcesoCodigo,
            //    (int)EnumeratorTipoDominio.Tipo_Proceso_Seleccion) : "";

            strContenido = strContenido.Replace("_Numero_Solicitud_", controversiaContractual.NumeroSolicitud);
            strContenido = strContenido.Replace("_Fecha_Solicitud_", controversiaContractual.FechaSolicitud.ToString("dd/MM/yyyy"));
            //strContenido = strContenido.Replace("_Tipo_Controversia_", strTipoControversia);
            strContenido = strContenido.Replace("_Legitimacion_", defensaJudicial.LegitimacionCodigo);
            strContenido = strContenido.Replace("_Tipo_proceso_", defensaJudicial.TipoProcesoCodigo);
            strContenido = strContenido.Replace("_Numero_contratos_proceso_", "PENDIENTE");

            strContenido = strContenido.Replace("_Numero_Contrato_", contrato.NumeroContrato);
            strContenido = strContenido.Replace("_Nombre_Contratista_", contratista.Nombre ?? "");
            //strContenido = strContenido.Replace("_Fecha_inicio_contrato_", "PENDIENTE");
            //strContenido = strContenido.Replace("_Fecha_fin_contrato_", "PENDIENTE");
            //if (contratacion != null)
            //    strContenido = strContenido.Replace("_Cantidad_proyectos_asociados_", contratacion.ContratacionProyecto.Count().ToString());


            string TipoPlantillaDefensaJudicial = ((int)ConstanCodigoPlantillas.Ficha_Estudio_Defensa_Judicial).ToString();
            string DetalleProyecto = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaDefensaJudicial).Select(r => r.Contenido).FirstOrDefault();

            string TipoPlantillaRegistrosAlcance = ((int)ConstanCodigoPlantillas.Registros_Tabla_Alcance).ToString();
            string RegistroAlcance = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaRegistrosAlcance).Select(r => r.Contenido).FirstOrDefault();

            List<Dominio> ListaParametricas = _context.Dominio.ToList();

            List<Localizacion> ListaLocalizaciones = _context.Localizacion.ToList();
            List<InstitucionEducativaSede> ListaInstitucionEducativaSedes = _context.InstitucionEducativaSede.ToList();

            string DetallesProyectos = "";

            string RegistrosAlcance = "";
            //Contratacion pContratacion
            if (contratacion != null)
            {
                foreach (var proyecto in contratacion.ContratacionProyecto)
                {
                    DetallesProyectos += DetalleProyecto;

                    Localizacion Municipio = ListaLocalizaciones.Where(r => r.LocalizacionId == proyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    Localizacion Departamento = ListaLocalizaciones.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                    Localizacion Region = ListaLocalizaciones.Where(r => r.LocalizacionId == Departamento.IdPadre).FirstOrDefault();

                    InstitucionEducativaSede IntitucionEducativa = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == proyecto.Proyecto.InstitucionEducativaId).FirstOrDefault();
                    InstitucionEducativaSede Sede = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == proyecto.Proyecto.SedeId).FirstOrDefault();


                    strContenido = strContenido.Replace("_Tipo_Intervencion_", ListaParametricas
                    .Where(r => r.Codigo == proyecto.Proyecto.TipoIntervencionCodigo
                    && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).FirstOrDefault().Nombre);

                    strContenido = strContenido.Replace("_Llave_MEN_", proyecto.Proyecto.LlaveMen);
                    strContenido = strContenido.Replace("_Departamento_", Departamento.Descripcion);
                    strContenido = strContenido.Replace("_Municipio_", Municipio.Descripcion);

                    strContenido = strContenido.Replace("_Institucion_Educativa_", IntitucionEducativa.Nombre);

                    strContenido = strContenido.Replace("_Sede_", Sede.Nombre);

                    //strContenido = strContenido.Replace("_Tipo_Intervencion_", ListaParametricas
                    //.Where(r => r.Codigo == proyecto.Proyecto.TipoIntervencionCodigo
                    //&& r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).FirstOrDefault().Nombre);

                    strContenido = strContenido.Replace("_Llave_MEN_", proyecto.Proyecto.LlaveMen);
                    strContenido = strContenido.Replace("_Departamento_", Departamento.Descripcion);
                    strContenido = strContenido.Replace("_Municipio_", Municipio.Descripcion);
                    strContenido = strContenido.Replace("_Institucion_Educativa_", IntitucionEducativa.Nombre);
                    strContenido = strContenido.Replace("_Sede_", Sede.Nombre);

                    //Plazo de obra
                    strContenido = strContenido.Replace("_Meses_", proyecto.Proyecto.PlazoDiasObra.ToString());
                    strContenido = strContenido.Replace("_Dias_", proyecto.Proyecto.PlazoMesesObra.ToString());
                    //Plazo de Interventoría
                    strContenido = strContenido.Replace("_Meses_", proyecto.Proyecto.PlazoDiasInterventoria.ToString());
                    strContenido = strContenido.Replace("_Dias_", proyecto.Proyecto.PlazoMesesInterventoria.ToString());
                    strContenido = strContenido.Replace("_Valor_obra_", (proyecto.Proyecto.ValorObra != null) ? proyecto.Proyecto.ValorObra.ToString() : "0");
                    strContenido = strContenido.Replace("_Valor_Interventoria_", "$" + String.Format("{0:n0}", proyecto.Proyecto.ValorInterventoria));
                    strContenido = strContenido.Replace("_Valor_Total_proyecto_", "$" + String.Format("{0:n0}", proyecto.Proyecto.ValorTotal));
                    //strContenido = strContenido.Replace("", );

                }
                //DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, RegistrosAlcance);
                //DetallesProyectos = DetallesProyectos.Replace("[REGISTROS_ALCANCE]", RegistrosAlcance);                

            }

            //    strContenido = strContenido.Replace("Espacios_intervenir_nombre", );
            //strContenido = strContenido.Replace("Espacios_intervenir_cantidad", );

            //contratoAprobar.EstadoVerificacionCodigo = ConstanCodigoEstadoVerificacionContratoObra.Con_requisitos_del_contratista_de_obra_avalados;
            strContenido = strContenido.Replace("_Estado_obra_", "PENDIENTE");
            strContenido = strContenido.Replace("_Programacion_obra_acumulada_", "PENDIENTE");  //??
            strContenido = strContenido.Replace("_Avance_físico_acumulado_ejecutado_", "PENDIENTE");
            strContenido = strContenido.Replace("Facturacion_programada_acumulada_", "PENDIENTE");
            strContenido = strContenido.Replace("_Facturacion_ejecutada_acumulada_", "PENDIENTE");

            //strContenido = strContenido.Replace(">>>>>SECCION_TAI>>>>>>>>", "");
            //strContenido = strContenido.Replace("_Motivos_solicitud_", controversiaMotivoSolicitudNombre);
            //strContenido = strContenido.Replace("_Fecha_Comite_Pre_Tecnico_", controversiaContractual.FechaComitePreTecnico != null ? Convert.ToDateTime(controversiaContractual.FechaComitePreTecnico).ToString("dd/MM/yyyy") : controversiaContractual.FechaComitePreTecnico.ToString());

            //strContenido = strContenido.Replace("_Conclusion_Comite_pretecnico_", controversiaContractual.ConclusionComitePreTecnico);
            strContenido = strContenido.Replace("_URL_soportes_solicitud_", controversiaContractual.RutaSoporte);

            //strContenido = strContenido.Replace(">>>>> SECCION_OTRAS >>>>>>>>", "");
            //strContenido = strContenido.Replace("_Fecha_radicado_SAC_Numero_radicado_SAC_", controversiaContractual.NumeroRadicadoSac);
            //strContenido = strContenido.Replace("_Motivos_solicitud_", controversiaMotivoSolicitudNombre);
            //strContenido = strContenido.Replace("_Resumen_justificacion_solicitud_", controversiaContractual.MotivoJustificacionRechazo);
            strContenido = strContenido.Replace("_URL_soportes_solicitud_", controversiaContractual.RutaSoporte);

            //Historial de Modificaciones                           

            string prefijo = "";

            if (contrato != null)
            {
                if (contrato.TipoContratoCodigo == ConstanCodigoTipoContrato.Obra)
                    prefijo = ConstanPrefijoNumeroSolicitudControversia.Obra;
                else if (contrato.TipoContratoCodigo == ConstanCodigoTipoContrato.Interventoria)
                    prefijo = ConstanPrefijoNumeroSolicitudControversia.Interventoria;
            }
            controversiaContractual.NumeroSolicitudFormat = prefijo + controversiaContractual.ControversiaContractualId.ToString("000");

            strContenido = strContenido.Replace("Modificación 1", "");
            strContenido = strContenido.Replace("_Numero_solicitud_", controversiaContractual.NumeroSolicitudFormat);
            strContenido = strContenido.Replace("_Tipo_Novedad_", novedadContractual.TipoNovedadCodigo);

            strContenido = strContenido.Replace(">>>>> SECCION_SUSPENSION_PRORROGA_REINICIO >>>>>>>>", "");
            strContenido = strContenido.Replace("_Plazo_solicitado_", Convert.ToInt32(novedadContractual.PlazoAdicionalMeses).ToString());
            //strContenido = strContenido.Replace("_Plazo_solicitado_", novedadContractual.PlazoAdicionalDias);
            strContenido = strContenido.Replace("_Fecha_Inicio_", novedadContractual.FechaInicioSuspension != null ? Convert.ToDateTime(novedadContractual.FechaInicioSuspension).ToString("dd/MM/yyyy") : novedadContractual.FechaInicioSuspension.ToString());

            strContenido = strContenido.Replace("_Fecha_Fin_", novedadContractual.FechaFinSuspension != null ? Convert.ToDateTime(novedadContractual.FechaFinSuspension).ToString("dd/MM/yyyy") : novedadContractual.FechaFinSuspension.ToString());
            //ContratacionProyecto ComiteTecnicoProyecto  ComiteTecnico
            strContenido = strContenido.Replace("_Numero_Comite_Tecnico_", "PENDIENTE");
            strContenido = strContenido.Replace("_Numero_Comite_Fiduciario_", "PENDIENTE");

            if (disponibilidadPresupuestal != null)
            {
                //empuezo con fuentes
                var gestionfuentes = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado
                && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == disponibilidadPresupuestal.DisponibilidadPresupuestalId).
                    Include(x => x.FuenteFinanciacion).
                        ThenInclude(x => x.Aportante).
                        ThenInclude(x => x.CofinanciacionDocumento).
                    Include(x => x.DisponibilidadPresupuestalProyecto).
                        ThenInclude(x => x.Proyecto).
                            ThenInclude(x => x.Sede).
                    ToList();

                foreach (var gestion in gestionfuentes)
                {
                    //el saldo actual de la fuente son todas las solicitudes a la fuentes
                    //var consignadoemnfuente = _context.ControlRecurso.Where(x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId).Sum(x => x.ValorConsignacion);
                    var consignadoemnfuente = _context.FuenteFinanciacion.Where(x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId).Sum(x => x.ValorFuente);
                    var saldofuente = _context.GestionFuenteFinanciacion.Where(
                        x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId &&
                        x.DisponibilidadPresupuestalProyectoId != gestion.DisponibilidadPresupuestalProyectoId).Sum(x => x.ValorSolicitado);
                    string fuenteNombre = _context.Dominio.Where(x => x.Codigo == gestion.FuenteFinanciacion.FuenteRecursosCodigo
                            && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;
                    //(decimal)font.FuenteFinanciacion.ValorFuente,
                    // Saldo_actual_de_la_fuente = (decimal)font.FuenteFinanciacion.ValorFuente - saldofuente
                    //saldototal += (decimal)consignadoemnfuente - saldofuente;
                    string institucion = _context.InstitucionEducativaSede.Where(x => x.InstitucionEducativaSedeId == gestion.DisponibilidadPresupuestalProyecto.Proyecto.Sede.PadreId).FirstOrDefault().Nombre;
                    //var tr = plantilla_fuentes.Replace("[LLAVEMEN]", gestion.DisponibilidadPresupuestalProyecto.Proyecto.LlaveMen)
                    var tr = strContenido.Replace("[LLAVEMEN]", gestion.DisponibilidadPresupuestalProyecto.Proyecto.LlaveMen)
                        //.Replace("[INSTITUCION]", institucion)
                        //.Replace("[SEDE]", gestion.DisponibilidadPresupuestalProyecto.Proyecto.Sede.Nombre)
                        //.Replace("[APORTANTE]", this.getNombreAportante(gestion.FuenteFinanciacion.Aportante))
                        .Replace("[VALOR_APORTANTE]", gestion.FuenteFinanciacion.Aportante.CofinanciacionDocumento
                        .Sum(x => x.ValorDocumento).ToString())
                        .Replace("[FUENTE]", fuenteNombre)
                        //.Replace("[SALDO_FUENTE]", saldototal.ToString())
                        .Replace("[VALOR_FUENTE]", gestion.ValorSolicitado.ToString());
                    //.Replace("[NUEVO_SALDO_FUENTE]", (saldototal - gestion.ValorSolicitado).ToString());

                    //tablafuentes += tr;
                }

                //usos                    

            }

            string EstadoAvanceTramiteCodigoNombre = "";
            string ActuacionAdelantadaCodigoNombre = "";

            Dominio EstadoAvanceTramiteCodigo;
            //Dominio EstadoAvanceTramiteCodigo;
            Dominio ActuacionAdelantadaCodigo;

            if (controversiaActuacion != null)
            {
                EstadoAvanceTramiteCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.EstadoAvanceTramiteCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
                if (EstadoAvanceTramiteCodigo != null)
                {
                    //strTipoControversia = MotivoSolicitudCodigo.Nombre;
                    EstadoAvanceTramiteCodigoNombre = EstadoAvanceTramiteCodigo.Nombre;

                }

                ActuacionAdelantadaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.ActuacionAdelantadaCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
                if (ActuacionAdelantadaCodigo != null)
                {
                    ActuacionAdelantadaCodigoNombre = ActuacionAdelantadaCodigo.Nombre;
                    //ActuacionAdelantadaCodigoNombre = ActuacionAdelantadaCodigo.Codigo;

                }

            }

            //DemandadoConvocado fichaestudio DefensaJudicial
            DemandadoConvocado demandadoConvocado = _context.DemandadoConvocado
                    /*.Where(r => r.ContratistaId == contratacion.ContratistaId)*/.FirstOrDefault();
            FichaEstudio fichaestudio = _context.FichaEstudio
                    /*.Where(r => r.ContratistaId == contratacion.ContratistaId)*/.FirstOrDefault();
            //DefensaJudicial defensaJudicial;

            strContenido = strContenido.Replace("_Departamento_inicio_proceso_", "" );
            
            strContenido = strContenido.Replace("_Municipio_inicio_proceso_", "");
            strContenido = strContenido.Replace("_Autoridad_despacho_conocimiento_"  , demandadoConvocado.ConvocadoAutoridadDespacho );
            strContenido = strContenido.Replace("_Fecha_radicado_despacho_conocimiento_", demandadoConvocado.RadicadoDespacho);
            strContenido = strContenido.Replace("_Nombre_convocante_demandante_", demandadoConvocado.Nombre);
            strContenido = strContenido.Replace("_Nombre_convocado_demandado_", demandadoConvocado.Nombre);
            strContenido = strContenido.Replace("_Fecha_radicado_FFIE_", defensaJudicial.FechaRadicadoFfie != null ? Convert.ToDateTime(defensaJudicial.FechaRadicadoFfie).ToString("dd/MM/yyyy") : defensaJudicial.FechaRadicadoFfie.ToString());
            
            strContenido = strContenido.Replace("_Numero_radicado_FFIE_", defensaJudicial.NumeroRadicadoFfie);

            strContenido = strContenido.Replace("_Medio_Control_Accion_evitar_", demandadoConvocado.MedioControlAccion);
            strContenido = strContenido.Replace("_Proxima_actuacion_requerida_", controversiaActuacion.ProximaActuacionCodigo);
            strContenido = strContenido.Replace("_Fecha_vencimientos_terminos_proxima_actuacion_requerida_", controversiaActuacion.FechaVencimiento != null ? Convert.ToDateTime(controversiaActuacion.FechaVencimiento).ToString("dd/MM/yyyy") : controversiaActuacion.FechaVencimiento.ToString());
            
            strContenido = strContenido.Replace("_Caducidad_o_Prescripcion_", demandadoConvocado.CaducidadPrescripcion != null ? Convert.ToDateTime(demandadoConvocado.CaducidadPrescripcion).ToString("dd/MM/yyyy") : demandadoConvocado.CaducidadPrescripcion.ToString());
            
            strContenido = strContenido.Replace("_Pretensiones_", defensaJudicial.Pretensiones);
            strContenido = strContenido.Replace("_Cuantia_Perjuicios_", (defensaJudicial.CuantiaPerjuicios!=null)? defensaJudicial.CuantiaPerjuicios.ToString():"0");
            strContenido = strContenido.Replace("_Antecedentes_", fichaestudio.Antecedentes);
            strContenido = strContenido.Replace("_Hechos_relevantes_", fichaestudio.HechosRelevantes);

            //sin relaciones
            //fichaestudio.FichaEstudioId
            //defensaJudicial.DefensaJudicialId
            //    demandadoConvocado.DemandadoConvocadoId

            strContenido = strContenido.Replace("_Caducidad_o_Prescripcion_", demandadoConvocado.CaducidadPrescripcion != null ? Convert.ToDateTime(demandadoConvocado.CaducidadPrescripcion).ToString("dd/MM/yyyy") : demandadoConvocado.CaducidadPrescripcion.ToString());
            
            strContenido = strContenido.Replace("_Jurisprudencia_Doctrina_", fichaestudio.JurisprudenciaDoctrina);
            strContenido = strContenido.Replace("_Decision_Comite_casos_anteriores_Directrices_Conciliacion_", fichaestudio.DecisionComiteDirectrices);
            strContenido = strContenido.Replace("_Analisis_juridico_", fichaestudio.AnalisisJuridico);
            strContenido = strContenido.Replace("_URL_material_probatorio_", "PENDIENTE" );
            strContenido = strContenido.Replace("_Recomendaciones_", fichaestudio.Recomendaciones);
            strContenido = strContenido.Replace("_Abogado_elabora_estudio_", fichaestudio.Abogado);
            //strContenido = strContenido.Replace("", );
            //strContenido = strContenido.Replace("", );
            //strContenido = strContenido.Replace("", );


                        ///no van no incluir


                        //Historial de Actuaciones
               
            //(LISTA ACTUACIONES....)
     

            strContenido = strContenido.Replace("_Proxima_actuacion_requerida_", controversiaActuacion.ProximaActuacionCodigo);
            strContenido = strContenido.Replace("_Observaciones_", controversiaActuacion.Observaciones);
            strContenido = strContenido.Replace("_URL_soporte_", controversiaActuacion.RutaSoporte);

            //Resumen de la propuesta de reclamación ante la aseguradora:
            strContenido = strContenido.Replace("Actuación de la reclamación 1", "");
            strContenido = strContenido.Replace("_Estado_avance_reclamacion_", "PENDIENTE");
            strContenido = strContenido.Replace("_Fecha_actuacion_adelantada_", actuacionSeguimiento.FechaActuacionAdelantada != null ? Convert.ToDateTime(actuacionSeguimiento.FechaActuacionAdelantada).ToString("dd/MM/yyyy") : actuacionSeguimiento.FechaActuacionAdelantada.ToString());
            strContenido = strContenido.Replace("_Actuacion_adelantada_", actuacionSeguimiento.ActuacionAdelantada);
            strContenido = strContenido.Replace("_Proxima_actuacion_requerida_", actuacionSeguimiento.ProximaActuacion);
            strContenido = strContenido.Replace("_Observaciones_", actuacionSeguimiento.Observaciones);
            strContenido = strContenido.Replace("_URL_soporte_", actuacionSeguimiento.RutaSoporte);
            strContenido = strContenido.Replace("_reclamacion_resultado_definitivo_cerrado_ante_aseguradora_", Convert.ToBoolean(actuacionSeguimiento.EsResultadoDefinitivo).ToString());


            //datos exclusivos interventoria

            UsuarioPerfil UsuarioPerfil = _context.UsuarioPerfil.Where(y => y.Usuario.Email == usuario).Include(y => y.Perfil).FirstOrDefault();

            Perfil perfil = null;

            if (UsuarioPerfil != null)
            {
                perfil = _context.Perfil.Where(y => y.PerfilId == UsuarioPerfil.PerfilId).FirstOrDefault();

            }
            if (UsuarioPerfil != null)
            {
                strContenido = strContenido.Replace("_Cargo_Usuario_", perfil.Nombre);
            }
            strContenido = strContenido.Replace("_Nombre_Supervisor_", "_Nombre_Supervisor_");

            return strContenido;

        }
        public async Task<Respuesta> CreateOrEditFichaEstudio(FichaEstudio fichaEstudio)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Ficha_Estudio, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            FichaEstudio fichaEstudioBD = null;
            try
            {

                if (string.IsNullOrEmpty(fichaEstudio.FichaEstudioId.ToString()) || fichaEstudio.FichaEstudioId == 0)
                {
                    //Auditoria
                    strCrearEditar = "REGISTRAR FICHA ESTUDIO";
                    fichaEstudio.FechaCreacion = DateTime.Now;
                    fichaEstudio.UsuarioCreacion = fichaEstudio.UsuarioCreacion;
                    //fichaEstudio.DefensaJudicialId = fichaEstudio.DefensaJudicialId;
                    fichaEstudio.EsCompleto = ValidarRegistroCompletoFichaEstudio(fichaEstudio);
                    fichaEstudio.Eliminado = false;
                    _context.FichaEstudio.Add(fichaEstudio);
                }
                else
                {
                    strCrearEditar = "EDIT FICHA ESTUDIO";
                    fichaEstudioBD = _context.FichaEstudio.Find(fichaEstudio.FichaEstudioId);

                    //Auditoria
                    fichaEstudioBD.UsuarioModificacion = fichaEstudio.UsuarioModificacion;
                    fichaEstudioBD.Eliminado = false;

                    //Registros
                    fichaEstudioBD.AnalisisJuridico = fichaEstudio.AnalisisJuridico;
                    fichaEstudioBD.Antecedentes = fichaEstudio.Antecedentes;
                    fichaEstudioBD.DecisionComiteDirectrices = fichaEstudio.DecisionComiteDirectrices;
                    fichaEstudioBD.DefensaJudicial = fichaEstudio.DefensaJudicial;
                    fichaEstudioBD.TipoActuacionCodigo = fichaEstudio.TipoActuacionCodigo;
                    fichaEstudioBD.UsuarioModificacion = fichaEstudio.UsuarioModificacion;
                    fichaEstudioBD.Abogado = fichaEstudio.Abogado;
                    fichaEstudioBD.RutaSoporte = fichaEstudio.RutaSoporte;
                    fichaEstudioBD.Recomendaciones = fichaEstudio.Recomendaciones;
                    fichaEstudioBD.RecomendacionFinalComite = fichaEstudio.RecomendacionFinalComite;
                    fichaEstudioBD.JurisprudenciaDoctrina = fichaEstudio.JurisprudenciaDoctrina;

                    fichaEstudioBD.EsAprobadoAperturaProceso = fichaEstudio.EsAprobadoAperturaProceso;
                    fichaEstudioBD.EsPresentadoAnteComiteFfie = fichaEstudio.EsPresentadoAnteComiteFfie;

                    fichaEstudio.EsCompleto = ValidarRegistroCompletoFichaEstudio(fichaEstudioBD);

                    _context.FichaEstudio.Update(fichaEstudioBD);

                }

                _context.SaveChanges();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = fichaEstudio,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, fichaEstudio.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = fichaEstudio,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, fichaEstudio.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        private bool ValidarRegistroCompletoFichaEstudio(FichaEstudio fichaEstudio)
        {
            if (string.IsNullOrEmpty(fichaEstudio.Antecedentes)
             || string.IsNullOrEmpty(fichaEstudio.HechosRelevantes)
            || string.IsNullOrEmpty(fichaEstudio.JurisprudenciaDoctrina)
            || string.IsNullOrEmpty(fichaEstudio.DecisionComiteDirectrices)
            || string.IsNullOrEmpty(fichaEstudio.AnalisisJuridico)
            || string.IsNullOrEmpty(fichaEstudio.Recomendaciones)
            || string.IsNullOrEmpty(fichaEstudio.TipoActuacionCodigo)
                || (fichaEstudio.EsPresentadoAnteComiteFfie == null)
                || (fichaEstudio.EsAprobadoAperturaProceso == null)
               || (fichaEstudio.RecomendacionFinalComite == null)
               || string.IsNullOrEmpty(fichaEstudio.RutaSoporte))

            {
                return false;
            }

            return true;
        }
        public async Task<List<ProyectoGrilla>> GetListProyects(/*int pContratoId*/ int pProyectoId)
        {
            //Listar Los proyecto segun caso de uso solo trae los ue estado
            //estado de registro “Completo”, que tienen viabilidad jurídica y técnica
            List<ProyectoGrilla> ListProyectoGrilla = new List<ProyectoGrilla>();
            List<Proyecto> ListProyectos = new List<Proyecto>();
            try
            {
                ListProyectos = await _context.Proyecto.Where(
                         r => !(bool)r.Eliminado &&
                         r.EstadoJuridicoCodigo == ConstantCodigoEstadoJuridico.Aprobado
                         && (bool)r.RegistroCompleto
                         && r.ProyectoId == pProyectoId
                         //Se quitan los proyectos que ya esten vinculados a una contratacion
                         )
                                 .Include(r => r.ContratacionProyecto).ThenInclude(r => r.Contratacion)
                                 .Include(r => r.Sede).Include(r => r.InstitucionEducativa)
                                 .Include(r => r.LocalizacionIdMunicipioNavigation).Distinct().ToListAsync();

                //List<Localicacion> Municipios = new List<Localicacion>();

                //if (!string.IsNullOrEmpty(pDepartamento) && string.IsNullOrEmpty(pRegion) && string.IsNullOrEmpty(pMunicipio))
                //{
                //    Municipios = await _commonService.GetListMunicipioByIdDepartamento(pDepartamento);
                //}

                //if (!string.IsNullOrEmpty(pRegion) && string.IsNullOrEmpty(pDepartamento) && string.IsNullOrEmpty(pMunicipio))
                //{
                //    List<Localizacion> Departamentos = _context.Localizacion.Where(r => r.IdPadre == pRegion).ToList();
                //    foreach (var dep in Departamentos)
                //    {
                //        Municipios.AddRange(await _commonService.GetListMunicipioByIdDepartamento(dep.LocalizacionId));
                //    }
                //}
                //if (Municipios.Count() > 0)
                //{
                //    //ListContratacion.RemoveAll(item => LisIdContratacion.Contains(item.ContratacionId));
                //    ListProyectos.RemoveAll(item => !Municipios.Select(r => r.LocalizacionId).Contains(item.LocalizacionIdMunicipio));
                //}

                //List<Proyecto> ListaProyectosRemover = new List<Proyecto>();
                //foreach (var Proyecto in ListProyectos)
                //{
                //    foreach (var ContratacionProyecto in Proyecto.ContratacionProyecto)
                //    {
                //        if (ContratacionProyecto.Contratacion.EstadoSolicitudCodigo
                //            != ConstanCodigoEstadoSolicitudContratacion.Rechazado)
                //        {
                //            ListaProyectosRemover.Add(Proyecto);
                //        }
                //        else
                //        {
                //            if (Proyecto.ContratacionProyecto.Where(r => r.ProyectoId == Proyecto.ProyectoId).Count() > 1)
                //            {
                //                ListaProyectosRemover.Add(Proyecto);
                //            }
                //        }
                //    }
                //}

                //foreach (var proyecto in ListaProyectosRemover.Distinct())
                //{
                //    ListProyectos.Remove(proyecto);
                //}

                List<Dominio> ListTipoSolicitud = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Tipo_de_Solicitud_Obra_Interventorias);

                //Lista para Dominio intervencio
                List<Dominio> ListTipoIntervencion = await _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && (bool)r.Activo).ToListAsync();

                List<Localizacion> ListDepartamentos = await _context.Localizacion.Where(r => r.Nivel == 1).ToListAsync();

                List<Localizacion> ListRegiones = await _context.Localizacion.Where(r => r.Nivel == 3).ToListAsync();
                //departamneto 
                //    Region  
                List<Contratacion> ListContratacion = await _context.Contratacion.Where(r => !(bool)r.Eliminado).ToListAsync();

                string strProyectoUrlMonitoreo = string.Empty;


                foreach (var proyecto in ListProyectos)
                {
                    if (!string.IsNullOrEmpty(proyecto.TipoIntervencionCodigo))
                    {
                        Localizacion departamento = ListDepartamentos.Find(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipioNavigation.IdPadre);

                        if (proyecto.UrlMonitoreo != null)
                            strProyectoUrlMonitoreo = proyecto.UrlMonitoreo;

                        try
                        {
                            ProyectoGrilla proyectoGrilla = new ProyectoGrilla
                            {
                                TipoIntervencion = ListTipoIntervencion.Find(r => r.Codigo == proyecto.TipoIntervencionCodigo).Nombre,
                                LlaveMen = proyecto.LlaveMen,
                                Departamento = departamento.Descripcion,
                                Region = ListRegiones.Find(r => r.LocalizacionId == departamento.IdPadre).Descripcion,
                                //  Departamento = _commonService.GetNombreDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio),
                                // Municipio = _commonService.GetNombreLocalizacionByLocalizacionId(proyecto.LocalizacionIdMunicipio),
                                Municipio = proyecto.LocalizacionIdMunicipioNavigation.Descripcion,
                                //InstitucionEducativa = _context.InstitucionEducativaSede.Find(proyecto.InstitucionEducativaId).Nombre,
                                //Sede = _context.InstitucionEducativaSede.Find(proyecto.SedeId).Nombre,
                                InstitucionEducativa = proyecto.InstitucionEducativa.CodigoDane,
                                CodigoDane = proyecto.InstitucionEducativa.Nombre,
                                Sede = proyecto.Sede.Nombre,
                                SedeCodigo = proyecto.Sede.CodigoDane,
                                ProyectoId = proyecto.ProyectoId,


                                //URLMonitoreo = strProyectoUrlMonitoreo,
                                //ContratoId = 0, //await getContratoIdByProyectoId(proyecto.ProyectoId),


                            };

                            //r.TipoIntervencionCodigo == (string.IsNullOrEmpty(pTipoIntervencion) ? r.TipoIntervencionCodigo : pTipoIntervencion) &&
                            //List<Contrato> lstContratos = _context.Contrato.Where(r => r.ContratoId == pContratoId).ToList();


                            foreach (var item in proyecto.ContratacionProyecto)
                            {
                                item.Contratacion = ListContratacion.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefault();
                                //item.Contratacion = ListContratacion.Where(r => r.ContratacionId == contrato.ContratacionId ).FirstOrDefault();

                                //item.Contratacion= item.Contratacion.wh(r => r.ContratacionId == item.ContratacionId ).FirstOrDefault();

                                Contratista contratista = null;

                                if (item.Contratacion != null)
                                {
                                     contratista = _context.Contratista.Where(r=>r.ContratistaId== item.Contratacion.ContratistaId).FirstOrDefault();

                                    if (contratista != null)
                                        proyectoGrilla.NombreContratista = contratista.Nombre;
                                    else
                                        proyectoGrilla.NombreContratista = "";

                                    if (!string.IsNullOrEmpty(item.Contratacion.TipoSolicitudCodigo))
                                    {
                                        if (item.Contratacion.TipoSolicitudCodigo == "1")
                                        {
                                            proyectoGrilla.TieneObra = true;
                                        }
                                        if (item.Contratacion.TipoSolicitudCodigo == "2")
                                        {
                                            proyectoGrilla.TieneInterventoria = true;
                                        }
                                    }
                                }
                            }
                            ListProyectoGrilla.Add(proyectoGrilla);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return ListProyectoGrilla.OrderByDescending(r => r.ProyectoId).ToList();
            }
            //ListProyectoGrilla = ListProyectoGrilla.Where(r => r.ContratoId != 0).ToList();

            foreach (ProyectoGrilla element in ListProyectoGrilla)
            {
                //element.ContratoId = await getContratoIdByProyectoId(element.ProyectoId);
            }

            return ListProyectoGrilla.OrderByDescending(r => r.ProyectoId).ToList();
        }
        public async Task<Respuesta> EliminarDefensaJudicial(int pDefensaJudicialId, string pUsuarioModifico)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Defensa_Judicial, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            
            DefensaJudicial defensaJudicial = null;

            try
            {
                defensaJudicial = await _context.DefensaJudicial.Where(d => d.DefensaJudicialId == pDefensaJudicialId).FirstOrDefaultAsync();

                if (defensaJudicial != null)
                {
                    strCrearEditar = "ELIMINAR DEFENSA JUDICIAL";
                    defensaJudicial.FechaModificacion = DateTime.Now;
                    defensaJudicial.UsuarioModificacion = pUsuarioModifico;
                    //controversiaActuacion.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                    defensaJudicial.Eliminado = true;
                    _context.DefensaJudicial.Update(defensaJudicial);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = defensaJudicial,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, defensaJudicial.UsuarioModificacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = defensaJudicial,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, defensaJudicial.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> CambiarEstadoDefensaJudicial(int pDefensaJudicialId, string pCodigoEstado, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Proceso, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //DefensaJudicial /*defensaJudicial*/
                DefensaJudicial defensaJudicialOld = _context.DefensaJudicial.Find(pDefensaJudicialId);
                defensaJudicialOld.UsuarioModificacion = pUsuarioModifica;
                defensaJudicialOld.FechaModificacion = DateTime.Now;
                defensaJudicialOld.EstadoProcesoCodigo = pCodigoEstado;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO PROCESO DEFENSA JUDICIAL")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }
        public async Task<List<GrillaProcesoDefensaJudicial>> ListGrillaProcesosDefensaJudicial()
        {
            //await AprobarContratoByIdContrato(1);

            List<GrillaProcesoDefensaJudicial> ListDefensaJudicialGrilla = new List<GrillaProcesoDefensaJudicial>();
            //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)

            //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo        

            //List <Contrato> ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).Include(r => r.NumeroContrato).Include(r => r.Estado).Distinct().ToListAsync();
            List<DefensaJudicial> ListDefensaJudicial  = await _context.DefensaJudicial.Where(r => (bool)r.Eliminado==false).Distinct().ToListAsync();

            foreach (var defensaJudicial in ListDefensaJudicial)
            {
                try
                {     
                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    string TipoAccionNombre = "sin definir";

                    string strEstadoSolicitudCodigoContratoPoliza = "sin definir";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio TipoAccion;
                    Dominio EstadoSolicitudCodigoContratoPoliza;

                    //if (contratoPoliza != null)
                    //{
                    TipoAccion = await _commonService.GetDominioByNombreDominioAndTipoDominio(defensaJudicial.TipoAccionCodigo, (int)EnumeratorTipoDominio.Tipo_accion_judicial);
                    //TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo.Trim(), (int)EnumeratorTipoDominio.Tipo_de_Solicitud);
                    if (TipoAccion != null)
                        TipoAccionNombre = TipoAccion.Nombre;

                    //EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.EstadoPolizaCodigo.Trim(), (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                    //if (EstadoSolicitudCodigoContratoPoliza != null)
                    //    strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

                    //}
                    bool bRegistroCompleto = false;
                    string strRegistroCompleto = "Incompleto";

                    //if (defensaJudicial.EsCompleto != null)
                    //{
                        strRegistroCompleto = (bool)defensaJudicial.EsCompleto ? "Completo" : "Incompleto";
                    //}

                    //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                    GrillaProcesoDefensaJudicial defensaJudicialGrilla = new GrillaProcesoDefensaJudicial
                    {
                        DefensaJudicialId = defensaJudicial.DefensaJudicialId,
                        FechaRegistro = defensaJudicial.FechaCreacion.ToString("dd/MM/yyyy"),
                        LegitimacionPasivaActiva= (bool)defensaJudicial.EsLegitimacionActiva ? "Activa" : "Pasiva",
                        NumeroProceso="DJ"+ defensaJudicial.DefensaJudicialId.ToString("000") + defensaJudicial.FechaCreacion.ToString("yyyy"),
                        TipoAccionCodigo= defensaJudicial.TipoAccionCodigo,
                        TipoAccion = TipoAccionNombre,
                        EstadoProceso= "PENDIENTE",
                        EstadoProcesoCodigo =defensaJudicial.EstadoProcesoCodigo,
                        
                        RegistroCompletoNombre =strRegistroCompleto,
                        TipoProceso="PENDIENTE",
                        TipoProcesoCodigo = defensaJudicial.TipoProcesoCodigo,                       
                                          
                    };

                    //if (!(bool)proyecto.RegistroCompleto)
                    //{
                    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    //}
                    ListDefensaJudicialGrilla.Add(defensaJudicialGrilla);
                }
                catch (Exception e)
                {
                    GrillaProcesoDefensaJudicial defensaJudicialGrilla = new GrillaProcesoDefensaJudicial
                    {
                        DefensaJudicialId = defensaJudicial.DefensaJudicialId,
                        FechaRegistro = e.ToString(),
                        LegitimacionPasivaActiva = e.InnerException.ToString(),
                        NumeroProceso = "ERROR",
                        TipoAccionCodigo = "ERROR",
                        TipoAccion = "ERROR",
                        EstadoProceso = "ERROR",
                        EstadoProcesoCodigo = "ERROR",

                        RegistroCompletoNombre = "ERROR",
                        TipoProceso = "ERROR",
                        TipoProcesoCodigo = "ERROR",                  
    
                    };
                    ListDefensaJudicialGrilla.Add(defensaJudicialGrilla);
                }
            }
            return ListDefensaJudicialGrilla.ToList();

        }

        /*autor: jflorez
          descripción: trae listado de contratos
          impacto: CU 4.2.2*/
        public async Task<List<Contrato>> GetListContract()
        {
            var contratos = _context.Contrato.Where(x =>//x.UsuarioInterventoria==userID
             !(bool)x.Eliminado
            ).ToList();
            
            return contratos;
        }


        public async Task<List<ProyectoGrilla>> GetListProyectsByContract(int pContratoId)
        {
            List<ProyectoGrilla> ListProyectoGrilla = new List<ProyectoGrilla>();
            var contrato = _context.Contrato.Find(pContratoId);
            var proyecto = _context.Contratacion.Where(x => x.ContratacionId == contrato.ContratacionId).
                Include(x=>x.ContratacionProyecto).
                    ThenInclude(y=>y.Proyecto).
                    ThenInclude(y=>y.InstitucionEducativa).
                Include(x => x.ContratacionProyecto).
                    ThenInclude(y => y.Proyecto).
                    ThenInclude(y => y.Sede).
                Include(x=>x.Contratista).FirstOrDefault();
            foreach (var item in proyecto.ContratacionProyecto)
            {
                ListProyectoGrilla.Add(new ProyectoGrilla {
                    NombreContratista=proyecto.Contratista.Nombre,
                    TieneObra=proyecto.TipoSolicitudCodigo== ConstanCodigoTipoContratacion.Obra.ToString(),
                    TieneInterventoria= proyecto.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString(),
                    ProyectoId=item.ProyectoId,
                    ContratacionProyectoId= item.ContratacionProyectoId,
                    InstitucionEducativa=item.Proyecto.InstitucionEducativa.Nombre,
                    Sede=item.Proyecto.Sede.Nombre,
                    CodigoDane=item.Proyecto.InstitucionEducativa.CodigoDane,
                    SedeCodigo=item.Proyecto.Sede.CodigoDane
                });
                
            }
            return ListProyectoGrilla;
        }
    }
}
