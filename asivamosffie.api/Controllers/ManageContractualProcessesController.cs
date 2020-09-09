﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageContractualProcessesController : ControllerBase
    {
        public readonly IManageContractualProcessesService _manageContractualProcessesService;


        public ManageContractualProcessesController(IManageContractualProcessesService IManageContractualProcessesService)
        {
            _manageContractualProcessesService = IManageContractualProcessesService;
        }

        [Route("GetListSesionComiteSolicitud")]
        [HttpGet]
        public async Task<List<SesionComiteSolicitud>> GetListSesionComiteSolicitud()
        {
            var result = await _manageContractualProcessesService.GetListSesionComiteSolicitud();
            return result;
        }

        [Route("GetContratacionByContratacionId")]
        [HttpGet]
        public async Task<Contratacion> GetContratacionByContratacionId([FromQuery] int pContratacionId)
        {
            var result = await _manageContractualProcessesService.GetContratacionByContratacionId(pContratacionId);
            return result;
        }
         
    }
}
