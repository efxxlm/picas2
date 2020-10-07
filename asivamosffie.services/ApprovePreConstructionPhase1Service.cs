using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services
{
    public class ApprovePreConstructionPhase1Service : IApprovePreConstructionPhase1Service
    {
        private readonly devAsiVamosFFIEContext _context;

        public ApprovePreConstructionPhase1Service(devAsiVamosFFIEContext devAsiVamosFFIEContext ) {
            _context = devAsiVamosFFIEContext; 

        }
         
        public async Task<dynamic> GetListContratacion()
        {
            try
            {
                List<dynamic> ListContratacion = new List<dynamic>();
                List<Contrato> ListContratosConPolizasYDRP = new List<Contrato>();
                List<Contrato> listContratos = await _context.Contrato
                    .Where(r => !(bool)r.Eliminado)
                          .Include(r => r.Contratacion)
                             .ThenInclude(r => r.DisponibilidadPresupuestal)
                    .Include(r => r.Contratacion)
                         .ThenInclude(r => r.ContratacionProyecto)
                             .ThenInclude(r => r.Proyecto)
                       .Include(r => r.ContratoPoliza)
                          .ThenInclude(r => r.PolizaGarantia).ToListAsync();

                List<Dominio> listEstadosVerificacionContrato = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Verificacion_Contrato).ToList();

                listContratos = listContratos.Where(r => r.ContratoPoliza.Count() > 0).ToList();

                //TODO Ver boton 
                foreach (var contrato in listContratos)
                {
                    foreach (var DisponibilidadPresupuestal in contrato.Contratacion.DisponibilidadPresupuestal)
                    {
                        //Si no tiene drp No tiene registro presupuestal
                        if (!string.IsNullOrEmpty(DisponibilidadPresupuestal.NumeroDrp))
                        {
                            ListContratosConPolizasYDRP.Add(contrato);
                        }
                    }
                }
                //TODO Validar DRP
                foreach (var ContratoConPolizasYDRP in ListContratosConPolizasYDRP)
                {
                    int ProyectosNoCompletos = 0;
                    bool VerBotonAprobarInicio = true;

                    if (ContratoConPolizasYDRP.ContratoPerfil.Count() == 0)
                    {
                        VerBotonAprobarInicio = false;
                    }
                    foreach (var ContratoPerfil in ContratoConPolizasYDRP.ContratoPerfil)
                    {
                        if (!string.IsNullOrEmpty(ContratoPerfil.FechaAprobacion.ToString()))
                        {

                            VerBotonAprobarInicio = false;
                        }
                    }
                    foreach (var ContratacionProyecto in ContratoConPolizasYDRP.Contratacion.ContratacionProyecto)
                    {
                        if (ContratacionProyecto.Proyecto.RegistroCompleto == null || (bool)ContratacionProyecto.Proyecto.RegistroCompleto)
                        {

                            ProyectosNoCompletos++;
                        }
                    }
                    int CantidadProyectosAsociados = ContratoConPolizasYDRP.Contratacion.ContratacionProyecto.Where(r => !(bool)r.Eliminado).Count();
                    int ProyectosCompletos = CantidadProyectosAsociados - ProyectosNoCompletos;


                    if (ContratoConPolizasYDRP.ContratoPoliza.FirstOrDefault().FechaAprobacion != null)
                    {
                        ListContratacion.Add(new
                        {
                            FechaAprobacionPoliza = ((DateTime)ContratoConPolizasYDRP.ContratoPoliza.FirstOrDefault().FechaAprobacion).ToString("dd-MM-yyyy"),
                            ContratoConPolizasYDRP.NumeroContrato,
                            CantidadProyectosAsociados,
                            ProyectosCompletos,
                            ProyectosNoCompletos,
                            EstadoVerificacionNombre = listEstadosVerificacionContrato.Where(r => r.Codigo.Equals(ContratoConPolizasYDRP.EstadoVerificacionCodigo)).FirstOrDefault().Nombre,
                            idContrato = ContratoConPolizasYDRP.ContratoId,
                            VerBotonAprobarInicio
                        });
                    }
                }
                return ListContratacion;
            }
            catch (Exception ex)
            {
                return new List<dynamic>();
            }

        }


    }
}
