using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;



        public BankAccountService(devAsiVamosFFIEContext context, ICommonService commonService)
        {

            _commonService = commonService;
            _context = context;
        }

        public async Task<ActionResult<List<CuentaBancaria>>> GetBankAccount()
        {
            return await _context.CuentaBancaria.ToListAsync();
        }

        public async Task<CuentaBancaria> GetBankAccountById(int id)
        {
            return await _context.CuentaBancaria.FindAsync(id);
        }

        public async Task<Respuesta> Insert(CuentaBancaria cuentaBancaria)
        {
            Respuesta _response = new Respuesta();
            try
            {
                if (cuentaBancaria != null)
                {
                    cuentaBancaria.FechaCreacion = DateTime.Now;
                    _context.Add(cuentaBancaria);
                    await _context.SaveChangesAsync();

                    return _response = new Respuesta { IsSuccessful = true, IsValidation = false, Data = cuentaBancaria, Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.ErrorInterno };
            }

        }

        public async Task<Respuesta> Update(CuentaBancaria cuentaBancaria)
        {
            try
            {
                CuentaBancaria updateObj = await _context.CuentaBancaria.FindAsync(cuentaBancaria.CuentaBancariaId);

                updateObj.FuenteFinanciacionId = cuentaBancaria.FuenteFinanciacionId;
                updateObj.NumeroCuentaBanco = cuentaBancaria.NumeroCuentaBanco;
                updateObj.NombreCuentaBanco = cuentaBancaria.NombreCuentaBanco;
                updateObj.CodigoSifi = cuentaBancaria.CodigoSifi;
                updateObj.TipoCuentaCodigo = cuentaBancaria.TipoCuentaCodigo;
                updateObj.BancoCodigo = cuentaBancaria.BancoCodigo;
                updateObj.Exenta = cuentaBancaria.Exenta;

                _context.Update(updateObj);
                await _context.SaveChangesAsync();

                return new Respuesta { IsSuccessful = true, IsValidation = false, Data = updateObj, Code = ConstantMessagesBankAccount.EditadoCorrrectamente };
            }
            catch (Exception ex)
            {
                return new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesBankAccount.Error, Message = ex.Message };
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                CuentaBancaria entity = await GetBankAccountById(id);
                _context.CuentaBancaria.Remove(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Respuesta> CreateEditarCuentasBancarias(CuentaBancaria cuentaBancaria)
        { 
            int idAccionCrearCuentaBancaria = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Cuenta_Bancaria, (int)EnumeratorTipoDominio.Acciones);
            try
            { 
                string strCrearEditar;
                if (cuentaBancaria.CuentaBancariaId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR CUENTA BANCARIA";
                    cuentaBancaria.FechaCreacion = DateTime.Now;
                    cuentaBancaria.Eliminado = false;
                    //Registros
                    cuentaBancaria.NombreCuentaBanco = cuentaBancaria.NombreCuentaBanco.ToUpper();
                    cuentaBancaria.CodigoSifi = cuentaBancaria.CodigoSifi.ToUpper();
                    _context.CuentaBancaria.Add(cuentaBancaria);
                }
                else
                {
                    strCrearEditar = "EDITAR CUENTA BANCARIA";
                    CuentaBancaria cuentaBancariaAntigua = _context.CuentaBancaria.Find(cuentaBancaria.CuentaBancariaId);
                    //Auditoria
                    cuentaBancariaAntigua.UsuarioModificacion = cuentaBancaria.UsuarioCreacion;
                    cuentaBancariaAntigua.FechaModificacion = DateTime.Now;
                    //Registros
                    cuentaBancariaAntigua.NumeroCuentaBanco = cuentaBancaria.NumeroCuentaBanco;
                    cuentaBancariaAntigua.NombreCuentaBanco = cuentaBancaria.NombreCuentaBanco.ToUpper();
                    cuentaBancariaAntigua.CodigoSifi = cuentaBancaria.CodigoSifi.ToUpper();
                    cuentaBancariaAntigua.TipoCuentaCodigo = cuentaBancaria.TipoCuentaCodigo;
                    cuentaBancariaAntigua.BancoCodigo = cuentaBancaria.BancoCodigo;
                    cuentaBancariaAntigua.Exenta = cuentaBancaria.Exenta;
                    cuentaBancariaAntigua.FuenteFinanciacionId = cuentaBancaria.FuenteFinanciacionId;
                     
                }
               await _context.SaveChangesAsync();

                return  
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
                return  
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
    }
}