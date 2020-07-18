using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services
{
    public class ResourceControlService : IResourceControlService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public ResourceControlService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<List<ControlRecurso>> GetResourceControl()
        {
            return await _context.ControlRecurso.ToListAsync();
        }

        public async Task<ControlRecurso> GetResourceControlById(int id)
        {
            return await _context.ControlRecurso.FindAsync(id);
        }


        public async Task<List<ResourceControlGrid>> GetResourceControlBySourceId(int idFuente)
        {
            List<ResourceControlGrid> ListResourceControlGrid = new List<ResourceControlGrid>();

            try// List<Proyecto> ListProyectos = await _context.Proyecto.Where(r => !(bool)r.Eliminado).Include(r => r.InstitucionEducativa).Include(r => r.ProyectoPredio).Distinct().ToListAsync();
            {
                List<ControlRecurso> ListControlRecurso = await _context.ControlRecurso.Where(x => x.FuenteFinanciacionId == idFuente).ToListAsync();

                foreach (var Recurso in ListControlRecurso)
                {

                    Dominio ff = await _commonService.GetDominioByIdTipoDominio((int)EnumeratorTipoDominio.Fuentes_de_financiacion);
                    Dominio cb = await _commonService.GetDominioByIdTipoDominio((int)EnumeratorTipoDominio.Fuentes_de_financiacion);
                    // Dominio EstadoJuridicoPredios = await _commonService.GetDominioByNombreDominioAndTipoDominio(proyecto.ProyectoPredio.FirstOrDefault().EstadoJuridicoCodigo, (int)EnumeratorTipoDominio.Estado_Registro);

                    ResourceControlGrid grid = new ResourceControlGrid
                    {
                        ControlRecursoId = Recurso.ControlRecursoId,
                        FuenteFinanciacionId = Recurso.FuenteFinanciacionId,
                        FuenteFinanciacionText = ff.Nombre,
                        //CuentaBancariaText = ff.
                        //RegistroPresupuestalText
                        //VigenciaAporteText
                        //FechaConsignacion
                        //ValorConsignacion

                    };

                    ListResourceControlGrid.Add(grid);
                }

                return ListResourceControlGrid;


            }
            catch (Exception ex)
            {
                return ListResourceControlGrid;

            }
        }


        //public async Task<List<ControlRecurso>> GetResourceControlBySourceId(int idFuente)
        //{
        //    return await _context.ControlRecurso.Include(x => x.FuenteFinanciacion).Where(r => r.FuenteFinanciacionId == idFuente).ToListAsync();
        //}

        public async Task<List<ControlRecurso>> GetResourceControlGrid()
        {
            List<ControlRecurso> ControlGrid = new List<ControlRecurso>();
            try
            {
                ControlGrid = await _context.ControlRecurso
                    .Include(RC => RC.FuenteFinanciacion)
                    .Include(RC => RC.CuentaBancaria)
                    .Include(RC => RC.RegistroPresupuestal)
                    .Include(RC => RC.VigenciaAporte).ToListAsync();

                return ControlGrid;

            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public async Task<Respuesta> Insert(ControlRecurso controlRecurso)
        {
            Respuesta _response = new Respuesta();
            try
            {
                if (controlRecurso != null)
                {
                    _context.Add(controlRecurso);
                    await _context.SaveChangesAsync();

                    return _response = new Respuesta { IsSuccessful = true, IsValidation = false, Data = controlRecurso, Code = ConstantMessagesResourceControl.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesResourceControl.Error };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false,  Data = null,  Code = ConstantMessagesResourceControl.Error, Message = ex.Message };
            }
        }

        public async Task<Respuesta> Update(ControlRecurso controlRecurso)
        {
            Respuesta _response = new Respuesta();

            try
            {
                ControlRecurso updateObj = await _context.ControlRecurso.FindAsync(controlRecurso.ControlRecursoId);

                updateObj.FuenteFinanciacionId = controlRecurso.FuenteFinanciacionId;
                updateObj.CuentaBancariaId = controlRecurso.CuentaBancariaId;
                updateObj.RegistroPresupuestalId = controlRecurso.RegistroPresupuestalId;
                updateObj.VigenciaAporteId = controlRecurso.VigenciaAporteId;
                updateObj.FechaConsignacion = controlRecurso.FechaConsignacion;
                updateObj.ValorConsignacion = controlRecurso.ValorConsignacion;

                _context.Update(updateObj);
                await _context.SaveChangesAsync();

                return _response = new Respuesta  { IsSuccessful = true, IsValidation = false,  Data = updateObj, Code = ConstantMessagesResourceControl.EditadoCorrrectamente };
            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesResourceControl.Error, Message = ex.Message};
            }
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

    }
}
