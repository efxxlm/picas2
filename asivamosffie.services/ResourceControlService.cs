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
                int AportanteId = _context.FuenteFinanciacion.Where(r => r.FuenteFinanciacionId == id).FirstOrDefault().AportanteId;

                List<ControlRecurso> ControlGrid = (from cr in _context.ControlRecurso
                                                    join ff in _context.FuenteFinanciacion on cr.FuenteFinanciacionId equals ff.FuenteFinanciacionId
                                                    join a  in _context.CofinanciacionAportante on ff.AportanteId equals a.CofinanciacionAportanteId
                                                    join c  in _context.Cofinanciacion on a.CofinanciacionId equals c.CofinanciacionId
                                                    join cb in _context.CuentaBancaria on cr.CuentaBancariaId equals cb.CuentaBancariaId
                                                    //join rp in _context.RegistroPresupuestal on cr.RegistroPresupuestalId equals rp.RegistroPresupuestalId
                                                    //join cd in _context.CofinanciacionDocumento.DefaultIfEmpty() on cr.VigenciaAporteId equals cd.VigenciaAporte
                                                    //join va in _context.VigenciaAporte.DefaultIfEmpty() on cr.VigenciaAporteId equals va.VigenciaAporteId
                                                    where a.CofinanciacionAportanteId == AportanteId && cr.Eliminado != true
                                                    select cr).ToList();

                //List<ControlRecurso> ControlGrid = await _context.ControlRecurso
                //    .Include(RC => RC.FuenteFinanciacion)
                //    .ThenInclude(FF => FF.Aportante)
                //    .ThenInclude(APO => APO.Cofinanciacion)
                //    .Include(RC => RC.CuentaBancaria)
                //    .Include(RC => RC.RegistroPresupuestal)
                //     .Where(cr => cr.FuenteFinanciacion.AportanteId == AportanteId)
                //    .Include(RC => RC.VigenciaAporte)
                //    .ToListAsync();

                foreach (var r in ControlGrid)
                {
                    r.RegistroPresupuestal = _context.ControlRecurso.Where(ff => ff.ControlRecursoId == r.ControlRecursoId).Include(f => f.RegistroPresupuestal).Select(cc => cc.RegistroPresupuestal).FirstOrDefault();
                    r.CuentaBancaria = _context.ControlRecurso.Where(ff => ff.ControlRecursoId == r.ControlRecursoId).Include(f => f.CuentaBancaria).Select(cc=> cc.CuentaBancaria).FirstOrDefault();
                    r.FuenteFinanciacion = _context.ControlRecurso.Where(ff => ff.ControlRecursoId == r.ControlRecursoId).Include(f => f.FuenteFinanciacion).ThenInclude(a => a.Aportante).Select(r => r.FuenteFinanciacion).FirstOrDefault();
                    r.CofinanciacionDocumento = _context.CofinanciacionDocumento.Find(r.VigenciaAporteId);
                    r.VigenciaAporte = _context.VigenciaAporte.Find(r.VigenciaAporteId);
                };

                return ControlGrid.OrderBy(r => r.RegistroPresupuestal?.NumeroRp).ThenByDescending(r => r.RegistroPresupuestalId).ToList();

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
