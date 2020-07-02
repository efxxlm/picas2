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

namespace asivamosffie.services
{
    public class DocumentService : IDocumentService
    {

        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;

        public DocumentService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        public async Task<Respuesta> SetValidateCargueMasivo(IFormFile pFile, string pFilePatch, string pUsuarioCreo)
        {
            int CantidadRegistrosVacios = 0;
            int CantidadResgistrosValidos = 0;
            int CantidadRegistrosInvalidos = 0;
            Respuesta respuesta = new Respuesta();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            ArchivoCargue archivoCarge = await getSaveFile(pFile, pFilePatch);

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
                        //Filas
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
                                    temporalProyecto.EsValido = false;
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
                                    temporalProyecto.TipoIntervencionId = await _commonService.GetDominioIdByNombreDominioAndTipoDominio(worksheet.Cells[i, 3].Text, (int)EnumeratorTipoDominio.Tipo_de_Intervencion);

                                    //#4
                                    // Llave MEN  
                                    temporalProyecto.LlaveMen = worksheet.Cells[i, 4].Text;

                                    //#5
                                    //Región

                                    //#6
                                    //Departamento
                                    temporalProyecto.Departamento = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 6].Text, true);

                                    //#7
                                    //Municipio
                                    temporalProyecto.Municipio = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 7].Text, false);


                                    //#8
                                    //Institución Educativa 
                                    //Validar si existe institucion educativa y guardar id
                                    int idInstitucionEducativaSede = await _commonService.getInstitucionEducativaIdByName(worksheet.Cells[i, 8].Text);
                                    if (idInstitucionEducativaSede > 0)
                                    {
                                        temporalProyecto.InstitucionEducativa = idInstitucionEducativaSede.ToString();
                                    }
                                    else
                                    {
                                        archivoCarge.CantidadRegistrosInvalidos++;
                                        break;
                                    }


                                    //#9
                                    //Código DANE IE 
                                    temporalProyecto.CodigoDaneIe = Int32.Parse(worksheet.Cells[i, 9].Text);

                                    //#10
                                    //Código DANE IE 
                                    //Validar si existe la sede y poner id si no crear sede y poner id 
                                    int idInstitucionEducativaSede2 = await _commonService.getInstitucionEducativaIdByName(worksheet.Cells[i, 10].Text);
                                    if (idInstitucionEducativaSede2 > 0)
                                    { temporalProyecto.Sede = idInstitucionEducativaSede2.ToString(); }
                                    else
                                    {
                                        archivoCarge.CantidadRegistrosInvalidos++;
                                        break;
                                    }

                                    //#11
                                    //Código DANE SEDE 
                                    temporalProyecto.CodigoDaneSede = Int32.Parse(worksheet.Cells[i, 11].Text);

                                    //#12
                                    //¿Se encuentra dentro de una convocatoria? 
                                    if (Int32.Parse(worksheet.Cells[i, 12].Text).ToString().ToUpper().Contains("SI") || Int32.Parse(worksheet.Cells[i, 12].Text).ToString().ToUpper().Contains("VERDADERO"))
                                    { temporalProyecto.EstaEnConvotatoria = true; }
                                    else { temporalProyecto.EstaEnConvotatoria = false; };

                                    //#13
                                    //Convocatoria
                                    temporalProyecto.ConvocatoriaId = await _commonService.GetDominioIdByNombreDominioAndTipoDominio(worksheet.Cells[i, 13].Text, (int)EnumeratorTipoDominio.Convocatoria);

                                    //#14
                                    //Número de predios postulados
                                    temporalProyecto.NumeroPrediosPostulados = Int32.Parse(worksheet.Cells[i, 14].Text);

                                    //#15
                                    //Tipo de predio(s) 
                                    temporalProyecto.TipoPrediosId = await _commonService.GetDominioIdByNombreDominioAndTipoDominio(worksheet.Cells[i, 15].Text, (int)EnumeratorTipoDominio.Tipo_de_Predios);

                                    //#16
                                    //Ubicación del predio principal
                                    temporalProyecto.UbicacionPredioPrincipalLatitud = worksheet.Cells[i, 16].Text;

                                    //#17
                                    //Dirección del predio principal 
                                    temporalProyecto.DireccionPredioPrincipal = worksheet.Cells[i, 17].Text;

                                    //#18
                                    //Documento de acreditación del predio 
                                    temporalProyecto.DocumentoAcreditacionPredioId = await _commonService.GetDominioIdByNombreDominioAndTipoDominio(worksheet.Cells[i, 18].Text, (int)EnumeratorTipoDominio.Documento_Acreditacion);

                                    //#19
                                    //Número del documento de acreditación 
                                    temporalProyecto.NumeroDocumentoAcreditacion = worksheet.Cells[i, 19].Text;

                                    //#20
                                    //Cédula Catastral del predio 
                                    temporalProyecto.CedulaCatastralPredio = worksheet.Cells[i, 20].Text;

                                    //#21
                                    //Tipo de aportante 1 
                                    temporalProyecto.TipoAportanteId1 = await _commonService.GetDominioIdByNombreDominioAndTipoDominio(worksheet.Cells[i, 21].Text, (int)EnumeratorTipoDominio.Tipo_de_aportante);

                                    //#22
                                    //Aportante 1 
                                    temporalProyecto.Aportante1 = await _commonService.GetDominioIdByNombreDominioAndTipoDominio(worksheet.Cells[i, 22].Text, (int)EnumeratorTipoDominio.Nombre_Aportante_Aportante);

                                    //#23
                                    //Tipo de aportante 2
                                    temporalProyecto.TipoAportanteId2 = await _commonService.GetDominioIdByNombreDominioAndTipoDominio(worksheet.Cells[i, 23].Text, (int)EnumeratorTipoDominio.Tipo_de_aportante);

                                    //#24
                                    //Aportante 2
                                    temporalProyecto.Aportante2 = await _commonService.GetDominioIdByNombreDominioAndTipoDominio(worksheet.Cells[i, 24].Text, (int)EnumeratorTipoDominio.Nombre_Aportante_Aportante);

                                    //#25
                                    //Tipo de aportante 3
                                    temporalProyecto.TipoAportanteId3 = await _commonService.GetDominioIdByNombreDominioAndTipoDominio(worksheet.Cells[i, 25].Text, (int)EnumeratorTipoDominio.Tipo_de_aportante);

                                    //#26
                                    //Aportante 3
                                    temporalProyecto.Aportante3 = await _commonService.GetDominioIdByNombreDominioAndTipoDominio(worksheet.Cells[i, 26].Text, (int)EnumeratorTipoDominio.Nombre_Aportante_Aportante);

                                    //#27
                                    //Vigencia del acuerdo de cofinanciación 
                                    temporalProyecto.VigenciaAcuerdoCofinanciacion = Int32.Parse(worksheet.Cells[i, 27].Text);

                                    //#28
                                    //Valor obra 
                                    temporalProyecto.ValorObra = (Int32.Parse(worksheet.Cells[i, 28].Text)).ToString();

                                    //#29
                                    //Valor interventoría 
                                    temporalProyecto.ValorInterventoria = (Int32.Parse(worksheet.Cells[i, 29].Text)).ToString();

                                    //#30
                                    //Valor Total 
                                    temporalProyecto.ValorTotal = (Int32.Parse(worksheet.Cells[i, 30].Text)).ToString();

                                    //#31
                                    //Infraestructura para intervenir 
                                    temporalProyecto.EspacioIntervenirId = await _commonService.GetDominioIdByNombreDominioAndTipoDominio(worksheet.Cells[i, 31].Text, (int)EnumeratorTipoDominio.Espacios_Intervenir);

                                    //#32
                                    //Cantidad 
                                    //Revisar El siguiente comentario del caso de uso
                                    //Si son varias infraestructuras, se   deben registrar los valores al lado de cada una 
                                    temporalProyecto.Cantidad = (Int32.Parse(worksheet.Cells[i, 32].Text));

                                    //#33
                                    //Plazo en meses Obra 
                                    temporalProyecto.PlazoMesesObra = (Int32.Parse(worksheet.Cells[i, 33].Text));

                                    //#34
                                    //Plazo en días Obra 
                                    temporalProyecto.PlazoDiasObra = (Int32.Parse(worksheet.Cells[i, 34].Text));

                                    //#35
                                    //Plazo en meses Interventoría 
                                    temporalProyecto.PlazoMesesInterventoria = (Int32.Parse(worksheet.Cells[i, 35].Text));

                                    //#36
                                    //Plazo en meses Interventoría 
                                    temporalProyecto.PlazoDiasInterventoria = (Int32.Parse(worksheet.Cells[i, 36].Text));

                                    //#37
                                    //Coordinación responsable 
                                    temporalProyecto.CoordinacionResponsableId = await _commonService.GetDominioIdByNombreDominioAndTipoDominio(worksheet.Cells[i, 37].Text, (int)EnumeratorTipoDominio.Coordinaciones);


                                    //#37
                                    //Ubicación del predio principal
                                    temporalProyecto.UbicacionPredioPrincipalLatitud = worksheet.Cells[i, 37].Text;


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
                                    else {
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


        public async Task<ArchivoCargue> getSaveFile(IFormFile pFile, string pFilePatch)
        {
            try
            {
                ArchivoCargue archivoCargue = new ArchivoCargue();
                Guid g = Guid.NewGuid();
                archivoCargue.OrigenId = 1;///Poner ID Para traer las cuestiones 
                archivoCargue.Activo = true;
                archivoCargue.FechaCreacion = DateTime.Now;
                archivoCargue.Ruta = pFilePatch;
                archivoCargue.Nombre = g.ToString();
                archivoCargue.Tamano = pFile.Length.ToString();
                if (!Directory.Exists(pFilePatch))
                {
                    Directory.CreateDirectory(pFilePatch);
                }
                var streamFile = new FileStream(pFilePatch + "/" + g.ToString() + "." + pFile.FileName.Split('.').Last(), FileMode.Create);
                using (streamFile)
                {
                    await pFile.CopyToAsync(streamFile);
                    _context.ArchivoCargue.Add(archivoCargue);
                    await _context.SaveChangesAsync();
                    return archivoCargue;
                }
            }
            catch (Exception e)
            {
                ArchivoCargue archivoCargue = new ArchivoCargue();
                return archivoCargue;
            }

        }

    }
}
