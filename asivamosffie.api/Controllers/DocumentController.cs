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

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {

        private readonly IDocumentService _documentService;
        private readonly IOptions<AppSettings> _settings;

        public DocumentController(IDocumentService documentService , IOptions<AppSettings> settings ) {
            _settings = settings;
            _documentService = documentService;
        }
        // POST: api/Documentsadsd
        [HttpPost]
        public async Task<IActionResult> CreateFile(IFormCollection form)
        {
            Guid g = Guid.NewGuid();
            if (!Directory.Exists(_settings.Value.DirectoryBase + _settings.Value.DirectoryBaseProyectos))
            {
                Directory.CreateDirectory(_settings.Value.DirectoryBase + _settings.Value.DirectoryBaseProyectos);
            }
            var filePath = _settings.Value.DirectoryBase + _settings.Value.DirectoryBaseProyectos + "/" + g.ToString() + form.Files[0].FileName;
            int id = Convert.ToInt32(form["id"]);

            // userparam.Ip = HttpContext.Connection.RemoteIpAddress.ToString();
            //   userparam.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
            Task<Respuesta> result = _documentService.(form);
            Respuesta respuesta = await result;
        }
  
    }
}
