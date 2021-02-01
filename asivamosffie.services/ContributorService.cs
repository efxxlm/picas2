﻿using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asivamosffie.services
{
    public class ContributorService : IContributorService
    {

        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;


        public ContributorService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }



 





        // Grilla de control? { AportanteId }
     

        //Registrar Aportante
     
        //Registrar Control de recursos
        public async Task<Respuesta> ResourceControl(ControlRecurso controlRecurso)
        {
            Respuesta _reponse = new Respuesta();
            try
            {
                var result = _context.ControlRecurso.Add(controlRecurso);
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
            Respuesta _reponse = new Respuesta();
            try
            {
                //var result = _context.RegistroPresupuestal.Add(registroPresupuestal);
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

        public Task<ActionResult<List<CofinanciacionAportante>>> GetContributor()
        {
            throw new NotImplementedException();
        }

        public Task<CofinanciacionAportante> GetContributorById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<List<CofinanciacionAportante>>> GetControlGrid(int ContributorId)
        {
            throw new NotImplementedException();
        }

        public Task<Respuesta> Insert(CofinanciacionAportante CofnaAportante)
        {
            throw new NotImplementedException();
        }

        public Task<Respuesta> Update(CofinanciacionAportante CofnaAportante)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<List<RegistroPresupuestal>>> GetRegisterBudget()
        {
            throw new NotImplementedException();
        }

        public Task<RegistroPresupuestal> GetRegisterBudgetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Respuesta> UpdateBudgetRegister(RegistroPresupuestal registroPresupuestal)
        {
            throw new NotImplementedException();
        }

        public Task<Respuesta> CreateEditBudgetRecords(RegistroPresupuestal registroPresupuestal)
        {
            throw new NotImplementedException();
        }
    }
}
