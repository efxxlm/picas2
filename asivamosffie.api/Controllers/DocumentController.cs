using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using Microsoft.Extensions.Options;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using System.Text;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {

        private readonly IDocumentService _documentService;
        private readonly IOptions<AppSettings> _settings;

        public DocumentController(IDocumentService documentService, IOptions<AppSettings> settings)
        {
            _settings = settings;
            _documentService = documentService;
        } 


    }
}
