using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.api.Controllers;
using AuthorizationTest.JwtHelpers;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.model.APIModels;
using Microsoft.AspNetCore.Mvc;
using Z.EntityFramework.Plus;
using Microsoft.EntityFrameworkCore;

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


        public async Task<Cofinanciacion> GetCofinanciacionByIdCofinanciacion(int idCofinanciacion)
        {//con include
         //Cofinanciacion cofinanciacion = await
         //  _context.Cofinanciacion
         // .Include(r => r.CofinanciacionAportante)
         //    .ThenInclude(post => post.CofinanciacionDocumento)
         //    .Where(c => c.CofinanciacionId == idCofinanciacion && !(bool)c.Eliminado)
         //    .Where(e => e.CofinanciacionAportante.Any(r => !(bool)r.Eliminado))
         //    .Where(d => d.CofinanciacionAportante.Any(r => r.CofinanciacionDocumento.Any(x => !(bool)x.Eliminado)))
         // .FirstOrDefaultAsync();

            //includefilter
            Cofinanciacion cofinanciacion = new Cofinanciacion();
             cofinanciacion =  _context.Cofinanciacion.Where(r => !(bool)r.Eliminado && r.CofinanciacionId == idCofinanciacion).FirstOrDefault();

            if (cofinanciacion != null)
            {
                List<CofinanciacionAportante> cofinanciacionAportante = await _context.CofinanciacionAportante.Where(r => r.CofinanciacionId == idCofinanciacion && !(bool)r.Eliminado).IncludeFilter(r => r.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado)).ToListAsync();

                cofinanciacion.CofinanciacionAportante = cofinanciacionAportante;
            }
            //Con linq
            //var cofinanciacion = (
            //            from cof in _context.Cofinanciacion  

            //            where cof.CofinanciacionId == idCofinanciacion
            //             join  cofApor in _context.CofinanciacionAportante
            //             on idCofinanciacion equals cofApor.CofinanciacionId
            //             into JoinedCofCofApor
            //             from cofApor in JoinedCofCofApor.DefaultIfEmpty()
            //            where cofApor.CofinanciacionId == cof.CofinanciacionId
            //            select  new 
            //            { 
            //                cofinanciacion = cof

            //            }).ToList()  
            //            .Select(coff => new Cofinanciacion() {
            //                CofinanciacionAportante = coff.cofinanciacion.CofinanciacionAportante 
            //            });



            return cofinanciacion;
        }

        public async Task<object> CreateorUpdateCofinancing(Cofinanciacion cofinanciacion)
        {
            Respuesta respuesta = new Respuesta();
            bool IsSuccessful = false;
            int idCofinancicacionAportante = 0;

            if (cofinanciacion.CofinanciacionAportante.Count > 0)
            {
                try
                {
                    if (string.IsNullOrEmpty(cofinanciacion.CofinanciacionId.ToString()) || cofinanciacion.CofinanciacionId == 0)
                    {

                        cofinanciacion.Eliminado = false;
                        cofinanciacion.FechaCreacion = DateTime.Now;
                        _context.Cofinanciacion.Add(cofinanciacion);
                        respuesta = new Respuesta() { IsValidation = true, Code = ConstantMessagesCofinanciacion.CreadoCorrrectamente };

                    }
                    else
                    {
                        Cofinanciacion cofinanciacionEdit = _context.Cofinanciacion.Find(cofinanciacion.CofinanciacionId);
                        cofinanciacionEdit.VigenciaCofinanciacionId = cofinanciacion.VigenciaCofinanciacionId;
                        cofinanciacionEdit.FechaModificacion = DateTime.Now;
                        respuesta = new Respuesta() { IsValidation = true, Code = ConstantMessagesCofinanciacion.EditadoCorrrectamente };
                    }


                    foreach (var cofinanciacionAportante in cofinanciacion.CofinanciacionAportante)
                    {
                        cofinanciacionAportante.CofinanciacionId = cofinanciacion.CofinanciacionId;
                        cofinanciacionAportante.UsuarioCreacion = cofinanciacion.UsuarioCreacion;
                        idCofinancicacionAportante = await CreateCofinancingContributor(cofinanciacionAportante);

                        //Se crear los CofinanciacionDocumento relacionados a este aportante
                        if (cofinanciacionAportante.CofinanciacionAportanteId > 0)
                        {
                            foreach (var cofinancicacionDocumento in cofinanciacionAportante.CofinanciacionDocumento)
                            {
                                cofinancicacionDocumento.CofinanciacionAportanteId = idCofinancicacionAportante;
                                cofinancicacionDocumento.UsuarioCreacion = cofinanciacionAportante.UsuarioCreacion;
                                await CreateCofinancingDocuments(cofinancicacionDocumento);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    respuesta = new Respuesta() { IsValidation = false, Code = ConstantMessagesUsuarios.ErrorGuardarCambios };
                    respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Usuario, respuesta.Code, (int)enumeratorAccion.Error, cofinanciacion.UsuarioCreacion, ex.ToString() + ex.InnerException);
                    return respuesta;
                }
            }
            else
            {
                respuesta = new Respuesta() { IsValidation = false, Code = ConstantMessagesCofinanciacion.CamposIncompletos };

            }
            respuesta.Data = cofinanciacion.CofinanciacionId;
            respuesta.IsSuccessful = IsSuccessful;
            respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Cofinanciacion, respuesta.Code, (int)enumeratorAccion.CrearActualizarCofinanciacion, cofinanciacion.UsuarioCreacion, " ");
            return respuesta;
        }

        public async Task<int> CreateCofinancingContributor(CofinanciacionAportante pcofinanciacionAportante)
        {
            try
            {

                if (string.IsNullOrEmpty(pcofinanciacionAportante.CofinanciacionAportanteId.ToString()) || pcofinanciacionAportante.CofinanciacionAportanteId == 0)
                {
                    pcofinanciacionAportante.FechaCreacion = DateTime.Now;
                    pcofinanciacionAportante.Eliminado = false;
                    _context.CofinanciacionAportante.Add(pcofinanciacionAportante);
                }
                else
                {

                    CofinanciacionAportante cofinanciacionAportanteEdit = _context.CofinanciacionAportante.Find(pcofinanciacionAportante.CofinanciacionAportanteId);
                    cofinanciacionAportanteEdit.UsuarioModificacion = pcofinanciacionAportante.UsuarioCreacion;
                    cofinanciacionAportanteEdit.FechaModificacion = DateTime.Now;
                    cofinanciacionAportanteEdit.MunicipioId = pcofinanciacionAportante.MunicipioId;
                    cofinanciacionAportanteEdit.NombreAportanteId = pcofinanciacionAportante.NombreAportanteId;
                    cofinanciacionAportanteEdit.TipoAportanteId = pcofinanciacionAportante.TipoAportanteId;
                    cofinanciacionAportanteEdit.NombreAportanteId = pcofinanciacionAportante.NombreAportanteId;
                }

                //await _context.SaveChangesAsync();
                return pcofinanciacionAportante.CofinanciacionAportanteId;
            }
            catch (Exception ex)
            {
                CofinanciacionAportante pcofinanciacionAportanteNull = new CofinanciacionAportante();
                return 0;
            }

        }

        public async Task<int> CreateCofinancingDocuments(CofinanciacionDocumento pCofinanciacionDocumento)
        {
            try
            {
                if (string.IsNullOrEmpty(pCofinanciacionDocumento.CofinanciacionDocumentoId.ToString()) || pCofinanciacionDocumento.CofinanciacionDocumentoId == 0)
                {
                    pCofinanciacionDocumento.FechaCreacion = DateTime.Now;
                    pCofinanciacionDocumento.Eliminado = false;
                    _context.CofinanciacionDocumento.Add(pCofinanciacionDocumento);
                }
                else
                {
                    CofinanciacionDocumento cofinanciacionDocumentoEdit = _context.CofinanciacionDocumento.Find(pCofinanciacionDocumento.CofinanciacionDocumentoId);

                    cofinanciacionDocumentoEdit.UsuarioModificacion = pCofinanciacionDocumento.UsuarioCreacion;
                    cofinanciacionDocumentoEdit.FechaModificacion = DateTime.Now;
                    cofinanciacionDocumentoEdit.FechaActa = pCofinanciacionDocumento.FechaActa;
                    cofinanciacionDocumentoEdit.FechaAcuerdo = pCofinanciacionDocumento.FechaAcuerdo;
                    cofinanciacionDocumentoEdit.NumeroActa = pCofinanciacionDocumento.NumeroActa;
                    cofinanciacionDocumentoEdit.TipoDocumentoId = pCofinanciacionDocumento.TipoDocumentoId;
                    cofinanciacionDocumentoEdit.ValorDocumento = pCofinanciacionDocumento.ValorDocumento;
                    cofinanciacionDocumentoEdit.ValorTotalAportante = pCofinanciacionDocumento.ValorTotalAportante;
                }

                //await _context.SaveChangesAsync();
                return pCofinanciacionDocumento.CofinanciacionDocumentoId;
            }
            catch (Exception ex)
            {
                CofinanciacionDocumento cofinanciacionDocumentoNull = new CofinanciacionDocumento();
                return 0;
            }

        }

        public async Task<List<Cofinanciacion>> GetListCofinancing()
        {
            List<Cofinanciacion> Listcofinanciacion = await _context.Cofinanciacion.Where(r => !(bool)r.Eliminado).ToListAsync();
             
            foreach (var item in Listcofinanciacion)
            {
                item.CofinanciacionAportante = await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado && r.CofinanciacionId == item.CofinanciacionId).IncludeFilter(r=> r.CofinanciacionDocumento.Where(r=> !(bool)r.Eliminado)).ToListAsync();
             } 
            return Listcofinanciacion;
        }

        //GetDocument?cofinancionAportanteId
        public async Task<ActionResult<List<CofinanciacionDocumento>>> GetDocument(int id)
        {

            try
            {
                //return await _context.DocumentoApropiacion.Include(x => x.Aportante).Where(x => x.AportanteId == ContributorId).ToListAsync();
                return await _context.CofinanciacionDocumento.Where(x => x.CofinanciacionAportanteId == id).Include(x=>x.TipoDocumento).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ActionResult<List<CofinanciacionAportante>>> GetListTipoAportante(int pTipoAportanteID)
        {
            //Lista tipo Aportante Cuando el tipo de aportante es otro o tercero
            return await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado && r.TipoAportanteId == pTipoAportanteID).Include(r => r.Cofinanciacion).ToListAsync();
        }
    }
}
