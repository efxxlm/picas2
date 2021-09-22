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
using iTextSharp.text.pdf.codec;
using Microsoft.Extensions.Options;
using asivamosffie.model.AditionalModels;

namespace asivamosffie.services
{
    public class RegisterPersonalProgrammingService : IRegisterPersonalProgrammingService
    {
        public AppSettings _mailSettings { get; }

        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;
        private readonly IContractualControversy _contractualControversy;

        public RegisterPersonalProgrammingService(devAsiVamosFFIEContext context, ICommonService commonService, IOptions<AppSettings> mailSettings, IContractualControversy contractualControversy)
        {
            _mailSettings = mailSettings.Value;
            _context = context;
            _commonService = commonService;
            _contractualControversy = contractualControversy;
        }

        public async Task<List<VRegistrarPersonalObra>> GetListProyectos()
        {
            List<VRegistrarPersonalObra> vRegistrarPersonalObra = await _context.VRegistrarPersonalObra.ToListAsync();
            vRegistrarPersonalObra.ForEach(r => {
                //Nueva restricción control de cambios
                r.CumpleCondicionesTai = _contractualControversy.ValidarCumpleTaiContratista(r.ContratoId,false);
            });
            return vRegistrarPersonalObra;
        }

        private int CalcularSemanasPlazoProyecto(int ContratacionProyectoId)
        {
            int CantidadDias = 0;
            int CantidadSemanas = 0;

            ContratacionProyecto contratacionProyecto = _context.ContratacionProyecto
                .Where(r => r.ContratacionProyectoId == ContratacionProyectoId)
                .Include(r => r.Proyecto)
                .Include(r => r.Contratacion)
                .ThenInclude(r => r.Contrato)
                .FirstOrDefault();

            if (contratacionProyecto.Contratacion.TipoSolicitudCodigo == ConstantCodigoEstadoProyecto.Disponible)
            {
                CantidadDias = contratacionProyecto.Proyecto.PlazoMesesObra ?? 0;
                CantidadDias *= 30;
                CantidadDias += contratacionProyecto.Proyecto.PlazoDiasObra.HasValue ? (int)contratacionProyecto.Proyecto.PlazoDiasObra : 0;

                CantidadSemanas = CantidadDias / 7;

                if (CantidadDias % 7 == 1)
                    CantidadSemanas = (CantidadDias / 7) + 1;
            }
            else
            {

                CantidadDias = contratacionProyecto.Proyecto.PlazoMesesInterventoria ?? 0;
                CantidadDias *= 30;
                CantidadDias += contratacionProyecto.Proyecto.PlazoDiasInterventoria.HasValue ? (int)contratacionProyecto.Proyecto.PlazoDiasInterventoria : 0;

                CantidadSemanas = CantidadDias / 7;

                if (CantidadDias % 7 == 1)
                    CantidadSemanas = (CantidadDias / 7) + 1;
            }
            return CantidadSemanas;
        }

        public async Task<List<SeguimientoSemanal>> GetProgramacionPersonalByContratoId(int pContratacionProyectoId, string pUsuario)
        {
            try
            {
                List<SeguimientoSemanal> List = await _context.SeguimientoSemanal
                    .Where(r => r.ContratacionProyectoId == pContratacionProyectoId)
                    .Include(r => r.SeguimientoSemanalPersonalObra)
                    .ToListAsync();

                if (List.Count() == 0)
                {
                    int CantidadSemanas = CalcularSemanasPlazoProyecto(pContratacionProyectoId);
                    for (int i = 1; i < CantidadSemanas + 1; i++)
                    {
                        SeguimientoSemanal seguimientoSemanal = new SeguimientoSemanal
                        {

                            ContratacionProyectoId = pContratacionProyectoId,
                            NumeroSemana = i,

                            Eliminado = false,
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = pUsuario
                        };
                        List.Add(seguimientoSemanal);
                        _context.SeguimientoSemanal.Add(seguimientoSemanal);
                        _context.SaveChanges();
                    }
                }
                return List;
            }
            catch (Exception ex)
            {
                return new List<SeguimientoSemanal>();
            }
        }

        public async Task<Respuesta> UpdateSeguimientoSemanalPersonalObra(List<SeguimientoSemanal> pListSeguimientoSemanal)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.RegistrarProgramacionPersonal, (int)EnumeratorTipoDominio.Acciones);
          
            try
            {
                bool RegistroCompleto = !pListSeguimientoSemanal.Any(s => s.SeguimientoSemanalPersonalObra
                                                                .Any(c => c.CantidadPersonal == null));

                ContratacionProyecto ContratacionProyecto = _context.ContratacionProyecto.Where(r => r.ContratacionProyectoId == pListSeguimientoSemanal.FirstOrDefault().ContratacionProyectoId).FirstOrDefault();

                Proyecto proyecto = _context.Proyecto.Where(r => r.ProyectoId == ContratacionProyecto.ProyectoId).FirstOrDefault();

                proyecto.UsuarioModificacion = pListSeguimientoSemanal.FirstOrDefault().UsuarioModificacion;
                proyecto.FechaModificacion = DateTime.Now;

                foreach (var SeguimientoSemanal in pListSeguimientoSemanal)
                {
                    
                        if (SeguimientoSemanal.SeguimientoSemanalPersonalObra.FirstOrDefault().SeguimientoSemanalPersonalObraId == 0)
                        {
                            SeguimientoSemanalPersonalObra seguimientoSemanalPersonalObra = new SeguimientoSemanalPersonalObra
                            {
                                SeguimientoSemanalId = SeguimientoSemanal.SeguimientoSemanalId,
                                UsuarioCreacion = proyecto.UsuarioModificacion,
                                Eliminado = false,
                                FechaCreacion = DateTime.Now,
                                CantidadPersonal = SeguimientoSemanal.SeguimientoSemanalPersonalObra.FirstOrDefault().CantidadPersonal
                            };
                            _context.SeguimientoSemanalPersonalObra.Add(seguimientoSemanalPersonalObra);
                        }
                        else
                        {
                            SeguimientoSemanalPersonalObra SeguimientoSemanalPersonalObraOld = _context.SeguimientoSemanalPersonalObra.Find(SeguimientoSemanal.SeguimientoSemanalPersonalObra.FirstOrDefault().SeguimientoSemanalPersonalObraId);
                            SeguimientoSemanalPersonalObraOld.CantidadPersonal = SeguimientoSemanal.SeguimientoSemanalPersonalObra.FirstOrDefault().CantidadPersonal;
                            SeguimientoSemanalPersonalObraOld.UsuarioModificacion = proyecto.UsuarioModificacion;
                            SeguimientoSemanalPersonalObraOld.FechaModificacion = DateTime.Now;
                        } 
                }
                if (RegistroCompleto)
                    proyecto.EstadoProgramacionCodigo = ConstanCodigoEstadoProgramacionInicial.Sin_aprobacion_de_programacion_personal;
                else
                    proyecto.EstadoProgramacionCodigo = ConstanCodigoEstadoProgramacionInicial.En_registro_programacion;

                await _context.SaveChangesAsync();

                return
                     new Respuesta
                     {
                         IsSuccessful = true,
                         IsException = false,
                         IsValidation = false,
                         Code = ConstantSesionComiteTecnico.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Programacion_Personal_Obra, ConstantMessagesRegistrarProgramacionPersonal.OperacionExitosa, idAccion, proyecto.UsuarioModificacion, "REGISTRAR PROGRAMACION DE PERSONAL")
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
                        Code = ConstantSesionComiteTecnico.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Programacion_Personal_Obra, ConstantMessagesRegistrarProgramacionPersonal.Error, idAccion, pListSeguimientoSemanal.FirstOrDefault().UsuarioCreacion, ex.InnerException.ToString())
                    };
            }

        }

        public async Task<Respuesta> ChangeStatusProgramacionContratoPersonal(int pContratacionProyecto, string pEstadoProgramacionCodigo, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Programacion_Especial, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ContratacionProyecto contratacionProyecto = _context.ContratacionProyecto.Find(pContratacionProyecto);

                Proyecto proyecto = _context.Proyecto.Where(r => r.ProyectoId == contratacionProyecto.ProyectoId)
                                                        .Include(r => r.Sede)
                                                        .Include(r => r.InstitucionEducativa)
                                                        .Include(r => r.ContratacionProyecto)
                                                           .ThenInclude(r => r.Contratacion)
                                                               .ThenInclude(r => r.Contrato)
                                                        .FirstOrDefault();
                proyecto.UsuarioModificacion = pUsuario;
                proyecto.FechaModificacion = DateTime.Now;
                proyecto.EstadoProgramacionCodigo = pEstadoProgramacionCodigo;

                //enviar correo a supervisor y apoyo si esta aprobada la solicitud

                if (pEstadoProgramacionCodigo == ConstanCodigoEstadoProgramacionInicial.Con_aprobacion_de_programacion_de_personal)
                    AprobarProgramacionCorreo(
                        proyecto, _mailSettings.DominioFront,
                        _mailSettings.MailServer,
                        _mailSettings.MailPort,
                        _mailSettings.EnableSSL,
                        _mailSettings.Password
                        , _mailSettings.Sender);

                await _context.SaveChangesAsync();

                return
                     new Respuesta
                     {
                         IsSuccessful = true,
                         IsException = false,
                         IsValidation = false,
                         Code = ConstantSesionComiteTecnico.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Programacion_Personal_Obra, ConstantMessagesRegistrarProgramacionPersonal.OperacionExitosa, idAccion, pUsuario, "CAMBIAR ESTADO PROGRAMACION PERSONAL")
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
                        Code = ConstantSesionComiteTecnico.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Programacion_Personal_Obra, ConstantMessagesRegistrarProgramacionPersonal.Error, idAccion, pUsuario, ex.InnerException.ToString())
                    };
            }
        }

        public async void AprobarProgramacionCorreo(Proyecto pProyecto, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Supervisor || x.PerfilId == (int)EnumeratorPerfil.Apoyo).Include(y => y.Usuario);
            List<Dominio> ListTipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            Template TemplateRecoveryPassword = _context.Template.Find((int)enumeratorTemplate.Aprobado_Programacion_4_1_10);

            string template = TemplateRecoveryPassword.Contenido
                     .Replace("_LinkF_", pDominioFront)
                     .Replace("[LLAVE_MEN]", pProyecto.LlaveMen)
                     .Replace("[NUMERO_CONTRATO]", pProyecto.ContratacionProyecto.FirstOrDefault().Contratacion.Contrato.FirstOrDefault().NumeroContrato)
                     .Replace("[INSTITUCION_EDUCATIVA]", pProyecto.InstitucionEducativa.Nombre)
                     .Replace("[SEDE]", pProyecto.Sede.Nombre)
                     .Replace("[TIPO_INTERVENCION]", ListTipoIntervencion.Where(r => r.Codigo == pProyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre)
                     .Replace("[FECHA_ACTA_INICIO]", ((DateTime)pProyecto.ContratacionProyecto.FirstOrDefault().Contratacion.Contrato.FirstOrDefault().FechaActaInicioFase2).ToString("dd-MM-yyyy"));

            foreach (var item in usuarios)
            {
                Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Aprobación de programación de obra", template, pSender, pPassword, pMailServer, pMailPort);
            }
        }

        /// <summary>
        /// Rutina 5 dias despues que se tiene acta y no se ha aprobado la programacion de obra
        /// </summary>
        public async Task TareaProgramada(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            var usuarios = _context.UsuarioPerfil
                .Where(x => x.PerfilId == (int)EnumeratorPerfil.Supervisor
                         || x.PerfilId == (int)EnumeratorPerfil.Apoyo
                         || x.PerfilId == (int)EnumeratorPerfil.Interventor
                         ).Include(y => y.Usuario);

            DateTime RangoFechaConDiasHabiles = await _commonService.CalculardiasLaborales(5, DateTime.Now);
            List<InstitucionEducativaSede> ListInstitucionEducativaSedes = _context.InstitucionEducativaSede.ToList();
            List<Dominio> ListTipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            Template TemplateRecoveryPassword = _context.Template.Find((int)enumeratorTemplate.Alerta_Automatica_5_Dias);

            List<Contrato> listContratos = _context.Contrato
                                                    .Where(c => c.FechaActaInicioFase2.HasValue && c.FechaActaInicioFase2 > RangoFechaConDiasHabiles)
                                                    .Include(r => r.Contratacion)
                                                        .ThenInclude(cp => cp.ContratacionProyecto)
                                                            .ThenInclude(t => t.Proyecto)
                                                                .ThenInclude(r => r.InstitucionEducativa)
                                                                                                        .ToList();

            foreach (var pContrato in listContratos)
            {
                foreach (var ContratacionProyecto in pContrato.Contratacion.ContratacionProyecto.Where(r => r.Proyecto.EstadoProgramacionCodigo != ConstanCodigoEstadoProgramacionInicial.Con_aprobacion_de_programacion_de_personal).ToList())
                {
                    string template = TemplateRecoveryPassword.Contenido
                           .Replace("_LinkF_", pDominioFront)
                           .Replace("[LLAVE_MEN]", ContratacionProyecto.Proyecto.LlaveMen)
                           .Replace("[NUMERO_CONTRATO]", ContratacionProyecto.Contratacion.Contrato.FirstOrDefault().NumeroContrato)
                           .Replace("[INSTITUCION_EDUCATIVA]", ContratacionProyecto.Proyecto.InstitucionEducativa.Nombre)
                           .Replace("[SEDE]", ListInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == (int)ContratacionProyecto.Proyecto.SedeId).FirstOrDefault().Nombre)
                           .Replace("[TIPO_INTERVENCION]", ListTipoIntervencion.Where(r => r.Codigo == ContratacionProyecto.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre)
                           .Replace("[FECHA_ACTA_INICIO]", ((DateTime)pContrato.FechaActaInicioFase2).ToString("dd-MM-yyy"));

                    foreach (var item in usuarios)
                    {
                        Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Programación de obra sin aprobación", template, pSender, pPassword, pMailServer, pMailPort);
                    }
                }
            }

        }

    }

}

