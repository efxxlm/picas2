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



        [Route("SetCargueMasivoProyectos")]
        [HttpPost]
        public async Task<IActionResult> SetCargueMasivoProyectos(IFormCollection form)
        {
            try
            {

                Guid g = Guid.NewGuid();
                Respuesta respuesta = new Respuesta();
                Usuario usuario = new Usuario();
                if (!Directory.Exists(_settings.Value.DirectoryBase + _settings.Value.DirectoryBaseProyectos))
                {
                    Directory.CreateDirectory(_settings.Value.DirectoryBase + _settings.Value.DirectoryBaseProyectos);
                }
                var filePath = _settings.Value.DirectoryBase + _settings.Value.DirectoryBaseProyectos + "/" + g.ToString() + form.Files[0].FileName;
                int id = Convert.ToInt32(form["id"]);

                usuario.Ip = HttpContext.Connection.RemoteIpAddress.ToString();
                usuario.Email = HttpContext.User.FindFirst("User").Value; ;
                if (form.Files[0].Length > 0)
                {

                    //error encoding 125 https://stackoverflow.com/questions/49215791/vs-code-c-sharp-system-notsupportedexception-no-data-is-available-for-encodin
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    var streamFile = new FileStream(filePath, FileMode.Create);
                    using (streamFile)
                    {
                        await form.Files[0].CopyToAsync(streamFile);
                        respuesta = await documentService.SetArchivo(streamFile.Name, filePath, ReadFully(streamFile), usuario);
                    }
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.ToString());
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }


    }
}
