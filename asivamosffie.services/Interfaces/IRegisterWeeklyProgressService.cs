using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IRegisterWeeklyProgressService
    {
        Task<List<VRegistrarAvanceSemanal>> GetVRegistrarAvanceSemanal();

        Task<SeguimientoSemanal> GetLastSeguimientoSemanalByContratacionProyectoIdOrSeguimientoSemanalId(int pContratacionProyectoId, int pSeguimientoSemanalId);

        Task<List<dynamic>> GetListSeguimientoSemanalByContratacionProyectoId(int pContratacionProyectoId);

        Task<Respuesta> SaveUpdateSeguimientoSemanal(SeguimientoSemanal pSeguimientoSemanal);

        Task<Respuesta> DeleteManejoMaterialesInsumosProveedor(int ManejoMaterialesInsumosProveedorId, string pUsuarioModificacion);
    }
}
