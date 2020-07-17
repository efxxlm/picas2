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

namespace asivamosffie.services
{
    public class ProjectService : IProjectService
    {
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        private readonly devAsiVamosFFIEContext _context;

        public ProjectService(devAsiVamosFFIEContext context, ICommonService commonService, IDocumentService documentService)
        {
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
        }

        public bool ValidarRegistroCompleto(Proyecto proyecto)
        {

            if (
                !string.IsNullOrEmpty(proyecto.FechaSesionJunta.ToString())
                || !string.IsNullOrEmpty(proyecto.NumeroActaJunta.ToString())
                || !string.IsNullOrEmpty(proyecto.TipoIntervencionCodigo.ToString())
                || !string.IsNullOrEmpty(proyecto.LlaveMen.ToString())
                || !string.IsNullOrEmpty(proyecto.LocalizacionIdMunicipio.ToString())
                || !string.IsNullOrEmpty(proyecto.InstitucionEducativaId.ToString())
                || !string.IsNullOrEmpty(proyecto.SedeId.ToString())
                || !string.IsNullOrEmpty(proyecto.EnConvocatoria.ToString())
                || !string.IsNullOrEmpty(proyecto.ConvocatoriaId.ToString())
                ) { }

            return false;

        }

        public async Task<List<ProyectoGrilla>> ListProyectos(string pUsuarioConsulto)
        {
            List<ProyectoGrilla> ListProyectoGrilla = new List<ProyectoGrilla>();

            try
            {
                List<Proyecto> ListProyectos = await _context.Proyecto.Where(r => !(bool)r.Eliminado).Include(r => r.InstitucionEducativa).Include(r => r.ProyectoPredio).Distinct().ToListAsync();

                foreach (var proyecto in ListProyectos)
                {
                    Localizacion municipio = await _commonService.GetLocalizacionByLocalizacionId(proyecto.LocalizacionIdMunicipio);
                    Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio estadoRegistro = await _commonService.GetDominioByNombreDominioAndTipoDominio(proyecto.EstadoProyectoCodigo, (int)EnumeratorTipoDominio.Estado_Registro);
                    Dominio EstadoJuridicoPredios = await _commonService.GetDominioByNombreDominioAndTipoDominio(proyecto.EstadoJuridicoCodigo, (int)EnumeratorTipoDominio.Estado_Juridico_Predios);

                    ProyectoGrilla proyectoGrilla = new ProyectoGrilla
                    {
                        ProyectoId = proyecto.ProyectoId,
                        Departamento = departamento.Descripcion,
                        Municipio = municipio.Descripcion,
                        InstitucionEducativa = _context.InstitucionEducativaSede.Find(proyecto.InstitucionEducativaId).Nombre,
                        Sede = _context.InstitucionEducativaSede.Find(proyecto.SedeId).Nombre,
                        EstadoRegistro = estadoRegistro.Nombre,
                        EstadoJuridicoPredios = EstadoJuridicoPredios.Nombre,
                        Fecha = proyecto.FechaCreacion == null ? Convert.ToDateTime(proyecto.FechaCreacion).ToString("yyyy-MM-dd") : ""
                    };
                    ListProyectoGrilla.Add(proyectoGrilla);
                }

                return ListProyectoGrilla;


            }
            catch (Exception ex)
            {
                return ListProyectoGrilla;

            }


        }

        public async Task<Respuesta> CreateOrEditProyect(Proyecto pProyecto)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                // Crear o editar el predio principal 

                //Predio  principal 
                //el principal se supone que no va relacionado con la tabla ProyectoPredio 
                //Ya que tiene una relacion directa en la tabla proyecto.predioPrincipalId
                if (pProyecto.PredioPrincipal.PredioId == null || pProyecto.PredioPrincipal.PredioId == 0)
                {
                    pProyecto.PredioPrincipal.InstitucionEducativaSedeId = null;
                    pProyecto.PredioPrincipal.UsuarioCreacion = pProyecto.UsuarioCreacion;
                    pProyecto.PredioPrincipal.FechaCreacion = DateTime.Now;
                    pProyecto.PredioPrincipal.Activo = true;
                    _context.Predio.Add(pProyecto.PredioPrincipal);
                }
                else
                {
                    Predio predio = _context.Predio.Find(pProyecto.PredioPrincipal.PredioId);
                    //predio.InstitucionEducativaSedeId = pProyecto.PredioPrincipal.InstitucionEducativaSedeId;
                    predio.TipoPredioCodigo = pProyecto.PredioPrincipal.TipoPredioCodigo;
                    predio.UbicacionLatitud = pProyecto.PredioPrincipal.UbicacionLatitud;
                    predio.UbicacionLongitud = pProyecto.PredioPrincipal.UbicacionLongitud;
                    predio.Direccion = pProyecto.PredioPrincipal.Direccion;
                    predio.DocumentoAcreditacionCodigo = pProyecto.PredioPrincipal.DocumentoAcreditacionCodigo;
                    predio.NumeroDocumento = pProyecto.PredioPrincipal.NumeroDocumento;
                    predio.CedulaCatastral = pProyecto.PredioPrincipal.CedulaCatastral;
                    _context.Update(predio);
                }
                _context.SaveChanges();



                //infraestructura
                foreach (var infraestructura in pProyecto.InfraestructuraIntervenirProyecto)
                {
                    if (infraestructura.InfraestrucutraIntervenirProyectoId == 0 || infraestructura.InfraestrucutraIntervenirProyectoId == null)
                    {
                        infraestructura.FechaCreacion = DateTime.Now;
                    }
                }

                //Los otros predios  
                foreach (var predio in pProyecto.ProyectoPredio)
                {
                    if (predio.Predio.PredioId == null)
                    {
                        predio.Predio.UsuarioCreacion = pProyecto.UsuarioCreacion;
                        predio.Predio.UsuarioCreacion = pProyecto.UsuarioCreacion;
                        predio.Predio.FechaCreacion = DateTime.Now;
                        predio.Predio.Activo = true;
                        _context.Predio.Add(predio.Predio);
                        _context.SaveChanges();

                        //Relacion Proyecto Predio 
                        ProyectoPredio proyectoPredio = new ProyectoPredio();
                        proyectoPredio.Activo = true;
                        proyectoPredio.UsuarioCreacion = pProyecto.UsuarioCreacion;
                        proyectoPredio.FechaCreacion = DateTime.Now;
                        proyectoPredio.PredioId = predio.Predio.PredioId;
                        proyectoPredio.ProyectoId = pProyecto.ProyectoId;
                        //Definir que poner
                        proyectoPredio.EstadoJuridicoCodigo = " ";
                        _context.ProyectoPredio.Add(proyectoPredio);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Predio predioedit = _context.Predio.Find(predio.Predio.PredioId);
                        predioedit.InstitucionEducativaSedeId = predio.Predio.InstitucionEducativaSedeId;
                        predioedit.TipoPredioCodigo = predio.Predio.TipoPredioCodigo;
                        predioedit.UbicacionLatitud = predio.Predio.UbicacionLatitud;
                        predioedit.UbicacionLongitud = predio.Predio.UbicacionLongitud;
                        predioedit.Direccion = predio.Predio.Direccion;
                        predioedit.DocumentoAcreditacionCodigo = predio.Predio.DocumentoAcreditacionCodigo;
                        predioedit.NumeroDocumento = predio.Predio.NumeroDocumento;
                        predioedit.CedulaCatastral = predio.Predio.CedulaCatastral;

                        //Relacion Proyecto Predio 
                        //si envia el predio . activo como false elimina logicamente tambien la relacion  proyectoPredio 
                        ProyectoPredio proyectoPredio = _context.ProyectoPredio.Where(r => r.ProyectoId == pProyecto.ProyectoId && r.PredioId == predio.PredioId).FirstOrDefault();
                        proyectoPredio.Activo = predio.Activo;
                    }
                }

                //Aportantes
                foreach (var aportante in pProyecto.ProyectoAportante)
                {
                    aportante.Aportante = null;
                    aportante.CofinanciacionDocumento = null;
                    if (aportante.ProyectoAportanteId == null || aportante.ProyectoAportanteId == 0)
                    {
                        //Definir como llega vigencia de cofinanciacion para relacionarlo con cofinanciacionAportante
                        //Relacion cofinanciacion aportante 

                        // aportante.Aportante.Cofinanciacion.
                        //cofinanciacionAportante 
                        /*CofinanciacionAportante cofinanciacionAportante = new CofinanciacionAportante();
                        cofinanciacionAportante.UsuarioCreacion = pProyecto.UsuarioCreacion;
                        cofinanciacionAportante.Eliminado = false;
                        cofinanciacionAportante.FechaCreacion = DateTime.Now;
                        */
                        aportante.Eliminado = false;
                        aportante.ProyectoId = pProyecto.ProyectoId;
                        aportante.FechaCreacion = DateTime.Now;
                        aportante.UsuarioCreacion = pProyecto.UsuarioCreacion;
                        _context.ProyectoAportante.Add(aportante);
                    }

                }
                //Proyecto
                if (pProyecto.ProyectoId == null || pProyecto.ProyectoId == 0)
                {
                    //El proyecto es nuevo  
                    pProyecto.InstitucionEducativa = _context.InstitucionEducativaSede.Find(pProyecto.InstitucionEducativaId);
                    pProyecto.Sede = _context.InstitucionEducativaSede.Find(pProyecto.SedeId);
                    pProyecto.ProyectoAportante = null;


                    //TODO:
                    //Validar SI el proyecto viene completo o como es?
                    pProyecto.EstadoProyectoCodigo = ConstantCodigoEstadoProyecto.Completo;
                    //pProyecto.EstadoProyectoCodigo = ConstantCodigoEstadoProyecto.Incompleto;


                    //si el tipo de intervancion es nuevo el estado juridico es sin revision 
                    if (pProyecto.TipoIntervencionCodigo.Equals(ConstantCodigoTipoIntervencion.Nuevo))
                    {
                        pProyecto.EstadoJuridicoCodigo = ConstantCodigoEstadoJuridico.Sin_Revision;
                    }
                    else
                    {
                        pProyecto.EstadoJuridicoCodigo = ConstantCodigoEstadoJuridico.Aprobado;
                    }
                    //pProyecto.InfraestructuraIntervenirProyecto = null;
                    pProyecto.Eliminado = false;
                    pProyecto.FechaCreacion = DateTime.Now;
                    _context.Proyecto.Add(pProyecto);
                }
                else
                {
                    Proyecto proyectoAntiguo = _context.Proyecto.Find(pProyecto.ProyectoId);
                    proyectoAntiguo.FechaModificacion = DateTime.Now;
                    proyectoAntiguo.UsuarioModificacion = pProyecto.UsuarioCreacion;

                    proyectoAntiguo.FechaSesionJunta = pProyecto.FechaSesionJunta;
                    proyectoAntiguo.NumeroActaJunta = pProyecto.NumeroActaJunta;
                    proyectoAntiguo.TipoIntervencionCodigo = pProyecto.TipoIntervencionCodigo;
                    proyectoAntiguo.LlaveMen = pProyecto.LlaveMen;
                    proyectoAntiguo.LocalizacionIdMunicipio = pProyecto.LocalizacionIdMunicipio;
                    proyectoAntiguo.InstitucionEducativaId = pProyecto.InstitucionEducativaId;
                    proyectoAntiguo.Sede = pProyecto.Sede;
                    proyectoAntiguo.EnConvocatoria = pProyecto.EnConvocatoria;
                    proyectoAntiguo.ConvocatoriaId = pProyecto.ConvocatoriaId;
                    proyectoAntiguo.CantPrediosPostulados = pProyecto.CantPrediosPostulados;
                    proyectoAntiguo.TipoPredioCodigo = pProyecto.TipoPredioCodigo;
                    _context.Update(proyectoAntiguo);
                }

                _context.SaveChanges();

                return respuesta =
                new Respuesta
                {
                    IsSuccessful = false,
                    IsException = false,
                    IsValidation = true,
                    Data = pProyecto,
                    Code = ConstantMessagesProyecto.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearProyecto, pProyecto.UsuarioCreacion, " ")
                };

            }
            catch (Exception ex)
            {
                return respuesta =
                   new Respuesta
                   {
                       IsSuccessful = false,
                       IsException = false,
                       IsValidation = true,
                       Code = ConstantMessagesProyecto.Error,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.Error, idAccionCrearProyecto, pProyecto.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }

        public async Task<Respuesta> SetValidateCargueMasivo(IFormFile pFile, string pFilePatch, string pUsuarioCreo)
        {
            int CantidadRegistrosVacios = 0;
            int CantidadResgistrosValidos = 0;
            int CantidadRegistrosInvalidos = 0;
            Respuesta respuesta = new Respuesta();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.Proyecto, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);

            ArchivoCargue archivoCarge = await _documentService.getSaveFile(pFile, pFilePatch, Int32.Parse(OrigenArchivoCargue.Proyecto));

            // if (!string.IsNullOrEmpty(archivoCarge.ArchivoCargueId.ToString()))
            if (archivoCarge != null)
            {
                using (var stream = new MemoryStream())
                {
                    await pFile.CopyToAsync(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        //Controlar Registros
                        //Filas <=
                        //No comienza desde 0 por lo tanto el = no es necesario
                        for (int i = 2; i < worksheet.Dimension.Rows; i++)
                        {
                            try
                            {
                                /* Columnas Obligatorias de excel
                                 2	3	4	5	6	7	8	10	11	12	13	14 28 29 30 31 32		
                                Campos Obligatorios Validos   */
                                if (
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 2].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 3].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 4].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 5].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 6].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 7].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 8].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 10].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 12].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 13].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 14].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 28].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 29].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 30].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 31].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 32].Text)
                                    )
                                {

                                    TemporalProyecto temporalProyecto = new TemporalProyecto();
                                    //Auditoria
                                    temporalProyecto.ArchivoCargueId = archivoCarge.ArchivoCargueId;
                                    temporalProyecto.EstaValidado = false;
                                    temporalProyecto.FechaCreacion = DateTime.Now;
                                    temporalProyecto.UsuarioCreacion = pUsuarioCreo;

                                    // #1
                                    //Fecha sesion Junta 
                                    string strDateTime = (worksheet.Cells[i, 1].Text).ToString();
                                    if (!string.IsNullOrEmpty(strDateTime))
                                    {
                                        temporalProyecto.FechaSesionJunta = DateTime.Parse(strDateTime);
                                    }

                                    //#2
                                    //Número de acta de la junta 
                                    int intNumeroActaJunta = Int32.Parse(worksheet.Cells[i, 2].Text);
                                    temporalProyecto.NumeroActaJunta = intNumeroActaJunta;

                                    //#3
                                    // Tipo de Intervención 
                                    temporalProyecto.TipoIntervencionId = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 3].Text, (int)EnumeratorTipoDominio.Tipo_de_Intervencion));

                                    //#4
                                    // Llave MEN  
                                    temporalProyecto.LlaveMen = worksheet.Cells[i, 4].Text;

                                    //#5
                                    //Región

                                    //#6
                                    //Departamento
                                    temporalProyecto.Departamento = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 6].Text, "0");

                                    //#7
                                    //Municipio ///aqui debe recibir el parametro iddepartamento, pueden haber municipios del mismo nombre para diferente departamento
                                    temporalProyecto.Municipio = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 7].Text, temporalProyecto.Departamento.ToString());


                                    //#8
                                    //Institución Educativa 
                                    //Validar si existe institucion educativa y guardar id el codigo dane es mejor identificador de id, es único, así el Excel es menos complejo con la lista
                                    int idInstitucionEducativaSede = await _commonService.getInstitucionEducativaIdByCodigoDane(Int32.Parse(worksheet.Cells[i, 9].Text));
                                    if (idInstitucionEducativaSede > 0)
                                    {
                                        temporalProyecto.InstitucionEducativaId = idInstitucionEducativaSede;
                                    }
                                    else
                                    {
                                        archivoCarge.CantidadRegistrosInvalidos++;
                                        break;
                                    }

                                    //#9
                                    //Código DANE IE 
                                    //     temporalProyecto.CodigoDaneIe = Int32.TryParse(worksheet.Cells[i, 9].Text, out 1);

                                    //#10
                                    //Código DANE IE 
                                    //Validar si existe la sede y poner id si no crear sede y poner id  el codigo dane es mejor identificador de id, es único, así el Excel es menos complejo con la lista
                                    int SedeId = await _commonService.getInstitucionEducativaIdByCodigoDane(Int32.Parse(worksheet.Cells[i, 11].Text));
                                    if (SedeId > 0)
                                    { temporalProyecto.SedeId = SedeId; }
                                    else
                                    {
                                        archivoCarge.CantidadRegistrosInvalidos++;
                                        break;
                                    }

                                    //#11
                                    //Código DANE SEDE 
                                    //          temporalProyecto.CodigoDaneSede = Int32.Parse(worksheet.Cells[i, 11].Text);

                                    //#12
                                    //¿Se encuentra dentro de una convocatoria? 
                                    if ((worksheet.Cells[i, 12].Text).ToString().ToUpper().Contains("SI") || Int32.Parse(worksheet.Cells[i, 12].Text).ToString().ToUpper().Contains("VERDADERO"))
                                    { temporalProyecto.EnConvotatoria = true; }
                                    else { temporalProyecto.EnConvotatoria = false; };

                                    //#13
                                    //Convocatoria
                                    temporalProyecto.ConvocatoriaId = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 13].Text, (int)EnumeratorTipoDominio.Convocatoria));

                                    //#14
                                    //Número de predios postulados
                                    temporalProyecto.CantPrediosPostulados = Int32.Parse(worksheet.Cells[i, 14].Text);

                                    //#15
                                    //Tipo de predio(s) 
                                    temporalProyecto.TipoPredioId = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 15].Text, (int)EnumeratorTipoDominio.Tipo_de_Predios));

                                    //#16
                                    //Ubicación del predio principal latitud
                                    temporalProyecto.UbicacionPredioPrincipalLatitud = worksheet.Cells[i, 16].Text;

                                    //#17
                                    //Ubicación del predio principal longitud
                                    temporalProyecto.UbicacionPredioPrincipalLontitud = worksheet.Cells[i, 17].Text;

                                    //#18
                                    //Dirección del predio principal 
                                    temporalProyecto.DireccionPredioPrincipal = worksheet.Cells[i, 18].Text;

                                    //#19
                                    //Documento de acreditación del predio 
                                    temporalProyecto.DocumentoAcreditacionPredioId = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 19].Text, (int)EnumeratorTipoDominio.Documento_Acreditacion));

                                    //#20
                                    //Número del documento de acreditación 
                                    temporalProyecto.NumeroDocumentoAcreditacion = worksheet.Cells[i, 20].Text;

                                    //#21
                                    //Cédula Catastral del predio 
                                    temporalProyecto.CedulaCatastralPredio = worksheet.Cells[i, 21].Text;

                                    //#22
                                    //Tipo de aportante 1 
                                    if (!string.IsNullOrEmpty(worksheet.Cells[i, 22].Text))
                                    {
                                        temporalProyecto.TipoAportanteId1 = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 22].Text, (int)EnumeratorTipoDominio.Tipo_de_aportante));

                                        //#23
                                        //Aportante 1 
                                        temporalProyecto.Aportante1 = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 23].Text, (int)EnumeratorTipoDominio.Nombre_Aportante_Aportante));
                                    }
                                    //#24
                                    //Tipo de aportante 2
                                    if (!string.IsNullOrEmpty(worksheet.Cells[i, 24].Text))
                                    {
                                        temporalProyecto.TipoAportanteId2 = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 24].Text, (int)EnumeratorTipoDominio.Tipo_de_aportante));

                                        //#25
                                        //Aportante 2
                                        temporalProyecto.Aportante2 = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 25].Text, (int)EnumeratorTipoDominio.Nombre_Aportante_Aportante));
                                    }
                                    //#26
                                    //Tipo de aportante 3
                                    if (!string.IsNullOrEmpty(worksheet.Cells[i, 26].Text))
                                    {
                                        temporalProyecto.TipoAportanteId3 = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 26].Text, (int)EnumeratorTipoDominio.Tipo_de_aportante));

                                        //#27
                                        //Aportante 3
                                        temporalProyecto.Aportante3 = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 27].Text, (int)EnumeratorTipoDominio.Nombre_Aportante_Aportante));
                                    }
                                    //#28
                                    //Vigencia del acuerdo de cofinanciación 
                                    if (!string.IsNullOrEmpty(worksheet.Cells[i, 28].Text))
                                    {
                                        temporalProyecto.VigenciaAcuerdoCofinanciacion = Int32.Parse(worksheet.Cells[i, 28].Text);
                                    }

                                    //#29
                                    //Valor obra 
                                    string onlyNumber = worksheet.Cells[i, 29].Text.Replace("$", "");
                                    temporalProyecto.ValorObra = decimal.Parse(worksheet.Cells[i, 29].Text.Replace("$", ""));

                                    //#30
                                    //Valor interventoría 
                                    temporalProyecto.ValorInterventoria = decimal.Parse(worksheet.Cells[i, 30].Text.Replace("$", ""));

                                    //#31
                                    //Valor Total 
                                    temporalProyecto.ValorTotal = decimal.Parse(worksheet.Cells[i, 31].Text.Replace("$", ""));

                                    //#32
                                    //Infraestructura para intervenir 
                                    temporalProyecto.EspacioIntervenirId = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 32].Text, (int)EnumeratorTipoDominio.Espacios_Intervenir));

                                    //#33
                                    //Cantidad 
                                    temporalProyecto.Cantidad = (Int32.Parse(worksheet.Cells[i, 33].Text));

                                    //#34
                                    //Plazo en meses Obra 
                                    if (!string.IsNullOrEmpty(worksheet.Cells[i, 34].Text))
                                    {
                                        temporalProyecto.PlazoMesesObra = (Int32.Parse(worksheet.Cells[i, 34].Text));
                                    }
                                    //#35
                                    //Plazo en días Obra 
                                    if (!string.IsNullOrEmpty(worksheet.Cells[i, 35].Text))
                                    {
                                        temporalProyecto.PlazoDiasObra = (Int32.Parse(worksheet.Cells[i, 35].Text));
                                    }
                                    //#36
                                    //Plazo en meses Interventoría 
                                    if (!string.IsNullOrEmpty(worksheet.Cells[i, 36].Text))
                                    {
                                        temporalProyecto.PlazoMesesInterventoria = (Int32.Parse(worksheet.Cells[i, 36].Text));
                                    }
                                    //#37
                                    //Plazo en meses Interventoría 
                                    if (!string.IsNullOrEmpty(worksheet.Cells[i, 37].Text))
                                    {
                                        temporalProyecto.PlazoDiasInterventoria = (Int32.Parse(worksheet.Cells[i, 37].Text));
                                    }
                                    //#38
                                    //Coordinación responsable 
                                    if (!string.IsNullOrEmpty(worksheet.Cells[i, 38].Text))
                                    {
                                        temporalProyecto.CoordinacionResponsableId = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 38].Text, (int)EnumeratorTipoDominio.Coordinaciones));
                                    }

                                    //Guarda Cambios en una tabla temporal

                                    _context.TemporalProyecto.Add(temporalProyecto);
                                    _context.SaveChanges();

                                    if (temporalProyecto.TemporalProyectoId > 0)
                                    {
                                        CantidadResgistrosValidos++;
                                    }
                                    else
                                    {
                                        CantidadRegistrosInvalidos++;
                                    }
                                }
                                else
                                {
                                    //Aqui entra cuando alguno de los campos obligatorios no viene diligenciado
                                    string strValidateCampNullsOrEmpty = "";
                                    //Valida que todos los campos esten vacios porque las validaciones del excel hacen que lea todos los rows como ingresado información 

                                    for (int j = 1; j < 37; j++)
                                    {
                                        strValidateCampNullsOrEmpty += (worksheet.Cells[i, j].Text);
                                    }
                                    if (string.IsNullOrEmpty(strValidateCampNullsOrEmpty))
                                    {
                                        CantidadRegistrosVacios++;
                                    }
                                    else
                                    {
                                        CantidadRegistrosInvalidos++;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                CantidadRegistrosInvalidos++;
                            }
                        }

                        //Actualizo el archivoCarge con la cantidad de registros validos , invalidos , y el total;
                        //-2 ya los registros comienzan desde esta fila
                        archivoCarge.CantidadRegistrosInvalidos = CantidadRegistrosInvalidos;
                        archivoCarge.CantidadRegistrosValidos = CantidadResgistrosValidos;
                        archivoCarge.CantidadRegistros = (worksheet.Dimension.Rows - CantidadRegistrosVacios - 2);
                        _context.ArchivoCargue.Update(archivoCarge);


                        ArchivoCargueRespuesta archivoCargueRespuesta = new ArchivoCargueRespuesta
                        {
                            CantidadDeRegistros = archivoCarge.CantidadRegistros.ToString(),
                            CantidadDeRegistrosInvalidos = archivoCarge.CantidadRegistrosInvalidos.ToString(),
                            CantidadDeRegistrosValidos = archivoCarge.CantidadRegistrosValidos.ToString(),
                            LlaveConsulta = archivoCarge.Nombre

                        };
                        return respuesta =
                            new Respuesta
                            {
                                Data = archivoCargueRespuesta,
                                IsSuccessful = true,
                                IsException = false,
                                IsValidation = false,
                                Code = ConstantMessagesCargueProyecto.OperacionExitosa,
                                Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.OperacionExitosa, (int)enumeratorAccion.ValidarExcel, pUsuarioCreo, "")
                            };
                    }
                }
            }
            else
            {
                return respuesta =
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantMessagesCargueProyecto.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.Error, (int)enumeratorAccion.ValidarExcel, pUsuarioCreo, "")
                    };
            }
        }

        public async Task<Respuesta> UploadMassiveLoadProjects(string pIdDocument, string pUsuarioModifico)
        {
            Respuesta respuesta = new Respuesta();

            if (string.IsNullOrEmpty(pIdDocument))
            {
                return respuesta =
                 new Respuesta
                 {
                     IsSuccessful = false,
                     IsException = false,
                     IsValidation = true,
                     Code = ConstantMessagesCargueProyecto.CamposVacios,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.CamposVacios, (int)enumeratorAccion.CargueProyectosMasivos, pUsuarioModifico, "")
                 };
            }
            try
            {


                int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.Proyecto, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);

                ArchivoCargue archivoCargue = _context.ArchivoCargue.Where(r => r.OrigenId == 1 && r.Nombre.Trim().ToUpper().Equals(pIdDocument.ToUpper().Trim())).FirstOrDefault();

                List<TemporalProyecto> ListTemporalProyecto = await _context.TemporalProyecto.Where(r => r.ArchivoCargueId == archivoCargue.ArchivoCargueId && !(bool)r.EstaValidado).ToListAsync();

                if (ListTemporalProyecto.Count() > 0)
                {
                    foreach (var temporalProyecto in ListTemporalProyecto)
                    {

                        //Predio 
                        //Predio Auditoria
                        Predio predio = new Predio();
                        predio.FechaCreacion = DateTime.Now;
                        predio.Activo = true;
                        predio.UsuarioCreacion = temporalProyecto.UsuarioCreacion;

                        //Predio Registros
                        predio.InstitucionEducativaSedeId = temporalProyecto.InstitucionEducativaId;
                        predio.TipoPredioCodigo = temporalProyecto.TipoPredioId.ToString();
                        predio.UbicacionLatitud = temporalProyecto.UbicacionPredioPrincipalLatitud;
                        predio.UbicacionLongitud = temporalProyecto.UbicacionPredioPrincipalLontitud;
                        predio.Direccion = temporalProyecto.DireccionPredioPrincipal;
                        predio.DocumentoAcreditacionCodigo = temporalProyecto.DocumentoAcreditacionPredioId.ToString();
                        predio.NumeroDocumento = temporalProyecto.NumeroDocumentoAcreditacion;
                        predio.CedulaCatastral = temporalProyecto.CedulaCatastralPredio;
                        //
                        _context.Predio.Add(predio);
                        _context.SaveChanges();

                        //Proyecto
                        //Proyecto Auditoria
                        Proyecto proyecto = new Proyecto();
                        proyecto.Eliminado = false;
                        proyecto.FechaCreacion = DateTime.Now;
                        proyecto.UsuarioCreacion = temporalProyecto.UsuarioCreacion;

                        //Proyecto Registros

                        proyecto.FechaSesionJunta = temporalProyecto.FechaSesionJunta;
                        proyecto.TipoIntervencionCodigo = temporalProyecto.TipoIntervencionId.ToString();
                        proyecto.LlaveMen = temporalProyecto.LlaveMen;
                        proyecto.LocalizacionIdMunicipio = temporalProyecto.Municipio.ToString();
                        proyecto.InstitucionEducativaId = temporalProyecto.InstitucionEducativaId;
                        proyecto.SedeId = temporalProyecto.SedeId;
                        proyecto.EnConvocatoria = temporalProyecto.EnConvotatoria;
                        proyecto.ConvocatoriaId = temporalProyecto.ConvocatoriaId;
                        proyecto.CantPrediosPostulados = temporalProyecto.CantPrediosPostulados;
                        proyecto.TipoIntervencionCodigo = temporalProyecto.TipoIntervencionId.ToString();
                        proyecto.PredioPrincipalId = predio.PredioId;
                        proyecto.ValorObra = temporalProyecto.ValorObra;
                        proyecto.ValorInterventoria = temporalProyecto.ValorInterventoria;
                        proyecto.ValorTotal = temporalProyecto.ValorTotal;
                        proyecto.EstadoProyectoCodigo = " ";
                        proyecto.TipoPredioCodigo = temporalProyecto.EspacioIntervenirId.ToString();
                        //
                        _context.Proyecto.Add(proyecto);
                        _context.SaveChanges();



                        //ProyectoPredio
                        //Proyecto  Auditoria
                        ProyectoPredio proyectoPredio = new ProyectoPredio();
                        proyectoPredio.FechaCreacion = DateTime.Now;
                        proyectoPredio.UsuarioCreacion = temporalProyecto.UsuarioCreacion;
                        proyectoPredio.Activo = true;

                        //ProyectoPredio
                        //Registros
                        //proyectoPredio.EstadoJuridicoCodigo = "";
                        proyectoPredio.ProyectoId = proyecto.ProyectoId;
                        proyectoPredio.PredioId = predio.PredioId;
                        //
                        _context.ProyectoPredio.Add(proyectoPredio);
                        _context.SaveChanges();

                        //Relacionar Ids


                        //   _context.Proyecto.Add(proyecto);

                        //Cofinanciacion
                        Cofinanciacion cofinanciacion = new Cofinanciacion();
                        cofinanciacion.Eliminado = false;
                        cofinanciacion.FechaCreacion = DateTime.Now;
                        cofinanciacion.VigenciaCofinanciacionId = temporalProyecto.VigenciaAcuerdoCofinanciacion;
                        cofinanciacion.UsuarioCreacion = temporalProyecto.UsuarioCreacion;
                        //
                        _context.Cofinanciacion.Add(cofinanciacion);
                        _context.SaveChanges();

                        //CofinanciacionAportante 1 
                        if (!string.IsNullOrEmpty(temporalProyecto.TipoAportanteId1.ToString()))
                        {
                            //Auditoria
                            CofinanciacionAportante cofinanciacionAportante1 = new CofinanciacionAportante();
                            cofinanciacionAportante1.FechaCreacion = DateTime.Now;
                            cofinanciacionAportante1.Eliminado = false;
                            //Registros
                            cofinanciacionAportante1.UsuarioCreacion = temporalProyecto.UsuarioCreacion;
                            cofinanciacionAportante1.CofinanciacionId = cofinanciacion.CofinanciacionId;
                            cofinanciacionAportante1.TipoAportanteId = (int)temporalProyecto.TipoAportanteId1;
                            cofinanciacionAportante1.NombreAportanteId = (int)temporalProyecto.Aportante1;
                            //
                            _context.CofinanciacionAportante.Add(cofinanciacionAportante1);
                            _context.SaveChanges();
                            //ProyectoAportante
                            //Auditoria
                            ProyectoAportante proyectoAportante = new ProyectoAportante();
                            proyectoAportante.FechaCreacion = DateTime.Now;
                            proyectoAportante.UsuarioCreacion = temporalProyecto.UsuarioCreacion;
                            proyectoAportante.Eliminado = false;

                            //Registros
                            proyectoAportante.ProyectoId = proyecto.ProyectoId;
                            proyectoAportante.AportanteId = cofinanciacionAportante1.CofinanciacionAportanteId;

                            //
                            _context.ProyectoAportante.Add(proyectoAportante);
                            _context.SaveChanges();
                            // proyectoAportante.
                        }
                        //CofinanciacionAportante 2
                        if (!string.IsNullOrEmpty(temporalProyecto.TipoAportanteId2.ToString()))
                        {
                            //Auditoria
                            CofinanciacionAportante cofinanciacionAportante2 = new CofinanciacionAportante();
                            cofinanciacionAportante2.FechaCreacion = DateTime.Now;
                            cofinanciacionAportante2.Eliminado = false;
                            //Registros 
                            cofinanciacionAportante2.UsuarioCreacion = temporalProyecto.UsuarioCreacion;
                            cofinanciacionAportante2.CofinanciacionId = cofinanciacion.CofinanciacionId;
                            cofinanciacionAportante2.TipoAportanteId = (int)temporalProyecto.TipoAportanteId2;
                            cofinanciacionAportante2.NombreAportanteId = (int)temporalProyecto.Aportante2;
                            //
                            _context.CofinanciacionAportante.Add(cofinanciacionAportante2);
                            _context.SaveChanges();
                            //ProyectoAportante
                            //Auditoria
                            ProyectoAportante proyectoAportante = new ProyectoAportante();
                            proyectoAportante.FechaCreacion = DateTime.Now;
                            proyectoAportante.UsuarioCreacion = temporalProyecto.UsuarioCreacion;
                            proyectoAportante.Eliminado = false;

                            //Registros
                            proyectoAportante.ProyectoId = proyecto.ProyectoId;
                            proyectoAportante.AportanteId = cofinanciacionAportante2.CofinanciacionAportanteId;

                            //
                            _context.ProyectoAportante.Add(proyectoAportante);
                            _context.SaveChanges();
                        }
                        //CofinanciacionAportante 3
                        if (!string.IsNullOrEmpty(temporalProyecto.TipoAportanteId2.ToString()))
                        {
                            //Auditoria
                            CofinanciacionAportante cofinanciacionAportante3 = new CofinanciacionAportante();
                            cofinanciacionAportante3.FechaCreacion = DateTime.Now;
                            cofinanciacionAportante3.Eliminado = false;
                            //Registros
                            cofinanciacionAportante3.UsuarioCreacion = temporalProyecto.UsuarioCreacion;
                            cofinanciacionAportante3.CofinanciacionId = cofinanciacion.CofinanciacionId;
                            cofinanciacionAportante3.TipoAportanteId = (int)temporalProyecto.TipoAportanteId3;
                            cofinanciacionAportante3.NombreAportanteId = (int)temporalProyecto.Aportante3;
                            //
                            _context.CofinanciacionAportante.Add(cofinanciacionAportante3);
                            _context.SaveChanges();
                            //ProyectoAportante
                            //Auditoria
                            ProyectoAportante proyectoAportante = new ProyectoAportante();
                            proyectoAportante.FechaCreacion = DateTime.Now;
                            proyectoAportante.UsuarioCreacion = temporalProyecto.UsuarioCreacion;
                            proyectoAportante.Eliminado = false;

                            //Registros
                            proyectoAportante.ProyectoId = proyecto.ProyectoId;
                            proyectoAportante.AportanteId = cofinanciacionAportante3.CofinanciacionAportanteId;

                            //
                            _context.ProyectoAportante.Add(proyectoAportante);
                            _context.SaveChanges();
                        }

                        //Temporal proyecto update
                        temporalProyecto.EstaValidado = true;
                        temporalProyecto.FechaModificacion = DateTime.Now;
                        temporalProyecto.UsuarioModificacion = pUsuarioModifico;
                        _context.TemporalProyecto.Update(temporalProyecto);
                        _context.SaveChanges();
                    }


                    return respuesta =
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesCargueProyecto.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.OperacionExitosa, (int)enumeratorAccion.CargueProyectosMasivos, pUsuarioModifico, "Cantidad de Proyectos subidos : " + ListTemporalProyecto.Count())
                    };
                }
                else
                {
                    return respuesta =
                        new Respuesta
                        {
                            IsSuccessful = false,
                            IsException = false,
                            IsValidation = true,
                            Code = ConstantMessagesCargueProyecto.NoExitenArchivos,
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.NoExitenArchivos, (int)enumeratorAccion.CargueProyectosMasivos, pUsuarioModifico, "")
                        };
                }
            }
            catch (Exception ex)
            {
                return respuesta =
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesCargueProyecto.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.Error, (int)enumeratorAccion.CargueProyectosMasivos, pUsuarioModifico, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Proyecto> GetProyectoByProyectoId(int idProyecto)
        {
            Proyecto proyecto = await _context.Proyecto.Where(r => r.ProyectoId == idProyecto).FirstOrDefaultAsync();
            proyecto.ProyectoAportante = _context.ProyectoAportante.Where(x => x.ProyectoId == proyecto.ProyectoId && x.Eliminado == false).Include(y => y.Aportante).Include(z => z.CofinanciacionDocumento).ToList();
            proyecto.PredioPrincipal = _context.Predio.Where(x => x.PredioId == proyecto.PredioPrincipalId && x.Activo == true).FirstOrDefault();
            List<InfraestructuraIntervenirProyecto> infraestructuras = _context.InfraestructuraIntervenirProyecto.Where(x => x.ProyectoId == proyecto.ProyectoId && x.Eliminado == false).ToList();
            foreach (var infraestructura in infraestructuras)
            {
                infraestructura.Proyecto = null;
            }
            proyecto.InfraestructuraIntervenirProyecto = infraestructuras;
            //proyecto.PredioPrincipal = _context.Predio.Where(x => x.PredioId == proyecto.PredioPrincipalId && x.Activo == true).ToList();
            return proyecto;
        }

        public async Task<bool> DeleteProyectoByProyectoId(int idProyecto)
        {
            Proyecto proyecto = _context.Proyecto.Find(idProyecto);
            bool retorno = true;
            try
            {
                proyecto.Eliminado = true;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return retorno;
        }

        public async Task<Respuesta> CreateOrEditAdministrativeProject(ProyectoAdministrativo pProyectoAdministrativo)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearProyectoAdministrativo = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proyecto_Administrativo, (int)EnumeratorTipoDominio.Acciones);


            //Crear Proyecto Administrativo 
            //Es nuevo 
            if (pProyectoAdministrativo.ProyectoAdministrativoId == null || pProyectoAdministrativo.ProyectoAdministrativoId == 0)
            {
                //Auditoria
                pProyectoAdministrativo.Eliminado = false;
                pProyectoAdministrativo.FechaCreado = DateTime.Now;
                _context.ProyectoAdministrativo.Add(pProyectoAdministrativo);
                _context.SaveChanges();

                //Como es nuevo creo la relacion en la tabla  
                //Proyecto Administrativo aportante
                foreach (var ProyectoAdministrativo in pProyectoAdministrativo.ProyectoAdministrativoAportante)
                {
                    ProyectoAdministrativoAportante proyectoAdministrativoAportante = new ProyectoAdministrativoAportante
                    {
                        //Auditoria 
                        Eliminado = false,
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = pProyectoAdministrativo.UsuarioCreacion,

                        //Registros  
                        AportanteId = ProyectoAdministrativo.AportanteId,
                        ProyectoAdminstrativoId = ProyectoAdministrativo.ProyectoAdminstrativoId
                    };
                    _context.ProyectoAdministrativoAportante.Add(proyectoAdministrativoAportante);
                    //Guarda relacion
                    _context.SaveChanges();

                    //Como la relacion es de 3 niveles dentro de este esta el otro nivel fuentes de financiación
                    foreach (var FuenteFinanciacion in ProyectoAdministrativo.Aportante.FuenteFinanciacion)
                    {
                        AportanteFuenteFinanciacion aportanteFuenteFinanciacion = new AportanteFuenteFinanciacion
                        {
                            //Auditoria
                            UsuarioCreacion = pProyectoAdministrativo.UsuarioCreacion,
                            FechaCreacion = DateTime.Now,
                            Eliminado = false,

                            //Registros
                            FuenteFinanciacionId = FuenteFinanciacion.FuenteFinanciacionId,
                            AportanteId = ProyectoAdministrativo.AportanteId
                        };
                        _context.AportanteFuenteFinanciacion.Add(aportanteFuenteFinanciacion);
                        //Guarda relacion
                        _context.SaveChanges();
                    }
                }

            }
            //Editar Proyecto Administrativo
            else
            {
                //Auditoria
                ProyectoAdministrativo proyectoAdministrativoAntiguo = _context.ProyectoAdministrativo.Find(pProyectoAdministrativo.ProyectoAdministrativoId);
                proyectoAdministrativoAntiguo.FechaModificacion = DateTime.Now;
                proyectoAdministrativoAntiguo.UsuarioModificacion = pProyectoAdministrativo.UsuarioCreacion;
                //Cambio Campos
                proyectoAdministrativoAntiguo.Eliminado = pProyectoAdministrativo.Eliminado;
                proyectoAdministrativoAntiguo.Enviado = pProyectoAdministrativo.Enviado;
                _context.Update(proyectoAdministrativoAntiguo);

            }

            try
            {

                return respuesta =
                   new Respuesta
                   {
                       IsSuccessful = true,
                       IsException = false,
                       IsValidation = false,
                       Code = ConstantMessagesProyecto.OperacionExitosa,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearProyectoAdministrativo, pProyectoAdministrativo.UsuarioCreacion, " ")
                   };
            }
            catch (Exception ex)
            {
                return respuesta =
                     new Respuesta
                     {
                         IsSuccessful = false,
                         IsException = true,
                         IsValidation = true,
                         Code = ConstantMessagesProyecto.Error,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesProyecto.Error, idAccionCrearProyectoAdministrativo, pProyectoAdministrativo.UsuarioCreacion, ex.InnerException.ToString())
                     };
            }
        }

        public async Task<List<ProyectoAdministracionGrilla>> ListAdministrativeProyectos(string pUsuarioConsulto)
        {
            List<ProyectoAdministracionGrilla> ListProyectoAdministrativoGrilla = new List<ProyectoAdministracionGrilla>();

            try
            {
                List<ProyectoAdministrativo> ListProyectosAdministrativo = await _context.ProyectoAdministrativo.Where(r => !(bool)r.Eliminado).ToListAsync();

                foreach (var proyecto in ListProyectosAdministrativo)
                {
                    /*Localizacion municipio = await _commonService.GetLocalizacionByLocalizacionId(proyecto.LocalizacionIdMunicipio);
                    Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio estadoRegistro = await _commonService.GetDominioByNombreDominioAndTipoDominio(proyecto.EstadoProyectoCodigo, (int)EnumeratorTipoDominio.Estado_Registro);
                    // Dominio EstadoJuridicoPredios = await _commonService.GetDominioByNombreDominioAndTipoDominio(proyecto.ProyectoPredio.FirstOrDefault().EstadoJuridicoCodigo, (int)EnumeratorTipoDominio.Estado_Registro);
                    */
                    ProyectoAdministracionGrilla proyectoAdministrativoGrilla = new ProyectoAdministracionGrilla
                    {
                        ProyectoAdminitracionId = proyecto.ProyectoAdministrativoId,
                        Enviado = (bool)proyecto.Enviado
                    };
                    ListProyectoAdministrativoGrilla.Add(proyectoAdministrativoGrilla);
                }
                return ListProyectoAdministrativoGrilla;
            }
            catch (Exception ex)
            {
                return ListProyectoAdministrativoGrilla;

            }
        }

        public async Task<bool> DeleteProyectoAdministrativoByProyectoId(int pProyectoId, string pUsuario)
        {
            int idAccionCrearProyectoAdministrativo = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Proyecto, (int)EnumeratorTipoDominio.Acciones);
            ProyectoAdministrativo proyecto = _context.ProyectoAdministrativo.Find(pProyectoId);
            bool retorno = true;
            try
            {
                proyecto.Eliminado = true;
                proyecto.UsuarioModificacion = pUsuario;
                proyecto.FechaModificacion = DateTime.Now;
                _context.SaveChanges();
                //auditoria
                string msg = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearProyectoAdministrativo, pUsuario, "ELIMINACIÓN DE PROYECTO");
            }
            catch (Exception ex)
            {
                string msg = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.Error, idAccionCrearProyectoAdministrativo, pUsuario, ex.InnerException.ToString());
                return false;
            }
            return retorno;
        }

        public async Task<bool> EnviarProyectoAdministrativoByProyectoId(int pProyectoId, string pUsuario)
        {
            int idAccionCrearProyectoAdministrativo = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Proyecto, (int)EnumeratorTipoDominio.Acciones);
            ProyectoAdministrativo proyecto = _context.ProyectoAdministrativo.Find(pProyectoId);
            bool retorno = true;
            try
            {
                proyecto.Enviado = true;
                proyecto.UsuarioModificacion = pUsuario;
                proyecto.FechaModificacion = DateTime.Now;
                _context.SaveChanges();
                string msg = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearProyectoAdministrativo, pUsuario, "ENVIAR PROYECTO");
            }
            catch (Exception ex)
            {
                string msg = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.Error, idAccionCrearProyectoAdministrativo, pUsuario, ex.InnerException.ToString());
                return false;
            }
            return retorno;
        }

        public async Task<List<FuenteFinanciacion>> GetFontsByAportantId(int pAportanteId)
        {
            return _context.FuenteFinanciacion.Where(x => x.Aportante.NombreAportanteId == pAportanteId).ToList();
        }
    }
}
