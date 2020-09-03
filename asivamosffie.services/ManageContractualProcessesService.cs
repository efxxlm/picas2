using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.api.Controllers;
using AuthorizationTest.JwtHelpers;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using Newtonsoft.Json;

namespace asivamosffie.services
{
    public class ManageContractualProcessesService : IManageContractualProcessesService
    {
        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;

        public ManageContractualProcessesService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<List<SesionComiteSolicitud>> GetListSesionComiteSolicitud()
        {
            // Estado de la sesionComiteSolicitud
            //• Recibidas sin tramitar ante Fiduciaria
            //• Enviadas a la fiduciaria
            //• Registradas por la fiduciaria

            List<SesionComiteSolicitud> ListSesionComiteSolicitud = await _context.SesionComiteSolicitud
                    .Where(r => (r.TipoSolicitud == ConstanCodigoTipoSolicitud.Contratacion)
                    ).ToListAsync();


            List<Dominio> ListasParametricas = _context.Dominio.ToList();

            List<Contratacion> ListContratacion = _context.Contratacion.Where(r => !(bool)r.Eliminado).ToList();

            foreach (var sesionComiteSolicitud in ListSesionComiteSolicitud)
            {
                sesionComiteSolicitud.EstadoCodigo = ListasParametricas
                    .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Comite
                    && r.Codigo == sesionComiteSolicitud.EstadoCodigo
                    ).FirstOrDefault().Nombre;

                switch (sesionComiteSolicitud.TipoSolicitudCodigo)
                {
                    case ConstanCodigoTipoSolicitud.Contratacion:
                        sesionComiteSolicitud.FechaSolicitud = (DateTime)ListContratacion
                      .Where(r => r.ContratacionId == sesionComiteSolicitud.SolicitudId)
                      .FirstOrDefault()
                      .FechaCreacion;

                        sesionComiteSolicitud.Contratacion = ListContratacion
                                .Where(r => r.ContratacionId == sesionComiteSolicitud.SolicitudId)
                                .FirstOrDefault();

                        sesionComiteSolicitud.FechaSolicitud = sesionComiteSolicitud.Contratacion.FechaTramite;

                        sesionComiteSolicitud.TipoSolicitud = ListasParametricas
                            .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud
                            && r.Codigo == ConstanCodigoTipoSolicitud.Contratacion
                            ).FirstOrDefault().Nombre;

                        sesionComiteSolicitud.NumeroSolicitud = sesionComiteSolicitud.Contratacion.NumeroSolicitud;

                        _ = (bool)sesionComiteSolicitud.Contratacion.RegistroCompleto
                            ? sesionComiteSolicitud.EstadoDelRegistro == "Completo"
                            : sesionComiteSolicitud.EstadoDelRegistro == "Incompleto";


                        break;
                }
            }
            return ListSesionComiteSolicitud;
        }


    }
}
