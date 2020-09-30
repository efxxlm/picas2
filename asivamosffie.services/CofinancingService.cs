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

        public static bool ValidarRegistroCompleto(Cofinanciacion cofinanciacion)
        {

            if (string.IsNullOrEmpty(cofinanciacion.VigenciaCofinanciacionId.ToString()))
            {
                return false;
            }
            else
            {
                foreach (var cofinanciacionAportante in cofinanciacion.CofinanciacionAportante)
                {
                    if (string.IsNullOrEmpty(cofinanciacionAportante.TipoAportanteId.ToString()) 
                        //|| string.IsNullOrEmpty(cofinanciacionAportante.MunicipioId.ToString()) los aportantes 3eros no tienen municipio
                        )

                    {
                        return false;
                    }
                    else
                    {
                        foreach (var CofinanciacionDocumento in cofinanciacionAportante.CofinanciacionDocumento)
                        {

                            if (string.IsNullOrEmpty(CofinanciacionDocumento.VigenciaAporte.ToString())
                                           || string.IsNullOrEmpty(CofinanciacionDocumento.ValorDocumento.ToString())
                                           || string.IsNullOrEmpty(CofinanciacionDocumento.TipoDocumentoId.ToString())
                                           //|| string.IsNullOrEmpty(CofinanciacionDocumento.NumeroActa.ToString())
                                           //|| string.IsNullOrEmpty(CofinanciacionDocumento.FechaActa.ToString())
                                           //|| string.IsNullOrEmpty(CofinanciacionDocumento.ValorTotalAportante.ToString())
                                           || string.IsNullOrEmpty(CofinanciacionDocumento.NumeroAcuerdo.ToString())
                                           || string.IsNullOrEmpty(CofinanciacionDocumento.FechaAcuerdo.ToString())
                                        )
                            {
                                return false;
                            }
                        }
                    }

                }

                return true;
            }



        }

        public async Task<Respuesta> EliminarCofinanciacionByCofinanciacionId(int pCofinancicacionId, string pUsuarioModifico)
        {
            //Julian Martinez
            int IdAccionEliminarCofinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Cofinanciacion, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                Cofinanciacion cofinanciacion = _context.Cofinanciacion.Find(pCofinancicacionId);
                cofinanciacion.Eliminado = true;
                cofinanciacion.UsuarioModificacion = pUsuarioModifico.ToUpper();
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Cofinanciacion, ConstantMessagesCofinanciacion.EliminacionExitosa, IdAccionEliminarCofinanciacion, pUsuarioModifico, "COFINANCIACIÓN ELIMINADA")
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
            //Se modifica los campos obligatorios de base de datos y se devuelve el CofinanicacionId en el data respuesta para cuando se

            int IdAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Cofinanciacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                string CreadoEditado;
                if (cofinanciacion.CofinanciacionId == 0)
                {
                    CreadoEditado = "CREAR COFINANCIACIÓN ";
                    cofinanciacion.Eliminado = false;
                    cofinanciacion.FechaCreacion = DateTime.Now;
                    cofinanciacion.RegistroCompleto = ValidarRegistroCompleto(cofinanciacion);

                    //Validar Registro Completo
                    /*if (string.IsNullOrEmpty(cofinanciacion.VigenciaCofinanciacionId.ToString()))
                    {
                        cofinanciacion.RegistroCompleto = false;
                    }
                    else
                    {
                        cofinanciacion.RegistroCompleto = true;
                    }*/


                    //Foreach para poner campos de auditoria en las tablas relacionadas !

                    foreach (var cofinanciacionAportante in cofinanciacion.CofinanciacionAportante)
                    {
                        cofinanciacionAportante.UsuarioCreacion = cofinanciacion.UsuarioCreacion.ToUpper();
                        cofinanciacionAportante.FechaCreacion = DateTime.Now;
                        cofinanciacionAportante.Eliminado = false;

                        foreach (var documentoAportante in cofinanciacionAportante.CofinanciacionDocumento)
                        {
                            documentoAportante.Eliminado = false;
                            documentoAportante.UsuarioCreacion = cofinanciacion.UsuarioCreacion.ToUpper();
                            documentoAportante.FechaCreacion = DateTime.Now;
                        }
                    }
                    _context.Cofinanciacion.Add(cofinanciacion);
                }
                else
                {
                    CreadoEditado = "EDITAR COFINANCIACIÓN";
                    Cofinanciacion cofinanciacionEdit = _context.Cofinanciacion.Find(cofinanciacion.CofinanciacionId);
                    cofinanciacionEdit.RegistroCompleto = ValidarRegistroCompleto(cofinanciacion);
                    cofinanciacionEdit.VigenciaCofinanciacionId = cofinanciacion.VigenciaCofinanciacionId;
                    cofinanciacionEdit.FechaModificacion = DateTime.Now;
                    cofinanciacionEdit.UsuarioModificacion = cofinanciacion.UsuarioCreacion.ToUpper();
                }

                foreach (var cofinanciacionAportante in cofinanciacion.CofinanciacionAportante)
                {

                    if (cofinanciacionAportante.CofinanciacionAportanteId > 0)
                    {
                        cofinanciacionAportante.CofinanciacionId = cofinanciacion.CofinanciacionId;
                        cofinanciacionAportante.UsuarioCreacion = cofinanciacion.UsuarioCreacion.ToUpper();
                        int idCofinancicacionAportante = EditCofinancingContributor(cofinanciacionAportante);


                        if (idCofinancicacionAportante > 0)
                        {
                            foreach (var cofinancicacionDocumento in cofinanciacionAportante.CofinanciacionDocumento)
                            {

                                if (cofinancicacionDocumento.CofinanciacionDocumentoId > 0)
                                {
                                    cofinancicacionDocumento.CofinanciacionAportanteId = idCofinancicacionAportante;
                                    cofinancicacionDocumento.UsuarioCreacion = cofinanciacionAportante.UsuarioCreacion.ToUpper();
                                    CreateCofinancingDocuments(cofinancicacionDocumento);

                                }

                            }
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
                           Data = cofinanciacion,
                           Code = ConstantMessagesProyecto.OperacionExitosa,
                           Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Cofinanciacion, ConstantMessagesProyecto.OperacionExitosa, IdAccion, cofinanciacion.UsuarioCreacion.ToUpper(), CreadoEditado)
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
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Cofinanciacion, ConstantMessagesProyecto.Error, IdAccion, cofinanciacion.UsuarioCreacion.ToUpper(), ex.InnerException.ToString().Substring(0, 500))
                     };
            }

        }

        public int EditCofinancingContributor(CofinanciacionAportante pcofinanciacionAportante)
        {
            try
            {
                CofinanciacionAportante cofinanciacionAportanteEdit = _context.CofinanciacionAportante.Find(pcofinanciacionAportante.CofinanciacionAportanteId);
                cofinanciacionAportanteEdit.UsuarioModificacion = pcofinanciacionAportante.UsuarioCreacion.ToUpper();
                cofinanciacionAportanteEdit.FechaModificacion = DateTime.Now;
                cofinanciacionAportanteEdit.MunicipioId = pcofinanciacionAportante.MunicipioId;
                cofinanciacionAportanteEdit.NombreAportanteId = pcofinanciacionAportante.NombreAportanteId;
                cofinanciacionAportanteEdit.TipoAportanteId = pcofinanciacionAportante.TipoAportanteId;
                cofinanciacionAportanteEdit.NombreAportanteId = pcofinanciacionAportante.NombreAportanteId;

                return pcofinanciacionAportante.CofinanciacionAportanteId;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public int CreateCofinancingDocuments(CofinanciacionDocumento pCofinanciacionDocumento)
        {
            try
            {
                CofinanciacionDocumento cofinanciacionDocumentoEdit = _context.CofinanciacionDocumento.Find(pCofinanciacionDocumento.CofinanciacionDocumentoId);

                cofinanciacionDocumentoEdit.UsuarioModificacion = pCofinanciacionDocumento.UsuarioCreacion.ToUpper();
                cofinanciacionDocumentoEdit.FechaModificacion = DateTime.Now;
                cofinanciacionDocumentoEdit.FechaActa = pCofinanciacionDocumento.FechaActa;
                cofinanciacionDocumentoEdit.FechaAcuerdo = pCofinanciacionDocumento.FechaAcuerdo;
                cofinanciacionDocumentoEdit.NumeroActa = pCofinanciacionDocumento.NumeroActa;
                cofinanciacionDocumentoEdit.TipoDocumentoId = pCofinanciacionDocumento.TipoDocumentoId;
                cofinanciacionDocumentoEdit.ValorDocumento = pCofinanciacionDocumento.ValorDocumento;
                cofinanciacionDocumentoEdit.ValorTotalAportante = pCofinanciacionDocumento.ValorTotalAportante;
                cofinanciacionDocumentoEdit.VigenciaAporte = pCofinanciacionDocumento.VigenciaAporte;
                cofinanciacionDocumentoEdit.NumeroAcuerdo = pCofinanciacionDocumento.NumeroAcuerdo;

                return pCofinanciacionDocumento.CofinanciacionDocumentoId;
            }
            catch (Exception)
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
                item.ValorTotal = _context.CofinanciacionDocumento.Where(x=> item.CofinanciacionAportante.Select(x=>x.CofinanciacionAportanteId).ToList().Contains((int)x.CofinanciacionAportanteId)).Sum(x=>x.ValorDocumento);
            }
            return Listcofinanciacion.OrderByDescending(r => r.CofinanciacionId).ToList();
        }

        public async Task<List<CofinanciacionDocumento>> GetDocument(int ContributorId)
        {
            return await _context.CofinanciacionDocumento.Where(r => r.CofinanciacionAportanteId == ContributorId && !(bool)r.Eliminado).Include(x=>x.TipoDocumento).ToListAsync();
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
            //jflorez: modifico para setear el nombre del aportante
            var retorno= await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado && r.TipoAportanteId == pTipoAportanteID && !(bool)r.Cofinanciacion.Eliminado).Include(r => r.Cofinanciacion).Include(x => x.TipoAportante).ToListAsync();
            foreach(var ret in retorno)
            {
                if(ret.TipoAportanteId==ConstanTipoAportante.Ffie)
                {
                    ret.NombreAportanteString = ConstanStringTipoAportante.Ffie;
                }
                else if (ret.TipoAportanteId == ConstanTipoAportante.ET)
                {
                    //verifico si tiene municipio
                    if(ret.MunicipioId!=null)
                    {
                        ret.NombreAportanteString = _context.Localizacion.Find(ret.MunicipioId).Descripcion;
                    }
                    else//solo departamento
                    {
                        ret.NombreAportanteString = ret.DepartamentoId==null?"Error":_context.Localizacion.Find(ret.DepartamentoId).Descripcion;
                    }
                    
                }
                else
                {
                    ret.NombreAportanteString = _context.Dominio.Find(ret.NombreAportanteId).Nombre;
                }                
                ret.TipoAportanteString = ret.TipoAportante.Nombre;
                ret.Departamento = null;
                ret.Municipio = null;
                ret.RegistroPresupuestal = null;
                ret.ProyectoAportante = null;
                ret.NombreAportante = null;
                ret.TipoAportante.CofinanciacionAportanteTipoAportante = null;
                ret.FuenteFinanciacion = _context.FuenteFinanciacion.Where(x=>x.AportanteId==ret.CofinanciacionAportanteId).ToList();
            }
            return retorno.OrderByDescending(x=>x.FechaCreacion).ToList();
        }
    }
}
