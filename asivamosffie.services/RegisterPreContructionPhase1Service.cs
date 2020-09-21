using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services
{
    public class RegisterPreContructionPhase1Service : IRegisterPreContructionPhase1Service
    {
        private readonly devAsiVamosFFIEContext _context;

        public RegisterPreContructionPhase1Service(devAsiVamosFFIEContext context)
        {
            _context = context;
        }


        public async Task<dynamic> GetListContratacion()
        {

            List<dynamic> ListContratacion = new List<dynamic>();

            List<Contrato> listContratos = _context.Contrato
                .Where(r => !(bool)r.Eliminado)
                   .Include(r => r.ContratoPoliza)
                      .ThenInclude(r => r.PolizaGarantia)
                   .Include(r => r.ContratoPoliza)
                      .ThenInclude(r => r.PolizaObservacion).ToList();

            foreach (var Contrato in listContratos.Where(r=> r.ContratoPoliza.Count() > 0))
            {

                if (!string.IsNullOrEmpty(Contrato.ContratoPoliza.FirstOrDefault().FechaAprobacion.ToString()))
                    ListContratacion.Add(new
                    {
                        Contrato.ContratoPoliza.FirstOrDefault().FechaAprobacion,
                        Contrato.NumeroContrato,



                    });
            }


            return ListContratacion;

        }
    }
}
