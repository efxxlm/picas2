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

        public async Task<List<ProgramacionPersonalContrato>> GetProgramacionPersonalByContratoId(int pContrato, string pUsuario)
        {

            try
            {

                List<ProgramacionPersonalContrato> List = _context.ProgramacionPersonalContrato.Where(r => r.ContratoId == pContrato).ToList();

                ////Crear Los registros De programacion Si no existen
                //if (List.Count() == 0)
                //{
                //    for (int i = 1; i < contratoConstruccion.Programacion.FirstOrDefault().Duracion + 1; i++)
                //    {
                //        ProgramacionPersonalContratoConstruccion programacionPersonalContratoConstruccion = new ProgramacionPersonalContratoConstruccion
                //        {
                //            UsuarioCreacion = pUsuario,
                //            FechaCreacion = DateTime.Now,
                //            Eliminado = true,

                //            ContratoConstruccionId = contratoConstruccion.ContratoConstruccionId,
                //            NumeroSemana = i,
                //        };
                //        _context.ProgramacionPersonalContratoConstruccion.Add(programacionPersonalContratoConstruccion);
                //        List.Add(programacionPersonalContratoConstruccion);
                //    }
                _context.SaveChanges();

                return List;
            }
            catch (Exception ex)
            {
                return new List<ProgramacionPersonalContrato>();
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
