using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;
using asivamosffie.services.Helpers.Constants;

namespace asivamosffie.services
{
    public class ReprogrammingService : IReprogrammingService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirementsConstructionPhaseService;

        public ReprogrammingService(devAsiVamosFFIEContext context,ICommonService commonService, ITechnicalRequirementsConstructionPhaseService technicalRequirementsConstructionPhaseService)
        {
            _context = context;
            _commonService = commonService;
            _technicalRequirementsConstructionPhaseService = technicalRequirementsConstructionPhaseService;
        }

        #region GET
            public async Task<List<VAjusteProgramacion>> GetAjusteProgramacionGrid()
            {
                List<VAjusteProgramacion> ajustes = _context.VAjusteProgramacion.ToList();

                return ajustes.OrderBy(x => x.AjusteProgramacionId)
                                .ToList();
            }

            public async Task<AjusteProgramacion> GetAjusteProgramacionById(int pAjusteProgramacionId)
            {
                AjusteProgramacion ajusteProgramacion = await _context.AjusteProgramacion
                                                                    .Include(x => x.AjustePragramacionObservacion)
                                                                    .FirstOrDefaultAsync(x => x.AjusteProgramacionId == pAjusteProgramacionId);

                ajusteProgramacion.ObservacionObra = getObservacion(ajusteProgramacion, true);
                ajusteProgramacion.ObservacionFlujo = getObservacion(ajusteProgramacion, false);

                return ajusteProgramacion;
            }

            private AjustePragramacionObservacion getObservacion(AjusteProgramacion pAjusteProgramacion, bool? pEsObra)
            {
                AjustePragramacionObservacion ajustePragramacionObservacion = pAjusteProgramacion.AjustePragramacionObservacion.ToList()
                            .Where(r =>
                                        r.Archivada != true &&
                                        r.Eliminado != true &&
                                        r.EsObra == pEsObra
                                    )
                            .FirstOrDefault();

                return ajustePragramacionObservacion;
            }

            public async Task<List<ArchivoCargue>> GetLoadAdjustProgrammingGrid(int pAjusteProgramacionId)
            {
                List<ArchivoCargue> listaCargas = _context.ArchivoCargue
                                                                .Where(a => a.ReferenciaId == pAjusteProgramacionId &&
                                                                            a.Eliminado != true &&
                                                                            a.OrigenId == int.Parse(OrigenArchivoCargue.AjusteProgramacionObra))
                                                                .ToList();


                listaCargas.ForEach(archivo =>
                {
                    archivo.estadoCargue = archivo.CantidadRegistros == archivo.CantidadRegistrosValidos ? "Válido" : "Fallido";
                    archivo.TempAjustePragramacionObservacion = _context.AjustePragramacionObservacion.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId && r.EsObra == true && r.ArchivoCargueId == archivo.ArchivoCargueId && r.Eliminado != true).FirstOrDefault();

                });

                return listaCargas;

            }

            public async Task<List<ArchivoCargue>> GetLoadAdjustInvestmentFlowGrid(int pAjusteProgramacionId)
            {
                List<ArchivoCargue> listaCargas = _context.ArchivoCargue
                                                                .Where(a => a.ReferenciaId == pAjusteProgramacionId &&
                                                                       a.Eliminado != true &&
                                                                       a.OrigenId == int.Parse(OrigenArchivoCargue.AjusteFlujoInversion))
                                                                .ToList();


                listaCargas.ForEach(archivo =>
                {
                    archivo.estadoCargue = archivo.CantidadRegistros == archivo.CantidadRegistrosValidos ? "Válido" : "Fallido";
                    archivo.TempAjustePragramacionObservacion = _context.AjustePragramacionObservacion.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId && r.EsObra != true && r.ArchivoCargueId == archivo.ArchivoCargueId && r.Eliminado != true).FirstOrDefault();

                });

                return listaCargas;


            }

        #endregion

        #region VALIDACIONES
        private async Task<bool> ValidarRegistroCompletoValidacionAjusteProgramacion(int id)
            {
                bool esCompleto = true;

                AjusteProgramacion ap = await _context.AjusteProgramacion
                                                            .Include(cc => cc.AjustePragramacionObservacion)
                                                            .FirstOrDefaultAsync(cc => cc.AjusteProgramacionId == id);


                ap.ObservacionObra = getObservacion(ap, true);
                ap.ObservacionFlujo = getObservacion(ap, false);

                if (
                     ap.TieneObservacionesProgramacionObra == null ||
                     (ap.TieneObservacionesProgramacionObra == true && string.IsNullOrEmpty(ap.ObservacionObra != null ? ap.ObservacionObra.Observaciones : null)) ||
                     ap.TieneObservacionesFlujoInversion == null ||
                     (ap.TieneObservacionesFlujoInversion == true && string.IsNullOrEmpty(ap.ObservacionFlujo != null ? ap.ObservacionFlujo.Observaciones : null))

                   )
                {
                    esCompleto = false;
                }

                return esCompleto;
            }

            private void VerificarRegistroCompletoAjusteProgramacion(int pAjusteProgramacionId)
            {
                bool esCompleto = true;

                AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacionId);

                if (
                        ajusteProgramacion.ArchivoCargueIdProgramacionObra == null ||
                        ajusteProgramacion.ArchivoCargueIdFlujoInversion == null
                    )
                {
                    esCompleto = false;
                }

                ajusteProgramacion.RegistroCompleto = esCompleto;

                _context.SaveChanges();
            }

        #endregion

        #region CRUD
        private async Task<Respuesta> CreateEditObservacionAjusteProgramacion(AjustePragramacionObservacion pObservacion, string pUsuarioCreacion, bool esObra)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Observacion_Ajuste_Programacion, (int)EnumeratorTipoDominio.Acciones);

            Respuesta respuesta = new Respuesta();
            try
            {
                string strCrearEditar = "";
                if (pObservacion.AjustePragramacionObservacionId > 0)
                {
                    strCrearEditar = "EDITAR OBSERVACION AJUSTE PROGRAMACION";
                    AjustePragramacionObservacion ajustePragramacionObservacion = _context.AjustePragramacionObservacion.Find(pObservacion.AjustePragramacionObservacionId);

                    ajustePragramacionObservacion.FechaModificacion = DateTime.Now;
                    ajustePragramacionObservacion.UsuarioModificacion = pUsuarioCreacion;

                    ajustePragramacionObservacion.Observaciones = pObservacion.Observaciones;

                }
                else
                {
                    strCrearEditar = "CREAR OBSERVACION AJUSTE PROGRAMACION";
                    pObservacion.FechaCreacion = DateTime.Now;
                    pObservacion.UsuarioCreacion = pUsuarioCreacion;
                    _context.AjustePragramacionObservacion.Add(pObservacion);

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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> CreateEditObservacionAjusteProgramacion(AjusteProgramacion pAjusteProgramacion, bool esObra, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Observacion_Ajuste_Programacion, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";

            try
            {
                CreateEdit = "AJUSTE AJUSTE PROGRAMACION";
                int idObservacion = 0;

                if (pAjusteProgramacion.AjustePragramacionObservacion.Count() > 0)
                    idObservacion = pAjusteProgramacion.AjustePragramacionObservacion.FirstOrDefault().AjusteProgramacionId.Value;

                AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacion.AjusteProgramacionId);

                //ajusteProgramacion.UsuarioModificacion = pUsuario;
                //ajusteProgramacion.FechaModificacion = DateTime.Now;

                //ajusteProgramacion.EstadoCodigo = ConstanCodigoEstadoAjusteProgramacion.En_proceso_de_validacion_a_la_programacion;

                if (esObra == true)
                {
                    ajusteProgramacion.TieneObservacionesProgramacionObra = pAjusteProgramacion.TieneObservacionesProgramacionObra;

                    if (ajusteProgramacion.TieneObservacionesProgramacionObra.HasValue ? ajusteProgramacion.TieneObservacionesProgramacionObra.Value : false)
                    {

                        await CreateEditObservacionAjusteProgramacion(pAjusteProgramacion.AjustePragramacionObservacion.FirstOrDefault(), pUsuario, esObra);
                    }
                    else
                    {
                        AjustePragramacionObservacion observacionDelete = _context.AjustePragramacionObservacion.Find(idObservacion);

                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }

                }
                else
                {
                    ajusteProgramacion.TieneObservacionesFlujoInversion = pAjusteProgramacion.TieneObservacionesFlujoInversion;

                    if (ajusteProgramacion.TieneObservacionesFlujoInversion.HasValue ? ajusteProgramacion.TieneObservacionesFlujoInversion.Value : false)
                    {

                        await CreateEditObservacionAjusteProgramacion(pAjusteProgramacion.AjustePragramacionObservacion.FirstOrDefault(), pUsuario, esObra);
                    }
                    else
                    {
                        AjustePragramacionObservacion observacionDelete = _context.AjustePragramacionObservacion.Find(idObservacion);

                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }

                }

                ajusteProgramacion.RegistroCompletoValidacion = await ValidarRegistroCompletoValidacionAjusteProgramacion(ajusteProgramacion.AjusteProgramacionId);
                /*if (ajusteProgramacion.RegistroCompletoValidacion)
                {
                    ajusteProgramacion.EstadoCodigo = ConstanCodigoEstadoAjusteProgramacion.;
                }
                else
                {
                    ajusteProgramacion.EstadoCodigo = ConstanCodigoEstadoAjusteProgramacion.Enviada_al_supervisor;
                }*/
                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.OperacionExitosa, idAccion, pUsuario, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> DeleteAdjustProgrammingOrInvestmentFlow(int pArchivoCargueId, int pAjusteProgramacionId,string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Archivo_Cargue, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                string strCrearEditar = string.Empty;

                if (pArchivoCargueId > 0)
                {
                    strCrearEditar = "ELIMINAR ARCHIVO CARGE - AJUSTE PROGRAMACIÓN OBRA";

                    await _context.Set<ArchivoCargue>().Where(r => r.ArchivoCargueId == pArchivoCargueId)
                                               .UpdateAsync(r => new ArchivoCargue()
                                               {
                                                   FechaModificacion = DateTime.Now,
                                                   UsuarioModificacion = pUsuario,
                                                   Eliminado = true
                                               });


                    await _context.Set<AjustePragramacionObservacion>().Where(r => r.ArchivoCargueId == pArchivoCargueId)
                                               .UpdateAsync(r => new AjustePragramacionObservacion()
                                               {
                                                   FechaModificacion = DateTime.Now,
                                                   UsuarioModificacion = pUsuario,
                                                   Eliminado = true,
                                                   AjusteProgramacionId = pAjusteProgramacionId
                                               });

                    //se pueden borrar , no es necesario dejar el registro.

                    List<AjusteProgramacionObra> listaAjusteProgramacionObra = _context.AjusteProgramacionObra.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId).ToList();
                    _context.AjusteProgramacionObra.RemoveRange(listaAjusteProgramacionObra);

                    List<TempProgramacion> listaTempAjusteProgramacionObra = _context.TempProgramacion.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId).ToList();
                    _context.TempProgramacion.RemoveRange(listaTempAjusteProgramacionObra);

                    List<AjusteProgramacionFlujo> listaAjusteProgramacionFlujo = _context.AjusteProgramacionFlujo.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId).ToList();
                    _context.AjusteProgramacionFlujo.RemoveRange(listaAjusteProgramacionFlujo);

                    bool state = await ValidarRegistroCompletoValidacionAjusteProgramacion(pAjusteProgramacionId);
                    AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacionId);
                    if (ajusteProgramacion != null)
                    {
                        ajusteProgramacion.FechaModificacion = DateTime.Now;
                        ajusteProgramacion.UsuarioModificacion = pUsuario;
                        ajusteProgramacion.RegistroCompletoValidacion = state;
                        ajusteProgramacion.AjusteProgramacionId = pAjusteProgramacionId;
                        _context.AjusteProgramacion.Update(ajusteProgramacion);
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.OperacionExitosa, idAccion, pUsuario, strCrearEditar)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> EnviarAlSupervisorAjusteProgramacion(int pAjusteProgramacionId, string pUsuarioCreacion, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.EnviarAlSupervisorAjusteProgramacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //Contrato contrato = _context.Contrato.Where(c => c.ContratoId == pContratoId).Include(x => x.Contratacion).FirstOrDefault();
                AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacionId);
                //envio correo
                //envio correo a supervisor
                //Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.AprobarRequisitosTecnicosFase2);

                //string ncontrato = "";
                //string fechaContrato = "";
                //string template = TemplateRecoveryPassword.Contenido.
                //    Replace("[NUMEROCONTRATO]", contrato.NumeroContrato).
                //    Replace("_LinkF_", pDominioFront).
                //    Replace("[FECHAVERIFICACION]", DateTime.Now.ToString("dd/MM/yyyy")).
                //    Replace("[CANTIDADPROYECTOSASOCIADOS]", _context.ContratoConstruccion.Where(x => x.RegistroCompleto == true && x.ContratoId == contrato.ContratoId).Count().ToString()).
                //    Replace("[CANTIDADPROYECTOSVERIFICADOS]", _context.ContratoConstruccion.Where(x => x.RegistroCompleto == true && x.ContratoId == contrato.ContratoId).Count().ToString()).
                //    Replace("[TIPOCONTRATO]", contrato.Contratacion.TipoSolicitudCodigo == "1" ? "obra" : "interventoría");//OBRA O INTERVENTORIA


                //var usuariosadmin = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Apoyo).Include(y => y.Usuario).ToList();
                //foreach (var usuarioadmin in usuariosadmin)
                //{
                //    bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioadmin.Usuario.Email, "Aprobacion de requisitos técnicos de inicio para fase 2-construcción", template, pSender, pPassword, pMailServer, pMailPort);
                //}

                //Contrato contrato = _context.Contrato.Find(pContratoId);
                //jflorez, este evento solo sucede cuando esta completo y se aprueban los requisitos, por ello seteo el dato 20201202
                //contrato.FechaAprobacionRequisitosConstruccionInterventor = DateTime.Now;

                //ajusteProgramacion.UsuarioModificacion = pUsuarioCreacion;
                //ajusteProgramacion.FechaModificacion = DateTime.Now;

                ajusteProgramacion.EstadoCodigo = ConstanCodigoEstadoAjusteProgramacion.Enviada_al_supervisor;

                _context.SaveChanges();
                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreacion, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pUsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> AprobarAjusteProgramacion(int pAjusteProgramacionId, string pUsuarioCreacion, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Aprobar_Ajuste_Programacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //Contrato contrato = _context.Contrato.Where(c => c.ContratoId == pContratoId).Include(x => x.Contratacion).FirstOrDefault();
                AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion
                                                                    .Include(x => x.NovedadContractual)
                                                                        .ThenInclude(x => x.NovedadContractualDescripcion)
                                                                    .Include(x => x.ContratacionProyecto)
                                                                    .FirstOrDefault(x => x.AjusteProgramacionId == pAjusteProgramacionId);

                ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion
                                                                            .FirstOrDefault(x => x.ContratoId == ajusteProgramacion.NovedadContractual.ContratoId &&
                                                                                             x.ProyectoId == ajusteProgramacion.ContratacionProyecto.ProyectoId);

                Programacion[] programacions = _context.Programacion
                                                            .Where(x => x.ContratoConstruccionId == contratoConstruccion.ContratoConstruccionId)
                                                            .OrderBy(x => x.ProgramacionId)
                                                            .ToArray();

                FlujoInversion[] flujoInversions = _context.FlujoInversion
                                                                .Where(x => x.ContratoConstruccionId == contratoConstruccion.ContratoConstruccionId)
                                                                .OrderBy(x => x.FlujoInversionId)
                                                                .ToArray();

                AjusteProgramacionObra[] ajusteProgramacionObras = _context.AjusteProgramacionObra
                                                                                .Where(x => x.AjusteProgramacionId == ajusteProgramacion.AjusteProgramacionId)
                                                                                .OrderBy(x => x.AjusteProgramacionId)
                                                                                .ToArray();

                AjusteProgramacionFlujo[] ajusteProgramacionFlujos = _context.AjusteProgramacionFlujo
                                                                                .Where(x => x.AjusteProgramacionId == ajusteProgramacion.AjusteProgramacionId)
                                                                                .OrderBy(x => x.AjusteProgramacionId)
                                                                                .ToArray();


                //envio correo
                //envio correo a supervisor
                //Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.AprobarRequisitosTecnicosFase2);

                //string ncontrato = "";
                //string fechaContrato = "";
                //string template = TemplateRecoveryPassword.Contenido.
                //    Replace("[NUMEROCONTRATO]", contrato.NumeroContrato).
                //    Replace("_LinkF_", pDominioFront).
                //    Replace("[FECHAVERIFICACION]", DateTime.Now.ToString("dd/MM/yyyy")).
                //    Replace("[CANTIDADPROYECTOSASOCIADOS]", _context.ContratoConstruccion.Where(x => x.RegistroCompleto == true && x.ContratoId == contrato.ContratoId).Count().ToString()).
                //    Replace("[CANTIDADPROYECTOSVERIFICADOS]", _context.ContratoConstruccion.Where(x => x.RegistroCompleto == true && x.ContratoId == contrato.ContratoId).Count().ToString()).
                //    Replace("[TIPOCONTRATO]", contrato.Contratacion.TipoSolicitudCodigo == "1" ? "obra" : "interventoría");//OBRA O INTERVENTORIA


                //var usuariosadmin = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Apoyo).Include(y => y.Usuario).ToList();
                //foreach (var usuarioadmin in usuariosadmin)
                //{
                //    bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioadmin.Usuario.Email, "Aprobacion de requisitos técnicos de inicio para fase 2-construcción", template, pSender, pPassword, pMailServer, pMailPort);
                //}

                //Contrato contrato = _context.Contrato.Find(pContratoId);
                //jflorez, este evento solo sucede cuando esta completo y se aprueban los requisitos, por ello seteo el dato 20201202
                //contrato.FechaAprobacionRequisitosConstruccionInterventor = DateTime.Now;

                //ajusteProgramacion.UsuarioModificacion = pUsuarioCreacion;
                //ajusteProgramacion.FechaModificacion = DateTime.Now;

                ajusteProgramacion.EstadoCodigo = ConstanCodigoEstadoAjusteProgramacion.Aprobada_por_supervisor;

                foreach (NovedadContractualDescripcion novedad in ajusteProgramacion.NovedadContractual.NovedadContractualDescripcion)
                {
                    switch (novedad.TipoNovedadCodigo)
                    {
                        case ConstanTiposNovedades.Adición:
                            {
                                for (int i = 0; i < programacions.Length; i++)
                                {
                                    if (ajusteProgramacionObras.Length > i)
                                    {
                                        programacions[i].FechaInicio = ajusteProgramacionObras[i].FechaInicio;
                                        programacions[i].FechaFin = ajusteProgramacionObras[i].FechaFin;
                                        programacions[i].Duracion = ajusteProgramacionObras[i].Duracion;
                                    }
                                }

                                for (int i = 0; i < flujoInversions.Length; i++)
                                {
                                    if (ajusteProgramacionFlujos.Length > i)
                                    {
                                        flujoInversions[i].Valor = ajusteProgramacionFlujos[i].Valor;
                                    }
                                }

                                break;
                            }
                        case ConstanTiposNovedades.Prórroga:
                            {
                                for (int i = 0; i < programacions.Length; i++)
                                {
                                    if (ajusteProgramacionObras.Length > i)
                                    {
                                        programacions[i].FechaInicio = ajusteProgramacionObras[i].FechaInicio;
                                        programacions[i].FechaFin = ajusteProgramacionObras[i].FechaFin;
                                        programacions[i].Duracion = ajusteProgramacionObras[i].Duracion;
                                    }
                                }

                                for (int i = 0; i < ajusteProgramacionFlujos.Length; i++)
                                {
                                    if (flujoInversions.Length > i)
                                    {
                                        flujoInversions[i].Valor = ajusteProgramacionFlujos[i].Valor;
                                    }
                                    else
                                    {
                                        FlujoInversion flujo = new FlujoInversion
                                        {
                                            ContratoConstruccionId = contratoConstruccion.ContratoConstruccionId,
                                            Semana = ajusteProgramacionFlujos[i].Semana,
                                            Valor = ajusteProgramacionFlujos[i].Valor,
                                            MesEjecucionId = ajusteProgramacionFlujos[i].MesEjecucionId,
                                            ProgramacionId = ajusteProgramacionFlujos[i].ProgramacionId,
                                            SeguimientoSemanalId = ajusteProgramacionFlujos[i].SeguimientoSemanalId,

                                        };

                                        _context.FlujoInversion.Add(flujo);
                                    }
                                }

                                break;
                            }
                        case ConstanTiposNovedades.Reinicio:
                            {
                                for (int i = 0; i < programacions.Length; i++)
                                {
                                    if (ajusteProgramacionObras.Length > i)
                                    {
                                        programacions[i].FechaInicio = ajusteProgramacionObras[i].FechaInicio;
                                        programacions[i].FechaFin = ajusteProgramacionObras[i].FechaFin;
                                        programacions[i].Duracion = ajusteProgramacionObras[i].Duracion;
                                    }
                                }

                                for (int i = 0; i < ajusteProgramacionFlujos.Length; i++)
                                {
                                    if (flujoInversions.Length > i)
                                    {
                                        flujoInversions[i].Valor = ajusteProgramacionFlujos[i].Valor;
                                    }
                                }
                                break;
                            }
                    }

                };

                _context.SaveChanges();
                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreacion, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pUsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }
        #endregion

        #region CARGUE ARCHIVOS

        #region Programación obra

        public async Task<Respuesta> UploadFileToValidateAdjustmentProgramming(IFormFile pFile, string pFilePatch, string pUsuarioCreo,
                                                                        int pAjusteProgramacionId, int pContratacionProyectId, int pNovedadContractualId,
                                                                        int pContratoId, int pProyectoId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Validar_Excel_Ajuste_Programacion_Obra, (int)EnumeratorTipoDominio.Acciones);

            if (pAjusteProgramacionId == 0)
            {
                AjusteProgramacion ajusteProgramacionTemp = new AjusteProgramacion();

                //ajusteProgramacion.UsuarioCreacion = pUsuarioCreo;
                //ajusteProgramacion.FechaCreacion = DateTime.Now;

                ajusteProgramacionTemp.ContratacionProyectoId = pContratacionProyectId;
                ajusteProgramacionTemp.NovedadContractualId = pNovedadContractualId;
                ajusteProgramacionTemp.EstadoCodigo = ConstanCodigoEstadoAjusteProgramacion.En_proceso_de_ajuste_a_la_programacion;
                ajusteProgramacionTemp.FechaCreacion = DateTime.Now;
                ajusteProgramacionTemp.UsuarioCreacion = pUsuarioCreo;

                _context.AjusteProgramacion.Add(ajusteProgramacionTemp);
                _context.SaveChanges();

                pAjusteProgramacionId = ajusteProgramacionTemp.AjusteProgramacionId;
            }

            ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion
                                                                        .Where(cc => cc.ContratoId == pContratoId && cc.ProyectoId == pProyectoId)
                                                                        .Include(r => r.Contrato)
                                                                            .ThenInclude(r => r.Contratacion)
                                                                                .ThenInclude(r => r.DisponibilidadPresupuestal)
                                                                        .Include(r => r.Contrato)
                                                                            .ThenInclude(r => r.ContratoPoliza)
                                                                        .Include(r => r.Proyecto)
                                                                        .FirstOrDefault();

            NovedadContractual novedadContractual = _context.NovedadContractual
                                                                .Include(x => x.NovedadContractualDescripcion)
                                                                .FirstOrDefault(x => x.NovedadContractualId == pNovedadContractualId);

            Proyecto proyectoTemp = _technicalRequirementsConstructionPhaseService.CalcularFechasContrato(contratoConstruccion.ProyectoId, contratoConstruccion.FechaInicioObra, contratoConstruccion.ContratoId);

            AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacionId);

            DateTime? fechaInicioContrato = proyectoTemp.FechaInicioEtapaObra;
            //DateTime fechaFinalContrato = proyectoTemp.FechaFinEtapaObra;
            DateTime fechaFinalContrato = _commonService.GetFechaEstimadaFinalizacion(pContratoId) ?? proyectoTemp.FechaFinEtapaObra;

           /* novedadContractual.NovedadContractualDescripcion.ToList().ForEach(ncd =>
            {
                if (ncd.TipoNovedadCodigo == ConstanTiposNovedades.Reinicio && ajusteProgramacion.EstadoCodigo == ConstanCodigoEstadoAjusteProgramacion.En_proceso_de_ajuste_a_la_programacion)
                {
                    double cantidadDiasAgregados = (ncd.FechaFinSuspension.Value - ncd.FechaInicioSuspension.Value).TotalDays;
                    proyectoTemp.FechaFinEtapaObra = proyectoTemp.FechaFinEtapaObra.AddDays(cantidadDiasAgregados);
                }
            });*/

            int CantidadRegistrosVacios = 0;
            int CantidadResgistrosValidos = 0;
            int CantidadRegistrosInvalidos = 0;
            int cantidadRutaCritica = 0;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            DocumentService _documentService = new DocumentService(_context, _commonService);

            ArchivoCargue archivoCarge = await _documentService.getSaveFile(pFile, pFilePatch, Int32.Parse(OrigenArchivoCargue.AjusteProgramacionObra), pAjusteProgramacionId);

            // if (!string.IsNullOrEmpty(archivoCarge.ArchivoCargueId.ToString()))
            if (archivoCarge != null)
            {
                using (var stream = new MemoryStream())
                {
                    await pFile.CopyToAsync(stream);

                    using var package = new ExcelPackage(stream);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    bool estructuraValidaValidacionGeneral = true;
                    string mensajeRespuesta = string.Empty;

                    int cantidadActividades = 0;
                    int posicion = 2;
                    while (!string.IsNullOrEmpty(worksheet.Cells[posicion++, 1].Text))
                    {
                        cantidadActividades++;
                    }

                    for (int i = 2; i <= cantidadActividades + 1; i++)
                    {
                        bool tieneErrores = false;
                        try
                        {

                            TempProgramacion temp = new TempProgramacion();
                            //Auditoria
                            temp.ArchivoCargueId = archivoCarge.ArchivoCargueId;
                            temp.EstaValidado = false;
                            temp.FechaCreacion = DateTime.Now;
                            temp.UsuarioCreacion = pUsuarioCreo;
                            temp.AjusteProgramacionId = pAjusteProgramacionId;

                            List<AjusteProgramacionFlujo> listaFlujo = _context.AjusteProgramacionFlujo.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId).ToList();

                            if (listaFlujo.Count() > 1)
                            {
                                worksheet.Cells[1, 1].AddComment("se debe eliminar una carga de flujo de inversión asociada a este Proyecto", "Admin");
                                worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                                CantidadResgistrosValidos--;
                                estructuraValidaValidacionGeneral = false;
                                mensajeRespuesta = "se debe eliminar una carga de flujo de inversión asociada a este Proyecto";
                            }

                            #region Tipo Actividad
                            // #1
                            //Tipo Actividad
                            if (string.IsNullOrEmpty(worksheet.Cells[i, 1].Text))
                            {
                                worksheet.Cells[i, 1].AddComment("Dato Obligatorio", "Admin");
                                worksheet.Cells[i, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }
                            else if (new string[3] { "C", "SC", "I" }.Where(r => r == worksheet.Cells[i, 1].Text).Count() == 0)
                            {
                                worksheet.Cells[i, 1].AddComment("Tipo de actividad invalido", "Admin");
                                worksheet.Cells[i, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }
                            else
                            {
                                temp.TipoActividadCodigo = worksheet.Cells[i, 1].Text;
                            }

                            #endregion Tipo Actividad

                            #region Actividad

                            //#2
                            //Actividad
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 2].Text))
                            {
                                temp.Actividad = worksheet.Cells[i, 2].Text;
                            }
                            else
                            {
                                worksheet.Cells[i, 2].AddComment("Dato Obligatorio", "Admin");
                                worksheet.Cells[i, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }

                            #endregion Actividad

                            #region Marca de ruta critica

                            //#3
                            //Marca de ruta critica
                            if (string.IsNullOrEmpty(worksheet.Cells[i, 3].Text))
                            {
                                temp.EsRutaCritica = false;
                            }
                            else
                            {
                                if (temp.TipoActividadCodigo == "I" && worksheet.Cells[i, 3].Text == "1")
                                {
                                    temp.EsRutaCritica = true;
                                    cantidadRutaCritica++;
                                }
                                else if (temp.TipoActividadCodigo != "I" && worksheet.Cells[i, 3].Text == "1")
                                {
                                    worksheet.Cells[i, 3].AddComment("No se puede asignar ruta critica a este tipo de actividad", "Admin");
                                    worksheet.Cells[i, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[i, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                    tieneErrores = true;
                                }

                            }

                            #endregion Marca de ruta critica

                            #region Fecha Inicio

                            //#4
                            //Fecha Inicio
                            DateTime fechaTemp;
                            if (string.IsNullOrEmpty(worksheet.Cells[i, 4].Text))
                            {
                                worksheet.Cells[i, 4].AddComment("Dato Obligatorio", "Admin");
                                worksheet.Cells[i, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }
                            else
                            {
                                temp.FechaInicio = DateTime.TryParse(worksheet.Cells[i, 4].Text, out fechaTemp) ? fechaTemp : DateTime.MinValue;
                            }

                            #endregion Fecha Inicio

                            #region Fecha final

                            //#5
                            //Fecha final
                            if (string.IsNullOrEmpty(worksheet.Cells[i, 5].Text))
                            {
                                worksheet.Cells[i, 5].AddComment("Dato Obligatorio", "Admin");
                                worksheet.Cells[i, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }
                            else
                            {
                                temp.FechaFin = DateTime.TryParse(worksheet.Cells[i, 5].Text, out fechaTemp) ? fechaTemp : DateTime.MinValue;
                            }

                            #endregion Fecha final

                            #region validacion fechas

                            // validacion fechas
                            if (temp.FechaInicio.Date > temp.FechaFin.Date)
                            {
                                worksheet.Cells[i, 5].AddComment("La fecha no puede ser inferior a la fecha inicial", "Admin");
                                worksheet.Cells[i, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;

                            }

                            // fechas contrato
                            if (temp.FechaInicio.Date < fechaInicioContrato.Value.Date)
                            {
                                worksheet.Cells[i, 4].AddComment("La fecha Inicial de la actividad no puede ser inferior a la fecha inicial del contrato", "Admin");
                                worksheet.Cells[i, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }

                            if (temp.FechaFin.Date > fechaFinalContrato.Date)
                            {
                                worksheet.Cells[i, 5].AddComment("La fecha final de la actividad no puede ser mayor a la fecha final del contrato", "Admin");
                                worksheet.Cells[i, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }
                            // No se pueden programar actividades en fechas donde se ejecuto avance semanal o programacion v1.
                            // temp = Fechas del registro que esta leyendo
                            // fechaAct = Fecha de registros existentes

                            bool validacionFecha = false;

                            List<VFechasValidacionAjusteProgramacion> vFechas = _context.VFechasValidacionAjusteProgramacion.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId).ToList();
                            foreach (var fechaAct in vFechas)
                            {
                                if (!((temp.FechaInicio.Date < fechaAct.FechaInicio && temp.FechaFin.Date < fechaAct.FechaInicio) || (temp.FechaInicio.Date > fechaAct.FechaFin)))
                                {
                                    validacionFecha = true;
                                    break;
                                }
                            }

                            if (validacionFecha)
                            {
                                worksheet.Cells[i, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                                worksheet.Cells[i, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                worksheet.Cells[i, 5].AddComment("No se pueden programar nuevas actividades en fechas en que ya se ejecutaron actividades.", "Admin");
                                tieneErrores = true;
                            }

                            #endregion validacion fechas

                            #region Duracion

                            //#6
                            //Duracion
                            if (string.IsNullOrEmpty(worksheet.Cells[i, 6].Text))
                            {
                                worksheet.Cells[i, 6].AddComment("Dato Obligatorio", "Admin");
                                worksheet.Cells[i, 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }
                            else
                            {
                                temp.Duracion = Int32.Parse(worksheet.Cells[i, 6].Text);
                            }

                            #endregion Duracion


                            //Guarda Cambios en una tabla temporal

                            if (!tieneErrores)
                            {
                                _context.TempProgramacion.Add(temp);
                                _context.SaveChanges();
                            }

                            if (temp.TempProgramacionId > 0)
                            {
                                CantidadResgistrosValidos++;
                            }
                            else
                            {
                                CantidadRegistrosInvalidos++;
                            }

                        }
                        catch (Exception ex)
                        {
                            CantidadRegistrosInvalidos++;
                        }
                    }

                    if (cantidadRutaCritica == 0 && worksheet.Cells[1, 1].Comment == null)
                    {
                        worksheet.Cells[1, 1].AddComment("Debe existir por lo menos una ruta critica", "Admin");
                        worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        CantidadRegistrosInvalidos++;
                        CantidadResgistrosValidos--;
                        estructuraValidaValidacionGeneral = false;
                        mensajeRespuesta = "Debe existir por lo menos una ruta critica";
                    }

                    ArchivoCargueRespuesta archivoCargueRespuesta = new ArchivoCargueRespuesta();

                    if (estructuraValidaValidacionGeneral == true)
                    {
                        archivoCargueRespuesta = new ArchivoCargueRespuesta
                        {
                            CantidadDeRegistros = cantidadActividades.ToString(),
                            CantidadDeRegistrosInvalidos = CantidadRegistrosInvalidos.ToString(),
                            CantidadDeRegistrosValidos = CantidadResgistrosValidos.ToString(),
                            LlaveConsulta = archivoCarge.Nombre,
                            CargaValida = true,

                        };
                    }
                    else if (estructuraValidaValidacionGeneral == false)
                    {
                        CantidadResgistrosValidos = 0;

                        archivoCargueRespuesta = new ArchivoCargueRespuesta
                        {
                            CantidadDeRegistros = archivoCarge.CantidadRegistros.ToString(),
                            CantidadDeRegistrosInvalidos = "0",
                            CantidadDeRegistrosValidos = "0",
                            LlaveConsulta = archivoCarge.Nombre,
                            CargaValida = false,
                            Mensaje = mensajeRespuesta,

                        };
                    }

                    //Actualizo el archivoCarge con la cantidad de registros validos , invalidos , y el total;
                    //-2 ya los registros comienzan desde esta fila
                    archivoCarge.CantidadRegistrosInvalidos = CantidadRegistrosInvalidos;
                    archivoCarge.CantidadRegistrosValidos = CantidadResgistrosValidos;
                    archivoCarge.CantidadRegistros = cantidadActividades;
                    _context.ArchivoCargue.Update(archivoCarge);


                    byte[] bin = package.GetAsByteArray();
                    string pathFile = archivoCarge.Ruta + "/" + archivoCarge.Nombre + ".xlsx";
                    File.WriteAllBytes(pathFile, bin);


                    return new Respuesta
                    {
                        Data = archivoCargueRespuesta,
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreo, "VALIDAR EXCEL PROGRAMACION")
                    };
                }




            }
            else
            {
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesCargueElegibilidad.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pUsuarioCreo, "VALIDAR EXCEL PROGRAMACION")
                };
            }


        }
        #endregion

        #region Flujo de inversión

        public async Task<Respuesta> UploadFileToValidateAdjustmentInvestmentFlow(IFormFile pFile, string pFilePatch, string pUsuarioCreo,
                                                                                int pAjusteProgramacionId, int pContratacionProyectId, int pNovedadContractualId,
                                                                                int pContratoId, int pProyectoId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Validar_Excel_Ajuste_Flujo_Inversion, (int)EnumeratorTipoDominio.Acciones);

            if (pAjusteProgramacionId == 0)
            {
                AjusteProgramacion ajusteProgramacionTemp = new AjusteProgramacion();

                ajusteProgramacionTemp.UsuarioCreacion = pUsuarioCreo;
                ajusteProgramacionTemp.FechaCreacion = DateTime.Now;
                ajusteProgramacionTemp.ContratacionProyectoId = pContratacionProyectId;
                ajusteProgramacionTemp.NovedadContractualId = pNovedadContractualId;

                _context.AjusteProgramacion.Add(ajusteProgramacionTemp);
                _context.SaveChanges();

                pAjusteProgramacionId = ajusteProgramacionTemp.AjusteProgramacionId;
            }

            int CantidadRegistrosVacios = 0;
            int CantidadResgistrosValidos = 0;
            int CantidadRegistrosInvalidos = 0;

            AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacionId);

            // rango de fechas
            ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion
                                                                        .Where(cc => cc.ContratoId == pContratoId && cc.ProyectoId == pProyectoId)
                                                                        .FirstOrDefault();

            Proyecto proyecto = _technicalRequirementsConstructionPhaseService.CalcularFechasContrato(pProyectoId, contratoConstruccion.FechaInicioObra, contratoConstruccion.ContratoId);

            NovedadContractual novedadContractual = _context.NovedadContractual
                                                                .Include(x => x.NovedadContractualDescripcion)
                                                                .FirstOrDefault(x => x.NovedadContractualId == ajusteProgramacion.NovedadContractualId);

            novedadContractual.NovedadContractualDescripcion.ToList().ForEach(ncd =>
            {
                if (ncd.TipoNovedadCodigo == ConstanTiposNovedades.Reinicio && ajusteProgramacion.EstadoCodigo == ConstanCodigoEstadoAjusteProgramacion.En_proceso_de_ajuste_a_la_programacion)
                {
                    double cantidadDiasAgregados = (ncd.FechaFinSuspension.Value - ncd.FechaInicioSuspension.Value).TotalDays;
                    proyecto.FechaFinEtapaObra = proyecto.FechaFinEtapaObra.AddDays(cantidadDiasAgregados);
                }
            });

            List<dynamic> listaFechas = CrearNuevasFecha(proyecto, novedadContractual, ajusteProgramacion);

            //Numero semanas
            int numberOfWeeks = listaFechas.Count();

            //int numberOfWeeks = Convert.ToInt32(Math.Floor((proyecto.FechaFinEtapaObra - proyecto.FechaInicioEtapaObra).TotalDays / 7));
            //if (Convert.ToInt32(Math.Round((proyecto.FechaFinEtapaObra - proyecto.FechaInicioEtapaObra).TotalDays % 7)) > 0)
            //    numberOfWeeks++;

            //Capitulos cargados
            //AjusteProgramacionObra[] listaProgramacion = _context.AjusteProgramacionObra
            //                                                    .Where(
            //                                                            p => p.AjusteProgramacionId == pAjusteProgramacionId &&
            //                                                            p.TipoActividadCodigo == "C")
            //                                                    .OrderBy(p => p.AjusteProgramacionObraId)
            //                                                    .ToArray();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.Proyecto, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);
            DocumentService _documentService = new DocumentService(_context, _commonService);

            ArchivoCargue archivoCarge = await _documentService.getSaveFile(pFile, pFilePatch, Int32.Parse(OrigenArchivoCargue.AjusteFlujoInversion), pAjusteProgramacionId);

            // if (!string.IsNullOrEmpty(archivoCarge.ArchivoCargueId.ToString()))

            bool estructuraValidaValidacionGeneral = true;
            string mensajeRespuesta = string.Empty;

            if (archivoCarge != null)
            {
                using (var stream = new MemoryStream())
                {
                    await pFile.CopyToAsync(stream);

                    using var package = new ExcelPackage(stream);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    int cantidadCapitulos = 0;
                    int cantidadSemnas = 0;

                    int posicion = 2;
                    while (!string.IsNullOrEmpty(worksheet.Cells[1, posicion++].Text))
                    {
                        cantidadSemnas++;
                    }

                    posicion = 2;
                    while (!string.IsNullOrEmpty(worksheet.Cells[posicion++, 1].Text))
                    {
                        cantidadCapitulos++;
                    }

                    bool tieneErrores = false;

                    // valida numero semanas
                    if (numberOfWeeks != cantidadSemnas)
                    {
                        worksheet.Cells[1, 1].AddComment("Numero de semanas no es igual al del proyecto", "Admin");
                        worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        tieneErrores = true;
                        estructuraValidaValidacionGeneral = false;
                        mensajeRespuesta = "Numero de semanas no es igual al del proyecto";
                    }
                    /*
                    //valida numero capitulos
                    if (listaProgramacion.Count() != cantidadCapitulos && worksheet.Cells[1, 1].Comment == null)
                    {
                        worksheet.Cells[1, 1].AddComment("Numero de capitulos no es igual a la programacion", "Admin");
                        worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        tieneErrores = true;
                        estructuraValidaValidacionGeneral = false;
                        mensajeRespuesta = "Numero de capitulos no es igual a la programacion";
                    }*/

                    decimal sumaTotal = 0;

                    // Capitulos
                    //int i = 2;
                    for (int i = 2; i <= cantidadCapitulos + 1; i++)
                    {
                        bool tieneErroresCapitulo = false;

                        try
                        {
                            // semanas
                            //int k = 2;
                            for (int k = 2; k < cantidadSemnas + 2; k++)
                            {

                                TempFlujoInversion temp = new TempFlujoInversion();
                                //Auditoria
                                temp.ArchivoCargueId = archivoCarge.ArchivoCargueId;
                                temp.EstaValidado = false;
                                temp.FechaCreacion = DateTime.Now;
                                temp.UsuarioCreacion = pUsuarioCreo;
                                temp.AjusteProgramacionId = pAjusteProgramacionId;
                                temp.Posicion = k - 2;
                                temp.PosicionCapitulo = i - 2;

                                #region Mes

                                // Mes
                                if (string.IsNullOrEmpty(worksheet.Cells[1, k].Text))
                                {
                                    worksheet.Cells[1, k].AddComment("Dato Obligatorio", "Admin");
                                    worksheet.Cells[1, k].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[1, k].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                    tieneErrores = true;
                                    tieneErroresCapitulo = true;
                                }
                                else
                                {
                                    temp.Semana = worksheet.Cells[1, k].Text;
                                }

                                #endregion Mes

                                #region Capitulo

                                //Capitulo
                                if (string.IsNullOrEmpty(worksheet.Cells[i, 1].Text))
                                {
                                    worksheet.Cells[i, 1].AddComment("Dato Obligatorio", "Admin");
                                    worksheet.Cells[i, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                    tieneErrores = true;
                                    tieneErroresCapitulo = true;
                                }

                                #endregion Capitulo

                                string valorTemp = worksheet.Cells[i, k].Text;

                                valorTemp = valorTemp.Replace("$", "").Replace(".", "").Replace(" ", "");

                                //Valor
                                temp.Valor = string.IsNullOrEmpty(valorTemp) ? 0 : decimal.Parse(valorTemp);
                                sumaTotal += temp.Valor.Value;

                                //Guarda Cambios en una tabla temporal

                                if (!tieneErrores)
                                {
                                    _context.TempFlujoInversion.Add(temp);
                                    _context.SaveChanges();
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            CantidadRegistrosInvalidos++;
                        }

                        if (tieneErroresCapitulo == true)
                        {
                            CantidadRegistrosInvalidos++;
                        }
                        else
                        {
                            CantidadResgistrosValidos++;
                        }
                    }

                    if (proyecto.ValorObra != sumaTotal && worksheet.Cells[1, 1].Comment == null)
                    {
                        worksheet.Cells[1, 1].AddComment("La suma de los valores no es igual al valor total de obra del proyecto", "Admin");
                        worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        estructuraValidaValidacionGeneral = false;
                        mensajeRespuesta = "La suma de los valores no es igual al valor total de obra del proyecto";
                    }

                    ArchivoCargueRespuesta archivoCargueRespuesta = new ArchivoCargueRespuesta();

                    if (estructuraValidaValidacionGeneral == true)
                    {
                        archivoCargueRespuesta = new ArchivoCargueRespuesta
                        {
                            CantidadDeRegistros = cantidadCapitulos.ToString(),
                            CantidadDeRegistrosInvalidos = CantidadRegistrosInvalidos.ToString(),
                            CantidadDeRegistrosValidos = CantidadResgistrosValidos.ToString(),
                            LlaveConsulta = archivoCarge.Nombre,
                            CargaValida = true,
                            Mensaje = mensajeRespuesta,

                        };
                    }
                    else if (estructuraValidaValidacionGeneral == false)
                    {
                        CantidadResgistrosValidos = 0;

                        archivoCargueRespuesta = new ArchivoCargueRespuesta
                        {
                            CantidadDeRegistros = archivoCarge.CantidadRegistros.ToString(),
                            CantidadDeRegistrosInvalidos = archivoCarge.CantidadRegistrosInvalidos.ToString(),
                            CantidadDeRegistrosValidos = archivoCarge.CantidadRegistrosValidos.ToString(),
                            LlaveConsulta = archivoCarge.Nombre,
                            CargaValida = false,
                            Mensaje = mensajeRespuesta,

                        };
                    }

                    //Actualizo el archivoCarge con la cantidad de registros validos , invalidos , y el total;
                    //-2 ya los registros comienzan desde esta fila
                    archivoCarge.CantidadRegistrosInvalidos = CantidadRegistrosInvalidos;
                    archivoCarge.CantidadRegistrosValidos = CantidadResgistrosValidos;
                    archivoCarge.CantidadRegistros = cantidadCapitulos;
                    _context.ArchivoCargue.Update(archivoCarge);

                    byte[] bin = package.GetAsByteArray();
                    string pathFile = archivoCarge.Ruta + "/" + archivoCarge.Nombre + ".xlsx";
                    File.WriteAllBytes(pathFile, bin);

                    return new Respuesta
                    {
                        Data = archivoCargueRespuesta,
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreo, "VALIDAR EXCEL FLUJO IN")
                    };
                }
            }
            else
            {
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesCargueElegibilidad.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pUsuarioCreo, "VALIDAR EXCEL FLUJO INV")
                };
            }


        }
        #endregion
        #endregion

        #region Ajutes a la reprogramación
        private List<dynamic> CrearNuevasFecha(Proyecto proyecto, NovedadContractual novedadContractual, AjusteProgramacion ajusteProgramacion)
        {
            List<dynamic> listaFechasTemp = new List<dynamic>();
            List<dynamic> listaFechas = new List<dynamic>();

            DateTime fechaTemp = proyecto.FechaInicioEtapaObra;

            while (proyecto.FechaFinEtapaObra >= fechaTemp)
            {
                listaFechasTemp.Add(new { fechaInicio = fechaTemp, fechaFin = fechaTemp.AddDays(6) });
                fechaTemp = fechaTemp.AddDays(7);
            }

            // agrega la cantidad de dias del reinicio
            if (
                novedadContractual.NovedadContractualDescripcion.Where(x => x.TipoNovedadCodigo == ConstanTiposNovedades.Reinicio).Count() > 0 &&
                ajusteProgramacion.EstadoCodigo == ConstanCodigoEstadoAjusteProgramacion.En_proceso_de_ajuste_a_la_programacion)
            {
                NovedadContractualDescripcion novedadContractualDescripcion = novedadContractual.NovedadContractualDescripcion
                                                                                                    .Where(x => x.TipoNovedadCodigo == ConstanTiposNovedades.Reinicio)
                                                                                                    .FirstOrDefault();

                double cantidadDiasAgregados = (novedadContractualDescripcion.FechaFinSuspension.Value - novedadContractualDescripcion.FechaInicioSuspension.Value).TotalDays;

                for (int j = 0; j < listaFechasTemp.Count(); j++)
                {
                    // busco el rango donde este la fecha de inicio de la novedad
                    if (
                            novedadContractualDescripcion.FechaInicioSuspension.Value >= listaFechasTemp[j].fechaInicio &&
                            novedadContractualDescripcion.FechaInicioSuspension.Value <= listaFechasTemp[j].fechaFin
                        )
                    {
                        // asigno la nueva fecha fin del rango
                        listaFechasTemp[j].fechaFin = novedadContractualDescripcion.FechaInicioSuspension.Value;
                        listaFechas.Add(new { fechaInicio = listaFechasTemp[j].fechaInicio, fechaFin = listaFechasTemp[j].fechaFin });

                        // busco el rango donde este la fecha de inicio de la novedad si estan en la misma semana
                        if (
                                novedadContractualDescripcion.FechaFinSuspension.Value >= listaFechasTemp[j].fechaInicio &&
                                novedadContractualDescripcion.FechaFinSuspension.Value <= listaFechasTemp[j].fechaFin
                            )
                        {
                            // asigno la nueva fecha Inicio del rango
                            listaFechasTemp[j].fechaInicio = novedadContractualDescripcion.FechaFinSuspension.Value;
                            listaFechas.Add(new { fechaInicio = listaFechasTemp[j].fechaInicio, fechaFin = listaFechasTemp[j].fechaFin });
                        }
                    }
                    else
                    // busco el rango donde este la fecha de inicio de la novedad
                    if (
                            novedadContractualDescripcion.FechaFinSuspension.Value >= listaFechasTemp[j].fechaInicio &&
                            novedadContractualDescripcion.FechaFinSuspension.Value <= listaFechasTemp[j].fechaFin
                        )
                    {
                        // asigno la nueva fecha Inicio del rango
                        listaFechasTemp[j].fechaInicio = novedadContractualDescripcion.FechaFinSuspension.Value;
                        listaFechas.Add(new { fechaInicio = listaFechasTemp[j].fechaInicio, fechaFin = listaFechasTemp[j].fechaFin });
                    }
                    // suma la cantidad de dias a los rangos despues del reinicio
                    else if (novedadContractualDescripcion.FechaFinSuspension < listaFechasTemp[j].fechaInicio)
                    {
                        listaFechasTemp[j].fechaInicio = listaFechasTemp[j].fechaInicio.AddDays(cantidadDiasAgregados);
                        listaFechasTemp[j].fechaFin = listaFechasTemp[j].fechaFin.AddDays(cantidadDiasAgregados);
                        listaFechas.Add(new { fechaInicio = listaFechasTemp[j].fechaInicio, fechaFin = listaFechasTemp[j].fechaFin });
                    }


                }
            }
            else
            {
                listaFechas = listaFechasTemp.ToList();
            }

            return listaFechas;
        }

        public async Task<Respuesta> TransferMassiveLoadAdjustmentProgramming(string pIdDocument, string pUsuarioModifico, int pProyectoId, int pContratoId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Load_Data_Ajuste_Programacion_Obra, (int)EnumeratorTipoDominio.Acciones);

            Respuesta respuesta = new Respuesta();

            int ajusteProgramacionId = 0;

            if (string.IsNullOrEmpty(pIdDocument))
            {
                return respuesta =
                 new Respuesta
                 {
                     IsSuccessful = false,
                     IsException = false,
                     IsValidation = true,
                     Code = GeneralCodes.CamposVacios,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.CamposVacios, idAccion, pUsuarioModifico, "")
                 };
            }
            try
            {


                int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.AjusteProgramacionObra, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);

                ArchivoCargue archivoCargue = _context.ArchivoCargue
                                                .Where(r => r.OrigenId == int.Parse(OrigenArchivoCargue.AjusteProgramacionObra) &&
                                                        r.Nombre.Trim().ToUpper().Equals(pIdDocument.ToUpper().Trim())
                                                      )
                                                .FirstOrDefault();

                List<TempProgramacion> listTempProgramacion = await _context.TempProgramacion
                                                                .Where(r => r.ArchivoCargueId == archivoCargue.ArchivoCargueId && !(bool)r.EstaValidado)
                                                                .ToListAsync();



                if (listTempProgramacion.Count() > 0)
                {

                    ajusteProgramacionId = listTempProgramacion.FirstOrDefault().AjusteProgramacionId.Value;

                    // Eliminar meses ya cargados
                    //List<MesEjecucion> listaMeses = _context.MesEjecucion.Where(m => m.ContratoConstruccionId == contratoConstruccionId).ToList();
                    //_context.MesEjecucion.RemoveRange(listaMeses);

                    //eliminar Ajuste Programacion Obra
                    List<AjusteProgramacionObra> listaProgramacion = _context.AjusteProgramacionObra
                                                                                .Where(p => p.AjusteProgramacionId == ajusteProgramacionId)
                                                                                .ToList();
                    _context.AjusteProgramacionObra.RemoveRange(listaProgramacion);

                    // copia la información
                    foreach (TempProgramacion tempProgramacion in listTempProgramacion)
                    {

                        AjusteProgramacionObra programacion = new AjusteProgramacionObra()
                        {
                            AjusteProgramacionId = tempProgramacion.AjusteProgramacionId.Value,
                            TipoActividadCodigo = tempProgramacion.TipoActividadCodigo,
                            Actividad = tempProgramacion.Actividad,
                            EsRutaCritica = tempProgramacion.EsRutaCritica,
                            FechaInicio = tempProgramacion.FechaInicio,
                            FechaFin = tempProgramacion.FechaFin,
                            Duracion = tempProgramacion.Duracion

                        };

                        _context.AjusteProgramacionObra.Add(programacion);
                        _context.SaveChanges();



                        //Temporal proyecto update
                        tempProgramacion.EstaValidado = true;
                        tempProgramacion.FechaModificacion = DateTime.Now;
                        tempProgramacion.UsuarioModificacion = pUsuarioModifico;
                        _context.TempProgramacion.Update(tempProgramacion);
                        _context.SaveChanges();
                    }

                    AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(ajusteProgramacionId);

                    ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion
                                                                        .Where(cc => cc.ContratoId == pContratoId && cc.ProyectoId == pProyectoId)
                                                                        .FirstOrDefault();

                    Proyecto proyecto = _technicalRequirementsConstructionPhaseService.CalcularFechaInicioContrato(contratoConstruccion.ContratoConstruccionId);

                    NovedadContractual novedadContractual = _context.NovedadContractual
                                                                .Include(x => x.NovedadContractualDescripcion)
                                                                .FirstOrDefault(x => x.NovedadContractualId == ajusteProgramacion.NovedadContractualId);

                    DateTime fechaFinalContrato = _commonService.GetFechaEstimadaFinalizacion(pContratoId) ?? proyecto.FechaFinEtapaObra;
                    proyecto.FechaFinEtapaObra = fechaFinalContrato;

                    /*novedadContractual.NovedadContractualDescripcion.ToList().ForEach(ncd =>
                    {
                        if (ncd.TipoNovedadCodigo == ConstanTiposNovedades.Reinicio && ajusteProgramacion.EstadoCodigo == ConstanCodigoEstadoAjusteProgramacion.En_proceso_de_ajuste_a_la_programacion)
                        {
                            double cantidadDiasAgregados = (ncd.FechaFinSuspension.Value - ncd.FechaInicioSuspension.Value).TotalDays;
                            proyecto.FechaFinEtapaObra = proyecto.FechaFinEtapaObra.AddDays(cantidadDiasAgregados);
                        }
                    });*/

                    MesEjecucion[] meses = _context.MesEjecucion.Where(x => x.ContratoConstruccionId == contratoConstruccion.ContratoConstruccionId).ToArray();

                    int numeroMes = 1;
                    int idMes = 0;
                    for (DateTime fecha = proyecto.FechaInicioEtapaObra; fecha <= proyecto.FechaFinEtapaObra; fecha = fecha.AddMonths(1))
                    {
                        if (meses.Length < numeroMes)
                        {
                            MesEjecucion mes = new MesEjecucion()
                            {
                                ContratoConstruccionId = contratoConstruccion.ContratoConstruccionId,
                                Numero = numeroMes,
                                FechaInicio = fecha,
                                FechaFin = fecha.AddMonths(1).AddDays(-1),

                            };

                            _context.MesEjecucion.Add(mes);
                        }

                        numeroMes++;
                    }


                    MesEjecucion ultimoMes = _context.MesEjecucion
                                                            .Where(m => m.ContratoConstruccionId == contratoConstruccion.ContratoConstruccionId)
                                                            .OrderByDescending(m => m.Numero)
                                                            .FirstOrDefault();

                    ultimoMes.FechaFin = proyecto.FechaFinEtapaObra;

                    _context.SaveChanges();

                    if (ajusteProgramacion != null)
                    {
                        ajusteProgramacion.ArchivoCargueIdProgramacionObra = archivoCargue.ArchivoCargueId;
                        ajusteProgramacion.EstadoCodigo = ConstanCodigoEstadoAjusteProgramacion.En_proceso_de_ajuste_a_la_programacion;

                        VerificarRegistroCompletoAjusteProgramacion(ajusteProgramacionId);
                    }



                    return respuesta =
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = true,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioModifico, "Cantidad de registros subidos : " + listTempProgramacion.Count())
                    };
                }
                else
                {
                    return respuesta =
                        new Respuesta
                        {
                            IsSuccessful = false,
                            IsException = false,
                            IsValidation = true,
                            Code = GeneralCodes.OperacionExitosa,
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.NoExitenArchivos, idAccion, pUsuarioModifico, "")
                        };
                }
            }
            catch (Exception ex)
            {
                return respuesta =
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesCargueElegibilidad.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, ConstantMessagesCargueElegibilidad.Error, (int)enumeratorAccion.CargueProyectosMasivos, pUsuarioModifico, ex.InnerException.ToString())
                    };
            }

        }

        public async Task<Respuesta> TransferMassiveLoadAdjustmentInvestmentFlow(string pIdDocument, string pUsuarioModifico, int pProyectoId, int pContratoId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Load_Data_Ajuste_Flujo_Inversion, (int)EnumeratorTipoDominio.Acciones);

            Respuesta respuesta = new Respuesta();

            int ajusteProgramacionId = 0;

            if (string.IsNullOrEmpty(pIdDocument))
            {
                return respuesta =
                 new Respuesta
                 {
                     IsSuccessful = false,
                     IsException = false,
                     IsValidation = true,
                     Code = GeneralCodes.CamposVacios,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.CamposVacios, idAccion, pUsuarioModifico, "")
                 };
            }
            try
            {


                int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.AjusteFlujoInversion, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);

                ArchivoCargue archivoCargue = _context.ArchivoCargue
                                                .Where(r => r.OrigenId == int.Parse(OrigenArchivoCargue.AjusteFlujoInversion) &&
                                                        r.Nombre.Trim().ToUpper().Equals(pIdDocument.ToUpper().Trim())
                                                      )
                                                .FirstOrDefault();

                List<TempFlujoInversion> listTempFlujoInversion = await _context.TempFlujoInversion
                                                                .Where(r => r.ArchivoCargueId == archivoCargue.ArchivoCargueId && !(bool)r.EstaValidado)
                                                                .ToListAsync();

                if (listTempFlujoInversion.Count() > 0)
                {
                    ajusteProgramacionId = listTempFlujoInversion.FirstOrDefault().AjusteProgramacionId.Value;

                    AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(ajusteProgramacionId);

                    ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion
                                                                            .Where(r => r.ContratoId == pContratoId && r.ProyectoId == pProyectoId)
                                                                            .Include(r => r.Contrato)
                                                                                .ThenInclude(r => r.Contratacion)
                                                                                    .ThenInclude(r => r.ContratacionProyecto)
                                                                            .Include(r => r.Proyecto)

                                                                            .FirstOrDefault();

                    Proyecto proyecto = _technicalRequirementsConstructionPhaseService.CalcularFechaInicioContrato(contratoConstruccion.ContratoConstruccionId);



                    // 
                    ContratacionProyecto contratacionProyecto = contratoConstruccion.Contrato.Contratacion.ContratacionProyecto.Where(p => p.ProyectoId == contratoConstruccion.ProyectoId).FirstOrDefault();

                    NovedadContractual novedadContractual = _context.NovedadContractual
                                                                .Include(x => x.NovedadContractualDescripcion)
                                                                .FirstOrDefault(x => x.NovedadContractualId == ajusteProgramacion.NovedadContractualId);

                    if (contratacionProyecto != null)
                    {
                        novedadContractual.NovedadContractualDescripcion.ToList().ForEach(ncd =>
                        {
                            if (ncd.TipoNovedadCodigo == ConstanTiposNovedades.Reinicio && ajusteProgramacion.EstadoCodigo == ConstanCodigoEstadoAjusteProgramacion.En_proceso_de_ajuste_a_la_programacion)
                            {
                                double cantidadDiasAgregados = (ncd.FechaFinSuspension.Value - ncd.FechaInicioSuspension.Value).TotalDays;
                                proyecto.FechaFinEtapaObra = proyecto.FechaFinEtapaObra.AddDays(cantidadDiasAgregados);
                            }
                        });

                        // ajusta las nuevas fechas 
                        List<dynamic> listaFechas = CrearNuevasFecha(proyecto, novedadContractual, ajusteProgramacion);

                        int idContratacionproyecto = contratacionProyecto.ContratacionProyectoId;

                        List<SeguimientoSemanalTemp> listaSeguimientos = _context.SeguimientoSemanalTemp
                                                                    .Where(p => p.ContratacionProyectoId == idContratacionproyecto && p.AjusteProgramaionId == ajusteProgramacionId)
                                                                    .ToList();

                        // eliminar registros cargados
                        List<AjusteProgramacionFlujo> listaFlujo = _context.AjusteProgramacionFlujo.Where(r => r.AjusteProgramacionId == ajusteProgramacionId).ToList();
                        _context.AjusteProgramacionFlujo.RemoveRange(listaFlujo);

                        // elimina los existentes
                        _context.SeguimientoSemanalTemp.RemoveRange(listaSeguimientos);

                        int i = 1;
                        listaFechas.OrderBy(p => p.fechaInicio).ToList().ForEach(f =>
                        {
                            //    if ( listaSeguimientos.Where(x => x.NumeroSemana  )
                            SeguimientoSemanalTemp seguimientoSemanal = new SeguimientoSemanalTemp()
                            {
                                ContratacionProyectoId = idContratacionproyecto,
                                AjusteProgramaionId = ajusteProgramacionId,
                                Eliminado = false,
                                UsuarioCreacion = pUsuarioModifico,
                                FechaCreacion = DateTime.Now,
                                NumeroSemana = i,
                                FechaInicio = f.fechaInicio,
                                FechaFin = f.fechaFin,
                                RegistroCompleto = false,
                            };

                            _context.SeguimientoSemanalTemp.Add(seguimientoSemanal);
                            _context.SaveChanges();

                            i++;

                        });

                        SeguimientoSemanalTemp seguimientoSemanal = _context.SeguimientoSemanalTemp
                                                                            .Where(s => s.ContratacionProyectoId == idContratacionproyecto && s.AjusteProgramaionId == ajusteProgramacionId)
                                                                            .OrderByDescending(s => s.NumeroSemana)
                                                                            .FirstOrDefault();

                        seguimientoSemanal.FechaFin = proyecto.FechaFinEtapaObra;
                        _context.SaveChanges();

                    }

                    SeguimientoSemanal[] listaSeguimientoSemanal = _context.SeguimientoSemanal
                                                                                    .Where(s => s.ContratacionProyectoId == contratacionProyecto.ContratacionProyectoId)
                                                                                    .OrderBy(s => s.NumeroSemana)
                                                                                    .ToArray();

                    MesEjecucion[] listaMeses = _context.MesEjecucion
                                                            .Where(s => s.ContratoConstruccionId == contratoConstruccion.ContratoConstruccionId)
                                                            .OrderBy(s => s.Numero)
                                                            .ToArray();

                    Programacion[] listaProgramacion = _context.Programacion
                                                                .Where(
                                                                        p => p.ContratoConstruccionId == contratoConstruccion.ContratoConstruccionId &&
                                                                        p.TipoActividadCodigo == "C")
                                                                .OrderBy(p => p.ProgramacionId)
                                                                .ToArray();



                    _context.SaveChanges();

                    listTempFlujoInversion.ForEach(tempFlujo =>
                    {

                        int? mesId = 0;

                        listaMeses.ToList().ForEach(m =>
                        {
                            if (
                                    listaSeguimientoSemanal[tempFlujo.Posicion.Value].FechaInicio.Value.Date >= m.FechaInicio.Date &&
                                    listaSeguimientoSemanal[tempFlujo.Posicion.Value].FechaFin.Value.Date <= m.FechaFin.Date
                                )
                            {
                                mesId = m.MesEjecucionId;
                            }
                            else if (
                                       listaSeguimientoSemanal[tempFlujo.Posicion.Value].FechaInicio.Value.Date <= m.FechaFin &&
                                       listaSeguimientoSemanal[tempFlujo.Posicion.Value].FechaFin.Value.Date >= m.FechaFin
                                    )
                            {
                                mesId = m.MesEjecucionId;
                            }
                        });

                        AjusteProgramacionFlujo flujo = new AjusteProgramacionFlujo()
                        {
                            AjusteProgramacionId = tempFlujo.AjusteProgramacionId.Value,
                            Semana = tempFlujo.Semana,
                            Valor = tempFlujo.Valor,
                            SeguimientoSemanalId = listaSeguimientoSemanal[tempFlujo.Posicion.Value].SeguimientoSemanalId,
                            MesEjecucionId = mesId.Value == 0 ? null : mesId,
                            ProgramacionId = listaProgramacion[tempFlujo.PosicionCapitulo.Value].ProgramacionId,

                        };

                        _context.AjusteProgramacionFlujo.Add(flujo);
                        //_context.SaveChanges();



                        //Temporal proyecto update
                        tempFlujo.EstaValidado = true;
                        tempFlujo.FechaModificacion = DateTime.Now;
                        tempFlujo.UsuarioModificacion = pUsuarioModifico;
                        //_context.TempFlujoInversion.Update(tempFlujo);
                        _context.SaveChanges();

                    });



                    if (ajusteProgramacion != null)
                    {
                        List<dynamic> listaFechas = new List<dynamic>();

                        ajusteProgramacion.ArchivoCargueIdFlujoInversion = archivoCargue.ArchivoCargueId;
                        ajusteProgramacion.EstadoCodigo = ConstanCodigoEstadoAjusteProgramacion.En_proceso_de_ajuste_a_la_programacion;

                        VerificarRegistroCompletoAjusteProgramacion(ajusteProgramacion.AjusteProgramacionId);


                    }

                    return respuesta =
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = true,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioModifico, "Cantidad de registros subidos : " + listTempFlujoInversion.Count())
                    };
                }
                else
                {
                    return respuesta =
                        new Respuesta
                        {
                            IsSuccessful = false,
                            IsException = false,
                            IsValidation = true,
                            Code = GeneralCodes.OperacionExitosa,
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.NoExitenArchivos, idAccion, pUsuarioModifico, "")
                        };
                }
            }
            catch (Exception ex)
            {
                return respuesta =
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesCargueElegibilidad.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, ConstantMessagesCargueElegibilidad.Error, (int)enumeratorAccion.CargueProyectosMasivos, pUsuarioModifico, ex.InnerException.ToString())
                    };
            }

        }
        #endregion
    }
}
