using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Constants;
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
    public class VerifyPreConstructionRequirementsPhase1Service : IVerifyPreConstructionRequirementsPhase1Service
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public VerifyPreConstructionRequirementsPhase1Service(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        public async Task<List<VRegistrarFase1>> GetListContratacionInterventoria2()
        {
            return await _context.VRegistrarFase1.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString() && r.TieneFasePreconstruccion.Value > 0).OrderByDescending(r => r.FechaAprobacion).ToListAsync();
        }

        public async Task<dynamic> GetListContratacion()
        {
            List<dynamic> listaContrats = new List<dynamic>();
            List<Dominio> Parametricas = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Verificacion_Contrato).ToList();
            try
            {
                List<Contrato> listContratos = await _context.Contrato
                      .FromSqlRaw("SELECT c.* FROM dbo.Contrato AS c " +
                      "INNER JOIN dbo.Contratacion AS ctr ON c.ContratacionId = ctr.ContratacionId " +
                      "INNER JOIN dbo.DisponibilidadPresupuestal AS dp ON ctr.ContratacionId = dp.ContratacionId " +
                      "INNER JOIN dbo.ContratoPoliza AS cp ON c.ContratoId = cp.ContratoId " +
                      "WHERE dp.NumeroDRP IS NOT NULL " +     //Documento Registro Presupuestal
                      "AND cp.FechaAprobacion is not null " + //Fecha Aprobacion Poliza
                      "AND ctr.TipoSolicitudCodigo = 1")
                      .Include(r => r.ContratoPoliza)
                      .Include(r => r.Contratacion)
                         .ThenInclude(r => r.ContratacionProyecto)
                             .ThenInclude(r => r.Proyecto)
                                  .ThenInclude(r => r.ContratoPerfil)
                                     .ThenInclude(r => r.ContratoPerfilObservacion)
                     .ToListAsync();

                foreach (var c in listContratos.OrderBy(r => r.EstadoVerificacionCodigo))
                {
                    int CantidadProyectosConPerfilesAprobados = 0;
                    int CantidadProyectosConPerfilesPendientes = 0;
                    bool RegistroCompleto = false;
                    bool EstaDevuelto = false;
                    if (c.EstaDevuelto.HasValue && (bool)c.EstaDevuelto)
                        EstaDevuelto = true;
                    foreach (var ContratacionProyecto in c.Contratacion.ContratacionProyecto.Where(r => !(bool)r.Eliminado))
                    {
                        bool RegistroCompletoObservaciones = true;
                        foreach (var ContratoPerfil in c.ContratoPerfil.Where(r => !(bool)r.Eliminado && r.ProyectoId == ContratacionProyecto.ProyectoId))
                        {

                            string UltimaObservacionApoyo = string.Empty;

                            UltimaObservacionApoyo = ContratoPerfil?.ContratoPerfilObservacion.Where(r => r.TipoObservacionCodigo == ConstanCodigoTipoObservacion.ApoyoSupervisor).Count() > 0 ? ContratoPerfil?.ContratoPerfilObservacion?.OrderBy(r => r.ContratoPerfilObservacionId).Where(r => r.TipoObservacionCodigo == ConstanCodigoTipoObservacion.ApoyoSupervisor).LastOrDefault().Observacion : string.Empty;

                            if ((ContratoPerfil.TieneObservacionApoyo.HasValue && (bool)ContratoPerfil.TieneObservacionApoyo && UltimaObservacionApoyo == null))
                                RegistroCompleto = false;

                            if (!ContratoPerfil.TieneObservacionApoyo.HasValue)
                                RegistroCompletoObservaciones = false;

                            if (ContratoPerfil.ContratoPerfilObservacion.Count(r => r.TipoObservacionCodigo == ConstanCodigoTipoObservacion.ApoyoSupervisor) == 0)
                                RegistroCompleto = false;
                            else if ((ContratoPerfil.TieneObservacionApoyo == null)
                                 || (ContratoPerfil.TieneObservacionApoyo.HasValue
                                 && (bool)ContratoPerfil.TieneObservacionApoyo
                                 && (ContratoPerfil.ContratoPerfilObservacion.LastOrDefault().Observacion == null
                                 && ContratoPerfil.ContratoPerfilObservacion.LastOrDefault().TipoObservacionCodigo == ConstanCodigoTipoObservacion.ApoyoSupervisor)))
                                RegistroCompleto = false;
                        }
                        if (RegistroCompletoObservaciones)
                            CantidadProyectosConPerfilesAprobados++;
                        else
                            CantidadProyectosConPerfilesPendientes++;
                    }

                    if (c.Contratacion.ContratacionProyecto.Count(r => !r.Eliminado) == CantidadProyectosConPerfilesAprobados)
                        RegistroCompleto = true;
                    listaContrats.Add(new
                    {
                        c.ContratoId,
                        c.ContratoPoliza.FirstOrDefault().FechaAprobacion,
                        c.Contratacion.TipoSolicitudCodigo,
                        c.NumeroContrato,
                        CantidadProyectosAsociados = c.Contratacion.ContratacionProyecto.Count(r => !r.Eliminado),
                        CantidadProyectosRequisitosAprobados = CantidadProyectosConPerfilesAprobados,
                        CantidadProyectosConPerfilesPendientes,
                        EstadoCodigo = c.EstadoVerificacionCodigo,
                        EstaDevuelto,
                        RegistroCompleto,
                        EstadoNombre = c.EstadoVerificacionCodigo == ConstanCodigoEstadoVerificacionContrato.Con_requisitos_tecnicos_aprobados ? Parametricas.Where(r => r.Codigo == c.EstadoVerificacionCodigo).FirstOrDefault().Descripcion : Parametricas.Where(r => r.Codigo == c.EstadoVerificacionCodigo).FirstOrDefault().Nombre
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return listaContrats.OrderByDescending(r => r.FechaAprobacion).ToList();

        }

        public async Task<dynamic> GetListContratacionInterventoria()
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
                     "AND c.EstadoVerificacionCodigo is not null " +
                     "AND ctr.TipoSolicitudCodigo = 2")  //Enviado al apoyo
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
                            c.ContratoPoliza.FirstOrDefault().FechaAprobacion,
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

            return listaContrats.OrderByDescending(r => r.FechaAprobacion).ToList();
        }

        public async Task<Contrato> GetContratoByContratoId(int pContratoId)
        {
            try
            {
                List<Dominio> ListParametricas = _context.Dominio.ToList();
                List<Localizacion> Listlocalizacion = _context.Localizacion.ToList();
                Contrato contrato = await _context.Contrato.Where(r => r.ContratoId == pContratoId)
                     .Include(r => r.Contratacion)
                       .ThenInclude(r => r.ContratacionProyecto)
                             .ThenInclude(r => r.Proyecto)
                                .ThenInclude(r => r.InstitucionEducativa)
                    .Include(r => r.Contratacion)
                       .ThenInclude(r => r.ContratacionProyecto)
                             .ThenInclude(r => r.Proyecto)
                                 .ThenInclude(r => r.Sede)
                    .Include(r => r.ContratoObservacion)
                    .Include(r => r.ContratoPerfil)
                        .ThenInclude(r => r.ContratoPerfilNumeroRadicado)
                    .Include(r => r.ContratoPerfil)
                         .ThenInclude(r => r.ContratoPerfilObservacion)
                    .Include(r => r.ContratoPoliza)
                    .Include(r => r.Contratacion)
                        .ThenInclude(r => r.ContratacionProyecto)
                             .ThenInclude(r => r.Proyecto)
                    .Include(r => r.Contratacion)
                        .ThenInclude(r => r.Contratista).FirstOrDefaultAsync();

                if (contrato.ContratoPerfil.Count() > 0)
                {
                    contrato.ContratoPerfil = contrato.ContratoPerfil.Where(r => !(bool)r.Eliminado).ToList();
                }

                foreach (var ContratacionProyecto in contrato.Contratacion.ContratacionProyecto)
                {
                    Localizacion Municipio = Listlocalizacion.Where(r => r.LocalizacionId == ContratacionProyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    ContratacionProyecto.Proyecto.Departamento = Listlocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault().Descripcion;
                    ContratacionProyecto.Proyecto.Municipio = Municipio.Descripcion;
                    ContratacionProyecto.Proyecto.TipoIntervencionCodigo = ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && r.Codigo == ContratacionProyecto.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;

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
                return contrato;
            }
            catch (Exception ex)
            {
                return new Contrato();
            }
        }

        public async Task<Respuesta> CreateEditContratoPerfil(Contrato pContrato)
        {

            string CreateEdit = "";
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Contrato_Perfil, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //Create Edit
                foreach (var ContratoPerfil in pContrato.ContratoPerfil)
                {
                    if (ContratoPerfil.ContratoPerfilId > 0)
                    {
                        CreateEdit = "CREAR CONTRATO PERFIL";
                        ContratoPerfil contratoPerfilOld = _context.ContratoPerfil.Find(ContratoPerfil.ContratoPerfilId);
                        contratoPerfilOld.ContratoPerfilId = ContratoPerfil.ContratoPerfilId;
                        contratoPerfilOld.PerfilCodigo = ContratoPerfil.PerfilCodigo;
                        contratoPerfilOld.CantidadHvRequeridas = ContratoPerfil.CantidadHvRequeridas;
                        contratoPerfilOld.CantidadHvRecibidas = ContratoPerfil.CantidadHvRecibidas;
                        contratoPerfilOld.CantidadHvAprobadas = ContratoPerfil.CantidadHvAprobadas;
                        contratoPerfilOld.FechaAprobacion = ContratoPerfil.FechaAprobacion;
                        contratoPerfilOld.RutaSoporte = ContratoPerfil.RutaSoporte;
                        contratoPerfilOld.RegistroCompleto = ValidarRegistroCompletoContratoPerfil(contratoPerfilOld);


                        foreach (var ContratoPerfilObservacion in ContratoPerfil.ContratoPerfilObservacion)
                        {
                            if (ContratoPerfilObservacion.ContratoPerfilObservacionId > 0)
                            {
                                ContratoPerfilObservacion contratoPerfilObservacionOld = _context.ContratoPerfilObservacion.Find(ContratoPerfilObservacion.ContratoPerfilObservacionId);
                                contratoPerfilObservacionOld.UsuarioModificacion = pContrato.UsuarioCreacion;
                                contratoPerfilObservacionOld.FechaModificacion = DateTime.Now;
                                contratoPerfilObservacionOld.Eliminado = false;
                                if (!string.IsNullOrEmpty(ContratoPerfilObservacion.Observacion))
                                    contratoPerfilObservacionOld.Observacion = ContratoPerfilObservacion.Observacion;
                            }
                            else
                            {
                                ContratoPerfilObservacion.UsuarioCreacion = pContrato.UsuarioCreacion;
                                ContratoPerfilObservacion.FechaCreacion = DateTime.Now;
                                ContratoPerfilObservacion.TipoObservacionCodigo = ConstanCodigoTipoObservacion.Interventoria;

                                _context.ContratoPerfilObservacion.Add(ContratoPerfilObservacion);
                            }
                        }
                    }
                    else
                    {
                        CreateEdit = "EDITAR CONTRATO PERFIL";
                        ContratoPerfil.UsuarioCreacion = pContrato.UsuarioCreacion;
                        ContratoPerfil.FechaCreacion = DateTime.Now;
                        ContratoPerfil.Eliminado = false;
                        ContratoPerfil.RegistroCompleto = ValidarRegistroCompletoContratoPerfil(ContratoPerfil);
                        _context.ContratoPerfil.Add(ContratoPerfil);


                        foreach (var ContratoPerfilObservacion in ContratoPerfil.ContratoPerfilObservacion)
                        {
                            ContratoPerfilObservacion.UsuarioCreacion = pContrato.UsuarioCreacion;
                            ContratoPerfilObservacion.FechaCreacion = DateTime.Now;
                            ContratoPerfilObservacion.TipoObservacionCodigo = ConstanCodigoTipoObservacion.Interventoria;

                            _context.ContratoPerfilObservacion.Add(ContratoPerfilObservacion);
                        }
                    }
                }
                //Cambiar Estado Requisitos 
                if (pContrato.ContratoPerfil.Where(r => (bool)r.RegistroCompleto).Count() == pContrato.ContratoPerfil.Count() && pContrato.ContratoPerfil.Count() > 0)
                {
                    Contrato contratoOld = _context.Contrato.Find(pContrato.ContratoId);
                    contratoOld.EstadoVerificacionCodigo = ConstanCodigoEstadoVerificacionContratoObra.Con_requisitos_del_contratista_de_obra_avalados;
                    contratoOld.UsuarioModificacion = pContrato.UsuarioCreacion;
                    contratoOld.FechaModificacion = DateTime.Now;
                }
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

        public bool ValidarRegistroCompletoContratoPerfil(ContratoPerfil contratoPerfilOld)
        {
            if (
                    string.IsNullOrEmpty(contratoPerfilOld.PerfilCodigo)
                 || string.IsNullOrEmpty(contratoPerfilOld.CantidadHvRequeridas.ToString())
                 || string.IsNullOrEmpty(contratoPerfilOld.CantidadHvRecibidas.ToString())
                 || string.IsNullOrEmpty(contratoPerfilOld.CantidadHvAprobadas.ToString())
                 || string.IsNullOrEmpty(contratoPerfilOld.FechaAprobacion.ToString())

                 || string.IsNullOrEmpty(contratoPerfilOld.RutaSoporte)

                //|| string.IsNullOrEmpty(contratoPerfilOld.ConObervacionesSupervision.ToString() 
                )
            {
                return false;
            }
            return true;
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

        public async Task<Respuesta> CrearContratoPerfilObservacion(ContratoPerfilObservacion pContratoPerfilObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Observacion_Contrato_Perfil, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ContratoPerfil contratoPerfilOld = _context.ContratoPerfil.Find(pContratoPerfilObservacion.ContratoPerfilId);
                contratoPerfilOld.UsuarioModificacion = pContratoPerfilObservacion.UsuarioCreacion;
                contratoPerfilOld.FechaModificacion = DateTime.Now;
                contratoPerfilOld.TieneObservacionApoyo = pContratoPerfilObservacion.TieneObservacionApoyo;

                if (pContratoPerfilObservacion.ContratoPerfilObservacionId == 0)
                {
                    pContratoPerfilObservacion.FechaCreacion = DateTime.Now;
                    pContratoPerfilObservacion.Eliminado = false;
                    pContratoPerfilObservacion.TipoObservacionCodigo = ConstanCodigoTipoObservacion.ApoyoSupervisor;
                    if (pContratoPerfilObservacion.Observacion != null)
                        pContratoPerfilObservacion.Observacion = pContratoPerfilObservacion.Observacion.ToUpper();
                    _context.ContratoPerfilObservacion.Add(pContratoPerfilObservacion);
                }
                else
                {
                    ContratoPerfilObservacion contratoPerfilObservacionOld = _context.ContratoPerfilObservacion.Find(pContratoPerfilObservacion.ContratoPerfilObservacionId);
                    contratoPerfilObservacionOld.FechaModificacion = DateTime.Now;
                    contratoPerfilObservacionOld.UsuarioModificacion = pContratoPerfilObservacion.UsuarioCreacion;
                    if (pContratoPerfilObservacion.Observacion != null)
                        contratoPerfilObservacionOld.Observacion = pContratoPerfilObservacion.Observacion.ToUpper();
                }
                _context.Update(contratoPerfilOld);
                _context.SaveChanges();

                //Validar Estados Completos
                Contrato contrato = _context.Contrato
                    .Where(r => r.ContratoId == contratoPerfilOld.ContratoId)
                    .Include(r => r.Contratacion)
                    .Include(r => r.ContratoPerfil)
                    .ThenInclude(r => r.ContratoPerfilObservacion).FirstOrDefault();
                contrato.EstaDevuelto = false;
                bool RegistroCompleto = true;

                foreach (var ContratoPerfil in contrato.ContratoPerfil.Where(r => !(bool)r.Eliminado))
                {
                    string UltimaObservacionApoyo = string.Empty;

                    UltimaObservacionApoyo = ContratoPerfil?.ContratoPerfilObservacion
                        .Where(r => r.TipoObservacionCodigo == ConstanCodigoTipoObservacion.ApoyoSupervisor).Count() > 0 ? ContratoPerfil?.ContratoPerfilObservacion?.OrderBy(r => r.ContratoPerfilObservacionId).Where(r => r.TipoObservacionCodigo == ConstanCodigoTipoObservacion.ApoyoSupervisor).LastOrDefault().Observacion : string.Empty;

                    if ((ContratoPerfil.TieneObservacionApoyo.HasValue && (bool)ContratoPerfil.TieneObservacionApoyo && UltimaObservacionApoyo == null))
                        RegistroCompleto = false;
                    if (!ContratoPerfil.TieneObservacionApoyo.HasValue)
                        RegistroCompleto = false;
                }

                if (RegistroCompleto)
                {
                    contrato.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_verificados;
                    // contrato.EstaDevuelto = false;
                }
                else
                    contrato.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.En_proceso_de_verificacion_de_requisitos_tecnicos;

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = RegisterPreContructionPhase1.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.OperacionExitosa, idAccion, pContratoPerfilObservacion.UsuarioCreacion, "CREAR OBSERVACION CONTRATO PERFIL")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.Error, idAccion, pContratoPerfilObservacion.UsuarioCreacion, ex.InnerException.ToString().ToUpper())
                    };
            }
        }


    }
}
