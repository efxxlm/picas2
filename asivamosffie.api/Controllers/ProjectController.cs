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
using asivamosffie.model.APIModels;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IProjectService _projectService;
        private readonly IOptions<AppSettings> _settings;


        public ProjectController(IDocumentService documentService, IOptions<AppSettings> settings, IProjectService projectService)
        {
            _projectService = projectService;
            _settings = settings;
            _documentService = documentService;
        }

        [HttpGet]
        [Route("GetProyectoGrillaByProyectoId")]
        public async Task<ProyectoGrilla> GetProyectoGrillaByProyectoId(int idProyecto)
        {
            var result = await _projectService.GetProyectoGrillaByProyectoId(idProyecto);
            return result;
        }


        [HttpGet]
        [Route("GetProyectoGrillaByProyecto")]
        public async Task<ProyectoGrilla> GetProyectoGrillaByProyecto(Proyecto pProyecto)
        {
            var result = await _projectService.GetProyectoGrillaByProyecto(pProyecto);
            return result;
        }


        [Route("CreateOrEditAdministrativeProject")]
        [HttpPost]
        public async Task<IActionResult> CreateOrEditAdministrativeProject([FromBody] ProyectoAdministrativo pProyectoAdministrativo)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //string pUsuarioModifico = " ";
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
                pProyectoAdministrativo.UsuarioCreacion = pUsuarioModifico;
                respuesta = await _projectService.CreateOrEditAdministrativeProject(pProyectoAdministrativo);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
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
                    //string strUsuario = "";
                    string strUsuario =HttpContext.User.FindFirst("User").Value.ToUpper();
                    respuesta = await _projectService.SetValidateCargueMasivo(file, Path.Combine(_settings.Value.DirectoryBase, _settings.Value.DirectoryBaseCargue, _settings.Value.DirectoryBaseProyectos), strUsuario);
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
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
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
            Respuesta respuesta = new Respuesta();
            try
            {
                pProyecto.UsuarioCreacion = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _projectService.CreateOrEditProyect(pProyecto);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return Ok(respuesta);
            }
        }





        [Route("ListAdministrativeProject")]
        [HttpGet]
        public async Task<List<ProyectoAdministracionGrilla>> ListAdministrativeProjects()
        {
            string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
            var respuesta = await _projectService.ListAdministrativeProyectos(pUsuarioModifico);
            return respuesta;

        }

        [Route("DeleteProyectoAdministrativoByProyectoId")]
        [HttpGet]
        public async Task<bool> DeleteProyectoAdministrativoByProyectoId(int pProyectoId)
        {
            string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
            var respuesta = await _projectService.DeleteProyectoAdministrativoByProyectoId(pProyectoId, pUsuarioModifico);
            return respuesta;
        }



        [Route("EnviarProyectoAdministrativoByProyectoId")]
        [HttpGet]
        public async Task<bool> EnviarProyectoAdministrativoByProyectoId(int pProyectoId)
        {
            string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
            var respuesta = await _projectService.EnviarProyectoAdministrativoByProyectoId(pProyectoId, pUsuarioModifico,_settings.Value.DominioFront
                , _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
            return respuesta;
        }

        [Route("ListProject")]
        [HttpGet]
        public async Task<List<ProyectoGrilla>> ListProjects()
        {
            var respuesta = await _projectService.ListProyectos();
            return respuesta;

        }


        [Route("GetProyectoByProyectoId")]
        [HttpGet]
        public async Task<Proyecto> GetProyectoByProyectoId(int pProyectoId)
        {
            var respuesta = await _projectService.GetProyectoByProyectoId(pProyectoId);
            return respuesta;
        }

        [Route("DeleteProyectoByProyectoId")]
        [HttpGet]
        public async Task<bool> DeleteProyectoByProyectoId(int pProyectoId)
        {
            string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
            var respuesta = await _projectService.DeleteProyectoByProyectoId(pProyectoId,pUsuarioModifico);
            return respuesta;
        }

        [Route("GetFontsByAportantID")]
        [HttpGet]
        public async Task<List<FuenteFinanciacion>> GetFontsByAportantId(int pAportanteId)
        {
            var respuesta = await _projectService.GetFontsByAportantId(pAportanteId);
            return respuesta;
        }

        [Route("deleteFontByID")]
        [HttpPost]
        public async Task<bool> deleteFontByID(int pId)
        {
            string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
            var respuesta = await _projectService.deleteFontByID(pId, pUsuarioModifico);
            return respuesta;
        }
        [Route("deletePredioByID")]
        [HttpPost]
        public async Task<bool> deletePredioByID(int pId)
        {
            string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
            var respuesta = await _projectService.deletePredioByID(pId, pUsuarioModifico);
            return respuesta;
        }
        [Route("deleteAportantesByID")]
        [HttpPost]
        public async Task<bool> deleteAportantesByID(int pId)
        {
            string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
            var respuesta = await _projectService.deleteAportantesByID(pId, pUsuarioModifico);
            return respuesta;
        }
        [Route("deleteInfraestructuraByID")]
        [HttpPost]
        public async Task<bool> deleteInfraestructuraByID(int pId)
        {
            string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
            var respuesta = await _projectService.deleteInfraestructuraByID(pId, pUsuarioModifico);
            return respuesta;
        }
    }
}
