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
            return await _context.FuenteFinanciacion.FindAsync(id);
            //return await _context.FuenteFinanciacion.Where(r => r.AportanteId == id).ToListAsync();
        }

        public async Task<Respuesta> CreateFuentesFinanciacion(FuenteFinanciacion fuentefinanciacion)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearFuentesFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Fuentes_Financiacion, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                fuentefinanciacion.FechaCreacion = DateTime.Now;
                fuentefinanciacion.Eliminado = false;
                _context.Add(fuentefinanciacion);
                await _context.SaveChangesAsync();

                //No se puede saber que datos estan incompletos si solo se valida el objeto y no campo a campo  
                //else

                //{
                //    respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesContributor.CamposIncoletos };
                //}
                return respuesta =
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
                return respuesta =
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
            Respuesta respuesta = new Respuesta();

            int idAccionEliminarFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Fuentes_Financiacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                var entity = await _context.FuenteFinanciacion.FindAsync(id);
                entity.FechaModificacion = DateTime.Now;
                entity.UsuarioModificacion = UsuarioModifico;
                entity.Eliminado = true;

                _context.Update(entity);
                await _context.SaveChangesAsync();

                return respuesta =
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
                return respuesta =
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
            Respuesta respuesta = new Respuesta();
            int idAccionCrearFuentesFinanciacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Fuentes_Financiacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                FuenteFinanciacion updateObj = await _context.FuenteFinanciacion.FindAsync(fuentefinanciacion.FuenteFinanciacionId);

                updateObj.AportanteId = fuentefinanciacion.AportanteId;
                updateObj.FuenteRecursosCodigo = fuentefinanciacion.FuenteRecursosCodigo;
                updateObj.ValorFuente = fuentefinanciacion.ValorFuente;

                _context.Update(updateObj);
                await _context.SaveChangesAsync();

                return respuesta =
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
                return respuesta =
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
            return await _context.FuenteFinanciacion.Where(r=> !(bool)r.Eliminado)
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
            return await _context.FuenteFinanciacion.Where(r => !(bool)r.Eliminado).Include(r => r.ControlRecurso).Include(r => r.CuentaBancaria).Include(r => r.VigenciaAporte).Include(r => r.Aportante).ThenInclude(r => r.RegistroPresupuestal).ToListAsync();
        }

        public async Task<Respuesta> CreateEditarCuentasBancarias(CuentaBancaria cuentaBancaria)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearCuentaBancaria = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Cuenta_Bancaria, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = "";

            try
            {

                if (cuentaBancaria.CuentaBancariaId != null || cuentaBancaria.CuentaBancariaId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR CUENTA BANCARIA";
                    cuentaBancaria.FechaCreacion = DateTime.Now;
                    cuentaBancaria.Eliminado = false;

                    _context.CuentaBancaria.Add(cuentaBancaria);
                }
                else
                {
                    strCrearEditar = "EDIT CUENTA BANCARIA";
                    CuentaBancaria cuentaBancariaAntigua = _context.CuentaBancaria.Find(cuentaBancaria.CuentaBancariaId);
                    //Auditoria
                    cuentaBancariaAntigua.UsuarioModificacion = cuentaBancaria.UsuarioCreacion;
                    cuentaBancariaAntigua.FechaModificacion = DateTime.Now;
                    //Registros
                    cuentaBancariaAntigua.FuenteFinanciacion = cuentaBancaria.FuenteFinanciacion;
                    cuentaBancariaAntigua.NumeroCuentaBanco = cuentaBancaria.NumeroCuentaBanco;
                    cuentaBancariaAntigua.NombreCuentaBanco = cuentaBancaria.NombreCuentaBanco;
                    cuentaBancariaAntigua.CodigoSifi = cuentaBancaria.CodigoSifi;
                    cuentaBancariaAntigua.TipoCuentaCodigo = cuentaBancaria.TipoCuentaCodigo;
                    cuentaBancariaAntigua.BancoCodigo = cuentaBancaria.BancoCodigo;
                    cuentaBancariaAntigua.Exenta = cuentaBancaria.Exenta;

                    _context.CuentaBancaria.Update(cuentaBancariaAntigua);
                }
                await _context.SaveChangesAsync();

                return respuesta =
               new Respuesta
               {
                   IsSuccessful = true,
                   IsException = false,
                   IsValidation = false,
                   Code = ConstantMessagesFuentesFinanciacion.OperacionExitosa,
                   Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.OperacionExitosa, idAccionCrearCuentaBancaria, cuentaBancaria.UsuarioCreacion, strCrearEditar)
               };
            }
            catch (Exception ex)
            {
                return respuesta =
                       new Respuesta
                       {
                           IsSuccessful = false,
                           IsException = true,
                           IsValidation = false,
                           Code = ConstantMessagesFuentesFinanciacion.Error,
                           Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.Error, idAccionCrearCuentaBancaria, cuentaBancaria.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                       };
            }
        }

        public async Task<Respuesta> CreateEditarVigenciaAporte(VigenciaAporte vigenciaAporte)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearVigenciaAporte = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Vigencia_Aporte, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = "";

            try
            {

                if (vigenciaAporte.VigenciaAporteId != null || vigenciaAporte.VigenciaAporteId == 0)
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
                    VigenciaAporteAntigua.FuenteFinanciacion = vigenciaAporte.FuenteFinanciacion;
                    VigenciaAporteAntigua.TipoVigenciaCodigo = vigenciaAporte.TipoVigenciaCodigo;
                    VigenciaAporteAntigua.ValorAporte = vigenciaAporte.ValorAporte;

                    _context.VigenciaAporte.Update(VigenciaAporteAntigua);
                }
                await _context.SaveChangesAsync();

                return respuesta =
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
                return respuesta =
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

    }
}
