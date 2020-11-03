using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.api.Controllers
{
    public class RegisterPersonalProgrammingController : Controller
    {
        public readonly IRegisterPersonalProgrammingService _IRegisterPersonalProgrammingService;


        public RegisterPersonalProgrammingController(IRegisterPersonalProgrammingService registerPersonalProgrammingService) {

           _IRegisterPersonalProgrammingService = registerPersonalProgrammingService;
        }


    }
}
