using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using Microsoft.Extensions.Options;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using System.Text;

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


        [HttpGet]
        [Route("GetListloadedDocuments")]
        public async Task<ActionResult<List<ArchivoCargue>>> GetListloadedDocuments(string pOrigenId = "1")
        {
            var result = await _documentService.GetListloadedDocuments(pOrigenId);
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
                string pUser = " ";
                // string  pUser = HttpContext.User.FindFirst("User").Value;
                ArchivoCargue archivoCargue = await  _documentService.GetArchivoCargueByName(pNameFiles  , pUser);

                string Ruta = archivoCargue.Ruta + '/' + archivoCargue.Nombre+ ".xlsx";

                Stream stream = new FileStream(Ruta, FileMode.Open, FileAccess.Read);

                if (stream == null)
                    return NotFound();
                return  File(stream, "application/octet-stream");

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
                throw new Exception( "Parametro invalido" );

            try
            {
                string  pUser = HttpContext.User.FindFirst("User").Value;
                ArchivoCargue archivoCargue = await  _documentService.GetArchivoCargueById(pArchivoCargueId  , pUser);

                string Ruta = archivoCargue.Ruta + '/' + archivoCargue.Nombre+ ".xlsx";

                Stream stream = new FileStream(Ruta, FileMode.Open, FileAccess.Read);

                if (stream == null)
                    return NotFound();
                return  File(stream, "application/octet-stream");

            }
            catch (Exception e)
            {
                return BadRequest("Archivo no encontrado");
            }

        }



    }
}
