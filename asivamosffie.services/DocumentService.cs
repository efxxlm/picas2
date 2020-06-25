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


namespace asivamosffie.services
{
   public class DocumentService : IDocumentService
    {
       
        private readonly devAsiVamosFFIEContext _context;

        public DocumentService(devAsiVamosFFIEContext context)
        {
            _context = context;
        }
 

    }
}
