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
                    foreach (var defFicha in defensaJudicial.FichaEstudio)
                    {
                        defFicha.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                        defFicha.FechaCreacion = DateTime.Now;
                        defFicha.Eliminado = false;
                    }
                    defensaJudicial.EstadoProcesoCodigo = "1";
                    defensaJudicial.LegitimacionCodigo = "";
                    defensaJudicial.NumeroProceso = Helpers.Helpers.Consecutive("DJ",_context.DefensaJudicial.Count());
                    _context.DefensaJudicial.Add(defensaJudicial);
                }
                else
                {
                    strCrearEditar = "EDIT DEFENSA JUDICIAL";
                    defensaJudicialBD = _context.DefensaJudicial.Where(x=>x.DefensaJudicialId==defensaJudicial.DefensaJudicialId).
                        Include(x=>x.DemandadoConvocado).Include(x=>x.DemandanteConvocante).
                        Include(x=>x.DefensaJudicialSeguimiento).Include(x=>x.DefensaJudicialContratacionProyecto).
                        Include(x=>x.FichaEstudio).FirstOrDefault();

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

                    //url
                    defensaJudicialBD.UrlSoporteProceso = defensaJudicial.UrlSoporteProceso;
                    //contratos
                    defensaJudicialBD.CantContratos = defensaJudicial.CantContratos;

                    foreach (var defContratcionProyecto in defensaJudicial.DefensaJudicialContratacionProyecto)
                    {
                        if(defContratcionProyecto.UsuarioCreacion==null)
                        {
                            defContratcionProyecto.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                            defContratcionProyecto.FechaCreacion = DateTime.Now;
                            defContratcionProyecto.EsCompleto = true;
                            defContratcionProyecto.Eliminado = false;
                            defContratcionProyecto.DefensaJudicialId = defensaJudicialBD.DefensaJudicialId;
                            _context.DefensaJudicialContratacionProyecto.Add(defContratcionProyecto);
                        }
                        else
                        {
                            defContratcionProyecto.UsuarioModificacion = defensaJudicial.UsuarioModificacion;
                            defContratcionProyecto.FechaModificacion = DateTime.Now;
                            defContratcionProyecto.EsCompleto = true;
                            defContratcionProyecto.Eliminado = false;
                        }
                        
                    }
                    foreach (var defConvocado in defensaJudicial.DemandadoConvocado)
                    {
                        if (defConvocado.UsuarioCreacion == null)
                        {
                            defConvocado.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                            defConvocado.FechaCreacion = DateTime.Now;
                            defConvocado.Eliminado = false;
                        }
                        else
                        {
                            defConvocado.UsuarioModificacion = defensaJudicial.UsuarioCreacion;
                            defConvocado.FechaModificacion = DateTime.Now;
                            defConvocado.Eliminado = false;
                        }
                            
                    }
                    foreach (var defFicha in defensaJudicial.FichaEstudio)
                    {
                        if(defFicha.UsuarioCreacion==null)
                        {
                            defFicha.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                            defFicha.FechaCreacion = DateTime.Now;
                            defFicha.Eliminado = false;
                            defFicha.DefensaJudicialId = defensaJudicialBD.DefensaJudicialId;
                            _context.FichaEstudio.Add(defFicha);
                        }
                        else
                        {
                            defFicha.UsuarioModificacion = defensaJudicial.UsuarioCreacion;
                            defFicha.FechaModificacion = DateTime.Now;
                            defFicha.Eliminado = false;
                        }
                        
                    }
                    foreach (var defFicha in defensaJudicial.DefensaJudicialSeguimiento)
                    {
                        if (defFicha.DefensaJudicialSeguimientoId == 0)
                        {
                            defFicha.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                            defFicha.FechaCreacion = DateTime.Now;
                            defFicha.Eliminado = false;
                            defFicha.EsCompleto = true;
                            defFicha.DefensaJudicialId = defensaJudicialBD.DefensaJudicialId;
                            _context.DefensaJudicialSeguimiento.Add(defFicha);
                        }
                        //si entra aqui, cambio el estado de la defensa judicial
                        defensaJudicialBD.EstadoProcesoCodigo = "9";//en desaroolo
                    }
                    foreach (var defFicha in defensaJudicial.DemandadoConvocado)
                    {
                        if (defFicha.DemandadoConvocadoId == 0)
                        {
                            defFicha.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                            defFicha.FechaCreacion = DateTime.Now;
                            defFicha.Eliminado = false;
                            defFicha.DefensaJudicialId = defensaJudicialBD.DefensaJudicialId;
                            _context.DemandadoConvocado.Add(defFicha);
                        }
                    }
                    foreach (var defFicha in defensaJudicial.DemandanteConvocante)
                    {
                        if (defFicha.DemandanteConvocadoId == 0)
                        {
                            defFicha.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                            defFicha.FechaCreacion = DateTime.Now;
                            defFicha.Eliminado = false;
                            defFicha.DefensaJucicialId = defensaJudicialBD.DefensaJudicialId;
                            _context.DemandanteConvocante.Add(defFicha);
                        }
                    }
                    defensaJudicialBD.EsCompleto = ValidarRegistroCompleto(defensaJudicialBD);
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

        private bool ValidarRegistroCompleto(DefensaJudicial defensaJudicial)
        {
            bool retorno = true;
            //valido contratos
            if(defensaJudicial.CantContratos==0 || defensaJudicial.CantContratos == null)
            {
                retorno = false;
            }
            if (defensaJudicial.DefensaJudicialContratacionProyecto.Count()==0)
            {
                retorno = false;
            }

            //valido detalle
            if(defensaJudicial.LocalizacionIdMunicipio==null|| defensaJudicial.TipoAccionCodigo== null || defensaJudicial.JurisdiccionCodigo == null ||
                defensaJudicial.Pretensiones == null || defensaJudicial.Pretensiones == "" || defensaJudicial.CuantiaPerjuicios == null ||
                defensaJudicial.EsRequiereSupervisor == null 
                /*|| defensaJudicial.FechaRadicadoFfie == null || defensaJudicial.NumeroRadicadoFfie == null ||
                defensaJudicial.CanalIngresoCodigo == null*/)
            {
                retorno = false;
            }

            //demandantes/convocantes
            if(defensaJudicial.DemandadoConvocado.Count()==0)
            {
                retorno = false;
            }
            if (defensaJudicial.DemandanteConvocante.Count() == 0)
            {
                retorno = false;
            }

            //soporte
            if (defensaJudicial.UrlSoporteProceso==null)
            {
                retorno = false;
            }

            //ficha de estudio
            if(defensaJudicial.FichaEstudio.Count()>0)
            {
                if (!ValidarRegistroCompletoFichaEstudio(defensaJudicial.FichaEstudio.FirstOrDefault()))
                {
                    retorno = false;
                }
            }
            else
            {
                retorno = false;                
            }
            

            return retorno;
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
                    Include(x=>x.DemandadoConvocado).
                    Include(x=>x.DemandanteConvocante).
                    Include(x=>x.FichaEstudio).
                    
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

                    defensaJudicial.DepartamentoID = defensaJudicial.LocalizacionIdMunicipio == null ? "" : _context.Localizacion.Where(z=>z.LocalizacionId==defensaJudicial.LocalizacionIdMunicipio.ToString()).FirstOrDefault().IdPadre;
                    foreach (var contr in defensaJudicial.DefensaJudicialContratacionProyecto)
                    {
                        var contratacionProyecto = _context.ContratacionProyecto.Where(x => x.ContratacionProyectoId == contr.ContratacionProyectoId).
                             Include(y => y.Proyecto).
                                ThenInclude(y => y.InstitucionEducativa).
                            Include(y => y.Proyecto).
                                ThenInclude(y => y.Sede).FirstOrDefault();
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

        public async Task<byte[]> GetPlantillaDefensaJudicial(int pDefensaJudicialId)
        {
            if (pDefensaJudicialId == 0)
            {
                return Array.Empty<byte>();
            }   
                 
    
            Plantilla plantilla = null;


            //else if (contrato.TipoContratoCodigo == ((int)ConstanCodigoTipoContratacion.Obra).ToString())

            plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Ficha_Estudio_Defensa_Judicial).ToString()).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();


            //Plantilla plantilla = new Plantilla();
            //plantilla.Contenido = "";
            if (plantilla != null)
                plantilla.Contenido = await ReemplazarDatosPlantillaDefensaJudicial(plantilla.Contenido, pDefensaJudicialId);
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

        private async Task<string> ReemplazarDatosPlantillaDefensaJudicial(string strContenido, int prmdefensaJudicialID )
        {
            string str = "";
            string valor = "";

            var defPrincial =await GetVistaDatosBasicosProceso(prmdefensaJudicialID);

            strContenido = strContenido.Replace("_Numero_Solicitud_", defPrincial.NumeroProceso);
            strContenido = strContenido.Replace("_Fecha_Solicitud_", defPrincial.FechaCreacion.ToString("dd/MM/yyyy"));
            //strContenido = strContenido.Replace("_Tipo_Controversia_", strTipoControversia);
            strContenido = strContenido.Replace("_Legitimacion_", defPrincial.EsLegitimacionActiva?"Activa":"Pasiva");
            strContenido = strContenido.Replace("_Tipo_proceso_", defPrincial.TipoProcesoCodigoNombre);
            strContenido = strContenido.Replace("_Numero_contratos_proceso_", defPrincial.CantContratos.ToString());

            var plantillatrContratos = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Ficha_estudio_tr_contratos).ToString()).FirstOrDefault().Contenido;
            
            var ListaLocalizaciones = _context.Localizacion.ToList();
            var ListaInstitucionEducativaSedes = _context.InstitucionEducativaSede.ToList();
            var ListaParametricas = _context.Dominio.ToList();
            if (defPrincial.DefensaJudicialContratacionProyecto != null)
            {
                
                int contador = 1;
                foreach (var defcontratac in defPrincial.DefensaJudicialContratacionProyecto)
                {
                    
                    Localizacion Municipio = ListaLocalizaciones.Where(r => r.LocalizacionId == defcontratac.ContratacionProyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    Localizacion Departamento = ListaLocalizaciones.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                    Localizacion Region = ListaLocalizaciones.Where(r => r.LocalizacionId == Departamento.IdPadre).FirstOrDefault();

                    InstitucionEducativaSede IntitucionEducativa = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == defcontratac.ContratacionProyecto.Proyecto.InstitucionEducativaId).FirstOrDefault();
                    InstitucionEducativaSede Sede = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == defcontratac.ContratacionProyecto.Proyecto.SedeId).FirstOrDefault();
                    var contratacion = _context.Contratacion.
                        Where(x => x.ContratacionId == defcontratac.ContratacionProyecto.ContratacionId).
                        Include(x => x.Contratista).FirstOrDefault();

                    plantillatrContratos = plantillatrContratos.Replace("_Tipo_Intervencion_", ListaParametricas
                    .Where(r => r.Codigo == defcontratac.ContratacionProyecto.Proyecto.TipoIntervencionCodigo
                    && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).FirstOrDefault().Nombre);

                    plantillatrContratos = plantillatrContratos.Replace("_Llave_MEN_", defcontratac.ContratacionProyecto.Proyecto.LlaveMen);
                    plantillatrContratos = plantillatrContratos.Replace("_Departamento_", Departamento.Descripcion);
                    plantillatrContratos = plantillatrContratos.Replace("_Municipio_", Municipio.Descripcion);

                    plantillatrContratos = plantillatrContratos.Replace("_Institucion_Educativa_", IntitucionEducativa.Nombre);

                    plantillatrContratos = plantillatrContratos.Replace("_Sede_", Sede.Nombre);

                    //Plazo de obra
                    plantillatrContratos = plantillatrContratos.Replace("_MesesObra_", defcontratac.ContratacionProyecto.Proyecto.PlazoDiasObra.ToString());
                    plantillatrContratos = plantillatrContratos.Replace("_DiasObra_", defcontratac.ContratacionProyecto.Proyecto.PlazoMesesObra.ToString());
                    //Plazo de Interventoría
                    plantillatrContratos = plantillatrContratos.Replace("_Meses_", defcontratac.ContratacionProyecto.Proyecto.PlazoDiasInterventoria.ToString());
                    plantillatrContratos = plantillatrContratos.Replace("_Dias_", defcontratac.ContratacionProyecto.Proyecto.PlazoMesesInterventoria.ToString());
                    plantillatrContratos = plantillatrContratos.Replace("_Valor_obra_", (defcontratac.ContratacionProyecto.Proyecto.ValorObra != null) ? defcontratac.ContratacionProyecto.Proyecto.ValorObra.ToString() : "0");
                    plantillatrContratos = plantillatrContratos.Replace("_Valor_Interventoria_", "$" + String.Format("{0:n0}", defcontratac.ContratacionProyecto.Proyecto.ValorInterventoria));
                    plantillatrContratos = plantillatrContratos.Replace("_Valor_Total_proyecto_", "$" + String.Format("{0:n0}", defcontratac.ContratacionProyecto.Proyecto.ValorTotal));
                    plantillatrContratos = plantillatrContratos.Replace("_contador_", contador.ToString());
                    plantillatrContratos = plantillatrContratos.Replace("_Numero_Contrato_", defcontratac.numeroContrato);

                    plantillatrContratos = plantillatrContratos.Replace("_Nombre_Contratista_", contratacion.Contratista.Nombre);

                    plantillatrContratos = plantillatrContratos.Replace("_Estado_obra_", "PENDIENTE");
                    plantillatrContratos = plantillatrContratos.Replace("_Programacion_obra_acumulada_", "PENDIENTE");  //??
                    plantillatrContratos = plantillatrContratos.Replace("_Avance_físico_acumulado_ejecutado_", "PENDIENTE");
                    plantillatrContratos = plantillatrContratos.Replace("Facturacion_programada_acumulada_", "PENDIENTE");
                    plantillatrContratos = plantillatrContratos.Replace("_Facturacion_ejecutada_acumulada_", "PENDIENTE");
                    contador++;

                }
                strContenido = strContenido.Replace("_trplantillacontratacion_", plantillatrContratos);
                



            }


            //strContenido = strContenido.Replace("_URL_soportes_solicitud_", controversiaContractual.RutaSoporte);

            Localizacion Municipioproceso = ListaLocalizaciones.Where(r => r.LocalizacionId == defPrincial.LocalizacionIdMunicipio.ToString()).FirstOrDefault();
            Localizacion DepartamentoProceso = ListaLocalizaciones.Where(r => r.LocalizacionId == Municipioproceso.IdPadre).FirstOrDefault();
            var demandadoConvocado = defPrincial.DemandadoConvocado.FirstOrDefault();

            strContenido = strContenido.Replace("_Departamento_inicio_proceso_", DepartamentoProceso.Descripcion );
            
            strContenido = strContenido.Replace("_Municipio_inicio_proceso_", Municipioproceso.Descripcion);
            strContenido = strContenido.Replace("_Autoridad_despacho_conocimiento_"  , demandadoConvocado.ConvocadoAutoridadDespacho );
            strContenido = strContenido.Replace("_Fecha_radicado_despacho_conocimiento_", demandadoConvocado.RadicadoDespacho);
            strContenido = strContenido.Replace("_Nombre_convocante_demandante_", demandadoConvocado.Nombre);
            strContenido = strContenido.Replace("_Nombre_convocado_demandado_", demandadoConvocado.Nombre);
            strContenido = strContenido.Replace("_Fecha_radicado_FFIE_", defPrincial.FechaRadicadoFfie != null ? Convert.ToDateTime(defPrincial.FechaRadicadoFfie).ToString("dd/MM/yyyy") : defPrincial.FechaRadicadoFfie.ToString());
            
            strContenido = strContenido.Replace("_Numero_radicado_FFIE_", defPrincial.NumeroRadicadoFfie);

            strContenido = strContenido.Replace("_Medio_Control_Accion_evitar_", demandadoConvocado.MedioControlAccion);
            strContenido = strContenido.Replace("_Proxima_actuacion_requerida_", defPrincial.DefensaJudicialSeguimiento.Count()==0?"": defPrincial.DefensaJudicialSeguimiento.FirstOrDefault().ProximaActuacion);
            strContenido = strContenido.Replace("_Fecha_vencimientos_terminos_proxima_actuacion_requerida_", defPrincial.DefensaJudicialSeguimiento.Count() == 0 ? "" : defPrincial.DefensaJudicialSeguimiento.FirstOrDefault().FechaVencimiento != null ? Convert.ToDateTime(defPrincial.DefensaJudicialSeguimiento.FirstOrDefault().FechaVencimiento).ToString("dd/MM/yyyy") : defPrincial.DefensaJudicialSeguimiento.FirstOrDefault().FechaVencimiento.ToString());
            
            strContenido = strContenido.Replace("_Caducidad_o_Prescripcion_", demandadoConvocado.CaducidadPrescripcion != null ? Convert.ToDateTime(demandadoConvocado.CaducidadPrescripcion).ToString("dd/MM/yyyy") : demandadoConvocado.CaducidadPrescripcion.ToString());
            
            strContenido = strContenido.Replace("_Pretensiones_", defPrincial.Pretensiones);
            strContenido = strContenido.Replace("_Cuantia_Perjuicios_", (defPrincial.CuantiaPerjuicios!=null)? defPrincial.CuantiaPerjuicios.ToString():"0");
            strContenido = strContenido.Replace("_Antecedentes_", defPrincial.FichaEstudio.Count()==0?"":defPrincial.FichaEstudio.FirstOrDefault().Antecedentes);
            strContenido = strContenido.Replace("_Hechos_relevantes_", defPrincial.FichaEstudio.Count() == 0 ? "" : defPrincial.FichaEstudio.FirstOrDefault().HechosRelevantes);

            
            strContenido = strContenido.Replace("_Caducidad_o_Prescripcion_", demandadoConvocado.CaducidadPrescripcion != null ? Convert.ToDateTime(demandadoConvocado.CaducidadPrescripcion).ToString("dd/MM/yyyy") : demandadoConvocado.CaducidadPrescripcion.ToString());
            
            strContenido = strContenido.Replace("_Jurisprudencia_Doctrina_", defPrincial.FichaEstudio.Count() == 0 ? "" : defPrincial.FichaEstudio.FirstOrDefault().JurisprudenciaDoctrina);
            strContenido = strContenido.Replace("_Decision_Comite_casos_anteriores_Directrices_Conciliacion_", defPrincial.FichaEstudio.Count() == 0 ? "" : defPrincial.FichaEstudio.FirstOrDefault().DecisionComiteDirectrices);
            strContenido = strContenido.Replace("_Analisis_juridico_", defPrincial.FichaEstudio.Count() == 0 ? "" : defPrincial.FichaEstudio.FirstOrDefault().AnalisisJuridico);
            strContenido = strContenido.Replace("_URL_material_probatorio_", defPrincial.FichaEstudio.Count() == 0 ? "" : defPrincial.FichaEstudio.FirstOrDefault().RutaSoporte );
            strContenido = strContenido.Replace("_Recomendaciones_", defPrincial.FichaEstudio.Count() == 0 ? "" : defPrincial.FichaEstudio.FirstOrDefault().Recomendaciones);
            strContenido = strContenido.Replace("_Abogado_elabora_estudio_", defPrincial.FichaEstudio.Count() == 0 ? "" : defPrincial.FichaEstudio.FirstOrDefault().Abogado);

            //Historial de Actuaciones
            /*var controversiaActuaciones = defPrincial.DefensaJudicialSeguimiento.ToList();

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
            */
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
               //|| (fichaEstudio.RecomendacionFinalComite == null)
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.EliminacionExitosa, idAccion, defensaJudicial.UsuarioModificacion, strCrearEditar)

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
            List<GrillaProcesoDefensaJudicial> ListDefensaJudicialGrilla = new List<GrillaProcesoDefensaJudicial>();
            
            List<DefensaJudicial> ListDefensaJudicial  = await _context.DefensaJudicial.Where(r => (bool)r.Eliminado==false).Distinct().Include(x=>x.FichaEstudio).ToListAsync();

            foreach (var defensaJudicial in ListDefensaJudicial)
            {
                try
                {     
                   string TipoAccionNombre = "";
                   

                   Dominio TipoAccion;
                    Dominio EstadoSolicitudCodigoContratoPoliza;

                    TipoAccion = await _commonService.GetDominioByNombreDominioAndTipoDominio(defensaJudicial.TipoAccionCodigo, (int)EnumeratorTipoDominio.Tipo_accion_judicial);
                    if (TipoAccion != null)
                        TipoAccionNombre = TipoAccion.Nombre;

                    bool bRegistroCompleto = false;
                    string strRegistroCompleto = "Incompleto";
                    strRegistroCompleto = (bool)defensaJudicial.EsCompleto ? "Completo" : "Incompleto";
                   

                    
                    GrillaProcesoDefensaJudicial defensaJudicialGrilla = new GrillaProcesoDefensaJudicial
                    {
                        DefensaJudicialId = defensaJudicial.DefensaJudicialId,
                        FechaRegistro = defensaJudicial.FechaCreacion.ToString("dd/MM/yyyy"),
                        LegitimacionPasivaActiva= (bool)defensaJudicial.EsLegitimacionActiva ? "Activa" : "Pasiva",
                        NumeroProceso=defensaJudicial.NumeroProceso,
                        TipoAccionCodigo= defensaJudicial.TipoAccionCodigo,
                        TipoAccion = TipoAccionNombre,
                        EstadoProceso= _commonService.GetDominioByNombreDominioAndTipoDominio(defensaJudicial.EstadoProcesoCodigo, (int)EnumeratorTipoDominio.Estados_Defensa_Judicial).Result.Nombre,
                        EstadoProcesoCodigo =defensaJudicial.EstadoProcesoCodigo,
                        
                        RegistroCompletoNombre =strRegistroCompleto,
                        TipoProceso= _commonService.GetDominioByNombreDominioAndTipoDominio(defensaJudicial.TipoProcesoCodigo, (int)EnumeratorTipoDominio.Procesos_judiciales).Result.Nombre,
                        TipoProcesoCodigo = defensaJudicial.TipoProcesoCodigo,
                        VaAProcesoJudicial= defensaJudicial.FichaEstudio.Count()==0?false:defensaJudicial.FichaEstudio.FirstOrDefault().EsActuacionTramiteComite,
                        FechaCreacion = defensaJudicial.FechaCreacion,
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
            return ListDefensaJudicialGrilla.OrderByDescending(x=>x.FechaCreacion).ToList();

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

        public async Task<List<DefensaJudicialSeguimiento>> GetActuacionesByDefensaJudicialID(int pDefensaJudicialId)
        {
            return _context.DefensaJudicialSeguimiento.Where(x=>x.DefensaJudicialId==pDefensaJudicialId && x.EstadoProcesoCodigo!="3").ToList();//diferente a finalizado
        }

        public async Task<Respuesta> EnviarAComite(int pDefensaJudicialId, string pUsuarioModifico)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_Defensa_Judicial, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;

            DefensaJudicial defensaJudicial = null;

            try
            {
                defensaJudicial = await _context.DefensaJudicial.Where(d => d.DefensaJudicialId == pDefensaJudicialId).FirstOrDefaultAsync();

                if (defensaJudicial != null)
                {
                    strCrearEditar = "ENVIAR A COMITE DEFENSA JUDICIAL";
                    defensaJudicial.FechaModificacion = DateTime.Now;
                    defensaJudicial.UsuarioModificacion = pUsuarioModifico;
                    defensaJudicial.EstadoProcesoCodigo = "2";//cambiar esto                    
                    defensaJudicial.Eliminado = false;
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, pUsuarioModifico, strCrearEditar)

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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, pUsuarioModifico, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> DeleteActuation(int pId, string pUsuarioModifico)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Defensa_Judicial, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;

            DefensaJudicialSeguimiento defensaJudicial = null;

            try
            {
                defensaJudicial = await _context.DefensaJudicialSeguimiento.Where(d => d.DefensaJudicialSeguimientoId == pId).FirstOrDefaultAsync();

                if (defensaJudicial != null)
                {
                    strCrearEditar = "ELIMINAR ACTUACION DE COMITE DEFENSA JUDICIAL";
                    defensaJudicial.FechaModificacion = DateTime.Now;
                    defensaJudicial.UsuarioModificacion = pUsuarioModifico;                    
                    defensaJudicial.Eliminado = true;
                    _context.DefensaJudicialSeguimiento.Update(defensaJudicial);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = defensaJudicial,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.EliminacionExitosa, idAccion, pUsuarioModifico, strCrearEditar)

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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, pUsuarioModifico, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> FinalizeActuation(int pId, string pUsuarioModifico)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Defensa_Judicial, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;

            DefensaJudicialSeguimiento defensaJudicial = null;

            try
            {
                defensaJudicial = await _context.DefensaJudicialSeguimiento.Where(d => d.DefensaJudicialSeguimientoId == pId).FirstOrDefaultAsync();

                if (defensaJudicial != null)
                {
                    strCrearEditar = "FINALIZAR ACTUACION DE COMITE DEFENSA JUDICIAL";
                    defensaJudicial.FechaModificacion = DateTime.Now;
                    defensaJudicial.UsuarioModificacion = pUsuarioModifico;
                    defensaJudicial.Eliminado = false;
                    defensaJudicial.EstadoProcesoCodigo = "3";//cambiar
                    _context.DefensaJudicialSeguimiento.Update(defensaJudicial);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = defensaJudicial,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, pUsuarioModifico, strCrearEditar)

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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, pUsuarioModifico, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> CerrarProceso(int pDefensaJudicialId, string pUsuarioModifico)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_Defensa_Judicial, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;

            DefensaJudicial defensaJudicial = null;

            try
            {
                defensaJudicial = await _context.DefensaJudicial.Where(d => d.DefensaJudicialId == pDefensaJudicialId).FirstOrDefaultAsync();

                if (defensaJudicial != null)
                {
                    strCrearEditar = "CERRAR COMITE DEFENSA JUDICIAL";
                    defensaJudicial.FechaModificacion = DateTime.Now;
                    defensaJudicial.UsuarioModificacion = pUsuarioModifico;
                    defensaJudicial.EstadoProcesoCodigo = "10";//cambiar esto                    
                    defensaJudicial.Eliminado = false;
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, pUsuarioModifico, strCrearEditar)

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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, pUsuarioModifico, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }
    }
}
