﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IPaymentRequierementsService
    {
        Task<Respuesta> CreateUpdateSolicitudPagoObservacion(SolicitudPagoObservacion pSolicitudPagoObservacion);

        Task<dynamic> GetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(int pMenuId, int pSolicitudPagoId, int pPadreId);

        Task<dynamic> GetListSolicitudPago(int pMenuId);

        Task<Respuesta> ChangueStatusSolicitudPago(SolicitudPago pSolicitudPago);
    }
}