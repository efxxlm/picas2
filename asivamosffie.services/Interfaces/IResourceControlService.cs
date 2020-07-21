using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IResourceControlService
    {
        Task<List<ControlRecurso>> GetResourceControl();
        Task<ControlRecurso> GetResourceControlById(int id);

        Task<List<ControlRecurso>> GetResourceControlGridBySourceFunding( int id );
        Task<Respuesta> Insert(ControlRecurso controlRecurso);

        Task<Respuesta> Update(ControlRecurso controlRecurso);

        Task<bool> Delete(int id);

    }
}
