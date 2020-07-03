using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services
{
    public class SourceFundingService : ISourceFundingService
    {
        private readonly ICommonService _commonService;
        private readonly IContributorService _contributor;
        private readonly devAsiVamosFFIEContext _context;

        public SourceFundingService(devAsiVamosFFIEContext context, ICommonService commonService, IContributorService contributor)
        {
            _context = context;
            _commonService = commonService;
            _contributor = contributor;
        }

        public async Task<List<FuenteFinanciacion>> GetISourceFunding()
        {
            return await _context.FuenteFinanciacion.ToListAsync();
        }

        public async Task<FuenteFinanciacion> GetISourceFundingById(int id)
        {
            return await _context.FuenteFinanciacion.FindAsync(id);
        }


        public async Task<Respuesta> Insert(FuenteFinanciacion fuentefinanciacion)
        {
            Respuesta respuesta = new Respuesta();
            var contributor = await _context.FuenteFinanciacion.Where(r => r.AportanteId == fuentefinanciacion.AportanteId).ToListAsync();
            //Pendiente, No mostrar la fuente que ya fue utilizada
            if (contributor != null)
            { 
            
            }

            Console.WriteLine(contributor);
         
            try
            {
                if (fuentefinanciacion != null)
                {
                    _context.Add(fuentefinanciacion);
                    await _context.SaveChangesAsync();
                    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Data = fuentefinanciacion, Code = ConstantMessagesContributor.OperacionExitosa };
                }
                else
                {
                    respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesContributor.CamposIncompletos };
                }

            }
            catch (Exception ex)
            {
                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.ErrorInterno, Message = ex.InnerException.ToString() };
            }
            return respuesta;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }


        public Task<bool> Update(FuenteFinanciacion fuentefinanciacion)
        {
            throw new NotImplementedException();
        }
    }
}
