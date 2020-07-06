﻿using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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


        // Grilla de control? { AportanteId }
        public async Task<Aportante> GetControlGrid(int ContributorId)
        {
            try
            {
                return await _context.Aportante.Where(s => s.AportanteId == ContributorId)
                         .Include(s => s.FuenteFinanciacion)
                             .ThenInclude(g => g.ControlRecurso) // (ThenInclude) Para cargar varios niveles de entidades  relacionadas
                         .FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }



        public async Task<Respuesta> Insert(Aportante aportante)
        {
            Respuesta _reponse = new Respuesta();
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
            Respuesta _reponse = new Respuesta();
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
            Respuesta _reponse = new Respuesta();
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
