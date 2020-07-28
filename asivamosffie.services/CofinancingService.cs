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

        public async Task<Respuesta> EliminarCofinanciacionByCofinanciacionId(int pCofinancicacionId, string pUsuarioModifico)
        {
            //Julian Martinez
            int IdAccionEliminarCofinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Cofinanciacion, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                Cofinanciacion cofinanciacion = _context.Cofinanciacion.Find(pCofinancicacionId);
                cofinanciacion.Eliminado = true;
                cofinanciacion.UsuarioModificacion = pUsuarioModifico;
                cofinanciacion.FechaModificacion = DateTime.Now;
                //Si falla descomentar el de abajo
                // _context.Update(cofinanciacion);
                _context.SaveChanges();

                return 
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesCofinanciacion.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Cofinanciacion, ConstantMessagesCofinanciacion.OperacionExitosa, IdAccionEliminarCofinanciacion, pUsuarioModifico, "COFINANCIACIÓN ELIMINADA")
                };
            }
            catch (Exception ex)
            {
                return  
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesProyecto.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Cofinanciacion, ConstantMessagesProyecto.Error, IdAccionEliminarCofinanciacion, pUsuarioModifico, ex.InnerException.ToString().Substring(0, 500))
                             };
            }

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
            cofinanciacion = _context.Cofinanciacion.Where(r => !(bool)r.Eliminado && r.CofinanciacionId == idCofinanciacion).FirstOrDefault();

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

        public async Task<Respuesta> CreateorUpdateCofinancing(Cofinanciacion cofinanciacion)
        { 
            int IdAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Cofinanciacion, (int)EnumeratorTipoDominio.Acciones);
      
            try
            {
                string CreadoEditado; 
                if (string.IsNullOrEmpty(cofinanciacion.CofinanciacionId.ToString()) || cofinanciacion.CofinanciacionId == 0)
                {
                    CreadoEditado = "CREAR COFINANCIACIÓN ";
                    cofinanciacion.Eliminado = false;
                    cofinanciacion.FechaCreacion = DateTime.Now;
                    _context.Cofinanciacion.Add(cofinanciacion);
                }
                else
                {
                    CreadoEditado = "EDITAR COFINANCIACIÓN";
                    Cofinanciacion cofinanciacionEdit = _context.Cofinanciacion.Find(cofinanciacion.CofinanciacionId);
                    cofinanciacionEdit.VigenciaCofinanciacionId = cofinanciacion.VigenciaCofinanciacionId;
                    cofinanciacionEdit.FechaModificacion = DateTime.Now;

                }

                await _context.SaveChangesAsync();
                foreach (var cofinanciacionAportante in cofinanciacion.CofinanciacionAportante)
                {
                    cofinanciacionAportante.CofinanciacionId = cofinanciacion.CofinanciacionId;
                    cofinanciacionAportante.UsuarioCreacion = cofinanciacion.UsuarioCreacion;
                    int idCofinancicacionAportante = CreateCofinancingContributor(cofinanciacionAportante);

                    //Se crear los CofinanciacionDocumento relacionados a este aportante
                    if (cofinanciacionAportante.CofinanciacionAportanteId > 0)
                    {
                        foreach (var cofinancicacionDocumento in cofinanciacionAportante.CofinanciacionDocumento)
                        {
                            cofinancicacionDocumento.CofinanciacionAportanteId = idCofinancicacionAportante;
                            cofinancicacionDocumento.UsuarioCreacion = cofinanciacionAportante.UsuarioCreacion;
                            CreateCofinancingDocuments(cofinancicacionDocumento);
                        }
                    }
                }
                await _context.SaveChangesAsync();

                return  
                       new Respuesta
                       {
                           IsSuccessful = true,
                           IsException = false,
                           IsValidation = false,
                           Code = ConstantMessagesProyecto.OperacionExitosa,
                           Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Cofinanciacion, ConstantMessagesProyecto.OperacionExitosa, IdAccion, cofinanciacion.UsuarioCreacion, CreadoEditado)
                       };

            }
            catch (Exception ex)
            {
                return 
                     new Respuesta
                     {
                         IsSuccessful = true,
                         IsException = false,
                         IsValidation = false,
                         Code = ConstantMessagesProyecto.Error,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Cofinanciacion, ConstantMessagesProyecto.Error, IdAccion, cofinanciacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                     };
            }

        }

        public int CreateCofinancingContributor(CofinanciacionAportante pcofinanciacionAportante)
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
                return pcofinanciacionAportante.CofinanciacionAportanteId;
            }
            catch (Exception )
            { 
                return 0;
            }

        }

        public int CreateCofinancingDocuments(CofinanciacionDocumento pCofinanciacionDocumento)
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
            catch (Exception )
            { 
                return 0;
            }

        }

        public async Task<List<Cofinanciacion>> GetListCofinancing()
        {
            List<Cofinanciacion> Listcofinanciacion = await _context.Cofinanciacion.Where(r => !(bool)r.Eliminado).ToListAsync();

            foreach (var item in Listcofinanciacion)
            {
                item.CofinanciacionAportante = await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado && r.CofinanciacionId == item.CofinanciacionId).IncludeFilter(r => r.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado)).ToListAsync();
            }
            return Listcofinanciacion;
        }

        public async Task<List<CofinanciacionDocumento>> GetDocument(int ContributorId)
        {
            return await _context.CofinanciacionDocumento.Where(r => r.CofinanciacionAportanteId == ContributorId && !(bool)r.Eliminado).ToListAsync();
        }

        public async Task<List<CofinanciacionAportante>> GetListAportante()
        {

            return await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado).ToListAsync();
        }

        public async Task<ActionResult<List<CofinanicacionAportanteGrilla>>> GetListAportanteByTipoAportanteId(int pTipoAportanteID)
        {
            List<CofinanciacionAportante> ListCofinanciacionAportante = await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado && r.TipoAportanteId == pTipoAportanteID).ToListAsync();

            List<CofinanicacionAportanteGrilla> ListCofinanicacionAportanteGrilla = new List<CofinanicacionAportanteGrilla>();

            foreach (var cofinanciacionAportante in ListCofinanciacionAportante)
            {
                CofinanicacionAportanteGrilla cofinanicacionAportanteGrilla = new CofinanicacionAportanteGrilla
                {
                    CofinanciacionAportanteId = cofinanciacionAportante.CofinanciacionAportanteId,
                    Nombre = cofinanciacionAportante.NombreAportanteId != null ? await _commonService.GetNombreDominioByDominioID((int)cofinanciacionAportante.NombreAportanteId) : "",
                    TipoAportante = await _commonService.GetNombreDominioByDominioID((int)cofinanciacionAportante.TipoAportanteId)
                };
                ListCofinanicacionAportanteGrilla.Add(cofinanicacionAportanteGrilla);
            }

            return ListCofinanicacionAportanteGrilla;
        }

        public async Task<ActionResult<List<CofinanciacionDocumento>>> GetListDocumentoByAportanteId(int pAportanteID)
        {
            return await _context.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado && r.CofinanciacionAportanteId == pAportanteID).ToListAsync();
        }

        public async Task<ActionResult<List<CofinanciacionAportante>>> GetListTipoAportante(int pTipoAportanteID)
        {
            //Lista tipo Aportante Cuando el tipo de aportante es otro o tercero
            return await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado && r.TipoAportanteId == pTipoAportanteID).Include(r => r.Cofinanciacion).ToListAsync();
        }
    }
}
