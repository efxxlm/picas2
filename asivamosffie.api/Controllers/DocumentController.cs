﻿using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {

        private readonly IDocumentService _documentService;
        private readonly IOptions<AppSettings> _settings;

        public DocumentController(IDocumentService documentService, IOptions<AppSettings> settings)
        {
            _settings = settings;
            _documentService = documentService;
        }

        [Route("GetFileByPath")]
        [HttpGet]
        public async Task<ActionResult> GetFileByPath([FromQuery] string pPath)
        {
            if (String.IsNullOrEmpty(pPath))
                return BadRequest();
            try
            {
                Stream stream = new FileStream(pPath, FileMode.Open, FileAccess.Read);

                if (stream == null)
                    return NotFound();
                return File(stream, "application/octet-stream");

            }
            catch (Exception ex)
            {
                return BadRequest("Archivo no encontrado");
            }
        }

        [HttpGet]
        [Route("GetListloadedDocuments")]
        public async Task<ActionResult<List<ArchivoCargue>>> GetListloadedDocuments(string pOrigenId = "1")
        {
            var result = await _documentService.GetListloadedDocuments(pOrigenId);
            return result;
        }

        [HttpGet]
        [Route("GetListloadedDocumentsByRelacion")]
        public async Task<ActionResult<List<ArchivoCargue>>> GetListloadedDocumentsByRelacion(string pOrigenId = "1", int pRelacionId = 0)
        {
            var result = await _documentService.GetListloadedDocumentsByRelacionId(pOrigenId, pRelacionId);
            return result;
        }


        [HttpGet]
        [Route("DownloadFilesByName")]
        public async Task<ActionResult> DownloadFilesByName([FromQuery] string pNameFiles)
        {
            if (String.IsNullOrEmpty(pNameFiles))
                return BadRequest();

            try
            {
                //string pUser = " ";
                string pUser = HttpContext.User.FindFirst("User").Value;
                ArchivoCargue archivoCargue = await _documentService.GetArchivoCargueByName(pNameFiles, pUser);

                string Ruta = archivoCargue.Ruta + '/' + archivoCargue.Nombre + ".xlsx";

                Stream stream = new FileStream(Ruta, FileMode.Open, FileAccess.Read);

                if (stream == null)
                    return NotFound();
                return File(stream, "application/octet-stream");

            }
            catch (Exception e)
            {
                return BadRequest("Archivo no encontrado");
            }

        }

        [HttpGet]
        [Route("DownloadOrdenElegibilidadFilesByName")]
        public async Task<ActionResult> DownloadOrdenElegibilidadFilesByName([FromQuery] string pNameFiles)
        {
            if (String.IsNullOrEmpty(pNameFiles))
                return BadRequest();

            try
            {
                //string pUser = " ";
                string pUser = HttpContext.User.FindFirst("User").Value;
                string Ruta = await _documentService.DownloadOrdenElegibilidadFilesByName(pNameFiles, Path.Combine(_settings.Value.DirectoryBase, _settings.Value.DirectoryBaseCargue, _settings.Value.DirectoryBaseOrdeELegibilidad), pUser);

                /*string Ruta = archivoCargue.Ruta + '/' + archivoCargue.Nombre + ".xlsx";
                */
                Stream stream = new FileStream(Ruta, FileMode.Open, FileAccess.Read);

                if (stream == null)
                    return NotFound();
                return File(stream, "application/octet-stream");

            }
            catch (Exception e)
            {
                return BadRequest("Archivo no encontrado");
            }
        }

        [HttpGet]
        [Route("DownloadFilesById")]
        public async Task<ActionResult> DownloadFilesById([FromQuery] int pArchivoCargueId)
        {
            if (pArchivoCargueId == null || pArchivoCargueId == 0)
                throw new Exception("Parametro invalido");

            try
            {
                string pUser = HttpContext.User.FindFirst("User").Value;
                ArchivoCargue archivoCargue = await _documentService.GetArchivoCargueById(pArchivoCargueId, pUser);

                string Ruta = archivoCargue.Ruta + '/' + archivoCargue.Nombre + ".xlsx";

                Stream stream = new FileStream(Ruta, FileMode.Open, FileAccess.Read);

                if (stream == null)
                    return NotFound();
                return File(stream, "application/octet-stream");

            }
            catch (Exception e)
            {
                return BadRequest("Archivo no encontrado");
            }

        }


    }
}
