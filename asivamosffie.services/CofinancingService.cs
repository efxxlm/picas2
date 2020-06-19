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

namespace asivamosffie.services
{
    public class CofinancingService : ICofinancingService
    {

        private readonly devAsiVamosFFIEContext _context;

        public CofinancingService(devAsiVamosFFIEContext context)
        {
            _context = context;
        }
        public async Task<object> CreateCofinancing(Cofinanciacion cofinanciacion, List<CofinanciacionAportante> pListCofinanciacionAportante, List<CofinanciacionDocumento> pListconinanciacionDocumentos)
        {
             cofinanciacion.Eliminado = false;
            _context.Cofinanciacion.Add(cofinanciacion);

            foreach (var item in pListCofinanciacionAportante)
            {
                item.CofinanciacionId = cofinanciacion.CofinanciacionId;
                item.UsuarioCreacion = cofinanciacion.UsuarioCreacion;
                await CreateCofinancingContributor(item);
            }

            foreach (var item in pListconinanciacionDocumentos)
            {
                item.CofinanciacionId = cofinanciacion.CofinanciacionId;
                await CreateCofinancingDocuments(item);
                item.UsuarioCreacion = cofinanciacion.UsuarioCreacion;
            }


            return cofinanciacion.CofinanciacionId;

        }

        public async Task<object> CreateCofinancingContributor(CofinanciacionAportante pcofinanciacionAportante)
        {
            try
            {
                pcofinanciacionAportante.FechaCreacion = DateTime.Now;
                pcofinanciacionAportante.Eliminado = false;
                _context.CofinanciacionAportante.Add(pcofinanciacionAportante);
                await _context.SaveChangesAsync();
                return pcofinanciacionAportante.CofinanciacionId;
            }
            catch (Exception ex)
            {
                return ex;
            }

        }

        public async Task<object> CreateCofinancingDocuments(CofinanciacionDocumento pCofinanciacionDocumento)
        {
            try
            {
                pCofinanciacionDocumento.FechaCreacion = DateTime.Now;
                pCofinanciacionDocumento.Eliminado = false;
                _context.CofinanciacionDocumento.Add(pCofinanciacionDocumento);
                await _context.SaveChangesAsync();
                return pCofinanciacionDocumento.CofinanciacionId;
            }
            catch (Exception ex)
            {
                return ex;
            }

        }

        public async Task<List<Cofinanciacion>> GetListCofinancing()
        {  
            return await _context.Cofinanciacion.Where(r=> !(bool)r.Eliminado).Include(r => r.CofinanciacionAportante).Include(r => r.CofinanciacionDocumento).ToListAsync();
        }
    }
}
