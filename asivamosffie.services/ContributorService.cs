using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services
{
    public class ContributorService: IContributorService
    {

        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;
        private Respuesta _reponse = new  Respuesta();

        public ContributorService(devAsiVamosFFIEContext context, ICommonService commonService, Respuesta reponse)
        {

            _commonService = commonService;
            _context = context;
            _reponse = reponse;
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

            try
            {
                if (aportante != null)
                {
                    _context.Add(aportante);
                    await _context.SaveChangesAsync();
                    _reponse = new Respuesta { IsSuccessful = true, IsValidation = true, Data = aportante, Code = ConstantMessagesContributor.OperacionExitosa };
                }
                else
                {
                    _reponse = new Respuesta { IsSuccessful = false, IsValidation = false, Code =  ConstantMessagesContributor.CamposIncompletos };
                }
               
            }
            catch (Exception ex)
            {
                _reponse = new Respuesta() { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.ErrorInterno, Message = ex.InnerException.ToString() };
            }
            return _reponse;
        }


        //Registrar Control de recursos
        public async Task<Respuesta> ResourceControl(ControlRecurso controlRecurso)
        {
            try
            {
                var result =  _context.ControlRecurso.Add(controlRecurso);
                _context.Add(controlRecurso);
                await _context.SaveChangesAsync();
                _reponse = new Respuesta() { IsSuccessful = true, IsValidation = true, Data = controlRecurso, Code = ConstantMessagesContributor.OperacionExitosa };
            }
            catch (Exception ex)
            {
                _reponse = new Respuesta() { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.ErrorInterno, Message = ex.InnerException.ToString() };
            }

            return _reponse;
        }

        //Registrar Registros presupuestales
        public async Task<Respuesta> BudgetRecords(RegistroPresupuestal registroPresupuestal)
        {
            //Pendiente : validaciones
            try
            {
                var result = _context.RegistroPresupuestal.Add(registroPresupuestal);
                _context.Add(registroPresupuestal);
                await _context.SaveChangesAsync();
                _reponse = new Respuesta() { IsSuccessful = true, IsValidation = true, Data = registroPresupuestal, Code = ConstantMessagesContributor.OperacionExitosa };
            }
            catch (Exception ex)
            {
                _reponse = new Respuesta() { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.ErrorInterno, Message = ex.InnerException.ToString() };
            }

            return _reponse;
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
