using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
namespace asivamosffie.services
{
    public class FichaContratoService : IFichaContratoService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public FichaContratoService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;

        }
        public async Task<dynamic> GetFlujoContratoByContratoId(int pContratoId)
        { 
            var contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId)
                                                .Include(r=> r.ContratoPoliza).ThenInclude(c=> c.PolizaGarantia)
                                                .Include(c => c.Contratacion).ThenInclude(c => c.Contratista)
                                                .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(r => r.Proyecto) 
                                                .FirstOrDefault();


            List<dynamic> infoProyectos = new List<dynamic>();

            foreach (var ContratacionProyecto in contrato.Contratacion.ContratacionProyecto)
            {
                Proyecto proyecto = _context.Proyecto.Where(r => r.ProyectoId == ContratacionProyecto.ProyectoId)
                                                    .Include(c => c.InstitucionEducativa)
                                                    .Include(c => c.Sede)
                                                    .Include(c => c.LocalizacionIdMunicipioNavigation)
                                                    .FirstOrDefault();

                Localizacion Departamento = _context.Localizacion.Find(proyecto.LocalizacionIdMunicipioNavigation.IdPadre);
                infoProyectos.Add(new 
                {
                   DepartamentoMunicipio = Departamento.Descripcion + " / "+ ContratacionProyecto.Proyecto.LocalizacionIdMunicipioNavigation.Descripcion,
                   InstitucionEducativaSede = ContratacionProyecto.Proyecto.InstitucionEducativa.Nombre + " / " + ContratacionProyecto.Proyecto.Sede.Nombre,
                }); 
            }

            var infoContrato = new
            {
                ContratoId = pContratoId, 
                NumeroContrato = contrato.NumeroContrato,
                Contratista = contrato.Contratacion.Contratista.Nombre,
                InfoProyectos = infoProyectos,
                TipoContrato = contrato.Contratacion.TipoSolicitudCodigo == 
                ConstanCodigoTipoContratacionString.Obra ? (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(ConstanCodigoTipoContratacionSTRING.Obra)) 
                : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(ConstanCodigoTipoContratacionSTRING.Interventoria),
                Vigencia = contrato.ContratoPoliza.FirstOrDefault().PolizaGarantia.FirstOrDefault().Vigencia
            };
             
            return new
            {
                Informacion = infoContrato,
                TieneResumen = true,
                TieneSeleccion = true,
                TieneContratacion = true,
                TienePolizasSeguros = true,
                TieneEjecucionFinanciera = true,
                TieneNovedades = true,
                TieneControversias= true,
                TieneProcesosDefenzaJudicial= true,
                TieneLiquidacion = true,
            }; 
        }
        public async Task<dynamic> GetContratosByNumeroContrato(string pNumeroContrato)
        {

            List<VFichaContratoBusquedaContrato> LsitVFichaContratoBusquedaContrato = await _context.VFichaContratoBusquedaContrato
                                                                                        //.Where(r => r.NumeroContrato.ToUpper().Contains(pNumeroContrato.ToUpper()))
                                                                                        .ToListAsync();

            return LsitVFichaContratoBusquedaContrato.OrderByDescending(o => o.ContratoId).ToList();
        }
    }
}
