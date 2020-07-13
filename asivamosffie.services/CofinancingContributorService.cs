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



//        //public async Task<ActionResult<List<Aportante>>> GetContributor()
//        //{
//        //    return await _context.Aportante.ToListAsync();
//        //}

//        //public async Task<Aportante> GetContributorById(int id)
//        //{
//        //    return await _context.Aportante.FindAsync(id);

//        //}
 

        // Grilla de control? { AportanteId }
        public async Task<ActionResult<List<CofinanciacionAportante>>> GetControlGrid(int ContributorId)
        {
            return await _context.CofinanciacionAportante.Where(x => x.CofinanciacionAportanteId == ContributorId).ToListAsync();
        }

//        //        if (result == null)
//        //        {
//        //            return null; // _reponse = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.RecursoNoEncontrado };
//        //        }

        //Registrar Aportante
        public async Task<Respuesta> Insert(CofinanciacionAportante CofnaAportante)
        {
            Respuesta _reponse = new Respuesta();
            int IdAccionCRegistrarAportante = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Acciones && x.Codigo.Equals(ConstantCodigoAcciones.RegistrarAportante)).Select(x => x.DominioId).First();

            try
            {
                if (CofnaAportante != null)
                {
                    //var AP = Helpers.Helpers.ConvertToUpercase(aportante);
                    _context.Add(CofnaAportante);
                    await _context.SaveChangesAsync();



                    return _reponse = new Respuesta
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

                    return _reponse = new Respuesta
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
                return _reponse = new Respuesta
                {
                    IsSuccessful = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesContributor.RecursoNoEncontrado,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.ErrorInterno, IdAccionCRegistrarAportante, CofnaAportante.UsuarioCreacion.ToString(), ex.InnerException.ToString()),

                };
            }

//        //    try
//        //    {
//        //        if (aportante != null)
//        //        {
//        //            //var AP = Helpers.Helpers.ConvertToUpercase(aportante);
//        //            _context.Add(aportante);
//        //            await _context.SaveChangesAsync();

                   

//        //            return _reponse = new Respuesta
//        //            {
//        //                IsSuccessful = true, IsValidation = false,
//        //                Data = aportante, Code = ConstantMessagesContributor.OperacionExitosa,
//        //                Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.OperacionExitosa, IdAccionCRegistrarAportante, aportante.UsuarioCreacion.ToString(), ConstantMessagesContributor.OperacionExitosa)
//        //            };
//        //        }
//        //        else
//        //        {
                   
//        //            return _reponse = new Respuesta
//        //            {
//        //                IsSuccessful = false,IsValidation = false,
//        //                Data = null, Code = ConstantMessagesContributor.RecursoNoEncontrado,
//        //                Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.OperacionExitosa, IdAccionCRegistrarAportante, aportante.UsuarioCreacion.ToString(), ConstantMessagesContributor.RecursoNoEncontrado)
//        //            };
//        //        }

//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return _reponse = new Respuesta
//        //        {
//        //            IsSuccessful = false, IsValidation = false,
//        //            Data = null,Code = ConstantMessagesContributor.RecursoNoEncontrado,
//        //            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.ErrorInterno, IdAccionCRegistrarAportante, aportante.UsuarioCreacion.ToString(), ex.InnerException.ToString()),
                    
//        //        };
//        //    }

//        //}


//        //Registrar Control de recursos
//        public async Task<Respuesta> ResourceControl(ControlRecurso controlRecurso)
//        {
//            Respuesta _reponse = new Respuesta();
//            try
//            {
//                var result = _context.ControlRecurso.Add(controlRecurso);
//                _context.Add(controlRecurso);
//                await _context.SaveChangesAsync();
//                _reponse = new Respuesta() { IsSuccessful = true, IsValidation = true, Data = controlRecurso, Code = ConstantMessagesContributor.OperacionExitosa };
//            }
//            catch (Exception ex)
//            {
//                _reponse = new Respuesta() { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.ErrorInterno, Message = ex.InnerException.ToString() };
//            }

//            return _reponse;
//        }

//        //Registrar Registros presupuestales
//        public async Task<Respuesta> BudgetRecords(RegistroPresupuestal registroPresupuestal)
//        {
//            //Pendiente : validaciones
//            Respuesta _reponse = new Respuesta();
//            try
//            {
//                //var result = _context.RegistroPresupuestal.Add(registroPresupuestal);
//                _context.Add(registroPresupuestal);
//                await _context.SaveChangesAsync();
//                _reponse = new Respuesta() { IsSuccessful = true, IsValidation = true, Data = registroPresupuestal, Code = ConstantMessagesContributor.OperacionExitosa };
//            }
//            catch (Exception ex)
//            {
//                _reponse = new Respuesta() { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.ErrorInterno, Message = ex.InnerException.ToString() };
//            }

//            return _reponse;
//        }

//        public Task<bool> Update(Respuesta aportante)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<bool> Delete(int id)
//        {
//            throw new NotImplementedException();
//        }

//    }
//}
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
    }
}
