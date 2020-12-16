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
            var retorno= await _context.FuenteFinanciacion.Where(r => r.FuenteFinanciacionId == id)
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
            if(retorno.Aportante.TipoAportanteId.Equals(ConstanTipoAportante.Ffie))
            {
                retorno.Aportante.NombreAportanteString = ConstanStringTipoAportante.Ffie;
            }
            else if (retorno.Aportante.TipoAportanteId.Equals(ConstanTipoAportante.Tercero))
            {
                retorno.Aportante.NombreAportanteString = retorno.Aportante.NombreAportanteId == null
                    ? "" :
                    _context.Dominio.Find(retorno.Aportante.NombreAportanteId).Nombre;                    
            }
            else
            {
                if(retorno.Aportante.MunicipioId==null)
                {
                    retorno.Aportante.NombreAportanteString = retorno.Aportante.DepartamentoId == null
                    ? "" :
                    "Gobernación " + _context.Localizacion.Find(retorno.Aportante.DepartamentoId).Descripcion;
                }
                else
                {
                    retorno.Aportante.NombreAportanteString = retorno.Aportante.MunicipioId == null
                    ? "" :
                    "Alcaldía " + _context.Localizacion.Find(retorno.Aportante.MunicipioId).Descripcion;
                }
            }

            return retorno;
        }

        public async Task<Respuesta> CreateEditFuentesFinanciacion(FuenteFinanciacion fuentefinanciacion)
        {
            
            BankAccountService bankAccountService = new BankAccountService(_context, _commonService);
            fuentefinanciacion.CofinanciacionDocumentoId = fuentefinanciacion.CofinanciacionDocumentoId == 0 ? null : fuentefinanciacion.CofinanciacionDocumentoId;
            int idAccionCrearFuentesFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Fuentes_Financiacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //antes de guardar extraigo el aportante porque solo voy a actualizar el rp
                if(fuentefinanciacion.Aportante!=null)
                {
                    var aportante = _context.CofinanciacionAportante.Find(fuentefinanciacion.AportanteId);
                    aportante.CuentaConRp = fuentefinanciacion.Aportante.CuentaConRp;
                    _context.CofinanciacionAportante.Update(aportante);
                    _context.SaveChanges();
                    fuentefinanciacion.Aportante = null;//si no depronto me actualiza el aportante dejando todo en nulo
                }
                fuentefinanciacion.RegistroCompleto = validarRegistroCompleto(fuentefinanciacion);
                if (fuentefinanciacion.FuenteFinanciacionId == null || fuentefinanciacion.FuenteFinanciacionId == 0)
                {
                    fuentefinanciacion.FechaCreacion = DateTime.Now;
                    fuentefinanciacion.Eliminado = false;
                    
                    _context.FuenteFinanciacion.Add(fuentefinanciacion);
                    _context.SaveChanges();
                }
                else
                {
                    FuenteFinanciacion fuente = _context.FuenteFinanciacion.Find(fuentefinanciacion.FuenteFinanciacionId);
                    fuente.FechaModificacion = DateTime.Now;
                    fuente.ValorFuente = fuentefinanciacion.ValorFuente;
                    fuente.RegistroCompleto = fuentefinanciacion.RegistroCompleto;
                    _context.FuenteFinanciacion.Update(fuente);
                    _context.SaveChanges();
                }

                foreach (VigenciaAporte vi in fuentefinanciacion.VigenciaAporte)
                {
                    vi.FuenteFinanciacionId = fuentefinanciacion.FuenteFinanciacionId;
                    vi.UsuarioCreacion = fuentefinanciacion.UsuarioCreacion==null? fuentefinanciacion.UsuarioModificacion.ToUpper():fuentefinanciacion.UsuarioCreacion.ToUpper();
                    vi.FechaCreacion = DateTime.Now;
                    await this.CreateEditarVigenciaAporte(vi);
                };

                foreach (CuentaBancaria cb in fuentefinanciacion.CuentaBancaria)
                {
                    cb.UsuarioCreacion = fuentefinanciacion.UsuarioCreacion == null ? fuentefinanciacion.UsuarioModificacion.ToUpper() : fuentefinanciacion.UsuarioCreacion.ToUpper();
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
                   Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.OperacionExitosa, idAccionCrearFuentesFinanciacion, fuentefinanciacion.UsuarioCreacion == null ? fuentefinanciacion.UsuarioModificacion.ToUpper() : fuentefinanciacion.UsuarioCreacion.ToUpper(), "CREAR FUENTES DE FINANCIACIÓN")
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

        private bool? validarRegistroCompleto(FuenteFinanciacion fuentefinanciacion)
        {
            bool retorno = true;
            if(fuentefinanciacion.ValorFuente==null)
            {
                retorno = false;
            }
            if(fuentefinanciacion.FuenteRecursosCodigo == null || fuentefinanciacion.FuenteRecursosCodigo.Equals(""))
            {
                retorno = false;
            }
            if(fuentefinanciacion.CuentaBancaria.Count()==0)
            {
                retorno = false;
            }
            else
            {
                foreach(var cuentas in fuentefinanciacion.CuentaBancaria)
                {
                    if(cuentas.BancoCodigo==null)
                    {
                        retorno = false;
                    }
                    if (cuentas.CodigoSifi == null)
                    {
                        retorno = false;
                    }
                    if (cuentas.NombreCuentaBanco == null)
                    {
                        retorno = false;
                    }
                    if (cuentas.TipoCuentaCodigo == null)
                    {
                        retorno = false;
                    }
                    if (cuentas.Exenta == null)
                    {
                        retorno = false;
                    }
                }

            }
            if(fuentefinanciacion.AportanteId!=null)
            {
                var aportante = _context.CofinanciacionAportante.Where(x => x.CofinanciacionAportanteId == fuentefinanciacion.AportanteId).FirstOrDefault();
                if (aportante.TipoAportanteId == ConstanTipoAportante.Ffie)
                {
                    if (fuentefinanciacion.VigenciaAporte.Count() == 0)
                    {
                        retorno = false;
                    }
                }
                else
                {
                    if(fuentefinanciacion.Aportante!=null)
                    {
                        if (fuentefinanciacion.Aportante.RegistroPresupuestal.Count() == 0)
                        {
                            retorno = false;
                        }
                    }   
                    else//osea que reviso rp, si no tiene esta incompleto
                    {
                        if(_context.RegistroPresupuestal.Where(x=>x.AportanteId==fuentefinanciacion.AportanteId).Count()==0)
                        {
                            retorno = false;
                        }                        
                    }
                }
            }   
            else
            {
                if (fuentefinanciacion.VigenciaAporte.Count() == 0)
                {
                    retorno = false;
                }
            }
            
            return retorno;
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
                          Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.EliminacionExitosa, idAccionEliminarFinanciacion, UsuarioModifico, "ELIMINAR FUENTE DE FINANCIACIÓN")
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
                        .Include(r => r.CofinanciacionDocumento)
                        .Include(r => r.Aportante)                        
                        .ThenInclude(r => r.RegistroPresupuestal)
                        .ToListAsync();
        }

        public async Task<List<FuenteFinanciacion>> GetListFuentesFinanciacion()
        {
            //jflorez, cambio esto para tener el retorno con todas las variables

            var retorno= await _context.FuenteFinanciacion.
                Where(r => !(bool)r.Eliminado).Distinct().Include(r => r.ControlRecurso).
                Include(r => r.CuentaBancaria).
                Include(r => r.VigenciaAporte).
                Include(r => r.Aportante).
                ThenInclude(r => r.RegistroPresupuestal).ToListAsync();
            foreach(var ret in retorno)
            {
                ret.Aportante.Cofinanciacion = _context.Cofinanciacion.Find(ret.Aportante.CofinanciacionId);
                if (ret.Aportante.TipoAportanteId == ConstanTipoAportante.Ffie)
                {
                    ret.Aportante.NombreAportanteString = ConstanStringTipoAportante.Ffie;
                }
                else if (ret.Aportante.TipoAportanteId == ConstanTipoAportante.ET)
                {
                    //verifico si tiene municipio
                    if (ret.Aportante.MunicipioId != null)
                    {
                        ret.Aportante.NombreAportanteString = ret.Aportante.MunicipioId==null?
                            "":"Alcaldía de "+_context.Localizacion.Find(ret.Aportante.MunicipioId).Descripcion;
                    }
                    else//solo departamento
                    {
                        ret.Aportante.NombreAportanteString = ret.Aportante.DepartamentoId == null ?
                            "" : "Gobernación de " + _context.Localizacion.Find(ret.Aportante.DepartamentoId).Descripcion;
                    }
                }
                else
                {
                    ret.Aportante.NombreAportanteString = _context.Dominio.Find(ret.Aportante.NombreAportanteId).Nombre;
                }
                ret.Aportante.TipoAportanteString = _context.Dominio.Find(ret.Aportante.TipoAportanteId).Nombre;
                ret.FuenteRecursosString = ret.FuenteRecursosCodigo == null ? "" : _context.Dominio.Where(x => x.Codigo == ret.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;
            }
            return retorno.OrderByDescending(x=>x.FechaCreacion).ToList();
        }

        public async Task<List<FuenteFinanciacion>> GetListFuentesFinanciacionshort()
        {
            //jflorez, cambio esto para tener el retorno con todas las variables

            var retorno = await _context.FuenteFinanciacion.
                Where(r => !(bool)r.Eliminado).
                Include(r => r.Aportante).ToListAsync();
            foreach (var ret in retorno)
            {
                ret.ControlRecurso = _context.ControlRecurso.Where(x=>x.FuenteFinanciacionId==ret.FuenteFinanciacionId && !(bool)x.Eliminado).ToList();
                foreach(var control in ret.ControlRecurso)
                {
                    control.FuenteFinanciacion = null;
                }
                ret.Aportante.Cofinanciacion = _context.Cofinanciacion.Find(ret.Aportante.CofinanciacionId);
                ret.Aportante.Cofinanciacion.CofinanciacionAportante = null;
                ret.Aportante.FuenteFinanciacion = null;
                if (ret.Aportante.TipoAportanteId == ConstanTipoAportante.Ffie)
                {
                    ret.Aportante.NombreAportanteString = ConstanStringTipoAportante.Ffie;
                }
                else if (ret.Aportante.TipoAportanteId == ConstanTipoAportante.ET)
                {
                    //verifico si tiene municipio
                    if (ret.Aportante.MunicipioId != null)
                    {
                        ret.Aportante.NombreAportanteString = ret.Aportante.MunicipioId == null ?
                            "" : "Alcaldía de " + _context.Localizacion.Find(ret.Aportante.MunicipioId).Descripcion;
                    }
                    else//solo departamento
                    {
                        ret.Aportante.NombreAportanteString = ret.Aportante.DepartamentoId == null ?
                            "" : "Gobernación de " + _context.Localizacion.Find(ret.Aportante.DepartamentoId).Descripcion;
                    }
                }
                else
                {
                    ret.Aportante.NombreAportanteString = _context.Dominio.Find(ret.Aportante.NombreAportanteId).Nombre;
                }
                ret.Aportante.TipoAportanteString = _context.Dominio.Find(ret.Aportante.TipoAportanteId).Nombre;
                ret.FuenteRecursosString = ret.FuenteRecursosCodigo == null ? "" : _context.Dominio.Where(x => x.Codigo == ret.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;
            }
            return retorno.OrderByDescending(x => x.FechaCreacion).ToList();
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
                    Saldo_actual_de_la_fuente= (decimal)financiacion.ValorFuente,
                    Valor_solicitado_de_la_fuente=0
                }); 
            }
            return ListaRetorno;
        }

        public async Task<List<GrillaFuentesFinanciacion>> GetListFuentesFinanciacionByDisponibilidadPresupuestalProyectoid(int disponibilidadPresupuestalProyectoid,int aportanteID)
        {
            List<GrillaFuentesFinanciacion> ListaRetorno = new List<GrillaFuentesFinanciacion>();            
            var gestion = _context.FuenteFinanciacion.Where(x => x.AportanteId == aportanteID && x.Eliminado == false).Select(x => x.FuenteFinanciacionId).ToList();
            
            var financiaciones = _context.FuenteFinanciacion.Where(x => gestion.Contains(x.FuenteFinanciacionId) && x.Eliminado == false).ToList();
            foreach (var financiacion in financiaciones)
            {
                var valorDisponible = (decimal)financiacion.ValorFuente-_context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyectoId != disponibilidadPresupuestalProyectoid).Sum(x => x.ValorSolicitado);
                var valorsolicitado = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId
                     && x.DisponibilidadPresupuestalProyectoId == disponibilidadPresupuestalProyectoid).Sum(x => x.ValorSolicitado);
                ListaRetorno.Add(new GrillaFuentesFinanciacion
                {
                    FuenteFinanciacionID = financiacion.FuenteFinanciacionId,
                    Fuente = _context.Dominio.Where(x => x.Codigo == financiacion.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre,
                    Nuevo_saldo_de_la_fuente = valorDisponible-valorsolicitado,
                    Saldo_actual_de_la_fuente = valorDisponible,
                    Valor_solicitado_de_la_fuente =valorsolicitado,
                    GestionFuenteFinanciacionID = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId
                     && x.DisponibilidadPresupuestalProyectoId == disponibilidadPresupuestalProyectoid).Select(x => x.GestionFuenteFinanciacionId).FirstOrDefault(),
                });
            }
            return ListaRetorno;
        }

        public async Task GetConsignationValue(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            var fuentes = _context.FuenteFinanciacion.Where(x => (bool)x.RegistroCompleto).Include(x=>x.ControlRecurso).Include(x=>x.Aportante).ThenInclude(x=>x.Cofinanciacion).ToList();
            foreach(var fuente in fuentes)
            {
                decimal valoracompara = 0;
                foreach(var control in fuente.ControlRecurso)
                {
                    valoracompara += control.ValorConsignacion;
                }
                if (valoracompara != fuente.ValorFuente)
                {
                    //envio correo a cordinandor de financiera
                    var usuariosadmin = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Financiera).Include(y => y.Usuario);
                    Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjCoordinadorFuentes);
                    string nombre = getNameAportante(fuente.Aportante);
                    string template = TemplateRecoveryPassword.Contenido.Replace("[vigencia]", fuente.Aportante.Cofinanciacion.VigenciaCofinanciacionId.ToString()).Replace("_LinkF_", pDominioFront).Replace("[aportante]", nombre).Replace("[valor1]", string.Format("{0:c}", valoracompara.ToString())).Replace("[valor2]", string.Format("{0:c}", fuente.ValorFuente.ToString()));
                    bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuariosadmin.FirstOrDefault().Usuario.Email, "Fuentes de financiación", template, pSender, pPassword, pMailServer, pMailPort);
                }       
            }
        }

        private string getNameAportante(CofinanciacionAportante aportante)
        {
            string nombre = "";
            if (aportante.TipoAportanteId == ConstanTipoAportante.Ffie)
            {
                nombre = ConstanStringTipoAportante.Ffie;
            }
            else if (aportante.TipoAportanteId == ConstanTipoAportante.ET)
            {
                //verifico si tiene municipio
                if (aportante.MunicipioId != null)
                {
                    nombre = aportante.MunicipioId == null ?
                        "" : "Alcaldía de " + _context.Localizacion.Find(aportante.MunicipioId).Descripcion;
                }
                else//solo departamento
                {
                    nombre = aportante.DepartamentoId == null ?
                        "" : "Gobernación de " + _context.Localizacion.Find(aportante.DepartamentoId).Descripcion;
                }
            }
            else
            {
                nombre = _context.Dominio.Find(aportante.NombreAportanteId).Nombre;
            }
            return nombre;
        }
        
        public async Task<List<GrillaFuentesFinanciacion>> GetListFuentesFinanciacionByDisponibilidadPresupuestald(int disponibilidadPresupuestaId)
        {
            List<GrillaFuentesFinanciacion> ListaRetorno = new List<GrillaFuentesFinanciacion>();
            var gestion = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == disponibilidadPresupuestaId).Select(x => x.FuenteFinanciacionId).ToList();
            if (gestion.Count() == 0)
            {
                var disponibilidad = _context.DisponibilidadPresupuestalProyecto.Where(x => x.DisponibilidadPresupuestalId == disponibilidadPresupuestaId).
                    Include(x => x.ProyectoAdministrativo).
                        ThenInclude(x => x.ProyectoAdministrativoAportante).
                            ThenInclude(x => x.AportanteFuenteFinanciacion).
                   Include(x=>x.Proyecto).
                    ThenInclude(x => x.ProyectoAportante).
                        ThenInclude(x => x.Aportante).
                            ThenInclude(x => x.FuenteFinanciacion).ToList();
                if(_context.DisponibilidadPresupuestal.Where(x=>x.DisponibilidadPresupuestalId==disponibilidadPresupuestaId).
                    FirstOrDefault().TipoSolicitudCodigo==ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)//entonces es una disponivlidad espacial
                {
                    var ddp = _context.DisponibilidadPresupuestal.Where(x => x.DisponibilidadPresupuestalId == disponibilidadPresupuestaId).
                        Include(x => x.Aportante).
                            ThenInclude(x => x.FuenteFinanciacion).ToList();
                    foreach (var d in ddp)
                    {
                        foreach (var fuente in d.Aportante.FuenteFinanciacion)
                        {
                            if (fuente.FuenteFinanciacionId != null)
                            {
                                gestion.Add(Convert.ToInt32(fuente.FuenteFinanciacionId));
                            }
                        }
                    }
                }
                foreach (var dDisponibilidadProyecto in disponibilidad)
                {
                    if(dDisponibilidadProyecto.ProyectoAdministrativoId!=null)
                    {
                        foreach (var pAdminsitrativoApo in dDisponibilidadProyecto.ProyectoAdministrativo.ProyectoAdministrativoAportante)
                        {
                            foreach (var d in pAdminsitrativoApo.AportanteFuenteFinanciacion)
                            {
                                if (d.FuenteFinanciacionId != null)
                                {
                                    gestion.Add(Convert.ToInt32(d.FuenteFinanciacionId));
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var pAdminsitrativoApo in dDisponibilidadProyecto.Proyecto.ProyectoAportante)
                        {
                            foreach (var d in pAdminsitrativoApo.Aportante.FuenteFinanciacion)
                            {
                                if (d.FuenteFinanciacionId != null)
                                {
                                    gestion.Add(Convert.ToInt32(d.FuenteFinanciacionId));
                                }
                            }
                        }
                    }
                    
                }                
            }
            var financiaciones = _context.FuenteFinanciacion.Where(x => gestion.Contains(x.FuenteFinanciacionId) && x.Eliminado == false).ToList();
            
            foreach (var financiacion in financiaciones)
            {
                var gestionfienteid= _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == disponibilidadPresupuestaId).Select(x => x.GestionFuenteFinanciacionId);
                int gestionid = 0;
                if(gestionfienteid!=null)
                {
                    gestionid = gestionfienteid.FirstOrDefault();
                }
                decimal valor = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == disponibilidadPresupuestaId).Sum(x => x.ValorSolicitado);
                if(valor==0)//si es disponibilidad especial entonces la relacion es diferente
                {
                    var gestionfiente = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalId == disponibilidadPresupuestaId).Select(x => x.GestionFuenteFinanciacionId);
                    if (gestionfiente != null)
                    {
                        gestionid = gestionfiente.FirstOrDefault();
                    }
                    valor = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalId == disponibilidadPresupuestaId).Sum(x => x.ValorSolicitado);
                }
                var saldoFuente = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId != disponibilidadPresupuestaId).Sum(x => x.ValorSolicitado);
                //si no he gestionado fuentes, soy especial y mi saldo es el total de la fuente
                if(saldoFuente==0)
                {
                    saldoFuente = Convert.ToDecimal(financiacion.ValorFuente);
                }
                ListaRetorno.Add(new GrillaFuentesFinanciacion
                {
                    GestionFuenteFinanciacionID= gestionid,
                    FuenteFinanciacionID = financiacion.FuenteFinanciacionId,
                    Fuente = _context.Dominio.Where(x => x.Codigo == financiacion.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre,
                    Nuevo_saldo_de_la_fuente = saldoFuente-valor,
                    Saldo_actual_de_la_fuente = saldoFuente,
                    Valor_solicitado_de_la_fuente = valor
                });
            }
            return ListaRetorno;
        }
    }
}
