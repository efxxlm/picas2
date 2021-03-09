﻿using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Constants;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class RegisterValidateSpinOrderService : IRegisterValidateSpinOrderService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;

        public RegisterValidateSpinOrderService(IDocumentService documentService, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
        }


        public async Task<Respuesta> CreateEditSpinOrderObservations(OrdenGiroObservacion pOrdenGiroObservacion)
        {
            if (pOrdenGiroObservacion.OrdenGiroObservacionId == 0)
            {
                pOrdenGiroObservacion.RegistroCompleto = ValidateCompleteRecordOrdenGiroObservacion(pOrdenGiroObservacion);
                await _context.OrdenGiroObservacion.AddAsync(pOrdenGiroObservacion);
            }
            else
            {
              await  _context.Set<OrdenGiroObservacion>()
                        .Where(og => og.OrdenGiroObservacionId == pOrdenGiroObservacion.OrdenGiroObservacionId)
                        .UpdateAsync(og => new OrdenGiroObservacion
                        {
                            Observacion = pOrdenGiroObservacion.Observacion,
                            TieneObservacion = pOrdenGiroObservacion.TieneObservacion,
                            TipoObservacionCodigo = pOrdenGiroObservacion.TipoObservacionCodigo,
                            FechaModificacion = DateTime.Now,
                            IdPadre = pOrdenGiroObservacion.IdPadre,
                            RegistroCompleto = ValidateCompleteRecordOrdenGiroObservacion(pOrdenGiroObservacion)
                        }); 
            } 
            return new Respuesta();
        }

        private bool ValidateCompleteRecordOrdenGiroObservacion(OrdenGiroObservacion pOrdenGiroObservacion)
        {
            if (pOrdenGiroObservacion.TieneObservacion == false)
                return false;
            else
                if (string.IsNullOrEmpty(pOrdenGiroObservacion.Observacion))
                    return false;
             
            return true;
        }
    }
}