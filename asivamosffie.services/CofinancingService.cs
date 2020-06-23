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
            string strMensaje = "";
            bool IsSuccessful = false;

            if (cofinanciacion.CofinanciacionAportante.Count > 0 || cofinanciacion.CofinanciacionDocumento.Count > 0)
            { 
                try
                { 
                    if (string.IsNullOrEmpty(cofinanciacion.CofinanciacionId.ToString()) || cofinanciacion.CofinanciacionId == 0)
                    {
                        strMensaje = "103";
                        cofinanciacion.Eliminado = false;
                        cofinanciacion.FechaCreacion = DateTime.Now;
                        _context.Cofinanciacion.Add(cofinanciacion);
                    }
                    else
                    {
                        strMensaje = "102";
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
                    return new Respuesta() { IsSuccessful = false, IsValidation = false, Code = "500", Message = ex.ToString() + ex.InnerException };

                }
            }
            else
            {
                IsSuccessful = false;
                strMensaje = "101";
            }


            string strMensajeValidacion = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Cofinancicacion, strMensaje);
            return new Respuesta() { IsSuccessful = IsSuccessful, IsValidation = true, Code = strMensaje, Message = strMensajeValidacion };


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
