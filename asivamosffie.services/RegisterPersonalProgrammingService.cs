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

namespace asivamosffie.services
{
    public class RegisterPersonalProgrammingService : IRegisterPersonalProgrammingService
    {
        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;

        public RegisterPersonalProgrammingService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<List<VRegistrarPersonalObra>> GetListProyectos()
        {
            return await _context.VRegistrarPersonalObra.ToListAsync();
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

            // contratacionProyecto.Contratacion.Contrato.FirstOrDefault().fecha

            if (contratacionProyecto.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
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

                    for (int i = 1; i < CalcularSemanasPlazoProyecto(pContratacionProyectoId) + 1; i++)
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

                        SeguimientoSemanalPersonalObra seguimientoSemanalPersonalObra = new SeguimientoSemanalPersonalObra
                        {
                            SeguimientoSemanalId = seguimientoSemanal.SeguimientoSemanalId,
                            Eliminado = false,
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = pUsuario
                        };

                        _context.SeguimientoSemanalPersonalObra.Add(seguimientoSemanalPersonalObra);
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
            bool RegistroCompleto = true;

            try
            {
                ContratacionProyecto ContratacionProyecto = _context.ContratacionProyecto.Where(r => r.ContratacionProyectoId == pListSeguimientoSemanal.FirstOrDefault().ContratacionProyectoId).FirstOrDefault();
                Proyecto proyecto = _context.Proyecto.Where(r => r.ProyectoId == ContratacionProyecto.ProyectoId).FirstOrDefault();

                proyecto.UsuarioModificacion = pListSeguimientoSemanal.FirstOrDefault().UsuarioCreacion;
                proyecto.FechaModificacion = DateTime.Now;

                foreach (var SeguimientoSemanal in pListSeguimientoSemanal)
                {
                    if (SeguimientoSemanal.SeguimientoSemanalPersonalObra.FirstOrDefault().CantidadPersonal == null)
                    {

                        RegistroCompleto = false;
                    }

                    if (SeguimientoSemanal.SeguimientoSemanalPersonalObra.FirstOrDefault().SeguimientoSemanalPersonalObraId == 0)
                    {
                        SeguimientoSemanal.SeguimientoSemanalPersonalObra.FirstOrDefault().UsuarioCreacion = proyecto.UsuarioModificacion;
                        SeguimientoSemanal.SeguimientoSemanalPersonalObra.FirstOrDefault().FechaCreacion = DateTime.Now;
                        SeguimientoSemanal.SeguimientoSemanalPersonalObra.FirstOrDefault().Eliminado = true;

                        _context.SeguimientoSemanalPersonalObra.Add(SeguimientoSemanal.SeguimientoSemanalPersonalObra.FirstOrDefault());
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

                Proyecto proyecto = _context.Proyecto.Find(contratacionProyecto.ProyectoId);

                proyecto.UsuarioModificacion = pUsuario;
                proyecto.FechaModificacion = DateTime.Now;
                proyecto.EstadoProgramacionCodigo = pEstadoProgramacionCodigo;

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
    }
}
