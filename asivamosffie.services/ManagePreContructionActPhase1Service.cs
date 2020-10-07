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
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace asivamosffie.services
{
    public class ManagePreContructionActPhase1Service : IManagePreContructionActPhase1Service
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public ManagePreContructionActPhase1Service(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }
        public async Task<dynamic> GetListContrato()
        { 
            try
            {
                List<Contrato> listContratos = await _context.Contrato
                       .Where(r => r.EstadoVerificacionCodigo == ConstanCodigoEstadoVerificacionContratoObra.Con_requisitos_del_contratista_de_obra_avalados).ToListAsync();

                List<Dominio> listEstadosActa = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Del_Acta_Contrato).ToList();

                List<dynamic> ListContratacionDynamic = new List<dynamic>();
                 
                listContratos.ForEach(contrato =>
                { 
                    ListContratacionDynamic.Add(new
                    { 
                        fechaAprobacionRequisitosSupervisor = "",
                        contrato.NumeroContrato,
                        estadoActaContrato = listEstadosActa.Where(r => r.Codigo == contrato.EstadoActa).FirstOrDefault().Nombre,
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



        public async Task<Contrato> GetContratoByContratoId(int pContratoId) {

            try
            {
                Contrato contrato = await _context.Contrato.Where(r => r.ContratoId == pContratoId)
                    .Include(r => r.Contratacion)
                     .ThenInclude(r => r.DisponibilidadPresupuestal).FirstOrDefaultAsync();



                return contrato;
            }
            catch (Exception)
            {
                return new Contrato();
            }    
        }

    }
}
