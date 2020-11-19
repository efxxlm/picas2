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
                .Include(r => r.Contratacion).FirstOrDefault();

            if (contratacionProyecto.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
            {

                CantidadDias = contratacionProyecto.Proyecto.PlazoMesesObra ?? 0;
                CantidadDias *= 30;
                CantidadDias += contratacionProyecto.Proyecto.PlazoDiasObra.HasValue ? (int)contratacionProyecto.Proyecto.PlazoDiasObra : 0;

                CantidadSemanas = CantidadDias / 7;

                if (CantidadDias % 7 == 1)
                    CantidadSemanas = (CantidadDias / 7) + 1;
            }

            return CantidadSemanas;
        }

        public async Task<List<SeguimientoSemanal>> GetProgramacionPersonalByContratoId(int ContratacionProyectoId, string pUsuario)
        {
            try
            {
                List<SeguimientoSemanal> List = _context.SeguimientoSemanal
                    .Where(r => r.ContratacionProyectoId == ContratacionProyectoId)
                    .Include(r => r.SeguimientoSemanalPersonalObra)
                    .ToList();

                if (List.Count() == 0)
                {

                    for (int i = 1; i < CalcularSemanasPlazoProyecto(ContratacionProyectoId) + 1; i++)
                    {
                        SeguimientoSemanal seguimientoSemanal = new SeguimientoSemanal
                        {

                            ContratacionProyectoId = ContratacionProyectoId,
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

        public async Task<Respuesta> UpdateProgramacionContratoPersonal(ContratoConstruccion pContratoConstruccion)
        {

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.RegistrarProgramacionPersonal, (int)EnumeratorTipoDominio.Acciones);
            //bool RegistroCompleto = true;

            try
            {
                //Proyecto proyecto = _context.Proyecto.Find(pContratoConstruccion.ProyectoId);

                //proyecto.UsuarioModificacion = pContratoConstruccion.UsuarioCreacion;
                //proyecto.FechaModificacion = DateTime.Now;

                //foreach (var item in pContratoConstruccion.ProgramacionPersonalContrato)
                //{
                //    ProgramacionPersonalContratoConstruccion programacionPersonalContratoConstruccion = _context.ProgramacionPersonalContratoConstruccion.Find(item.ProgramacionPersonalContratoConstruccionId);
                //    programacionPersonalContratoConstruccion.UsuarioModificacion = pContratoConstruccion.UsuarioCreacion;
                //    programacionPersonalContratoConstruccion.FechaModificacion = DateTime.Now;
                //    programacionPersonalContratoConstruccion.CantidadPersonal = item.CantidadPersonal;

                //    if (programacionPersonalContratoConstruccion.CantidadPersonal == 0)
                //        RegistroCompleto = false;
                //}
                //if (RegistroCompleto)
                //    proyecto.EstadoProgramacionCodigo = ConstanCodigoEstadoProgramacionInicial.Sin_aprobacion_de_programacion_personal;
                //else
                //    proyecto.EstadoProgramacionCodigo = ConstanCodigoEstadoProgramacionInicial.En_registro_programacion;

                //await _context.SaveChangesAsync();

                return
                     new Respuesta
                     {
                         IsSuccessful = true,
                         IsException = false,
                         IsValidation = false,
                         Code = ConstantSesionComiteTecnico.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Programacion_Personal_Obra, ConstantMessagesRegistrarProgramacionPersonal.OperacionExitosa, idAccion, pContratoConstruccion.UsuarioCreacion, "REGISTRAR PROGRAMACION DE PERSONAL")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Programacion_Personal_Obra, ConstantMessagesRegistrarProgramacionPersonal.Error, idAccion, pContratoConstruccion.UsuarioCreacion, ex.InnerException.ToString())
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
