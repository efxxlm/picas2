using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using System;
using System.Linq;
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

  
  
    }
}
