using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicalRequirementsConstructionPhaseController : ControllerBase
    {
        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirementsConstructionPhaseService;
        private readonly IOptions<AppSettings> _settings;
        private readonly IConverter _converter;


        public TechnicalRequirementsConstructionPhaseController(IOptions<AppSettings> settings, IConverter converter, ITechnicalRequirementsConstructionPhaseService technicalRequirementsConstructionPhaseService)
        {
            _technicalRequirementsConstructionPhaseService = technicalRequirementsConstructionPhaseService;
            _settings = settings;
            _converter = converter;

        }

        [Route("GetContractsGrid")]
        [HttpGet]
        public async Task<List<dynamic>> GetContractsGrid( int pUsuarioId )
        {
            try
            {
                return await _technicalRequirementsConstructionPhaseService.GetContractsGrid( pUsuarioId );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetContratoByContratoId")]
        [HttpGet]
        public async Task<Contrato> GetContratoByContratoId(int pContratoId)
        {
            try
            {
                return await _technicalRequirementsConstructionPhaseService.GetContratoByContratoId( pContratoId );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("CreateEditDiagnostico")]
        [HttpPost]
        public async Task<Respuesta> CreateEditDiagnostico(ContratoConstruccion pConstruccion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pConstruccion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _technicalRequirementsConstructionPhaseService.CreateEditDiagnostico(pConstruccion);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("CreateEditPlanesProgramas")]
        [HttpPost]
        public async Task<Respuesta> CreateEditPlanesProgramas(ContratoConstruccion pConstruccion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pConstruccion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _technicalRequirementsConstructionPhaseService.CreateEditPlanesProgramas(pConstruccion);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("CreateEditManejoAnticipo")]
        [HttpPost]
        public async Task<Respuesta> CreateEditManejoAnticipo(ContratoConstruccion pConstruccion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pConstruccion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _technicalRequirementsConstructionPhaseService.CreateEditManejoAnticipo(pConstruccion);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

    }
}
