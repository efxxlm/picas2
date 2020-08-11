using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services
{
    public class SelectionProcessGroupService: ISelectionProcessGroupService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public SelectionProcessGroupService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<ActionResult<List<ProcesoSeleccionGrupo>>> GetSelectionProcessGroup()
        {
            return await _context.ProcesoSeleccionGrupo.ToListAsync();
        }

        public async Task<ProcesoSeleccionGrupo> GetSelectionProcessGroupById(int id)
        {
            return await _context.ProcesoSeleccionGrupo.FindAsync(id);
        }
          

        //Registrar procesos seleccion grupo
        public async Task<Respuesta> Insert(ProcesoSeleccionGrupo procesoSeleccionGrupo)
        {
            Respuesta _response = new Respuesta();
            try
            {
                if (procesoSeleccionGrupo != null)
                {
                    if ((int)procesoSeleccionGrupo.ValorMinimoCategoria > (int)procesoSeleccionGrupo.ValorMaximoCategoria) 
                    {
                        return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesSelectionProcessGroup.CamposIncompletos, Message = "Los valores de las categorías no son coherentes." };
                    }

                    _context.Add(procesoSeleccionGrupo);
                    await _context.SaveChangesAsync();
                    return _response = new Respuesta {  IsSuccessful = true, IsValidation = false, Data = procesoSeleccionGrupo, Code = ConstantMessagesSelectionProcessGroup.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null,Code = ConstantMessagesSelectionProcessGroup.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta
                {
                    IsSuccessful = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesSelectionProcessGroup.ErrorInterno,
                };
            }
        }

        public async Task<Respuesta> Update(ProcesoSeleccionGrupo procesoSeleccionGrupo)
        {
            Respuesta _response = new Respuesta();


            ProcesoSeleccionGrupo updateObj = await _context.ProcesoSeleccionGrupo.FindAsync(procesoSeleccionGrupo.ProcesoSeleccionGrupoId);
            updateObj.ProcesoSeleccionId = procesoSeleccionGrupo.ProcesoSeleccionId;
            updateObj.NombreGrupo = procesoSeleccionGrupo.NombreGrupo;
            updateObj.TipoPresupuestoCodigo = procesoSeleccionGrupo.TipoPresupuestoCodigo;
            updateObj.Valor = procesoSeleccionGrupo.Valor;
            updateObj.ValorMinimoCategoria = procesoSeleccionGrupo.ValorMinimoCategoria;
            updateObj.ValorMaximoCategoria = procesoSeleccionGrupo.ValorMaximoCategoria;
            updateObj.PlazoMeses = procesoSeleccionGrupo.PlazoMeses;
            updateObj.FechaModificacion = DateTime.Now;
            updateObj.UsuarioModificacion = procesoSeleccionGrupo.UsuarioModificacion; //HttpContext.User.FindFirst("User").Value


            try
            {
                if ((int)procesoSeleccionGrupo.ValorMinimoCategoria > (int)procesoSeleccionGrupo.ValorMaximoCategoria)
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesSelectionProcessGroup.CamposIncompletos, Message = "Los valores de las categorías no son coherentes." };
                }
                _context.Update(updateObj);
                await _context.SaveChangesAsync();
                return _response = new Respuesta
                {
                    IsSuccessful = true,
                    IsValidation = false,
                    Data = updateObj,
                    Code = ConstantMessagesSelectionProcessGroup.EditadoCorrrectamente,

                };
            }
            catch (Exception ex)
            {
                return _response = new Respuesta
                {
                    IsSuccessful = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesSelectionProcessGroup.ErrorInterno,

                };
            }
        }


        public async Task<bool> Delete(int id)
        {
            try
            {
                ProcesoSeleccionGrupo entity = await GetSelectionProcessGroupById(id);
                _context.ProcesoSeleccionGrupo.Remove(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
