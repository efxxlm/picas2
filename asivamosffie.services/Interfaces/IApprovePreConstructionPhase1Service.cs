﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
namespace asivamosffie.services.Interfaces
{
    public interface IApprovePreConstructionPhase1Service
    {
        Task<dynamic> GetListContratacion();

        Task<Respuesta> CreateEditContratoPerfil(ContratoPerfilObservacion pContratoPerfilObservacion);
    }
}
