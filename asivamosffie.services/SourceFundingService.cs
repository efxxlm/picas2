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
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class SourceFundingService : ISourceFundingService
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public SourceFundingService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<List<VSaldosFuenteXaportanteId>> GetVSaldosFuenteXaportanteId(int pAportanteId)
        {
            return await _context.VSaldosFuenteXaportanteId.Where(r => r.CofinanciacionAportanteId == pAportanteId).ToListAsync();
        }

        public async Task<List<FuenteFinanciacion>> GetISourceFunding()
        {
            return await _context.FuenteFinanciacion.ToListAsync();
        }

        public async Task<FuenteFinanciacion> GetISourceFundingById(int id)
        {
            var retorno = await _context.FuenteFinanciacion.Where(r => r.FuenteFinanciacionId == id)
                        //.Include(r => r.ControlRecurso)
                        .Include(r => r.CuentaBancaria)
                        .Include(r => r.VigenciaAporte)
                        .Include(r => r.Aportante)
                        .ThenInclude(apo => apo.RegistroPresupuestal)
                        .Include(r => r.Aportante)
                        .ThenInclude(apo => apo.Cofinanciacion)
                        .Include(r => r.Aportante)
                        .ThenInclude(apo => apo.CofinanciacionDocumento)
                        .FirstOrDefaultAsync();
            if (retorno != null)
            {
                List<ControlRecurso> cr = _context.ControlRecurso.Where(r => r.Eliminado != true && r.FuenteFinanciacionId == retorno.FuenteFinanciacionId).ToList();
                retorno.ControlRecurso = cr;
            }

            if (retorno.Aportante.TipoAportanteId.Equals(ConstanTipoAportante.Ffie))
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
                if (retorno.Aportante.MunicipioId == null)
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
                if (fuentefinanciacion.Aportante != null)
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
                    fuente.Eliminado = fuentefinanciacion.Eliminado;

                    _context.FuenteFinanciacion.Update(fuente);
                    _context.SaveChanges();
                }

                foreach (VigenciaAporte vi in fuentefinanciacion.VigenciaAporte)
                {
                    vi.FuenteFinanciacionId = fuentefinanciacion.FuenteFinanciacionId;
                    vi.UsuarioCreacion = fuentefinanciacion.UsuarioCreacion == null ? fuentefinanciacion.UsuarioModificacion.ToUpper() : fuentefinanciacion.UsuarioCreacion.ToUpper();
                    vi.FechaCreacion = DateTime.Now;
                    await this.CreateEditarVigenciaAporte(vi);
                };

                foreach (CuentaBancaria cb in fuentefinanciacion.CuentaBancaria)
                {
                    cb.UsuarioCreacion = fuentefinanciacion.UsuarioCreacion == null ? fuentefinanciacion.UsuarioModificacion.ToUpper() : fuentefinanciacion.UsuarioCreacion.ToUpper();
                    cb.FechaCreacion = DateTime.Now;
                    cb.Eliminado = false;
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
            if (fuentefinanciacion.ValorFuente == null)
            {
                retorno = false;
            }
            if (fuentefinanciacion.FuenteRecursosCodigo == null || fuentefinanciacion.FuenteRecursosCodigo.Equals(""))
            {
                retorno = false;
            }
            if (fuentefinanciacion.CuentaBancaria.Count() == 0)
            {
                retorno = false;
            }
            else
            {
                foreach (var cuentas in fuentefinanciacion.CuentaBancaria)
                {
                    if (cuentas.BancoCodigo == null)
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
            if (fuentefinanciacion.AportanteId != null)
            {
                var aportante = _context.CofinanciacionAportante.Where(x => x.CofinanciacionAportanteId == fuentefinanciacion.AportanteId).FirstOrDefault();
                if (aportante.TipoAportanteId == ConstanTipoAportante.Ffie)
                {
                    if (fuentefinanciacion.VigenciaAporte.Count() == 0)
                    {
                        retorno = false;
                    }

                    fuentefinanciacion.VigenciaAporte.Where(x => x.Eliminado != true).ToList().ForEach(vig =>
                  {
                      if (
                               vig.ValorAporte == null ||
                               vig.ValorAporte == 0 ||
                               vig.VigenciaAporteId == null
                      )
                      {
                          retorno = false;
                      }
                  });
                }
                else
                {
                    if (fuentefinanciacion.Aportante != null)
                    {
                        if (fuentefinanciacion.Aportante.RegistroPresupuestal.Count() == 0)
                        {
                            retorno = false;
                        }
                    }
                    else//osea que reviso rp, si no tiene esta incompleto
                    {
                        if (_context.RegistroPresupuestal.Where(x => x.AportanteId == fuentefinanciacion.AportanteId).Count() == 0)
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

        public async Task<Respuesta> ValidaEliminarFuenteFinanciancion(int id, string UsuarioModifico)
        {
            int idAccionEliminarFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Fuentes_Financiacion, (int)EnumeratorTipoDominio.Acciones);

            //verifico que no tenga relación con proyectos de infraestructura
            var proyectoFuentes = _context.ProyectoFuentes.Where(x => x.FuenteId == id && !(bool)x.Eliminado).Count();
            if (proyectoFuentes > 0)
            {
                return
                  new Respuesta
                  {
                      IsSuccessful = true,
                      IsException = false,
                      IsValidation = true,
                      Code = ConstantMessagesFuentesFinanciacion.EliminacionFallida,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.EliminacionFallida, idAccionEliminarFinanciacion, UsuarioModifico, "ELIMINAR FUENTE DE FINANCIACIÓN IMPOSIBLE")
                  };
            }
            //verifico que no tenga control de recursos
            var proyectoFuentesCR = _context.ControlRecurso.Where(x => x.FuenteFinanciacionId == id && x.Eliminado != true).Count();
            if (proyectoFuentesCR > 0)
            {
                return
                  new Respuesta
                  {
                      IsSuccessful = true,
                      IsException = false,
                      IsValidation = true,
                      Code = ConstantMessagesFuentesFinanciacion.EliminarFuenteConControlRecursos,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.EliminarFuenteConControlRecursos, idAccionEliminarFinanciacion, UsuarioModifico, "ELIMINAR FUENTE DE FINANCIACIÓN IMPOSIBLE")
                  };
            }

            //verifico que no tenga relación con proyectos administrativos
            var proyectoFuentesad = _context.AportanteFuenteFinanciacion.Where(x => x.FuenteFinanciacionId == id && !(bool)x.Eliminado).Count();
            if (proyectoFuentesad > 0)
            {
                return
                  new Respuesta
                  {
                      IsSuccessful = true,
                      IsException = false,
                      IsValidation = true,
                      Code = ConstantMessagesFuentesFinanciacion.EliminacionFallida,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.EliminacionFallida, idAccionEliminarFinanciacion, UsuarioModifico, "ELIMINAR FUENTE DE FINANCIACIÓN IMPOSIBLE")
                  };
            }

            return
                  new Respuesta
                  {
                      IsSuccessful = true,
                      IsException = false,
                      IsValidation = false
                  };
        }

        public async Task<Respuesta> EliminarFuentesFinanciacion(int id, string UsuarioModifico)
        {

            int idAccionEliminarFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Fuentes_Financiacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                var validationResponse = await ValidaEliminarFuenteFinanciancion(id, UsuarioModifico);
                if (validationResponse.IsValidation)
                {
                    return validationResponse;
                }

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

        public async Task<Respuesta> EliminarFuentesFinanciacionCompleto(int id, string UsuarioModifico)
        {

            int idAccionEliminarFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Fuentes_Financiacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //verifico que no tenga relación con proyectos de infraestructura
                var proyectoFuentes = _context.ProyectoFuentes.Where(x => x.FuenteId == id && !(bool)x.Eliminado).Count();
                if (proyectoFuentes > 0)
                {
                    return
                      new Respuesta
                      {
                          IsSuccessful = true,
                          IsException = false,
                          IsValidation = false,
                          Code = ConstantMessagesFuentesFinanciacion.EliminacionFallida,
                          Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.EliminacionFallida, idAccionEliminarFinanciacion, UsuarioModifico, "ELIMINAR FUENTE DE FINANCIACIÓN IMPOSIBLE")
                      };
                }
                //verifico que no tenga control de recursos
                var proyectoFuentesCR = _context.ControlRecurso.Where(x => x.FuenteFinanciacionId == id && x.Eliminado != true).Count();
                if (proyectoFuentesCR > 0)
                {
                    return
                      new Respuesta
                      {
                          IsSuccessful = true,
                          IsException = false,
                          IsValidation = false,
                          Code = ConstantMessagesFuentesFinanciacion.EliminarFuenteConControlRecursos,
                          Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.EliminarFuenteConControlRecursos, idAccionEliminarFinanciacion, UsuarioModifico, "ELIMINAR FUENTE DE FINANCIACIÓN IMPOSIBLE")
                      };
                }

                //verifico que no tenga relación con proyectos administrativos
                var proyectoFuentesad = _context.AportanteFuenteFinanciacion.Where(x => x.FuenteFinanciacionId == id && !(bool)x.Eliminado).Count();
                if (proyectoFuentesad > 0)
                {
                    return
                      new Respuesta
                      {
                          IsSuccessful = true,
                          IsException = false,
                          IsValidation = false,
                          Code = ConstantMessagesFuentesFinanciacion.EliminacionFallida,
                          Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.EliminacionFallida, idAccionEliminarFinanciacion, UsuarioModifico, "ELIMINAR FUENTE DE FINANCIACIÓN IMPOSIBLE")
                      };
                }

                // Elimino la fuente
                var entity = await _context.FuenteFinanciacion.FindAsync(id);

                entity.FechaModificacion = DateTime.Now;
                entity.UsuarioModificacion = UsuarioModifico;
                entity.Eliminado = true;

                // Elimino los registros presupuestales asociados
                List<RegistroPresupuestal> listaRPs = _context.RegistroPresupuestal.Where(r => r.AportanteId == entity.AportanteId).ToList();

                foreach (RegistroPresupuestal RP in listaRPs)
                {
                    RP.FechaModificacion = DateTime.Now;
                    RP.UsuarioModificacion = UsuarioModifico;
                    RP.Eliminado = true;
                }

                // dejo el aportante libre
                CofinanciacionAportante cofinanciacionAportante = _context.CofinanciacionAportante.Find(entity.AportanteId);

                cofinanciacionAportante.CuentaConRp = false;

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
            var res = await _context.FuenteFinanciacion.Where(r => !(bool)r.Eliminado)
                        .Where(r => r.AportanteId == AportanteId)
                        //.Include(r => r.ControlRecurso)
                        .Include(r => r.CofinanciacionDocumento)
                        .Include(r => r.Aportante)
                        .ThenInclude(r => r.RegistroPresupuestal)
                        .IncludeFilter(r => r.CuentaBancaria.Where(r => !(bool)r.Eliminado))
                        .IncludeFilter(r => r.VigenciaAporte.Where(r => !(bool)r.Eliminado))
                        .ToListAsync();
            foreach (var item in res)
            {
                List<ControlRecurso> cr = _context.ControlRecurso.Where(r => r.Eliminado != true && r.FuenteFinanciacionId == item.FuenteFinanciacionId).ToList();
                item.ControlRecurso = cr;
            }
            return res;
        }

        public async Task<List<FuenteFinanciacion>> GetListFuentesFinanciacion()
        {
            //jflorez, cambio esto para tener el retorno con todas las variables

            var retorno = await _context.FuenteFinanciacion.
                Where(r => !(bool)r.Eliminado).Distinct().Include(r => r.ControlRecurso).
                Include(r => r.CuentaBancaria).
                IncludeFilter(r => r.VigenciaAporte.Where(r => !(bool)r.Eliminado)).
                Include(r => r.Aportante).
                ThenInclude(r => r.RegistroPresupuestal).ToListAsync();
            foreach (var ret in retorno)
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

        public async Task<List<FuenteFinanciacion>> GetListFuentesFinanciacionshort()
        {
            //jflorez, cambio esto para tener el retorno con todas las variables

            var retorno = await _context.FuenteFinanciacion.
                Where(r => !(bool)r.Eliminado).
                Include(r => r.Aportante)
                    .ThenInclude(r => r.Cofinanciacion)
                    .ToListAsync();

            List<ControlRecurso> controlRecursos = _context.ControlRecurso.Where(x => !(bool)x.Eliminado).ToList();
            List<Localizacion> localizacions = _context.Localizacion.ToList();
            List<Dominio> dominiosFuentes = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).ToList();
            //List<Dominio> dominiosTipoAportante = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_aportante).ToList();
            List<Dominio> dominiosNombreAportante = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Nombre_Aportante_Aportante).ToList();

            foreach (var ret in retorno)
            {
                ret.ControlRecurso = controlRecursos.Where(x => x.FuenteFinanciacionId == ret.FuenteFinanciacionId).ToList();
                foreach (var control in ret.ControlRecurso)
                {
                    control.FuenteFinanciacion = null;
                }
                //ret.Aportante.Cofinanciacion = _context.Cofinanciacion.Find(ret.Aportante.CofinanciacionId);
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
                            "" : "Alcaldía de " + localizacions.Where(r => r.LocalizacionId == ret.Aportante.MunicipioId)?.FirstOrDefault()?.Descripcion;
                    }
                    else//solo departamento
                    {
                        ret.Aportante.NombreAportanteString = ret.Aportante.DepartamentoId == null ?
                            "" : "Gobernación de " + localizacions.Where(r => r.LocalizacionId == ret.Aportante.DepartamentoId)?.FirstOrDefault()?.Descripcion;
                    }
                }
                else
                {
                    ret.Aportante.NombreAportanteString = dominiosNombreAportante.Where(r => r.DominioId == ret.Aportante.NombreAportanteId)?.FirstOrDefault()?.Nombre;
                }

                // Diego, se quema por que la consulta se demora mucho
                if (ret.Aportante.TipoAportanteId == 6)
                    ret.Aportante.TipoAportanteString = "FFIE";
                else if (ret.Aportante.TipoAportanteId == 9)
                    ret.Aportante.TipoAportanteString = "ET";
                else if (ret.Aportante.TipoAportanteId == 10)
                    ret.Aportante.TipoAportanteString = "Tercero";

                //ret.Aportante.TipoAportanteString = dominiosTipoAportante.Where(r => r.DominioId == ret.Aportante.TipoAportanteId)?.FirstOrDefault()?.Nombre;
                ret.FuenteRecursosString = ret.FuenteRecursosCodigo == null ? "" : dominiosFuentes.Where(x => x.Codigo == ret.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;
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
                    VigenciaAporteAntigua.Eliminado = vigenciaAporte.Eliminado;

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
            var financiaciones = _context.FuenteFinanciacion.Where(x => x.AportanteId == aportanteId && x.Eliminado == false).ToList();
            foreach (var financiacion in financiaciones)
            {
                ListaRetorno.Add(new GrillaFuentesFinanciacion
                {
                    FuenteFinanciacionID = financiacion.FuenteFinanciacionId,
                    Fuente = _context.Dominio.Where(x => x.Codigo == financiacion.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre,
                    Nuevo_saldo_de_la_fuente = 0,
                    Saldo_actual_de_la_fuente = (decimal)financiacion.ValorFuente,
                    Valor_solicitado_de_la_fuente = 0
                });
            }
            return ListaRetorno;
        }

        public async Task<List<GrillaFuentesFinanciacion>> GetListFuentesFinanciacionByDisponibilidadPresupuestalProyectoid(int disponibilidadPresupuestalProyectoid, int aportanteID)
        {
            List<GrillaFuentesFinanciacion> ListaRetorno = new List<GrillaFuentesFinanciacion>();
            List<int> listaFuentesProyecto = new List<int>();

            DisponibilidadPresupuestalProyecto disponibilidadProyecto = _context.DisponibilidadPresupuestalProyecto.Find(disponibilidadPresupuestalProyectoid);

            DisponibilidadPresupuestal disponibilidadPresupuestal = _context.DisponibilidadPresupuestal
                                                                                .Where(x => x.DisponibilidadPresupuestalId == disponibilidadProyecto.DisponibilidadPresupuestalId)
                                                                                .Include(x => x.Contratacion)
                                                                                    .ThenInclude(x => x.ContratacionProyecto)
                                                                                .FirstOrDefault();

            ContratacionProyecto contratacionProyecto = disponibilidadPresupuestal.Contratacion.ContratacionProyecto
                                                                                                    .Where(x => x.ProyectoId == disponibilidadProyecto.ProyectoId)
                                                                                                    .FirstOrDefault();

            if (contratacionProyecto != null)
            {
                ContratacionProyectoAportante aportante = _context.ContratacionProyectoAportante
                                                                        .Where(x => x.CofinanciacionAportanteId == aportanteID &&
                                                                               x.ContratacionProyectoId == contratacionProyecto.ContratacionProyectoId)
                                                                        .Include(x => x.ComponenteAportante)
                                                                            .ThenInclude(x => x.ComponenteUso)
                                                                        .FirstOrDefault();

                if (aportante != null)
                {
                    aportante.ComponenteAportante.ToList().ForEach(ca =>
                   {
                       ca.ComponenteUso.ToList().ForEach(cu =>
                      {
                          if (cu.FuenteFinanciacionId.HasValue)
                              listaFuentesProyecto.Add(cu.FuenteFinanciacionId.Value);
                      });
                   });
                }
            }

            var gestion = _context.FuenteFinanciacion.Where(x => x.AportanteId == aportanteID && x.Eliminado == false).Select(x => x.FuenteFinanciacionId).ToList();

            var financiaciones = _context.FuenteFinanciacion.Where(x => gestion.Contains(x.FuenteFinanciacionId) && x.Eliminado == false).ToList();

            financiaciones = financiaciones.Where(x => listaFuentesProyecto.Contains(x.FuenteFinanciacionId) && x.Eliminado == false).ToList();


            foreach (var financiacion in financiaciones)
            {
                List<GestionFuenteFinanciacion> gestionFuenteFinanciacion = _context.GestionFuenteFinanciacion
                                                                                    .Where(x => !(bool)x.Eliminado &&
                                                                                           x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId &&
                                                                                           x.DisponibilidadPresupuestalProyectoId == disponibilidadPresupuestalProyectoid &&
                                                                                           x.Eliminado != true)
                                                                                    .ToList();
                var gestionAlGuardar = _context.GestionFuenteFinanciacion
                    .Where(x => x.DisponibilidadPresupuestalProyectoId == disponibilidadPresupuestalProyectoid &&
                                x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.Eliminado != true)
                    .FirstOrDefault();

                decimal valorDisponible = 0;

                // cuando ya se guardó
                if (gestionFuenteFinanciacion.Count() > 0)
                {
                    valorDisponible = gestionFuenteFinanciacion
                                                    .Sum(x => x.SaldoActual);
                }
                else
                {
                    valorDisponible = (decimal)financiacion.ValorFuente - _context.GestionFuenteFinanciacion
                                                                                        .Where(x => !(bool)x.Eliminado &&
                                                                                               x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId &&
                                                                                               x.DisponibilidadPresupuestalProyectoId != disponibilidadPresupuestalProyectoid)
                                                                                        .Sum(x => x.ValorSolicitado);
                }



                decimal valorsolicitado = 0;

                // cuando ya se guardó
                if (gestionFuenteFinanciacion.Count() > 0)
                {
                    valorsolicitado = gestionFuenteFinanciacion
                                                    .Sum(x => x.ValorSolicitado);
                }



                ListaRetorno.Add(new GrillaFuentesFinanciacion
                {
                    FuenteFinanciacionID = financiacion.FuenteFinanciacionId,
                    Fuente = _context.Dominio.Where(x => x.Codigo == financiacion.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre,
                    Nuevo_saldo_de_la_fuente = valorDisponible - valorsolicitado,
                    Saldo_actual_de_la_fuente = valorDisponible,
                    Valor_solicitado_de_la_fuente = valorsolicitado,
                    GestionFuenteFinanciacionID = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId
                     && x.DisponibilidadPresupuestalProyectoId == disponibilidadPresupuestalProyectoid).Select(x => x.GestionFuenteFinanciacionId).FirstOrDefault(),
                    Nuevo_saldo_de_la_fuente_al_guardar = gestionAlGuardar != null ? gestionAlGuardar.NuevoSaldo : 0,
                    Saldo_actual_de_la_fuente_al_guardar = gestionAlGuardar != null ? gestionAlGuardar.SaldoActual : 0,

                });
            }
            return ListaRetorno;
        }

        public List<GrillaFuentesFinanciacion> GetListFuentesFinanciacionByDisponibilidadPresupuestalProyectoid(int disponibilidadPresupuestalProyectoid, int aportanteID, bool esNovedad, int novedadContractualRegistroPresupuestalId)
        {
            List<GrillaFuentesFinanciacion> ListaRetorno = new List<GrillaFuentesFinanciacion>();
            List<int> listaFuentesProyecto = new List<int>();

            DisponibilidadPresupuestalProyecto disponibilidadProyecto = _context.DisponibilidadPresupuestalProyecto.Find(disponibilidadPresupuestalProyectoid);

            DisponibilidadPresupuestal disponibilidadPresupuestal = _context.DisponibilidadPresupuestal
                                                                                .Where(x => x.DisponibilidadPresupuestalId == disponibilidadProyecto.DisponibilidadPresupuestalId)
                                                                                .Include(x => x.Contratacion)
                                                                                    .ThenInclude(x => x.ContratacionProyecto)
                                                                                .FirstOrDefault();

            ContratacionProyecto contratacionProyecto = disponibilidadPresupuestal.Contratacion.ContratacionProyecto
                                                                                                    .Where(x => x.ProyectoId == disponibilidadProyecto.ProyectoId)
                                                                                                    .FirstOrDefault();

            if (contratacionProyecto != null)
            {
                ContratacionProyectoAportante aportante = _context.ContratacionProyectoAportante
                                                                        .Where(x => x.CofinanciacionAportanteId == aportanteID &&
                                                                               x.ContratacionProyectoId == contratacionProyecto.ContratacionProyectoId)
                                                                        .Include(x => x.ComponenteAportante)
                                                                            .ThenInclude(x => x.ComponenteUso)
                                                                        .FirstOrDefault();

                if (aportante != null)
                {
                    aportante.ComponenteAportante.ToList().ForEach(ca =>
                    {
                        ca.ComponenteUso.ToList().ForEach(cu =>
                        {
                            if (cu.FuenteFinanciacionId.HasValue)
                                listaFuentesProyecto.Add(cu.FuenteFinanciacionId.Value);
                        });
                    });
                }
            }

            var gestion = _context.FuenteFinanciacion.Where(x => x.AportanteId == aportanteID && x.Eliminado == false).Select(x => x.FuenteFinanciacionId).ToList();

            var financiaciones = _context.FuenteFinanciacion.Where(x => gestion.Contains(x.FuenteFinanciacionId) && x.Eliminado == false).ToList();

            financiaciones = financiaciones.Where(x => listaFuentesProyecto.Contains(x.FuenteFinanciacionId) && x.Eliminado == false).ToList();


            foreach (var financiacion in financiaciones)
            {
                List<GestionFuenteFinanciacion> gestionFuenteFinanciacion = _context.GestionFuenteFinanciacion
                                                                                    .Where(x => x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId &&
                                                                                           x.DisponibilidadPresupuestalProyectoId == disponibilidadPresupuestalProyectoid &&
                                                                                           x.EsNovedad == esNovedad &&
                                                                                           x.NovedadContractualRegistroPresupuestalId == novedadContractualRegistroPresupuestalId &&
                                                                                           x.Eliminado != true
                                                                                           )
                                                                                    .ToList();
                var gestionAlGuardar = _context.GestionFuenteFinanciacion
                    .Where(x => x.DisponibilidadPresupuestalProyectoId == disponibilidadPresupuestalProyectoid &&
                                x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId &&
                                x.EsNovedad == esNovedad &&
                                x.NovedadContractualRegistroPresupuestalId == novedadContractualRegistroPresupuestalId &&
                                x.Eliminado != true
                                )
                    .FirstOrDefault();

                decimal valorDisponible = 0;

                // cuando ya se guardó
                if (gestionFuenteFinanciacion.Count() > 0)
                {
                    valorDisponible = gestionFuenteFinanciacion
                                                    .Sum(x => x.SaldoActual);
                }
                else
                {
                    valorDisponible = (decimal)financiacion.ValorFuente - _context.GestionFuenteFinanciacion
                                                                                        .Where(x => x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId &&
                                                                                               x.DisponibilidadPresupuestalProyectoId != disponibilidadPresupuestalProyectoid &&
                                                                                               x.EsNovedad == esNovedad &&
                                                                                               x.NovedadContractualRegistroPresupuestalId == novedadContractualRegistroPresupuestalId &&
                                                                                               x.Eliminado != true)
                                                                                        .Sum(x => x.ValorSolicitado);
                }



                decimal valorsolicitado = 0;

                // cuando ya se guardó
                if (gestionFuenteFinanciacion.Count() > 0)
                {
                    valorsolicitado = gestionFuenteFinanciacion
                                                    .Sum(x => x.ValorSolicitado);
                }



                ListaRetorno.Add(new GrillaFuentesFinanciacion
                {
                    FuenteFinanciacionID = financiacion.FuenteFinanciacionId,
                    Fuente = _context.Dominio.Where(x => x.Codigo == financiacion.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre,
                    Nuevo_saldo_de_la_fuente = valorDisponible - valorsolicitado,
                    Saldo_actual_de_la_fuente = valorDisponible,
                    Valor_solicitado_de_la_fuente = valorsolicitado,
                    GestionFuenteFinanciacionID = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId
                     && x.DisponibilidadPresupuestalProyectoId == disponibilidadPresupuestalProyectoid).Select(x => x.GestionFuenteFinanciacionId).FirstOrDefault(),
                    Nuevo_saldo_de_la_fuente_al_guardar = gestionAlGuardar != null ? gestionAlGuardar.NuevoSaldo  : 0,
                    Saldo_actual_de_la_fuente_al_guardar = gestionAlGuardar != null ? gestionAlGuardar.SaldoActual : 0,

                });
            }
            return ListaRetorno;
        }

        public async Task<List<GrillaFuentesFinanciacion>> GetListFuentesFinanciacionByNovedadContractualRegistroPresupuestal(int NovedadContractualRegistroPresupuestalId, int aportanteID)
        {
            List<GrillaFuentesFinanciacion> ListaRetorno = new List<GrillaFuentesFinanciacion>();
            List<int> listaIds = new List<int>();

            NovedadContractualRegistroPresupuestal novedadContractualRegistroPresupuestal = _context.NovedadContractualRegistroPresupuestal
                                                                                                        .Where(x => x.NovedadContractualRegistroPresupuestalId == NovedadContractualRegistroPresupuestalId)
                                                                                                        .Include(x => x.NovedadContractual)
                                                                                                            .ThenInclude(x => x.NovedadContractualAportante)
                                                                                                                .ThenInclude(x => x.ComponenteAportanteNovedad)
                                                                                                                    .ThenInclude(x => x.ComponenteFuenteNovedad)
                                                                                                        .FirstOrDefault();
            List<FuenteFinanciacion> listaFuentes = new List<FuenteFinanciacion>();

            //var financiaciones = _context.FuenteFinanciacion.Where(x => gestion.Contains(x.FuenteFinanciacionId) && x.Eliminado == false).ToList();

            novedadContractualRegistroPresupuestal.NovedadContractual.NovedadContractualAportante = novedadContractualRegistroPresupuestal.NovedadContractual.NovedadContractualAportante.Where(x => x.Eliminado != true).ToList();
            novedadContractualRegistroPresupuestal.NovedadContractual.NovedadContractualAportante.ToList().ForEach(aportante =>
           {
               if (aportante.CofinanciacionAportanteId == aportanteID)
               {
                   aportante.ComponenteAportanteNovedad = aportante.ComponenteAportanteNovedad.Where(x => x.Eliminado != true).ToList();
                   aportante.ComponenteAportanteNovedad.ToList().ForEach(componente =>
                  {
                      componente.ComponenteFuenteNovedad = componente.ComponenteFuenteNovedad.Where(x => x.Eliminado != true).ToList();
                      componente.ComponenteFuenteNovedad.ToList().ForEach(fuente =>
                     {
                         //listaFuentes.Add(_context.FuenteFinanciacion.Find(fuente.FuenteFinanciacionId));
                         listaIds.Add(fuente.FuenteFinanciacionId);
                     });
                  });
               }
           });

            var financiaciones = _context.FuenteFinanciacion
                                               .Where(x => listaIds.Contains(x.FuenteFinanciacionId) &&
                                                      x.Eliminado == false)
                                               .ToList();

            foreach (var financiacion in financiaciones)
            {
                var valorDisponible = (decimal)financiacion.ValorFuente - _context.GestionFuenteFinanciacion
                                                                                        .Where(x => !(bool)x.Eliminado &&
                                                                                               x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId &&
                                                                                               x.NovedadContractualRegistroPresupuestalId != NovedadContractualRegistroPresupuestalId)
                                                                                        .Sum(x => x.ValorSolicitado);

                var valorsolicitado = _context.GestionFuenteFinanciacion
                                                    .Where(x => !(bool)x.Eliminado &&
                                                           x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId &&
                                                           x.NovedadContractualRegistroPresupuestalId == NovedadContractualRegistroPresupuestalId)
                                                    .Sum(x => x.ValorSolicitado);

                ListaRetorno.Add(new GrillaFuentesFinanciacion
                {
                    FuenteFinanciacionID = financiacion.FuenteFinanciacionId,
                    Fuente = _context.Dominio.Where(x => x.Codigo == financiacion.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre,
                    Nuevo_saldo_de_la_fuente = valorDisponible - valorsolicitado,
                    Saldo_actual_de_la_fuente = valorDisponible,
                    Valor_solicitado_de_la_fuente = valorsolicitado,
                    GestionFuenteFinanciacionID = _context.GestionFuenteFinanciacion
                                                                .Where(x => !(bool)x.Eliminado &&
                                                                       x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId &&
                                                                       x.NovedadContractualRegistroPresupuestalId == NovedadContractualRegistroPresupuestalId)
                                                                .Select(x => x.GestionFuenteFinanciacionId)
                                                                .FirstOrDefault(),
                });
            }
            return ListaRetorno;
        }

        public async Task GetConsignationValue(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            var fuentes = _context.FuenteFinanciacion.Where(x => (bool)x.RegistroCompleto).Include(x => x.ControlRecurso).Include(x => x.Aportante).ThenInclude(x => x.Cofinanciacion).ToList();
            foreach (var fuente in fuentes)
            {
                decimal valoracompara = 0;
                foreach (var control in fuente.ControlRecurso)
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
                   Include(x => x.Proyecto).
                    ThenInclude(x => x.ProyectoAportante).
                        ThenInclude(x => x.Aportante).
                            ThenInclude(x => x.FuenteFinanciacion).ToList();
                if (_context.DisponibilidadPresupuestal.Where(x => x.DisponibilidadPresupuestalId == disponibilidadPresupuestaId).
                    FirstOrDefault().TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)//entonces es una disponivlidad espacial
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
                else
                {
                    foreach (var dDisponibilidadProyecto in disponibilidad)
                    {
                        if (dDisponibilidadProyecto.ProyectoAdministrativoId != null)
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
            }
            var financiaciones = _context.FuenteFinanciacion.Where(x => gestion.Contains(x.FuenteFinanciacionId) && x.Eliminado == false).ToList();

            foreach (var financiacion in financiaciones)
            {
                var gestionfienteid = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == disponibilidadPresupuestaId).Select(x => x.GestionFuenteFinanciacionId);
                int gestionid = 0;
                if (gestionfienteid != null)
                {
                    gestionid = gestionfienteid.FirstOrDefault();
                }
                var gestionAlGuardar = _context.GestionFuenteFinanciacion
                    .Where(x => x.DisponibilidadPresupuestalProyectoId == disponibilidadPresupuestaId &&
                        x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.Eliminado != true)
                    .FirstOrDefault();
                decimal valor = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == disponibilidadPresupuestaId).Sum(x => x.ValorSolicitado);
                decimal valorSolicitado = 0;
                if (valor == 0)//si es disponibilidad especial entonces la relacion es diferente
                {
                    var gestionfiente = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalId == disponibilidadPresupuestaId).Select(x => x.GestionFuenteFinanciacionId);
                    if (gestionfiente != null)
                    {
                        gestionid = gestionfiente.FirstOrDefault();
                    }

                    List<GestionFuenteFinanciacion> gestionFuenteFinanciacions = _context.GestionFuenteFinanciacion
                                                                                             .Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId)
                                                                                             .Include(x => x.DisponibilidadPresupuestalProyecto)
                                                                                             .ToList();

                    gestionFuenteFinanciacions.ForEach(g =>
                    {
                        if (g.DisponibilidadPresupuestalId != disponibilidadPresupuestaId)
                            valor = valor + g.ValorSolicitado;
                        if (g.DisponibilidadPresupuestalId == disponibilidadPresupuestaId)
                            valorSolicitado = valorSolicitado + g.ValorSolicitado;
                    });

                    //valor = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalId == disponibilidadPresupuestaId).Sum(x => x.ValorSolicitado);
                    //valor = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId != disponibilidadPresupuestaId).Sum(x => x.ValorSolicitado);
                    //var e = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId != disponibilidadPresupuestaId).ToList();
                }
                //var saldoFuente = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId != disponibilidadPresupuestaId).Sum(x => x.SaldoActual);
                var saldoFuente = financiacion.ValorFuente;
                //var saldoFuente = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId != disponibilidadPresupuestaId).Sum(x => x.ValorSolicitado);

                //si no he gestionado fuentes, soy especial y mi saldo es el total de la fuente
                if (saldoFuente == 0)
                {
                    saldoFuente = Convert.ToDecimal(financiacion.ValorFuente);
                }
                ListaRetorno.Add(new GrillaFuentesFinanciacion
                {
                    GestionFuenteFinanciacionID = gestionid,
                    FuenteFinanciacionID = financiacion.FuenteFinanciacionId,
                    Fuente = _context.Dominio.Where(x => x.Codigo == financiacion.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre,
                    ValorFuente = financiacion.ValorFuente != null ? financiacion.ValorFuente : 0,
                    Nuevo_saldo_de_la_fuente = saldoFuente.Value - valor - valorSolicitado,
                    Saldo_actual_de_la_fuente = saldoFuente.Value - valor,
                    Valor_solicitado_de_la_fuente = valorSolicitado,
                    Nuevo_saldo_de_la_fuente_al_guardar = gestionAlGuardar != null ? gestionAlGuardar.NuevoSaldo  : 0,
                    Saldo_actual_de_la_fuente_al_guardar = gestionAlGuardar != null ? gestionAlGuardar.SaldoActual : 0,
                });
            }
            return ListaRetorno;
        }

        public List<GrillaFuentesFinanciacion> GetListFuentesFinanciacionByDisponibilidadPresupuestald(int disponibilidadPresupuestaId, bool esNovedad, int novedadContractualRegistroPresupuestalId)
        {
            List<GrillaFuentesFinanciacion> ListaRetorno = new List<GrillaFuentesFinanciacion>();
            var gestion = _context.GestionFuenteFinanciacion.Where(x => x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == disponibilidadPresupuestaId && x.EsNovedad == esNovedad && x.NovedadContractualRegistroPresupuestalId == novedadContractualRegistroPresupuestalId).Select(x => x.FuenteFinanciacionId).ToList();
            if (gestion.Count() == 0)
            {
                var disponibilidad = _context.DisponibilidadPresupuestalProyecto.Where(x => x.DisponibilidadPresupuestalId == disponibilidadPresupuestaId).
                    Include(x => x.ProyectoAdministrativo).
                        ThenInclude(x => x.ProyectoAdministrativoAportante).
                            ThenInclude(x => x.AportanteFuenteFinanciacion).
                   Include(x => x.Proyecto).
                    ThenInclude(x => x.ProyectoAportante).
                        ThenInclude(x => x.Aportante).
                            ThenInclude(x => x.FuenteFinanciacion).ToList();
                if (_context.DisponibilidadPresupuestal.Where(x => x.DisponibilidadPresupuestalId == disponibilidadPresupuestaId).
                    FirstOrDefault().TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)//entonces es una disponivlidad espacial
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
                else
                {
                    foreach (var dDisponibilidadProyecto in disponibilidad)
                    {
                        if (dDisponibilidadProyecto.ProyectoAdministrativoId != null)
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
            }
            var financiaciones = _context.FuenteFinanciacion.Where(x => gestion.Contains(x.FuenteFinanciacionId) && x.Eliminado == false).ToList();

            foreach (var financiacion in financiaciones)
            {
                var gestionfienteid = _context.GestionFuenteFinanciacion.Where(x => x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == disponibilidadPresupuestaId && x.EsNovedad == esNovedad && x.NovedadContractualRegistroPresupuestalId == novedadContractualRegistroPresupuestalId).Select(x => x.GestionFuenteFinanciacionId);
                int gestionid = 0;
                if (gestionfienteid != null)
                {
                    gestionid = gestionfienteid.FirstOrDefault();
                }
                var gestionAlGuardar = _context.GestionFuenteFinanciacion
                    .Where(x => x.DisponibilidadPresupuestalProyectoId == disponibilidadPresupuestaId &&
                        x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId &&
                        x.EsNovedad == esNovedad &&
                        x.NovedadContractualRegistroPresupuestalId == novedadContractualRegistroPresupuestalId &&
                        x.Eliminado != true)
                    .FirstOrDefault();
                decimal valor = _context.GestionFuenteFinanciacion.Where(x => x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == disponibilidadPresupuestaId && x.EsNovedad == esNovedad && x.NovedadContractualRegistroPresupuestalId == novedadContractualRegistroPresupuestalId).Sum(x => x.ValorSolicitado);
                decimal valorSolicitado = 0;
                if (valor == 0)//si es disponibilidad especial entonces la relacion es diferente
                {
                    var gestionfiente = _context.GestionFuenteFinanciacion.Where(x => x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalId == disponibilidadPresupuestaId && x.EsNovedad == esNovedad && x.NovedadContractualRegistroPresupuestalId == novedadContractualRegistroPresupuestalId).Select(x => x.GestionFuenteFinanciacionId);
                    if (gestionfiente != null)
                    {
                        gestionid = gestionfiente.FirstOrDefault();
                    }

                    List<GestionFuenteFinanciacion> gestionFuenteFinanciacions = _context.GestionFuenteFinanciacion
                                                                                             .Where(x => x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.EsNovedad == esNovedad && x.NovedadContractualRegistroPresupuestalId == novedadContractualRegistroPresupuestalId)
                                                                                             .Include(x => x.DisponibilidadPresupuestalProyecto)
                                                                                             .ToList();

                    gestionFuenteFinanciacions.ForEach(g =>
                    {
                        if (g.DisponibilidadPresupuestalId != disponibilidadPresupuestaId)
                            valor = valor + g.ValorSolicitado;
                        if (g.DisponibilidadPresupuestalId == disponibilidadPresupuestaId)
                            valorSolicitado = valorSolicitado + g.ValorSolicitado;
                    });

                    //valor = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalId == disponibilidadPresupuestaId).Sum(x => x.ValorSolicitado);
                    //valor = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId != disponibilidadPresupuestaId).Sum(x => x.ValorSolicitado);
                    //var e = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId != disponibilidadPresupuestaId).ToList();
                }
                //var saldoFuente = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId != disponibilidadPresupuestaId).Sum(x => x.SaldoActual);
                var saldoFuente = financiacion.ValorFuente;
                //var saldoFuente = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == financiacion.FuenteFinanciacionId && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId != disponibilidadPresupuestaId).Sum(x => x.ValorSolicitado);

                //si no he gestionado fuentes, soy especial y mi saldo es el total de la fuente
                if (saldoFuente == 0)
                {
                    saldoFuente = Convert.ToDecimal(financiacion.ValorFuente);
                }
                ListaRetorno.Add(new GrillaFuentesFinanciacion
                {
                    GestionFuenteFinanciacionID = gestionid,
                    FuenteFinanciacionID = financiacion.FuenteFinanciacionId,
                    Fuente = _context.Dominio.Where(x => x.Codigo == financiacion.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre,
                    ValorFuente = financiacion.ValorFuente != null ? financiacion.ValorFuente : 0,
                    Nuevo_saldo_de_la_fuente = saldoFuente.Value - valor - valorSolicitado,
                    Saldo_actual_de_la_fuente = saldoFuente.Value - valor,
                    Valor_solicitado_de_la_fuente = valorSolicitado,
                    Nuevo_saldo_de_la_fuente_al_guardar = gestionAlGuardar != null ? gestionAlGuardar.NuevoSaldo : 0,
                    Saldo_actual_de_la_fuente_al_guardar = gestionAlGuardar != null ? gestionAlGuardar.SaldoActual : 0,
                });
            }
            return ListaRetorno;
        }

        public async Task<Respuesta> EliminarCuentaBancaria(int id, string UsuarioModifico)
        {
            int idAccionEliminarFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Fuentes_Financiacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                var entity = await _context.CuentaBancaria.Where(x => x.CuentaBancariaId == id).Include(x => x.ControlRecurso).FirstOrDefaultAsync();
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
    }
}
