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
        private readonly ICofinancingContributorService _contributor;
        private readonly devAsiVamosFFIEContext _context;

        public SourceFundingService(devAsiVamosFFIEContext context, ICommonService commonService, ICofinancingContributorService contributor)
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
            //return await _context.FuenteFinanciacion.Where(r => r.AportanteId == id).ToListAsync();
        }


        public async Task<Respuesta> Insert(FuenteFinanciacion fuentefinanciacion)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                if (fuentefinanciacion != null)
                {
                    fuentefinanciacion.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                    fuentefinanciacion.FechaCreacion = DateTime.Now;
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


        public async Task<bool> Delete(int id)
        {
            try
            {
                var entity = await _context.FuenteFinanciacion.FindAsync(id);

                _context.FuenteFinanciacion.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<Respuesta> Update(FuenteFinanciacion fuentefinanciacion)
        {
            Respuesta _response = new Respuesta();

            try
            {
                FuenteFinanciacion updateObj = await _context.FuenteFinanciacion.FindAsync(fuentefinanciacion.FuenteFinanciacionId);

                updateObj.AportanteId = fuentefinanciacion.AportanteId;
                updateObj.FuenteRecursosCodigo = fuentefinanciacion.FuenteRecursosCodigo;
                updateObj.ValorFuente = fuentefinanciacion.ValorFuente;

                _context.Update(updateObj);
                await _context.SaveChangesAsync();

                return _response = new Respuesta { IsSuccessful = true, IsValidation = false, Data = updateObj, Code = ConstantMessagesSourceFunding.EditadoCorrrectamente };
            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesSourceFunding.Error, Message = ex.Message };
            }
        }
    }
}
