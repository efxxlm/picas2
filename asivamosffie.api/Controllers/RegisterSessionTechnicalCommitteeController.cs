using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterSessionTechnicalCommitteeController : ControllerBase
    {
        public readonly IRegisterSessionTechnicalCommitteeService _registerSessionTechnicalCommitteeService;

        public RegisterSessionTechnicalCommitteeController(IRegisterSessionTechnicalCommitteeService registerSessionTechnicalCommitteeService) {

            _registerSessionTechnicalCommitteeService = registerSessionTechnicalCommitteeService; 
        }

    
        [HttpGet]
        [Route("GetListSolicitudesContractuales")] 
        public async Task<dynamic> GetListSolicitudesContractuales() {
            return await _registerSessionTechnicalCommitteeService.GetListSolicitudesContractuales(); 
        }


    }
}
