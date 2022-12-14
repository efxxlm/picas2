using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Constants;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class RegisterPreContructionPhase1Service : IRegisterPreContructionPhase1Service
    {
        #region Constructor
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public RegisterPreContructionPhase1Service(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }
        #endregion

        #region Get
        public async Task<List<VRegistrarFase1>> GetListContratacion2(int pAuthor)
        {
            return await _context.VRegistrarFase1
                .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString()
                    && r.TieneFasePreconstruccion.Value > 0
                    && (  r.InterventorId == pAuthor
                       || r.ApoyoId == pAuthor
                       || r.SupervisorId == pAuthor 
                    )
                    )
                .OrderByDescending(r => r.FechaAprobacion)
                .ToListAsync();
        }

        public async Task<dynamic> GetListContratacion()
        {
            List<dynamic> listaContrats = new List<dynamic>();
            try
            {
                List<Contrato> listContratos = await _context.Contrato
                  .FromSqlRaw(QuerySql.GetListContratacion)
                  .Include(r => r.ContratoPoliza)
                  .Include(r => r.Contratacion)
                     .ThenInclude(r => r.ContratacionProyecto)
                         .ThenInclude(r => r.Proyecto)
                              .ThenInclude(r => r.ContratoPerfil)
                  .Include(r => r.Contratacion)
                    .ThenInclude(r => r.DisponibilidadPresupuestal)
                                                                   .ToListAsync();

                foreach (var c in listContratos)
                {
                    if (c.ContratoPoliza.FirstOrDefault().FechaAprobacion.HasValue)
                    {
                        int CantidadProyectosConPerfilesAprobados = 0;
                        int CantidadProyectosConPerfilesPendientes = 0;
                        bool RegistroCompleto = false;
                        bool EstaDevuelto = false;
                        if (c.EstaDevuelto.HasValue && (bool)c.EstaDevuelto)
                            EstaDevuelto = true;
                        foreach (var ContratacionProyecto in c.Contratacion.ContratacionProyecto)
                        {
                            if (ContratacionProyecto.Proyecto.ContratoPerfil.Count() == 0)
                                CantidadProyectosConPerfilesPendientes++;
                            else if (ContratacionProyecto.Proyecto.ContratoPerfil.Where(r => !(bool)r.Eliminado).Count() == ContratacionProyecto.Proyecto.ContratoPerfil.Where(r => !(bool)r.Eliminado && r.RegistroCompleto).Count())
                                CantidadProyectosConPerfilesAprobados++;
                            else
                                CantidadProyectosConPerfilesPendientes++;
                        }
                        if (c.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count() == CantidadProyectosConPerfilesAprobados)
                            RegistroCompleto = true;

                        listaContrats.Add(new
                        {
                            c.Contratacion.NumeroSolicitud,
                            c.ContratoId,
                            FechaAprobacion = c.ContratoPoliza.FirstOrDefault().FechaAprobacion.HasValue ? c.ContratoPoliza.FirstOrDefault().FechaAprobacion : DateTime.Now,
                            c.Contratacion.TipoSolicitudCodigo,
                            c.NumeroContrato,
                            CantidadProyectosAsociados = c.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count(),
                            CantidadProyectosRequisitosAprobados = CantidadProyectosConPerfilesAprobados,
                            CantidadProyectosConPerfilesPendientes,
                            EstadoCodigo = c.EstadoVerificacionCodigo,
                            EstaDevuelto,
                            RegistroCompleto
                        });
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return listaContrats;
        }

        public async Task<Contrato> GetContratoByContratoId(int pContratoId)
        {
            try
            {
                List<Dominio> ListParametricas = _context.Dominio.ToList();
                List<Localizacion> Listlocalizacion = _context.Localizacion.ToList();
                Contrato contrato = await _context.Contrato
                    .Where(r => r.ContratoId == pContratoId)
                    .Include(r => r.ContratoObservacion)
                    .Include(r => r.ContratoPoliza)
                    .Include(r => r.Contratacion).ThenInclude(r => r.Contratista)
                    .Include(r => r.Contratacion).ThenInclude(r=> r.DisponibilidadPresupuestal) 
                    .Include(r => r.ContratoPerfil).ThenInclude(r => r.ContratoPerfilObservacion)
                    .Include(r => r.ContratoPerfil).ThenInclude(r => r.ContratoPerfilNumeroRadicado) 
                    .Include(r => r.Contratacion).ThenInclude(r => r.ContratacionProyecto).ThenInclude(r => r.Proyecto).ThenInclude(r => r.InstitucionEducativa)
                    .Include(r => r.Contratacion).ThenInclude(r => r.ContratacionProyecto).ThenInclude(r => r.Proyecto).ThenInclude(r => r.Sede) 
                    .FirstOrDefaultAsync();

                foreach (var ContratacionProyecto in contrato.Contratacion.ContratacionProyecto)
                {
                    if (ContratacionProyecto.Proyecto.ContratoPerfil.Count() > 0)
                        ContratacionProyecto.Proyecto.ContratoPerfil = ContratacionProyecto.Proyecto.ContratoPerfil.Where(t => !(bool)t.Eliminado).ToList();

                    foreach (var ContratoPerfil in ContratacionProyecto.Proyecto.ContratoPerfil)
                    {
                        if (ContratoPerfil.ContratoPerfilObservacion.Count() > 0)
                            ContratoPerfil.ContratoPerfilObservacion = ContratoPerfil.ContratoPerfilObservacion.Where(r => !(bool)r.Eliminado).ToList();

                        if (ContratoPerfil.ContratoPerfilNumeroRadicado.Count() > 0)
                            ContratoPerfil.ContratoPerfilNumeroRadicado = ContratoPerfil.ContratoPerfilNumeroRadicado.Where(r => !(bool)r.Eliminado).ToList();
                    }
                }

                foreach (var ContratacionProyecto in contrato.Contratacion.ContratacionProyecto)
                {
                    Localizacion Municipio = Listlocalizacion.Where(r => r.LocalizacionId == ContratacionProyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    ContratacionProyecto.Proyecto.Departamento = Listlocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault().Descripcion;
                    ContratacionProyecto.Proyecto.Municipio = Municipio.Descripcion;
                    ContratacionProyecto.Proyecto.TipoIntervencionCodigo = ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && r.Codigo == ContratacionProyecto.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                }
                contrato.FechaAprobacionPoliza = _context.ContratoPoliza.Where(c => c.ContratoId == contrato.ContratoId).Select(c => c.FechaAprobacion).FirstOrDefault();
               
                return contrato;
            }
            catch (Exception ex)
            {
                return new Contrato();
            }
        }
        #endregion

        #region CRUD
        public async Task<Respuesta> CreateEditContratoPerfil(Contrato pContrato)
        {
            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Contrato_Perfil, (int)EnumeratorTipoDominio.Acciones);
            bool RegistroCompletoContrato = true;
            try
            {
                foreach (var ContratacionProyecto in pContrato.Contratacion.ContratacionProyecto)
                {
                    //Guardar estado de la fase 1 preConstruccion 
                    if (ContratacionProyecto.Proyecto.TieneEstadoFase1Diagnostico != null || ContratacionProyecto.Proyecto.TieneEstadoFase1EyD != null)
                    {
                        Proyecto proyectoOld = _context.Proyecto.Find(ContratacionProyecto.Proyecto.ProyectoId);
                        proyectoOld.TieneEstadoFase1Diagnostico = ContratacionProyecto.Proyecto.TieneEstadoFase1Diagnostico;
                        proyectoOld.TieneEstadoFase1EyD = ContratacionProyecto.Proyecto.TieneEstadoFase1EyD;
                        proyectoOld.FechaModificacion = DateTime.Now;
                        proyectoOld.UsuarioModificacion = pContrato.UsuarioCreacion;
                    }
                    if (ContratacionProyecto.Proyecto.ContratoPerfil.Count() == 0)
                        RegistroCompletoContrato = false;

                    foreach (var ContratoPerfil in ContratacionProyecto.Proyecto.ContratoPerfil)
                    {
                        if (ContratoPerfil.ContratoPerfilId > 0)
                        {
                            CreateEdit = "EDITAR CONTRATO PERFIL";
                            ContratoPerfil contratoPerfilOld = _context.ContratoPerfil.Find(ContratoPerfil.ContratoPerfilId);
                            contratoPerfilOld.Observacion = ContratoPerfil.Observacion;
                            contratoPerfilOld.ContratoPerfilId = ContratoPerfil.ContratoPerfilId;
                            contratoPerfilOld.PerfilCodigo = ContratoPerfil.PerfilCodigo;
                            contratoPerfilOld.CantidadHvRequeridas = ContratoPerfil.CantidadHvRequeridas;
                            contratoPerfilOld.CantidadHvRecibidas = ContratoPerfil.CantidadHvRecibidas;
                            contratoPerfilOld.CantidadHvAprobadas = ContratoPerfil.CantidadHvAprobadas;
                            contratoPerfilOld.FechaAprobacion = ContratoPerfil.FechaAprobacion;
                            contratoPerfilOld.RutaSoporte = ContratoPerfil.RutaSoporte;
                            contratoPerfilOld.TieneObservacionApoyo = ContratoPerfil.TieneObservacionApoyo;
                            contratoPerfilOld.RegistroCompleto = ValidarRegistroCompletoContratoPerfil(ContratoPerfil);
                            contratoPerfilOld.TieneObservacionSupervisor = ContratoPerfil.TieneObservacionSupervisor;
                            contratoPerfilOld.RegistroCompletoPerfilesProyecto = ValidarRegistroCompletoPerfilesProyecto(ContratacionProyecto.Proyecto);
                            if (contratoPerfilOld.RegistroCompleto == false)
                                RegistroCompletoContrato = false;

                            foreach (var ContratoPerfilObservacion in ContratoPerfil.ContratoPerfilObservacion)
                            {
                                if (ContratoPerfilObservacion.ContratoPerfilObservacionId > 0)
                                {
                                    ContratoPerfilObservacion contratoPerfilObservacionOld = _context.ContratoPerfilObservacion.Find(ContratoPerfilObservacion.ContratoPerfilObservacionId);
                                    contratoPerfilObservacionOld.UsuarioModificacion = pContrato.UsuarioCreacion;
                                    contratoPerfilObservacionOld.FechaModificacion = DateTime.Now;
                                    contratoPerfilObservacionOld.Eliminado = false;
                                    contratoPerfilObservacionOld.TipoObservacionCodigo = ContratoPerfilObservacion.TipoObservacionCodigo;
                                    contratoPerfilObservacionOld.Observacion = ContratoPerfilObservacion.Observacion?.ToUpper();
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(ContratoPerfilObservacion.Observacion))
                                    {
                                        ContratoPerfilObservacion.Observacion = ContratoPerfilObservacion.Observacion.ToUpper();
                                        ContratoPerfilObservacion.TipoObservacionCodigo = ConstanCodigoTipoObservacion.Interventoria;
                                        ContratoPerfilObservacion.UsuarioCreacion = pContrato.UsuarioCreacion;
                                        ContratoPerfilObservacion.FechaCreacion = DateTime.Now;
                                        ContratoPerfilObservacion.Eliminado = false;
                                        _context.ContratoPerfilObservacion.Add(ContratoPerfilObservacion);
                                    }
                                }
                            }

                            foreach (var ContratoPerfilNumeroRadicado in ContratoPerfil.ContratoPerfilNumeroRadicado)
                            {
                                if (ContratoPerfilNumeroRadicado.ContratoPerfilNumeroRadicadoId == 0)
                                {
                                    ContratoPerfilNumeroRadicado.Eliminado = false;
                                    ContratoPerfilNumeroRadicado.UsuarioCreacion = pContrato.UsuarioCreacion;
                                    ContratoPerfilNumeroRadicado.FechaCreacion = DateTime.Now;
                                    _context.ContratoPerfilNumeroRadicado.Add(ContratoPerfilNumeroRadicado);
                                }
                                else
                                {
                                    ContratoPerfilNumeroRadicado contratoPerfilNumeroRadicadoOld = _context.ContratoPerfilNumeroRadicado.Find(ContratoPerfilNumeroRadicado.ContratoPerfilNumeroRadicadoId);
                                    contratoPerfilNumeroRadicadoOld.NumeroRadicado = ContratoPerfilNumeroRadicado.NumeroRadicado;
                                    contratoPerfilNumeroRadicadoOld.UsuarioModificacion = pContrato.UsuarioCreacion;
                                    contratoPerfilNumeroRadicadoOld.FechaModificacion = DateTime.Now;
                                }
                            }
                        }
                        else
                        {
                            CreateEdit = "CREAR CONTRATO PERFIL";
                            ContratoPerfil.UsuarioCreacion = pContrato.UsuarioCreacion;
                            ContratoPerfil.FechaCreacion = DateTime.Now;
                            ContratoPerfil.RegistroCompletoPerfilesProyecto = ValidarRegistroCompletoPerfilesProyecto(ContratacionProyecto.Proyecto); 
                            ContratoPerfil.Eliminado = false; 
                            ContratoPerfil.RegistroCompleto = ValidarRegistroCompletoContratoPerfil(ContratoPerfil);
                            _context.ContratoPerfil.Add(ContratoPerfil);

                            if (ContratoPerfil.RegistroCompleto == false)
                                RegistroCompletoContrato = false;

                            foreach (var ContratoPerfilObservacion in ContratoPerfil.ContratoPerfilObservacion)
                            {
                                ContratoPerfilObservacion.Observacion = ContratoPerfilObservacion.Observacion == null ? null : ContratoPerfilObservacion.Observacion.ToUpper();
                                ContratoPerfilObservacion.UsuarioCreacion = pContrato.UsuarioCreacion;
                                ContratoPerfilObservacion.FechaCreacion = DateTime.Now;
                                ContratoPerfilObservacion.TipoObservacionCodigo = ConstanCodigoTipoObservacion.Interventoria;
                                ContratoPerfilObservacion.Eliminado = false;

                                _context.ContratoPerfilObservacion.Add(ContratoPerfilObservacion);
                            }
                            ContratoPerfil.ContratoPerfilNumeroRadicado.ToList().ForEach(ContratoPerfilNumeroRadicado =>
                            {
                                if (ContratoPerfilNumeroRadicado.ContratoPerfilNumeroRadicadoId == 0)
                                {
                                    ContratoPerfilNumeroRadicado.Eliminado = false;
                                    ContratoPerfilNumeroRadicado.UsuarioCreacion = pContrato.UsuarioCreacion;
                                    ContratoPerfilNumeroRadicado.FechaCreacion = DateTime.Now;
                                    _context.ContratoPerfilNumeroRadicado.Add(ContratoPerfilNumeroRadicado);
                                }
                                else
                                {
                                    ContratoPerfilNumeroRadicado contratoPerfilNumeroRadicadoOld = _context.ContratoPerfilNumeroRadicado.Find(ContratoPerfilNumeroRadicado.ContratoPerfilNumeroRadicadoId);
                                    contratoPerfilNumeroRadicadoOld.NumeroRadicado = ContratoPerfilNumeroRadicado.NumeroRadicado;
                                    ContratoPerfilNumeroRadicado.UsuarioModificacion = pContrato.UsuarioCreacion;
                                    ContratoPerfilNumeroRadicado.FechaModificacion = DateTime.Now;
                                }
                            });
                        }
                    }
                }

                //Cambiar Estado Contrato 
                Contrato contratoOld = _context.Contrato.
                        Where(r => r.ContratoId == pContrato.ContratoId)
                        .Include(r => r.Contratacion)
                        .Include(r => r.ContratoPerfil)
                        .FirstOrDefault();

                contratoOld.UsuarioModificacion = pContrato.UsuarioCreacion;
                contratoOld.FechaModificacion = DateTime.Now;

                if (contratoOld.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                    contratoOld.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.En_proceso_de_aprobacion_de_requisitos_tecnicos;

                else
                {
                    contratoOld.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.En_proceso_de_verificacion_de_requisitos_tecnicos;

                    if (contratoOld.ContratoPerfil.Count() > 0 && RegistroCompletoContrato)
                        contratoOld.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_verificados;
                }
                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = RegisterPreContructionPhase1.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                                                                    (int)enumeratorMenu.Preconstruccion_Fase_1,
                                                                                                    RegisterPreContructionPhase1.OperacionExitosa,
                                                                                                    idAccion,
                                                                                                    pContrato.UsuarioCreacion,
                                                                                                    CreateEdit
                                                                                                )
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
                        Code = RegisterPreContructionPhase1.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.Error, idAccion, pContrato.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        private bool  ValidarRegistroCompletoPerfilesProyecto(Proyecto proyecto)
        {
            foreach (var ContratoPerfil in proyecto.ContratoPerfil)
            {
                if (!ValidarRegistroCompletoContratoPerfil(ContratoPerfil))
                    return false;
            } 
            return true;
        }

        public async Task<Respuesta> DeleteContratoPerfil(int ContratoPerfilId, string UsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Contrato_Perfil, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                _context.Set<ContratoPerfil>().Where(cp => cp.ContratoPerfilId == ContratoPerfilId)
                                                                                                .Update(cp => new ContratoPerfil
                                                                                                {
                                                                                                    Eliminado = true,
                                                                                                    UsuarioModificacion = UsuarioModificacion,
                                                                                                    FechaModificacion = DateTime.Now,
                                                                                                });
                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = RegisterPreContructionPhase1.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.OperacionExitosa, idAccion, UsuarioModificacion, "CONTRATO PERFIL ELIMINADO")
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
                        Code = RegisterPreContructionPhase1.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.Error, idAccion, UsuarioModificacion, ex.InnerException.ToString().ToUpper())
                    };
            }
        }

        private bool ValidarRegistroCompletoContratoPerfil(ContratoPerfil contratoPerfilOld)
        {
            if (
                    string.IsNullOrEmpty(contratoPerfilOld.PerfilCodigo)
                 || contratoPerfilOld.CantidadHvRequeridas == 0
                 || contratoPerfilOld.CantidadHvRecibidas == 0
                 || contratoPerfilOld.CantidadHvAprobadas == 0
                 || string.IsNullOrEmpty(contratoPerfilOld.FechaAprobacion.ToString())
                 || string.IsNullOrEmpty(contratoPerfilOld.RutaSoporte)
                   || string.IsNullOrEmpty(contratoPerfilOld.Observacion)
                )
                return false;

            return true;
        }

        public async Task<Respuesta> DeleteContratoPerfilNumeroRadicado(int ContratoPerfilNumeroRadicadoId, string UsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Numero_Radicado, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ContratoPerfilNumeroRadicado contratoPerfilNumeroRadicadoOld = _context.ContratoPerfilNumeroRadicado.Find(ContratoPerfilNumeroRadicadoId);
                contratoPerfilNumeroRadicadoOld.Eliminado = true;
                contratoPerfilNumeroRadicadoOld.UsuarioModificacion = UsuarioModificacion;
                contratoPerfilNumeroRadicadoOld.FechaModificacion = DateTime.Now;

                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = RegisterPreContructionPhase1.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.OperacionExitosa, idAccion, UsuarioModificacion, "Contrato Perfil Numero Radicado".ToUpper())
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
                        Code = RegisterPreContructionPhase1.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.Error, idAccion, UsuarioModificacion, ex.InnerException.ToString().ToUpper())
                    };
            }
        }

        public async Task<Respuesta> ChangeStateContrato(int pContratoId, string UsuarioModificacion, string pEstadoVerificacionContratoCodigo, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Verificacion_Contrato, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contrato contratoMod = _context.Contrato.Where(r => r.ContratoId == pContratoId)
                    .Include(r => r.ContratoPerfil)
                    .Include(r => r.ContratoPoliza)
                    .Include(r => r.Contratacion)
                        .ThenInclude(r => r.ContratacionProyecto)
                    .FirstOrDefault();

                contratoMod.FechaModificacion = DateTime.Now;
                contratoMod.UsuarioModificacion = UsuarioModificacion;
                contratoMod.EstadoVerificacionCodigo = pEstadoVerificacionContratoCodigo;

                if (pEstadoVerificacionContratoCodigo == ConstanCodigoEstadoContrato.Enviado_al_interventor || pEstadoVerificacionContratoCodigo == ConstanCodigoEstadoContrato.Enviado_al_apoyo)
                    contratoMod.EstaDevuelto = true;

                //Enviar Correo Botón aprobar inicio 3.1.6
                if (pEstadoVerificacionContratoCodigo == ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_aprobados && contratoMod.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                {
                    GetReiniciarObservaciones(contratoMod);
                    await EnviarCorreo(contratoMod, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);
                    contratoMod.FechaAprobacionRequisitosInterventor = DateTime.Now;
                }

                //Enviar Correo Botón aprobar inicio 3.1.7
                if (pEstadoVerificacionContratoCodigo == ConstanCodigoEstadoContrato.Enviado_al_supervisor && contratoMod.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                {
                    GetReiniciarObservaciones(contratoMod);
                    GetDeleteTieneObservacionSupervisor(pContratoId);
                    await EnviarCorreoSupervisor(ConstanCodigoTipoContratacionSTRING.Interventoria, contratoMod, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);
                    contratoMod.FechaAprobacionRequisitosApoyo = DateTime.Now;
                }

                //Enviar Correo Botón aprobar inicio 3.1.7
                if (pEstadoVerificacionContratoCodigo == ConstanCodigoEstadoContrato.Enviado_al_supervisor && contratoMod.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                {
                    GetDeleteTieneObservacionSupervisor(pContratoId);
                    await EnviarCorreoSupervisor(ConstanCodigoTipoContratacionSTRING.Obra, contratoMod, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);
                    contratoMod.FechaAprobacionRequisitosApoyo = DateTime.Now;
                }

                //Enviar Correo Botón aprobar inicio 3.1.8
                if (pEstadoVerificacionContratoCodigo == ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_aprobados_por_supervisor)
                {
                    string strTipoContrato = contratoMod.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString() ? ConstanCodigoTipoContratacionSTRING.Obra : ConstanCodigoTipoContratacionSTRING.Interventoria;
                    await EnviarCorreoSupervisorAprobar(strTipoContrato, contratoMod, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);
                    contratoMod.FechaAprobacionRequisitosSupervisor = DateTime.Now;
                }

                //Enviar Correo Botón aprobar inicio 3.1.8
                if (pEstadoVerificacionContratoCodigo == ConstanCodigoEstadoContrato.Enviado_al_interventor)
                {
                    foreach (var ContratoPerfil in contratoMod.ContratoPerfil)
                    {
                        ContratoPerfil contratoPerfilOld = _context.ContratoPerfil.Find(ContratoPerfil.ContratoPerfilId);

                        if (contratoPerfilOld.TieneObservacionSupervisor == true)
                        {
                            contratoPerfilOld.FechaModificacion = DateTime.Now;
                            contratoPerfilOld.RegistroCompleto = false;
                            contratoPerfilOld.UsuarioModificacion = UsuarioModificacion;
                        }
                        _context.Update(contratoPerfilOld);
                    }
                    await EnviarCorreoSupervisor(ConstanCodigoTipoContratacionSTRING.Obra, contratoMod, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);
                }

                if (pEstadoVerificacionContratoCodigo == ConstanCodigoEstadoContrato.Enviado_al_apoyo)
                {
                    //se reinicia los contadores 
                    contratoMod.RegistroCompleto = false;
                    await EnviarCorreoSupervisor(ConstanCodigoTipoContratacionSTRING.Obra, contratoMod, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);
                }

                //Logica para devoluciones 
                if (pEstadoVerificacionContratoCodigo == ConstanCodigoEstadoContrato.Enviado_al_supervisor)
                {
                    foreach (var ContratoPerfil in contratoMod.ContratoPerfil)
                    {
                        ContratoPerfil contratoPerfilOld = _context.ContratoPerfil.Find(ContratoPerfil.ContratoPerfilId);
                        contratoPerfilOld.FechaModificacion = DateTime.Now;
                        contratoPerfilOld.TieneObservacionSupervisor = null;
                        contratoPerfilOld.UsuarioModificacion = UsuarioModificacion;

                        _context.Update(contratoPerfilOld);
                    }
                }

                //Logica de actas cuando se aprueba
                if (pEstadoVerificacionContratoCodigo == ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_aprobados_por_supervisor)
                {
                    if (contratoMod.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                        contratoMod.EstadoActa = ConstanCodigoEstadoActaContrato.Sin_Revision;
                    else
                        contratoMod.EstadoActa = ConstanCodigoEstadoActaContrato.Sin_acta_generada;
                }

                _context.SaveChanges();

                string NombreEstadoMod = _context.Dominio.Where(r => r.Codigo == pEstadoVerificacionContratoCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Verificacion_Contrato).FirstOrDefault().Nombre;

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = RegisterPreContructionPhase1.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.OperacionExitosa, idAccion, UsuarioModificacion, "EL CONTRATO N°: " + contratoMod.NumeroContrato + "CAMBIO A ESTADO DE VERIFICACION " + NombreEstadoMod.ToUpper())
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
                        Code = RegisterPreContructionPhase1.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.Error, idAccion, UsuarioModificacion, ex.InnerException.ToString().ToUpper())
                    };
            }
        }

        private void GetReiniciarObservaciones(Contrato contratoMod)
        {
            _context.Set<ContratoPerfilObservacion>()
                    .Where(c =>
                    c.ContratoPerfilId == contratoMod.ContratoPerfil.FirstOrDefault().ContratoPerfilId
                    && c.TipoObservacionCodigo != "1")
                    .Update(c => new ContratoPerfilObservacion
                    {
                        Eliminado = true,
                        UsuarioCreacion = contratoMod.UsuarioModificacion,
                        FechaModificacion = DateTime.Now
                    });
        }

        private void GetDeleteTieneObservacionSupervisor(int pContratoId)
        {
            _context.ContratoPerfil.Where(cp => cp.ContratoId == pContratoId).ToList().ForEach(ContratoPerfil =>
            {
                _context.Set<ContratoPerfil>()
                        .Where(c => c.ContratoPerfilId == ContratoPerfil.ContratoPerfilId)
                        .Update(
                                  c => new ContratoPerfil
                                  {
                                      TieneObservacionSupervisor = null,
                                      FechaModificacion = DateTime.Now
                                  });
            });
        }
        #endregion

        #region Correos y Alertas Automaticas
        /// <summary>
        /// Correos  Automaticos
        /// </summary>
        /// <returns></returns>
        public async Task<Respuesta> AprobarInicio(int pContratoId, string UsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Aprobar_Inicio_Contrato, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contrato contratoAprobar = _context.Contrato.Find(pContratoId);
                contratoAprobar.FechaModificacion = DateTime.Now;
                contratoAprobar.UsuarioModificacion = UsuarioModificacion;
                contratoAprobar.EstadoVerificacionCodigo = ConstanCodigoEstadoVerificacionContratoObra.Con_requisitos_del_contratista_de_obra_avalados;
                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = RegisterPreContructionPhase1.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.OperacionExitosa, idAccion, UsuarioModificacion, "APROBAR INICIO CONTRATO")
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
                        Code = RegisterPreContructionPhase1.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.Error, idAccion, UsuarioModificacion, ex.InnerException.ToString().ToUpper())
                    };
            }
        }

        private async Task<bool> EnviarCorreoSupervisorAprobar(string pTipoContrato, Contrato contratoMod, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            List<EnumeratorPerfil> enumeratorPerfils = new List<EnumeratorPerfil>
            {
                EnumeratorPerfil.Supervisor
            };
            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.AprobadoInterventoriaObra3_1_8_);

            string template = TemplateRecoveryPassword.Contenido
                .Replace("_LinkF_", pDominioFront)
                .Replace("[TIPO_CONTRATO]", pTipoContrato)
                .Replace("[NUMERO_CONTRATO]", contratoMod.NumeroContrato)
                .Replace("[FECHA_VERIFICACION]", ((DateTime.Now)).ToString("dd-MM-yy"))
                .Replace("[CANTIDAD_PROYECTOS]", contratoMod.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());
             
            return _commonService.EnviarCorreo(enumeratorPerfils, template, TemplateRecoveryPassword.Asunto);
        }

        private async Task<bool> EnviarCorreo(Contrato contratoMod, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            List<EnumeratorPerfil> enumeratorPerfils = new List<EnumeratorPerfil>
            {
                EnumeratorPerfil.Supervisor
            };
            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.AprobarInicio316);

            string template = TemplateRecoveryPassword.Contenido
                .Replace("_LinkF_", pDominioFront)
                .Replace("[NUMERO_CONTRATO]", contratoMod.NumeroContrato)
                .Replace("[FECHA_POLIZA]", ((DateTime)contratoMod.ContratoPoliza.FirstOrDefault().FechaAprobacion).ToString("dd-MM-yy"))
                .Replace("[CANTIDAD_PROYECTOS]", contratoMod.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());
             
            return _commonService.EnviarCorreo(enumeratorPerfils, template, TemplateRecoveryPassword.Asunto);
        }

        private async Task<bool> EnviarCorreoSupervisor(string pTipoContrato, Contrato contratoMod, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            List<EnumeratorPerfil> enumeratorPerfils = new List<EnumeratorPerfil>
            {
                EnumeratorPerfil.Supervisor
            };

            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.EnviarSupervisor317);

            string template = TemplateRecoveryPassword.Contenido
                .Replace("_LinkF_", pDominioFront)
                .Replace("[TIPO_CONTRATO]", pTipoContrato)
                .Replace("[NUMERO_CONTRATO]", contratoMod.NumeroContrato)
                .Replace("[FECHA_VERIFICACION]", ((DateTime.Now)).ToString("dd-MM-yy"))
                .Replace("[CANTIDAD_PROYECTOS]", contratoMod.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());
             
            return _commonService.EnviarCorreo(enumeratorPerfils, template, TemplateRecoveryPassword.Asunto);
        }

        /// <summary>
        /// Notificaciones Automaticas
        /// </summary>
        /// 
        //3.1.6
        public async Task EnviarNotificacion(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            DateTime RangoFechaConDiasHabiles = await _commonService.CalculardiasLaborales(4, DateTime.Now);

            List<Contrato> contratos = _context.Contrato
                .Where(r => r.EstadoVerificacionCodigo == ConstanCodigoEstadoContrato.Sin_aprobacion_de_requisitos_tecnicos || r.EstadoVerificacionCodigo == ConstanCodigoEstadoContrato.En_proceso_de_aprobacion_de_requisitos_tecnicos || !string.IsNullOrEmpty(r.EstadoVerificacionCodigo))
                 .Include(r => r.ContratoPoliza)
                 .Include(r => r.Contratacion)
                   .ThenInclude(r => r.DisponibilidadPresupuestal)
               .ToList();

            List<EnumeratorPerfil> enumeratorPerfils = new List<EnumeratorPerfil>
            {
                EnumeratorPerfil.Supervisor,
                EnumeratorPerfil.Interventor
            };

            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.AprobarPoliza4diasNoGestion);
            foreach (var contrato in contratos)
            {
                if (contrato.ContratoPoliza.Count() > 0 && contrato?.ContratoPoliza?.FirstOrDefault().FechaAprobacion > RangoFechaConDiasHabiles && contrato.Contratacion.TipoContratacionCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                {
                    if (!string.IsNullOrEmpty(contrato.Contratacion.DisponibilidadPresupuestal.LastOrDefault().NumeroDrp))
                    {
                        string template = TemplateRecoveryPassword.Contenido
                                    .Replace("_LinkF_", pDominioFront)
                                    .Replace("[NUMERO_CONTRATO]", contrato.NumeroContrato)
                                    .Replace("[FECHA_POLIZA]", ((DateTime)contrato.ContratoPoliza.FirstOrDefault().FechaAprobacion).ToString("dd-MMM-yy"))
                                    .Replace("[CANTIDAD_PROYECTOS]", contrato.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());

                        _commonService.EnviarCorreo(enumeratorPerfils, template, TemplateRecoveryPassword.Asunto);
                    }
                }
            }
        }

        //3.1.7 (28)
        public async Task GetContratosIntrerventoriaSinGestionar(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            DateTime RangoFechaConDiasHabiles = await _commonService.CalculardiasLaborales(2, DateTime.Now);

            List<Contrato> contratos = _context.Contrato
                .Where(r => r.EstadoVerificacionCodigo == ConstanCodigoEstadoContrato.Enviado_al_supervisor)
                 .Include(r => r.ContratoPoliza)
                 .Include(r => r.Contratacion)
                   .ThenInclude(r => r.DisponibilidadPresupuestal)
               .ToList();

            List<EnumeratorPerfil> enumeratorPerfils = new List<EnumeratorPerfil>
            {
                EnumeratorPerfil.Supervisor,
                EnumeratorPerfil.Interventor,
                EnumeratorPerfil.Tecnica
            };
             Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.AlertaSupervisor317_2dias);

            foreach (var contrato in contratos)
            {
                if (contrato.ContratoPoliza.Count() > 0 && contrato.FechaAprobacionRequisitosInterventor > RangoFechaConDiasHabiles)
                {
                    string template = TemplateRecoveryPassword.Contenido
                                .Replace("_LinkF_", pDominioFront)
                                .Replace("[NUMERO_CONTRATO]", contrato.NumeroContrato)
                                .Replace("[FECHA_POLIZA]", ((DateTime)contrato.ContratoPoliza.FirstOrDefault().FechaAprobacion).ToString("dd-MMM-yy"))
                                .Replace("[CANTIDAD_PROYECTOS]", contrato.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());
                    _commonService.EnviarCorreo  (enumeratorPerfils, template, TemplateRecoveryPassword.Asunto);
                }
            }
        }
        //3.1.7 (30)
        public async Task EnviarNotificacionInteventoria(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            DateTime RangoFechaConDiasHabiles = await _commonService.CalculardiasLaborales(4, DateTime.Now);

            List<Contrato> contratos = _context.Contrato
                .Where(r => !string.IsNullOrEmpty(r.EstadoVerificacionCodigo))
                 .Include(r => r.ContratoPoliza)
                 .Include(r => r.Contratacion)
                   .ThenInclude(r => r.DisponibilidadPresupuestal)
               .ToList();

            List<EnumeratorPerfil> enumeratorPerfils = new List<EnumeratorPerfil>
            {
                EnumeratorPerfil.Supervisor,
                EnumeratorPerfil.Interventor,
                EnumeratorPerfil.Tecnica
            };
            
            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.AlertaPolizas317_4dias);
            foreach (var contrato in contratos)
            {
                if (contrato.ContratoPoliza.Count() > 0 && contrato?.ContratoPoliza?.FirstOrDefault().FechaAprobacion > RangoFechaConDiasHabiles && contrato.Contratacion.TipoContratacionCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                {
                    if (!string.IsNullOrEmpty(contrato.Contratacion.DisponibilidadPresupuestal.LastOrDefault().NumeroDrp))
                    {
                        string template = TemplateRecoveryPassword.Contenido
                                    .Replace("_LinkF_", pDominioFront)
                                    .Replace("[NUMERO_CONTRATO]", contrato.NumeroContrato)
                                    .Replace("[FECHA_POLIZA]", ((DateTime)contrato.ContratoPoliza.FirstOrDefault().FechaAprobacion).ToString("dd-MMM-yy"))
                                    .Replace("[CANTIDAD_PROYECTOS]", contrato.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());

                        _commonService.EnviarCorreo(enumeratorPerfils, template, TemplateRecoveryPassword.Asunto);
                    }
                }
            }
        }
        //3.1.8() OBRA
        public async Task GetContratosObraSinGestionar(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            DateTime RangoFechaConDiasHabiles = await _commonService.CalculardiasLaborales(2, DateTime.Now);
            //" 3 - 4 -5" obra 
            List<Contrato> contratos = _context.Contrato
                .Where(r => r.EstadoVerificacionCodigo == ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_aprobados || r.EstadoVerificacionCodigo == ConstanCodigoEstadoContrato.En_proceso_de_verificacion_de_requisitos_tecnicos || r.EstadoVerificacionCodigo == ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_verificados)
                 .Include(r => r.ContratoPoliza)
                 .Include(r => r.Contratacion)
                   .ThenInclude(r => r.DisponibilidadPresupuestal)
               .ToList();

            List<EnumeratorPerfil> enumeratorPerfils = new List<EnumeratorPerfil>
            {
                EnumeratorPerfil.Supervisor, 
                EnumeratorPerfil.Tecnica
            };

            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.Alertas2DiasObraInterventoria318);

            foreach (var contrato in contratos)
            {

                if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                {
                    DateTime fechaValidacion = contrato.FechaAprobacionRequisitosApoyo.HasValue ? ((DateTime)contrato.FechaAprobacionRequisitosApoyo) : ((DateTime)contrato.FechaModificacion);

                    if (contrato.ContratoPoliza.Count() > 0 && fechaValidacion > RangoFechaConDiasHabiles)
                    {
                        string template = TemplateRecoveryPassword.Contenido
                                    .Replace("_LinkF_", pDominioFront)
                                    .Replace("[NUMERO_CONTRATO]", contrato.NumeroContrato)
                                    .Replace("[FECHA_REQUISITOS]", fechaValidacion.ToString("dd-MM-yy"))
                                    .Replace("[CANTIDAD_PROYECTOS]", contrato.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());

                        _commonService.EnviarCorreo(enumeratorPerfils, template, TemplateRecoveryPassword.Asunto);
                    }
                }
            }
        }
        //3.1.8()
        public async Task GetContratosInterventoriaSinGestionar(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            DateTime RangoFechaConDiasHabiles = await _commonService.CalculardiasLaborales(2, DateTime.Now);
            // interventoria sin registro 0 4 5
            List<Contrato> contratos = _context.Contrato
                .Where(r => r.EstadoVerificacionCodigo == ConstanCodigoEstadoContrato.En_proceso_de_verificacion_de_requisitos_tecnicos || r.EstadoVerificacionCodigo == ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_verificados || !string.IsNullOrEmpty(r.EstadoVerificacionCodigo))
                 .Include(r => r.ContratoPoliza)
                 .Include(r => r.Contratacion)
                   .ThenInclude(r => r.DisponibilidadPresupuestal)
               .ToList();

            List<EnumeratorPerfil> enumeratorPerfils = new List<EnumeratorPerfil>
            {
                EnumeratorPerfil.Supervisor,
                EnumeratorPerfil.Tecnica
            };
            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.Alertas2DiasObraInterventoria318);

            foreach (var contrato in contratos)
            { 
                if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                {
                    DateTime fechaValidacion = contrato.FechaAprobacionRequisitosApoyo.HasValue ? ((DateTime)contrato.FechaAprobacionRequisitosApoyo) : ((DateTime)contrato.FechaModificacion);

                    if (contrato.ContratoPoliza.Count() > 0 && fechaValidacion > RangoFechaConDiasHabiles)
                    {
                        string template = TemplateRecoveryPassword.Contenido
                                 .Replace("_LinkF_", pDominioFront)
                                 .Replace("[NUMERO_CONTRATO]", contrato.NumeroContrato)
                                 .Replace("[FECHA_REQUISITOS]", fechaValidacion.ToString("dd-MM-yy"))
                                 .Replace("[CANTIDAD_PROYECTOS]", contrato.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());
                     
                        _commonService.EnviarCorreo(enumeratorPerfils, template, TemplateRecoveryPassword.Asunto);
                    }
                }
            }
        }

        #endregion
    }
}