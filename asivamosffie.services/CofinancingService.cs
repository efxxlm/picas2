using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.api.Controllers;
using AuthorizationTest.JwtHelpers;
using asivamosffie.services.Models;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Helpers.Constant;
namespace asivamosffie.services
{
    public class CofinancingService : ICofinancingService
    {

        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        public CofinancingService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }
         
        public async Task<object> CreateorUpdateCofinancing(Cofinanciacion cofinanciacion)
        {
            Respuesta respuesta = new Respuesta();
            bool IsSuccessful = false;

            if (cofinanciacion.CofinanciacionAportante.Count > 0 || cofinanciacion.CofinanciacionDocumento.Count > 0)
            { 
                try
                { 
                    if (string.IsNullOrEmpty(cofinanciacion.CofinanciacionId.ToString()) || cofinanciacion.CofinanciacionId == 0)
                    {
                      
                        cofinanciacion.Eliminado = false;
                        cofinanciacion.FechaCreacion = DateTime.Now;
                        _context.Cofinanciacion.Add(cofinanciacion);
                        respuesta = new Respuesta() {IsValidation = true, Code = ConstantMessagesCofinanciacion.CreadoCorrrectamente};

                    }
                    else
                    {
                        respuesta = new Respuesta() { IsValidation = true, Code = ConstantMessagesCofinanciacion.EditadoCorrrectamente };

                        cofinanciacion.FechaModificacion = DateTime.Now;
                        _context.Cofinanciacion.Update(cofinanciacion);
                    }
                    await _context.SaveChangesAsync();


                    foreach (var item in cofinanciacion.CofinanciacionAportante)
                    {
                        item.CofinanciacionId = cofinanciacion.CofinanciacionId;
                        item.UsuarioCreacion = cofinanciacion.UsuarioCreacion;
                        IsSuccessful = await CreateCofinancingContributor(item);
                    }

                    foreach (var item in cofinanciacion.CofinanciacionDocumento)
                    {
                        item.UsuarioCreacion = cofinanciacion.UsuarioCreacion;
                        item.CofinanciacionId = cofinanciacion.CofinanciacionId;
                        IsSuccessful = await CreateCofinancingDocuments(item);
                    }
                }
                catch (Exception ex)
                {
                    respuesta = new Respuesta() { IsValidation = false, Code = ConstantMessagesUsuarios.ErrorGuardarCambios };
                    respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Usuario, respuesta.Code) + ": " + ex.ToString() + ex.InnerException;
                    return respuesta;
                }
            }
            else
            { 
                respuesta = new Respuesta() { IsValidation = false, Code = ConstantMessagesCofinanciacion.CamposIncompletos };

            }

            respuesta.IsSuccessful = IsSuccessful;
            respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Usuario, respuesta.Code);
            return respuesta; 
        }

        public async Task<bool> CreateCofinancingContributor(CofinanciacionAportante pcofinanciacionAportante)
        {
            try
            {
                pcofinanciacionAportante.FechaCreacion = DateTime.Now;
                pcofinanciacionAportante.Eliminado = false;
                if (string.IsNullOrEmpty(pcofinanciacionAportante.CofinanciacionAportanteId.ToString()) || pcofinanciacionAportante.CofinanciacionAportanteId == 0)
                {
                    pcofinanciacionAportante.FechaCreacion = DateTime.Now;
                    _context.CofinanciacionAportante.Add(pcofinanciacionAportante);
                }
                else
                {
                    pcofinanciacionAportante.UsuarioModificacion = pcofinanciacionAportante.UsuarioCreacion;
                    pcofinanciacionAportante.FechaModificacion = DateTime.Now;
                    _context.CofinanciacionAportante.Update(pcofinanciacionAportante);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> CreateCofinancingDocuments(CofinanciacionDocumento pCofinanciacionDocumento)
        {
            try
            {
                pCofinanciacionDocumento.FechaCreacion = DateTime.Now;
                pCofinanciacionDocumento.Eliminado = false;
                if (string.IsNullOrEmpty(pCofinanciacionDocumento.CofinancicacionDocumentoId.ToString()) || pCofinanciacionDocumento.CofinancicacionDocumentoId == 0)
                {
                    pCofinanciacionDocumento.FechaCreacion = DateTime.Now;
                    _context.CofinanciacionDocumento.Add(pCofinanciacionDocumento);
                }
                else
                {
                    pCofinanciacionDocumento.UsuarioModificacion = pCofinanciacionDocumento.UsuarioCreacion;
                    pCofinanciacionDocumento.FechaModificacion = DateTime.Now;
                    _context.CofinanciacionDocumento.Update(pCofinanciacionDocumento);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<List<Cofinanciacion>> GetListCofinancing()
        {
            List<Cofinanciacion> cofinanciacion = await _context.Cofinanciacion.Where(r => !(bool)r.Eliminado).ToListAsync();

            //Add todos los cofinanciacion Aportante que esten activo a cada cofinanciacion del foreach
            foreach (var item in cofinanciacion)
            {
                item.CofinanciacionAportante = await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado && r.CofinanciacionId == item.CofinanciacionId).ToListAsync();
                item.CofinanciacionDocumento = await _context.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado && r.CofinanciacionId == item.CofinanciacionId).ToListAsync();

            }

            return cofinanciacion;
        }
    }
}
