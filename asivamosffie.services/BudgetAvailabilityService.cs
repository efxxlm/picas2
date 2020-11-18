using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.api.Controllers;
using AuthorizationTest.JwtHelpers;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using Z.EntityFramework.Plus;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.IO;
using System.Globalization;
using System.Web;

namespace asivamosffie.services
{
    public class BudgetAvailabilityService : IBudgetAvailabilityService
    {
        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;
        public readonly IConverter _converter;

        public BudgetAvailabilityService(devAsiVamosFFIEContext context, ICommonService commonService, IConverter converter)
        {
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
                    DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? "Proyecto administrativo" :
                    "Contratación",
                    TipoSolicitud = strTipoSolicitud,
                    DisponibilidadPresupuestalId = DisponibilidadPresupuestal.DisponibilidadPresupuestalId,
                    NumeroSolicitud = DisponibilidadPresupuestal.NumeroSolicitud
                };

                ListDisponibilidadPresupuestalGrilla.Add(disponibilidadPresupuestalGrilla);
            }

            return ListDisponibilidadPresupuestalGrilla.OrderByDescending(r => r.EstadoRegistro).ToList();
        }
         
        public async Task<DisponibilidadPresupuestal> GetDisponibilidadPresupuestalByID(int pDisponibilidadPresupuestalId)
        {
            //las tabla DisponibilidadPresupuestalProyecto no tiene campos de auditoria
            var resultado= await _context.DisponibilidadPresupuestal.
                Where(r => r.DisponibilidadPresupuestalId == pDisponibilidadPresupuestalId).
                Include(r => r.DisponibilidadPresupuestalProyecto).
                ThenInclude(r=>r.Proyecto).FirstOrDefaultAsync();
            //busco comite técnico
            DateTime fechaComitetecnico = DateTime.Now;
            string numerocomietetecnico = "";
            foreach(var res in resultado.DisponibilidadPresupuestalProyecto)
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
            if(resultado.AportanteId>0)
            {
                resultado.stringAportante = getNombreAportante(_context.CofinanciacionAportante.Find(resultado.AportanteId));
            }
            
            return resultado;

        }
         
        public async Task<DisponibilidadPresupuestal> GetBudgetAvailabilityById(int id)
        {
            try
            {
                var dis= await _context.DisponibilidadPresupuestal.Where(d => d.DisponibilidadPresupuestalId == id)
                                    .Include(r => r.DisponibilidadPresupuestalProyecto)                                    
                                    .FirstOrDefaultAsync();
                DateTime fechaComitetecnico = DateTime.Now;
                string numerocomietetecnico = "";
                if (dis.ContratacionId != null)
                {
                    var contratacion = _context.SesionComiteSolicitud.Where(x => x.SolicitudId== dis.ContratacionId && x.TipoSolicitudCodigo==ConstanCodigoTipoSolicitud.Contratacion).
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
         
        public async Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(string pCodigoEstadoSolicitud)
        {

            List<DisponibilidadPresupuestal> ListDisponibilidadPresupuestal = 
                await _context.DisponibilidadPresupuestal.Where(r => !(bool)r.Eliminado && r.EstadoSolicitudCodigo.Equals(pCodigoEstadoSolicitud))
                .Include(x=>x.DisponibilidadPresupuestalProyecto)
                .ToListAsync();

            List<DisponibilidadPresupuestalGrilla> ListDisponibilidadPresupuestalGrilla = new List<DisponibilidadPresupuestalGrilla>();

            foreach (var DisponibilidadPresupuestal in ListDisponibilidadPresupuestal)
            {
                string strEstadoRegistro = "";
                string strTipoSolicitud = "";
                if (!string.IsNullOrEmpty(DisponibilidadPresupuestal.EstadoSolicitudCodigo))
                {
                    strEstadoRegistro = await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.EstadoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal);
                }

                if (!string.IsNullOrEmpty(DisponibilidadPresupuestal.TipoSolicitudCodigo))
                {
                    strTipoSolicitud = await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal);
                }

                DateTime? fechaContrato = null;
                string numeroContrato = "";
                if (DisponibilidadPresupuestal.ContratacionId != null)
                {
                    var contrato = _context.Contrato.Find(DisponibilidadPresupuestal.ContratacionId);
                    fechaContrato = contrato != null ? contrato.FechaFirmaContrato : null;
                    numeroContrato = DisponibilidadPresupuestal.NumeroContrato == null ?
                        contrato != null ? contrato.NumeroContrato : ""
                        : DisponibilidadPresupuestal.NumeroContrato;
                }
                else
                {
                    numeroContrato = DisponibilidadPresupuestal.NumeroContrato != null ?
                         DisponibilidadPresupuestal.NumeroContrato : "";
                }
                bool blnEstado = false;

                //si es administrativo, esta completo, si es tradicional, se verifica contra fuentes gestionadas
                //2020-11-08 ahora los administrativos y especiales tambien estionan fuentes
                if(DisponibilidadPresupuestal.TipoSolicitudCodigo== ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo)
                {
                    List<int> ddpproyectosId = DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.Select(x => x.DisponibilidadPresupuestalProyectoId).ToList();
                    if (_context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyectoId != null && ddpproyectosId.Contains((int)x.DisponibilidadPresupuestalProyectoId)).Count() > 0)
                    {
                        blnEstado = true;
                    }
                }
                else if(DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)
                {
                    
                    if (_context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId ==DisponibilidadPresupuestal.DisponibilidadPresupuestalId).Count() > 0)
                    {
                        blnEstado = true;
                    }
                }
                else
                {
                    List<int> ddpproyectosId = DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.Select(x=>x.DisponibilidadPresupuestalProyectoId).ToList();
                    if(_context.GestionFuenteFinanciacion.Where(x=> x.DisponibilidadPresupuestalProyectoId!=null && ddpproyectosId.Contains((int)x.DisponibilidadPresupuestalProyectoId)).Count()>0)
                    {
                        blnEstado = true;
                    }
                }
                var contratacion = _context.Contratacion.Where(x=>x.ContratacionId==DisponibilidadPresupuestal.ContratacionId);
                DisponibilidadPresupuestalGrilla disponibilidadPresupuestalGrilla = new DisponibilidadPresupuestalGrilla
                {

                    FechaSolicitud = DisponibilidadPresupuestal.FechaSolicitud.ToString("dd/MM/yyyy"),
                    EstadoRegistro = blnEstado,
                    TipoSolicitud = strTipoSolicitud,
                    TipoSolicitudEspecial= DisponibilidadPresupuestal.TipoSolicitudEspecialCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.TipoSolicitudEspecialCodigo, (int)EnumeratorTipoDominio.Tipo_DDP_Espacial) :
                    //si no viene el campo puede ser contratación
                    DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? "Proyecto administrativo" :
                    "Contratación",
                    DisponibilidadPresupuestalId = DisponibilidadPresupuestal.DisponibilidadPresupuestalId,
                    NumeroSolicitud = DisponibilidadPresupuestal.NumeroSolicitud,
                    NumeroDDP = DisponibilidadPresupuestal.NumeroDdp,
                    FechaFirmaContrato = fechaContrato == null ? "" : Convert.ToDateTime(fechaContrato).ToString("dd/MM/yyyy"),
                    NumeroContrato = numeroContrato,
                    Contratacion = contratacion

                };
                ListDisponibilidadPresupuestalGrilla.Add(disponibilidadPresupuestalGrilla);
            }
            return ListDisponibilidadPresupuestalGrilla.OrderByDescending(r => r.DisponibilidadPresupuestalId).ToList();

        }

        /*3.3.4 listado de rp
         jflorez: cambio 20201118 deben filtrarse por los contratos que esten en estado registrado*/
        public async Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestalContratacionByCodigoEstadoSolicitud(string pCodigoEstadoSolicitud)
        {
            int codigocondvalidacionpre = (int)EnumeratorEstadoSolicitudPresupuestal.Con_registro_presupuestal;
            int codigocancelada = (int)EnumeratorEstadoSolicitudPresupuestal.Sin_registro_presupuestal;
            List<DisponibilidadPresupuestal> ListDisponibilidadPresupuestal =
                await _context.DisponibilidadPresupuestal.Where(r => !(bool)r.Eliminado
                    && (r.EstadoSolicitudCodigo.Equals(pCodigoEstadoSolicitud) || r.EstadoSolicitudCodigo.Equals(codigocondvalidacionpre.ToString())
                    || r.EstadoSolicitudCodigo.Equals(codigocancelada.ToString()))
                    && r.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Tradicional
                    && r.Contratacion.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.Registrados)
                .Include(x => x.DisponibilidadPresupuestalProyecto).Include(x=>x.Contratacion).ThenInclude(x=>x.Contrato)
                .ToListAsync();

            List<DisponibilidadPresupuestalGrilla> ListDisponibilidadPresupuestalGrilla = new List<DisponibilidadPresupuestalGrilla>();

            foreach (var DisponibilidadPresupuestal in ListDisponibilidadPresupuestal)
            {
                string strEstadoRegistro = "";
                string strTipoSolicitud = "";
                if (!string.IsNullOrEmpty(DisponibilidadPresupuestal.EstadoSolicitudCodigo))
                {
                    strEstadoRegistro = await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.EstadoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal);
                }

                if (!string.IsNullOrEmpty(DisponibilidadPresupuestal.TipoSolicitudCodigo))
                {
                    strTipoSolicitud = await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal);
                }

                DateTime? fechaContrato = null;
                string numeroContrato = "";
                if (DisponibilidadPresupuestal.ContratacionId != null)
                {
                    var contrato = _context.Contrato.Where(x=>x.ContratacionId==DisponibilidadPresupuestal.ContratacionId).FirstOrDefault();
                    fechaContrato = contrato != null ? contrato.FechaFirmaContrato : null;
                    numeroContrato = DisponibilidadPresupuestal.NumeroContrato == null ?
                        contrato != null ? contrato.NumeroContrato : ""
                        : DisponibilidadPresupuestal.NumeroContrato;
                }
                else
                {
                    numeroContrato = DisponibilidadPresupuestal.NumeroContrato != null ?
                         DisponibilidadPresupuestal.NumeroContrato : "";
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

                    FechaSolicitud = DisponibilidadPresupuestal.FechaSolicitud.ToString("yyyy/MM/dd"),
                    EstadoRegistro = blnEstado,
                    TipoSolicitud = strTipoSolicitud,
                    TipoSolicitudEspecial = DisponibilidadPresupuestal.TipoSolicitudEspecialCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.TipoSolicitudEspecialCodigo, (int)EnumeratorTipoDominio.Tipo_DDP_Espacial) :
                    //si no viene el campo puede ser contratación
                    DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? "Proyecto administrativo" :
                    "Contratación",
                    DisponibilidadPresupuestalId = DisponibilidadPresupuestal.DisponibilidadPresupuestalId,
                    NumeroSolicitud = DisponibilidadPresupuestal.NumeroSolicitud,
                    FechaFirmaContrato = fechaContrato == null ? "" : Convert.ToDateTime(fechaContrato).ToString("dd/MM/yyyy"),
                    NumeroContrato = numeroContrato,
                    Estado =_context.Dominio.Where(x=>x.TipoDominioId== (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal && 
                        x.Codigo==DisponibilidadPresupuestal.EstadoSolicitudCodigo).FirstOrDefault().Nombre

                };
               
                ListDisponibilidadPresupuestalGrilla.Add(disponibilidadPresupuestalGrilla);
            }
            return ListDisponibilidadPresupuestalGrilla.OrderByDescending(r => r.DisponibilidadPresupuestalId).ToList();

        }

        /*autor: jflorez
            descripción: objeto para entregar a front los datos ordenados de disponibilidades
        impacto: CU 3.3.3*/
        public async Task<List<EstadosDisponibilidad>> GetListGenerarDisponibilidadPresupuestal()
        {
            List<EstadosDisponibilidad> estadosdisponibles = new List<EstadosDisponibilidad>();
            var estados = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal && x.Activo == true).ToList();
            foreach (var estado in estados)
            {
                estadosdisponibles.Add(new EstadosDisponibilidad { DominioId = estado.DominioId, NombreEstado = estado.Nombre, DisponibilidadPresupuestal = await this.GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(estado.Codigo) });
            }
            return estadosdisponibles;
        }

        public async Task<Respuesta> SetCancelRegistroPresupuestal(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion
            ,string urlDestino, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
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
                //envio correo
                var usuarioJuridico = _context.UsuarioPerfil.Where(x => (x.PerfilId == (int)EnumeratorPerfil.Juridica ||
                 x.PerfilId == (int)EnumeratorPerfil.Tecnica)
                ).Include(y => y.Usuario).ToList();
                bool blEnvioCorreo = true;
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DRPCancelado);
                string template = TemplateRecoveryPassword.Contenido;

                template = template.Replace("_LinkF_", urlDestino);
                template = template.Replace("[NUMERODISPONIBILIDAD]", DisponibilidadCancelar.NumeroSolicitud);
                foreach (var usuario in usuarioJuridico)
                {
                    blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuario.Usuario.Email, "DRP Cancelada", template, pSentender, pPassword, pMailServer, pMailPort);
                }
                this.eliminarGestion(DisponibilidadCancelar);
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

        public async Task<Respuesta> CreateDDP(int pId, string pUsuarioModificacion,string urlDestino, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Include(x => x.Contratacion).
                ThenInclude(x => x.ContratacionProyecto).ThenInclude(x => x.ContratacionProyectoAportante).ThenInclude(x => x.CofinanciacionAportante).
                ThenInclude(x => x.FuenteFinanciacion).FirstOrDefault(x => x.DisponibilidadPresupuestalId == pId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            int consecutivo = _context.DisponibilidadPresupuestal.Where(x => x.NumeroDdp != null).Count();
            /*busco usuario Juridico*/
            var usuarioJuridico = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Juridica).Include(y => y.Usuario).FirstOrDefault();
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Con_disponibilidad_presupuestal;
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
                var gestionfuentes = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == DisponibilidadCancelar.DisponibilidadPresupuestalId);
                foreach (var gestion in gestionfuentes)
                {
                    int estadocod = (int)EnumeratorEstadoGestionFuenteFinanciacion.Apartado_en_DDP;
                    gestion.EstadoCodigo = estadocod.ToString();
                    gestion.FechaModificacion = DateTime.Now;
                    gestion.UsuarioModificacion = pUsuarioModificacion.ToUpper();
                    _context.GestionFuenteFinanciacion.Update(gestion);
                }

                _context.SaveChanges();
                //envio correo a juridica
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DisponibilidadPresupuestalGenerada);
                string template = TemplateRecoveryPassword.Contenido;

                template = template.Replace("_LinkF_", urlDestino);
                template = template.Replace("[NUMERODISPONIBILIDAD]", DisponibilidadCancelar.NumeroSolicitud);

                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioJuridico.Usuario.Email, "DDP Generado", template, pSentender, pPassword, pMailServer, pMailPort);
                if (blEnvioCorreo)
                {
                    return new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantMessagesGenerateBudget.OperacionExitosa,
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
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Devuelta_por_coordinacion_financiera;
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion;
                DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();

                pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();
                _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                this.eliminarGestion(DisponibilidadCancelar);


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

        private bool eliminarGestion(DisponibilidadPresupuestal DisponibilidadCancelar)
        {
            bool retorno = false;
            if (DisponibilidadCancelar.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)
            {
                var gestionFuentes = _context.GestionFuenteFinanciacion.Where(x => x.DisponibilidadPresupuestalId == DisponibilidadCancelar.DisponibilidadPresupuestalId).ToList();
                foreach (var gestion in gestionFuentes)
                {
                    gestion.Eliminado = true;
                    gestion.FechaModificacion = DateTime.Now;
                    gestion.UsuarioModificacion = DisponibilidadCancelar.UsuarioModificacion;
                    _context.GestionFuenteFinanciacion.Update(gestion);
                }
            }
            else
            {
                var gestionFuentes = _context.GestionFuenteFinanciacion.Where(x => x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == DisponibilidadCancelar.DisponibilidadPresupuestalId).ToList();
                foreach (var gestion in gestionFuentes)
                {
                    gestion.Eliminado = true;
                    gestion.FechaModificacion = DateTime.Now;
                    gestion.UsuarioModificacion = DisponibilidadCancelar.UsuarioCreacion;
                    _context.GestionFuenteFinanciacion.Update(gestion);
                }
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
         
        public async Task<byte[]> GetPDFDDP(int id, string pUsurioGenero)
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
            Plantilla plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Ficha_De_DDP).ToString())
                .Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();

            plantilla.Contenido = ReemplazarDatosDDP(plantilla.Contenido, disponibilidad,false);
            return ConvertirPDF(plantilla);
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
                Orientation = Orientation.Portrait,
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

        private string ReemplazarDatosDDP(string pStrContenido, DisponibilidadPresupuestal pDisponibilidad,bool drp)
        {
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolderDDP).ToList();
            /*variables que pueden diferir de uno u otro tipo*/
            string opcionContratarCodigo = "";
            string proyecto = "";
            string pStrCabeceraProyectos = "";
            string limitacionEspecial = "";
            string tablaaportantes = "";
            decimal saldototal = 0;
            string tablafuentes = "";
            string tablauso = "";
            string tablaproyecto = "";
            if (drp)
            {
                int codtablafuentes = (int)ConstanCodigoPlantillas.DRP_TABLA_FUENTES;
                int codtablauso = (int)ConstanCodigoPlantillas.DRP_TABLA_USOS;
                var plantilla_fuentes = _context.Plantilla.Where(x => x.Codigo == codtablafuentes.ToString()).FirstOrDefault().Contenido;
                var plantilla_uso = _context.Plantilla.Where(x => x.Codigo == codtablauso.ToString()).FirstOrDefault().Contenido;
                //empuezo con fuentes
                var gestionfuentes = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == pDisponibilidad.DisponibilidadPresupuestalId).
                    Include(x=>x.FuenteFinanciacion).
                        ThenInclude(x => x.Aportante).
                        ThenInclude(x=>x.CofinanciacionDocumento).
                    Include(x => x.DisponibilidadPresupuestalProyecto).
                        ThenInclude(x => x.Proyecto).
                            ThenInclude(x => x.Sede).
                    ToList();
                


                foreach (var gestion in gestionfuentes)
                {
                    //el saldo actual de la fuente son todas las solicitudes a la fuentes
                    //var consignadoemnfuente = _context.ControlRecurso.Where(x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId).Sum(x => x.ValorConsignacion);
                    var consignadoemnfuente = _context.FuenteFinanciacion.Where(x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId).Sum(x => x.ValorFuente);
                    var saldofuente = _context.GestionFuenteFinanciacion.Where(
                        x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId && 
                        x.DisponibilidadPresupuestalProyectoId!=gestion.DisponibilidadPresupuestalProyectoId).Sum(x => x.ValorSolicitado);
                    string fuenteNombre = _context.Dominio.Where(x => x.Codigo == gestion.FuenteFinanciacion.FuenteRecursosCodigo
                            && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;
                    //(decimal)font.FuenteFinanciacion.ValorFuente,
                    // Saldo_actual_de_la_fuente = (decimal)font.FuenteFinanciacion.ValorFuente - saldofuente
                    saldototal += (decimal)consignadoemnfuente - saldofuente;
                    string institucion = _context.InstitucionEducativaSede.Where(x => x.InstitucionEducativaSedeId == gestion.DisponibilidadPresupuestalProyecto.Proyecto.Sede.PadreId).FirstOrDefault().Nombre;
                    var tr = plantilla_fuentes.Replace("[LLAVEMEN]", gestion.DisponibilidadPresupuestalProyecto.Proyecto.LlaveMen)
                        .Replace("[INSTITUCION]", institucion)
                        .Replace("[SEDE]", gestion.DisponibilidadPresupuestalProyecto.Proyecto.Sede.Nombre)
                        .Replace("[APORTANTE]", this.getNombreAportante(gestion.FuenteFinanciacion.Aportante))
                        .Replace("[VALOR_APORTANTE]", gestion.FuenteFinanciacion.Aportante.CofinanciacionDocumento.Sum(x=>x.ValorDocumento).ToString())
                        .Replace("[FUENTE]", fuenteNombre)
                        .Replace("[SALDO_FUENTE]", saldototal.ToString())
                        .Replace("[VALOR_FUENTE]", gestion.ValorSolicitado.ToString())
                        .Replace("[NUEVO_SALDO_FUENTE]", (saldototal- gestion.ValorSolicitado).ToString());
                    tablafuentes += tr;                  
                }

                //usos                    
                var componenteAp = _context.ComponenteAportante.Where(x => x.ContratacionProyectoAportante.ContratacionProyecto.ContratacionId == pDisponibilidad.ContratacionId).
                    Include(x => x.ComponenteUso).
                    Include(x => x.ContratacionProyectoAportante).
                    ThenInclude(x => x.ContratacionProyecto).
                    ThenInclude(x => x.Proyecto).ToList();
                foreach (var compAp in componenteAp)
                {
                    List<string> uso = new List<string>();
                    List<decimal> usovalor = new List<decimal>();
                    decimal total = 0;
                    var dom = _context.Dominio.Where(x => x.Codigo == compAp.TipoComponenteCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Componentes).ToList();
                    var strFase = _context.Dominio.Where(r => r.Codigo == compAp.FaseCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Fases).FirstOrDefault();
                    foreach (var comp in compAp.ComponenteUso)
                    {
                        

                        var usos = _context.Dominio.Where(x => x.Codigo == comp.TipoUsoCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Usos).ToList();
                        uso.Add(usos.Count() > 0 ? usos.FirstOrDefault().Nombre : "");
                        string llavemen = compAp.ContratacionProyectoAportante.ContratacionProyecto.Proyecto.LlaveMen;
                        var fuentestring = plantilla_uso.Replace("[LLAVEMEN2]", llavemen).
                            Replace("[FASE]", strFase.Nombre).
                            Replace("[COMPONENTE]", dom.FirstOrDefault().Nombre).
                            Replace("[USO]", usos.FirstOrDefault().Nombre).
                            Replace("[VALOR_USO]", comp.ValorUso.ToString());
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
                
                
                
                //empuezo con fuentes
                var gestionfuentes = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == pDisponibilidad.DisponibilidadPresupuestalId).
                    Include(x => x.FuenteFinanciacion).
                        ThenInclude(x => x.Aportante).
                        ThenInclude(x => x.CofinanciacionDocumento).
                    Include(x => x.DisponibilidadPresupuestalProyecto).
                        ThenInclude(x => x.Proyecto).
                            ThenInclude(x => x.Sede).
                    ToList();
                decimal total = 0;
                foreach (var gestion in gestionfuentes)
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
                    string institucion = _context.InstitucionEducativaSede.Where(x => x.InstitucionEducativaSedeId == gestion.DisponibilidadPresupuestalProyecto.Proyecto.Sede.PadreId).FirstOrDefault().Nombre;
                    var tr = plantilla_proycto.Replace("[DDP_LLAVE_MEN]", gestion.DisponibilidadPresupuestalProyecto.Proyecto.LlaveMen)
                        .Replace("[DDP_INSTITUCION_EDUCATIVA]", institucion)
                        .Replace("[DDP_SEDE]", gestion.DisponibilidadPresupuestalProyecto.Proyecto.Sede.Nombre)
                        .Replace("[DDP_APORTANTE]", this.getNombreAportante(gestion.FuenteFinanciacion.Aportante))
                        .Replace("[VALOR_APORTANTE]", gestion.FuenteFinanciacion.Aportante.CofinanciacionDocumento.Sum(x => x.ValorDocumento).ToString())
                        .Replace("[DDP_FUENTE]", fuenteNombre)
                        .Replace("[DDP_SALDO_ACTUAL_FUENTE]", saldototal.ToString())
                        .Replace("[DDP_VALOR_SOLICITADO_FUENTE]", gestion.ValorSolicitado.ToString())
                        .Replace("[DDP_NUEVO_SALDO_FUENTE]", (saldototal - gestion.ValorSolicitado).ToString());
                    tablaproyecto += tr;

                    var tr2 = plantilla_fuentes
                        .Replace("[NOMBRE_APORTANTE]", this.getNombreAportante(gestion.FuenteFinanciacion.Aportante))                        
                        .Replace("[FUENTE_APORTANTE]", fuenteNombre)                       
                        .Replace("[VALOR_NUMERO]", gestion.ValorSolicitado.ToString())
                        .Replace("[VALOR_LETRAS]", CultureInfo.CurrentCulture.TextInfo
                                        .ToTitleCase(Helpers.Conversores
                                        .NumeroALetras(gestion.ValorSolicitado).ToLower()));
                    tablafuentes += tr2;
                    total += gestion.ValorSolicitado;
                }

                if (pDisponibilidad.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Tradicional)
                {
                    opcionContratarCodigo = pDisponibilidad.OpcionContratarCodigo;
                    pStrCabeceraProyectos = _context.Plantilla.Where(x => x.Codigo == codcabeceraproycto.ToString()).FirstOrDefault().Contenido;
                    proyecto = tablaproyecto;
                    var limespecial = _context.Plantilla.Where(x => x.Codigo == ((int)ConstanCodigoPlantillas.DDP_limitacion).ToString());
                    limitacionEspecial = limespecial.Any() ? limespecial.FirstOrDefault().Contenido : "";
                    limitacionEspecial = limitacionEspecial.Replace(placeholders.Where(x => x.Codigo == ConstanCodigoVariablesPlaceHolders.DDP_LIMITACION_ESPECIAL).FirstOrDefault().Nombre
                        , limitacionEspecial);
                    tablaaportantes = plantilla_fuentecabecera.Replace("[TABLAAPORTANTES]", tablafuentes).
                        Replace("[TOTAL_DE_RECURSOS]", total.ToString()).
                        Replace("[TOTAL_DE_RECURSOSLETRAS]", CultureInfo.CurrentCulture.TextInfo
                                        .ToTitleCase(Helpers.Conversores
                                        .NumeroALetras(total).ToLower()));
                    

                }
                else if (pDisponibilidad.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo)
                {
                    opcionContratarCodigo = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal
                                    && r.Codigo == pDisponibilidad.TipoSolicitudCodigo).FirstOrDefault().Descripcion;
                    proyecto = "";
                    limitacionEspecial = "";
                    string aportanteTablaPrincipal = "";
                    string aportanteTr = "";
                    var aportantes = _context.Plantilla.Where(x => x.Codigo == ((int)ConstanCodigoPlantillas.DDP_Aportante_principal).ToString());
                    aportanteTablaPrincipal = aportantes.Any() ? aportantes.FirstOrDefault().Contenido : "";
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
                            aportanteTrDato = aportanteTrDato.Replace("[VALOR_NUMERO]", saldofuente.ToString());
                            aportanteTrDato = aportanteTrDato.Replace("[VALOR_LETRAS]", CultureInfo.CurrentCulture.TextInfo
                                        .ToTitleCase(Helpers.Conversores
                                        .NumeroALetras(saldofuente).ToLower()));
                            aportanteTr += aportanteTrDato;
                        }


                    }

                    tablaaportantes = aportanteTablaPrincipal.Replace("[TABLAAPORTANTES]", aportanteTr).
                        Replace("[TOTAL_DE_RECURSOS]", "2").
                        Replace("[TOTAL_DE_RECURSOSLETRAS]", "dos");
                }
                else//ddp especial
                {
                    //empuezo con fuentes
                    var gestionfuentesEspecial = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == pDisponibilidad.DisponibilidadPresupuestalId).
                        Include(x => x.FuenteFinanciacion).
                            ThenInclude(x => x.Aportante).
                            ThenInclude(x => x.CofinanciacionDocumento).
                        Include(x => x.DisponibilidadPresupuestalProyecto).
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
                        

                        var tr2 = plantilla_fuentes
                            .Replace("[NOMBRE_APORTANTE]", this.getNombreAportante(gestion.FuenteFinanciacion.Aportante))
                            .Replace("[FUENTE_APORTANTE]", fuenteNombre)
                            .Replace("[VALOR_NUMERO]", gestion.ValorSolicitado.ToString())
                            .Replace("[VALOR_LETRAS]", CultureInfo.CurrentCulture.TextInfo
                                            .ToTitleCase(Helpers.Conversores
                                            .NumeroALetras(gestion.ValorSolicitado).ToLower()));
                        tablafuentes += tr2;
                        totales += gestion.ValorSolicitado;
                    }




                    proyecto = "";
                    limitacionEspecial = "";
                    tablaaportantes = plantilla_fuentecabecera.Replace("[TABLAAPORTANTES]", tablafuentes).
                       Replace("[TOTAL_DE_RECURSOS]", totales.ToString()).
                       Replace("[TOTAL_DE_RECURSOSLETRAS]", CultureInfo.CurrentCulture.TextInfo
                                       .ToTitleCase(Helpers.Conversores
                                       .NumeroALetras(totales).ToLower()));
                }
            }

            //contratos
            Contrato contrato = new Contrato();
            if(pDisponibilidad.ContratacionId>0)
            {
                contrato = _context.Contrato.Where(x => x.ContratacionId == pDisponibilidad.ContratacionId).FirstOrDefault();
            }

            foreach (var place in placeholders)
            {
                switch (place.Codigo)
                {
                    case ConstanCodigoVariablesPlaceHolders.DDP_FECHA:
                        pStrContenido = pStrContenido
                            .Replace(place.Nombre, pDisponibilidad.FechaCreacion.ToString("dd/MM/yyy"));
                        break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NUMERO_SOLICITUD:
                        pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.NumeroSolicitud); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NO:
                        pStrContenido =
                            pStrContenido.Replace(place.Nombre, pDisponibilidad.NumeroDdp); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_RUBRO_POR_FINANCIAR:
                        pStrContenido = pStrContenido.Replace(place.Nombre, _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal
                                && r.Codigo == pDisponibilidad.TipoSolicitudCodigo).FirstOrDefault().Descripcion); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TIPO_SOLICITUD:
                        pStrContenido =
                            pStrContenido.Replace(place.Nombre, pDisponibilidad.TipoSolicitudEspecialCodigo != null ? _context.Dominio.Where(x=>x.Codigo==pDisponibilidad.TipoSolicitudEspecialCodigo && x.TipoDominioId==(int)EnumeratorTipoDominio.Tipo_DDP_Espacial).FirstOrDefault().Nombre :
                    //si no viene el campo puede ser contratación
                    pDisponibilidad.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? "Proyecto administrativo" :
                    "Contratación"); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_OPCION_CONTRATAR: pStrContenido =
                            pStrContenido.Replace(place.Nombre, opcionContratarCodigo); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_TABLA_LIMITACION_ESPECIAL:
                        pStrContenido =
                            pStrContenido.Replace(place.Nombre, limitacionEspecial); break;
                        
                    case ConstanCodigoVariablesPlaceHolders.DDP_FECHA_COMITE_TECNICO: pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.FechaCreacion.ToString("dd/MM/yyyy")); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_NUMERO_COMITE: pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.NumeroSolicitud); break;
                    case ConstanCodigoVariablesPlaceHolders.DDP_OBJETO: pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.Objeto); break;
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
                    case ConstanCodigoVariablesPlaceHolders.NUMEROCONTRATO: pStrContenido = pStrContenido.Replace(place.Nombre,contrato==null?"": contrato.NumeroContrato); break;
                    case ConstanCodigoVariablesPlaceHolders.DRP_NO: pStrContenido = pStrContenido.Replace(place.Nombre, pDisponibilidad.NumeroDrp); break;
                    case ConstanCodigoVariablesPlaceHolders.FECHACONTRATO: pStrContenido = pStrContenido.Replace(place.Nombre, contrato == null ? "" : contrato.FechaFirmaContrato!=null?Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy"):""); break;
                    case ConstanCodigoVariablesPlaceHolders.TABLAFUENTES:
                        pStrContenido = pStrContenido.Replace(place.Nombre, tablafuentes); break;
                    case ConstanCodigoVariablesPlaceHolders.TABLAUSOS:
                        pStrContenido = pStrContenido.Replace(place.Nombre, tablauso); break;

                }
            }
            return Helpers.Helpers.HtmlEntities(pStrContenido);
        }

        /*autor: jflorez
            descripción: return disponibilidad por validacion pres
        impacto: CU 3.3.2*/

        public async Task<Respuesta> SetReturnValidacionDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion
            , string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Devuelta_por_validacion_presupuestal;
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion;
                DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();

                pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();
                _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                _context.SaveChanges();
                //envio correo a tecnico
                //////
                
                
                var usuarioTecnico = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario).FirstOrDefault();
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DDPDevolucion);
                string template = TemplateRecoveryPassword.Contenido;
                template = template.Replace("[NUMERODISPONIBILIDAD]", DisponibilidadCancelar.NumeroSolicitud).
                    Replace("_LinkF_", pDominioFront);
                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioTecnico.Usuario.Email, "SDP Devuelto por validación presupuestal", template, pSentender, pPassword, pMailServer, pMailPort);
                this.eliminarGestion(DisponibilidadCancelar);
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
        public async Task<Respuesta> SetRechazarValidacionDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion
            , string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(pDisponibilidadPresObservacion.DisponibilidadPresupuestalId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Rechazada_por_validacion_presupuestal;
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = pDisponibilidadPresObservacion.UsuarioCreacion;
                DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();

                pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();
                _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                _context.SaveChanges();
                //envio correo a técnico
                var usuarioTecnico = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario).FirstOrDefault();
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DDPRechazado);
                string template = TemplateRecoveryPassword.Contenido;
                template = template.Replace("[NUMERODISPONIBILIDAD]", DisponibilidadCancelar.NumeroSolicitud).
                    Replace("_LinkF_", pDominioFront);
                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioTecnico.Usuario.Email, "SDP rechazado por validación presupuestal", template, pSentender, pPassword, pMailServer, pMailPort);
                this.eliminarGestion(DisponibilidadCancelar);
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
        public async Task<Respuesta> SetValidarValidacionDDP(int id, string usuariomod
            , string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(id);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Con_validacion_presupuestal;
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = usuariomod;
                DisponibilidadCancelar.EstadoSolicitudCodigo = estado.ToString();
                /*
                pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                pDisponibilidadPresObservacion.EstadoSolicitudCodigo = estado.ToString();
                _context.DisponibilidadPresupuestalObservacion.Add(pDisponibilidadPresObservacion);
                _context.SaveChanges();*/
                //envio correo a juridica
                var usuarioJuridico = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Financiera).Include(y => y.Usuario).FirstOrDefault();
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DisponibilidadPresupuestalGenerada);
                string template = TemplateRecoveryPassword.Contenido.Replace("[NUMERODISPONIBILIDAD]",DisponibilidadCancelar.NumeroSolicitud).
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
                if(pDisponibilidadPresObservacion.GestionFuenteFinanciacionId>0)
                {
                    var gsertion = _context.GestionFuenteFinanciacion.Find(pDisponibilidadPresObservacion.GestionFuenteFinanciacionId);
                    decimal valoresSolicitados = 0;
                    if(gsertion.DisponibilidadPresupuestalId>0)
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
                this.eliminarGestion(DisponibilidadCancelar);
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
        public async Task<Respuesta> CreateDRP(int pId, string pUsuarioModificacion, string urlDestino, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Include(x => x.Contratacion).
                ThenInclude(x => x.ContratacionProyecto).ThenInclude(x => x.ContratacionProyectoAportante).ThenInclude(x => x.CofinanciacionAportante).
                ThenInclude(x => x.FuenteFinanciacion).FirstOrDefault(x => x.DisponibilidadPresupuestalId == pId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            int consecutivo = _context.DisponibilidadPresupuestal.Where(x => x.NumeroDrp != null).Count();
            try
            {
                int estado = (int)EnumeratorEstadoSolicitudPresupuestal.Con_registro_presupuestal;
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
                _context.SaveChanges();
                //envio correo a juridica
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DRPNotificacion);
                string template = TemplateRecoveryPassword.Contenido;

                template = template.Replace("_LinkF_", urlDestino);
                template = template.Replace("[NUMERODISPONIBILIDAD]", DisponibilidadCancelar.NumeroSolicitud);
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
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Registro_Presupuestal, ConstantMessagesGenerateBudget.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<byte[]> GetPDFDRP(int id, string usuarioModificacion)
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
            Plantilla plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Ficha_DRP).ToString()).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            string contenido = ReemplazarDatosDDP(plantilla.Contenido, disponibilidad, true);
            plantilla.Contenido = contenido;
            return ConvertirPDF(plantilla);
        }

        private string getNombreAportante(CofinanciacionAportante confinanciacion)
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
    }
}