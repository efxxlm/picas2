﻿using asivamosffie.model.APIModels;
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

        Task<Respuesta> Insert(FuenteFinanciacion fuentefinanciacion);

        Task<Respuesta> Update(FuenteFinanciacion fuentefinanciacion);

        Task<Respuesta> Delete(int id , string UsuarioModifico);

        Task<List<FuenteFinanciacion>> GetFuentesFinanciacionByAportanteId(int AportanteId);

        Task<List<FuenteFinanciacion>> GetListFuentesFinanciacion();
    }
}
