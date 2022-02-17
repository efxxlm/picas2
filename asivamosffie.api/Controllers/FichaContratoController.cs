using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace asivamosffie.api.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
 
    public class FichaContratoController : ControllerBase
    { 
        private readonly IFichaContratoService  _fichaContratoService;


        public FichaContratoController(IFichaContratoService fichaContratoService)
        {
            _fichaContratoService  = fichaContratoService ; 
        }
         




    }
}
