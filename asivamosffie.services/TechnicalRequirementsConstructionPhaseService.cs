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
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Globalization;

namespace asivamosffie.services
{
    public class TechnicalRequirementsConstructionPhaseService : ITechnicalRequirementsConstructionPhaseService
    {
        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        public readonly IConverter _converter;

        public TechnicalRequirementsConstructionPhaseService(IConverter converter, devAsiVamosFFIEContext context, ICommonService commonService, IDocumentService documentService)
        {
            _converter = converter;
            _context = context;
            _documentService = documentService;
            _commonService = commonService;
        }

        public async Task<List<dynamic>> GetContractsGrid()
        {
            List<dynamic> listaContrats = new List<dynamic>();

            List<VRequisitosTecnicosInicioConstruccion> lista = _context.VRequisitosTecnicosInicioConstruccion.ToList();

            lista.ForEach( c => {
                listaContrats.Add(new {
                    ContratoId = c.ContratoId,
                    FechaAprobacion = c.FechaAprobacion,
                    NumeroContrato = c.NumeroContrato,
                    CantidadProyectosAsociados = c.CantidadProyectosAsociados,
                    CantidadProyectosRequisitosAprobados = c.CantidadProyectosRequisitosAprobados,   
                    CantidadProyectosRequisitosPendientes = c.CantidadProyectosAsociados - c.CantidadProyectosRequisitosAprobados,
                    EstadoCodigo = c.EstadoCodigo,
                    EstadoNombre = c.EstadoNombre,
                    Existeregistro = c.ExisteRegistro,

                });
            });

            return listaContrats;

        }

        public async Task<ContratoConstruccion> GetContratoConstruccionById( int pIdContratoConstruccion )
        {
            ContratoConstruccion contratoConstruccion = new ContratoConstruccion();

            contratoConstruccion = _context.ContratoConstruccion.Find( pIdContratoConstruccion );
            contratoConstruccion.EsCompletoDiagnostico = this.EsCompletoDiagnostico( contratoConstruccion );

            return contratoConstruccion;

        }

        private bool EsCompletoDiagnostico(ContratoConstruccion pContratoConstruccion){
            bool esCompleto = true;

            if (    pContratoConstruccion.EsInformeDiagnostico == null ||
                    ( pContratoConstruccion.EsInformeDiagnostico == true && string.IsNullOrEmpty( pContratoConstruccion.RutaInforme ) ) ||
                    pContratoConstruccion.CostoDirecto == null ||
                    pContratoConstruccion.Administracion == null ||
                    pContratoConstruccion.Imprevistos == null ||
                    pContratoConstruccion.Utilidad == null ||
                    pContratoConstruccion.ValorTotalFaseConstruccion == null ||
                    pContratoConstruccion.RequiereModificacionContractual == null ||
                    ( pContratoConstruccion.RequiereModificacionContractual == true && string.IsNullOrEmpty( pContratoConstruccion.NumeroSolicitudModificacion ) )
               )
            {
                esCompleto = false;
            }

            return esCompleto;
        }
         
    }

}
