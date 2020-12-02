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

namespace asivamosffie.services
{
    public class RegisterPreContructionPhase1Service : IRegisterPreContructionPhase1Service
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public RegisterPreContructionPhase1Service(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }
        public async Task<List<VRegistrarFase1>> GetListContratacion2()
        {
            return await _context.VRegistrarFase1.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString() && r.TieneFasePreconstruccion.Value > 0).OrderByDescending(r => r.FechaAprobacion).ToListAsync();
        }

        public async Task<dynamic> GetListContratacion()
        {
            List<dynamic> listaContrats = new List<dynamic>();
            try
            {
                List<Contrato> listContratos = await _context.Contrato
              .FromSqlRaw("SELECT c.* FROM dbo.Contrato AS c " +
              "INNER JOIN dbo.Contratacion AS ctr ON c.ContratacionId = ctr.ContratacionId " +
              "INNER JOIN dbo.DisponibilidadPresupuestal AS dp ON ctr.ContratacionId = dp.ContratacionId " +
              "INNER JOIN dbo.ContratoPoliza AS cp ON c.ContratoId = cp.ContratoId " +
              "WHERE dp.NumeroDRP IS NOT NULL " +     //Documento Registro Presupuestal
              "AND cp.FechaAprobacion is not null " + //Fecha Aprobacion Poliza
              "AND ctr.TipoSolicitudCodigo = 1" +   //Solo contratos Tipo Obra
              "OR  c.EstadoVerificacionCodigo = 1" +  //Sin aprobación de requisitos técnicos
              "OR  c.EstadoVerificacionCodigo = 2" +  //En proceso de aprobación de requisitos técnicos
              "OR  c.EstadoVerificacionCodigo = 3" +  //Con requisitos técnicos aprobados
              "OR  c.EstadoVerificacionCodigo = 4" +  //Con requisitos técnicos aprobados
              "OR  c.EstadoVerificacionCodigo = 10")  //Enviado al interventor -- Enviado por el supervisor

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
                            else if (ContratacionProyecto.Proyecto.ContratoPerfil.Count(r => !(bool)r.Eliminado) == ContratacionProyecto.Proyecto.ContratoPerfil.Count(r => !(bool)r.Eliminado && r.RegistroCompleto))
                                CantidadProyectosConPerfilesAprobados++;
                            else
                                CantidadProyectosConPerfilesPendientes++;
                        }
                        if (c.Contratacion.ContratacionProyecto.Count(r => !r.Eliminado) == CantidadProyectosConPerfilesAprobados)
                            RegistroCompleto = true;

                        listaContrats.Add(new
                        {
                            c.Contratacion.NumeroSolicitud,
                            c.ContratoId,
                            FechaAprobacion = c.ContratoPoliza.FirstOrDefault().FechaAprobacion.HasValue ? c.ContratoPoliza.FirstOrDefault().FechaAprobacion : DateTime.Now,
                            c.Contratacion.TipoSolicitudCodigo,
                            c.NumeroContrato,
                            CantidadProyectosAsociados = c.Contratacion.ContratacionProyecto.Count(r => !r.Eliminado),
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

                throw;
            }

            return listaContrats;
        }

        public async Task<Contrato> GetContratoByContratoId(int pContratoId)
        {
            try
            {
                List<Dominio> ListParametricas = _context.Dominio.ToList();
                List<Localizacion> Listlocalizacion = _context.Localizacion.ToList();
                Contrato contrato = await _context.Contrato.Where(r => r.ContratoId == pContratoId)
                    .Include(r => r.ContratoObservacion)
                    .Include(r => r.ContratoPerfil)
                           .ThenInclude(r => r.ContratoPerfilObservacion)
                    .Include(r => r.ContratoPerfil)
                           .ThenInclude(r => r.ContratoPerfilNumeroRadicado)
                    .Include(r => r.ContratoPoliza)
                    .Include(r => r.Contratacion)
                       .ThenInclude(r => r.ContratacionProyecto)
                             .ThenInclude(r => r.Proyecto)
                                .ThenInclude(r => r.InstitucionEducativa)
                   .Include(r => r.Contratacion)
                       .ThenInclude(r => r.ContratacionProyecto)
                             .ThenInclude(r => r.Proyecto)
                                 .ThenInclude(r => r.Sede)
                    .Include(r => r.Contratacion)
                        .ThenInclude(r => r.Contratista)
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
                return contrato;
            }
            catch (Exception ex)
            {
                return new Contrato();
            }
        }

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

                    foreach (var ContratoPerfil in ContratacionProyecto.Proyecto.ContratoPerfil)
                    {
                        if (ContratoPerfil.ContratoPerfilId > 0)
                        {
                            CreateEdit = "EDITAR CONTRATO PERFIL";
                            ContratoPerfil contratoPerfilOld = _context.ContratoPerfil.Find(ContratoPerfil.ContratoPerfilId);
                            contratoPerfilOld.ContratoPerfilId = ContratoPerfil.ContratoPerfilId;
                            contratoPerfilOld.PerfilCodigo = ContratoPerfil.PerfilCodigo;
                            contratoPerfilOld.CantidadHvRequeridas = ContratoPerfil.CantidadHvRequeridas;
                            contratoPerfilOld.CantidadHvRecibidas = ContratoPerfil.CantidadHvRecibidas;
                            contratoPerfilOld.CantidadHvAprobadas = ContratoPerfil.CantidadHvAprobadas;
                            contratoPerfilOld.FechaAprobacion = ContratoPerfil.FechaAprobacion;
                            contratoPerfilOld.RutaSoporte = ContratoPerfil.RutaSoporte;

                            contratoPerfilOld.TieneObservacionApoyo = ContratoPerfil.TieneObservacionApoyo;
                            contratoPerfilOld.RegistroCompleto = ValidarRegistroCompletoContratoPerfil(contratoPerfilOld);

                            if (contratoPerfilOld.RegistroCompleto)
                            {
                                if (contratoPerfilOld.TieneObservacionSupervisor.HasValue)
                                {
                                    contratoPerfilOld.TieneObservacionSupervisor = false;
                                }
                            }
                            else
                            {
                                RegistroCompletoContrato = false;
                            }

                            foreach (var ContratoPerfilObservacion in ContratoPerfil.ContratoPerfilObservacion)
                            {
                                if (ContratoPerfilObservacion.ContratoPerfilObservacionId > 0)
                                {
                                    ContratoPerfilObservacion contratoPerfilObservacionOld = _context.ContratoPerfilObservacion.Find(ContratoPerfilObservacion.ContratoPerfilObservacionId);
                                    contratoPerfilObservacionOld.UsuarioModificacion = pContrato.UsuarioCreacion;
                                    contratoPerfilObservacionOld.FechaModificacion = DateTime.Now;
                                    contratoPerfilObservacionOld.Eliminado = false;
                                    contratoPerfilObservacionOld.Observacion = ContratoPerfilObservacion.Observacion.ToUpper();
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
                            ContratoPerfil.Eliminado = false;
                            ContratoPerfil.RegistroCompleto = ValidarRegistroCompletoContratoPerfil(ContratoPerfil);
                            _context.ContratoPerfil.Add(ContratoPerfil);


                            foreach (var ContratoPerfilObservacion in ContratoPerfil.ContratoPerfilObservacion)
                            {

                                if (!string.IsNullOrEmpty(ContratoPerfilObservacion.Observacion))
                                    ContratoPerfilObservacion.Observacion = ContratoPerfilObservacion.Observacion.ToUpper();
                                ContratoPerfilObservacion.UsuarioCreacion = pContrato.UsuarioCreacion;
                                ContratoPerfilObservacion.FechaCreacion = DateTime.Now;
                                ContratoPerfilObservacion.TipoObservacionCodigo = ConstanCodigoTipoObservacion.Interventoria;
                                ContratoPerfilObservacion.Eliminado = false;


                                _context.ContratoPerfilObservacion.Add(ContratoPerfilObservacion);
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
                                    ContratoPerfilNumeroRadicado.UsuarioModificacion = pContrato.UsuarioCreacion;
                                    ContratoPerfilNumeroRadicado.FechaModificacion = DateTime.Now;
                                }
                            }
                        }
                    }
                }

                //Cambiar Estado Contrato 
                Contrato contratoOld = _context.Contrato.
                    Where(r => r.ContratoId == pContrato.ContratoId)
                    .Include(r => r.Contratacion)
                    .Include(r => r.ContratoPerfil)
                    .FirstOrDefault();

                if (contratoOld.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                {
                    contratoOld.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.En_proceso_de_aprobacion_de_requisitos_tecnicos;

                    //if (contratoOld.ContratoPerfil.Count() > 0 && RegistroCompletoContrato)
                    //{
                    //    contratoOld.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_verificados;
                    //}
                }

                else
                {
                    contratoOld.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.En_proceso_de_verificacion_de_requisitos_tecnicos;

                    if (contratoOld.ContratoPerfil.Count() > 0 && RegistroCompletoContrato)
                    {
                        contratoOld.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_verificados;
                    }
                }
                contratoOld.UsuarioModificacion = pContrato.UsuarioCreacion;
                contratoOld.FechaModificacion = DateTime.Now;

                _context.Update(contratoOld);
                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = RegisterPreContructionPhase1.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.OperacionExitosa, idAccion, pContrato.UsuarioCreacion, CreateEdit)
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

        public async Task<Respuesta> DeleteContratoPerfil(int ContratoPerfilId, string UsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Contrato_Perfil, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ContratoPerfil contratoPerfilOld = _context.ContratoPerfil.Find(ContratoPerfilId);
                contratoPerfilOld.Eliminado = true;
                _context.SaveChanges();

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

                //|| string.IsNullOrEmpty(contratoPerfilOld.ConObervacionesSupervision.ToString() 
                )
            {
                return false;
            }
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
                    await EnviarCorreo(contratoMod, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);
                    contratoMod.FechaAprobacionRequisitosInterventor = DateTime.Now;
                }
                //Enviar Correo Botón “Enviar al supervisor”
                if (pEstadoVerificacionContratoCodigo == ConstanCodigoEstadoContrato.Enviado_al_supervisor && contratoMod.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                {
                    await EnviarCorreoSupervisor(ConstanCodigoTipoContratacionSTRING.Interventoria, contratoMod, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);
                    contratoMod.FechaAprobacionRequisitosApoyo = DateTime.Now;
                }

                //Enviar Correo Botón aprobar inicio 3.1.7
                if (pEstadoVerificacionContratoCodigo == ConstanCodigoEstadoContrato.Enviado_al_supervisor && contratoMod.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                {
                    await EnviarCorreoSupervisor(ConstanCodigoTipoContratacionSTRING.Obra, contratoMod, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);
                    contratoMod.FechaAprobacionRequisitosApoyo = DateTime.Now;
                }


                //Enviar Correo Botón aprobar inicio 3.1.8
                if (pEstadoVerificacionContratoCodigo == ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_aprobados_por_supervisor)
                    contratoMod.FechaAprobacionRequisitosSupervisor = DateTime.Now;


                //Enviar Correo Botón aprobar inicio 3.1.8
                if (pEstadoVerificacionContratoCodigo == ConstanCodigoEstadoContrato.Enviado_al_interventor) {

                    await EnviarCorreoSupervisor(ConstanCodigoTipoContratacionSTRING.Obra, contratoMod, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);
                    foreach (var ContratoPerfil in contratoMod.ContratoPerfil)
                    {
                        ContratoPerfil contratoPerfilOld = _context.ContratoPerfil.Find(ContratoPerfil.ContratoPerfilId);

                        contratoPerfilOld.TieneObservacionSupervisor = null;
                        contratoPerfilOld.TieneObservacionApoyo = null;
                        contratoPerfilOld.FechaModificacion = DateTime.Now;
                        contratoPerfilOld.UsuarioModificacion = UsuarioModificacion;
                    }
                }
                   
                if (pEstadoVerificacionContratoCodigo == ConstanCodigoEstadoContrato.Enviado_al_apoyo)
                {
                    await EnviarCorreoSupervisor(ConstanCodigoTipoContratacionSTRING.Obra, contratoMod, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);

                    foreach (var ContratoPerfil in contratoMod.ContratoPerfil)
                    {
                        ContratoPerfil contratoPerfilOld = _context.ContratoPerfil.Find(ContratoPerfil.ContratoPerfilId);

                        contratoPerfilOld.TieneObservacionSupervisor = null;
                        contratoPerfilOld.TieneObservacionApoyo = null;
                        contratoPerfilOld.FechaModificacion = DateTime.Now;
                        contratoPerfilOld.UsuarioModificacion = UsuarioModificacion; 
                    } 
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

        private async Task<bool> EnviarCorreo(Contrato contratoMod, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario);

            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.AprobarInicio316);

            string template = TemplateRecoveryPassword.Contenido
                .Replace("_LinkF_", pDominioFront)
                .Replace("[NUMERO_CONTRATO]", contratoMod.NumeroContrato)
                .Replace("[FECHA_POLIZA]", ((DateTime)contratoMod.ContratoPoliza.FirstOrDefault().FechaAprobacion).ToString("dd-MMMM-yy"))
                .Replace("[CANTIDAD_PROYECTOS]", contratoMod.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());

            bool blEnvioCorreo = false;

            foreach (var item in usuarios)
            {
                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Requisitos de inicio para verificación", template, pSender, pPassword, pMailServer, pMailPort);
            }
            return blEnvioCorreo;
        }

        private async Task<bool> EnviarCorreoSupervisor(string pTipoContrato, Contrato contratoMod, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Supervisor).Include(y => y.Usuario);

            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.EnviarSupervisor317);

            string template = TemplateRecoveryPassword.Contenido
                .Replace("_LinkF_", pDominioFront)
                .Replace("[TIPO_CONTRATO]", pTipoContrato)
                .Replace("[NUMERO_CONTRATO]", contratoMod.NumeroContrato)
                .Replace("[FECHA_VERIFICACION]", ((DateTime.Now)).ToString("dd-MMMM-yy"))
                .Replace("[CANTIDAD_PROYECTOS]", contratoMod.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());

            bool blEnvioCorreo = false;

            foreach (var item in usuarios)
            {
                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Requisitos de inicio para verificación", template, pSender, pPassword, pMailServer, pMailPort);
            }
            return blEnvioCorreo;
        }

        /// <summary>
        /// Notificar Cuando pasen 4 dias despues de la aprobacion de la poliza 
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

            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Interventor || x.PerfilId == (int)EnumeratorPerfil.Supervisor || x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario);
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

                        foreach (var item in usuarios)
                        {
                            Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Verificación y Aprobación de requisitos pendientes", template, pSender, pPassword, pMailServer, pMailPort);
                        }
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

            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Interventor || x.PerfilId == (int)EnumeratorPerfil.Supervisor || x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario);
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

                    foreach (var item in usuarios)
                    {
                        Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Verificación y Aprobación de requisitos pendientes", template, pSender, pPassword, pMailServer, pMailPort);
                    }

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

            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Interventor || x.PerfilId == (int)EnumeratorPerfil.Supervisor || x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario);
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

                        foreach (var item in usuarios)
                        {
                            Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Verificación y Aprobación de requisitos pendiente", template, pSender, pPassword, pMailServer, pMailPort);
                        }
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

            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Supervisor || x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario);

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
                                    .Replace("[FECHA_REQUISITOS]", fechaValidacion.ToString("dd-MMMM-yy"))
                                    .Replace("[CANTIDAD_PROYECTOS]", contrato.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());

                        foreach (var item in usuarios)
                        {
                            Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Verificación y Aprobación de requisitos pendientes", template, pSender, pPassword, pMailServer, pMailPort);
                        }
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

            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Supervisor || x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario);

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
                                 .Replace("[FECHA_REQUISITOS]", fechaValidacion.ToString("dd-MMMM-yy"))
                                 .Replace("[CANTIDAD_PROYECTOS]", contrato.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());
                        foreach (var item in usuarios)
                        {
                            Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Verificación y Aprobación de requisitos pendientes", template, pSender, pPassword, pMailServer, pMailPort);
                        }
                    }
                }
            }
        }
    }
}