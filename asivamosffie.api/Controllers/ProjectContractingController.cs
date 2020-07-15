using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
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

    }
}
