﻿using System;
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

        [Route("CreateOrEditAdministrativeProject")]
        [HttpPost]
        public async Task<IActionResult> CreateOrEditAdministrativeProject([FromBody] ProyectoAdministrativo pProyectoAdministrativo)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                 
                string pUsuarioModifico = " ";
                //string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
                pProyectoAdministrativo.UsuarioCreacion = pUsuarioModifico;
                respuesta = await _projectService.CreateOrEditAdministrativeProject(pProyectoAdministrativo);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return Ok(respuesta);
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
            Respuesta respuesta = new Respuesta();
            try
            {
               

                string pUsuarioModifico = " ";
                //string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
                pProyecto.UsuarioCreacion = pUsuarioModifico;
                respuesta = await _projectService.CreateOrEditProyect(pProyecto);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return Ok(respuesta);
            }
        }


        [Route("ListProject")]
        [HttpGet]
        public async Task<List<ProyectoGrilla>> ListProjects()
        {

            string pUsuarioModifico = "";
            //string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
            var respuesta = await _projectService.ListProyectos(pUsuarioModifico);
            return respuesta;

        }


        [Route("GetProyectoByProyectoId")]
        [HttpGet]
        public async Task<Proyecto> GetProyectoByProyectoId(int pProyectoId)
        {
              
            var respuesta = await _projectService.GetProyectoByProyectoId(pProyectoId);
            return respuesta;

        }
    }
}
