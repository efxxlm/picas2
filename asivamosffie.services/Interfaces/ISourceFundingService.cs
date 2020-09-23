using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface ISourceFundingService
    {
        Task<List<FuenteFinanciacion>> GetISourceFunding();
        Task<FuenteFinanciacion> GetISourceFundingById(int id);

        Task<Respuesta> CreateEditFuentesFinanciacion(FuenteFinanciacion fuentefinanciacion);

        Task<Respuesta> EditFuentesFinanciacion(FuenteFinanciacion fuentefinanciacion);

        Task<Respuesta> EliminarFuentesFinanciacion(int id, string UsuarioModifico);

        Task<List<FuenteFinanciacion>> GetFuentesFinanciacionByAportanteId(int AportanteId);

        Task<List<FuenteFinanciacion>> GetListFuentesFinanciacion();

        Task<Respuesta> CreateEditarVigenciaAporte(VigenciaAporte vigenciaAporte);
        Task<List<GrillaFuentesFinanciacion>> GetListFuentesFinanciacionByAportanteId(int aportanteId);
        Task<List<GrillaFuentesFinanciacion>> GetListFuentesFinanciacionByDisponibilidadPresupuestalProyectoid(int disponibilidadPresupuestalProyectoid, int idaportante);
    }
}
