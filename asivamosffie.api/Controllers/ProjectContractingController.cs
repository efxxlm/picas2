﻿using System;
using System.Collections.Generic;
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
    public class ProjectContractingController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IProjectContractingService _projectContractingService;
        private readonly IOptions<AppSettings> _settings;


        public ProjectContractingController(IDocumentService documentService, IOptions<AppSettings> settings, IProjectContractingService projectContractingService)
        {
            _projectContractingService = projectContractingService;
            _settings = settings;
            _documentService = documentService;
        }


        [Route("GetListProyectsByFilters")]
        [HttpGet]
        public async Task<List<ProyectoGrilla>> GetListProyectsByFilters(string pTipoIntervencion, string pLlaveMen, string pMunicipio, int pIdInstitucionEducativa, int pIdSede)
        {
            var respuesta = await _projectContractingService.GetListProyectsByFilters(pTipoIntervencion, pLlaveMen, pMunicipio, pIdInstitucionEducativa, pIdSede);
            return respuesta;
        }

        [Route("GetListContractingByFilters")]
        [HttpGet]
        public async Task<List<ContratistaGrilla>> GetListContractingByFilters(string pTipoIdentificacionCodigo, string pNumeroIdentidicacion, string pNombre, bool? EsConsorcio)
        {
            var respuesta = await _projectContractingService.GetListContractingByFilters(pTipoIdentificacionCodigo, pNumeroIdentidicacion, pNombre, EsConsorcio);
            return respuesta;
        }

        [Route("GetListContratacion")]
        [HttpGet]
        public async Task<List<Contratacion>> GetListContratacion()
        {
            var respuesta = await _projectContractingService.GetListContratacion();
            return respuesta;
        }

        [Route("CreateContratacionProyecto")]
        [HttpPost]
        public async Task<IActionResult> CreateContratacionProyecto(int[] idsProyectos, string tipoSolicitudCodigo)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                string pUsuarioModifico = " ";
                //string pUsuarioModifico = HttpContext.User.FindFirst("User").Value; 
                respuesta = await _projectContractingService.CreateContratacionProyecto(idsProyectos, tipoSolicitudCodigo, pUsuarioModifico);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return Ok(respuesta);
            }
        }

        [Route("GetListContratacionProyectoByContratacionId")]
        [HttpGet]
        public async Task<List<ContratacionProyecto>> GetListContratacionProyectoByContratacionId(int idContratacion)
        {
            var respuesta = await _projectContractingService.GetListContratacionProyectoByContratacionId(idContratacion);
            return respuesta;
        }


        [Route("CreateEditContratacion")]
        [HttpPost]
        public async Task<IActionResult> CreateEditContratacion(Contratacion pContratacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                string pUsuarioModifico = " ";
                //string pUsuarioModifico = HttpContext.User.FindFirst("User").Value; 
                pContratacion.UsuarioCreacion = pUsuarioModifico;
                respuesta = await _projectContractingService.CreateEditContratacion(pContratacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return Ok(respuesta);
            }
        }



        [Route("CreateEditContratacionProyecto")]
        [HttpPost]
        public async Task<IActionResult> CreateEditContratacionProyecto(ContratacionProyecto pContratacionProyecto)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                string pUsuarioModifico = " ";
                //string pUsuarioModifico = HttpContext.User.FindFirst("User").Value; 
                pContratacionProyecto.UsuarioCreacion = pUsuarioModifico;
                respuesta = await _projectContractingService.CreateEditContratacionProyecto(pContratacionProyecto);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return Ok(respuesta);
            }
        }
    }
}
