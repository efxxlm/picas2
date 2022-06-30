using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Constants;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class DailyFollowUpService : IDailyFollowUpService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirementsConstructionPhaseService;
        private IHostingEnvironment _environment;
        private readonly IContractualControversy _contractualControversy;

        public DailyFollowUpService(devAsiVamosFFIEContext context,
                                    ICommonService commonService,
                                    ITechnicalRequirementsConstructionPhaseService technicalRequirementsConstructionPhaseService,
                                    IHostingEnvironment hostingEnvironment,
                                    IContractualControversy contractualControversy)
        {
            _context = context;
            _commonService = commonService;
            _technicalRequirementsConstructionPhaseService = technicalRequirementsConstructionPhaseService;
            _environment = hostingEnvironment;
            _contractualControversy = contractualControversy;
        }

        public async Task<List<VProyectosXcontrato>> gridRegisterDailyFollowUp(int usuarioId)
        {
            List<VProyectosXcontrato> listaInfoProyectos = await _context.VProyectosXcontrato
                                                                        .Where(
                                                                                r => r.FechaActaInicioFase2 <= DateTime.Now &&
                                                                                r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString() &&
                                                                                r.InterventorId == usuarioId
                                                                              )
                                                                        .ToListAsync();

            try
            {

                List<VProyectosXcontrato> ListvProyectosXcontratos = new List<VProyectosXcontrato>();

                foreach (var p in listaInfoProyectos)
                {
                    //no se muestran los que no tengan fase de construcción (requisitos)
                    int existe = _context.ContratoConstruccion.Where(r => r.ContratoId == p.ContratoId && r.ProyectoId == p.ProyectoId).Count();

                    List<SeguimientoDiario> listaSeguimientoDiario = _context.SeguimientoDiario
                                                                    .Where(s => s.ContratacionProyectoId == p.ContratacionProyectoId &&
                                                                           s.Eliminado != true)
                                                                    .OrderByDescending(r => r.FechaSeguimiento)
                                                                    .ToList();
                    //Nueva restricción control de cambios
                    p.CumpleCondicionesTai = _contractualControversy.ValidarCumpleTaiContratista(p.ContratoId, false, true, p.ProyectoId);

                    if (listaSeguimientoDiario.Count() > 0)
                    {
                        SeguimientoDiario seguimientoDiario = listaSeguimientoDiario.FirstOrDefault();

                        p.FechaUltimoSeguimientoDiario = seguimientoDiario.FechaSeguimiento;
                        p.SeguimientoDiarioId = seguimientoDiario.SeguimientoDiarioId;
                        p.RegistroCompleto = seguimientoDiario.RegistroCompleto.HasValue ? seguimientoDiario.RegistroCompleto.Value : false;
                        p.EstadoCodigo = seguimientoDiario.EstadoCodigo;

                        if (listaSeguimientoDiario.Where(sd => sd.EstadoCodigo == ConstanCodigoEstadoSeguimientoDiario.ConObservacionesEnviadas).Count() > 0)
                        {
                            SeguimientoDiario seguimientoTemp = listaSeguimientoDiario
                                                                    .Where(sd => sd.EstadoCodigo == ConstanCodigoEstadoSeguimientoDiario.ConObservacionesEnviadas)
                                                                    .FirstOrDefault();

                            p.TieneObservaciones = true;
                            p.RegistroCompletoTieneObservaciones = seguimientoTemp.RegistroCompleto;


                        }

                        if (listaSeguimientoDiario
                                .Where(x => x.EstadoCodigo != null &&
                                       x.EstadoCodigo != "0" &&
                                       x.EstadoCodigo != ConstanCodigoEstadoSeguimientoDiario.EnProcesoDeRegistro)
                                .Count() > 0)
                        {
                            p.MostrarBitacora = true;
                        }
                        if (existe > 0)
                        {
                            ListvProyectosXcontratos.Add(p);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return listaInfoProyectos;
        }

        public async Task<List<VProyectosXcontrato>> gridVerifyDailyFollowUp(int usuarioId)
        {
            List<Dominio> listaParametricas = _context.Dominio.Where(d => d.Activo == true).ToList();

            List<VProyectosXcontrato> listaInfoProyectos = await _context.VProyectosXcontrato
                                                                        .Where(r => r.FechaActaInicioFase2 <= DateTime.Now &&
                                                                               r.ApoyoId == usuarioId &&
                                                                               r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString()
                                                                               )
                                                                        .ToListAsync();

            //List<VProyectosXcontrato> listaSeguimientos = new List<VProyectosXcontrato>();

            listaInfoProyectos.ForEach(p =>
            {
                // Estados se puede editar
                SeguimientoDiario seguimientoDiario = _context.SeguimientoDiario
                                                                .Where(s => s.ContratacionProyectoId == p.ContratacionProyectoId &&
                                                                       s.Eliminado != true &&
                                                                           (s.EstadoCodigo == ConstanCodigoEstadoSeguimientoDiario.SeguimientoDiarioEnviado ||
                                                                             s.EstadoCodigo == ConstanCodigoEstadoSeguimientoDiario.RevisadoApoyo
                                                                           )
                                                                        )
                                                                .OrderBy(r => r.FechaSeguimiento)
                                                                .FirstOrDefault();

                // Estados mostrar ultimo sin editar
                SeguimientoDiario seguimientoDiarioEnviado = _context.SeguimientoDiario
                                                                .Where(s => s.ContratacionProyectoId == p.ContratacionProyectoId &&
                                                                       s.Eliminado != true &&
                                                                            (
                                                                                s.EstadoCodigo == ConstanCodigoEstadoSeguimientoDiario.EnviadoAsupervisor ||
                                                                                s.EstadoCodigo == ConstanCodigoEstadoSeguimientoDiario.EnProcesoDeValidación ||
                                                                                s.EstadoCodigo == ConstanCodigoEstadoSeguimientoDiario.ConObservacionesEnviadas ||
                                                                                s.EstadoCodigo == ConstanCodigoEstadoSeguimientoDiario.Aprobado
                                                                            )
                                                                        )
                                                                .OrderByDescending(r => r.FechaSeguimiento)
                                                                .FirstOrDefault();



                if (seguimientoDiario != null)
                {
                    p.FechaUltimoSeguimientoDiario = seguimientoDiario.FechaSeguimiento;
                    p.SeguimientoDiarioId = seguimientoDiario.SeguimientoDiarioId;
                    p.RegistroCompleto = seguimientoDiario.RegistroCompleto.HasValue ? seguimientoDiario.RegistroCompleto.Value : false;
                    p.EstadoCodigo = seguimientoDiario.EstadoCodigo;
                    p.EstadoNombre = listaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Seguimiento_Diario &&
                                                             r.Codigo == seguimientoDiario.EstadoCodigo)
                                                      .FirstOrDefault()?.Descripcion;

                    p.TieneAlertas = VerificarAlertas(seguimientoDiario);
                }
                else if (seguimientoDiarioEnviado != null)
                {
                    p.FechaUltimoSeguimientoDiario = seguimientoDiarioEnviado.FechaSeguimiento;
                    p.SeguimientoDiarioId = seguimientoDiarioEnviado.SeguimientoDiarioId;
                    p.RegistroCompleto = seguimientoDiarioEnviado.RegistroCompleto.HasValue ? seguimientoDiarioEnviado.RegistroCompleto.Value : false;
                    p.EstadoCodigo = seguimientoDiarioEnviado.EstadoCodigo;
                    p.EstadoNombre = listaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Seguimiento_Diario &&
                                                             r.Codigo == seguimientoDiarioEnviado.EstadoCodigo)
                                                      .FirstOrDefault()?.Descripcion;

                    p.TieneAlertas = VerificarAlertas(seguimientoDiarioEnviado);
                }

                if (seguimientoDiarioEnviado != null)
                {
                    p.MostrarBitacora = true;
                }

            });

            // filtro los que tiene registros
            listaInfoProyectos = listaInfoProyectos.Where(p => p.SeguimientoDiarioId > 0).ToList();

            //listaSeguimientos = listaSeguimientos.OrderBy(r => r.ProyectoId).OrderBy(r => r.FechaUltimoSeguimientoDiario).ToList();

            return listaInfoProyectos;
        }

        public async Task<List<VProyectosXcontrato>> gridValidateDailyFollowUp(int usuarioId)
        {
            List<Dominio> listaParametricas = _context.Dominio.Where(d => d.Activo == true).ToList();

            List<VProyectosXcontrato> listaInfoProyectos = await _context.VProyectosXcontrato
                                                                        .Where(r => r.FechaActaInicioFase2 <= DateTime.Now &&
                                                                               r.SupervisorId == usuarioId &&
                                                                               r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                                                                        .ToListAsync();

            listaInfoProyectos.ForEach(p =>
            {
                // Estados se puede editar
                SeguimientoDiario seguimientoDiario = _context.SeguimientoDiario
                                                                .Where(s => s.ContratacionProyectoId == p.ContratacionProyectoId &&
                                                                       s.Eliminado != true &&
                                                                           (
                                                                               s.EstadoCodigo == ConstanCodigoEstadoSeguimientoDiario.EnviadoAsupervisor ||
                                                                               s.EstadoCodigo == ConstanCodigoEstadoSeguimientoDiario.EnProcesoDeValidación

                                                                           )
                                                                        )
                                                                .OrderBy(r => r.FechaSeguimiento)
                                                                .FirstOrDefault();

                //Estados mostrar ultimo sin editar
                SeguimientoDiario seguimientoDiarioEnviado = _context.SeguimientoDiario
                                                                .Where(s => s.ContratacionProyectoId == p.ContratacionProyectoId &&
                                                                       s.Eliminado != true &&
                                                                           (
                                                                               s.EstadoCodigo == ConstanCodigoEstadoSeguimientoDiario.ConObservacionesEnviadas ||
                                                                               s.EstadoCodigo == ConstanCodigoEstadoSeguimientoDiario.Aprobado
                                                                           )
                                                                        )
                                                                .OrderByDescending(r => r.FechaSeguimiento).FirstOrDefault();


                if (seguimientoDiario != null)
                {
                    p.FechaUltimoSeguimientoDiario = seguimientoDiario.FechaSeguimiento;
                    p.SeguimientoDiarioId = seguimientoDiario.SeguimientoDiarioId;
                    p.RegistroCompleto = seguimientoDiario.RegistroCompleto.HasValue ? seguimientoDiario.RegistroCompleto.Value : false;
                    p.EstadoCodigo = seguimientoDiario.EstadoCodigo;
                    p.EstadoNombre = listaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Seguimiento_Diario &&
                                                             r.Codigo == seguimientoDiario.EstadoCodigo)
                                                      .FirstOrDefault()?.Nombre;

                    p.TieneAlertas = VerificarAlertas(seguimientoDiario);
                    p.TieneObservaciones = seguimientoDiario.TieneObservacionSupervisor;
                }
                else if (seguimientoDiarioEnviado != null)
                {
                    p.FechaUltimoSeguimientoDiario = seguimientoDiarioEnviado.FechaSeguimiento;
                    p.SeguimientoDiarioId = seguimientoDiarioEnviado.SeguimientoDiarioId;
                    p.RegistroCompleto = seguimientoDiarioEnviado.RegistroCompleto.HasValue ? seguimientoDiarioEnviado.RegistroCompleto.Value : false;
                    p.EstadoCodigo = seguimientoDiarioEnviado.EstadoCodigo;
                    p.EstadoNombre = listaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Seguimiento_Diario &&
                                                             r.Codigo == seguimientoDiarioEnviado.EstadoCodigo)
                                                      .FirstOrDefault()?.Nombre;

                    p.TieneAlertas = VerificarAlertas(seguimientoDiarioEnviado);
                }

                if (seguimientoDiarioEnviado != null)
                {
                    p.MostrarBitacora = true;
                }

            });


            // filtro los que tiene registros
            listaInfoProyectos = listaInfoProyectos.Where(p => p.SeguimientoDiarioId > 0).ToList();

            return listaInfoProyectos;
        }


        private bool VerificarAlertas(SeguimientoDiario pSeguimientoDiario)
        {
            bool tieneAlertas = false;

            if (
                    pSeguimientoDiario.DisponibilidadPersonal == false ||
                    pSeguimientoDiario.DisponibilidadMaterialCodigo == "2" ||
                    pSeguimientoDiario.DisponibilidadMaterialCodigo == "3" ||
                    pSeguimientoDiario.DisponibilidadEquipo == "2" ||
                    pSeguimientoDiario.DisponibilidadEquipo == "3" ||
                    pSeguimientoDiario.ProductividadCodigo == "3"

               )
            {
                tieneAlertas = true;
            }

            return tieneAlertas;
        }

        private bool VerificarRegistroCompleto(SeguimientoDiario pSeguimientoDiario)
        {
            bool completo = true;

            List<string> listaBajaDisponibilidadMaterial = new List<string> { { "2" }, { "3" } };
            List<string> listaBajaDisponibilidadEquipo = new List<string> { { "2" }, { "3" } };

            if (
                    pSeguimientoDiario.FechaSeguimiento == null ||

                    pSeguimientoDiario.DisponibilidadPersonal == null ||
                    string.IsNullOrEmpty(pSeguimientoDiario.DisponibilidadPersonalObservaciones) ||
                    (pSeguimientoDiario.DisponibilidadPersonal == false && pSeguimientoDiario.CantidadPersonalProgramado == null) ||
                    (pSeguimientoDiario.DisponibilidadPersonal == false && pSeguimientoDiario.CantidadPersonalTrabajando == null) ||
                    (pSeguimientoDiario.DisponibilidadPersonal == false && pSeguimientoDiario.SeGeneroRetrasoPersonal == null) ||
                    (pSeguimientoDiario.SeGeneroRetrasoPersonal == true && pSeguimientoDiario.NumeroHorasRetrasoPersonal == null) ||

                    string.IsNullOrEmpty(pSeguimientoDiario.DisponibilidadMaterialCodigo) ||
                    string.IsNullOrEmpty(pSeguimientoDiario.DisponibilidadMaterialObservaciones) ||
                    (listaBajaDisponibilidadMaterial.Where(r => r == pSeguimientoDiario.DisponibilidadMaterialCodigo).Count() > 0 &&
                        pSeguimientoDiario.CausaIndisponibilidadMaterialCodigo == null) ||
                    (listaBajaDisponibilidadMaterial.Where(r => r == pSeguimientoDiario.DisponibilidadMaterialCodigo).Count() > 0 &&
                        pSeguimientoDiario.SeGeneroRetrasoMaterial == null) ||
                    (pSeguimientoDiario.SeGeneroRetrasoMaterial == true && pSeguimientoDiario.NumeroHorasRetrasoMaterial == null) ||


                    string.IsNullOrEmpty(pSeguimientoDiario.DisponibilidadEquipoCodigo) ||
                    string.IsNullOrEmpty(pSeguimientoDiario.DisponibilidadEquipoObservaciones) ||
                    (listaBajaDisponibilidadEquipo.Where(r => r == pSeguimientoDiario.DisponibilidadEquipoCodigo).Count() > 0 &&
                        pSeguimientoDiario.CausaIndisponibilidadEquipoCodigo == null) ||
                    (listaBajaDisponibilidadEquipo.Where(r => r == pSeguimientoDiario.DisponibilidadEquipoCodigo).Count() > 0 &&
                        pSeguimientoDiario.SeGeneroRetrasoEquipo == null) ||
                    (pSeguimientoDiario.SeGeneroRetrasoEquipo == true && pSeguimientoDiario.NumeroHorasRetrasoEquipo == null) ||

                    string.IsNullOrEmpty(pSeguimientoDiario.ProductividadCodigo) ||
                    string.IsNullOrEmpty(pSeguimientoDiario.ProductividadObservaciones) ||
                    (pSeguimientoDiario.ProductividadCodigo == "3" && pSeguimientoDiario.CausaIndisponibilidadProductividadCodigo == null) ||
                    (pSeguimientoDiario.ProductividadCodigo == "3" && pSeguimientoDiario.SeGeneroRetrasoProductividad == null) ||
                    (pSeguimientoDiario.SeGeneroRetrasoProductividad == true && pSeguimientoDiario.NumeroHorasRetrasoProductividad == null)

               )
            {
                completo = false;
            }

            return completo;
        }

        public async Task<Respuesta> CreateEditDailyFollowUp(SeguimientoDiario pSeguimientoDiario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Create_Edit_Seguimiento_Diario, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            try
            {
                pSeguimientoDiario.EstadoCodigo = ConstanCodigoEstadoSeguimientoDiario.EnProcesoDeRegistro;
                SeguimientoSemanal seguimientoSemanal = new SeguimientoSemanal();

                seguimientoSemanal = _context.SeguimientoSemanal
                                                                .Where(
                                                                        r => r.ContratacionProyectoId == pSeguimientoDiario.ContratacionProyectoId &&
                                                                        r.FechaInicio.Value.Date <= pSeguimientoDiario.FechaSeguimiento.Date &&
                                                                        r.FechaFin.Value.Date >= pSeguimientoDiario.FechaSeguimiento.Date
                                                                        )
                                                                .FirstOrDefault();

                if (pSeguimientoDiario.SeguimientoDiarioId == 0)
                {
                    CreateEdit = "CREAR SEGUIMIENTO DIARIO";
                    pSeguimientoDiario.FechaCreacion = DateTime.Now;
                    pSeguimientoDiario.Eliminado = false;
                    pSeguimientoDiario.SeguimientoSemanalId = seguimientoSemanal.SeguimientoSemanalId;
                    pSeguimientoDiario.RegistroCompleto = VerificarRegistroCompleto(pSeguimientoDiario);

                    _context.SeguimientoDiario.Add(pSeguimientoDiario);
                }
                else
                {
                    CreateEdit = "CREAR SEGUIMIENTO DIARIO";
                    SeguimientoDiario seguimientoDiario = _context.SeguimientoDiario.Find(pSeguimientoDiario.SeguimientoDiarioId);

                    seguimientoDiario.FechaModificacion = DateTime.Now;
                    seguimientoDiario.UsuarioModificacion = pSeguimientoDiario.UsuarioCreacion;

                    seguimientoDiario.FechaSeguimiento = pSeguimientoDiario.FechaSeguimiento;
                    seguimientoDiario.DisponibilidadPersonal = pSeguimientoDiario.DisponibilidadPersonal;
                    seguimientoDiario.DisponibilidadPersonalObservaciones = pSeguimientoDiario.DisponibilidadPersonalObservaciones;
                    seguimientoDiario.CantidadPersonalProgramado = pSeguimientoDiario.CantidadPersonalProgramado;
                    seguimientoDiario.CantidadPersonalTrabajando = pSeguimientoDiario.CantidadPersonalTrabajando;
                    seguimientoDiario.SeGeneroRetrasoPersonal = pSeguimientoDiario.SeGeneroRetrasoPersonal;
                    seguimientoDiario.NumeroHorasRetrasoPersonal = pSeguimientoDiario.NumeroHorasRetrasoPersonal;
                    seguimientoDiario.DisponibilidadMaterialCodigo = pSeguimientoDiario.DisponibilidadMaterialCodigo;
                    seguimientoDiario.DisponibilidadMaterialObservaciones = pSeguimientoDiario.DisponibilidadMaterialObservaciones;
                    seguimientoDiario.CausaIndisponibilidadMaterialCodigo = pSeguimientoDiario.CausaIndisponibilidadMaterialCodigo;
                    seguimientoDiario.SeGeneroRetrasoMaterial = pSeguimientoDiario.SeGeneroRetrasoMaterial;
                    seguimientoDiario.NumeroHorasRetrasoMaterial = pSeguimientoDiario.NumeroHorasRetrasoMaterial;
                    seguimientoDiario.DisponibilidadEquipoCodigo = pSeguimientoDiario.DisponibilidadEquipoCodigo;
                    seguimientoDiario.DisponibilidadEquipoObservaciones = pSeguimientoDiario.DisponibilidadEquipoObservaciones;
                    seguimientoDiario.CausaIndisponibilidadEquipoCodigo = pSeguimientoDiario.CausaIndisponibilidadEquipoCodigo;
                    seguimientoDiario.SeGeneroRetrasoEquipo = pSeguimientoDiario.SeGeneroRetrasoEquipo;
                    seguimientoDiario.NumeroHorasRetrasoEquipo = pSeguimientoDiario.NumeroHorasRetrasoEquipo;
                    seguimientoDiario.ProductividadCodigo = pSeguimientoDiario.ProductividadCodigo;
                    seguimientoDiario.ProductividadObservaciones = pSeguimientoDiario.ProductividadObservaciones;
                    seguimientoDiario.CausaIndisponibilidadProductividadCodigo = pSeguimientoDiario.CausaIndisponibilidadProductividadCodigo;
                    seguimientoDiario.SeGeneroRetrasoProductividad = pSeguimientoDiario.SeGeneroRetrasoProductividad;
                    seguimientoDiario.NumeroHorasRetrasoProductividad = pSeguimientoDiario.NumeroHorasRetrasoProductividad;
                    seguimientoDiario.SeguimientoSemanalId = seguimientoSemanal.SeguimientoSemanalId;

                    seguimientoDiario.RegistroCompleto = VerificarRegistroCompleto(seguimientoDiario);

                }

                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.OperacionExitosa, idAccion, pSeguimientoDiario.UsuarioCreacion, CreateEdit)
                };
            }
            catch (Exception ex)
            {
                return
                  new Respuesta
                  {
                      IsSuccessful = false,
                      IsException = true,
                      IsValidation = false,
                      Code = GeneralCodes.Error,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.Error, idAccion, pSeguimientoDiario.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        private async Task<Respuesta> CreateEditObservacionSeguimientoDiario(SeguimientoDiarioObservaciones pObservacion, string pUsuarioCreacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Create_Edit_Observacion_Seguimiento_Diario, (int)EnumeratorTipoDominio.Acciones);

            Respuesta respuesta = new Respuesta();
            try
            {
                string strCrearEditar = "";
                if (pObservacion.SeguimientoDiarioObservacionesId > 0)
                {
                    strCrearEditar = "EDITAR OBSERVACION SEGUIMIENTO DIARIO";
                    SeguimientoDiarioObservaciones seguimientoDiarioObservaciones = _context.SeguimientoDiarioObservaciones.Find(pObservacion.SeguimientoDiarioObservacionesId);

                    seguimientoDiarioObservaciones.FechaModificacion = DateTime.Now;
                    seguimientoDiarioObservaciones.UsuarioModificacion = pUsuarioCreacion;

                    seguimientoDiarioObservaciones.Observaciones = pObservacion.Observaciones;

                }
                else
                {
                    strCrearEditar = "CREAR OBSERVACION SEGUIMIENTO DIARIO";

                    SeguimientoDiarioObservaciones seguimientoDiarioObservaciones = new SeguimientoDiarioObservaciones
                    {
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = pUsuarioCreacion,

                        SeguimientoDiarioId = pObservacion.SeguimientoDiarioId,
                        Observaciones = pObservacion.Observaciones,
                        EsSupervision = pObservacion.EsSupervision,
                    };

                    _context.SeguimientoDiarioObservaciones.Add(seguimientoDiarioObservaciones);
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                return
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = GeneralCodes.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_seguimiento_diario, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        private SeguimientoDiarioObservaciones getObservacion(SeguimientoDiario pSeguimientoDiario, bool pEsSupervicion)
        {
            SeguimientoDiarioObservaciones seguimientoDiarioObservaciones = pSeguimientoDiario.SeguimientoDiarioObservaciones.ToList()
                        .Where(r => r.EsSupervision == pEsSupervicion &&
                                    r.Archivado != true
                              )
                        .FirstOrDefault();

            return seguimientoDiarioObservaciones;
        }

        private async Task<bool> ValidarRegistroCompletoVerificacion(int id, bool pEsSupervicion)
        {
            bool esCompleto = true;

            SeguimientoDiario sd = await _context.SeguimientoDiario.Where(cc => cc.SeguimientoDiarioId == id)
                                                                .FirstOrDefaultAsync();


            sd.ObservacionApoyo = getObservacion(sd, pEsSupervicion);

            if (sd.TieneObservacionApoyo == null ||
                 (sd.TieneObservacionApoyo == true && string.IsNullOrEmpty(sd.ObservacionApoyo != null ? sd.ObservacionApoyo.Observaciones : null))
               )
            {
                esCompleto = false;
            }

            return esCompleto;
        }

        private async Task<bool> ValidarRegistroCompletoValidacion(int id, bool pEsSupervicion)
        {
            bool esCompleto = true;

            SeguimientoDiario sd = await _context.SeguimientoDiario.Where(cc => cc.SeguimientoDiarioId == id)
                                                                .FirstOrDefaultAsync();


            sd.ObservacionSupervisor = getObservacion(sd, pEsSupervicion);

            if (sd.ObservacionSupervisor == null ||
                 (sd.TieneObservacionSupervisor == true && string.IsNullOrEmpty(sd.ObservacionSupervisor != null ? sd.ObservacionSupervisor.Observaciones : null))
               )
            {
                esCompleto = false;
            }

            return esCompleto;
        }


        public async Task<Respuesta> CreateEditObservacion(SeguimientoDiario pSeguimientoDiario, bool esSupervisor)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Create_Edit_Observacion_Seguimiento_Diario, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";

            try
            {
                CreateEdit = "EDIT OBSERVACION SEGUIMIENTO DIARIO";
                int idObservacion = 0;

                if (pSeguimientoDiario.SeguimientoDiarioObservaciones.Count() > 0)
                    idObservacion = pSeguimientoDiario.SeguimientoDiarioObservaciones.FirstOrDefault().SeguimientoDiarioObservacionesId;

                SeguimientoDiario seguimientoDiario = _context.SeguimientoDiario.Find(pSeguimientoDiario.SeguimientoDiarioId);

                seguimientoDiario.UsuarioModificacion = pSeguimientoDiario.UsuarioCreacion;
                seguimientoDiario.FechaModificacion = DateTime.Now;

                if (esSupervisor)
                {
                    seguimientoDiario.EstadoCodigo = ConstanCodigoEstadoSeguimientoDiario.EnProcesoDeValidación;

                    seguimientoDiario.TieneObservacionSupervisor = pSeguimientoDiario.TieneObservacionSupervisor;

                    if (seguimientoDiario.TieneObservacionSupervisor.HasValue ? seguimientoDiario.TieneObservacionSupervisor.Value : false)
                    {

                        await CreateEditObservacionSeguimientoDiario(pSeguimientoDiario.SeguimientoDiarioObservaciones.FirstOrDefault(), pSeguimientoDiario.UsuarioCreacion);
                    }
                    else
                    {
                        SeguimientoDiarioObservaciones observacionDelete = _context.SeguimientoDiarioObservaciones.Find(idObservacion);

                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }

                    seguimientoDiario.RegistroCompletoValidacion = await ValidarRegistroCompletoValidacion(seguimientoDiario.SeguimientoDiarioId, esSupervisor);
                    if (seguimientoDiario.RegistroCompletoValidacion.Value)
                    {
                        seguimientoDiario.EstadoCodigo = ConstanCodigoEstadoSeguimientoDiario.EnProcesoDeValidación;
                    }
                    // else
                    // {
                    //     seguimientoDiario.EstadoCodigo = ConstanCodigoEstadoSeguimientoDiario.EnviadoAsupervisor;
                    // }

                }
                else
                {
                    seguimientoDiario.TieneObservacionApoyo = pSeguimientoDiario.TieneObservacionApoyo;

                    if (seguimientoDiario.TieneObservacionApoyo.HasValue ? seguimientoDiario.TieneObservacionApoyo.Value : true)
                    {
                        await CreateEditObservacionSeguimientoDiario(pSeguimientoDiario.SeguimientoDiarioObservaciones.FirstOrDefault(), pSeguimientoDiario.UsuarioCreacion);
                    }
                    else
                    {
                        SeguimientoDiarioObservaciones observacionDelete = _context.SeguimientoDiarioObservaciones.Find(idObservacion);

                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }

                    seguimientoDiario.RegistroCompletoVerificacion = await ValidarRegistroCompletoVerificacion(seguimientoDiario.SeguimientoDiarioId, esSupervisor);
                    if (seguimientoDiario.RegistroCompletoVerificacion.Value)
                    {
                        seguimientoDiario.EstadoCodigo = ConstanCodigoEstadoSeguimientoDiario.RevisadoApoyo;
                    }
                    else
                    {
                        seguimientoDiario.EstadoCodigo = ConstanCodigoEstadoSeguimientoDiario.SeguimientoDiarioEnviado;
                    }
                }





                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_seguimiento_diario, GeneralCodes.OperacionExitosa, idAccion, pSeguimientoDiario.UsuarioCreacion, CreateEdit)
                    };

            }
            catch (Exception ex)
            {
                return
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = GeneralCodes.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_seguimiento_diario, GeneralCodes.Error, idAccion, pSeguimientoDiario.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<SeguimientoDiario> GetDailyFollowUpById(int pId)
        {
            List<Dominio> listaParametricas = _context.Dominio.Where(d => d.Activo == true).ToList();

            SeguimientoDiario seguimiento = _context.SeguimientoDiario.Where(s => s.SeguimientoDiarioId == pId)
                                                                      .Include(r => r.SeguimientoDiarioObservaciones)
                                                                      .FirstOrDefault();

            seguimiento.DisponibilidadMaterialNombre = listaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Disponibilidad_Material &&
                                                                           r.Codigo == seguimiento.DisponibilidadMaterialCodigo)
                                                                .FirstOrDefault()?.Nombre;

            seguimiento.DisponibilidadEquipoNombre = listaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Disponibilidad_Equipo &&
                                                                           r.Codigo == seguimiento.DisponibilidadEquipoCodigo)
                                                                .FirstOrDefault()?.Nombre;

            seguimiento.ProductividadNombre = listaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Productividad &&
                                                                           r.Codigo == seguimiento.ProductividadCodigo)
                                                                .FirstOrDefault()?.Nombre;

            seguimiento.CausaBajaDisponibilidadMaterialNombre = listaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Causa_Baja_Disponibilidad_Material &&
                                                                           r.Codigo == seguimiento.CausaIndisponibilidadMaterialCodigo)
                                                                .FirstOrDefault()?.Nombre;

            seguimiento.CausaBajaDisponibilidadEquipoNombre = listaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Causa_Baja_Disponibilidad_Equipo &&
                                                                           r.Codigo == seguimiento.CausaIndisponibilidadEquipoCodigo)
                                                                .FirstOrDefault()?.Nombre;

            seguimiento.CausaBajaDisponibilidadProductividadNombre = listaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Causa_Baja_Disponibilidad_Productividad &&
                                                                           r.Codigo == seguimiento.CausaIndisponibilidadProductividadCodigo)
                                                                .FirstOrDefault()?.Nombre;

            seguimiento.ObservacionApoyo = getObservacion(seguimiento, false);
            seguimiento.ObservacionSupervisor = getObservacion(seguimiento, true);

            seguimiento.ObservacionDevolucion = _context.SeguimientoDiarioObservaciones.Find(seguimiento.ObservacionSupervisorId);

            return seguimiento;
        }

        public async Task<List<SeguimientoDiario>> GetDailyFollowUpByContratacionProyectoId(int pId)
        {
            List<Dominio> listaParametricas = _context.Dominio.Where(d => d.Activo == true).ToList();
            List<VProyectosXcontrato> listaProyectos = _context.VProyectosXcontrato.ToList();

            List<SeguimientoDiario> listaSeguimientos = _context.SeguimientoDiario
                                                                    .Where(r => r.ContratacionProyectoId == pId && r.Eliminado != true)
                                                                    .Include(r => r.ContratacionProyecto)
                                                                    .ToList();

            listaSeguimientos.ForEach(s =>
            {

                s.ContratacionProyecto.Proyecto = new Proyecto();
                s.ContratacionProyecto.Proyecto.InfoProyecto = listaProyectos.Where(r => r.ProyectoId == s.ContratacionProyecto.ProyectoId).FirstOrDefault();
                s.ProductividadNombre = listaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Productividad && r.Codigo == s.ProductividadCodigo).FirstOrDefault()?.Nombre;
                s.EstadoNombre = listaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Seguimiento_Diario && r.Codigo == s.EstadoCodigo).FirstOrDefault()?.Nombre;
                s.EstadoDescripcion = listaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Seguimiento_Diario && r.Codigo == s.EstadoCodigo).FirstOrDefault()?.Descripcion;
            });

            return listaSeguimientos;
        }

        public async Task<List<string>> GetDatesAvailableByContratacioProyectoId(int pId)
        {
            List<string> listaFechas = new List<string>();
            List<DateTime> listaFechasTotal = new List<DateTime>();
            List<string> listFechasSuspendidas = _context.VDiasSuspendidosNovedadXproyectoXcontrato.Where(r => r.ContratacionProyectoId == pId && r.FechaSuspendida != null).Select(x => ((DateTime)x.FechaSuspendida).ToShortDateString()).ToList();

            ContratacionProyecto contratacion = await _context.ContratacionProyecto
                                                                .Where(r => r.ContratacionProyectoId == pId)
                                                                .Include(r => r.Proyecto)
                                                                    .ThenInclude(r => r.ContratoConstruccion)
                                                                .Include(r => r.Contratacion)
                                                                    .ThenInclude(r => r.Contrato)
                                                                .Include(r => r.SeguimientoDiario)
                                                                .FirstOrDefaultAsync();

            Proyecto proyectoTemp = _technicalRequirementsConstructionPhaseService.CalcularFechaInicioContratoFase2(contratacion.Proyecto.ContratoConstruccion.FirstOrDefault().ContratoConstruccionId);
            VContratoProyectoFechaEstimadaFinalizacion datosFechas = _context.VContratoProyectoFechaEstimadaFinalizacion.Where(r => r.ContratacionProyectoId == pId).FirstOrDefault();

            DateTime fechaInicial = datosFechas != null ? datosFechas.FechaInicioProyecto ?? proyectoTemp.FechaInicioEtapaObra : proyectoTemp.FechaInicioEtapaObra;
            //SE TOMA FechaEstimadaFinProyectoRep, LAS NOVEDADES APROBADAS X REPROGRAMACIÓN -- NO NOVEDADES COMO EN LAS OTRAS PANTALLAS
            DateTime fechaFin = datosFechas != null ? datosFechas.FechaEstimadaFinProyectoRep ?? datosFechas.FechaFinProyecto ?? proyectoTemp.FechaFinEtapaObra : proyectoTemp.FechaFinEtapaObra;

            while (fechaFin >= fechaInicial)
            {
                listaFechasTotal.Add(fechaInicial);
                fechaInicial = fechaInicial.AddDays(1);
            }

            {
                if (listaFechasTotal.Count() > 0)
                {
                    bool fechaSeleccionada = false;
                    string ultimaFecha = string.Empty;

                    foreach (var f in listaFechasTotal.OrderBy(r => r.Date).ToList())
                    {
                        if (contratacion.SeguimientoDiario.Where(s => s.FechaSeguimiento.ToShortDateString() == f.ToShortDateString() && s.Eliminado != true)
                          .Count() == 0 && fechaSeleccionada == false && !listFechasSuspendidas.Contains(f.ToShortDateString()))
                        {
                            ultimaFecha = f.ToString("d/M/yyyy", CultureInfo.InvariantCulture);
                            fechaSeleccionada = true;
                            break;
                        }
                    }

                    listaFechas.Add(ultimaFecha);
                }
            }





            return listaFechas;
        }

        public async Task<Respuesta> DeleteDailyFollowUp(int pId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Seguimiento_Diario, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SeguimientoDiario seguimientoDiario = _context.SeguimientoDiario.Find(pId);

                seguimientoDiario.UsuarioModificacion = pUsuario;
                seguimientoDiario.FechaModificacion = DateTime.Now;
                seguimientoDiario.Eliminado = true;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.EliminacionExitosa, idAccion, pUsuario, "ELIMINAR SEGUIMIENTO DIARIO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> SendToSupervisionSupport(int pId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_Seguimiento_Diario_A_Apoyo, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SeguimientoDiario seguimientoDiario = _context.SeguimientoDiario.Find(pId);

                seguimientoDiario.UsuarioModificacion = pUsuario;
                seguimientoDiario.FechaModificacion = DateTime.Now;

                seguimientoDiario.EstadoCodigo = ConstanCodigoEstadoSeguimientoDiario.SeguimientoDiarioEnviado;
                seguimientoDiario.FechaVerificacion = DateTime.Now;

                _context.SaveChanges();

                EnviarNotificacionAApoyo(seguimientoDiario);

                VerificarDiasConsecutivosNegativos(seguimientoDiario);

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "ELIMINAR SEGUIMIENTO DIARIO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> SendToSupervision(int pId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_Seguimiento_Diario_A_Apoyo, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SeguimientoDiario seguimientoDiario = _context.SeguimientoDiario.Find(pId);

                seguimientoDiario.UsuarioModificacion = pUsuario;
                seguimientoDiario.FechaModificacion = DateTime.Now;
                seguimientoDiario.EstadoCodigo = ConstanCodigoEstadoSeguimientoDiario.EnviadoAsupervisor;

                _context.SaveChanges();

                EnviarNotificacionASupervisor(seguimientoDiario);

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "ELIMINAR SEGUIMIENTO DIARIO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> ApproveDailyFollowUp(int pId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Aprobar_Seguimiento_Diario, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SeguimientoDiario seguimientoDiario = _context.SeguimientoDiario.Find(pId);

                seguimientoDiario.UsuarioModificacion = pUsuario;
                seguimientoDiario.FechaModificacion = DateTime.Now;

                seguimientoDiario.EstadoCodigo = ConstanCodigoEstadoSeguimientoDiario.Aprobado;
                seguimientoDiario.FechaValidacion = DateTime.Now;
                seguimientoDiario.ObservacionSupervisorId = null;

                _context.SaveChanges();

                EnviarNotificacionSeguimientoValidado(seguimientoDiario);

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "ELIMINAR SEGUIMIENTO DIARIO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> ReturnToComptroller(int pId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Devolver_Seguimiento_Diario, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SeguimientoDiario seguimientoDiario = _context.SeguimientoDiario
                                                                    .Where(sd => sd.SeguimientoDiarioId == pId)
                                                                    .Include(r => r.SeguimientoDiarioObservaciones)
                                                                    .FirstOrDefault();

                seguimientoDiario.UsuarioModificacion = pUsuario;
                seguimientoDiario.FechaModificacion = DateTime.Now;

                if (seguimientoDiario.TieneObservacionApoyo == true)
                {

                    SeguimientoDiarioObservaciones observacionesApoyo = getObservacion(seguimientoDiario, false);

                    if (observacionesApoyo != null)
                        observacionesApoyo.Archivado = true;

                }

                if (seguimientoDiario.TieneObservacionSupervisor == true)
                {

                    SeguimientoDiarioObservaciones observacionesSupervisor = getObservacion(seguimientoDiario, true);

                    if (observacionesSupervisor != null)
                    {
                        observacionesSupervisor.Archivado = true;
                        seguimientoDiario.ObservacionSupervisorId = observacionesSupervisor.SeguimientoDiarioObservacionesId;
                    }

                }

                seguimientoDiario.EstadoCodigo = ConstanCodigoEstadoSeguimientoDiario.ConObservacionesEnviadas;
                seguimientoDiario.TieneObservacionApoyo = null;
                seguimientoDiario.TieneObservacionSupervisor = null;
                seguimientoDiario.RegistroCompleto = null;
                seguimientoDiario.RegistroCompletoValidacion = null;
                seguimientoDiario.RegistroCompletoVerificacion = null;

                _context.SaveChanges();

                EnviarNotificacionSeguimientoDevuelto(seguimientoDiario);

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "ELIMINAR SEGUIMIENTO DIARIO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }

        private async void EnviarNotificacionAApoyo(SeguimientoDiario seguimientoDiario)
        {
            VProyectosXcontrato proyecto = _context.VProyectosXcontrato
                                                        .Where(x => x.ContratacionProyectoId == seguimientoDiario.ContratacionProyectoId)
                                                        .AsNoTracking()
                                                        .FirstOrDefault();

            Contrato contrato = _context.Contrato
                                            .Where(x => x.ContratoId == proyecto.ContratoId)
                                            .Include(x => x.Apoyo)
                                            .FirstOrDefault();

            Template templateEnviar = _context.Template
                                                .Where(r => r.TemplateId == (int)enumeratorTemplate.EnviarSeguimientoDiarioAApoyo && r.Activo == true)
                                                .FirstOrDefault();

            string template = templateEnviar.Contenido.Replace("[LLAVE_MEN]", proyecto.LlaveMen)
                                                      .Replace("[NUMERO_CONTRATO]", proyecto.NumeroContrato)
                                                      .Replace("[INSTITUCION_EDUCATIVA]", proyecto.InstitucionEducativa)
                                                      .Replace("[SEDE]", proyecto.Sede)
                                                      .Replace("[TIPO_INTERVENCION]", proyecto.TipoIntervencion)
                                                      .Replace("[FECHA_ULTIMO_REPORTE]", seguimientoDiario.FechaSeguimiento.ToString("dd/MM/yyyy"));
            ;
            //List<Usuario> ListUsuarios = await _commonService.GetUsuariosByPerfil((int)EnumeratorPerfil.Miembros_Comite);
            List<string> listaMails = new List<string> { contrato?.Apoyo?.Email };
            _commonService.EnviarCorreo(listaMails, template, "Seguimiento Diario Enviado");


        }

        private async void EnviarNotificacionASupervisor(SeguimientoDiario seguimientoDiario)
        {
            VProyectosXcontrato proyecto = _context.VProyectosXcontrato
                                                        .Where(x => x.ContratacionProyectoId == seguimientoDiario.ContratacionProyectoId)
                                                        .AsNoTracking()
                                                        .FirstOrDefault();

            Contrato contrato = _context.Contrato
                                            .Where(x => x.ContratoId == proyecto.ContratoId)
                                            .Include(x => x.Supervisor)
                                            .FirstOrDefault();

            Template templateEnviar = _context.Template
                                                .Where(r => r.TemplateId == (int)enumeratorTemplate.EnviarSeguimientoDiarioASupervisor && r.Activo == true)
                                                .FirstOrDefault();

            string template = templateEnviar.Contenido.Replace("[LLAVE_MEN]", proyecto.LlaveMen)
                                                      .Replace("[NUMERO_CONTRATO]", proyecto.NumeroContrato)
                                                      .Replace("[INSTITUCION_EDUCATIVA]", proyecto.InstitucionEducativa)
                                                      .Replace("[SEDE]", proyecto.Sede)
                                                      .Replace("[TIPO_INTERVENCION]", proyecto.TipoIntervencion)
                                                      .Replace("[FECHA_ULTIMO_REPORTE]", seguimientoDiario.FechaSeguimiento.ToString("dd/MM/yyyy"));
            ;
            //List<Usuario> ListUsuarios = await _commonService.GetUsuariosByPerfil((int)EnumeratorPerfil.Miembros_Comite);
            List<string> listaMails = new List<string> { contrato?.Supervisor?.Email };
            _commonService.EnviarCorreo(listaMails, template, "Seguimiento Diario Enviado");


        }

        private async void EnviarNotificacionSeguimientoValidado(SeguimientoDiario seguimientoDiario)
        {
            VProyectosXcontrato proyecto = _context.VProyectosXcontrato
                                                        .Where(x => x.ContratacionProyectoId == seguimientoDiario.ContratacionProyectoId)
                                                        .AsNoTracking()
                                                        .FirstOrDefault();

            Contrato contrato = _context.Contrato
                                            .Where(x => x.ContratoId == proyecto.ContratoId)
                                            .Include(x => x.Interventor)
                                            .FirstOrDefault();

            Template templateEnviar = _context.Template
                                                .Where(r => r.TemplateId == (int)enumeratorTemplate.SeguimientoDiarioValidado && r.Activo == true)
                                                .FirstOrDefault();

            string template = templateEnviar.Contenido.Replace("[LLAVE_MEN]", proyecto.LlaveMen)
                                                      .Replace("[NUMERO_CONTRATO]", proyecto.NumeroContrato)
                                                      .Replace("[INSTITUCION_EDUCATIVA]", proyecto.InstitucionEducativa)
                                                      .Replace("[SEDE]", proyecto.Sede)
                                                      .Replace("[TIPO_INTERVENCION]", proyecto.TipoIntervencion)
                                                      .Replace("[FECHA_ULTIMO_REPORTE]", seguimientoDiario.FechaSeguimiento.ToString("dd/MM/yyyy"));

            List<string> listaMails = new List<string> { contrato?.Interventor?.Email };
            _commonService.EnviarCorreo(listaMails, template, "Seguimiento Diario Validado");


        }

        private async void EnviarNotificacionSeguimientoDevuelto(SeguimientoDiario seguimientoDiario)
        {
            VProyectosXcontrato proyecto = _context.VProyectosXcontrato
                                                        .Where(x => x.ContratacionProyectoId == seguimientoDiario.ContratacionProyectoId)
                                                        .AsNoTracking()
                                                        .FirstOrDefault();

            Contrato contrato = _context.Contrato
                                            .Where(x => x.ContratoId == proyecto.ContratoId)
                                            .Include(x => x.Interventor)
                                            .FirstOrDefault();

            Template templateEnviar = _context.Template
                                                .Where(r => r.TemplateId == (int)enumeratorTemplate.SeguimientoDiarioDevuelto && r.Activo == true)
                                                .FirstOrDefault();

            string template = templateEnviar.Contenido.Replace("[LLAVE_MEN]", proyecto.LlaveMen)
                                                      .Replace("[NUMERO_CONTRATO]", proyecto.NumeroContrato)
                                                      .Replace("[INSTITUCION_EDUCATIVA]", proyecto.InstitucionEducativa)
                                                      .Replace("[SEDE]", proyecto.Sede)
                                                      .Replace("[TIPO_INTERVENCION]", proyecto.TipoIntervencion)
                                                      .Replace("[FECHA_REPORTE]", seguimientoDiario.FechaSeguimiento.ToString("dd/MM/yyyy"));


            List<string> listaMails = new List<string> { contrato?.Interventor?.Email };
            _commonService.EnviarCorreo(listaMails, template, "Seguimiento Diario Devuelto");


        }

        private async void EnviarNotificacionSeguimientoDiasNegativos(SeguimientoDiario seguimientoDiario)
        {
            VProyectosXcontrato proyecto = _context.VProyectosXcontrato
                                                        .Where(x => x.ContratacionProyectoId == seguimientoDiario.ContratacionProyectoId)
                                                        .AsNoTracking()
                                                        .FirstOrDefault();

            Contrato contrato = _context.Contrato
                                            .Where(x => x.ContratoId == proyecto.ContratoId)
                                            .Include(x => x.Apoyo)
                                            .Include(x => x.Supervisor)
                                            .FirstOrDefault();

            Template templateEnviar = _context.Template
                                                .Where(r => r.TemplateId == (int)enumeratorTemplate.NotificacionTresDiasNegativosSeguimientoDiario && r.Activo == true)
                                                .FirstOrDefault();

            string template = templateEnviar.Contenido.Replace("[LLAVE_MEN]", proyecto.LlaveMen)
                                                      .Replace("[NUMERO_CONTRATO]", proyecto.NumeroContrato)
                                                      .Replace("[INSTITUCION_EDUCATIVA]", proyecto.InstitucionEducativa)
                                                      .Replace("[SEDE]", proyecto.Sede)
                                                      .Replace("[TIPO_INTERVENCION]", proyecto.TipoIntervencion)
                                                      .Replace("[FECHA_ULTIMO_REPORTE]", seguimientoDiario.FechaSeguimiento.ToString("dd/MM/yyyy"));


            List<string> listaMails = new List<string> { contrato?.Apoyo?.Email, contrato?.Supervisor?.Email };
            _commonService.EnviarCorreo(listaMails, template, "Seguimiento Diario - días consecutivos negativos");


        }

        private async void VerificarDiasConsecutivosNegativos(SeguimientoDiario seguimientoDiario)
        {
            List<SeguimientoDiario> seguimientos = _context.SeguimientoDiario
                                                                .Where(r => r.ContratacionProyectoId == seguimientoDiario.ContratacionProyectoId)
                                                                .ToList();
            int cantidad = 0;
            for (int i = 0; i < 3; i++)
            {
                SeguimientoDiario seguimiento = seguimientos
                                                    .Where(r => r.FechaSeguimiento.Date == seguimientoDiario.FechaSeguimiento.AddDays(-i).Date)
                                                    .FirstOrDefault();
                if (seguimiento != null && VerificarAlertas(seguimientoDiario))
                {
                    cantidad++;
                }
            }

            if (cantidad == 3)
            {
                EnviarNotificacionSeguimientoDiasNegativos(seguimientoDiario);

            }

        }
    }
}
