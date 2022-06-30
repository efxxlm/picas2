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
         
        public async Task<ArchivoCargue> getSaveFile(IFormFile pFile, string pFilePatch , int OrigenId, int idrelacion=0)
        {
            try
            {
                Guid g = Guid.NewGuid();

                ArchivoCargue archivoCargue = new ArchivoCargue
                {
                    OrigenId = OrigenId,
                    Activo = true,
                    FechaCreacion = DateTime.Now,
                    Ruta = pFilePatch,
                    Nombre = g.ToString(),
                    Tamano = pFile.Length.ToString(),
                    ReferenciaId=idrelacion
                };
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
                return new ArchivoCargue();
            }

        }


        public async Task<bool> SaveFileContratacion(IFormFile pFile, string pFilePatch ,string pNameFile)
        {
            try
            {
                if (!Directory.Exists(pFilePatch))
                {
                    Directory.CreateDirectory(pFilePatch);
                }
                if (!string.IsNullOrEmpty(pNameFile)) {
                    var streamFile = new FileStream(pFilePatch + "/" + pNameFile, FileMode.Create);
                    using (streamFile)
                    {
                        await pFile.CopyToAsync(streamFile);

                        return true;
                    }
                }
                else{
                    var streamFile = new FileStream(pFilePatch, FileMode.Create);
                    using (streamFile)
                    {
                        await pFile.CopyToAsync(streamFile); 
                        return true;
                    }
                } 
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async  Task <List<ArchivoCargue>> GetListloadedDocuments(string pOrigenId = "1")
        {
            return await _context.ArchivoCargue.Where(r => r.OrigenId.ToString().Equals( pOrigenId ) && (bool)r.Activo).OrderByDescending(r=> r.ArchivoCargueId).OrderByDescending(r=> r.ArchivoCargueId).ToListAsync();
        }

        public async Task<ArchivoCargue> GetArchivoCargueByName(string pNombre , string pUser) {

            Respuesta respuesta = new Respuesta();
            ArchivoCargue archivoCargue = new ArchivoCargue();
            try
            { 
                archivoCargue = await _context.ArchivoCargue.Where(r => r.Nombre.Equals(pNombre)).FirstOrDefaultAsync();

                if (archivoCargue != null)
                {
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesCargueProyecto.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.DescargaExcelExitosa, (int)enumeratorAccion.DescargarExcelProyectos, pUser, "Se descargo el archivo")
                    };
                    return archivoCargue;
                }
                else {
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesCargueProyecto.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.ErrorDescargarArchivo, (int)enumeratorAccion.DescargarExcelProyectos, pUser, "No se encontro el archivo")
                    };
                    return archivoCargue;
                }
            }
            catch (Exception ex)
            {
                new Respuesta
                {
                    IsSuccessful = false,
                    IsException = false,
                    IsValidation = true,
                    Code = ConstantMessagesCargueProyecto.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.ErrorDescargarArchivo, (int)enumeratorAccion.DescargarExcelProyectos, pUser, ex.ToString())
                };
                return archivoCargue;
            }
        }

        public async Task<List<ArchivoCargue>> GetListloadedDocumentsByRelacionId(string pOrigenId, int pRelacionId)
        {
            return await _context.ArchivoCargue.Where(r => r.OrigenId.ToString().Equals(pOrigenId) && r.ReferenciaId==pRelacionId && (bool)r.Activo).OrderByDescending(r => r.ArchivoCargueId).OrderByDescending(r => r.ArchivoCargueId).ToListAsync();
        }

        public async Task<string> DownloadOrdenElegibilidadFilesByName(string pNameFiles, string pFilePatch, string pUser)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var streams = new MemoryStream();
            using (var packages = new ExcelPackage(streams))
            {
                var workSheet = packages.Workbook.Worksheets.Add("Sheet1");
                var collection = _context.TempOrdenLegibilidad.Where(x => x.ArchivoCargue.Nombre == pNameFiles).Select(
                    x=> new { Es_valido= x.EstaValidado,
                        Tipo_de_proponente = x.TipoProponenteId, //1
                        Nombre_del_proponente = x.NombreProponente, //2
                        Numero_de_identificacion_del_proponente = x.NumeroIddentificacionProponente, //3
                        Departamento_del_domicilio_del_proponente = x.Departamento, //4
                        Municipio_del_domicilio_del_proponente = x.Minicipio, //5
                        Direccion_del_proponente = x.Direccion, //6
                        Telefono_del_proponente = x.Telefono, //7
                        Correo_electronico_del_proponente = x.Correo, //8
                        Nombre_de_la_Entidad = x.NombreEntidad, //9
                        Numero_de_identificacion_Tributaria = x.IdentificacionTributaria, //10
                        Nombre_del_Representante_legal = x.RepresentanteLegal, //11
                        Cedula_del_representante = x.CedulaRepresentanteLegal, //12
                        Departamento_del_domicilio_del_representante_legal = x.DepartamentoRl, //13
                        Municipio_del_domicilio_del_representante_legal = x.MunucipioRl, //14
                        Direccion_del_representante_legal = x.DireccionRl, //15
                        Telefono_del_representante_legal = x.TelefonoRl,//16
                        Correo_electronico_del_representante_legal = x.CorreoRl, //17
                        Nombre_de_la_UT_o_consorcio = x.NombreOtoConsorcio, //18
                        Cuantas_entidades_integran_la_Union_Temporal_o_consorcio = x.EntiddaesQueIntegranLaUnionTemporal, //19
                        Nombre_Integrante_1 = x.NombreIntegrante, // 20
                        Porcentaje_de_Participacion_integrante_1 = x.PorcentajeParticipacion, //21
                        Nombre_Integrante_2 ="",// 22
                        Porcentaje_de_Participacion_integrante_2 ="", //23
                        Nombre_Integrante_3 ="", //24
                        Porcentaje_de_Participacion_integrante_3 ="", //25
                        Nombre_del_Representante_legal_de_la_UT_o_Consorcio = x.NombreRlutoConsorcio, //26
                        Cedula_del_representante_de_la_UT_o_Consorcio = x.CcrlutoConsorcio, //27
                        Departamento_del_domicilio_del_representante_legal_de_la_UT_o_Consorcio = x.DepartamentoRlutoConsorcio, //28
                        Municipio_del_domicilio_del_representante_legal_de_la_UT_o_Consorcio = x.MinicipioRlutoConsorcio, //29
                        Direccion_del_representante_legal_de_la_UT_o_Consorcio = x.DireccionRlutoConsorcio, //30
                        Telefono_del_representante_legal_de_la_UT_o_Consorcio = x.TelefonoRlutoConsorcio, //31
                        Correo_electronico_del_representante_legal_de_la_UT_o_Consorcio = x.CorreoRlutoConsorcio, //32

                    }).ToList();
                workSheet.Cells.LoadFromCollection(collection, true);
                string filePath = pFilePatch + "/" + pNameFiles + "_rev.xlsx";
                var xlFile = new FileInfo(filePath);
                packages.SaveAs(xlFile);
                
                return filePath;
            }
            return "";
        }

        public async Task<ArchivoCargue> GetArchivoCargueById(int pArchivoCargueId , string pUser) {

            Respuesta respuesta = new Respuesta();
            ArchivoCargue archivoCargue = new ArchivoCargue();
            try
            { 
                archivoCargue = _context.ArchivoCargue.Find( pArchivoCargueId );

                if (archivoCargue != null)
                {
                    return archivoCargue;
                }
                else {
                    throw new Exception( "No se encontro el archivo" );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
