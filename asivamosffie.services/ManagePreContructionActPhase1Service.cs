using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
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
namespace asivamosffie.services
{
    public class ManagePreContructionActPhase1Service : IManagePreContructionActPhase1Service
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public ManagePreContructionActPhase1Service(devAsiVamosFFIEContext context , ICommonService commonService) {
            _context = context;
            _commonService = commonService; 
        }
        public async Task<dynamic> GetListContrato() {
             
            try
            { 
                List<Contrato> listContratos = await _context.Contrato
                       .Where(r => r.EstadoVerificacionCodigo == ConstanCodigoEstadoVerificacionContrato.Con_requisitos_tecnicos_aprobados).ToListAsync();

                List<Dominio> listEstadosActa = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Del_Acta_Contrato).ToList();

                List<dynamic> ListContratacionDynamic = new List<dynamic>();


                listContratos.ForEach(contrato => {
                    string estadoActaContrato = "";
                    if (string.IsNullOrEmpty(contrato.EstadoActa))
                    {
                        estadoActaContrato = listEstadosActa.Where(r => r.Codigo == ConstanCodigoEstadoActa.Sin_acta_generada).FirstOrDefault().Nombre;
                    }
                    else 
                    { 
                        estadoActaContrato = listEstadosActa.Where(r => r.Codigo == contrato.EstadoActa).FirstOrDefault().Nombre;
                    }
                    ListContratacionDynamic.Add(new
                    {
                        estadoActaContrato,
                        contrato.NumeroContrato,
                        contrato.ContratoId
                    });
                });
            
                  
         
                return ListContratacionDynamic;
            }
            catch (Exception ex)
            {
                return new List<dynamic>();
            }

        }

    }
}
