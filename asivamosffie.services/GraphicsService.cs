using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.api;
using asivamosffie.services.Helpers.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Globalization;
 

namespace asivamosffie.services
{
    public class GraphicsService
    {

        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;
        private readonly IOptions<AppSettingsService> _settings;
        private readonly IDocumentService _documentService;
        public readonly IConverter _converter;

        public GraphicsService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;

        }
 

    }
}
