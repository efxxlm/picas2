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



        [Route("SetValidateMassiveLoadProjects")]
        [HttpPost]
        public async Task<IActionResult> SetValidateCargueMasivoProyectos(IFormFile file)
        {
            try
            {
                Respuesta respuesta = new Respuesta();

                if (file.Length > 0 && file.FileName.Contains(".xls"))
                {
                    string strUsuario = HttpContext.User.FindFirst("User").Value;
                    respuesta = await _documentService.SetValidateCargueMasivo(file , Path.Combine(_settings.Value.DirectoryBase, _settings.Value.DirectoryBaseCargue, _settings.Value.DirectoryBaseProyectos), strUsuario); 
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }



    }
}
