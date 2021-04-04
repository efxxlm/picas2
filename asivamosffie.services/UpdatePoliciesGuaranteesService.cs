using asivamosffie.model.APIModels;
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
    public class UpdatePoliciesGuaranteesService : IUpdatePoliciesGuaranteesService
    { 
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IUpdatePoliciesGuaranteesService _updatePoliciesGuaranteesService;

        public UpdatePoliciesGuaranteesService(IUpdatePoliciesGuaranteesService updatePoliciesGuaranteesService, devAsiVamosFFIEContext context, ICommonService commonService )
        {
            _commonService = commonService;
            _context = context;
            _updatePoliciesGuaranteesService = updatePoliciesGuaranteesService;
        }
   

        public async Task<dynamic> GetContratoByNumeroContrato(string pNumeroContrato)
        {
            try
            { 
                List<dynamic> List = new List<dynamic>();
                List<Contrato> ListContratos = new List<Contrato>();

                ListContratos = await _context.Contrato
                                .Include(c => c.Contratacion)
                                .ThenInclude(c => c.Contratista)
                                         .Where(c => c.NumeroContrato.Trim().ToLower().Contains(pNumeroContrato.Trim().ToLower())
                                                     && (
                                                         (c.EstadoActaFase2.Trim() == ConstanCodigoEstadoActaInicioObra.Con_acta_suscrita_y_cargada
                                                         && c.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Obra) ||
                                                        (c.EstadoActaFase2.Trim() == ConstanCodigoEstadoActaInicioInterventoria.Con_acta_suscrita_y_cargada
                                                         && c.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria)
                                                        )
                                                  ).ToListAsync();

                foreach (var Contrato in ListContratos)
                {
                    List.Add(new
                    {
                        Contrato.Contratacion.Contratista.Nombre,
                        Contrato.Contratacion.TipoSolicitudCodigo,
                        Contrato.ContratoId,
                        Contrato.NumeroContrato,
                    });
                }


                return List;
            }
            catch (Exception ex)
            {
                return new { };
            }
        }


    }
}
