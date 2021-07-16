using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class BudgetAvailabilityService : IBudgetAvailabilityService
    {
        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;
        public readonly IConverter _converter;
        public readonly IRequestBudgetAvailabilityService _requestBudgetAvailabilityService;

        public BudgetAvailabilityService(devAsiVamosFFIEContext context, IRequestBudgetAvailabilityService requestBudgetAvailabilityService, ICommonService commonService, IConverter converter)
        {
            _requestBudgetAvailabilityService = requestBudgetAvailabilityService;
            _context = context;
            _commonService = commonService;
            _converter = converter;
        }

        public async Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestal()
        {
            List<DisponibilidadPresupuestal> ListDisponibilidadPresupuestal = await _context.DisponibilidadPresupuestal.Where(r => !(bool)r.Eliminado).ToListAsync();

            List<DisponibilidadPresupuestalGrilla> ListDisponibilidadPresupuestalGrilla = new List<DisponibilidadPresupuestalGrilla>();

            foreach (var DisponibilidadPresupuestal in ListDisponibilidadPresupuestal)
            {
                string strEstadoRegistro = "";
                string strTipoSolicitud = "";

                if (string.IsNullOrEmpty(DisponibilidadPresupuestal.EstadoSolicitudCodigo))
                {
                    strEstadoRegistro = await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.EstadoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal);
                }

                if (string.IsNullOrEmpty(DisponibilidadPresupuestal.TipoSolicitudCodigo))
                {
                    strTipoSolicitud = await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_de_Solicitud);
                }
                bool blnEstado = false;

                //si es administrativo, esta completo, si es tradicional, se verifica contra fuentes gestionadas
                if (DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo)
                {
                    blnEstado = true;
                }
                else if (DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)
                {
                    blnEstado = true;
                }
                else
                {
                    List<int> ddpproyectosId = DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.Select(x => x.DisponibilidadPresupuestalProyectoId).ToList();
                    if (_context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyectoId != null && ddpproyectosId.Contains((int)x.DisponibilidadPresupuestalProyectoId)).Count() > 0)
                    {
                        blnEstado = true;
                    }
                }
                DisponibilidadPresupuestalGrilla disponibilidadPresupuestalGrilla = new DisponibilidadPresupuestalGrilla
                {

                    FechaSolicitud = DisponibilidadPresupuestal.FechaSolicitud.ToString(),
                    EstadoRegistro = blnEstado,
                    TipoSolicitudEspecial = DisponibilidadPresupuestal.TipoSolicitudEspecialCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.TipoSolicitudEspecialCodigo, (int)EnumeratorTipoDominio.Tipo_DDP_Espacial) :
                    //si no viene el campo puede ser contratación
                    DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? ConstanStringTipoSolicitudContratacion.proyectoAdministrativo :
                    ConstanStringTipoSolicitudContratacion.contratacion,
                    TipoSolicitud = strTipoSolicitud,
                    DisponibilidadPresupuestalId = DisponibilidadPresupuestal.DisponibilidadPresupuestalId,
                    NumeroSolicitud = DisponibilidadPresupuestal.NumeroSolicitud,
                    //NovedadContractual = DisponibilidadPresupuestal.NovedadContractualId != null ? _context.NovedadContractual.Where(x => x.NovedadContractualId == DisponibilidadPresupuestal.NovedadContractualId).Include(x => x.NovedadContractualDescripcion).FirstOrDefault() : null
                };

                ListDisponibilidadPresupuestalGrilla.Add(disponibilidadPresupuestalGrilla);
            }

            return ListDisponibilidadPresupuestalGrilla.OrderByDescending(r => r.EstadoRegistro).ToList();
        }

        public async Task<DisponibilidadPresupuestal> GetDisponibilidadPresupuestalByID(int pDisponibilidadPresupuestalId)
        {
            //las tabla DisponibilidadPresupuestalProyecto no tiene campos de auditoria
            var resultado = await _context.DisponibilidadPresupuestal.
                Where(r => r.DisponibilidadPresupuestalId == pDisponibilidadPresupuestalId).
                Include(r => r.DisponibilidadPresupuestalProyecto).
                ThenInclude(r => r.Proyecto).
                Include(x => x.DisponibilidadPresupuestalObservacion)
                .FirstOrDefaultAsync();
            //busco comite técnico
            DateTime fechaComitetecnico = DateTime.Now;
            string numerocomietetecnico = "";
            foreach (var res in resultado.DisponibilidadPresupuestalProyecto)
            {
                res.Proyecto.tipoIntervencionString = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion &&
                  x.Codigo == res.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                res.Proyecto.sedeString = _context.InstitucionEducativaSede.Find(res.Proyecto.SedeId).Nombre;
                res.Proyecto.institucionEducativaString = _context.InstitucionEducativaSede.Find(res.Proyecto.InstitucionEducativaId).Nombre;
            }

            if (resultado.ContratacionId != null)
            {
                var contratacion = _context.SesionComiteSolicitud.Where(x => x.SolicitudId == resultado.ContratacionId && x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion).
                    Include(x => x.ComiteTecnico).ToList();
                if (contratacion.Count() > 0)
                {
                    fechaComitetecnico = Convert.ToDateTime(contratacion.FirstOrDefault().ComiteTecnico.FechaOrdenDia);
                }
            }
            resultado.FechaComiteTecnicoNotMapped = fechaComitetecnico;
            if (resultado.AportanteId > 0)
            {
                resultado.stringAportante = getNombreAportante(_context.CofinanciacionAportante.Find(resultado.AportanteId));
                resultado.stringTipoAportante = getNombreTipoAportante(_context.CofinanciacionAportante.Find(resultado.AportanteId));
            }

            return resultado;

        }

        public async Task<DisponibilidadPresupuestal> GetBudgetAvailabilityById(int id)
        {
            try
            {
                var dis = await _context.DisponibilidadPresupuestal.Where(d => d.DisponibilidadPresupuestalId == id)
                                    .Include(r => r.DisponibilidadPresupuestalProyecto)
                                    .Include(x => x.DisponibilidadPresupuestalObservacion)
                                    .Include(x => x.Contratacion)
                                    .FirstOrDefaultAsync();
                DateTime fechaComitetecnico = DateTime.Now;
                string numerocomietetecnico = "";
                if (dis.ContratacionId != null)
                {
                    var contratacion = _context.SesionComiteSolicitud.Where(x => x.SolicitudId == dis.ContratacionId && x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion).
                        Include(x => x.ComiteTecnico).ToList();
                    if (contratacion.Count() > 0)
                    {
                        fechaComitetecnico = Convert.ToDateTime(contratacion.FirstOrDefault().ComiteTecnico.FechaOrdenDia);
                    }
                }
                dis.FechaComiteTecnicoNotMapped = fechaComitetecnico;

                return dis;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(string pCodigoEstadoSolicitud, bool showRechazado)
        { 
            List<DisponibilidadPresupuestal> ListDisponibilidadPresupuestal =
                                                                                await _context.DisponibilidadPresupuestal
                                                                                                    .Where(r => !(bool)r.Eliminado &&
                                                                                                            r.EstadoSolicitudCodigo.Equals(pCodigoEstadoSolicitud))
                                                                                                    .Include(x => x.DisponibilidadPresupuestalProyecto)
                                                                                                    .Include(x => x.GestionFuenteFinanciacion)
                                                                                                     .ToListAsync();

            List<NovedadContractualRegistroPresupuestal> listRegistroPresupuestal =
                                                                                    await _context.NovedadContractualRegistroPresupuestal
                                                                                                        .Where(x => x.Eliminado != true &&
                                                                                                               x.EstadoSolicitudCodigo.Equals(pCodigoEstadoSolicitud))
                                                                                                        .Include(x => x.DisponibilidadPresupuestal)
                                                                                                            .ThenInclude(x => x.DisponibilidadPresupuestalProyecto)
                                                                                                        .Include(x => x.DisponibilidadPresupuestal)
                                                                                                            .ThenInclude(x => x.GestionFuenteFinanciacion)
                                                                                                        .ToListAsync();
            if (pCodigoEstadoSolicitud == "5")
            {
                List<DisponibilidadPresupuestal> ListDisponibilidadPresupuestalConDrp =
                                                                                await _context.DisponibilidadPresupuestal
                                                                                                    .Where(r => !(bool)r.Eliminado &&
                                                                                                            r.EstadoSolicitudCodigo.Equals("8"))
                                                                                                    .Include(x => x.DisponibilidadPresupuestalProyecto)
                                                                                                    .Include(x => x.GestionFuenteFinanciacion)
                                                                                                    .ToListAsync();

                ListDisponibilidadPresupuestal.AddRange(ListDisponibilidadPresupuestalConDrp);
            }


            List<GestionFuenteFinanciacion> ListGestionFuenteFinanciacion = _context.GestionFuenteFinanciacion.ToList();
            List<ProyectoAportante> ListProyectoAportante = _context.ProyectoAportante.ToList();
            List<Contratacion> ListContratacion = _context.Contratacion.ToList();
            List<Contrato> ListContrato = _context.Contrato.ToList();

            foreach (var RegistroPresupuestal in listRegistroPresupuestal)
            {
                RegistroPresupuestal.DisponibilidadPresupuestal.NovedadContractualRegistroPresupuestalId = RegistroPresupuestal.NovedadContractualRegistroPresupuestalId;
                RegistroPresupuestal.DisponibilidadPresupuestal.NovedadContractualId = RegistroPresupuestal.NovedadContractualId;
                RegistroPresupuestal.DisponibilidadPresupuestal.EsNovedad = true;
                RegistroPresupuestal.DisponibilidadPresupuestal.RegistroCompleto = RegistroPresupuestal.RegistroCompleto;
                RegistroPresupuestal.DisponibilidadPresupuestal.NumeroSolicitud = RegistroPresupuestal.NumeroSolicitud;
                RegistroPresupuestal.DisponibilidadPresupuestal.ValorSolicitud = RegistroPresupuestal.ValorSolicitud;
                RegistroPresupuestal.DisponibilidadPresupuestal.EstadoSolicitudCodigo = RegistroPresupuestal.EstadoSolicitudCodigo;
                RegistroPresupuestal.DisponibilidadPresupuestal.Objeto = RegistroPresupuestal.Objeto;
                RegistroPresupuestal.DisponibilidadPresupuestal.FechaDdp = RegistroPresupuestal.FechaDdp;
                RegistroPresupuestal.DisponibilidadPresupuestal.NumeroDrp = RegistroPresupuestal.NumeroDrp; 
                RegistroPresupuestal.DisponibilidadPresupuestal.FechaDrp = RegistroPresupuestal.FechaDrp;
                RegistroPresupuestal.DisponibilidadPresupuestal.FechaDrp = RegistroPresupuestal.FechaDrp;

                ListDisponibilidadPresupuestal.Remove(RegistroPresupuestal.DisponibilidadPresupuestal);
                ListDisponibilidadPresupuestal.Add(RegistroPresupuestal.DisponibilidadPresupuestal);
            }
            List<DisponibilidadPresupuestalGrilla> ListDisponibilidadPresupuestalGrilla = new List<DisponibilidadPresupuestalGrilla>();

            List<Dominio> listaDominioEstadoSolicitud = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal).ToList();
            List<Dominio> listaDominioTipoSolicitud = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal).ToList();
            List<Dominio> listaDominioTipoEspecial = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_DDP_Espacial).ToList();

            try
            {
                foreach (var DisponibilidadPresupuestal in ListDisponibilidadPresupuestal)
                {
                    string strEstadoRegistro = "";
                    string strTipoSolicitud = "";
                    if (!string.IsNullOrEmpty(DisponibilidadPresupuestal.EstadoSolicitudCodigo))
                        strEstadoRegistro = listaDominioEstadoSolicitud.Where(r => r.Codigo == DisponibilidadPresupuestal.EstadoSolicitudCodigo)?.FirstOrDefault()?.Nombre;

                    if (!string.IsNullOrEmpty(DisponibilidadPresupuestal.TipoSolicitudCodigo))
                        strTipoSolicitud = listaDominioTipoSolicitud.Where(r => r.Codigo == DisponibilidadPresupuestal.TipoSolicitudCodigo)?.FirstOrDefault()?.Nombre;


                    DateTime? fechaContrato = null;
                    string numeroContrato = "";
                    if (DisponibilidadPresupuestal.ContratacionId != null)
                    {

                        var contrato = ListContrato.Where(r => r.ContratacionId == DisponibilidadPresupuestal.ContratacionId).FirstOrDefault();
                        fechaContrato = contrato?.FechaFirmaContrato;
                        numeroContrato = DisponibilidadPresupuestal.NumeroContrato ?? (contrato != null ? contrato.NumeroContrato : "");
                    }
                    else
                    {
                        numeroContrato = DisponibilidadPresupuestal.NumeroContrato ?? "";
                    }
                    bool blnEstado = false;

                    //si es administrativo, esta completo, si es tradicional, se verifica contra fuentes gestionadas
                    //2020-11-08 ahora los administrativos y especiales tambien estionan fuentes
                    if (DisponibilidadPresupuestal.EstadoSolicitudCodigo != ((int)EnumeratorEstadoSolicitudPresupuestal.Rechazada_por_validacion_presupuestal).ToString())
                    {
                        if (DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo)
                        {
                            List<int> ddpproyectosId = DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.Select(x => x.DisponibilidadPresupuestalProyectoId).ToList();
                            if (ListGestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == DisponibilidadPresupuestal.DisponibilidadPresupuestalId).Count() > 0)
                                blnEstado = true;

                        }
                        else if (DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)
                        {
                            if (ListGestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == DisponibilidadPresupuestal.DisponibilidadPresupuestalId).Count() > 0)
                                blnEstado = true;
                        }
                        else
                        {
                            List<int> proyectosId = DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.Where(x => x.ProyectoId > 0).Select(x => (int)x.ProyectoId).ToList();
                            List<int> ddpproyectosId = DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.Select(x => (int)x.DisponibilidadPresupuestalProyectoId).ToList();
                            var aportantes = ListProyectoAportante.Where(x => proyectosId.Contains(x.ProyectoId)).ToList();
                            //var fuentes = _context.FuenteFinanciacion.Where(x => aportantes.Contains(x.AportanteId)).Count();

                            if (DisponibilidadPresupuestal.EsNovedad != true)
                            {
                                if (ListGestionFuenteFinanciacion
                                    .Where(x => x.DisponibilidadPresupuestalProyectoId != null &&
                                           ddpproyectosId.Contains((int)x.DisponibilidadPresupuestalProyectoId))
                                    .Count() == aportantes.Count())
                                    blnEstado = true;
                            }
                            else
                            {
                                if (ListGestionFuenteFinanciacion
                                    .Where(x => x.DisponibilidadPresupuestalProyectoId != null && x.EsNovedad == true && x.NovedadContractualRegistroPresupuestalId == DisponibilidadPresupuestal.NovedadContractualRegistroPresupuestalId &&
                                           ddpproyectosId.Contains((int)x.DisponibilidadPresupuestalProyectoId))
                                    .Count() == aportantes.Count())
                                    blnEstado = true;
                            }
                        }
                    }
                    else
                    {
                        blnEstado = true;
                    }

                    var contratacion = ListContratacion.Where(x => x.ContratacionId == DisponibilidadPresupuestal.ContratacionId).ToList();
                    // LCT
                    bool rechazadaFiduciaria = ListContratacion.Where(x => x.ContratacionId == DisponibilidadPresupuestal.ContratacionId && x.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.RechazadoComiteFiduciario && x.Eliminado != true).Count() > 0;

                    DisponibilidadPresupuestalGrilla disponibilidadPresupuestalGrilla = new DisponibilidadPresupuestalGrilla
                    {

                        FechaSolicitud = DisponibilidadPresupuestal.FechaSolicitud.ToString("dd/MM/yyyy"),
                        TipoSolicitud = strTipoSolicitud,
                        TipoSolicitudEspecial = DisponibilidadPresupuestal.TipoSolicitudEspecialCodigo != null ? listaDominioTipoEspecial.Where(r => r.Codigo == DisponibilidadPresupuestal.TipoSolicitudEspecialCodigo)?.FirstOrDefault()?.Nombre :
                        //si no viene el campo puede ser contratación
                        DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? "Proyecto administrativo" :
                        "Contratación",
                        DisponibilidadPresupuestalId = DisponibilidadPresupuestal.DisponibilidadPresupuestalId,
                        NumeroSolicitud = DisponibilidadPresupuestal.NumeroSolicitud,
                        NumeroDDP = DisponibilidadPresupuestal.NumeroDdp,
                        FechaFirmaContrato = fechaContrato == null ? "" : Convert.ToDateTime(fechaContrato).ToString("dd/MM/yyyy"),
                        NumeroContrato = numeroContrato,
                        Contratacion = contratacion,
                        NovedadContractualRegistroPresupuestalId = DisponibilidadPresupuestal.NovedadContractualRegistroPresupuestalId,
                        EsNovedad = DisponibilidadPresupuestal.EsNovedad,
                        NovedadContractualId = DisponibilidadPresupuestal.NovedadContractualId,
                        EstadoRegistro = blnEstado,
                        RechazadaFiduciaria = rechazadaFiduciaria
                    };
                    // si showRechazado, muestra sin excepción
                    if (showRechazado)
                    {
                        ListDisponibilidadPresupuestalGrilla.Add(disponibilidadPresupuestalGrilla);
                    }
                    else
                    {
                        //No muestra los que estan rechazados por fiduciaria en contratación
                        if (!rechazadaFiduciaria)
                        {
                            ListDisponibilidadPresupuestalGrilla.Add(disponibilidadPresupuestalGrilla);
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

            return ListDisponibilidadPresupuestalGrilla.OrderByDescending(r => r.DisponibilidadPresupuestalId).ToList();

        }

        /*3.3.4 listado de rp
         jflorez: cambio 20201118 deben filtrarse por los contratos que esten en estado registrado*/
        public async Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestalContratacionByCodigoEstadoSolicitud(string pCodigoEstadoSolicitud)
        {
            int codigocondvalidacionpre = (int)EnumeratorEstadoSolicitudPresupuestal.Con_registro_presupuestal;
            int codigocancelada = (int)EnumeratorEstadoSolicitudPresupuestal.Sin_registro_presupuestal;

            List<DisponibilidadPresupuestal> ListDisponibilidadPresupuestal = await _context.DisponibilidadPresupuestal
                                                                                                .Where(r => !(bool)r.Eliminado
                                                                                                        && (r.EstadoSolicitudCodigo.Equals(pCodigoEstadoSolicitud) || r.EstadoSolicitudCodigo.Equals(codigocondvalidacionpre.ToString())
                                                                                                        || r.EstadoSolicitudCodigo.Equals(codigocancelada.ToString()))
                                                                                                        && r.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Tradicional
                                                                                                        && r.Contratacion.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.Registrados)
                                                                                                .Include(x => x.DisponibilidadPresupuestalProyecto).Include(x => x.Contratacion).ThenInclude(x => x.Contrato)
                                                                                                .ToListAsync();

            List<NovedadContractualRegistroPresupuestal> listRegistroPresupuestal =
                                                                                    await _context.NovedadContractualRegistroPresupuestal
                                                                                                        .Where(x => x.Eliminado != true &&
                                                                                                               (x.EstadoSolicitudCodigo.Equals(pCodigoEstadoSolicitud) || x.EstadoSolicitudCodigo.Equals(codigocondvalidacionpre.ToString())
                                                                                                                || x.EstadoSolicitudCodigo.Equals(codigocancelada.ToString())))
                                                                                                        .Include(x => x.DisponibilidadPresupuestal)
                                                                                                            .ThenInclude(x => x.DisponibilidadPresupuestalProyecto)
                                                                                                        .Include(x => x.DisponibilidadPresupuestal)
                                                                                                            .ThenInclude(x => x.GestionFuenteFinanciacion)
                                                                                                        .ToListAsync();

            foreach (var RegistroPresupuestal in listRegistroPresupuestal)
            {
                NovedadContractual novedadContractual = _context.NovedadContractual.Find(RegistroPresupuestal.NovedadContractualId);

                RegistroPresupuestal.DisponibilidadPresupuestal.NovedadContractualRegistroPresupuestalId = RegistroPresupuestal.NovedadContractualRegistroPresupuestalId;
                RegistroPresupuestal.DisponibilidadPresupuestal.NovedadContractualId = RegistroPresupuestal.NovedadContractualId;
                RegistroPresupuestal.DisponibilidadPresupuestal.EsNovedad = true;
                RegistroPresupuestal.DisponibilidadPresupuestal.RegistroCompleto = RegistroPresupuestal.RegistroCompleto;
                RegistroPresupuestal.DisponibilidadPresupuestal.NumeroSolicitud = RegistroPresupuestal.NumeroSolicitud;
                RegistroPresupuestal.DisponibilidadPresupuestal.ValorSolicitud = RegistroPresupuestal.ValorSolicitud;
                RegistroPresupuestal.DisponibilidadPresupuestal.EstadoSolicitudCodigo = RegistroPresupuestal.EstadoSolicitudCodigo;
                RegistroPresupuestal.DisponibilidadPresupuestal.Objeto = RegistroPresupuestal.Objeto;
                RegistroPresupuestal.DisponibilidadPresupuestal.FechaDdp = RegistroPresupuestal.FechaDdp;
                RegistroPresupuestal.DisponibilidadPresupuestal.NumeroDrp = RegistroPresupuestal.NumeroDrp;
                //RegistroPresupuestal.DisponibilidadPresupuestal.PlazoMeses = RegistroPresupuestal.PlazoMeses;
                //RegistroPresupuestal.DisponibilidadPresupuestal.PlazoDias = RegistroPresupuestal.PlazoDias;
                RegistroPresupuestal.DisponibilidadPresupuestal.FechaDrp = RegistroPresupuestal.FechaDrp;
                RegistroPresupuestal.DisponibilidadPresupuestal.FechaDrp = RegistroPresupuestal.FechaDrp;
                RegistroPresupuestal.DisponibilidadPresupuestal.NumeroOtroSi = novedadContractual != null ? novedadContractual.NumeroOtroSi : string.Empty;

                ListDisponibilidadPresupuestal.Remove(RegistroPresupuestal.DisponibilidadPresupuestal);
                ListDisponibilidadPresupuestal.Add(RegistroPresupuestal.DisponibilidadPresupuestal);
            }

            List<DisponibilidadPresupuestalGrilla> ListDisponibilidadPresupuestalGrilla = new List<DisponibilidadPresupuestalGrilla>();
            List<Dominio> ListEstados = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal || r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal).ToList();
            List<GestionFuenteFinanciacion> ListGestionFuenteFinanciacions = _context.GestionFuenteFinanciacion.ToList();
            List<Contrato> ListContratos = _context.Contrato.Where(r => r.Contratacion.ContratacionId > 0).ToList();

            foreach (var DisponibilidadPresupuestal in ListDisponibilidadPresupuestal)
            {
                string strEstadoRegistro = string.Empty;
                string strTipoSolicitud = string.Empty;
                if (!string.IsNullOrEmpty(DisponibilidadPresupuestal.EstadoSolicitudCodigo))
                {
                    strEstadoRegistro = ListEstados.Where(r => r.Codigo == DisponibilidadPresupuestal.EstadoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal).FirstOrDefault().Nombre;
                }

                if (!string.IsNullOrEmpty(DisponibilidadPresupuestal.TipoSolicitudCodigo))
                {
                    strTipoSolicitud = ListEstados.Where(r => r.Codigo == DisponibilidadPresupuestal.TipoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal).FirstOrDefault().Nombre;
                }

                DateTime? fechaContrato = null;
                string numeroContrato = string.Empty;
                if (DisponibilidadPresupuestal.ContratacionId != null)
                {
                    var contrato = ListContratos.Where(x => x.ContratacionId == DisponibilidadPresupuestal.ContratacionId).FirstOrDefault();
                    fechaContrato = contrato?.FechaFirmaContrato;
                    numeroContrato = DisponibilidadPresupuestal.NumeroContrato ?? (contrato != null ? contrato.NumeroContrato : string.Empty);
                }
                else
                {
                    numeroContrato = DisponibilidadPresupuestal.NumeroContrato ?? string.Empty;
                }
                bool blnEstado = false;

                //si es administrativo, esta completo, si es tradicional, se verifica contra fuentes gestionadas
                if (DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo)
                {
                    blnEstado = true;
                }
                else if (DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)
                {
                    blnEstado = true;
                }
                else
                {
                    List<int> ddpproyectosId = DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.Select(x => x.DisponibilidadPresupuestalProyectoId).ToList();
                    if (ListGestionFuenteFinanciacions.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyectoId != null && ddpproyectosId.Contains((int)x.DisponibilidadPresupuestalProyectoId)).Count() > 0)
                    {
                        blnEstado = true;
                    }
                }

                DisponibilidadPresupuestalGrilla disponibilidadPresupuestalGrilla = new DisponibilidadPresupuestalGrilla
                {

                    FechaSolicitud = DisponibilidadPresupuestal.FechaSolicitud.ToString("yyyy/MM/dd"),
                    EstadoRegistro = blnEstado,
                    TipoSolicitud = strTipoSolicitud,
                    TipoSolicitudEspecial = DisponibilidadPresupuestal.TipoSolicitudEspecialCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.TipoSolicitudEspecialCodigo, (int)EnumeratorTipoDominio.Tipo_DDP_Espacial) :
                    //si no viene el campo puede ser contratación
                    DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? ConstanStringTipoSolicitudContratacion.proyectoAdministrativo :
                    DisponibilidadPresupuestal.EsNovedad == null ? ConstanStringTipoSolicitudContratacion.contratacion : !Convert.ToBoolean(DisponibilidadPresupuestal.EsNovedad) ? ConstanStringTipoSolicitudContratacion.contratacion : ConstanStringTipoSolicitudContratacion.novedadContractual,
                    DisponibilidadPresupuestalId = DisponibilidadPresupuestal.DisponibilidadPresupuestalId,
                    NumeroSolicitud = DisponibilidadPresupuestal.NumeroSolicitud,
                    FechaFirmaContrato = fechaContrato == null ? string.Empty : Convert.ToDateTime(fechaContrato).ToString("dd/MM/yyyy"),
                    NumeroContrato = numeroContrato,
                    Estado = ListEstados.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal &&
                         x.Codigo == DisponibilidadPresupuestal.EstadoSolicitudCodigo).FirstOrDefault().Nombre,
                    NovedadContractualRegistroPresupuestalId = DisponibilidadPresupuestal.NovedadContractualRegistroPresupuestalId,
                    EsNovedad = DisponibilidadPresupuestal.EsNovedad,
                    NumeroOtroSi = DisponibilidadPresupuestal.NumeroOtroSi

                };

                ListDisponibilidadPresupuestalGrilla.Add(disponibilidadPresupuestalGrilla);
            }
            return ListDisponibilidadPresupuestalGrilla.OrderByDescending(r => r.DisponibilidadPresupuestalId).ToList();

        }

        /*autor: jflorez
            descripción: objeto para entregar a front los datos ordenados de disponibilidades
        impacto: CU 3.3.3*/
        public async Task<List<EstadosDisponibilidad>> GetListGenerarDisponibilidadPresupuestal(bool showRechazado)
        {
            List<EstadosDisponibilidad> estadosdisponibles = new List<EstadosDisponibilidad>();
            var estados = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal && x.Activo == true).ToList();


            foreach (var estado in estados)
            {
                estadosdisponibles.Add(
                                    new EstadosDisponibilidad
                                    {
                                        DominioId = estado.DominioId,
                                        NombreEstado = estado.Nombre,
                                        DisponibilidadPresupuestal = await this.GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(estado.Codigo, showRechazado)
                                    });
            }
            return estadosdisponibles;
        }

     
        public async Task<Respuesta> SetCancelRegistroPresupuestal(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion
            , string urlDestino, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Con_disponibilidad_cancelada;
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion;
                DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();
                pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();
                _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                _context.SaveChanges();
                //envio correo
                var usuarioJuridico = _context.UsuarioPerfil
                    .Where(x => (x.PerfilId == (int)EnumeratorPerfil.Juridica ||  x.PerfilId == (int)EnumeratorPerfil.Tecnica))
                    .Include(y => y.Usuario)
                    .ToList();

                bool blEnvioCorreo = true;
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DRPCancelado);
                string template = TemplateRecoveryPassword.Contenido;

                template = template.Replace("_LinkF_", urlDestino);
                template = template.Replace("[NUMERODISPONIBILIDAD]", DisponibilidadCancelar.NumeroSolicitud);
                foreach (var usuario in usuarioJuridico)
                {
                    blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuario.Usuario.Email, "DRP Cancelada", template, pSentender, pPassword, pMailServer, pMailPort);
                }
                this.EliminarGestion(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);
                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.CanceladoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.CanceladoCorrrectamente, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, "CANCELAR DISPONIBILIDAD PRESUPUESTAL")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        public async Task<Respuesta> CreateDDP(int pId, string pUsuarioModificacion, bool esNovedad, int RegistroPresupuestalId, string urlDestino, string pMailServer
                                                , int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal
                                                        .Include(x => x.Contratacion).
                                                            ThenInclude(x => x.ContratacionProyecto)
                                                                .ThenInclude(x => x.ContratacionProyectoAportante)
                                                                    .ThenInclude(x => x.CofinanciacionAportante)
                                                                        .ThenInclude(x => x.FuenteFinanciacion)
                                                         .FirstOrDefault(x => x.DisponibilidadPresupuestalId == pId);

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            int consecutivo = _context.DisponibilidadPresupuestal.Where(x => x.NumeroDdp != null).Count();
            /*busco usuario Juridico*/
            var usuarioJuridico = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Juridica).Include(y => y.Usuario).ToList();
            int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Con_disponibilidad_presupuestal;

            try
            {
                if (esNovedad)
                {
                    NovedadContractualRegistroPresupuestal novedadContractualRegistroPresupuestal = _context.NovedadContractualRegistroPresupuestal
                                                                                                                .Find(RegistroPresupuestalId);

                    List<GestionFuenteFinanciacion> listaGestion = _context.GestionFuenteFinanciacion
                                                                    .Where(x => x.NovedadContractualRegistroPresupuestalId == RegistroPresupuestalId &&
                                                                           x.Eliminado != true &&
                                                                           x.EsNovedad == true
                                                                           )
                                                                    .ToList();

                    novedadContractualRegistroPresupuestal.EstadoSolicitudCodigo = estado.ToString();


                    //TODO VALIDAR SI LOS PARAMETROS  
                    //List<DetailValidarDisponibilidadPresupuesal> ListDetailValidarDisponibilidadPresupuesal = await
                    //    _requestBudgetAvailabilityService.
                    //    GetDetailAvailabilityBudgetProyectNew(DisponibilidadCancelar.DisponibilidadPresupuestalId, false, 0);


                    foreach (GestionFuenteFinanciacion gestion in listaGestion)
                    {
                        int estadocod = (int)EnumeratorEstadoGestionFuenteFinanciacion.Apartado_en_DDP;
                        gestion.EstadoCodigo = estadocod.ToString();
                        gestion.FechaModificacion = DateTime.Now;
                        gestion.UsuarioModificacion = pUsuarioModificacion.ToUpper();

                        //llamar GetDetailAvailabilityBudgetProyect y buscar los saldos calculados 
                        //gestion.SaldoActualGenerado = ListDetailValidarDisponibilidadPresupuesal?.FirstOrDefault()?.Aportantes?.FirstOrDefault()?.FuentesFinanciacion?.FirstOrDefault()?.Saldo_actual_de_la_fuente;
                        //gestion.NuevoSaldoGenerado = ListDetailValidarDisponibilidadPresupuesal?.FirstOrDefault()?.Aportantes?.FirstOrDefault()?.FuentesFinanciacion?.FirstOrDefault()?.Nuevo_saldo_de_la_fuente_al_guardar;
                        //gestion.SaldoActualGenerado = ListDetailValidarDisponibilidadPresupuesal?.FirstOrDefault()?.Aportantes?.FirstOrDefault()?.FuentesFinanciacion?.FirstOrDefault()?.Saldo_actual_de_la_fuente_al_guardar;

                        _context.GestionFuenteFinanciacion.Update(gestion);
                    }
                }
                else
                {
                    DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                    DisponibilidadCancelar.UsuarioModificacion = pUsuarioModificacion.ToUpper();
                    DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();
                    string tipo = "";
                    if (DisponibilidadCancelar.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Tradicional)
                    {
                        tipo = "PI";
                    }
                    else if (DisponibilidadCancelar.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo)
                    {
                        tipo = "PA";
                    }
                    else
                    {
                        tipo = "ESP";
                    }
                    DisponibilidadCancelar.NumeroDdp = "DDP_" + tipo + "_" + consecutivo.ToString();

                    //
                    //guardar el tema de platas
                    //

                    Dictionary<int, List<decimal>> fuente = new Dictionary<int, List<decimal>>();
                    //var contratacionproyecto = DisponibilidadCancelar.Contratacion.ContratacionProyecto;
                    var gestionfuentes =
                        _context.GestionFuenteFinanciacion
                        .Include(f => f.FuenteFinanciacion).ThenInclude(a => a.Aportante)
                        .Where(x => !(bool)x.Eliminado
                        && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == DisponibilidadCancelar.DisponibilidadPresupuestalId);

                    //TODO VALIDAR SI LOS PARAMETROS  
                    List<DetailValidarDisponibilidadPresupuesal> ListDetailValidarDisponibilidadPresupuesal = await
                        _requestBudgetAvailabilityService.
                        GetDetailAvailabilityBudgetProyectNew(DisponibilidadCancelar.DisponibilidadPresupuestalId, false, 0);


                    foreach (var gestion in gestionfuentes)
                    {
                        int estadocod = (int)EnumeratorEstadoGestionFuenteFinanciacion.Apartado_en_DDP;
                        gestion.EstadoCodigo = estadocod.ToString();
                        gestion.FechaModificacion = DateTime.Now;
                        gestion.UsuarioModificacion = pUsuarioModificacion.ToUpper();


                        foreach (var DetailValidarDisponibilidadPresupuesal in ListDetailValidarDisponibilidadPresupuesal)
                        {
                            foreach (var Aportantes in DetailValidarDisponibilidadPresupuesal.Aportantes)
                            {
                                if (Aportantes.CofinanciacionAportanteId == gestion.FuenteFinanciacion.AportanteId)
                                {
                                    gestion.SaldoActualGenerado = Aportantes.FuentesFinanciacion.Where(r => r.FuenteFinanciacionID == gestion.FuenteFinanciacionId).Select(r => r.Saldo_actual_de_la_fuente).FirstOrDefault();
                                    gestion.ValorSolicitadoGenerado = Aportantes.FuentesFinanciacion.Where(r => r.FuenteFinanciacionID == gestion.FuenteFinanciacionId).Select(r => r.Valor_solicitado_de_la_fuente).FirstOrDefault();
                                    gestion.NuevoSaldoGenerado = Aportantes.FuentesFinanciacion.Where(r => r.FuenteFinanciacionID == gestion.FuenteFinanciacionId).Select(r => r.Nuevo_saldo_de_la_fuente).FirstOrDefault();

                                    _context.GestionFuenteFinanciacion.Update(gestion);
                                }
                            }
                        }


                    }
                }




                _context.SaveChanges();
                //envio correo a juridica
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DisponibilidadPresupuestalGenerada);
                string template = TemplateRecoveryPassword.Contenido;

                template = template.Replace("_LinkF_", urlDestino);
                template = template.Replace("[NUMERODISPONIBILIDAD]", DisponibilidadCancelar.NumeroSolicitud);

                List<string> usuarios = new List<string>();

                usuarioJuridico.ForEach(u =>
               {
                   usuarios.Add(u?.Usuario?.Email);
               });

                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreoMultipleDestinatario(usuarios, "DDP Generado", template, pSentender, pPassword, pMailServer, pMailPort);
                if (blEnvioCorreo)
                {
                    return new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantMessagesGenerateBudget.OperacionExitosa,
                        Data = DisponibilidadCancelar,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.OperacionExitosa, idAccion, pUsuarioModificacion, "GENERAR DDP DISPONIBILIDAD PRESUPUESTAL")
                    };
                }
                else
                {
                    return new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantMessagesGenerateBudget.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pUsuarioModificacion, "ERROR ENVIO MAIL GENERAR DDP DISPONIBILIDAD PRESUPUESTAL")
                    };
                }
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        public async Task<Respuesta> returnDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                _context.Set<DisponibilidadPresupuestal>()
                        .Where(d => d.DisponibilidadPresupuestalId == pDisponibilidadPresObservacion.DisponibilidadPresupuestalId)
                        .Update(d => new DisponibilidadPresupuestal
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion,
                            EstadoSolicitudCodigo = ((int)EnumeratorEstadoSolicitudPresupuestal.Devuelta_por_coordinacion_financiera).ToString()
                        });


                pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                pDisponibilidadPresObservacion.EstadoSolicitudCodigo = ((int)EnumeratorEstadoSolicitudPresupuestal.Devuelta_por_coordinacion_financiera).ToString();
                _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                this.EliminarGestion(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);

                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.DevueltoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.DevueltoCorrrectamente, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, "DEVOLVER DISPONIBILIDAD PRESUPUESTAL")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        private bool EliminarGestion(int DisponibilidadPresupuestalId)
        {
            DisponibilidadPresupuestal DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(DisponibilidadPresupuestalId);
            bool retorno = false;
            if (DisponibilidadCancelar.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)
            {
                _context.Set<GestionFuenteFinanciacion>()
                       .Where(g => g.DisponibilidadPresupuestalId == DisponibilidadPresupuestalId)
                       .Update(g => new GestionFuenteFinanciacion
                       {
                           Eliminado = true,
                           FechaModificacion = DateTime.Now,
                           UsuarioModificacion = DisponibilidadCancelar.UsuarioModificacion
                       });

                // var gestionFuentes = _context.GestionFuenteFinanciacion.Where(x => x.DisponibilidadPresupuestalId == DisponibilidadPresupuestalId).ToList();
                //foreach (var gestion in gestionFuentes)
                //{

                //    gestion.Eliminado = true;
                //    gestion.FechaModificacion = DateTime.Now;
                //    gestion.UsuarioModificacion = DisponibilidadCancelar.UsuarioModificacion;
                //    _context.GestionFuenteFinanciacion.Update(gestion);
                //}
            }
            else
            {


                _context.Set<GestionFuenteFinanciacion>()
                        .Where(g => g.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == DisponibilidadPresupuestalId)
                        .Update(g => new GestionFuenteFinanciacion
                        {
                            Eliminado = true,
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = DisponibilidadCancelar.UsuarioCreacion
                        });
                //var gestionFuentes = _context.GestionFuenteFinanciacion.Where(x => x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == DisponibilidadPresupuestalId).ToList(); 
                //foreach (var gestion in gestionFuentes)
                //{
                //    gestion.Eliminado = true;
                //    gestion.FechaModificacion = DateTime.Now;
                //    gestion.UsuarioModificacion = DisponibilidadCancelar.UsuarioCreacion;
                //    _context.GestionFuenteFinanciacion.Update(gestion);
                //}
            }
            return retorno;
        }

        public async Task<Respuesta> CreateEditarDisponibilidadPresupuestal(DisponibilidadPresupuestal DP)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = "";
            DisponibilidadPresupuestal disponibilidadPresupuestalAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(DP.DisponibilidadPresupuestalId.ToString()) || DP.DisponibilidadPresupuestalId == 0)
                {
                    //Concecutivo
                    var LastRegister = _context.DisponibilidadPresupuestal.OrderByDescending(x => x.DisponibilidadPresupuestalId).First().DisponibilidadPresupuestalId;


                    //Auditoria
                    strCrearEditar = "CREAR DISPONIBILIDAD PRESUPUESTAL";
                    DP.FechaCreacion = DateTime.Now;
                    DP.Eliminado = false;

                    //DP.NumeroDdp = ""; TODO: traer consecutivo del modulo de proyectos, DDP_PI_autoconsecutivo
                    DP.EstadoSolicitudCodigo = "4"; // Sin registrar

                    _context.DisponibilidadPresupuestal.Add(DP);
                    return respuesta = new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Data = DP,
                        Code = ConstantMessagesDisponibilidadPresupuesta.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesDisponibilidadPresupuesta.OperacionExitosa, idAccion, DP.UsuarioCreacion, strCrearEditar)
                    };

                }
                else
                {
                    strCrearEditar = "EDITAR DISPONIBILIDAD PRESUPUESTAL PROYECTO";
                    disponibilidadPresupuestalAntiguo = _context.DisponibilidadPresupuestal.Find(DP.DisponibilidadPresupuestalId);
                    //Auditoria
                    disponibilidadPresupuestalAntiguo.UsuarioModificacion = "jsorozco";
                    disponibilidadPresupuestalAntiguo.FechaModificacion = DateTime.Now;

                    //Registros
                    disponibilidadPresupuestalAntiguo.FechaSolicitud = DP.FechaSolicitud;
                    disponibilidadPresupuestalAntiguo.TipoSolicitudCodigo = DP.TipoSolicitudCodigo;
                    disponibilidadPresupuestalAntiguo.NumeroSolicitud = DP.NumeroSolicitud;
                    disponibilidadPresupuestalAntiguo.OpcionContratarCodigo = DP.OpcionContratarCodigo;
                    disponibilidadPresupuestalAntiguo.ValorSolicitud = DP.ValorSolicitud;
                    disponibilidadPresupuestalAntiguo.EstadoSolicitudCodigo = DP.EstadoSolicitudCodigo;
                    disponibilidadPresupuestalAntiguo.Objeto = DP.Objeto;
                    disponibilidadPresupuestalAntiguo.FechaDdp = DP.FechaDdp;
                    disponibilidadPresupuestalAntiguo.NumeroDdp = DP.NumeroDdp;
                    disponibilidadPresupuestalAntiguo.RutaDdp = DP.RutaDdp;
                    //disponibilidadPresupuestalAntiguo.Observacion = DP.Observacion;
                    disponibilidadPresupuestalAntiguo.Eliminado = false;

                    _context.DisponibilidadPresupuestal.Update(disponibilidadPresupuestalAntiguo);
                }

                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = disponibilidadPresupuestalAntiguo,
                    Code = ConstantMessagesDisponibilidadPresupuesta.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Cronograma, ConstantMessagesDisponibilidadPresupuesta.OperacionExitosa, idAccion, DP.UsuarioCreacion, strCrearEditar)
                };
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesDisponibilidadPresupuesta.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Cronograma, ConstantMessagesDisponibilidadPresupuesta.Error, idAccion, DP.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<List<GrillaDisponibilidadPresupuestal>> GetGridBudgetAvailability(int? DisponibilidadPresupuestalId)
        {

            List<DisponibilidadPresupuestal> ListDisponibilidadPresupuestal =
               DisponibilidadPresupuestalId != null ? await _context.DisponibilidadPresupuestal.Where(x => x.DisponibilidadPresupuestalId == DisponibilidadPresupuestalId).ToListAsync()
               : await _context.DisponibilidadPresupuestal.OrderByDescending(a => a.FechaSolicitud).ToListAsync();

            List<GrillaDisponibilidadPresupuestal> ListGrillaControlCronograma = new List<GrillaDisponibilidadPresupuestal>();

            foreach (var dp in ListDisponibilidadPresupuestal)
            {
                GrillaDisponibilidadPresupuestal DisponibilidadPresupuestalGrilla = new GrillaDisponibilidadPresupuestal
                {
                    DisponibilidadPresupuestalId = dp.DisponibilidadPresupuestalId,
                    FechaSolicitud = dp.FechaSolicitud,
                    TipoSolicitudCodigo = dp.TipoSolicitudCodigo,
                    TipoSolicitudText = dp.TipoSolicitudCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(dp.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Solicitud) : "",
                    NumeroSolicitud = dp.NumeroSolicitud,
                    OpcionPorContratarCodigo = dp.OpcionContratarCodigo,
                    OpcionPorContratarText = dp.OpcionContratarCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(dp.OpcionContratarCodigo, (int)EnumeratorTipoDominio.Opcion_Por_Contratar) : "",
                    ValorSolicitado = dp.ValorSolicitud,
                    EstadoSolicitudCodigo = dp.EstadoSolicitudCodigo,
                    EstadoSolicitudText = dp.EstadoSolicitudCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(dp.EstadoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Solicitud_Disponibilidad_Presupuestal) : "",


                };

                ListGrillaControlCronograma.Add(DisponibilidadPresupuestalGrilla);
            }

            return ListGrillaControlCronograma;
        }

        public async Task<byte[]> GetPDFDDP(int id, string pUsurioGenero, bool esNovedad, int pRegistroPresupuestalId, bool esValidar)
        {
            if (id == 0)
            {
                return Array.Empty<byte>();
            }
            DisponibilidadPresupuestal disponibilidad = await _context.DisponibilidadPresupuestal
                .Where(r => r.DisponibilidadPresupuestalId == id)
                .Include(r => r.Contratacion).FirstOrDefaultAsync();

            if (disponibilidad == null)
            {
                return Array.Empty<byte>();
            }

            NovedadContractualRegistroPresupuestal novedadContractualRegistro = new NovedadContractualRegistroPresupuestal();

            if (esNovedad)
            {
                novedadContractualRegistro = _context.NovedadContractualRegistroPresupuestal.Find(pRegistroPresupuestalId);
            }

            Plantilla plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Ficha_De_DDP).ToString())
                .Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();

            plantilla.Contenido = await ReemplazarDatosDDPAsync(plantilla.Contenido, disponibilidad, false, esNovedad, novedadContractualRegistro, esValidar);
            //return ConvertirPDF(plantilla);
            return Helpers.PDF.Convertir(plantilla, true);
        }

        private byte[] ConvertirPDF(Plantilla pPlantilla)
        {
            string strEncabezado = "";
            if (!string.IsNullOrEmpty(pPlantilla.Encabezado.Contenido))
            {
                strEncabezado = Helpers.Helpers.HtmlStringLimpio(pPlantilla.Encabezado.Contenido);
            }

            var globalSettings = new GlobalSettings
            {
                ImageQuality = 1080,
                PageOffset = 0,
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings
                {
                    Top = pPlantilla.MargenArriba,
                    Left = pPlantilla.MargenIzquierda,
                    Right = pPlantilla.MargenDerecha,
                    Bottom = pPlantilla.MargenAbajo
                },
                DocumentTitle = DateTime.Now.ToString(),
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = pPlantilla.Contenido,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "pdf-styles.css") },
                HeaderSettings = { FontName = "Roboto", FontSize = 8, Center = strEncabezado, Line = false, Spacing = 18 },
                FooterSettings = { FontName = "Ariel", FontSize = 10, Center = "[page]" },
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            return _converter.Convert(pdf);
        }
        ///Validar campos solicitues $$  nuevos 
        private async Task<string> ReemplazarDatosDDPAsync(string pStrContenido, DisponibilidadPresupuestal pDisponibilidad, bool drp, bool esNovedad, NovedadContractualRegistroPresupuestal pRegistro, bool esValidar)
        {
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolderDDP).ToList();
            /*variables que pueden diferir de uno u otro tipo*/
            //Anexos
            string Anexos = string.Empty;

            //Plantilla Compromisos Solicitud
            string PlantillaFichaDDPAnexos = _context.Plantilla
             .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Ficha_DDP_Anexos)
                .ToString()).FirstOrDefault()
             .Contenido;

            string opcionContratarCodigo = string.Empty;
            string proyecto = string.Empty;
            string pStrCabeceraProyectos = string.Empty;
            string limitacionEspecial = string.Empty;
            string tablaaportantes = string.Empty;
            decimal saldototal = 0;
            string tablafuentes = string.Empty;
            string tablauso = string.Empty;
            string tablaproyecto = string.Empty;
            if (drp)
            {
                int codtablafuentes = (int)ConstanCodigoPlantillas.DRP_TABLA_FUENTES;
                int codtablauso = (int)ConstanCodigoPlantillas.DRP_TABLA_USOS;
                var plantilla_fuentes = _context.Plantilla.Where(x => x.Codigo == codtablafuentes.ToString()).FirstOrDefault().Contenido;
                var plantilla_uso = _context.Plantilla.Where(x => x.Codigo == codtablauso.ToString()).FirstOrDefault().Contenido;
                //empiezo con fuentes
                var gestionfuentes = _context.GestionFuenteFinanciacion
                    .Where(x => x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == pDisponibilidad.DisponibilidadPresupuestalId).
                    Include(x => x.FuenteFinanciacion).
                        ThenInclude(x => x.Aportante).
                            ThenInclude(x => x.CofinanciacionDocumento).
                    Include(x => x.DisponibilidadPresupuestalProyecto).
                        ThenInclude(x => x.Proyecto).
                            ThenInclude(x => x.Sede)
                    .Include(x => x.DisponibilidadPresupuestalProyecto).
                        ThenInclude(x => x.Proyecto)
                            .ThenInclude(x => x.ProyectoAportante)
                    .Include(x => x.DisponibilidadPresupuestal)
                       .ThenInclude(x => x.Contratacion)
                          .ThenInclude(x => x.ContratacionProyecto)
                              .ThenInclude(x => x.ContratacionProyectoAportante)
                    .ToList();

                Contratacion contratacion = _context.Contratacion
                    .Where(r => r.ContratacionId == pDisponibilidad.ContratacionId)
                    .Include(r => r.ContratacionProyecto).ThenInclude(r => r.ContratacionProyectoAportante).FirstOrDefault();

                //usos                    
                var componenteAp = _context.ComponenteAportante
                    .Where(x => x.ContratacionProyectoAportante.ContratacionProyecto.ContratacionId == pDisponibilidad.ContratacionId)
                    .Include(x => x.ComponenteUso)
                    .Include(x => x.ContratacionProyectoAportante)
                    .ThenInclude(x => x.ContratacionProyecto)
                    .ThenInclude(x => x.Proyecto).
                    Include(x => x.ContratacionProyectoAportante).ThenInclude(x => x.CofinanciacionAportante).ToList();
                int registroNovedadId = 0;

                if (pRegistro != null)
                {
                    registroNovedadId = pRegistro.NovedadContractualRegistroPresupuestalId > 0 ? pRegistro.NovedadContractualRegistroPresupuestalId : 0;
                }
                List <DetailValidarDisponibilidadPresupuesal> ListdetailavailabilityBudget = await _requestBudgetAvailabilityService.GetDetailAvailabilityBudgetProyectNew(pDisponibilidad.DisponibilidadPresupuestalId, esNovedad, registroNovedadId);
                DetailValidarDisponibilidadPresupuesal detailavailabilityBudget = ListdetailavailabilityBudget.FirstOrDefault();

                if (detailavailabilityBudget != null)
                {
                    detailavailabilityBudget?.Proyectos?.ForEach(proyecto =>
                    {
                        proyecto.Aportantes.ForEach(aportante =>
                        {
                            aportante.FuentesFinanciacion.ForEach(fuente =>
                            {
                                var tr = plantilla_fuentes
                                    .Replace("[LLAVEMEN]", proyecto.LlaveMen)
                                    .Replace("[INSTITUCION]", proyecto.InstitucionEducativa)
                                    .Replace("[SEDE]", proyecto.Sede)
                                    .Replace("[APORTANTE]", aportante.Nombre)
                                    .Replace("[VALOR_APORTANTE]", "$ " + String.Format("{0:n0}", aportante.ValorAportanteAlProyecto != null ? aportante.ValorAportanteAlProyecto : 0))
                                    .Replace("[FUENTE]", fuente.Fuente)
                                    .Replace("[SALDO_FUENTE]", "$ " + String.Format("{0:n0}", fuente.Saldo_actual_de_la_fuente > 0 ? fuente.Saldo_actual_de_la_fuente : 0))
                                    .Replace("[VALOR_FUENTE]", "$ " + String.Format("{0:n0}", fuente.Valor_solicitado_de_la_fuente > 0 ? fuente.Valor_solicitado_de_la_fuente : 0))
                                    .Replace("[NUEVO_SALDO_FUENTE]", "$ " + String.Format("{0:n0}", fuente.Nuevo_saldo_de_la_fuente > 0 ? fuente.Nuevo_saldo_de_la_fuente : 0));
                                tablafuentes += tr;
                            });
                        });
                    });
                }
                /*foreach (var gestion in gestionfuentes)
                {
                    ProyectoAportante proyectoAportante = gestion.DisponibilidadPresupuestalProyecto.Proyecto.ProyectoAportante.Where(r => r.AportanteId == gestion.FuenteFinanciacion.Aportante.CofinanciacionAportanteId)?.FirstOrDefault();

                    var gestionAlGuardar = _context.GestionFuenteFinanciacion
                                    .Where(x => x.DisponibilidadPresupuestalProyectoId == gestion.DisponibilidadPresupuestalProyectoId &&
                                        x.FuenteFinanciacionId == gestion.FuenteFinanciacionId && x.Eliminado != true)
                                    .FirstOrDefault();

                    decimal saldo =
                                     proyectoAportante != null ? 
                                     contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria ?
                                     proyectoAportante.ValorInterventoria ?? 0 :
                                     proyectoAportante.ValorObra ?? 0 : 0;


                    //el saldo actual de la fuente son todas las solicitudes a la fuentes
                    //var consignadoemnfuente = _context.ControlRecurso.Where(x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId).Sum(x => x.ValorConsignacion);
                    var consignadoemnfuente = _context.FuenteFinanciacion
                        .Where(x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId)
                        .Sum(x => x.ValorFuente);

                    var saldofuente = _context.GestionFuenteFinanciacion.Where(
                        x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId &&
                        x.DisponibilidadPresupuestalProyectoId != gestion.DisponibilidadPresupuestalProyectoId)
                        .Sum(x => x.ValorSolicitado);

                    var gestionFuente = _context.GestionFuenteFinanciacion.Where(
                        x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId &&
                        x.DisponibilidadPresupuestalProyectoId == gestion.DisponibilidadPresupuestalProyectoId)
                        .FirstOrDefault();

                    string fuenteNombre = _context.Dominio.Where(x => x.Codigo == gestion.FuenteFinanciacion.FuenteRecursosCodigo
                            && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;

                    decimal? valorsolicitado =
                        _context.GestionFuenteFinanciacion
                        .Where(x => !(bool)x.Eliminado
                            && x.DisponibilidadPresupuestalProyectoId == gestion.DisponibilidadPresupuestalProyectoId
                            && x.FuenteFinanciacionId == gestion.FuenteFinanciacionId)
                        .Sum(x => x.ValorSolicitado);

                    decimal valorsolicitadoxotros = (decimal)_context.GestionFuenteFinanciacion
                        .Where(x => !(bool)x.Eliminado &&
                               x.DisponibilidadPresupuestalProyectoId != gestion.DisponibilidadPresupuestalProyectoId &&
                               x.FuenteFinanciacionId == gestion.FuenteFinanciacionId)
                        .Sum(x => x.ValorSolicitadoGenerado);



                    saldototal += (decimal)consignadoemnfuente - saldofuente;

                    string institucion = _context.InstitucionEducativaSede
                        .Where(x => x.InstitucionEducativaSedeId ==
                        gestion.DisponibilidadPresupuestalProyecto.Proyecto.Sede.PadreId)
                        .FirstOrDefault().Nombre;

                    decimal ValorSaldoFuente = (saldo - valorsolicitadoxotros - valorsolicitado) ?? 0;
                    decimal SaldoActualFuente = saldo - valorsolicitadoxotros;


                    var tr = plantilla_fuentes
                        .Replace("[LLAVEMEN]", gestion.DisponibilidadPresupuestalProyecto.Proyecto.LlaveMen)
                        .Replace("[INSTITUCION]", institucion)
                        .Replace("[SEDE]", gestion.DisponibilidadPresupuestalProyecto.Proyecto.Sede.Nombre)
                        .Replace("[APORTANTE]", this.getNombreAportante(gestion.FuenteFinanciacion.Aportante))
                        .Replace("[VALOR_APORTANTE]", "$ " + String.Format("{0:n0}", proyectoAportante != null ? contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria ? proyectoAportante.ValorInterventoria : proyectoAportante.ValorObra : 0))
                        .Replace("[FUENTE]", fuenteNombre)
                        .Replace("[SALDO_FUENTE]", "$ " + String.Format("{0:n0}", proyectoAportante != null ? contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria ? proyectoAportante.ValorInterventoria : proyectoAportante.ValorObra : 0))
                        .Replace("[VALOR_FUENTE]", "$ " + String.Format("{0:n0}", Math.Abs(SaldoActualFuente)).ToString())
                        .Replace("[NUEVO_SALDO_FUENTE]", "$ " + String.Format("{0:n0}", Math.Abs(ValorSaldoFuente)).ToString());
                    tablafuentes += tr;
                }*/



                foreach (var compAp in componenteAp)
                {
                    List<string> uso = new List<string>();
                    List<decimal> usovalor = new List<decimal>();

                    var dom = _context.Dominio.Where(x => x.Codigo == compAp.TipoComponenteCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Componentes).ToList();
                    var strFase = _context.Dominio.Where(r => r.Codigo == compAp.FaseCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Fases).FirstOrDefault();
                    string aportante = this.getNombreAportante(compAp.ContratacionProyectoAportante.CofinanciacionAportante);

                    foreach (var comp in compAp.ComponenteUso)
                    {
                        var usos = _context.Dominio.Where(x => x.Codigo == comp.TipoUsoCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Usos).ToList();
                        uso.Add(usos.Count() > 0 ? usos.FirstOrDefault().Nombre : string.Empty);
                        string llavemen = compAp.ContratacionProyectoAportante.ContratacionProyecto.Proyecto.LlaveMen;
                        var fuentestring = plantilla_uso.Replace("[LLAVEMEN2]", llavemen).
                            Replace("[FASE]", strFase.Nombre).
                            Replace("[APORTANTE]", aportante).
                            Replace("[COMPONENTE]", dom.FirstOrDefault().Nombre).
                            Replace("[USO]", usos.FirstOrDefault().Nombre).
                            Replace("[VALOR_USO]", "$ " + String.Format("{0:n0}", comp.ValorUso).ToString());
                        tablauso += fuentestring;
                    }
                }
            }
            else
            {
                //para fuentes
                int codtablafuentes = (int)ConstanCodigoPlantillas.DDP_TR_Fuente;
                int codcabeceraFuente = (int)ConstanCodigoPlantillas.DDP_Aportante_principal;
                var plantilla_fuentes = _context.Plantilla.Where(x => x.Codigo == codtablafuentes.ToString()).FirstOrDefault().Contenido;
                var plantilla_fuentecabecera = _context.Plantilla.Where(x => x.Codigo == codcabeceraFuente.ToString()).FirstOrDefault().Contenido;
                int codtablaproyecto = (int)ConstanCodigoPlantillas.DDP_TR_Proyecto;
                int codcabeceraproycto = (int)ConstanCodigoPlantillas.DDP_Cabecera_Proyecto;
                var plantilla_proycto = _context.Plantilla.Where(x => x.Codigo == codtablaproyecto.ToString()).FirstOrDefault().Contenido;

                List<GestionFuenteFinanciacion> gestionfuentes = new List<GestionFuenteFinanciacion>();


                gestionfuentes = _context.GestionFuenteFinanciacion
                 .Include(x => x.FuenteFinanciacion).
                    ThenInclude(x => x.Aportante).
                    ThenInclude(x => x.CofinanciacionDocumento).
                Include(x => x.DisponibilidadPresupuestalProyecto).
                    ThenInclude(x => x.Proyecto).
                        ThenInclude(x => x.Sede)
                .Where(x => !(bool)x.Eliminado && !(bool)x.EsNovedad
                    && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == pDisponibilidad.DisponibilidadPresupuestalId).

                ToList();

                if (esNovedad && esValidar)
                {
                    List<GestionFuenteFinanciacion> gestionfuentesTemp = _context.GestionFuenteFinanciacion
                    .Where(x => !(bool)x.Eliminado
                        && x.NovedadContractualRegistroPresupuestalId == pRegistro.NovedadContractualRegistroPresupuestalId).
                    Include(x => x.FuenteFinanciacion).
                        ThenInclude(x => x.Aportante).
                        ThenInclude(x => x.CofinanciacionDocumento).
                    Include(x => x.DisponibilidadPresupuestalProyecto).
                        ThenInclude(x => x.Proyecto).
                            ThenInclude(x => x.Sede).
                    ToList();

                    for (int i = 0; i < gestionfuentesTemp.Count(); i++)
                    {
                        int index = gestionfuentes.FindIndex(gf => gf.FuenteFinanciacionId == gestionfuentesTemp[i].FuenteFinanciacionId);
                        if (index >= 0)
                        {
                            gestionfuentes[index].ValorSolicitado += gestionfuentesTemp[i].ValorSolicitado;
                            gestionfuentes[index].SaldoActual += gestionfuentesTemp[i].SaldoActual;
                            gestionfuentes[index].NuevoSaldo += gestionfuentesTemp[i].NuevoSaldo;
                            gestionfuentes[index].SaldoActualGenerado += gestionfuentesTemp[i].SaldoActualGenerado;
                            gestionfuentes[index].ValorSolicitadoGenerado += gestionfuentesTemp[i].ValorSolicitadoGenerado;
                            gestionfuentes[index].NuevoSaldoGenerado += gestionfuentesTemp[i].NuevoSaldoGenerado;
                            gestionfuentesTemp.RemoveAt(i);
                            i--;
                        }
                    }
                    gestionfuentes.AddRange(gestionfuentesTemp);
                }

                //empiezo con fuentes

                decimal total = 0;
                foreach (var gestion in gestionfuentes)
                {
                    /*GestionFuenteFinanciacion gestionAlGuardar = new GestionFuenteFinanciacion();
                    if (esNovedad)
                    {
                        gestionAlGuardar = _context.GestionFuenteFinanciacion
                                    .Where(x => x.NovedadContractualRegistroPresupuestalId == pRegistro.NovedadContractualRegistroPresupuestalId &&
                                        x.FuenteFinanciacionId == gestion.FuenteFinanciacionId
                                        && x.Eliminado != true)
                                    .FirstOrDefault();
                    }
                    else
                    {
                        gestionAlGuardar = _context.GestionFuenteFinanciacion
                                    .Where(x => x.DisponibilidadPresupuestalProyectoId == gestion.DisponibilidadPresupuestalProyectoId &&
                                        x.FuenteFinanciacionId == gestion.FuenteFinanciacionId
                                        && x.Eliminado != true)
                                    .FirstOrDefault();
                    }*/


                    //el saldo actual de la fuente son todas las solicitudes a la fuentes
                    //var consignadoemnfuente = _context.ControlRecurso.Where(x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId).Sum(x => x.ValorConsignacion);
                    var consignadoemnfuente = _context.FuenteFinanciacion.Where(x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId).Sum(x => x.ValorFuente);
                    var saldofuente = _context.GestionFuenteFinanciacion.Where(
                        x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId &&
                        x.DisponibilidadPresupuestalProyectoId != gestion.DisponibilidadPresupuestalProyectoId).Sum(x => x.ValorSolicitado);
                    string fuenteNombre = _context.Dominio.Where(x => x.Codigo == gestion.FuenteFinanciacion.FuenteRecursosCodigo
                            && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;
                    //(decimal)font.FuenteFinanciacion.ValorFuente,
                    // Saldo_actual_de_la_fuente = (decimal)font.FuenteFinanciacion.ValorFuente - saldofuente
                    saldototal += (decimal)consignadoemnfuente - saldofuente;
                    string institucion = _context.InstitucionEducativaSede.Where(x => x.InstitucionEducativaSedeId == gestion.DisponibilidadPresupuestalProyecto.Proyecto.Sede.PadreId).FirstOrDefault().Nombre;
                    var tr = plantilla_proycto
                        .Replace("[DDP_LLAVE_MEN]", gestion.DisponibilidadPresupuestalProyecto.Proyecto.LlaveMen)
                        .Replace("[DDP_INSTITUCION_EDUCATIVA]", institucion)
                        .Replace("[DDP_SEDE]", gestion.DisponibilidadPresupuestalProyecto.Proyecto.Sede.Nombre)
                        .Replace("[DDP_APORTANTE]", this.getNombreAportante(gestion.FuenteFinanciacion.Aportante))
                        .Replace("[VALOR_APORTANTE]", "$ " + String.Format("{0:n0}", gestion.FuenteFinanciacion.Aportante.CofinanciacionDocumento.Sum(x => x.ValorDocumento)).ToString())
                        .Replace("[DDP_FUENTE]", fuenteNombre)
                        .Replace("[DDP_SALDO_ACTUAL_FUENTE]", "$ " + String.Format("{0:n0}", gestion.SaldoActual).ToString())
                        .Replace("[DDP_VALOR_SOLICITADO_FUENTE]", "$ " + String.Format("{0:n0}", gestion.ValorSolicitado).ToString())
                        .Replace("[DDP_NUEVO_SALDO_FUENTE]", "$ " + String.Format("{0:n0}", gestion.NuevoSaldo).ToString());

                    //.Replace("[DDP_SALDO_ACTUAL_FUENTE]", "$ " + String.Format("{0:n0}", saldototal).ToString())
                    //.Replace("[DDP_VALOR_SOLICITADO_FUENTE]", "$ " + String.Format("{0:n0}", gestion.ValorSolicitado).ToString())
                    //.Replace("[DDP_NUEVO_SALDO_FUENTE]", "$ " + String.Format("{0:n0}", (saldototal - gestion.ValorSolicitado)).ToString());
                    tablaproyecto += tr;

                    var tr2 = plantilla_fuentes
                        .Replace("[NOMBRE_APORTANTE]", this.getNombreAportante(gestion.FuenteFinanciacion.Aportante))
                        .Replace("[FUENTE_APORTANTE]", fuenteNombre)
                        .Replace("[VALOR_NUMERO]", "$ " + String.Format("{0:n0}", gestion.ValorSolicitado).ToString())
                        .Replace("[VALOR_LETRAS]",
                        CultureInfo.CurrentCulture.TextInfo
                                                        .ToTitleCase(Conversores
                                                        .NumeroALetras(gestion.ValorSolicitado)
                                                        .ToLower()));
                    tablafuentes += tr2;
                    total += gestion.ValorSolicitado;
                }

                if (pDisponibilidad.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Tradicional)
                {
                    opcionContratarCodigo = pDisponibilidad.OpcionContratarCodigo;
                    pStrCabeceraProyectos = _context.Plantilla.Where(x => x.Codigo == codcabeceraproycto.ToString()).FirstOrDefault().Contenido;
                    proyecto = tablaproyecto;
                    var limespecial = _context.Plantilla.Where(x => x.Codigo == ((int)ConstanCodigoPlantillas.DDP_limitacion).ToString());
                    limitacionEspecial = limespecial.Any() ? limespecial.FirstOrDefault().Contenido : string.Empty;
                    limitacionEspecial = limitacionEspecial.Replace(placeholders.Where(x => x.Codigo == ConstanCodigoVariablesPlaceHolders.DDP_LIMITACION_ESPECIAL).FirstOrDefault().Nombre
                        , limitacionEspecial);
                    tablaaportantes = plantilla_fuentecabecera
                        .Replace("[TABLAAPORTANTES]", tablafuentes)
                        .Replace("[TOTAL_DE_RECURSOS]", "$ " + String.Format("{0:n0}", total).ToString())
                        .Replace("[TOTAL_DE_RECURSOSLETRAS]",
                        CultureInfo.CurrentCulture.TextInfo
                                                        .ToTitleCase(Conversores
                                                        .NumeroALetras(total).ToLower()));
                }
                else
                if (pDisponibilidad.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo)
                {
                    opcionContratarCodigo = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal
                                    && r.Codigo == pDisponibilidad.TipoSolicitudCodigo).FirstOrDefault().Descripcion;
                    proyecto = string.Empty;
                    limitacionEspecial = string.Empty;
                    string aportanteTablaPrincipal = string.Empty;
                    string aportanteTr = string.Empty;
                    var aportantes = _context.Plantilla.Where(x => x.Codigo == ((int)ConstanCodigoPlantillas.DDP_Aportante_principal).ToString());
                    aportanteTablaPrincipal = aportantes.Any() ? aportantes.FirstOrDefault().Contenido : string.Empty;
                    var aportantestr = _context.Plantilla.Where(x => x.Codigo == ((int)ConstanCodigoPlantillas.DDP_Registros_Tabla_Aportante).ToString());
                    //deberia tener solo un proyecto por se administrativo
                    var proyectoadmin = _context.DisponibilidadPresupuestalProyecto.Where(x => x.DisponibilidadPresupuestalId == pDisponibilidad.DisponibilidadPresupuestalId).FirstOrDefault();
                    var proyectoadministrativo = _context.ProyectoAdministrativo.Where(x => x.ProyectoAdministrativoId == proyectoadmin.ProyectoAdministrativoId).
                                Include(x => x.ProyectoAdministrativoAportante).ThenInclude(x => x.AportanteFuenteFinanciacion).ThenInclude(x => x.FuenteFinanciacion);
                    foreach (var apo in proyectoadministrativo.FirstOrDefault().ProyectoAdministrativoAportante)
                    {
                        foreach (var font in apo.AportanteFuenteFinanciacion)
                        {
                            //el saldo actual de la fuente son todas las solicitudes a la fuentes
                            var saldofuente = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == font.FuenteFinanciacionId).Sum(x => x.ValorSolicitado);
                            string fuenteNombre = _context.Dominio.Where(x => x.Codigo == font.FuenteFinanciacion.FuenteRecursosCodigo
                                    && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;
                            //(decimal)font.FuenteFinanciacion.ValorFuente,
                            // Saldo_actual_de_la_fuente = (decimal)font.FuenteFinanciacion.ValorFuente - saldofuente
                            saldototal += (decimal)font.FuenteFinanciacion.ValorFuente - saldofuente;
                            //nombreAportante = getNombreAportante(_context.CofinanciacionAportante.Find(font.FuenteFinanciacion.AportanteId));
                            //valorAportate = font.ValorFuente;
                            string aportanteTrDato = aportantestr.Any() ? aportantestr.FirstOrDefault().Contenido : "";

                            aportanteTrDato = aportanteTrDato.Replace("[NOMBRE_APORTANTE]", getNombreAportante(_context.CofinanciacionAportante.Find(font.FuenteFinanciacion.AportanteId)));
                            aportanteTrDato = aportanteTrDato.Replace("[FUENTE_APORTANTE]", fuenteNombre);
                            aportanteTrDato = aportanteTrDato.Replace("[VALOR_NUMERO]", "$ " + String.Format("{0:n0}", saldofuente).ToString());
                            aportanteTrDato = aportanteTrDato.Replace("[VALOR_LETRAS]", CultureInfo.CurrentCulture.TextInfo
                                        .ToTitleCase(Conversores.NumeroALetras(saldofuente).ToLower()));
                            aportanteTr += aportanteTrDato;
                        }
                    }
                    tablaaportantes = aportanteTablaPrincipal.Replace("[TABLAAPORTANTES]", aportanteTr).
                        Replace("[TOTAL_DE_RECURSOS]", "2").
                        Replace("[TOTAL_DE_RECURSOSLETRAS]", "dos");
                }
                //ddp especial
                else
                {
                    //empiezo con fuentes
                    var gestionfuentesEspecial = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == pDisponibilidad.DisponibilidadPresupuestalId).
                        Include(x => x.FuenteFinanciacion).
                            ThenInclude(x => x.Aportante).
                            ThenInclude(x => x.CofinanciacionDocumento).
                        Include(x => x.DisponibilidadPresupuestalProyecto).
                            ThenInclude(x => x.Proyecto).
                                ThenInclude(x => x.Sede).
                        Include(x => x.DisponibilidadPresupuestal).
                          ThenInclude(x => x.DisponibilidadPresupuestalProyecto).
                            ThenInclude(x => x.Proyecto).
                                ThenInclude(x => x.Sede).
                        ToList();
                    decimal totales = 0;
                    foreach (var gestion in gestionfuentesEspecial)
                    {
                        //el saldo actual de la fuente son todas las solicitudes a la fuentes
                        //var consignadoemnfuente = _context.ControlRecurso.Where(x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId).Sum(x => x.ValorConsignacion);
                        var consignadoemnfuente = _context.FuenteFinanciacion.Where(x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId).Sum(x => x.ValorFuente);
                        var saldofuente = _context.GestionFuenteFinanciacion.Where(
                            x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId &&
                            x.DisponibilidadPresupuestalProyectoId != gestion.DisponibilidadPresupuestalProyectoId).Sum(x => x.ValorSolicitado);
                        string fuenteNombre = _context.Dominio.Where(x => x.Codigo == gestion.FuenteFinanciacion.FuenteRecursosCodigo
                                && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;
                        //(decimal)font.FuenteFinanciacion.ValorFuente,
                        // Saldo_actual_de_la_fuente = (decimal)font.FuenteFinanciacion.ValorFuente - saldofuente
                        saldototal += (decimal)consignadoemnfuente - saldofuente;

                        /*GestionFuenteFinanciacion gestionAlGuardar = new GestionFuenteFinanciacion();
                        if (esNovedad)
                        {
                            gestionAlGuardar = _context.GestionFuenteFinanciacion
                                        .Where(x => x.NovedadContractualRegistroPresupuestalId == pRegistro.NovedadContractualRegistroPresupuestalId &&
                                            x.FuenteFinanciacionId == gestion.FuenteFinanciacionId
                                            && x.Eliminado != true)
                                        .FirstOrDefault();
                        }
                        else
                        {
                            gestionAlGuardar = _context.GestionFuenteFinanciacion
                                        .Where(x => x.DisponibilidadPresupuestalProyectoId == gestion.DisponibilidadPresupuestalProyectoId &&
                                            x.FuenteFinanciacionId == gestion.FuenteFinanciacionId
                                            && x.Eliminado != true)
                                        .FirstOrDefault();
                        }*/
                        Proyecto proyectoTemp = null;
                        if (gestion.DisponibilidadPresupuestal != null)
                        {
                            if (gestion.DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.Count() > 0)
                                proyectoTemp = gestion.DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.FirstOrDefault().Proyecto;

                        }
                        else if (gestion.DisponibilidadPresupuestalProyecto != null)
                        {
                            if (gestion.DisponibilidadPresupuestalProyecto.Proyecto != null)
                                proyectoTemp = gestion.DisponibilidadPresupuestalProyecto.Proyecto;
                        }

                        if (proyectoTemp != null)
                        {
                            string institucion = _context.InstitucionEducativaSede.Where(x => x.InstitucionEducativaSedeId == proyectoTemp.Sede.PadreId).FirstOrDefault().Nombre;
                            var tr = plantilla_proycto.Replace("[DDP_LLAVE_MEN]", proyectoTemp.LlaveMen)
                                .Replace("[DDP_INSTITUCION_EDUCATIVA]", institucion)
                                .Replace("[DDP_SEDE]", proyectoTemp.Sede.Nombre)
                                .Replace("[DDP_APORTANTE]", this.getNombreAportante(gestion.FuenteFinanciacion.Aportante))
                                .Replace("[VALOR_APORTANTE]", "$ " + String.Format("{0:n0}", gestion.FuenteFinanciacion.Aportante.CofinanciacionDocumento.Sum(x => x.ValorDocumento)).ToString())
                                .Replace("[DDP_FUENTE]", fuenteNombre)

                                .Replace("[DDP_SALDO_ACTUAL_FUENTE]", "$ " + String.Format("{0:n0}", gestion.SaldoActual).ToString())
                                .Replace("[DDP_VALOR_SOLICITADO_FUENTE]", "$ " + String.Format("{0:n0}", gestion.ValorSolicitado).ToString())
                                .Replace("[DDP_NUEVO_SALDO_FUENTE]", "$ " + String.Format("{0:n0}", (gestion.NuevoSaldo)).ToString());

                            //.Replace("[DDP_SALDO_ACTUAL_FUENTE]", "$ " + String.Format("{0:n0}", saldototal).ToString())
                            //.Replace("[DDP_VALOR_SOLICITADO_FUENTE]", "$ " + String.Format("{0:n0}", gestion.ValorSolicitado).ToString())
                            //.Replace("[DDP_NUEVO_SALDO_FUENTE]", "$ " + String.Format("{0:n0}", (saldototal - gestion.ValorSolicitado)).ToString());
                            tablaproyecto += tr;
                        }

                        var tr2 = plantilla_fuentes
                            .Replace("[NOMBRE_APORTANTE]", this.getNombreAportante(gestion.FuenteFinanciacion.Aportante))
                            .Replace("[FUENTE_APORTANTE]", fuenteNombre)
                            .Replace("[VALOR_NUMERO]", "$ " + String.Format("{0:n0}", gestion.ValorSolicitado).ToString())
                            .Replace("[VALOR_LETRAS]", CultureInfo.CurrentCulture.TextInfo
                                            .ToTitleCase(Helpers.Conversores
                                            .NumeroALetras(gestion.ValorSolicitado).ToLower()));
                        tablafuentes += tr2;
                        totales += gestion.ValorSolicitado;
                    }
                    proyecto = string.Empty;
                    proyecto = tablaproyecto;
                    if (!string.IsNullOrEmpty(proyecto))
                    {
                        pStrCabeceraProyectos = _context.Plantilla.Where(x => x.Codigo == codcabeceraproycto.ToString()).FirstOrDefault().Contenido;
                    }
                    limitacionEspecial = string.Empty;
                    tablaaportantes = plantilla_fuentecabecera.Replace("[TABLAAPORTANTES]", tablafuentes).
                       Replace("[TOTAL_DE_RECURSOS]", "$ " + String.Format("{0:n0}", totales).ToString()).
                       Replace("[TOTAL_DE_RECURSOSLETRAS]", CultureInfo.CurrentCulture.TextInfo
                                       .ToTitleCase(Helpers.Conversores
                                       .NumeroALetras(totales).ToLower()));
                }
            }
            DateTime? fechaComitetecnico = DateTime.Now;

            string numeroComiteTecnico = string.Empty;
            //contratos
            Contrato contrato = new Contrato();
            if (pDisponibilidad.ContratacionId > 0)
            {
                contrato = _context.Contrato.Where(x => x.ContratacionId == pDisponibilidad.ContratacionId).FirstOrDefault();
                //LCT - ajuste data plantilla DDP
                var sesionComiteSolicitud = _context.SesionComiteSolicitud.Where(x => x.SolicitudId == pDisponibilidad.ContratacionId && x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion).
                                                                          Include(x => x.ComiteTecnico).ToList();
                if (sesionComiteSolicitud.Count() > 0)
                {
                    numeroComiteTecnico = sesionComiteSolicitud.FirstOrDefault().ComiteTecnico.NumeroComite;
                    fechaComitetecnico = Convert.ToDateTime(sesionComiteSolicitud.FirstOrDefault().ComiteTecnico.FechaOrdenDia);
                }
            }

            //SI ES NOVEDAD
            var contratotd = string.Empty;
            var novedadtd = string.Empty;
            var objetotd = _context.Plantilla.Where(x => x.Codigo == ((int)ConstanCodigoPlantillas.DDP_objeto).ToString()).FirstOrDefault().Contenido;
            objetotd = objetotd.Replace("[OBJETOINNER]", Helpers.Helpers.HtmlStringLimpio(pDisponibilidad.Objeto));


            //LCT
            var tiporubro = pDisponibilidad.TipoSolicitudEspecialCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(pDisponibilidad.TipoSolicitudEspecialCodigo, (int)EnumeratorTipoDominio.Tipo_DDP_Espacial) : pDisponibilidad.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? ConstanStringTipoSolicitudContratacion.proyectoAdministrativo : ConstanStringTipoSolicitudContratacion.contratacion;


            if (pDisponibilidad.EsNovedadContractual != null && Convert.ToBoolean(pDisponibilidad.EsNovedadContractual) && !drp)
            {
                contratotd = _context.Plantilla.Where(x => x.Codigo == ((int)ConstanCodigoPlantillas.DDP_contrato).ToString()).FirstOrDefault().Contenido;
                contratotd = contratotd.Replace("[NUMERO_CONTRATO]", contrato.NumeroContrato);
                novedadtd = _context.Plantilla.Where(x => x.Codigo == ((int)ConstanCodigoPlantillas.DDP_novedad).ToString()).FirstOrDefault().Contenido;
                objetotd = objetotd.Replace("colspan=10", "colspan=7");
            }
            if (esNovedad && !esValidar)
            {
                Anexos = await ReemplazarDatosDDPAsyncDuplicate(PlantillaFichaDDPAnexos, pDisponibilidad, pRegistro);
                pStrContenido = pStrContenido.Replace("[REGISTROS_ANEXOS]", Anexos);
            }
            else
            {
                pStrContenido = pStrContenido.Replace("[REGISTROS_ANEXOS]", string.Empty);
            }


            foreach (var place in placeholders)
            {
                switch (place.Codigo)
                {
                    case ConstanCodigoVariablesPlaceHolders.DDP_FECHA:
                        pStrContenido = pStrContenido
                            .Replace(place.Nombre, fechaComitetecnico != null ? ((DateTime)fechaComitetecnico).ToString("dd/MM/yyyy") : "");
                        break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NUMERO_SOLICITUD:
                        pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.NumeroSolicitud); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NO:
                        pStrContenido =
                            pStrContenido.Replace(place.Nombre, pDisponibilidad.NumeroDdp); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_RUBRO_POR_FINANCIAR:
                        pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial ? tiporubro : _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal
                                         && r.Codigo == pDisponibilidad.TipoSolicitudCodigo).FirstOrDefault().Descripcion); break;

                    case ConstanCodigoVariablesPlaceHolders.DDP_TIPO_SOLICITUD:
                        pStrContenido =
                            pStrContenido.Replace(place.Nombre, pDisponibilidad.TipoSolicitudCodigo != null ? _context.Dominio.Where(x => x.Codigo == pDisponibilidad.TipoSolicitudCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal).FirstOrDefault().Nombre :
                    //si no viene el campo puede ser contratación
                    pDisponibilidad.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? ConstanStringTipoSolicitudContratacion.proyectoAdministrativo :
                    pDisponibilidad.EsNovedadContractual == null ? ConstanStringTipoSolicitudContratacion.contratacion : !Convert.ToBoolean(pDisponibilidad.EsNovedadContractual) ? ConstanStringTipoSolicitudContratacion.contratacion : ConstanStringTipoSolicitudContratacion.novedadContractual); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_OPCION_CONTRATAR:
                        pStrContenido =
                            pStrContenido.Replace(place.Nombre, opcionContratarCodigo); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TABLA_LIMITACION_ESPECIAL:
                        pStrContenido =
                            pStrContenido.Replace(place.Nombre, limitacionEspecial); break;

                    case ConstanCodigoVariablesPlaceHolders.DDP_FECHA_COMITE_TECNICO: pStrContenido = pStrContenido.Replace(place.Nombre, fechaComitetecnico != null && !String.IsNullOrEmpty(numeroComiteTecnico) ? ((DateTime)fechaComitetecnico).ToString("dd/MM/yyyy") : ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NUMERO_COMITE: pStrContenido = pStrContenido.Replace(place.Nombre, numeroComiteTecnico); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_OBJETO: pStrContenido = pStrContenido.Replace(place.Nombre, Helpers.Helpers.HtmlStringLimpio(pDisponibilidad.Objeto)); break;
                    //case ConstanCodigoVariablesPlaceHolders.DDP_OBJETO: pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.Objeto); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TABLAAPORTANTES: pStrContenido = pStrContenido.Replace(place.Nombre, tablaaportantes); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TOTAL_DE_RECURSOS: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TOTAL_DE_RECURSOSLETRAS: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TABLA_PROYECTOS: pStrContenido = pStrContenido.Replace(place.Nombre, proyecto); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_LIMITACION_ESPECIAL: pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.LimitacionEspecial); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NOMBRE_APORTANTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_FUENTE_APORTANTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_VALOR_NUMERO: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_VALOR_LETRAS: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.CABECERAPROYECTOS:
                        pStrContenido = pStrContenido.Replace(place.Nombre, pStrCabeceraProyectos); break;


                    case ConstanCodigoVariablesPlaceHolders.DDP_LLAVE_MEN: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_INSTITUCION_EDUCATIVA: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_SEDE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_FUENTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_SALDO_ACTUAL_FUENTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_VALOR_SOLICITADO_FUENTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NUEVO_SALDO_FUENTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;

                    //drp
                    case ConstanCodigoVariablesPlaceHolders.NUMEROCONTRATO: pStrContenido = pStrContenido.Replace(place.Nombre, contrato == null ? "" : contrato.NumeroContrato); break;
                    case ConstanCodigoVariablesPlaceHolders.DRP_NO: pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.NumeroDrp); break;
                    case ConstanCodigoVariablesPlaceHolders.FECHACONTRATO: pStrContenido = pStrContenido.Replace(place.Nombre, contrato == null ? "" : contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : ""); break;
                    case ConstanCodigoVariablesPlaceHolders.TABLAFUENTES:
                        pStrContenido = pStrContenido.Replace(place.Nombre, tablafuentes); break;
                    case ConstanCodigoVariablesPlaceHolders.TABLAUSOS:
                        pStrContenido = pStrContenido.Replace(place.Nombre, tablauso); break;
                    case ConstanCodigoVariablesPlaceHolders.CONTRATOTD:
                        pStrContenido = pStrContenido.Replace(place.Nombre, contratotd); break;
                    case ConstanCodigoVariablesPlaceHolders.TIPONOVEDADTD:
                        pStrContenido = pStrContenido.Replace(place.Nombre, novedadtd); break;
                }
            }

            return Helpers.Helpers.HtmlEntities(pStrContenido);
        }
        private async Task<string> ReemplazarDatosDDPAsyncDuplicate(string pStrContenido, DisponibilidadPresupuestal pDisponibilidad, NovedadContractualRegistroPresupuestal pRegistro)
        {
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolderDDP).ToList();
            /*variables que pueden diferir de uno u otro tipo*/
            string opcionContratarCodigo = string.Empty;
            string proyecto = string.Empty;
            string pStrCabeceraProyectos = string.Empty;
            string limitacionEspecial = string.Empty;
            string tablaaportantes = string.Empty;
            decimal saldototal = 0;
            string tablafuentes = string.Empty;
            string tablauso = string.Empty;
            string tablaproyecto = string.Empty;
                
            //para fuentes
            int codtablafuentes = (int)ConstanCodigoPlantillas.DDP_TR_Fuente;
            int codcabeceraFuente = (int)ConstanCodigoPlantillas.DDP_Aportante_principal;
            var plantilla_fuentes = _context.Plantilla.Where(x => x.Codigo == codtablafuentes.ToString()).FirstOrDefault().Contenido;
            var plantilla_fuentecabecera = _context.Plantilla.Where(x => x.Codigo == codcabeceraFuente.ToString()).FirstOrDefault().Contenido;
            int codtablaproyecto = (int)ConstanCodigoPlantillas.DDP_TR_Proyecto;
            int codcabeceraproycto = (int)ConstanCodigoPlantillas.DDP_Cabecera_Proyecto;
            var plantilla_proycto = _context.Plantilla.Where(x => x.Codigo == codtablaproyecto.ToString()).FirstOrDefault().Contenido;

            List<GestionFuenteFinanciacion> gestionfuentes = new List<GestionFuenteFinanciacion>();


            gestionfuentes = _context.GestionFuenteFinanciacion
                .Where(x => !(bool)x.Eliminado
                    && x.NovedadContractualRegistroPresupuestalId == pRegistro.NovedadContractualRegistroPresupuestalId).
                Include(x => x.FuenteFinanciacion).
                    ThenInclude(x => x.Aportante).
                    ThenInclude(x => x.CofinanciacionDocumento).
                Include(x => x.DisponibilidadPresupuestalProyecto).
                    ThenInclude(x => x.Proyecto).
                        ThenInclude(x => x.Sede).
                ToList();

                //empiezo con fuentes

                decimal total = 0;
                foreach (var gestion in gestionfuentes)
                {   
                    
                    var consignadoemnfuente = _context.FuenteFinanciacion.Where(x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId).Sum(x => x.ValorFuente);
                    var saldofuente = _context.GestionFuenteFinanciacion.Where(
                        x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId &&
                        x.DisponibilidadPresupuestalProyectoId != gestion.DisponibilidadPresupuestalProyectoId).Sum(x => x.ValorSolicitado);
                    string fuenteNombre = _context.Dominio.Where(x => x.Codigo == gestion.FuenteFinanciacion.FuenteRecursosCodigo
                            && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;
                    //(decimal)font.FuenteFinanciacion.ValorFuente,
                    // Saldo_actual_de_la_fuente = (decimal)font.FuenteFinanciacion.ValorFuente - saldofuente
                    saldototal += (decimal)consignadoemnfuente - saldofuente;
                    string institucion = _context.InstitucionEducativaSede.Where(x => x.InstitucionEducativaSedeId == gestion.DisponibilidadPresupuestalProyecto.Proyecto.Sede.PadreId).FirstOrDefault().Nombre;
                    var tr = plantilla_proycto
                        .Replace("[DDP_LLAVE_MEN]", gestion.DisponibilidadPresupuestalProyecto.Proyecto.LlaveMen)
                        .Replace("[DDP_INSTITUCION_EDUCATIVA]", institucion)
                        .Replace("[DDP_SEDE]", gestion.DisponibilidadPresupuestalProyecto.Proyecto.Sede.Nombre)
                        .Replace("[DDP_APORTANTE]", this.getNombreAportante(gestion.FuenteFinanciacion.Aportante))
                        .Replace("[VALOR_APORTANTE]", "$ " + String.Format("{0:n0}", gestion.FuenteFinanciacion.Aportante.CofinanciacionDocumento.Sum(x => x.ValorDocumento)).ToString())
                        .Replace("[DDP_FUENTE]", fuenteNombre)
                        .Replace("[DDP_SALDO_ACTUAL_FUENTE]", "$ " + String.Format("{0:n0}", gestion.SaldoActual).ToString())
                        .Replace("[DDP_VALOR_SOLICITADO_FUENTE]", "$ " + String.Format("{0:n0}", gestion.ValorSolicitado).ToString())
                        .Replace("[DDP_NUEVO_SALDO_FUENTE]", "$ " + String.Format("{0:n0}", gestion.NuevoSaldo).ToString());
                        
                        tablaproyecto += tr;

                        var tr2 = plantilla_fuentes
                        .Replace("[NOMBRE_APORTANTE]", this.getNombreAportante(gestion.FuenteFinanciacion.Aportante))
                        .Replace("[FUENTE_APORTANTE]", fuenteNombre)
                        .Replace("[VALOR_NUMERO]", "$ " + String.Format("{0:n0}", gestion.ValorSolicitado).ToString())
                        .Replace("[VALOR_LETRAS]",
                        CultureInfo.CurrentCulture.TextInfo
                                                        .ToTitleCase(Conversores
                                                        .NumeroALetras(gestion.ValorSolicitado)
                                                        .ToLower()));
                        tablafuentes += tr2;
                        total += gestion.ValorSolicitado;
                }

                if (pDisponibilidad.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Tradicional)
                {
                    opcionContratarCodigo = pDisponibilidad.OpcionContratarCodigo;
                    pStrCabeceraProyectos = _context.Plantilla.Where(x => x.Codigo == codcabeceraproycto.ToString()).FirstOrDefault().Contenido;
                    proyecto = tablaproyecto;
                    var limespecial = _context.Plantilla.Where(x => x.Codigo == ((int)ConstanCodigoPlantillas.DDP_limitacion).ToString());
                    limitacionEspecial = limespecial.Any() ? limespecial.FirstOrDefault().Contenido : string.Empty;
                    limitacionEspecial = limitacionEspecial.Replace(placeholders.Where(x => x.Codigo == ConstanCodigoVariablesPlaceHolders.DDP_LIMITACION_ESPECIAL).FirstOrDefault().Nombre
                        , limitacionEspecial);
                    tablaaportantes = plantilla_fuentecabecera
                        .Replace("[TABLAAPORTANTES]", tablafuentes)
                        .Replace("[TOTAL_DE_RECURSOS]", "$ " + String.Format("{0:n0}", total).ToString())
                        .Replace("[TOTAL_DE_RECURSOSLETRAS]",
                        CultureInfo.CurrentCulture.TextInfo
                                                        .ToTitleCase(Conversores
                                                        .NumeroALetras(total).ToLower()));
                }
                else
                if (pDisponibilidad.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo)
                {
                    opcionContratarCodigo = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal
                                    && r.Codigo == pDisponibilidad.TipoSolicitudCodigo).FirstOrDefault().Descripcion;
                    proyecto = string.Empty;
                    limitacionEspecial = string.Empty;
                    string aportanteTablaPrincipal = string.Empty;
                    string aportanteTr = string.Empty;
                    var aportantes = _context.Plantilla.Where(x => x.Codigo == ((int)ConstanCodigoPlantillas.DDP_Aportante_principal).ToString());
                    aportanteTablaPrincipal = aportantes.Any() ? aportantes.FirstOrDefault().Contenido : string.Empty;
                    var aportantestr = _context.Plantilla.Where(x => x.Codigo == ((int)ConstanCodigoPlantillas.DDP_Registros_Tabla_Aportante).ToString());
                    //deberia tener solo un proyecto por se administrativo
                    var proyectoadmin = _context.DisponibilidadPresupuestalProyecto.Where(x => x.DisponibilidadPresupuestalId == pDisponibilidad.DisponibilidadPresupuestalId).FirstOrDefault();
                    var proyectoadministrativo = _context.ProyectoAdministrativo.Where(x => x.ProyectoAdministrativoId == proyectoadmin.ProyectoAdministrativoId).
                                Include(x => x.ProyectoAdministrativoAportante).ThenInclude(x => x.AportanteFuenteFinanciacion).ThenInclude(x => x.FuenteFinanciacion);
                    foreach (var apo in proyectoadministrativo.FirstOrDefault().ProyectoAdministrativoAportante)
                    {
                        foreach (var font in apo.AportanteFuenteFinanciacion)
                        {
                            //el saldo actual de la fuente son todas las solicitudes a la fuentes
                            var saldofuente = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == font.FuenteFinanciacionId).Sum(x => x.ValorSolicitado);
                            string fuenteNombre = _context.Dominio.Where(x => x.Codigo == font.FuenteFinanciacion.FuenteRecursosCodigo
                                    && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;
                            //(decimal)font.FuenteFinanciacion.ValorFuente,
                            // Saldo_actual_de_la_fuente = (decimal)font.FuenteFinanciacion.ValorFuente - saldofuente
                            saldototal += (decimal)font.FuenteFinanciacion.ValorFuente - saldofuente;
                            //nombreAportante = getNombreAportante(_context.CofinanciacionAportante.Find(font.FuenteFinanciacion.AportanteId));
                            //valorAportate = font.ValorFuente;
                            string aportanteTrDato = aportantestr.Any() ? aportantestr.FirstOrDefault().Contenido : "";

                            aportanteTrDato = aportanteTrDato.Replace("[NOMBRE_APORTANTE]", getNombreAportante(_context.CofinanciacionAportante.Find(font.FuenteFinanciacion.AportanteId)));
                            aportanteTrDato = aportanteTrDato.Replace("[FUENTE_APORTANTE]", fuenteNombre);
                            aportanteTrDato = aportanteTrDato.Replace("[VALOR_NUMERO]", "$ " + String.Format("{0:n0}", saldofuente).ToString());
                            aportanteTrDato = aportanteTrDato.Replace("[VALOR_LETRAS]", CultureInfo.CurrentCulture.TextInfo
                                        .ToTitleCase(Conversores.NumeroALetras(saldofuente).ToLower()));
                            aportanteTr += aportanteTrDato;
                        }
                    }
                    tablaaportantes = aportanteTablaPrincipal.Replace("[TABLAAPORTANTES]", aportanteTr).
                        Replace("[TOTAL_DE_RECURSOS]", "2").
                        Replace("[TOTAL_DE_RECURSOSLETRAS]", "dos");
                }
                //ddp especial
                else
                {
                    //empiezo con fuentes
                    var gestionfuentesEspecial = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == pDisponibilidad.DisponibilidadPresupuestalId).
                        Include(x => x.FuenteFinanciacion).
                            ThenInclude(x => x.Aportante).
                            ThenInclude(x => x.CofinanciacionDocumento).
                        Include(x => x.DisponibilidadPresupuestalProyecto).
                            ThenInclude(x => x.Proyecto).
                                ThenInclude(x => x.Sede).
                        Include(x => x.DisponibilidadPresupuestal).
                          ThenInclude(x => x.DisponibilidadPresupuestalProyecto).
                            ThenInclude(x => x.Proyecto).
                                ThenInclude(x => x.Sede).
                        ToList();
                    decimal totales = 0;
                    foreach (var gestion in gestionfuentesEspecial)
                    {
                        //el saldo actual de la fuente son todas las solicitudes a la fuentes
                        //var consignadoemnfuente = _context.ControlRecurso.Where(x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId).Sum(x => x.ValorConsignacion);
                        var consignadoemnfuente = _context.FuenteFinanciacion.Where(x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId).Sum(x => x.ValorFuente);
                        var saldofuente = _context.GestionFuenteFinanciacion.Where(
                            x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId &&
                            x.DisponibilidadPresupuestalProyectoId != gestion.DisponibilidadPresupuestalProyectoId).Sum(x => x.ValorSolicitado);
                        string fuenteNombre = _context.Dominio.Where(x => x.Codigo == gestion.FuenteFinanciacion.FuenteRecursosCodigo
                                && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;
                        //(decimal)font.FuenteFinanciacion.ValorFuente,
                        // Saldo_actual_de_la_fuente = (decimal)font.FuenteFinanciacion.ValorFuente - saldofuente
                        saldototal += (decimal)consignadoemnfuente - saldofuente;

                        /*GestionFuenteFinanciacion gestionAlGuardar = new GestionFuenteFinanciacion();
                        if (esNovedad)
                        {
                            gestionAlGuardar = _context.GestionFuenteFinanciacion
                                        .Where(x => x.NovedadContractualRegistroPresupuestalId == pRegistro.NovedadContractualRegistroPresupuestalId &&
                                            x.FuenteFinanciacionId == gestion.FuenteFinanciacionId
                                            && x.Eliminado != true)
                                        .FirstOrDefault();
                        }
                        else
                        {
                            gestionAlGuardar = _context.GestionFuenteFinanciacion
                                        .Where(x => x.DisponibilidadPresupuestalProyectoId == gestion.DisponibilidadPresupuestalProyectoId &&
                                            x.FuenteFinanciacionId == gestion.FuenteFinanciacionId
                                            && x.Eliminado != true)
                                        .FirstOrDefault();
                        }*/
                        Proyecto proyectoTemp = null;
                        if (gestion.DisponibilidadPresupuestal != null)
                        {
                            if (gestion.DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.Count() > 0)
                                proyectoTemp = gestion.DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.FirstOrDefault().Proyecto;

                        }
                        else if (gestion.DisponibilidadPresupuestalProyecto != null)
                        {
                            if (gestion.DisponibilidadPresupuestalProyecto.Proyecto != null)
                                proyectoTemp = gestion.DisponibilidadPresupuestalProyecto.Proyecto;
                        }

                        if (proyectoTemp != null)
                        {
                            string institucion = _context.InstitucionEducativaSede.Where(x => x.InstitucionEducativaSedeId == proyectoTemp.Sede.PadreId).FirstOrDefault().Nombre;
                            var tr = plantilla_proycto.Replace("[DDP_LLAVE_MEN]", proyectoTemp.LlaveMen)
                                .Replace("[DDP_INSTITUCION_EDUCATIVA]", institucion)
                                .Replace("[DDP_SEDE]", proyectoTemp.Sede.Nombre)
                                .Replace("[DDP_APORTANTE]", this.getNombreAportante(gestion.FuenteFinanciacion.Aportante))
                                .Replace("[VALOR_APORTANTE]", "$ " + String.Format("{0:n0}", gestion.FuenteFinanciacion.Aportante.CofinanciacionDocumento.Sum(x => x.ValorDocumento)).ToString())
                                .Replace("[DDP_FUENTE]", fuenteNombre)

                                .Replace("[DDP_SALDO_ACTUAL_FUENTE]", "$ " + String.Format("{0:n0}", gestion.SaldoActual).ToString())
                                .Replace("[DDP_VALOR_SOLICITADO_FUENTE]", "$ " + String.Format("{0:n0}", gestion.ValorSolicitado).ToString())
                                .Replace("[DDP_NUEVO_SALDO_FUENTE]", "$ " + String.Format("{0:n0}", (gestion.NuevoSaldo)).ToString());

                            //.Replace("[DDP_SALDO_ACTUAL_FUENTE]", "$ " + String.Format("{0:n0}", saldototal).ToString())
                            //.Replace("[DDP_VALOR_SOLICITADO_FUENTE]", "$ " + String.Format("{0:n0}", gestion.ValorSolicitado).ToString())
                            //.Replace("[DDP_NUEVO_SALDO_FUENTE]", "$ " + String.Format("{0:n0}", (saldototal - gestion.ValorSolicitado)).ToString());
                            tablaproyecto += tr;
                        }

                        var tr2 = plantilla_fuentes
                            .Replace("[NOMBRE_APORTANTE]", this.getNombreAportante(gestion.FuenteFinanciacion.Aportante))
                            .Replace("[FUENTE_APORTANTE]", fuenteNombre)
                            .Replace("[VALOR_NUMERO]", "$ " + String.Format("{0:n0}", gestion.ValorSolicitado).ToString())
                            .Replace("[VALOR_LETRAS]", CultureInfo.CurrentCulture.TextInfo
                                            .ToTitleCase(Helpers.Conversores
                                            .NumeroALetras(gestion.ValorSolicitado).ToLower()));
                        tablafuentes += tr2;
                        totales += gestion.ValorSolicitado;
                    }
                    proyecto = string.Empty;
                    proyecto = tablaproyecto;
                    if (!string.IsNullOrEmpty(proyecto))
                    {
                        pStrCabeceraProyectos = _context.Plantilla.Where(x => x.Codigo == codcabeceraproycto.ToString()).FirstOrDefault().Contenido;
                    }
                    limitacionEspecial = string.Empty;
                    tablaaportantes = plantilla_fuentecabecera.Replace("[TABLAAPORTANTES]", tablafuentes).
                       Replace("[TOTAL_DE_RECURSOS]", "$ " + String.Format("{0:n0}", totales).ToString()).
                       Replace("[TOTAL_DE_RECURSOSLETRAS]", CultureInfo.CurrentCulture.TextInfo
                                       .ToTitleCase(Helpers.Conversores
                                       .NumeroALetras(totales).ToLower()));
                }
            DateTime? fechaComitetecnico = DateTime.Now;

            string numeroComiteTecnico = string.Empty;
            //contratos
            Contrato contrato = new Contrato();
            if (pDisponibilidad.ContratacionId > 0)
            {
                contrato = _context.Contrato.Where(x => x.ContratacionId == pDisponibilidad.ContratacionId).FirstOrDefault();
                //LCT - ajuste data plantilla DDP
                var sesionComiteSolicitud = _context.SesionComiteSolicitud.Where(x => x.SolicitudId == pDisponibilidad.ContratacionId && x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion).
                                                                          Include(x => x.ComiteTecnico).ToList();
                if (sesionComiteSolicitud.Count() > 0)
                {
                    numeroComiteTecnico = sesionComiteSolicitud.FirstOrDefault().ComiteTecnico.NumeroComite;
                    fechaComitetecnico = Convert.ToDateTime(sesionComiteSolicitud.FirstOrDefault().ComiteTecnico.FechaOrdenDia);
                }
            }

            //SI ES NOVEDAD
            var contratotd = string.Empty;
            var novedadtd = string.Empty;
            var objetotd = _context.Plantilla.Where(x => x.Codigo == ((int)ConstanCodigoPlantillas.DDP_objeto).ToString()).FirstOrDefault().Contenido;
            objetotd = objetotd.Replace("[OBJETOINNER]", Helpers.Helpers.HtmlStringLimpio(pDisponibilidad.Objeto));


            //LCT
            var tiporubro = pDisponibilidad.TipoSolicitudEspecialCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(pDisponibilidad.TipoSolicitudEspecialCodigo, (int)EnumeratorTipoDominio.Tipo_DDP_Espacial) : pDisponibilidad.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? ConstanStringTipoSolicitudContratacion.proyectoAdministrativo : ConstanStringTipoSolicitudContratacion.contratacion;


            if (pDisponibilidad.EsNovedadContractual != null && Convert.ToBoolean(pDisponibilidad.EsNovedadContractual))
            {
                contratotd = _context.Plantilla.Where(x => x.Codigo == ((int)ConstanCodigoPlantillas.DDP_contrato).ToString()).FirstOrDefault().Contenido;
                contratotd = contratotd.Replace("[NUMERO_CONTRATO]", contrato.NumeroContrato);
                novedadtd = _context.Plantilla.Where(x => x.Codigo == ((int)ConstanCodigoPlantillas.DDP_novedad).ToString()).FirstOrDefault().Contenido;
                objetotd = objetotd.Replace("colspan=10", "colspan=7");
            }


            foreach (var place in placeholders)
            {
                switch (place.Codigo)
                {
                    case ConstanCodigoVariablesPlaceHolders.DDP_FECHA:
                        pStrContenido = pStrContenido
                            .Replace(place.Nombre, fechaComitetecnico != null ? ((DateTime)fechaComitetecnico).ToString("dd/MM/yyyy") : "");
                        break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NUMERO_SOLICITUD:
                        pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.NumeroSolicitud); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NO:
                        pStrContenido =
                            pStrContenido.Replace(place.Nombre, pDisponibilidad.NumeroDdp); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_RUBRO_POR_FINANCIAR:
                        pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial ? tiporubro : _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal
                                         && r.Codigo == pDisponibilidad.TipoSolicitudCodigo).FirstOrDefault().Descripcion); break;

                    case ConstanCodigoVariablesPlaceHolders.DDP_TIPO_SOLICITUD:
                        pStrContenido =
                            pStrContenido.Replace(place.Nombre, pDisponibilidad.TipoSolicitudCodigo != null ? _context.Dominio.Where(x => x.Codigo == pDisponibilidad.TipoSolicitudCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal).FirstOrDefault().Nombre :
                    //si no viene el campo puede ser contratación
                    pDisponibilidad.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? ConstanStringTipoSolicitudContratacion.proyectoAdministrativo :
                    pDisponibilidad.EsNovedadContractual == null ? ConstanStringTipoSolicitudContratacion.contratacion : !Convert.ToBoolean(pDisponibilidad.EsNovedadContractual) ? ConstanStringTipoSolicitudContratacion.contratacion : ConstanStringTipoSolicitudContratacion.novedadContractual); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_OPCION_CONTRATAR:
                        pStrContenido =
                            pStrContenido.Replace(place.Nombre, opcionContratarCodigo); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TABLA_LIMITACION_ESPECIAL:
                        pStrContenido =
                            pStrContenido.Replace(place.Nombre, limitacionEspecial); break;

                    case ConstanCodigoVariablesPlaceHolders.DDP_FECHA_COMITE_TECNICO: pStrContenido = pStrContenido.Replace(place.Nombre, fechaComitetecnico != null && !String.IsNullOrEmpty(numeroComiteTecnico) ? ((DateTime)fechaComitetecnico).ToString("dd/MM/yyyy") : ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NUMERO_COMITE: pStrContenido = pStrContenido.Replace(place.Nombre, numeroComiteTecnico); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_OBJETO: pStrContenido = pStrContenido.Replace(place.Nombre, Helpers.Helpers.HtmlStringLimpio(pDisponibilidad.Objeto)); break;
                    //case ConstanCodigoVariablesPlaceHolders.DDP_OBJETO: pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.Objeto); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TABLAAPORTANTES: pStrContenido = pStrContenido.Replace(place.Nombre, tablaaportantes); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TOTAL_DE_RECURSOS: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TOTAL_DE_RECURSOSLETRAS: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TABLA_PROYECTOS: pStrContenido = pStrContenido.Replace(place.Nombre, proyecto); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_LIMITACION_ESPECIAL: pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.LimitacionEspecial); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NOMBRE_APORTANTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_FUENTE_APORTANTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_VALOR_NUMERO: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_VALOR_LETRAS: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.CABECERAPROYECTOS:
                        pStrContenido = pStrContenido.Replace(place.Nombre, pStrCabeceraProyectos); break;


                    case ConstanCodigoVariablesPlaceHolders.DDP_LLAVE_MEN: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_INSTITUCION_EDUCATIVA: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_SEDE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_FUENTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_SALDO_ACTUAL_FUENTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_VALOR_SOLICITADO_FUENTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NUEVO_SALDO_FUENTE: pStrContenido = pStrContenido.Replace(place.Nombre, ""); break;

                    //drp
                    case ConstanCodigoVariablesPlaceHolders.NUMEROCONTRATO: pStrContenido = pStrContenido.Replace(place.Nombre, contrato == null ? "" : contrato.NumeroContrato); break;
                    case ConstanCodigoVariablesPlaceHolders.DRP_NO: pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.NumeroDrp); break;
                    case ConstanCodigoVariablesPlaceHolders.FECHACONTRATO: pStrContenido = pStrContenido.Replace(place.Nombre, contrato == null ? "" : contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : ""); break;
                    case ConstanCodigoVariablesPlaceHolders.TABLAFUENTES:
                        pStrContenido = pStrContenido.Replace(place.Nombre, tablafuentes); break;
                    case ConstanCodigoVariablesPlaceHolders.TABLAUSOS:
                        pStrContenido = pStrContenido.Replace(place.Nombre, tablauso); break;
                    case ConstanCodigoVariablesPlaceHolders.CONTRATOTD:
                        pStrContenido = pStrContenido.Replace(place.Nombre, contratotd); break;
                    case ConstanCodigoVariablesPlaceHolders.TIPONOVEDADTD:
                        pStrContenido = pStrContenido.Replace(place.Nombre, novedadtd); break;
                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_ANEXOS:
                        pStrContenido = pStrContenido.Replace(place.Nombre, string.Empty); break;
                }
            }

            return pStrContenido;
        }

        /*autor: jflorez
            descripción: return disponibilidad por validacion pres
        impacto: CU 3.3.2*/

        public async Task<Respuesta> SetReturnValidacionDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion, bool esNovedad, int RegistroPresupuestalId
            , string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                if (esNovedad == true)
                {
                    NovedadContractualRegistroPresupuestal novedad = _context.NovedadContractualRegistroPresupuestal.Find(pDisponibilidadPresObservacion.NovedadContractualRegistroPresupuestalId);
                    if (novedad != null)
                    {
                        int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Devuelta_por_validacion_presupuestal;
                        novedad.FechaModificacion = DateTime.Now;
                        novedad.UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion;
                        novedad.EstadoSolicitudCodigo = estado.ToString();

                        pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                        pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();
                        pDisponibilidadPresObservacion.EsNovedad = esNovedad;
                        pDisponibilidadPresObservacion.NovedadContractualRegistroPresupuestalId = novedad.NovedadContractualRegistroPresupuestalId;

                        _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                    }
                }
                else
                {
                    int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Devuelta_por_validacion_presupuestal;
                    DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                    DisponibilidadCancelar.UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion;
                    DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();

                    pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                    pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();
                    pDisponibilidadPresObservacion.NovedadContractualRegistroPresupuestalId = null;

                    _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                }


                _context.SaveChanges();
                //envio correo a tecnico
                //////


                var usuarioTecnico = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario).FirstOrDefault();
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DDPDevolucion);
                string template = TemplateRecoveryPassword.Contenido;
                template = template.Replace("[NUMERODISPONIBILIDAD]", DisponibilidadCancelar.NumeroSolicitud).
                    Replace("_LinkF_", pDominioFront);
                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioTecnico.Usuario.Email, "SDP Devuelto por validación presupuestal", template, pSentender, pPassword, pMailServer, pMailPort);
                this.EliminarGestion(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);
                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.DevueltoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.DevueltoCorrrectamente, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, "RECHAZAR DISPONIBILIDAD PRESUPUESTAL")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        /*autor: jflorez
            descripción: rechaza disponibilidad por validacion pres
        impacto: CU 3.3.2*/

        /*autor: JMartinezC
        * descripción: cuando se rechaza tambien la contratacion debe quedar en estado rechazado
        */
        public async Task<Respuesta> SetRechazarValidacionDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion, bool esNovedad
            , string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                if (esNovedad == true)
                {
                    NovedadContractualRegistroPresupuestal novedad = _context.NovedadContractualRegistroPresupuestal.Find(pDisponibilidadPresObservacion.NovedadContractualRegistroPresupuestalId);
                    if (novedad != null)
                    {
                        int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Rechazada_por_validacion_presupuestal;
                        novedad.FechaModificacion = DateTime.Now;
                        novedad.UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion;
                        novedad.EstadoSolicitudCodigo = estado.ToString();

                        pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                        pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();
                        pDisponibilidadPresObservacion.EsNovedad = esNovedad;

                        _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                    }
                }
                else
                {

                    Contratacion contratacionCancelar = _context.Contratacion.Find(DisponibilidadCancelar.ContratacionId);
                    contratacionCancelar.EstadoSolicitudCodigo = ConstanCodigoEstadoSolicitudContratacion.RechazadoComiteFiduciario;
                    contratacionCancelar.FechaModificacion = DateTime.Now;
                    contratacionCancelar.UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion;
                     
                    int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Rechazada_por_validacion_presupuestal;
                    DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                    DisponibilidadCancelar.UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion;
                    DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();
                    pDisponibilidadPresObservacion.NovedadContractualRegistroPresupuestalId = null;
                    pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                    pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();

                    _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);

                }
                _context.SaveChanges();
                //envio correo a técnico
                var usuarioTecnico = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario).FirstOrDefault();
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DDPRechazado);
                string template = TemplateRecoveryPassword.Contenido;
                template = template.Replace("[NUMERODISPONIBILIDAD]", DisponibilidadCancelar.NumeroSolicitud).
                    Replace("_LinkF_", pDominioFront);
                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioTecnico.Usuario.Email, "SDP rechazado por validación presupuestal", template, pSentender, pPassword, pMailServer, pMailPort);
                this.EliminarGestion(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);
                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.DevueltoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.DevueltoCorrrectamente, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, "DEVOLVER DISPONIBILIDAD PRESUPUESTAL")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        /*autor: jflorez
            descripción: valida disponibilidad por validacion pres
        impacto: CU 3.3.2*/
        public async Task<Respuesta> SetValidarValidacionDDP(int id, string usuariomod, bool esNovedad, int RegistroPresupuestalId
            , string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {

            DisponibilidadPresupuestal DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(id);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Con_validacion_presupuestal;

                if (esNovedad == true)
                {
                    NovedadContractualRegistroPresupuestal novedadContractualRegistroPresupuestal = _context.NovedadContractualRegistroPresupuestal.Find(RegistroPresupuestalId);

                    novedadContractualRegistroPresupuestal.FechaModificacion = DateTime.Now;
                    novedadContractualRegistroPresupuestal.UsuarioModificacion = usuariomod;
                    novedadContractualRegistroPresupuestal.EstadoSolicitudCodigo = estado.ToString();
                }
                else
                {
                    DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                    DisponibilidadCancelar.UsuarioModificacion = usuariomod;
                    DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();
                }


                /*
                pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();
                _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                _context.SaveChanges();*/
                //envio correo a juridica
                var usuarioJuridico = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Financiera).Include(y => y.Usuario).FirstOrDefault();
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DisponibilidadPresupuestalGenerada);
                string template = TemplateRecoveryPassword.Contenido.Replace("[NUMERODISPONIBILIDAD]", DisponibilidadCancelar.NumeroSolicitud).
                    Replace("_LinkF_", pDominioFront);
                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioJuridico.Usuario.Email, "SDP con validación presupuestal", template, pSentender, pPassword, pMailServer, pMailPort);

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.OperacionExitosa, idAccion, usuariomod, "CON DISPONIBILIDAD PRESUPUESTAL")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, usuariomod, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        public async Task<Respuesta> CreateFinancialFundingGestion(GestionFuenteFinanciacion pDisponibilidadPresObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                if (pDisponibilidadPresObservacion.GestionFuenteFinanciacionId > 0)
                {
                    var gsertion = _context.GestionFuenteFinanciacion.Find(pDisponibilidadPresObservacion.GestionFuenteFinanciacionId);
                    decimal valoresSolicitados = 0;
                    if (gsertion.DisponibilidadPresupuestalId > 0)
                    {
                        valoresSolicitados = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId ==
                         pDisponibilidadPresObservacion.FuenteFinanciacionId &&
                         x.DisponibilidadPresupuestalId == gsertion.DisponibilidadPresupuestalId).Sum(x => x.ValorSolicitado);
                    }
                    else
                    {
                        valoresSolicitados = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId ==
                        pDisponibilidadPresObservacion.FuenteFinanciacionId &&
                        x.DisponibilidadPresupuestalProyectoId == gsertion.DisponibilidadPresupuestalProyectoId).Sum(x => x.ValorSolicitado);
                    }

                    var fuente = _context.FuenteFinanciacion.Find(pDisponibilidadPresObservacion.FuenteFinanciacionId);
                    pDisponibilidadPresObservacion.SaldoActual = (decimal)fuente.ValorFuente - valoresSolicitados;
                    pDisponibilidadPresObservacion.NuevoSaldo = pDisponibilidadPresObservacion.SaldoActual - pDisponibilidadPresObservacion.ValorSolicitado;

                    gsertion.FechaModificacion = DateTime.Now;
                    gsertion.UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion;
                    gsertion.ValorSolicitado = pDisponibilidadPresObservacion.ValorSolicitado;
                    gsertion.FuenteFinanciacionId = pDisponibilidadPresObservacion.FuenteFinanciacionId;
                    _context.GestionFuenteFinanciacion.Update(gsertion);
                    _context.SaveChanges();
                }
                else
                {
                    var valoresSolicitados = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == pDisponibilidadPresObservacion.FuenteFinanciacionId).Sum(x => x.ValorSolicitado);
                    var fuente = _context.FuenteFinanciacion.Find(pDisponibilidadPresObservacion.FuenteFinanciacionId);
                    pDisponibilidadPresObservacion.SaldoActual = (decimal)fuente.ValorFuente - valoresSolicitados;
                    pDisponibilidadPresObservacion.NuevoSaldo = pDisponibilidadPresObservacion.SaldoActual - pDisponibilidadPresObservacion.ValorSolicitado;
                    int estado = (int)EnumeratorEstadoGestionFuenteFinanciacion.Solicitado;
                    pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                    pDisponibilidadPresObservacion.EstadoCodigo = estado.ToString();
                    pDisponibilidadPresObservacion.Eliminado = false;
                    _context.GestionFuenteFinanciacion.Add(pDisponibilidadPresObservacion);
                    _context.SaveChanges();
                }

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.DevueltoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.OperacionExitosa, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, "SOLICITUD A FUENTE DE FINANCIACION CREADA")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        public async Task<Respuesta> DeleteFinancialFundingGestion(int pIdDisponibilidadPresObservacion, string usuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {

                var pDisponibilidadPresObservacion = _context.GestionFuenteFinanciacion.Find(pIdDisponibilidadPresObservacion);
                pDisponibilidadPresObservacion.FechaModificacion = DateTime.Now;
                pDisponibilidadPresObservacion.UsuarioModificacion = usuarioModificacion;
                pDisponibilidadPresObservacion.Eliminado = true;
                _context.SaveChanges();
                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.DevueltoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.OperacionExitosa, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, "SOLICITUD A FUENTE DE FINANCIACION ELIMINADA")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, usuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        public async Task<Respuesta> GetFinancialFundingGestionByDDPP(int pIdDisponibilidadPresupuestalProyecto, string usuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            return new Respuesta
            {
                IsSuccessful = true,
                IsException = false,
                IsValidation = false,
                Code = ConstantMessagesGenerateBudget.Error,
                Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, idAccion, usuarioModificacion, "TRAER FUENTES GESTIONADAS"),
                Data = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyectoId == pIdDisponibilidadPresupuestalProyecto).Include(x => x.FuenteFinanciacion).Include(x => x.DisponibilidadPresupuestalProyecto).ThenInclude(x => x.Proyecto).ToList()
            };
        }

        /*autor: jflorez
            descripción: listo los datos ordenados de disponibilidades
        impacto: CU 3.3.4*/
        public async Task<EstadosDisponibilidad> GetListGenerarRegistroPresupuestal()
        {
            EstadosDisponibilidad estadosdisponibles =
                 new EstadosDisponibilidad { DominioId = Convert.ToInt32(ConstanCodigoSolicitudDisponibilidadPresupuestal.Con_Disponibilidad_Presupuestal), NombreEstado = "", DisponibilidadPresupuestal = await this.GetListDisponibilidadPresupuestalContratacionByCodigoEstadoSolicitud(ConstanCodigoSolicitudDisponibilidadPresupuestal.Con_Disponibilidad_Presupuestal) };
            return estadosdisponibles;
        }
        /*autor: jflorez
           descripción: cancelo la disponibilidades
       impacto: CU 3.3.4*/
        public async Task<Respuesta> SetCancelDisponibilidadPresupuestal(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion,
            string urlDestino, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Sin_registro_presupuestal;
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion;
                DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();

                pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();
                _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                _context.SaveChanges();
                var usuarioJuridico = _context.UsuarioPerfil.Where(x => (x.PerfilId == (int)EnumeratorPerfil.Juridica ||
                 x.PerfilId == (int)EnumeratorPerfil.Tecnica)
                ).Include(y => y.Usuario).ToList();
                bool blEnvioCorreo = true;
                //envio correo a juridica
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DDPCancelado);
                string template = TemplateRecoveryPassword.Contenido;

                template = template.Replace("_LinkF_", urlDestino);
                template = template.Replace("[NUMERODISPONIBILIDAD]", DisponibilidadCancelar.NumeroSolicitud);
                foreach (var usuario in usuarioJuridico)
                {
                    blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuario.Usuario.Email, "DDP Cancelada", template, pSentender, pPassword, pMailServer, pMailPort);
                }
                this.EliminarGestion(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);
                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.CanceladoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Registro_Presupuestal, ConstantMessagesGenerateBudget.CanceladoCorrrectamente, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, "CANCELAR DISPONIBILIDAD PRESUPUESTAL")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Registro_Presupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pDisponibilidadPresObservacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        /*autor: jflorez
           descripción: listo los datos ordenados de disponibilidades
       impacto: CU 3.3.4*/
        public async Task<Respuesta> CreateDRP(int pId, string pUsuarioModificacion, bool esNovedad, int RegistroPresupuestalId, string urlDestino, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal
                                                    .Include(x => x.Contratacion)
                                                        .ThenInclude(x => x.Contrato)
                                                    .Include(x => x.Contratacion)
                                                        .ThenInclude(x => x.ContratacionProyecto)
                                                            .ThenInclude(x => x.ContratacionProyectoAportante)
                                                                .ThenInclude(x => x.CofinanciacionAportante).
                                                                    ThenInclude(x => x.FuenteFinanciacion)
                                                    .FirstOrDefault(x => x.DisponibilidadPresupuestalId == pId);

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            int consecutivo = _context.DisponibilidadPresupuestal.Where(x => x.NumeroDrp != null).Count();
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Con_registro_presupuestal;

                if (esNovedad)
                {
                    consecutivo = _context.NovedadContractualRegistroPresupuestal.Where(x => x.NumeroDrp != null).Count();


                    NovedadContractualRegistroPresupuestal novedadContractualRegistroPresupuestal = _context.NovedadContractualRegistroPresupuestal
                                                                                                                .Find(RegistroPresupuestalId);

                    List<GestionFuenteFinanciacion> listaGestion = _context.GestionFuenteFinanciacion
                                                                    .Where(x => x.NovedadContractualRegistroPresupuestalId == RegistroPresupuestalId &&
                                                                           x.Eliminado != true &&
                                                                           x.EsNovedad == true
                                                                           )
                                                                    .ToList();

                    novedadContractualRegistroPresupuestal.EstadoSolicitudCodigo = estado.ToString();
                    novedadContractualRegistroPresupuestal.NumeroDrp = "DRP_NOV_" + consecutivo.ToString();
                    novedadContractualRegistroPresupuestal.FechaDrp = DateTime.Now;

                    foreach (GestionFuenteFinanciacion gestion in listaGestion)
                    {
                        int estadocod = (int)EnumeratorEstadoGestionFuenteFinanciacion.Apartado_en_DDP;
                        gestion.EstadoCodigo = estadocod.ToString();
                        gestion.FechaModificacion = DateTime.Now;
                        gestion.UsuarioModificacion = pUsuarioModificacion.ToUpper();
                        _context.GestionFuenteFinanciacion.Update(gestion);
                    }


                }
                else
                {
                    DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                    DisponibilidadCancelar.UsuarioModificacion = pUsuarioModificacion.ToUpper();
                    DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();
                    DisponibilidadCancelar.NumeroDrp = "DRP_PI_" + consecutivo.ToString();
                    DisponibilidadCancelar.FechaDrp = DateTime.Now;
                    //
                    //guardar el tema de platas
                    //
                    var gestionfuentes = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == DisponibilidadCancelar.DisponibilidadPresupuestalId);
                    foreach (var gestion in gestionfuentes)
                    {
                        int estadocod = (int)EnumeratorEstadoGestionFuenteFinanciacion.Apartado_en_DDP;
                        gestion.EstadoCodigo = estadocod.ToString();
                        gestion.FechaModificacion = DateTime.Now;
                        gestion.UsuarioModificacion = pUsuarioModificacion.ToUpper();
                        _context.GestionFuenteFinanciacion.Update(gestion);
                    }
                }


                _context.SaveChanges();
                //envio correo a juridica
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DRPNotificacion);
                string template = TemplateRecoveryPassword.Contenido;

                template = template.Replace("_LinkF_", urlDestino);
                template = template.Replace("[NUMEROCONTRATO]", DisponibilidadCancelar.Contratacion.Contrato.FirstOrDefault().NumeroContrato);
                template = template.Replace("[FECHACONTRATO]", DisponibilidadCancelar.Contratacion.Contrato.FirstOrDefault().FechaFirmaContrato != null ? Convert.ToDateTime(DisponibilidadCancelar.Contratacion.Contrato.FirstOrDefault().FechaFirmaContrato).ToString("dd/MM/yyyy") : "");
                template = template.Replace("[TIPOSOLICITUD]", DisponibilidadCancelar.EsNovedadContractual == null ? ConstanStringTipoSolicitudContratacion.contratacion : !Convert.ToBoolean(DisponibilidadCancelar.EsNovedadContractual) ? ConstanStringTipoSolicitudContratacion.contratacion : ConstanStringTipoSolicitudContratacion.novedadContractual);//esto va a cambiar
                template = template.Replace("[NUMERODRP]", DisponibilidadCancelar.NumeroDrp);
                template = template.Replace("[NUMERODISPONIBILIDAD]", DisponibilidadCancelar.NumeroDdp);
                /*busco usuario Juridico*/
                var usuarioJuridico = _context.UsuarioPerfil.Where(x => (x.PerfilId == (int)EnumeratorPerfil.Juridica ||
                x.PerfilId == (int)EnumeratorPerfil.Financiera || x.PerfilId == (int)EnumeratorPerfil.Tecnica)
                ).Include(y => y.Usuario).ToList();
                bool blEnvioCorreo = true;
                foreach (var usuario in usuarioJuridico)
                {
                    blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuario.Usuario.Email, "DRP Generada", template, pSentender, pPassword, pMailServer, pMailPort);
                }

                if (blEnvioCorreo)
                {
                    return new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantMessagesGenerateBudget.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Registro_Presupuestal, ConstantMessagesGenerateBudget.OperacionExitosa, idAccion, pUsuarioModificacion, "GENERAR DRP REGISTRO PRESUPUESTAL")
                    };
                }
                else
                {
                    return new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantMessagesGenerateBudget.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Registro_Presupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pUsuarioModificacion, "ERROR ENVIO MAIL GENERAR DRP REGISTRO PRESUPUESTAL")
                    };
                }
            }
            catch (Exception ex)
            {
                return
                new Respuesta
                {
                    Data = ex,
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Registro_Presupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<byte[]> GetPDFDRP(int id, string usuarioModificacion, bool esNovedad, int pRegistroPresupuestalId)
        {
            if (id == 0)
            {
                return Array.Empty<byte>();
            }
            DisponibilidadPresupuestal disponibilidad = await _context.DisponibilidadPresupuestal
                .Where(r => r.DisponibilidadPresupuestalId == id).FirstOrDefaultAsync();
            //.Include(r => r.SesionComiteTema).FirstOrDefaultAsync();

            if (disponibilidad == null)
            {
                return Array.Empty<byte>();
            }

            NovedadContractualRegistroPresupuestal novedadContractualRegistro = new NovedadContractualRegistroPresupuestal();

            if (esNovedad)
            {
                novedadContractualRegistro = _context.NovedadContractualRegistroPresupuestal.Find(pRegistroPresupuestalId);
            }

            Plantilla plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Ficha_DRP).ToString()).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            string contenido = await ReemplazarDatosDDPAsync(plantilla.Contenido, disponibilidad, true, esNovedad, novedadContractualRegistro, false);
            plantilla.Contenido = contenido;
            //return ConvertirPDF(plantilla);
            return Helpers.PDF.Convertir(plantilla, true);
        }

        public string getNombreAportante(CofinanciacionAportante confinanciacion)
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

        private string getNombreTipoAportante(CofinanciacionAportante confinanciacion)
        {
            string nombreTipoAportante;
            if (confinanciacion.TipoAportanteId.Equals(ConstanTipoAportante.Ffie))
            {
                nombreTipoAportante = ConstanStringTipoAportante.Ffie;
            }
            else if (confinanciacion.TipoAportanteId.Equals(ConstanTipoAportante.Tercero))
            {
                nombreTipoAportante = "Tercero";
            }
            else
            {
                if (confinanciacion.MunicipioId == null)
                {
                    nombreTipoAportante = "Gobernación";
                }
                else
                {
                    nombreTipoAportante = "Alcaldía";
                }
            }
            return nombreTipoAportante;
        }

    }
}