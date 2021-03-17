﻿using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IVerifyPreConstructionRequirementsPhase1Service
    {
        Task<List<VRegistrarFase1>> GetListContratacionInterventoria2(int pAuthorId);
             
        Task<dynamic> GetListContratacion(int pAuthorId);

        Task<dynamic> GetListContratacionInterventoria();

        Task<Contrato> GetContratoByContratoId(int pContratoId);

        Task<Respuesta> CrearContratoPerfilObservacion(ContratoPerfilObservacion pContratoPerfilObservacion);

        bool ValidarRegistroCompletoContratoPerfil(ContratoPerfil contratoPerfilOld);
    }
}
