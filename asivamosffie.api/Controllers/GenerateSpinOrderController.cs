using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using asivamosffie.model.APIModels;
using System.IO;
using Microsoft.Extensions.Options;
using System.Reflection;
using Newtonsoft.Json;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateSpinOrderController : Controller
    {
        public readonly IGenerateSpinOrderService _generateSpinOrderService;
        private readonly IOptions<AppSettings> _settings;

        public GenerateSpinOrderController(IGenerateSpinOrderService _GenerateSpinOrderService, IOptions<AppSettings> settings)
        {
            _generateSpinOrderService = _GenerateSpinOrderService;
            _settings = settings;
        }
         
        [Route("GetSolicitudPagoBySolicitudPagoId")]
        [HttpGet]
        public async Task<SolicitudPago> GetSolicitudPagoBySolicitudPagoId([FromQuery] int SolicitudPagoId)
        { 
            try
            {
                return  await _generateSpinOrderService.GetSolicitudPagoBySolicitudPagoId(SolicitudPagoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 
        [Route("GetListSolicitudPago")]
        [HttpGet]
        public async Task<dynamic> GetListSolicitudPago()
        {
            var result = await _generateSpinOrderService.GetListSolicitudPago();
            return result;
        }

    }
}
