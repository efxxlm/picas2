using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services
{  
    public class ResourceControlService : IResourceControlService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public ResourceControlService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<List<ControlRecurso>> GetResourceControl()
        {
            return await _context.ControlRecurso.Where(r=> !(bool)r.Eliminado).ToListAsync();
        }

        public async Task<ControlRecurso> GetResourceControlById(int id)
        {
            return await _context.ControlRecurso.FindAsync(id);
        }
         
        public async Task<List<ControlRecurso>> GetResourceControlGridBySourceFunding(int id)
        {
     
            try
            {
                
                var ControlGrid = _context.ControlRecurso
                .Where( cr => cr.FuenteFinanciacionId == id && !(bool)cr.Eliminado)
                    .Include(RC => RC.FuenteFinanciacion)
                    .ThenInclude(FF => FF.Aportante)
                    .ThenInclude(APO => APO.Cofinanciacion)
                    .Include(RC => RC.CuentaBancaria)
                    .Include(RC => RC.RegistroPresupuestal)
                  ///  .Include(RC => RC.VigenciaAporte)
                    .Include(RC => RC.FuenteFinanciacion)
                    .Include(RC => RC.FuenteFinanciacion)
                    .ThenInclude(FF => FF.Aportante)
                    
                    .ToListAsync();

                return ControlGrid;

            }
            catch (Exception )
            {
                return null;
            }
        }
         
        public async Task<Respuesta> Insert(ControlRecurso controlRecurso)
        { 
            int idAccionCrearFuentesFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Recursos_Control, (int)EnumeratorTipoDominio.Acciones);
            try
            { 
                if (controlRecurso != null)
                {
                    controlRecurso.Eliminado = false;
                    if(controlRecurso.ControlRecursoId>0)
                    {
                        controlRecurso.FechaModificacion = DateTime.Now;
                        _context.Add(controlRecurso);
                        await _context.SaveChangesAsync();
                        return new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesResourceControl.OperacionExitosa,
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesResourceControl.OperacionExitosa, idAccionCrearFuentesFinanciacion, controlRecurso.UsuarioCreacion, "CREAR CONTROL RECURSO")
                        };
                    }
                    else                    
                    {
                        controlRecurso.FechaCreacion = DateTime.Now;
                        _context.Add(controlRecurso);
                        await _context.SaveChangesAsync();
                        return new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesResourceControl.OperacionExitosa,
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesResourceControl.OperacionExitosa, idAccionCrearFuentesFinanciacion, controlRecurso.UsuarioCreacion, "CREAR CONTROL RECURSO")
                        };
                    }                                        
                }
                else
                {                    
                    return new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesResourceControl.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesResourceControl.Error, idAccionCrearFuentesFinanciacion, controlRecurso.UsuarioCreacion, "ERROR CREAR CONTROL")
                    };
                }

            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesResourceControl.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesResourceControl.OperacionExitosa, idAccionCrearFuentesFinanciacion, controlRecurso.UsuarioCreacion, ex.InnerException.ToString().Substring(0,500))
                };                
            }
        }

        public async Task<Respuesta> Update(ControlRecurso controlRecurso)
        { 
            int idAccionCrearFuentesFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Recursos_Control, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                ControlRecurso updateObj = await _context.ControlRecurso.FindAsync(controlRecurso.ControlRecursoId);

                updateObj.FuenteFinanciacionId = controlRecurso.FuenteFinanciacionId;
                updateObj.CuentaBancariaId = controlRecurso.CuentaBancariaId;
                updateObj.RegistroPresupuestalId = controlRecurso.RegistroPresupuestalId;
                updateObj.VigenciaAporteId = controlRecurso.VigenciaAporteId;
                updateObj.FechaConsignacion = controlRecurso.FechaConsignacion;
                updateObj.ValorConsignacion = controlRecurso.ValorConsignacion;

                _context.Update(updateObj);

                await _context.SaveChangesAsync();
                
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesResourceControl.EditadoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesResourceControl.EditadoCorrrectamente, idAccionCrearFuentesFinanciacion, controlRecurso.UsuarioModificacion, "EDITAR FUENTES DE FINANCIACIÓN")
                };
            }
            catch (Exception ex)
            {                
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesResourceControl.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesResourceControl.EditadoCorrrectamente, idAccionCrearFuentesFinanciacion, controlRecurso.UsuarioModificacion, ex.Message)
                };
            }
        }

        public async Task<Respuesta> Delete(int id, string pUsuario)
        {  
            int idAccionCrearFuentesFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Recursos_Control, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                ControlRecurso updateObj = await _context.ControlRecurso.FindAsync(id);

                updateObj.Eliminado = true;
                updateObj.UsuarioModificacion = pUsuario;
                updateObj.FechaModificacion = DateTime.Now;

                _context.Update(updateObj);
                await _context.SaveChangesAsync();
                 
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesResourceControl.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesResourceControl.EliminadoExitosamente, idAccionCrearFuentesFinanciacion, pUsuario, "Eliminado")
                };
               
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesResourceControl.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesResourceControl.Error, idAccionCrearFuentesFinanciacion, pUsuario, ex.Message)
                }; 
            }
        }

    }
}
