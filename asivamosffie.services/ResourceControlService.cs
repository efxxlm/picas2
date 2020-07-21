﻿using asivamosffie.model.APIModels;
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
            return await _context.ControlRecurso.ToListAsync();
        }

        public async Task<ControlRecurso> GetResourceControlById(int id)
        {
            return await _context.ControlRecurso.FindAsync(id);
        }


        public async Task<List<ControlRecurso>> GetResourceControlGridBySourceFunding(int id)
        {
            List<ControlRecurso> ControlGrid = new List<ControlRecurso>();
            try
            {
                ControlGrid = await _context.ControlRecurso
                .Where( cr => cr.FuenteFinanciacionId == id)
                    .Include(RC => RC.FuenteFinanciacion)
                    .ThenInclude(FF => FF.Aportante)
                    .ThenInclude(APO => APO.Cofinanciacion)
                    .Include(RC => RC.CuentaBancaria)
                    .Include(RC => RC.RegistroPresupuestal)
                    .Include(RC => RC.VigenciaAporte)
                    .Include(RC => RC.FuenteFinanciacion)
                    .Include(RC => RC.FuenteFinanciacion)
                    .ThenInclude(FF => FF.Aportante)
                    
                    .ToListAsync();

                return ControlGrid;

            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public async Task<Respuesta> Insert(ControlRecurso controlRecurso)
        {
            Respuesta _response = new Respuesta();
            int idAccionCrearFuentesFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Recursos_Control, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                
                if (controlRecurso != null)
                {
                    _context.Add(controlRecurso);
                    await _context.SaveChangesAsync();
                    return new Respuesta
                       {
                           IsSuccessful = false,
                           IsException = true,
                           IsValidation = false,
                           Code = ConstantMessagesResourceControl.OperacionExitosa,
                           Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesResourceControl.OperacionExitosa, idAccionCrearFuentesFinanciacion, controlRecurso.UsuarioCreacion, "Crear Control")
                       };
                    
                }
                else
                {                    
                    return new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = ConstantMessagesResourceControl.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesResourceControl.Error, idAccionCrearFuentesFinanciacion, controlRecurso.UsuarioCreacion, "Crear Control")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesResourceControl.OperacionExitosa, idAccionCrearFuentesFinanciacion, controlRecurso.UsuarioCreacion, ex.Message)
                };                
            }
        }

        public async Task<Respuesta> Update(ControlRecurso controlRecurso)
        {
            Respuesta _response = new Respuesta();
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
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesResourceControl.EditadoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesResourceControl.EditadoCorrrectamente, idAccionCrearFuentesFinanciacion, controlRecurso.UsuarioModificacion, "Editar ")
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

        public async Task<bool> Delete(int id, string pUsuario)
        {
            Respuesta _response = new Respuesta();
            int idAccionCrearFuentesFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Recursos_Control, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                ControlRecurso updateObj = await _context.ControlRecurso.FindAsync(id);

                updateObj.Eliminado = true;
                updateObj.UsuarioModificacion = pUsuario;
                updateObj.FechaModificacion = DateTime.Now;

                _context.Update(updateObj);
                await _context.SaveChangesAsync();

                
                var response= new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesResourceControl.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesResourceControl.EditadoCorrrectamente, idAccionCrearFuentesFinanciacion, pUsuario, "Eliminado")
                };
                return true;
            }
            catch (Exception ex)
            {
                
                var response = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesResourceControl.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesResourceControl.Error, idAccionCrearFuentesFinanciacion, pUsuario, ex.Message)
                };
                return false;
            }
        }

    }
}
