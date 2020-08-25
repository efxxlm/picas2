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
using System.IO;
using System.Text;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Globalization;
using asivamosffie.services.Validators;
using asivamosffie.services.Filters;
using System.Data.Common;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class RegisterSessionTechnicalCommitteeService : IRegisterSessionTechnicalCommitteeService
    {
        private readonly ICommonService _commonService;
        private readonly IProjectContractingService _IProjectContractingService;
        private readonly devAsiVamosFFIEContext _context;

        public RegisterSessionTechnicalCommitteeService(devAsiVamosFFIEContext context, ICommonService commonService, IProjectContractingService projectContractingService)
        {
            _IProjectContractingService = projectContractingService;
            _commonService = commonService;
            _context = context;
        }

        public async Task<byte[]> GetPlantillaByTablaIdRegistroId(int pTablaId, int pRegistroId)
        {
            return pTablaId switch
            {
                (int)ConstanCodigoPlantillas.Ficha_De_Contratacion => await ReplacePlantillaFichaContratacion(pRegistroId),
                (int)ConstanCodigoPlantillas.Ficha_De_Procesos_De_Seleccion => await ReplacePlantillaFichaContratacion(pRegistroId),
                _ => Array.Empty<byte>(),
            };
        }

        public async Task<byte[]> ReplacePlantillaFichaContratacion(int pContratacionId)
        {
            Contratacion contratacion = await _IProjectContractingService.GetAllContratacionByContratacionId(pContratacionId);

            string TipoPlantilla = ((int)ConstanCodigoPlantillas.Ficha_De_Contratacion).ToString();

            Plantilla Plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            Plantilla.Contenido = ReemplazarDatosPlantillaContratacion(Plantilla.Contenido, contratacion);
            return PDF.Convertir(Plantilla);

        }

        public string ReemplazarDatosPlantillaContratacion(string pPlantilla, Contratacion pContratacion)
        {
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();

            string TipoPlantillaDetalleProyecto = ((int)ConstanCodigoPlantillas.Detalle_Proyecto).ToString();
            string DetalleProyecto = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaDetalleProyecto).Select(r => r.Contenido).FirstOrDefault();
            string DetallesProyectos = "";

            string TipoPlantillaRegistrosAlcance = ((int)ConstanCodigoPlantillas.Registros_Tabla_Alcance).ToString();
            string RegistroAlcance = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaRegistrosAlcance).Select(r => r.Contenido).FirstOrDefault();

            List<Dominio> ListaParametricas = _context.Dominio.ToList();
            List<Localizacion> ListaLocalizaciones = _context.Localizacion.ToList();
            List<InstitucionEducativaSede> ListaInstitucionEducativaSedes = _context.InstitucionEducativaSede.ToList();
            //Se crea el detalle de los proyectos asociado a contratacion - contratacionProyecto 
            int enumProyecto = 1;
            foreach (var proyecto in pContratacion.ContratacionProyecto)
            {
                //Se crear una nueva plantilla por cada vez que entra
                DetallesProyectos += DetalleProyecto;
                string RegistrosAlcance = "";

                Localizacion Municipio = ListaLocalizaciones.Where(r => r.LocalizacionId == proyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                Localizacion Departamento = ListaLocalizaciones.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                Localizacion Region = ListaLocalizaciones.Where(r => r.LocalizacionId == Departamento.IdPadre).FirstOrDefault();

                InstitucionEducativaSede IntitucionEducativa = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == proyecto.Proyecto.InstitucionEducativaId).FirstOrDefault();
                InstitucionEducativaSede Sede = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == proyecto.Proyecto.SedeId).FirstOrDefault();



                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {

                        case ConstanCodigoVariablesPlaceHolders.NOMBRE_PROYECTO:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, (enumProyecto++).ToString());
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TIPO_DE_INTERVENCION:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, ListaParametricas.Where(r => r.Codigo == proyecto.Proyecto.TipoIntervencionCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).FirstOrDefault().Nombre);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.LLAVE_MEN:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.LlaveMen);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.REGION:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Region.Descripcion);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.DEPARTAMENTO:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Departamento.Descripcion);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.MUNICIPIO:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Municipio.Descripcion);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.INSTITUCION_EDUCATIVA:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, IntitucionEducativa.Nombre);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.CODIGO_DANE_IE:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, IntitucionEducativa.CodigoDane.ToString());
                            break;

                        case ConstanCodigoVariablesPlaceHolders.SEDE:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Sede.Nombre);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.CODIGO_DANE_SEDE:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Sede.CodigoDane.ToString());
                            break;

                        case ConstanCodigoVariablesPlaceHolders.REGISTROS_ALCANCE:
                            //Predio Principal

                            //List<Predio> ListPredios = proyecto.Proyecto.ProyectoPredio.Select(r => r.Predio).ToList();
                            //ListPredios.Add(proyecto.Proyecto.PredioPrincipal); 
                            //var PrediosOrdenadosPorTipoPredio = ListPredios.GroupBy(x => x.TipoPredioCodigo)
                            //           .Select(x => new {
                            //               Espacio = x.Key,
                            //               Cantidad = x.Count()});

                            foreach (var infraestructura in proyecto.Proyecto.InfraestructuraIntervenirProyecto)
                            {
                                RegistrosAlcance += RegistroAlcance;
                                //TODO :GROUP BY espacios Intervenir
                                RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_ESPACIOS_A_INTERVENIR]", ListaParametricas.Where(r => r.Codigo == infraestructura.InfraestructuraCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Espacios_Intervenir).FirstOrDefault().Nombre);
                                RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_CANTIDAD]", infraestructura.Cantidad.ToString());
                            }
                         
                            //Dictionary<string, int> DictionaryRegistrosAlcance = new Dictionary<string, int>();

                            //foreach (var ListRegistrosAlcance in proyecto.Proyecto.ProyectoPredio.GroupBy(predio => predio.Predio.TipoPredioCodigo)
                            //       .Select(group => new
                            //       {
                            //           Espacio = group.Key,
                            //           Cantidad = group.Count()
                            //       })
                            //       .OrderBy(x => x.Cantidad)) ;
                            //          DictionaryRegistrosAlcance.Add(ListRegistrosAlcance.Espacio, ListRegistrosAlcance.Cantidad);

                            //Agregar el predio principal a los otros predios relacionados con el proyecto 
                            //RegistrosAlcance += RegistroAlcance;
                            ////TODO :GROUP BY espacios Intervenir
                            //RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_ESPACIOS_A_INTERVENIR]", ListaParametricas.Where(r => r.Codigo == proyecto.Proyecto.PredioPrincipal.TipoPredioCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Espacios_Intervenir).FirstOrDefault().Nombre);
                            //RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_CANTIDAD]", "1");

                            //// Lista Predios 
                            //foreach (var predio in proyecto.Proyecto.ProyectoPredio)
                            //{
                            //    RegistrosAlcance += RegistroAlcance;
                            //    //TODO :GROUP BY espacios Intervenir
                            //    RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_ESPACIOS_A_INTERVENIR]", ListaParametricas.Where(r => r.Codigo == predio.Predio.TipoPredioCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Espacios_Intervenir).FirstOrDefault().Nombre);
                            //    RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_CANTIDAD]", "1");

                            //}
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, RegistrosAlcance);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.PLAZO_OBRA_MESES:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.InfraestructuraIntervenirProyecto.Sum(r=> r.PlazoMesesObra).ToString());
                            break;

                        case ConstanCodigoVariablesPlaceHolders.PLAZO_OBRA_DIAS:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.InfraestructuraIntervenirProyecto.Sum(r => r.PlazoDiasObra).ToString());
                            break;
                             
                        case ConstanCodigoVariablesPlaceHolders.VALOR_OBRA:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, "$" + String.Format("{0:n0}", proyecto.Proyecto.ValorObra));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.VALOR_INTERVENTORIA:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, "$" + String.Format("{0:n0}", proyecto.Proyecto.ValorInterventoria));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.VALOR_TOTAL_PROYECTO:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, "$" + String.Format("{0:n0}", proyecto.Proyecto.ValorTotal));
                            break;


                    }
                }
            }


            foreach (Dominio placeholderDominio in placeholders)
            {
                //ConstanCodigoVariablesPlaceHolders placeholder = (ConstanCodigoVariablesPlaceHolders)placeholderDominio.Codigo.ToString();

                switch (placeholderDominio.Codigo)
                {
                    case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.NumeroSolicitud);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.FechaSolicitud != null ? ((DateTime)pContratacion.FechaSolicitud).ToString("yyyy-MM-dd") : " ");
                        break;

                    case ConstanCodigoVariablesPlaceHolders.OPCION_POR_CONTRATAR:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, ListaParametricas.Where(r=> r.Codigo == pContratacion.TipoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud_Obra_Interventorias).FirstOrDefault().Nombre);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.VALOR_TOTAL_DE_LA_SOLICITUD:
                        decimal? ValorTotal = pContratacion.ContratacionProyecto.Sum(r => r.Proyecto.ValorTotal);
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, "$" + String.Format("{0:n0}", ValorTotal));
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CANTIDAD_DE_PROYECTOS_ASOCIADOS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.ContratacionProyecto.Count().ToString());
                        break;
                    //Datos Contratista 
                    case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NOMBRE:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.Contratista.Nombre);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NUMERO_IDENTIFICACION:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.Contratista.NumeroIdentificacion);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NOMBRE_RE_LEGAL:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.Contratista.RepresentanteLegal);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NUMERO_IDENTIFICACION_RE_LEGAL:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, "?????????????");
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NUMERO_INVITACION:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.Contratista.NumeroInvitacion);
                        break;

                    //
                    case ConstanCodigoVariablesPlaceHolders.DETALLES_PROYECTOS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, DetallesProyectos);
                        break;
                }
            }


            return pPlantilla;
        }


        public static bool ValidarCamposSesionComiteTema(SesionComiteTema pSesionComiteTema)
        {
            if (
                !string.IsNullOrEmpty(pSesionComiteTema.ResponsableCodigo) ||
                !string.IsNullOrEmpty(pSesionComiteTema.TiempoIntervencion.ToString()) ||
                !string.IsNullOrEmpty(pSesionComiteTema.RutaSoporte) ||
                !string.IsNullOrEmpty(pSesionComiteTema.Observaciones) ||
                !string.IsNullOrEmpty(pSesionComiteTema.EsAprobado.ToString()) ||
                !string.IsNullOrEmpty(pSesionComiteTema.ObservacionesDecision) ||
                !string.IsNullOrEmpty(pSesionComiteTema.ObservacionesDecision)
                ) { return false; }

            return true;
        }

        public async Task<List<dynamic>> GetListSesionComiteTemaByIdSesion(int pIdSesion)
        {

            var ListSesionComiteTema = await _context.SesionComiteTema.Where(r => r.SesionId == pIdSesion && !(bool)r.Eliminado).ToListAsync();

            List<dynamic> ListSesionComiteTemaDyn = new List<dynamic>();

            foreach (var sesionComiteTema in ListSesionComiteTema)
            {
                ListSesionComiteTemaDyn.Add(
                                            new
                                            {
                                                SesionTemaId = sesionComiteTema.SesionTemaId,
                                                ResponsableCodigo = sesionComiteTema.ResponsableCodigo,
                                                TiempoIntervencion = sesionComiteTema.TiempoIntervencion,
                                                Tema = sesionComiteTema.Tema
                                            });
            }
            return ListSesionComiteTemaDyn;
        }

        public async Task<Respuesta> RegistrarParticipantesSesion(Sesion psesion)
        {
            try
            {
                int idAccionRegistrarParticipantesSesion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Participantes_Sesion, (int)EnumeratorTipoDominio.Acciones);

                try
                {
                    foreach (var SesionInvitado in psesion.SesionInvitado)
                    {
                        SesionInvitado.Eliminado = false;
                        SesionInvitado.FechaCreacion = DateTime.Now;
                        SesionInvitado.UsuarioCreacion = psesion.UsuarioCreacion;
                    }

                    foreach (var SesionUsuario in psesion.SesionUsuario)
                    {
                        SesionUsuario.Eliminado = false;
                        SesionUsuario.FechaCreacion = DateTime.Now;
                        SesionUsuario.UsuarioCreacion = psesion.UsuarioCreacion;
                    }
                    _context.Sesion.Add(psesion);
                    _context.SaveChanges();



                    return
                         new Respuesta
                         {
                             IsSuccessful = true,
                             IsException = false,
                             IsValidation = false,
                             Code = ConstantSesionComiteTecnico.OperacionExitosa,
                             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionRegistrarParticipantesSesion, psesion.UsuarioCreacion, "REGISTRAR PARTICIPANTES DE LA SESIÓN")
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
                               Code = ConstantSesionComiteTecnico.Error,
                               Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionRegistrarParticipantesSesion, psesion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                           };
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task<List<ComiteGrilla>> GetComiteGrilla()
        {
            List<ComiteGrilla> ListComiteGrilla = new List<ComiteGrilla>();

            try
            {
                var ListComiteTecnico = await _context.Sesion.Where(r => !(bool)r.Eliminado).Select(x => new
                {
                    Id = x.SesionId,
                    FechaComite = x.FechaCreacion.ToString(),
                    EstadoComite = x.EstadoComiteCodigo,
                    x.NumeroComite
                }).Distinct().OrderByDescending(r => r.Id).ToListAsync();

                foreach (var comite in ListComiteTecnico)
                {
                    ComiteGrilla comiteGrilla = new ComiteGrilla
                    {
                        Id = comite.Id,
                        FechaComite = comite.FechaComite,
                        EstadoComiteCodigo = comite.EstadoComite,
                        EstadoComite = !string.IsNullOrEmpty(comite.EstadoComite) ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(comite.EstadoComite, (int)EnumeratorTipoDominio.Estado_Comite) : "---",
                        NumeroComite = comite.NumeroComite
                    };

                    ListComiteGrilla.Add(comiteGrilla);

                }
            }
            catch (Exception)
            {
            }
            return ListComiteGrilla;
        }

        public async Task<Respuesta> SaveEditSesionComiteTema(Sesion session)
        {
            try
            {
                int idAccionCrearEditarSesionComiteTema = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_SesionComiteTema, (int)EnumeratorTipoDominio.Acciones);

                try
                {
                    string EditarOcrear = "";

                    if (session.SesionId == 0)
                    {
                        //Por defecto, el sistema incluirá automáticamente en cada orden del día un último tema
                        //sin responsable ni tiempo, este se denominará “Proposiciones y varios”.
                        session.SesionComiteTema.Add(new SesionComiteTema
                        {
                            // se pone nombre del tema solo para diferenciar
                            Tema = "Proposiciones Varios  esto se debe eliminar"
                        });

                        //SESION 
                        //Auditoria
                        session.FechaCreacion = DateTime.Now;
                        session.Eliminado = false;
                        //Registro
                        session.NumeroComite = await _commonService.EnumeradorComite();
                        session.EstadoComiteCodigo = ConstanCodigoEstadoComite.Sin_Convocatoria;
                        session.EsCompleto = false;
                        _context.Sesion.Add(session);
                    }
                    else
                    {
                        Sesion sessionOld = _context.Sesion.Find(session.SesionId);

                        sessionOld.UsuarioModificacion = session.UsuarioCreacion;
                        sessionOld.FechaModificacion = session.FechaModificacion;

                        sessionOld.RutaActaSesion = session.RutaActaSesion;
                        sessionOld.RutaActaSesion = session.RutaActaSesion;
                    }

                    foreach (var SesionComiteTema in session.SesionComiteTema)
                    {
                        if (SesionComiteTema.SesionTemaId == 0)
                        {
                            EditarOcrear = "CREAR SESIÓN COMITE TECNICO TEMA";
                            SesionComiteTema.Eliminado = false;
                            SesionComiteTema.FechaCreacion = DateTime.Now;
                            SesionComiteTema.UsuarioCreacion = session.UsuarioCreacion;

                        }
                        else
                        {
                            EditarOcrear = "EDITAR SESIÓN COMITE TECNICO TEMA";
                            SesionComiteTema sesionComiteTemaOld = _context.SesionComiteTema.Find(SesionComiteTema.SesionTemaId);
                            sesionComiteTemaOld.FechaModificacion = DateTime.Now;
                            sesionComiteTemaOld.UsuarioModificacion = session.UsuarioCreacion;

                            sesionComiteTemaOld.Tema = SesionComiteTema.Tema;
                            sesionComiteTemaOld.ResponsableCodigo = SesionComiteTema.ResponsableCodigo;
                            sesionComiteTemaOld.TiempoIntervencion = SesionComiteTema.TiempoIntervencion;
                            sesionComiteTemaOld.RutaSoporte = SesionComiteTema.RutaSoporte;
                            sesionComiteTemaOld.Observaciones = SesionComiteTema.Observaciones;
                            sesionComiteTemaOld.EsAprobado = SesionComiteTema.EsAprobado;
                        }
                    }


                    _context.SaveChanges();

                    return
                         new Respuesta
                         {
                             IsSuccessful = true,
                             IsException = false,
                             IsValidation = false,
                             Code = ConstantSesionComiteTecnico.OperacionExitosa,
                             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionCrearEditarSesionComiteTema, session.UsuarioCreacion, EditarOcrear)
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
                               Code = ConstantSesionComiteTecnico.Error,
                               Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionCrearEditarSesionComiteTema, session.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                           };
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<dynamic>> GetListSolicitudesContractuales(DateTime FechaComite)
        {
            List<dynamic> ListValidacionSolicitudesContractualesGrilla = new List<dynamic>();

            int CantidadDiasComite = Int32.Parse(_context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Dias_Comite && (bool)r.Activo).FirstOrDefault().Descripcion);
            /*Procesos de Seleccion Estado Apertura tramite  , Contratación Estado En tramite 
            “Apertura de proceso de selección”, 
            “Evaluación de proceso de selección”,
            “Contratación”,
            “Modificación contractual por novedad”, 
            “Controversia contractual”,
             “Procesos de defensa judicial”. */


            FechaComite = FechaComite.AddDays(-CantidadDiasComite);

            List<ProcesoSeleccion> ListProcesoSeleccion =
                _context.ProcesoSeleccion
                .Where(r => !(bool)r.Eliminado
                 && r.EstadoProcesoSeleccionCodigo == ConstanCodigoEstadoProcesoSeleccion.Apertura_En_Tramite
                 && r.FechaModificacion < FechaComite
                 )
                .OrderByDescending(r => r.ProcesoSeleccionId).ToList();

            List<Contratacion> ListContratacion = _context.Contratacion
                .Where(r => !(bool)r.Eliminado
                && r.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.En_tramite
                && r.FechaSolicitud < FechaComite
                )
                .OrderByDescending(r => r.ContratacionId).ToList();
            try
            {
                List<Dominio> ListTipoSolicitud = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud).ToList();

                foreach (var ProcesoSeleccion in ListProcesoSeleccion)
                {
                    ListValidacionSolicitudesContractualesGrilla.Add(new
                    {
                        Id = ProcesoSeleccion.ProcesoSeleccionId,
                        FechaSolicitud = ProcesoSeleccion.FechaCreacion.ToString("yyyy-MM-dd"),
                        NumeroSolicitud = ProcesoSeleccion.NumeroProceso,
                        TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion).FirstOrDefault().Nombre,
                        tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion
                    });
                };

                foreach (var Contratacion in ListContratacion)
                {
                    ListValidacionSolicitudesContractualesGrilla.Add(new
                    {
                        Id = Contratacion.ContratacionId,
                        FechaSolicitud = Contratacion.FechaSolicitud != null ? Convert.ToDateTime(Contratacion.FechaSolicitud).ToString("yyyy-MM-dd") : Contratacion.FechaSolicitud.ToString(),
                        Contratacion.NumeroSolicitud,
                        TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Contratacion).FirstOrDefault().Nombre,
                        tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Contratacion
                    });
                };

            }
            catch (Exception ex)
            {

            }

            return ListValidacionSolicitudesContractualesGrilla;
        }

        public async Task<Sesion> GetSesionBySesionId(int pSesionId)
        {
            return await _context.Sesion.Where(r => r.SesionId == pSesionId && !(bool)r.Eliminado).IncludeFilter(r => r.SesionComiteTema.Where(r => !(bool)r.Eliminado)).FirstOrDefaultAsync();
        }

        public async Task<Respuesta> EliminarSesionComiteTema(int pSesionComiteTemaId, string pUsuarioModificacion)
        {
            int idAccionEliminarSesionComiteTema = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Sesion_Comite_Tema, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SesionComiteTema sesionComiteTemaOld = await _context.SesionComiteTema.Where(r => r.SesionTemaId == pSesionComiteTemaId).FirstOrDefaultAsync();
                sesionComiteTemaOld.Eliminado = true;
                sesionComiteTemaOld.FechaModificacion = DateTime.Now;
                sesionComiteTemaOld.UsuarioCreacion = pUsuarioModificacion;
                _context.Update(sesionComiteTemaOld);
                _context.SaveChanges();

                return
                 new Respuesta
                 {
                     IsSuccessful = true,
                     IsException = false,
                     IsValidation = false,
                     Code = ConstantSesionComiteTecnico.OperacionExitosa,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionEliminarSesionComiteTema, pUsuarioModificacion, "ELIMINAR SESIÓN COMITE TEMA")
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
                        Code = ConstantSesionComiteTecnico.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionEliminarSesionComiteTema, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                    };
            }

        }

        public async Task<Respuesta> CambiarEstadoComite(Sesion pSesion)
        {
            int idAccionCambiarEstadoSesion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Comite_Sesion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Sesion sesionOld = _context.Sesion.Find(pSesion.SesionId);

                string NombreEstado = await _commonService.GetNombreDominioByCodigoAndTipoDominio(pSesion.EstadoComiteCodigo, (int)EnumeratorTipoDominio.Estado_Comite);
                sesionOld.UsuarioModificacion = pSesion.UsuarioCreacion;
                sesionOld.FechaModificacion = DateTime.Now;
                sesionOld.EstadoComiteCodigo = pSesion.EstadoComiteCodigo;

                _context.Update(sesionOld);
                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionCambiarEstadoSesion, pSesion.UsuarioCreacion, "ESTADO COMITE CAMBIADO A " + NombreEstado.ToUpper())
                    };
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<Respuesta> GuardarInvitado(Sesion pSesion)
        {
            int idAccionCambiarEstadoSesion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Comite_Sesion, (int)EnumeratorTipoDominio.Acciones);

            try
            {

                Sesion sesion = _context.Sesion.Where(r => (bool)r.Eliminado).IncludeFilter(r => r.SesionInvitado).FirstOrDefault();
                Sesion sesionOld = _context.Sesion.Find(pSesion.SesionId);

                string NombreEstado = await _commonService.GetNombreDominioByCodigoAndTipoDominio(pSesion.EstadoComiteCodigo, (int)EnumeratorTipoDominio.Estado_Comite);
                sesionOld.UsuarioModificacion = pSesion.UsuarioCreacion;
                sesionOld.FechaModificacion = DateTime.Now;
                sesionOld.EstadoComiteCodigo = pSesion.EstadoComiteCodigo;

                _context.Update(sesionOld);
                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionCambiarEstadoSesion, pSesion.UsuarioCreacion, "ESTADO COMITE CAMBIADO A " + NombreEstado.ToUpper())
                    };
            }
            catch (Exception)
            {

                throw;
            }

        }

        //public bool EjemploTransaction()
        //{
        //    using (DbContextTransaction transaction = (DbContextTransaction)_context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var standard = _context.ArchivoCargue.Add(new ArchivoCargue() { Activo = true });

        //            _context.Usuario.Add(new Usuario()
        //            {
        //                NombreMaquina = "Rama",
        //                Nombres = "Julian"
        //            });
        //            _context.SaveChanges();
        //            // throw exectiopn to test roll back transaction

        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            Console.WriteLine("Error occurred.");
        //        }
        //    }
        //    return false;
        //}
    }
}
