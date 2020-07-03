using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services
{
    public class ContributorService: IContributorService
    {

        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public ContributorService(devAsiVamosFFIEContext context, ICommonService commonService)
        {

            _commonService = commonService;
            _context = context;
        }




        public async Task<List<Aportante>> GetContributor()
        {
            return await _context.Aportante.ToListAsync();
        }

        public async Task<Aportante> GetContributorById(int id)
        {
            return await _context.Aportante.FindAsync(id);
        }

        public async Task<Respuesta> Insert(Aportante aportante)
        {
            Respuesta respuesta = new Respuesta();
            aportante.NombreCodigo = Helpers.Helpers.CleanStringInput(aportante.NombreCodigo);
            aportante.UsuarioCreacion = Helpers.Helpers.CleanStringInput(aportante.UsuarioCreacion);

            try
            {
                if (aportante != null)
                {
                    _context.Add(aportante);
                    await _context.SaveChangesAsync();
                    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Data = aportante, Code = ConstantMessagesContributor.OperacionExitosa };
                }
                else
                {
                    respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code =  ConstantMessagesContributor.CamposIncompletos };
                }
               
            }
            catch (Exception ex)
            {
                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.ErrorInterno, Message = ex.InnerException.ToString() };
            }
            return respuesta;
        }


        public Task<bool> Update(Respuesta aportante)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

    }
}
