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
    public class ApprovePreConstructionPhase1Service : IApprovePreConstructionPhase1Service
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IVerifyPreConstructionRequirementsPhase1Service _verifyPre;

        public ApprovePreConstructionPhase1Service(devAsiVamosFFIEContext devAsiVamosFFIEContext, IVerifyPreConstructionRequirementsPhase1Service verifyPreConstructionRequirementsPhase1Service, ICommonService commonService)
        {
            _context = devAsiVamosFFIEContext;
            _commonService = commonService;
            _verifyPre = verifyPreConstructionRequirementsPhase1Service;
        }

        public async Task<dynamic> GetListContratacion()
        {
            List<Dominio> Parametricas = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Verificacion_Contrato).ToList();
            List<dynamic> listaContrats = new List<dynamic>();

            List<Contrato> listContratos = await _context.Contrato
                .FromSqlRaw("SELECT c.* FROM dbo.Contrato AS c " +
                "INNER JOIN dbo.Contratacion AS ctr ON c.ContratacionId = ctr.ContratacionId " +
                "INNER JOIN dbo.DisponibilidadPresupuestal AS dp ON ctr.ContratacionId = dp.ContratacionId " +
                "INNER JOIN dbo.ContratoPoliza AS cp ON c.ContratoId = cp.ContratoId " +
                "WHERE dp.NumeroDRP IS NOT NULL " + //Documento Registro Presupuestal
                "AND cp.FechaAprobacion IS NOT NULL " +  //Fecha Aprobacion Poliza
                "AND  c.EstadoVerificacionCodigo = 6 " +  //Enviado al supervisor
                "OR  c.EstadoVerificacionCodigo = 7 " +  //En proceso de validación de requisitos técnicos
                "OR  c.EstadoVerificacionCodigo = 8 " +  //Con requisitos técnicos validados
                "OR  c.EstadoVerificacionCodigo = 9 " +  //Con requisitos técnicos aprobados por supervisor
                "OR  c.EstadoVerificacionCodigo = 10 " + //Enviado al interventor
                "OR  c.EstadoVerificacionCodigo = 11 ") //Enviado al apoyo

                .Include(r => r.ContratoPoliza)
                .Include(r => r.Contratacion)
                   .ThenInclude(r => r.ContratacionProyecto)
                       .ThenInclude(r => r.Proyecto)
                           .ThenInclude(r => r.ContratoPerfil)
                               .ThenInclude(r => r.ContratoPerfilObservacion)
                .ToListAsync();

            foreach (var c in listContratos)
            {
                if (c.ContratoPoliza.FirstOrDefault().FechaAprobacion.HasValue)
                {
                    bool TieneObservacionSupervisor = false;
                    int CantidadProyectosConPerfilesAprobados = 0;
                    int CantidadProyectosConPerfilesPendientes = 0;
                    bool RegistroCompleto = false;
                    bool EstaDevuelto = false;

                    if (c.EstaDevuelto.HasValue && (bool)c.EstaDevuelto)
                        EstaDevuelto = true;
                    foreach (var ContratacionProyecto in c.Contratacion.ContratacionProyecto.Where(r => !(bool)r.Eliminado))
                    { 
                        bool RegistroCompletoObservaciones = false;

                        foreach (var ContratoPerfil in c.ContratoPerfil.Where(r => !(bool)r.Eliminado && r.ProyectoId == ContratacionProyecto.ProyectoId))
                        {
                            if (ContratoPerfil.TieneObservacionSupervisor.HasValue && !(bool)ContratoPerfil.TieneObservacionSupervisor ||
                                    (ContratoPerfil.TieneObservacionSupervisor.HasValue && (bool)ContratoPerfil.TieneObservacionSupervisor && !string.IsNullOrEmpty(ContratoPerfil.ContratoPerfilObservacion.LastOrDefault().Observacion) && ContratoPerfil.ContratoPerfilObservacion.LastOrDefault().TipoObservacionCodigo == ConstanCodigoTipoObservacion.Supervisor))
                                RegistroCompletoObservaciones = false;
                             
                            if (ContratoPerfil.ContratoPerfilObservacion.Count(r => r.TipoObservacionCodigo == ConstanCodigoTipoObservacion.Supervisor) == 0)
                                RegistroCompleto = false;
                            else if ((ContratoPerfil.TieneObservacionSupervisor == null)
                                 || (ContratoPerfil.TieneObservacionSupervisor.HasValue
                                 && (bool)ContratoPerfil.TieneObservacionSupervisor
                                 && (ContratoPerfil.ContratoPerfilObservacion.LastOrDefault().Observacion == null
                                 && ContratoPerfil.ContratoPerfilObservacion.LastOrDefault().TipoObservacionCodigo == ConstanCodigoTipoObservacion.Supervisor)))
                                RegistroCompleto = false;
                            if (ContratoPerfil.TieneObservacionSupervisor.HasValue && (bool)ContratoPerfil.TieneObservacionSupervisor)
                            {
                                TieneObservacionSupervisor = true;
                            }

                           
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
                        TieneObservacionSupervisor,
                        EstadoNombre = Parametricas.Where(r => r.Codigo == c.EstadoVerificacionCodigo).FirstOrDefault().Nombre
                    });
                }
            }

            return listaContrats;

        }

        public async Task<Respuesta> CrearContratoPerfilObservacion(ContratoPerfilObservacion pContratoPerfilObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Observacion_Contrato_Perfil, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ContratoPerfil contratoPerfilOld = _context.ContratoPerfil.Find(pContratoPerfilObservacion.ContratoPerfilId);
                contratoPerfilOld.UsuarioModificacion = pContratoPerfilObservacion.UsuarioCreacion;
                contratoPerfilOld.FechaModificacion = DateTime.Now;
                contratoPerfilOld.TieneObservacionSupervisor = pContratoPerfilObservacion.TieneObservacionSupervisor;
                contratoPerfilOld.RegistroCompleto = false;

                if (pContratoPerfilObservacion.ContratoPerfilObservacionId == 0)
                {
                    pContratoPerfilObservacion.FechaCreacion = DateTime.Now;
                    pContratoPerfilObservacion.Eliminado = false;
                    pContratoPerfilObservacion.TipoObservacionCodigo = ConstanCodigoTipoObservacion.Supervisor;
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
                _context.SaveChanges();

                //Validar Estados Completos
                Contrato contrato = _context.Contrato
                    .Where(r => r.ContratoId == contratoPerfilOld.ContratoId)
                    .Include(r => r.Contratacion)
                    .Include(r => r.ContratoPerfil)
                    .ThenInclude(r => r.ContratoPerfilObservacion).FirstOrDefault();

                bool RegistroCompleto = true;
                bool TieneObservacionSupervisor = true;
                foreach (var ContratoPerfil in contrato.ContratoPerfil.Where(r => !(bool)r.Eliminado))
                {
                    if (ContratoPerfil.TieneObservacionSupervisor.HasValue && (bool)ContratoPerfil.TieneObservacionSupervisor)
                    {
                        TieneObservacionSupervisor = false;
                    }

                    if (ContratoPerfil.ContratoPerfilObservacion.Count(r => r.TipoObservacionCodigo == ConstanCodigoTipoObservacion.Supervisor) == 0)
                        RegistroCompleto = false;
                    else if ((ContratoPerfil.TieneObservacionSupervisor == null)
                         || (ContratoPerfil.TieneObservacionSupervisor.HasValue
                         && (bool)ContratoPerfil.TieneObservacionSupervisor
                         && (ContratoPerfil.ContratoPerfilObservacion.LastOrDefault().Observacion == null
                         && ContratoPerfil.ContratoPerfilObservacion.LastOrDefault().TipoObservacionCodigo == ConstanCodigoTipoObservacion.Supervisor)))
                        RegistroCompleto = false;
                }

                if (RegistroCompleto)
                {
                    if (contrato.Contratacion.TipoContratacionCodigo == ConstanCodigoTipoContrato.Obra)
                    {
                        contrato.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_validados;

                        //if (!(bool)TieneObservacionSupervisor)
                        //{
                        //    contrato.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_validados;
                        //}
                    }
                    else
                    {
                        contrato.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_validados;

                        //if (!(bool)TieneObservacionSupervisor)
                        //{
                        //    contrato.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_validados;
                        //}
                    }
                }
                else
                {
                    contrato.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.En_proceso_de_validacion_de_requisitos_tecnicos;
                }

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
