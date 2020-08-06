using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq; 
using Microsoft.EntityFrameworkCore;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.api.Controllers;
using AuthorizationTest.JwtHelpers; 
using System.IO;
using System.Text;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Globalization;
using asivamosffie.services.Validators;
using asivamosffie.services.Filters;
using System.Data.Common;
using Z.EntityFramework.Plus;
using System.Threading.Tasks;

namespace asivamosffie.services
{
    public class RegisterSessionTechnicalCommitteeService : IRegisterSessionTechnicalCommitteeService
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public RegisterSessionTechnicalCommitteeService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService; 
            _context = context;
        }


        public async Task<dynamic> GetListSolicitudesContractuales()
        { 
            dynamic dynamicList = await _context.ComiteTecnico.Where(r => !(bool)r.Eliminado).Select(x => new {
                x.ComiteTecnicoId,
                FechaSolicitud  = x.FechaCreacion.ToString(),
                TipoSolicitud = x.TipoSolicitudCodigo,
                x.NumeroSolicitud
            }).Distinct().OrderByDescending(r=> r.ComiteTecnicoId).ToListAsync();

            foreach (var item in dynamicList)
            {
                item.TipoSolicitud2 = _commonService.GetNombreDominioByCodigoAndTipoDominio(item.TipoSolicitud, (int)EnumeratorTipoDominio.Tipo_Solicitud);

            } 
            return dynamicList;
        } 
    }
}
