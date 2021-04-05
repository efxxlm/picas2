using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class RegisterContractualLiquidationRequestService : IRegisterContractualLiquidationRequestService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirementsConstructionPhaseService;

        public RegisterContractualLiquidationRequestService(devAsiVamosFFIEContext context, ICommonService commonService, ITechnicalRequirementsConstructionPhaseService technicalRequirementsConstructionPhaseService)
        {
            _context = context;
            _commonService = commonService;
            _technicalRequirementsConstructionPhaseService = technicalRequirementsConstructionPhaseService;
        }

        public async Task<List<VContratacionProyectoSolicitudLiquidacion>> GridRegisterContractualLiquidationObra()
        {
            return await _context.VContratacionProyectoSolicitudLiquidacion.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString()).ToListAsync();
        }

        public async Task<List<VContratacionProyectoSolicitudLiquidacion>> GridRegisterContractualLiquidationInterventoria()
        {
            return await _context.VContratacionProyectoSolicitudLiquidacion.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString()).ToListAsync();
        }

        public async Task<List<dynamic>> GridInformeFinal(int pContratacionProyectoId)
        {
            List<dynamic> ProyectoAjustado = new List<dynamic>();

            ContratacionProyecto contratacionProyecto = _context.ContratacionProyecto.Find(pContratacionProyectoId);

            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();

            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            if (contratacionProyecto != null)
            {
                Proyecto proyecto = await _context.Proyecto.Where(r => r.ProyectoId == contratacionProyecto.ProyectoId)
                                            .Include(r => r.InformeFinal)
                                             .Include(r => r.InstitucionEducativa)
                                             .FirstOrDefaultAsync();

                InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == proyecto.SedeId).FirstOrDefault();
                Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                proyecto.MunicipioObj = Municipio;
                proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                proyecto.Sede = Sede;

                ProyectoAjustado.Add(new
                {
                    fechaEnvio = proyecto.InformeFinal.FirstOrDefault().FechaEnvioEtc,
                    fechaAprobacion = proyecto.InformeFinal.FirstOrDefault().FechaAprobacionFinal,
                    llaveMen = proyecto.LlaveMen,
                    tipoIntervencion = proyecto.tipoIntervencionString,
                    sede = proyecto.Sede.Nombre,
                    estadoValidacion = "Sin validación",//definir!
                    proyectoId = proyecto.ProyectoId,
                    informeFinalId = proyecto.InformeFinal.FirstOrDefault().InformeFinalId,
                    institucionEducativa = proyecto.InstitucionEducativa.Nombre
                });
            }

            return ProyectoAjustado;
        }

        public async Task<List<dynamic>> GetInformeFinalByProyectoId(int pProyectoId)
        {
            String numeroContratoObra = string.Empty, nombreContratistaObra = string.Empty, numeroContratoInterventoria = string.Empty, nombreContratistaInterventoria = string.Empty;

            List<dynamic> ProyectoAjustado = new List<dynamic>();
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            List<Dominio> TipoObraIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar).ToList();

            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            Proyecto proyecto = await _context.Proyecto.Where(r => r.ProyectoId == pProyectoId)
                                                            .Include(r => r.InformeFinal)
                                                            .Include(r => r.InstitucionEducativa)
                                                            .FirstOrDefaultAsync();

            InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == proyecto.SedeId).FirstOrDefault();
            Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipio).FirstOrDefault();
            proyecto.MunicipioObj = Municipio;
            proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
            proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
            proyecto.Sede = Sede;
            List<ContratacionProyecto> ListContratacion = await _context.ContratacionProyecto
                                                        .Where(r => r.ProyectoId == pProyectoId)
                                                        .Include(r => r.Contratacion)
                                                         .ThenInclude(r => r.Contratista)
                                                        .Include(r => r.Contratacion)
                                                         .ThenInclude(r => r.Contrato)
                                                        .ToListAsync();

           ListContratacion.FirstOrDefault().Contratacion.TipoContratacionCodigo = TipoObraIntervencion.Where(r => r.Codigo == ListContratacion.FirstOrDefault().Contratacion.TipoSolicitudCodigo).Select(r => r.Nombre).FirstOrDefault();

            foreach (var item in ListContratacion)
            {
                Contrato contrato = await _context.Contrato.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefaultAsync();
                Contratacion contratacion = await _context.Contratacion.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefaultAsync();
                if (contrato != null)
                {
                    if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Obra)
                    {

                        nombreContratistaObra = contratacion.Contratista != null ? contratacion.Contratista.Nombre : string.Empty;
                        numeroContratoObra = contrato.NumeroContrato != null ? contrato.NumeroContrato : string.Empty;
                    }
                    else if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria)
                    {
                        nombreContratistaInterventoria = contratacion.Contratista != null ? contratacion.Contratista.Nombre : string.Empty;
                        numeroContratoInterventoria = contrato.NumeroContrato != null ? contrato.NumeroContrato : string.Empty;
                    }
                }
            }
            ProyectoAjustado.Add(new
            {
                numeroContratoObra = numeroContratoObra,
                nombreContratistaObra = nombreContratistaObra,
                numeroContratoInterventoria = numeroContratoInterventoria,
                nombreContratistaInterventoria = nombreContratistaInterventoria,
                informeFinal = proyecto.InformeFinal.FirstOrDefault()
            });

            return ProyectoAjustado;
        }

        public async Task<List<InformeFinalInterventoria>> GetInformeFinalAnexoByInformeFinalId(int pInformeFinalId)
        {
            List<InformeFinalInterventoria> ListInformeFinalChequeo = await _context.InformeFinalInterventoria
                    .Where(r => r.InformeFinalId == pInformeFinalId)
                    .Include(r => r.InformeFinalListaChequeo)
                    .Include(r => r.InformeFinalAnexo)
                    .OrderBy(r => r.InformeFinalListaChequeo.Posicion)
                    .ToListAsync();

            foreach (var item in ListInformeFinalChequeo)
            {
                item.CalificacionCodigoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.CalificacionCodigo, (int)EnumeratorTipoDominio.Calificacion_Informe_Final);

                if (item.InformeFinalAnexoId != null)
                {
                    item.InformeFinalAnexo.TipoAnexoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.InformeFinalAnexo.TipoAnexo, (int)EnumeratorTipoDominio.Tipo_Anexo_Informe_Final);
                }
            }
            return ListInformeFinalChequeo;
        }




    }
}
