using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.services.Helpers.Enumerator;
namespace asivamosffie.services
{
    public class SourceFundingService : ISourceFundingService
    {
        private readonly ICommonService _commonService;
        private readonly ICofinancingContributorService _contributor;
        private readonly devAsiVamosFFIEContext _context;

        public SourceFundingService(devAsiVamosFFIEContext context, ICommonService commonService, ICofinancingContributorService contributor)
        {
            _context = context;
            _commonService = commonService;
            _contributor = contributor;
        }

        public async Task<List<FuenteFinanciacion>> GetISourceFunding()
        {
            return await _context.FuenteFinanciacion.ToListAsync();
        }

        public async Task<FuenteFinanciacion> GetISourceFundingById(int id)
        {
            return await _context.FuenteFinanciacion.Where(r => r.FuenteFinanciacionId == id)
                        .Include(r => r.ControlRecurso)
                        .Include(r => r.CuentaBancaria)
                        .Include(r => r.VigenciaAporte)
                        .Include(r => r.Aportante)
                        .ThenInclude(apo => apo.RegistroPresupuestal)
                        .Include(r => r.Aportante)
                        .ThenInclude(apo => apo.Cofinanciacion)
                        .Include(r => r.Aportante)
                        .ThenInclude(apo => apo.CofinanciacionDocumento)
                        .FirstOrDefaultAsync();
        }

        public async Task<Respuesta> CreateEditFuentesFinanciacion(FuenteFinanciacion fuentefinanciacion)
        {
            BankAccountService bankAccountService = new BankAccountService(_context, _commonService);
            int idAccionCrearFuentesFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Fuentes_Financiacion, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                if (fuentefinanciacion.FuenteFinanciacionId == null || fuentefinanciacion.FuenteFinanciacionId == 0)
                {
                    fuentefinanciacion.FechaCreacion = DateTime.Now;
                    fuentefinanciacion.Eliminado = false;
                    _context.Add(fuentefinanciacion);
                }
                else
                {
                    FuenteFinanciacion fuente = _context.FuenteFinanciacion.Find(fuentefinanciacion.FuenteFinanciacionId);
                    fuente.FechaModificacion = DateTime.Now;
                    fuente.ValorFuente = fuentefinanciacion.ValorFuente;
                }

                foreach (VigenciaAporte vi in fuentefinanciacion.VigenciaAporte)
                {
                    vi.FuenteFinanciacionId = fuentefinanciacion.FuenteFinanciacionId;
                    vi.UsuarioCreacion = fuentefinanciacion.UsuarioCreacion;
                    vi.FechaCreacion = DateTime.Now;
                    await this.CreateEditarVigenciaAporte(vi);
                };

                foreach (CuentaBancaria cb in fuentefinanciacion.CuentaBancaria)
                {
                    cb.UsuarioCreacion = fuentefinanciacion.UsuarioCreacion;
                    cb.FechaCreacion = DateTime.Now;
                    await bankAccountService.CreateEditarCuentasBancarias(cb);
                };


                await _context.SaveChangesAsync();


                return 
               new Respuesta
               {
                   IsSuccessful = true,
                   IsException = false,
                   IsValidation = false,
                   Code = ConstantMessagesFuentesFinanciacion.OperacionExitosa,
                   Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.OperacionExitosa, idAccionCrearFuentesFinanciacion, fuentefinanciacion.UsuarioCreacion, "CREAR FUENTES DE FINANCIACIÓN")
               };
            }
            catch (Exception ex)
            {
                return 
                       new Respuesta
                       {
                           IsSuccessful = false,
                           IsException = true,
                           IsValidation = false,
                           Code = ConstantMessagesFuentesFinanciacion.Error,
                           Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.Error, idAccionCrearFuentesFinanciacion, fuentefinanciacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                       };
            }
        }

        public async Task<Respuesta> EliminarFuentesFinanciacion(int id, string UsuarioModifico)
        {
        
            int idAccionEliminarFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Fuentes_Financiacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                var entity = await _context.FuenteFinanciacion.FindAsync(id);
                entity.FechaModificacion = DateTime.Now;
                entity.UsuarioModificacion = UsuarioModifico;
                entity.Eliminado = true;

                _context.Update(entity);
                await _context.SaveChangesAsync();

                return 
                      new Respuesta
                      {
                          IsSuccessful = true,
                          IsException = false,
                          IsValidation = false,
                          Code = ConstantMessagesFuentesFinanciacion.OperacionExitosa,
                          Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.OperacionExitosa, idAccionEliminarFinanciacion, UsuarioModifico, "ELIMINAR FUENTE DE FINANCIACIÓN")
                      };
            }
            catch (Exception ex)
            {
                return 
                 new Respuesta
                 {
                     IsSuccessful = false,
                     IsException = true,
                     IsValidation = false,
                     Code = ConstantMessagesFuentesFinanciacion.Error,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.Error, idAccionEliminarFinanciacion, UsuarioModifico, ex.InnerException.ToString().Substring(0, 500))
                 };
            }
        }

        public async Task<Respuesta> EditFuentesFinanciacion(FuenteFinanciacion fuentefinanciacion)
        {
        
            int idAccionCrearFuentesFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Fuentes_Financiacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                FuenteFinanciacion updateObj = await _context.FuenteFinanciacion.FindAsync(fuentefinanciacion.FuenteFinanciacionId);

                updateObj.AportanteId = fuentefinanciacion.AportanteId;
                updateObj.FuenteRecursosCodigo = fuentefinanciacion.FuenteRecursosCodigo;
                updateObj.ValorFuente = fuentefinanciacion.ValorFuente;

                _context.Update(updateObj);
                await _context.SaveChangesAsync();

                return 
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantMessagesFuentesFinanciacion.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.OperacionExitosa, idAccionCrearFuentesFinanciacion, fuentefinanciacion.UsuarioCreacion, "EDITAR FUENTES DE FINANCIACIÓN")
                    };
            }
            catch (Exception ex)
            {
                return 
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = ConstantMessagesFuentesFinanciacion.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.Error, idAccionCrearFuentesFinanciacion, fuentefinanciacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                    };
            }
        }

        public async Task<List<FuenteFinanciacion>> GetFuentesFinanciacionByAportanteId(int AportanteId)
        {
            return await _context.FuenteFinanciacion.Where(r => !(bool)r.Eliminado)
                        .Where(r => r.AportanteId == AportanteId)
                        .Include(r => r.ControlRecurso)
                        .Include(r => r.CuentaBancaria)
                        .Include(r => r.VigenciaAporte)
                        .Include(r => r.Aportante)
                        .ThenInclude(r => r.RegistroPresupuestal)
                        .ToListAsync();
        }

        public async Task<List<FuenteFinanciacion>> GetListFuentesFinanciacion()
        {
            //jflorez, cambio esto para tener el retorno con todas las variables

            var retorno= await _context.FuenteFinanciacion.Where(r => !(bool)r.Eliminado).Distinct().Include(r => r.ControlRecurso).Include(r => r.CuentaBancaria).Include(r => r.VigenciaAporte).Include(r => r.Aportante).ThenInclude(r => r.RegistroPresupuestal).ToListAsync();
            foreach(var ret in retorno)
            {
                ret.Aportante.NombreAportante = _context.Dominio.Find(ret.Aportante.NombreAportanteId).Nombre;
                ret.Aportante.TipoAportante = _context.Dominio.Find(ret.Aportante.TipoAportanteId).Nombre;
            }
            return retorno;
        }

        public async Task<Respuesta> CreateEditarVigenciaAporte(VigenciaAporte vigenciaAporte)
        {
 
            int idAccionCrearVigenciaAporte = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Vigencia_Aporte, (int)EnumeratorTipoDominio.Acciones);
            try
            {

                string strCrearEditar;
                if (vigenciaAporte.VigenciaAporteId == null || vigenciaAporte.VigenciaAporteId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR VIGENCIA APORTE";
                    vigenciaAporte.FechaCreacion = DateTime.Now;
                    vigenciaAporte.Eliminado = false;
                    _context.VigenciaAporte.Add(vigenciaAporte);
                }
                else
                {
                    strCrearEditar = "EDITAR VIGENCIA APORTE";
                    VigenciaAporte VigenciaAporteAntigua = _context.VigenciaAporte.Find(vigenciaAporte.VigenciaAporteId);
                    //Auditoria
                    VigenciaAporteAntigua.UsuarioModificacion = vigenciaAporte.UsuarioCreacion;
                    VigenciaAporteAntigua.FechaModificacion = DateTime.Now;
                    //Registros
                    //VigenciaAporteAntigua.FuenteFinanciacion = vigenciaAporte.FuenteFinanciacion;
                    VigenciaAporteAntigua.TipoVigenciaCodigo = vigenciaAporte.TipoVigenciaCodigo;
                    VigenciaAporteAntigua.ValorAporte = vigenciaAporte.ValorAporte;

                    //_context.VigenciaAporte.Update(VigenciaAporteAntigua);
                }
                //await _context.SaveChangesAsync();

                return 
               new Respuesta
               {
                   IsSuccessful = true,
                   IsException = false,
                   IsValidation = false,
                   Code = ConstantMessagesFuentesFinanciacion.OperacionExitosa,
                   Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.OperacionExitosa, idAccionCrearVigenciaAporte, vigenciaAporte.UsuarioCreacion, strCrearEditar)
               };
            }
            catch (Exception ex)
            {
                return 
                       new Respuesta
                       {
                           IsSuccessful = false,
                           IsException = true,
                           IsValidation = false,
                           Code = ConstantMessagesFuentesFinanciacion.Error,
                           Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.Error, idAccionCrearVigenciaAporte, vigenciaAporte.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                       };
            }
        }

        public async Task<List<GrillaFuentesFinanciacion>> GetListFuentesFinanciacionByAportanteId(int aportanteId)
        {
            List<GrillaFuentesFinanciacion> ListaRetorno = new List<GrillaFuentesFinanciacion>(); 
            var financiaciones = _context.FuenteFinanciacion.Where(x=>x.AportanteId==aportanteId && x.Eliminado==false).ToList();
            foreach(var financiacion in financiaciones)
            {
                ListaRetorno.Add(new GrillaFuentesFinanciacion
                {
                    FuenteFinanciacionID = financiacion.FuenteFinanciacionId,
                    Fuente = _context.Dominio.Where(x => x.Codigo == financiacion.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre,
                    Nuevo_saldo_de_la_fuente = 0,
                    Saldo_actual_de_la_fuente= financiacion.ValorFuente,
                    Valor_solicitado_de_la_fuente=0
                }); 
            }
            return ListaRetorno;
        }

        public async Task<List<GrillaFuentesFinanciacion>> GetListFuentesFinanciacionByDisponibilidadPresupuestalProyectoid(int disponibilidadPresupuestalProyectoid,int aportanteID)
        {
            List<GrillaFuentesFinanciacion> ListaRetorno = new List<GrillaFuentesFinanciacion>();            
            var gestion = _context.GestionFuenteFinanciacion.Where(x => x.DisponibilidadPresupuestalProyectoId == disponibilidadPresupuestalProyectoid).Select(x => x.FuenteFinanciacionId).ToList();
            if(gestion.Count()==0)
            {
                gestion = _context.FuenteFinanciacion.Where(x => x.AportanteId == aportanteID && x.Eliminado == false).Select(x => x.FuenteFinanciacionId).ToList();
            }
            var financiaciones = _context.FuenteFinanciacion.Where(x => gestion.Contains(x.FuenteFinanciacionId) && x.Eliminado == false).ToList();
            foreach (var financiacion in financiaciones)
            {
                ListaRetorno.Add(new GrillaFuentesFinanciacion
                {
                    FuenteFinanciacionID = financiacion.FuenteFinanciacionId,
                    Fuente = _context.Dominio.Where(x => x.Codigo == financiacion.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre,
                    Nuevo_saldo_de_la_fuente = financiacion.ValorFuente - _context.GestionFuenteFinanciacion.Where(x => x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyectoId==disponibilidadPresupuestalProyectoid).Sum(x => x.ValorSolicitado),
                    Saldo_actual_de_la_fuente = financiacion.ValorFuente - _context.GestionFuenteFinanciacion.Where(x => x.FuenteFinanciacionId==financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyectoId == disponibilidadPresupuestalProyectoid).Sum(x=>x.ValorSolicitado),
                    Valor_solicitado_de_la_fuente = _context.GestionFuenteFinanciacion.Where(x => x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyectoId == disponibilidadPresupuestalProyectoid).Sum(x => x.ValorSolicitado)
                });
            }
            return ListaRetorno;
        }
    }
}
