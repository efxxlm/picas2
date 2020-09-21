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
                      .Include(r => r.Contratacion)
                     .ThenInclude(r => r.DisponibilidadPresupuestal)
                .Include(r => r.Contratacion)
                     .ThenInclude(r => r.ContratacionProyecto)
                      .ThenInclude(r => r.Proyecto)
                   .Include(r => r.ContratoPoliza)
                      .ThenInclude(r => r.PolizaGarantia)
                   .Include(r => r.ContratoPoliza)
                      .ThenInclude(r => r.PolizaObservacion).ToList();

            //List<>
            //listContratos = listContratos.Where(r => r.ContratoPoliza.Count() > 0).ToList();

            foreach (var contrato in listContratos)
            {
                foreach (var DisponibilidadPresupuestal in contrato.Contratacion.DisponibilidadPresupuestal)
                {
                    if (!string.IsNullOrEmpty(DisponibilidadPresupuestal.NumeroDrp))
                    {
                        listContratos.Remove(contrato);
                    }
                }
            }
            //TODO Validar DRP
            foreach (var Contrato in listContratos)
            {
                int ProyectosNoCompletos = 0;
               
                foreach (var ContratacionProyecto in Contrato.Contratacion.ContratacionProyecto)
                {
                    if (ContratacionProyecto.Proyecto.RegistroCompleto == null || (bool)ContratacionProyecto.Proyecto.RegistroCompleto)
                    {

                        ProyectosNoCompletos++;
                    }
                }
                int CantidadProyectosAsociados = Contrato.Contratacion.ContratacionProyecto.Where(r => !(bool)r.Eliminado).Count();
                int ProyectosCompletos = CantidadProyectosAsociados - ProyectosNoCompletos;



                if (!string.IsNullOrEmpty(Contrato.ContratoPoliza.FirstOrDefault().FechaAprobacion.ToString()))
                    ListContratacion.Add(new
                    {
                        FechaAprobacionPoliza = Contrato.ContratoPoliza.FirstOrDefault().FechaAprobacion,
                        Contrato.NumeroContrato,
                        CantidadProyectosAsociados,
                        ProyectosCompletos,
                        ProyectosNoCompletos,

                    });
            }


            return ListContratacion;

        }
    }
}
