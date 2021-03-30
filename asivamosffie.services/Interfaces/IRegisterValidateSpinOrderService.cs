﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IRegisterValidateSpinOrderService
    {
         Task<byte[]> GetListOrdenGiro(bool pBlRegistrosAprobados);

        Task<Respuesta> ChangueStatusOrdenGiro(OrdenGiro pOrdenGiro);

        Task<Respuesta> CreateEditSpinOrderObservations(OrdenGiroObservacion pOrdenGiroObservacion);
    }
}
