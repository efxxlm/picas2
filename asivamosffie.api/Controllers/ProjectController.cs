using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {

        private readonly IDocumentService _documentService; 
        private readonly IProjectService _projectService;
        private readonly IOptions<AppSettings> _settings;


        public ProjectController(IDocumentService documentService, IOptions<AppSettings> settings , IProjectService projectService)
        {
            _projectService = projectService;
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
                    //string strUsuario = HttpContext.User.FindFirst("User").Value;
                    respuesta = await _projectService.SetValidateCargueMasivo(file, Path.Combine(_settings.Value.DirectoryBase, _settings.Value.DirectoryBaseCargue, _settings.Value.DirectoryBaseProyectos), " ");
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        [Route("UploadMassiveLoadProjects")]
        [HttpPost]
        public async Task<IActionResult> UploadMassiveLoadProjects([FromQuery] string pIdDocument)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = "";
                //string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
                respuesta = await _projectService.UploadMassiveLoadProjects(pIdDocument, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        [Route("CreateOrUpdateProyect")]
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateProyect([FromBody] Proyecto pProyecto)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = "";
                //string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
               // respuesta = await _projectService.UploadMassiveLoadProjects(pProyecto, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
