using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.api.Controllers;
using AuthorizationTest.JwtHelpers;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using Newtonsoft.Json;
using System.Security.Claims;

namespace asivamosffie.services
{
    public class ValidacionesLineaPagoServices : IValidacionesLineaPagoServices
    {
        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;

        public ValidacionesLineaPagoServices(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }
         


        public async Task<dynamic> ValidacionFacturadosODG()
        { 
            return  _context.VOdgValoresFacturados.ToList(); 
        }

    }
}
