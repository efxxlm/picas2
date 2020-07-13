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
    public class BankAccountService: IBankAccountService
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
            Respuesta _reponse = new Respuesta();
           // int IdAccionCrearCuentaBancaria = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Acciones && x.Codigo.Equals(ConstantCodigoAcciones.CrearCuentaBancaria)).Select(x => x.DominioId).First();
            try
            {
                if (cuentaBancaria != null)
                {
                    _context.Add(cuentaBancaria);
                    await _context.SaveChangesAsync();

                    return _reponse = new Respuesta
                    {
                        IsSuccessful = true, IsValidation = false,
                        Data = cuentaBancaria,  Code = ConstantMessagesContributor.OperacionExitosa,
                  //      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.OperacionExitosa, IdAccionCrearCuentaBancaria, cuentaBancaria.UsuarioCreacion.ToString(), ConstantMessagesContributor.OperacionExitosa)
                    };
                }
                else
                {
                    return _reponse = new Respuesta
                    {
                        IsSuccessful = false,  IsValidation = false,
                        Data = null, Code = ConstantMessagesContributor.RecursoNoEncontrado,
//Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.RecursoNoEncontrado, IdAccionCrearCuentaBancaria, cuentaBancaria.UsuarioCreacion.ToString(), ConstantMessagesContributor.RecursoNoEncontrado)
                    };
                }

            }
            catch (Exception ex)
            {
                return _reponse = new Respuesta
                {
                    IsSuccessful = false, IsValidation = false,
                    Data = null, Code = ConstantMessagesContributor.ErrorInterno,
               //     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.ErrorInterno, IdAccionCrearCuentaBancaria, cuentaBancaria.UsuarioCreacion.ToString(), ex.InnerException.ToString()),

                };
            }

        }

        public Task<bool> Update(CuentaBancaria cuentaBancaria)
        {
            throw new NotImplementedException();
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

    }
}