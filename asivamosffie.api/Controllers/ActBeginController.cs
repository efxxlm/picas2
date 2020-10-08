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
    public class ActBeginController : ControllerBase
    {
        public readonly IActBeginService _ActBegin;
        public ActBeginController(IActBeginService actBegin)
        {
            _ActBegin = actBegin;
        }

        //public async Task<ActionResult<VistaGenerarActaInicioContrato>> GetListVistaGenerarActaInicio(int pContratoId)
        [Route("GetVistaGenerarActaInicio")]
        [HttpGet]        
        public async Task<ActionResult<VistaGenerarActaInicioContrato>> GetVistaGenerarActaInicio(int pContratoId)
        {
            try
            {
                return await _ActBegin.GetListVistaGenerarActaInicio(pContratoId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        
            
        [HttpGet]
        [Route("GetPlantillaActaInicio")]
        public async Task<FileResult> GetPlantillaActaInicio(int pContratoId)
        {
            return File(await _ActBegin.GetPlantillaActaInicio(pContratoId), "application/pdf");
        }


    }
   
}
