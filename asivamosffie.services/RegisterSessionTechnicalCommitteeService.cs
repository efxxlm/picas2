using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.api.Controllers;
using AuthorizationTest.JwtHelpers;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Globalization;
using asivamosffie.services.Validators;
using asivamosffie.services.Filters;
using System.Data.Common;
using Z.EntityFramework.Plus;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace asivamosffie.services
{
    public class RegisterSessionTechnicalCommitteeService : IRegisterSessionTechnicalCommitteeService
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public RegisterSessionTechnicalCommitteeService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }
        public bool ValidarCamposSesionComiteTema(SesionComiteTema pSesionComiteTema) {

            if (
                !string.IsNullOrEmpty(pSesionComiteTema.ResponsableCodigo) ||
                !string.IsNullOrEmpty(pSesionComiteTema.TiempoIntervencion.ToString()) ||
                !string.IsNullOrEmpty(pSesionComiteTema.RutaSoporte) ||
                !string.IsNullOrEmpty(pSesionComiteTema.Observaciones) ||
                !string.IsNullOrEmpty(pSesionComiteTema.EsAprobado.ToString()) ||
                !string.IsNullOrEmpty(pSesionComiteTema.ObservacionesDecision) ||
                !string.IsNullOrEmpty(pSesionComiteTema.ObservacionesDecision)  
                ) { return false; }

            return true;
        }

        public async Task<List<dynamic>> GetListSesionComiteTemaByIdSesion(int pIdSesion) {

            var ListSesionComiteTema = _context.SesionComiteTema.Where(r => r.SesionId == pIdSesion && !(bool)r.Eliminado).ToList(); 

            List<dynamic> ListSesionComiteTemaDyn = new List<dynamic>();

            foreach (var sesionComiteTema in ListSesionComiteTema)
            {
                ListSesionComiteTemaDyn.Add(
                                            new 
                                            {
                                                

                                            }
                                            );
            }


            return ListSesionComiteTemaDyn;
        }

        public async Task<List<ComiteGrilla>> GetComiteGrilla()
        {
            List<ComiteGrilla> ListComiteGrilla = new List<ComiteGrilla>();

            try
            {
                var ListComiteTecnico = await _context.Sesion.Where(r => !(bool)r.Eliminado).Select(x => new
                {
                    Id = x.SesionId,
                    FechaComite = x.FechaCreacion.ToString(),
                    EstadoComite = x.EstadoComiteCodigo,
                    x.NumeroComite
                }).Distinct().OrderByDescending(r => r.Id).ToListAsync();

                foreach (var comite in ListComiteTecnico)
                {
                    ComiteGrilla comiteGrilla = new ComiteGrilla
                    {
                        Id = comite.Id,
                        FechaComite = comite.FechaComite,
                        EstadoComite = !string.IsNullOrEmpty(comite.EstadoComite) ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(comite.EstadoComite, (int)EnumeratorTipoDominio.Estado_Comite) : "---"
                    };

                }
            }
            catch (Exception)
            {
            }
            return ListComiteGrilla;
        }

        public async Task<Respuesta> SaveEditSesionComiteTema(List<SesionComiteTema> pListSesionComiteTema, DateTime pFechaProximoComite)
        {
            try
            {
                int idAccionCrearEditarSesionComiteTema = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_SesionComiteTema, (int)EnumeratorTipoDominio.Acciones);

                using DbContextTransaction transaction = (DbContextTransaction)_context.Database.BeginTransaction();
                try
                {
                    string EditarOcrear = "";

                    //Crear Sesión para asignarla a los temas
                    Sesion newSesion = new Sesion();

                    foreach (var SesionComiteTema in pListSesionComiteTema)
                    {
                        if (SesionComiteTema.SesionTemaId == 0)
                        {
                            //Crear Sesion papa de comite tema 
                            //Valido que solo cree una ya que puede entrar a editar 
                            if (newSesion.SesionId == 0)
                            {
                                newSesion.FechaCreacion = DateTime.Now;
                                newSesion.UsuarioCreacion = pListSesionComiteTema.FirstOrDefault().UsuarioCreacion;
                                newSesion.Eliminado = false;

                                newSesion.FechaOrdenDia = pFechaProximoComite;
                                newSesion.NumeroComite = await _commonService.EnumeradorComite();
                                newSesion.EstadoComiteCodigo = ConstanCodigoEstadoComite.Sin_Convocatoria;

                                newSesion.EsCompleto = false;
                                _context.Sesion.Add(newSesion);
                                _context.SaveChanges();
                            }

                            EditarOcrear = "CREAR SESIÓN COMITE TECNICO TEMA";
                            SesionComiteTema.Eliminado = false;
                            SesionComiteTema.FechaCreacion = DateTime.Now;
                            SesionComiteTema.UsuarioCreacion = pListSesionComiteTema.FirstOrDefault().UsuarioCreacion;
                            SesionComiteTema.SesionId = newSesion.SesionId;
                            _context.SesionComiteTema.Add(SesionComiteTema);
                        }
                        else
                        {
                            EditarOcrear = "EDITAR SESIÓN COMITE TECNICO TEMA";
                            SesionComiteTema sesionComiteTemaOld = _context.SesionComiteTema.Find(SesionComiteTema.SesionTemaId);
                            sesionComiteTemaOld.FechaModificacion = DateTime.Now;
                            sesionComiteTemaOld.UsuarioModificacion = pListSesionComiteTema.FirstOrDefault().UsuarioCreacion;

                            sesionComiteTemaOld.Tema = SesionComiteTema.Tema;
                            sesionComiteTemaOld.ResponsableCodigo = SesionComiteTema.ResponsableCodigo;
                            sesionComiteTemaOld.TiempoIntervencion = SesionComiteTema.TiempoIntervencion;
                            sesionComiteTemaOld.RutaSoporte = SesionComiteTema.RutaSoporte;
                            sesionComiteTemaOld.Observaciones = SesionComiteTema.Observaciones;
                            sesionComiteTemaOld.EsAprobado = SesionComiteTema.EsAprobado;
                        }
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                    return
                         new Respuesta
                         {
                             IsSuccessful = true,
                             IsException = false,
                             IsValidation = false,
                             Code = ConstantSesionComiteTecnico.OperacionExitosa,
                             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionCrearEditarSesionComiteTema, pListSesionComiteTema.FirstOrDefault().UsuarioCreacion, EditarOcrear)
                         };

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return
                           new Respuesta
                           {
                               IsSuccessful = false,
                               IsException = true,
                               IsValidation = false,
                               Code = ConstantSesionComiteTecnico.Error,
                               Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionCrearEditarSesionComiteTema, pListSesionComiteTema.FirstOrDefault().UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                           };
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<dynamic>> GetListSolicitudesContractuales()
        {
            List<dynamic> ListValidacionSolicitudesContractualesGrilla = new List<dynamic>();

            try
            {
                var ListComiteTecnico = _context.ComiteTecnico.Where(r => !(bool)r.Eliminado).Select(x => new
                {
                    Id = x.ComiteTecnicoId,
                    FechaSolicitud = x.FechaCreacion,
                    TipoSolicitud = x.TipoSolicitudCodigo,
                    x.NumeroSolicitud
                }).OrderByDescending(r => r.Id).ToList();


                foreach (var comiteTecnico in ListComiteTecnico)
                {
                    ListValidacionSolicitudesContractualesGrilla.Add(new
                    {
                        comiteTecnico.Id,
                        FechaSolicitud = comiteTecnico.FechaSolicitud.ToString("yyyy-MM-dd"),
                        comiteTecnico.NumeroSolicitud,
                        TipoSolicitud = await _commonService.GetNombreDominioByCodigoAndTipoDominio(comiteTecnico.TipoSolicitud, (int)EnumeratorTipoDominio.Tipo_Solicitud)
                    });
                };

            }
            catch (Exception ex)
            {

            }

            return ListValidacionSolicitudesContractualesGrilla;
        }

        public async Task<Sesion> GetSesionBySesionId(int pSesionId)
        {
            return await _context.Sesion.Where(r => r.SesionId == pSesionId && !(bool)r.Eliminado).IncludeFilter(r => r.SesionComiteTema.Where(r => !(bool)r.Eliminado)).FirstOrDefaultAsync();
        }

        public async Task<Respuesta> EliminarSesionComiteTema(int pSesionComiteTemaId, string pUsuarioModificacion)
        {
            int idAccionEliminarSesionComiteTema = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Sesion_Comite_Tema, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SesionComiteTema sesionComiteTemaOld = await _context.SesionComiteTema.Where(r => r.SesionTemaId == pSesionComiteTemaId).FirstOrDefaultAsync();
                sesionComiteTemaOld.Eliminado = true;
                sesionComiteTemaOld.FechaModificacion = DateTime.Now;
                sesionComiteTemaOld.UsuarioCreacion = pUsuarioModificacion;
                _context.Update(sesionComiteTemaOld);
                _context.SaveChanges();

                return
                 new Respuesta
                 {
                     IsSuccessful = true,
                     IsException = false,
                     IsValidation = false,
                     Code = ConstantSesionComiteTecnico.OperacionExitosa,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionEliminarSesionComiteTema, pUsuarioModificacion, "ELIMINAR SESIÓN COMITE TEMA")
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
                        Code = ConstantSesionComiteTecnico.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionEliminarSesionComiteTema, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                    };
            }

        }

        public async Task<Respuesta> CambiarEstadoComite(Sesion pSesion)
        {
            int idAccionCambiarEstadoSesion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Comite_Sesion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Sesion sesionOld = _context.Sesion.Find(pSesion.SesionId);

                string NombreEstado = await _commonService.GetNombreDominioByCodigoAndTipoDominio(pSesion.EstadoComiteCodigo, (int)EnumeratorTipoDominio.Estado_Comite);
                sesionOld.UsuarioModificacion = pSesion.UsuarioCreacion;
                sesionOld.FechaModificacion = DateTime.Now;
                sesionOld.EstadoComiteCodigo = pSesion.EstadoComiteCodigo;

                _context.Update(sesionOld);
                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionCambiarEstadoSesion, pSesion.UsuarioCreacion, "ESTADO COMITE CAMBIADO A " + NombreEstado.ToUpper())
                    };
            }
            catch (Exception)
            {

                throw;
            }

        }
         
        public async Task<Respuesta> GuardarInvitado(Sesion pSesion)
        {
            int idAccionCambiarEstadoSesion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Comite_Sesion, (int)EnumeratorTipoDominio.Acciones);

            try
            {

                Sesion sesion = _context.Sesion.Where(r => (bool)r.Eliminado).IncludeFilter(r => r.SesionInvitado).FirstOrDefault();
                Sesion sesionOld = _context.Sesion.Find(pSesion.SesionId);

                string NombreEstado = await _commonService.GetNombreDominioByCodigoAndTipoDominio(pSesion.EstadoComiteCodigo, (int)EnumeratorTipoDominio.Estado_Comite);
                sesionOld.UsuarioModificacion = pSesion.UsuarioCreacion;
                sesionOld.FechaModificacion = DateTime.Now;
                sesionOld.EstadoComiteCodigo = pSesion.EstadoComiteCodigo;

                _context.Update(sesionOld);
                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionCambiarEstadoSesion, pSesion.UsuarioCreacion, "ESTADO COMITE CAMBIADO A " + NombreEstado.ToUpper())
                    };
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool EjemploTransaction()
        {
            using (DbContextTransaction transaction = (DbContextTransaction)_context.Database.BeginTransaction())
            {
                try
                {
                    var standard = _context.ArchivoCargue.Add(new ArchivoCargue() { Activo = true });

                    _context.Usuario.Add(new Usuario()
                    {
                        NombreMaquina = "Rama",
                        Nombres = "Julian"
                    });
                    _context.SaveChanges();
                    // throw exectiopn to test roll back transaction

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error occurred.");
                }
            }
            return false;
        }
    }
}
