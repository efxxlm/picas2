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
                .ThenInclude(r=> r.Contrato)
                .FirstOrDefault();

            // contratacionProyecto.Contratacion.Contrato.FirstOrDefault().fecha

            //if (contratacionProyecto.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
            //{

                CantidadDias = contratacionProyecto.Proyecto.PlazoMesesObra ?? 0;
                CantidadDias *= 30;
                CantidadDias += contratacionProyecto.Proyecto.PlazoDiasObra.HasValue ? (int)contratacionProyecto.Proyecto.PlazoDiasObra : 0;

                CantidadSemanas = CantidadDias / 7;

                if (CantidadDias % 7 == 1)
                    CantidadSemanas = (CantidadDias / 7) + 1;
            //}
             


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
                    }
                }

                return List;
            }
            catch (Exception ex)
            {
                return new List<SeguimientoSemanal>();
            }
        }

        public async Task<Respuesta> UpdateSeguimientoSemanalPersonalObra(SeguimientoSemanal pSeguimientoSemanal)
        {

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.RegistrarProgramacionPersonal, (int)EnumeratorTipoDominio.Acciones);
            bool RegistroCompleto = true;

            try
            {
                ContratacionProyecto ContratacionProyecto = _context.ContratacionProyecto.Where(r => r.ContratacionProyectoId == pSeguimientoSemanal.ContratacionProyectoId).FirstOrDefault();
                Proyecto proyecto = _context.Proyecto.Where(r => r.ProyectoId == ContratacionProyecto.ProyectoId).FirstOrDefault();

                proyecto.UsuarioModificacion = pSeguimientoSemanal.UsuarioCreacion;
                proyecto.FechaModificacion = DateTime.Now;

                foreach (var SeguimientoSemanalPersonalObra in pSeguimientoSemanal.SeguimientoSemanalPersonalObra)
                {

                    if (SeguimientoSemanalPersonalObra.SeguimientoSemanalPersonalObraId == 0)
                    {
                        SeguimientoSemanalPersonalObra.UsuarioCreacion = pSeguimientoSemanal.UsuarioCreacion;
                        SeguimientoSemanalPersonalObra.FechaCreacion = DateTime.Now;
                        SeguimientoSemanalPersonalObra.Eliminado = true;

                        _context.SeguimientoSemanalPersonalObra.Add(SeguimientoSemanalPersonalObra);
                    }
                    else
                    {
                        SeguimientoSemanalPersonalObra SeguimientoSemanalPersonalObraOld = _context.SeguimientoSemanalPersonalObra.Find(SeguimientoSemanalPersonalObra.SeguimientoSemanalPersonalObraId);
                        SeguimientoSemanalPersonalObraOld.CantidadPersonal = SeguimientoSemanalPersonalObra.CantidadPersonal;
                        SeguimientoSemanalPersonalObraOld.UsuarioModificacion = pSeguimientoSemanal.UsuarioCreacion;
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
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Programacion_Personal_Obra, ConstantMessagesRegistrarProgramacionPersonal.OperacionExitosa, idAccion, pSeguimientoSemanal.UsuarioCreacion, "REGISTRAR PROGRAMACION DE PERSONAL")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Programacion_Personal_Obra, ConstantMessagesRegistrarProgramacionPersonal.Error, idAccion, pSeguimientoSemanal.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }

        }

        public async Task<Respuesta> ChangeStatusProgramacionContratoPersonal(int pContratoConstruccionId, string pEstadoProgramacionCodigo, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Programacion_Especial, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion.Find(pContratoConstruccionId);

                //Proyecto proyecto = _context.Proyecto.Find(contratoConstruccion.ProyectoId);
                //proyecto.UsuarioModificacion = pUsuario;
                //proyecto.FechaModificacion = DateTime.Now;
                //proyecto.EstadoProgramacionCodigo = pEstadoProgramacionCodigo;

                //await _context.SaveChangesAsync();

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
