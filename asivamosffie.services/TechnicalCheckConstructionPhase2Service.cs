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
using Newtonsoft.Json;

namespace asivamosffie.services
{
    public class TechnicalCheckConstructionPhase2Service : ITechnicalCheckConstructionPhase2Service
    {
        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;

        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirements;

        public TechnicalCheckConstructionPhase2Service(ITechnicalRequirementsConstructionPhaseService technicalRequirements, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _technicalRequirements = technicalRequirements;
            _context = context;
            _commonService = commonService;
        }

        public async Task<List<VRequisitosTecnicosConstruccionAprobar>> GetContractsGrid(string pUsuarioId, string pTipoContrato)
        {
            if (pTipoContrato == ConstanCodigoTipoContratacion.Obra.ToString()) 
                return await _context.VRequisitosTecnicosConstruccionAprobar.Where(r => r.TipoContratoCodigo == ConstanCodigoTipoContratacion.Obra.ToString() && r.TieneFaseConstruccion > 0 && ( r.EstadoCodigo == "6" || r.EstadoCodigo == "7" || r.EstadoCodigo == "8" || r.EstadoCodigo == "9" || r.EstadoCodigo == "10")).ToListAsync();
            else
                return await _context.VRequisitosTecnicosConstruccionAprobar.Where(r => r.TipoContratoCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString() && r.TieneFaseConstruccion > 0  && ( r.EstadoCodigo == "6" || r.EstadoCodigo == "7" || r.EstadoCodigo == "8" || r.EstadoCodigo == "9" || r.EstadoCodigo == "10" || r.EstadoCodigo == "11")).ToListAsync();
             
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
                if (pEstadoVerificacionContratoCodigo == ConstanCodigoEstadoContrato.Enviado_al_interventor)
                {
                    foreach (var ContratoPerfil in contratoMod.ContratoPerfil)
                    {
                        ContratoPerfil contratoPerfilOld = _context.ContratoPerfil.Find(ContratoPerfil.ContratoPerfilId);
                        contratoPerfilOld.FechaModificacion = DateTime.Now;
                        contratoPerfilOld.RegistroCompleto = false;
                        contratoPerfilOld.UsuarioModificacion = UsuarioModificacion;

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


                ///Logica para devoluciones
                ///
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

    }
}
