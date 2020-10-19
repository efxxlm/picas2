using asivamosffie.model.APIModels;
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
    public class CofinancingContributorService : ICofinancingContributorService
    {

        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;
         
        public CofinancingContributorService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<ActionResult<List<CofinanciacionAportante>>> GetContributor()
        {
            return await _context.CofinanciacionAportante.Where(ca => !(bool)ca.Eliminado).ToListAsync();
        }

        public async Task<CofinanciacionAportante> GetContributorById(int id)
        {
            return await _context.CofinanciacionAportante.FindAsync(id);
        }

        //public async Task<ActionResult<List<Aportante>>> GetListAportanteByTipoAportanteId(int pTipoAportanteID)
        //{
        //    List<CofinanciacionAportante> ListCofinanciacionAportante = await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado && r.TipoAportanteId == pTipoAportanteID).ToListAsync();

        //    List<CofinanicacionAportanteGrilla> ListCofinanicacionAportanteGrilla = new List<CofinanicacionAportanteGrilla>();

        //    foreach (var cofinanciacionAportante in ListCofinanciacionAportante)
        //    {
        //        //return await _context.DocumentoApropiacion.Include(x => x.Aportante).Where(x => x.AportanteId == ContributorId).ToListAsync();
        //        cofinanciacionAportante.CofinanciacionDocumento = await _context.CofinanciacionDocumento.Where(x => x.CofinanciacionAportanteId == cofinanciacionAportante.CofinanciacionAportanteId).ToListAsync();
        //    }

        //    return ListCofinanicacionAportanteGrilla;
        //}


        //        //public async Task<ActionResult<List<Aportante>>> GetContributor()
        //        //{
        //        //    return await _context.Aportante.ToListAsync();
        //        //}

        //        //public async Task<Aportante> GetContributorById(int id)
        //        //{
        //        //    return await _context.Aportante.FindAsync(id);

        //        //}

        public async Task<ActionResult<List<RegistroPresupuestal>>> GetRegisterBudget()
        {
            return await _context.RegistroPresupuestal.ToListAsync();
        }

        public async Task<RegistroPresupuestal> GetRegisterBudgetById(int id)
        {
            return await _context.RegistroPresupuestal.FindAsync(id);
        }
         
        // Grilla de control? { AportanteId }
        public async Task<ActionResult<List<CofinanciacionAportante>>> GetControlGrid(int ContributorId)
        {
            return await _context.CofinanciacionAportante.Where(x => x.CofinanciacionAportanteId == ContributorId).ToListAsync();
        }

        public async Task<Respuesta> Insert(CofinanciacionAportante CofnaAportante)
        { 
            int IdAccionCRegistrarAportante = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Acciones && x.Codigo.Equals(ConstantCodigoAcciones.RegistrarAportante)).Select(x => x.DominioId).First();

            try
            {
                if (CofnaAportante != null)
                {
                    //var AP = Helpers.Helpers.ConvertToUpercase(aportante);                   
                    _context.Add(CofnaAportante);
                    await _context.SaveChangesAsync();
                     
                    return   new Respuesta
                    {
                        IsSuccessful = true,
                        IsValidation = false,
                        Data = CofnaAportante,
                        Code = ConstantMessagesContributor.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.OperacionExitosa, IdAccionCRegistrarAportante, CofnaAportante.UsuarioCreacion.ToString(), ConstantMessagesContributor.OperacionExitosa)
                    };
                }
                else
                {

                    return   new Respuesta
                    {
                        IsSuccessful = false,
                        IsValidation = false,
                        Data = null,
                        Code = ConstantMessagesContributor.RecursoNoEncontrado,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.OperacionExitosa, IdAccionCRegistrarAportante, CofnaAportante.UsuarioCreacion.ToString(), ConstantMessagesContributor.RecursoNoEncontrado)
                    };
                }

            }
            catch (Exception ex)
            {
                return   new Respuesta
                {
                    IsSuccessful = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesContributor.RecursoNoEncontrado,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.ErrorInterno, IdAccionCRegistrarAportante, CofnaAportante.UsuarioCreacion.ToString(), ex.InnerException.ToString()),

                };
            }
        } 

        public async Task<Respuesta> Update(CofinanciacionAportante CofnaAportante)
        { 
            try
            {
                CofinanciacionAportante updateObj = await _context.CofinanciacionAportante.FindAsync(CofnaAportante.CofinanciacionAportanteId);

                updateObj.CofinanciacionId = CofnaAportante.CofinanciacionId;
                updateObj.TipoAportanteId = CofnaAportante.TipoAportanteId;
                updateObj.NombreAportanteId = CofnaAportante.NombreAportanteId;
                updateObj.MunicipioId = CofnaAportante.MunicipioId;
                updateObj.Eliminado = false;
                updateObj.FechaModificacion = DateTime.Now;
                 
                await _context.SaveChangesAsync();

                return new Respuesta { IsSuccessful = true, IsValidation = false, Data = updateObj, Code = ConstantMessagesRegisterBudget.EditadoCorrrectamente };
            }
            catch (Exception ex)
            {
                return new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesRegisterBudget.Error, Message = ex.Message };
            }
        } 
        //Registrar Registros presupuestales
        public async Task<Respuesta> CreateEditBudgetRecords(RegistroPresupuestal registroPresupuestal)
        { 
            try
            {
                registroPresupuestal.Eliminado = false;
                if (registroPresupuestal.RegistroPresupuestalId == 0)
                    await this.BudgetRecords(registroPresupuestal);
                else
                    await this.UpdateBudgetRegister(registroPresupuestal);

                await _context.SaveChangesAsync();
                return new Respuesta() { IsSuccessful = true, IsValidation = true, Data = registroPresupuestal, Code = ConstantMessagesContributor.OperacionExitosa };
            }
            catch (Exception ex)
            {
                return new Respuesta() { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.ErrorInterno, Message = ex.InnerException.ToString() };
            }
             
        }
         
        public async Task<Respuesta> BudgetRecords(RegistroPresupuestal registroPresupuestal)
        {
            try
            {
                registroPresupuestal.FechaCreacion = DateTime.Now;
                registroPresupuestal.Eliminado = false;
                await _context.RegistroPresupuestal.AddAsync(registroPresupuestal);
          
                return new Respuesta() { IsSuccessful = true, IsValidation = true, Data = registroPresupuestal, Code = ConstantMessagesContributor.OperacionExitosa };
            }
            catch (Exception ex)
            {
                return new Respuesta() { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.ErrorInterno, Message = ex.InnerException.ToString() };
            }
        }

        public async Task<Respuesta> UpdateBudgetRegister(RegistroPresupuestal registroPresupuestal)
        {
            try
            {
                RegistroPresupuestal updateObj = await _context.RegistroPresupuestal.FindAsync(registroPresupuestal.RegistroPresupuestalId);

                updateObj.AportanteId = registroPresupuestal.AportanteId;
                updateObj.NumeroRp = registroPresupuestal.NumeroRp;
                updateObj.FechaRp = registroPresupuestal.FechaRp;
  
                return new Respuesta { IsSuccessful = true, IsValidation = false, Data = updateObj, Code = ConstantMessagesRegisterBudget.EditadoCorrrectamente };
            }
            catch (Exception ex)
            {
                return new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesRegisterBudget.Error, Message = ex.Message };
            }
        }
    }
}
