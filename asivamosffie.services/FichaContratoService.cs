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
        #region Resumen

        public async Task<dynamic> GetInfoResumenByContratoId(int pContratoId)
        {
            List<dynamic> InfoContrato = new List<dynamic>();

            var contrato = await _context.Contrato
                                .Where(c => c.ContratoId == pContratoId)
                                .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto)
                                .Include(c => c.Contratacion).ThenInclude(c => c.Contratista)
                                .FirstOrDefaultAsync();

            List<VFichaContratoProyectoDrp> vFichaContratoProyectoDrps = _context.VFichaContratoProyectoDrp.Where(c => c.ContratoId == pContratoId).ToList();
            string strEstadoContratacion = await _commonService.GetNombreDominioByCodigoAndTipoDominio(contrato.Contratacion.EstadoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Solicitud);

            foreach (var item in vFichaContratoProyectoDrps)
            {
                item.NombreAportante = GetNombreAportante(_context.CofinanciacionAportante.Find(item.AportanteId));
            }

            List<dynamic>  ListProyectos = new List<dynamic>();  
            foreach (var ContratacionProyecto in contrato.Contratacion.ContratacionProyecto)
            {
                List<VFichaContratoProyectoDrp> vFichaContratoProyectoDrp = vFichaContratoProyectoDrps.Where(c => c.ProyectoId == ContratacionProyecto.ProyectoId)
                                                                                                     .ToList();

                List<dynamic> fuentesUsos = new List<dynamic>();

                foreach (var ProyectoDrp in vFichaContratoProyectoDrp)
                {
                    FuenteFinanciacion fuenteFinanciacion = _context.FuenteFinanciacion.Find(ProyectoDrp.FuenteFinanciacionId);
                    string strNombreFuenteFinanciacion = await _commonService.GetNombreDominioByCodigoAndTipoDominio(fuenteFinanciacion.FuenteRecursosCodigo, (int)EnumeratorTipoDominio.Fuentes_de_financiacion);

                    fuentesUsos.Add(new
                    {
                        Aportante = ProyectoDrp.NombreAportante,
                        ValorAportante = ProyectoDrp.ValorUso,
                        Fuente = strNombreFuenteFinanciacion,
                        Uso = ProyectoDrp.Nombre,
                        ValorUso = ProyectoDrp.ValorUso
                    });
                }
                VFichaContratoProyectoDrp Proyecto = vFichaContratoProyectoDrp.FirstOrDefault();

                dynamic ProyectoInf = new
                {
                    LlaveMen = Proyecto.LlaveMen,
                    TipoIntervencion = Proyecto.TipoIntervencion,
                    Departamento = Proyecto.Departamento,
                    Municipio = Proyecto.Municipio,
                    InstitucionEducativaSede = Proyecto.InstitucionEducativa,
                    Sede = Proyecto.Sede,
                    FuentesUsos = fuentesUsos,
                    Inversion = vFichaContratoProyectoDrp.GroupBy(c => c.NumeroDrp)
                                                                .Select(s => new
                                                                {
                                                                    NumeroDrp = s.Key,
                                                                    Valor = s.Sum(c => c.ValorUso)
                                                                })
                                                                .ToList()
                };

                ListProyectos.Add(new
                {
                    Proyecto = ProyectoInf
                });
            }


            InfoContrato.Add(new
            {
                contrato = new
                {
                    NumeroContrato = contrato.NumeroContrato,
                    Contratista = contrato.Contratacion.Contratista.Nombre,
                    EstadoContrato = strEstadoContratacion,
                    ValorContrato = vFichaContratoProyectoDrps.Sum(c => c.ValorUso),
                    TipoContrato = contrato.Contratacion.TipoSolicitudCodigo ==
                ConstanCodigoTipoContratacionString.Obra ? (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(ConstanCodigoTipoContratacionSTRING.Obra))
                : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(ConstanCodigoTipoContratacionSTRING.Interventoria),
                    FechaSuscripcion = contrato.FechaAprobacionPoliza
                },
                ListProyectos
            });

            return InfoContrato;
        }


        #endregion

        #region Busqueda
        public async Task<dynamic> GetFlujoContratoByContratoId(int pContratoId)
        {
            var contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId)
                                                .Include(r => r.ContratoPoliza).ThenInclude(c => c.PolizaGarantia)
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
                    DepartamentoMunicipio = Departamento.Descripcion + " / " + ContratacionProyecto.Proyecto.LocalizacionIdMunicipioNavigation.Descripcion,
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
                TieneControversias = true,
                TieneProcesosDefenzaJudicial = true,
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
        #endregion

        #region Utilidades

        private string GetNombreAportante(CofinanciacionAportante confinanciacion)
        {
            string nombreAportante;
            if (confinanciacion.TipoAportanteId.Equals(ConstanTipoAportante.Ffie))
            {
                nombreAportante = ConstanStringTipoAportante.Ffie;
            }
            else if (confinanciacion.TipoAportanteId.Equals(ConstanTipoAportante.Tercero))
            {
                nombreAportante = confinanciacion.NombreAportanteId == null
                    ? "Error" :
                    _context.Dominio.Find(confinanciacion.NombreAportanteId).Nombre;
            }
            else
            {
                if (confinanciacion.MunicipioId == null)
                {
                    nombreAportante = confinanciacion.DepartamentoId == null
                    ? "Error" :
                    "Gobernación " + _context.Localizacion.Find(confinanciacion.DepartamentoId).Descripcion;
                }
                else
                {
                    nombreAportante = confinanciacion.MunicipioId == null
                    ? "Error" :
                    "Alcaldía " + _context.Localizacion.Find(confinanciacion.MunicipioId).Descripcion;
                }
            }
            return nombreAportante;
        }


        #endregion
    }
}
