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
  

        [Route("GetListContratoProyectos")]
        [HttpGet]
        public async Task<List<VistaContratoProyectos>> GetListContratoProyectos()
        {
            var respuesta = await _monitoringURLService.GetListContratoProyectos();
            return respuesta;
        }
        [HttpPost]
        [Route("EditarURLMonitoreo")]        
        public async Task<IActionResult> EditarURLMonitoreo(Int32 pProyectoId, string URLMonitoreo)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                string UsuarioModificacion= HttpContext.User.FindFirst("User").Value;                
                respuesta = await _monitoringURLService.EditarURLMonitoreo( pProyectoId,  URLMonitoreo,  UsuarioModificacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("VisitaURLMonitoreo")]
        public async Task<IActionResult> VisitaURLMonitoreo(string URLMonitoreo)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _monitoringURLService.VisitaURLMonitoreo(URLMonitoreo, UsuarioModificacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }
    }
}
