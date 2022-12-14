using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IParametricService
    {
        Task<Respuesta> CreateDominio(TipoDominio pTipoDominio);
        Task<List<VParametricas>> GetParametricas();
        Task<List<VDominio>> GetDominioByTipoDominioId(int pTipoDominioId);
    }
}
