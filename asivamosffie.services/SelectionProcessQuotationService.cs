using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace asivamosffie.services
{
    public class SelectionProcessQuotationService: ISelectionProcessQuotationService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public SelectionProcessQuotationService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<ActionResult<List<ProcesoSeleccionCotizacion>>> GetSelectionProcessQuotation()
        {
            return await _context.ProcesoSeleccionCotizacion.ToListAsync();
        }

        public async Task<ProcesoSeleccionCotizacion> GetSelectionProcessQuotationById(int id)
        {
            return await _context.ProcesoSeleccionCotizacion.FindAsync(id);
        }


        //Registrar procesos seleccion Cotizacion
        public async Task<Respuesta> Insert(ProcesoSeleccionCotizacion procesoSeleccionCotizacion)
        {
            Respuesta _response = new Respuesta();
            try
            {
                if (procesoSeleccionCotizacion != null)
                {
                    _context.ProcesoSeleccionCotizacion.Add(procesoSeleccionCotizacion);
                    await _context.SaveChangesAsync();

                    return _response = new Respuesta
                    {
                        IsSuccessful = true,
                        IsValidation = false,
                        Data = procesoSeleccionCotizacion,
                        Code = ConstantMessagesProcessSchedule.OperacionExitosa,
                    };
                }
                else
                {
                    return _response = new Respuesta
                    {
                        IsSuccessful = false,
                        IsValidation = false,
                        Data = null,
                        Code = ConstantMessagesProcessQuotation.RecursoNoEncontrado,
                    };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta
                {
                    IsSuccessful = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcessQuotation.ErrorInterno,
                };
            }
        }


        public async Task<Respuesta> Update(ProcesoSeleccionCotizacion procesoSeleccionCotizacion)
        {
            Respuesta _response = new Respuesta();


            ProcesoSeleccionCotizacion updateObj = await _context.ProcesoSeleccionCotizacion.FindAsync(procesoSeleccionCotizacion.ProcesoSeleccionCotizacionId);
            Console.WriteLine(updateObj);
            updateObj.ProcesoSeleccionId = procesoSeleccionCotizacion.ProcesoSeleccionId;
            updateObj.NombreOrganizacion = procesoSeleccionCotizacion.NombreOrganizacion;
            updateObj.ValorCotizacion =  Convert.ToDecimal(procesoSeleccionCotizacion.ValorCotizacion);
            updateObj.Descripcion = procesoSeleccionCotizacion.Descripcion;
            updateObj.UrlSoporte = procesoSeleccionCotizacion.UrlSoporte;
            updateObj.UrlSoporte = procesoSeleccionCotizacion.UrlSoporte;
            updateObj.FechaModificacion = DateTime.Now;
            updateObj.UsuarioModificacion = procesoSeleccionCotizacion.UsuarioModificacion; //HttpContext.User.FindFirst("User").Value


            try
            {
                _context.Update(updateObj);
                await _context.SaveChangesAsync();
                return _response = new Respuesta
                {
                    IsSuccessful = true,
                    IsValidation = false,
                    Data = updateObj,
                    Code = ConstantMessagesProcessQuotation.EditadoCorrrectamente,
                    // Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.ErrorInterno, IdAccionCrearCuentaBancaria, cuentaBancaria.UsuarioCreacion.ToString(), ex.InnerException.ToString()),

                };
            }
            catch (Exception ex)
            {
                return _response = new Respuesta
                {
                    IsSuccessful = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcessQuotation.ErrorInterno,
                    // Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.ErrorInterno, IdAccionCrearCuentaBancaria, cuentaBancaria.UsuarioCreacion.ToString(), ex.InnerException.ToString()),

                };
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                ProcesoSeleccionCotizacion entity = await GetSelectionProcessQuotationById(id);
                _context.ProcesoSeleccionCotizacion.Remove(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
