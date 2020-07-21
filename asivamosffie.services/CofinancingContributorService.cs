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
            return await _context.CofinanciacionAportante.ToListAsync();
        }

        public async Task<CofinanciacionAportante> GetContributorById(int id)
        {
            return await _context.CofinanciacionAportante.FindAsync(id);
        }


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
            Respuesta _reponse = new Respuesta();
            //Error:  //int IdAccionCRegistrarAportante = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Acciones && x.Codigo.Equals(ConstantCodigoAcciones.RegistrarAportante)).Select(x => x.DominioId).First();

            try
            {
                if (CofnaAportante != null)
                {
                    //var AP = Helpers.Helpers.ConvertToUpercase(aportante);
                    CofnaAportante.FechaCreacion = DateTime.Now;
                    CofnaAportante.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                    _context.Add(CofnaAportante);
                    await _context.SaveChangesAsync();



                    return _reponse = new Respuesta
                    {
                        IsSuccessful = true,
                        IsValidation = false,
                        Data = CofnaAportante,
                        Code = ConstantMessagesContributor.OperacionExitosa,
                        //Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.OperacionExitosa, IdAccionCRegistrarAportante, CofnaAportante.UsuarioCreacion.ToString(), ConstantMessagesContributor.OperacionExitosa)
                    };
                }
                else
                {

                    return _reponse = new Respuesta
                    {
                        IsSuccessful = false,
                        IsValidation = false,
                        Data = null,
                        Code = ConstantMessagesContributor.RecursoNoEncontrado,
                        //Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.OperacionExitosa, IdAccionCRegistrarAportante, CofnaAportante.UsuarioCreacion.ToString(), ConstantMessagesContributor.RecursoNoEncontrado)
                    };
                }

            }
            catch (Exception ex)
            {
                return _reponse = new Respuesta
                {
                    IsSuccessful = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesContributor.RecursoNoEncontrado,
                    //Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.ErrorInterno, IdAccionCRegistrarAportante, CofnaAportante.UsuarioCreacion.ToString(), ex.InnerException.ToString()),

                };
            }
        }


        public async Task<Respuesta> Update(CofinanciacionAportante CofnaAportante)
        {
            Respuesta _response = new Respuesta();

            try
            {
                CofinanciacionAportante updateObj = await _context.CofinanciacionAportante.FindAsync(CofnaAportante.CofinanciacionAportanteId);

                updateObj.CofinanciacionId = CofnaAportante.CofinanciacionId;
                updateObj.TipoAportanteId = CofnaAportante.TipoAportanteId;
                updateObj.NombreAportanteId = CofnaAportante.NombreAportanteId;
                updateObj.MunicipioId = CofnaAportante.MunicipioId;
                updateObj.Eliminado = false;
                updateObj.FechaModificacion = DateTime.Now;
                updateObj.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;

                _context.Update(updateObj);
                await _context.SaveChangesAsync();

                return _response = new Respuesta { IsSuccessful = true, IsValidation = false, Data = updateObj, Code = ConstantMessagesRegisterBudget.EditadoCorrrectamente };
            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesRegisterBudget.Error, Message = ex.Message };
            }
        }



        //Registrar Registros presupuestales
        public async Task<Respuesta> CreateEditBudgetRecords(RegistroPresupuestal registroPresupuestal)
        {
            Respuesta _reponse = new Respuesta();
            try
            {
                if (registroPresupuestal.RegistroPresupuestalId == null || registroPresupuestal.RegistroPresupuestalId == 0)
                    await this.BudgetRecords(registroPresupuestal);
                else
                    await this.UpdateBudgetRegister(registroPresupuestal);

                await _context.SaveChangesAsync();
                _reponse = new Respuesta() { IsSuccessful = true, IsValidation = true, Data = registroPresupuestal, Code = ConstantMessagesContributor.OperacionExitosa };
            }
            catch (Exception ex)
            {
                _reponse = new Respuesta() { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.ErrorInterno, Message = ex.InnerException.ToString() };
            }

            return _reponse;
        }


        public async Task<Respuesta> BudgetRecords(RegistroPresupuestal registroPresupuestal)
        {
            Respuesta _reponse = new Respuesta();
            try
            {
                registroPresupuestal.FechaCreacion = DateTime.Now;
                registroPresupuestal.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                _context.Add(registroPresupuestal);
                //await _context.SaveChangesAsync();
                _reponse = new Respuesta() { IsSuccessful = true, IsValidation = true, Data = registroPresupuestal, Code = ConstantMessagesContributor.OperacionExitosa };
            }
            catch (Exception ex)
            {
                _reponse = new Respuesta() { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.ErrorInterno, Message = ex.InnerException.ToString() };
            }

            return _reponse;
        }

        public async Task<Respuesta> UpdateBudgetRegister(RegistroPresupuestal registroPresupuestal)
        {
            Respuesta _response = new Respuesta();

            try
            {
                RegistroPresupuestal updateObj = await _context.RegistroPresupuestal.FindAsync(registroPresupuestal.RegistroPresupuestalId);

                updateObj.AportanteId = registroPresupuestal.AportanteId;
                updateObj.NumeroRp = registroPresupuestal.NumeroRp;
                updateObj.FechaRp = registroPresupuestal.FechaRp;
                _context.Update(updateObj);
                //await _context.SaveChangesAsync();

                return _response = new Respuesta { IsSuccessful = true, IsValidation = false, Data = updateObj, Code = ConstantMessagesRegisterBudget.EditadoCorrrectamente };
            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesRegisterBudget.Error, Message = ex.Message };
            }
        }
    }
}
