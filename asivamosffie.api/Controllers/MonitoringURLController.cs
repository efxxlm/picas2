using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitoringURLController : ControllerBase
    {

        private readonly IDocumentService _documentService;
        private readonly IMonitoringURL _monitoringURLService;
        

        public MonitoringURLController(IDocumentService documentService,  IMonitoringURL monitoringURLService)
        {
            _monitoringURLService = monitoringURLService;
            
            _documentService = documentService;
        }

        [Route("GetListProyects")]
        [HttpGet]
        public async Task<List<ProyectoGrilla>> GetListProyects()
        {
            var respuesta = await _monitoringURLService.GetListProyects();
            return respuesta;
        }

        [Route("GetListContratoProyectos")]
        [HttpGet]
        public async Task<List<VistaContratoProyectos>> GetListContratoProyectos()
        {
            var respuesta = await _monitoringURLService.GetListContratoProyectos();
            return respuesta;
        }

        


    }
}
