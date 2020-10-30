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

        public async Task<dynamic> GetListContratacion()
        {
            List<dynamic> listaContrats = new List<dynamic>();

            //List<VRequisitosTecnicosInicioConstruccion> lista = await _context.VRequisitosTecnicosInicioConstruccion.Where(r => r.TipoContratoCodigo == ConstanCodigoTipoContrato.Obra).ToListAsync();

            //lista.ForEach(c =>
            //{
            //    listaContrats.Add(new
            //    {
            //        c.ContratoId,
            //        c.FechaAprobacion,
            //        c.TipoContratoCodigo,
            //        c.NumeroContrato,
            //        c.CantidadProyectosAsociados,
            //        c.CantidadProyectosRequisitosAprobados,
            //        CantidadProyectosRequisitosPendientes = c.CantidadProyectosAsociados - c.CantidadProyectosRequisitosAprobados,
            //        c.EstadoCodigo,
            //        c.EstadoNombre,
            //        c.ExisteRegistro, 
            //    });
            //});

            List<Contrato> listContratos = await _context.Contrato
                .FromSqlRaw("SELECT c.* FROM	dbo.Contrato AS c " +
                "INNER JOIN dbo.Contratacion AS ctr ON c.ContratacionId = ctr.ContratacionId " +
                "INNER JOIN dbo.DisponibilidadPresupuestal AS dp ON ctr.ContratacionId = dp.ContratacionId " +
                "WHERE dp.NumeroDDP IS NOT NULL " +
                "AND ctr.TipoSolicitudCodigo = 1")
               .Include(r => r.Contratacion)
                   .ThenInclude(r => r.ContratacionProyecto)
                       .ThenInclude(r => r.Proyecto)
                            .ThenInclude(r => r.ContratoPerfil)
                .Include(r => r.Contratacion)
                  .ThenInclude(r => r.DisponibilidadPresupuestal)
             .ToListAsync();


            foreach (var c in listContratos)
            {
                int CantidadProyectosConPerfilesAprobados = 0;
                int CantidadProyectosConPerfilesPendientes = 0;
                foreach (var ContratacionProyecto in c.Contratacion.ContratacionProyecto)
                {
                    if (ContratacionProyecto.Proyecto.ContratoPerfil.Count() == 0)
                        CantidadProyectosConPerfilesPendientes++;
                    else if (ContratacionProyecto.Proyecto.ContratoPerfil.Count(r => !(bool)r.Eliminado) == ContratacionProyecto.Proyecto.ContratoPerfil.Count(r => !(bool)r.Eliminado && r.RegistroCompleto))
                        CantidadProyectosConPerfilesAprobados++;
                    else
                        CantidadProyectosConPerfilesPendientes++;
                }
                listaContrats.Add(new
                {
                    c.ContratoId,
                    FechaAprobacion = c.FechaAprobacionRequisitos,
                    c.Contratacion.TipoSolicitudCodigo,
                    c.NumeroContrato,
                    CantidadProyectosAsociados = c.Contratacion.ContratacionProyecto.Count(r => !r.Eliminado),
                    CantidadProyectosRequisitosAprobados = CantidadProyectosConPerfilesAprobados,
                    CantidadProyectosConPerfilesPendientes,
                    EstadoCodigo = c.EstadoVerificacionCodigo,
                    c.EstaDevuelto,
                });
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

            try
            {
                foreach (var ContratacionProyecto in pContrato.Contratacion.ContratacionProyecto)
                {
                    //Guardar estado de la fase 1 preConstruccion 
                    if (ContratacionProyecto.Proyecto.TieneEstadoFase1Diagnostico != null)
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
                             
                            contratoPerfilOld.ConObervacionesSupervision = ContratoPerfil.ConObervacionesSupervision;
                            contratoPerfilOld.RegistroCompleto = ValidarRegistroCompletoContratoPerfil(contratoPerfilOld);

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
                                        ContratoPerfilObservacion.Observacion = ContratoPerfilObservacion.Observacion.ToUpper();
                                    ContratoPerfilObservacion.UsuarioCreacion = pContrato.UsuarioCreacion;
                                    ContratoPerfilObservacion.FechaCreacion = DateTime.Now;
                                    ContratoPerfilObservacion.TipoObservacionCodigo = ConstanCodigoTipoObservacion.Interventoria;
                                    ContratoPerfilObservacion.Eliminado = false;
                                    _context.ContratoPerfilObservacion.Add(ContratoPerfilObservacion);
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
                //Cambiar Estado Requisitos 
                if (pContrato.ContratoPerfil.Where(r => (bool)r.RegistroCompleto).Count() == pContrato.ContratoPerfil.Count() && pContrato.ContratoPerfil.Count() > 1)
                {
                    Contrato contratoOld = _context.Contrato.Find(pContrato.ContratoId);
                    contratoOld.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_aprobados;
                    contratoOld.UsuarioModificacion = pContrato.UsuarioCreacion;
                    contratoOld.FechaModificacion = DateTime.Now;
                }
                else {
                    Contrato contratoOld = _context.Contrato.Find(pContrato.ContratoId);
                    contratoOld.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.En_proceso_de_aprobacion_de_requisitos_tecnicos;
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

        private bool ValidarRegistroCompletoContratoPerfil(ContratoPerfil contratoPerfilOld)
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

        public async Task<Respuesta> ChangeStateContrato(int pContratoId, string UsuarioModificacion, string pEstadoVerificacionContratoCodigo)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Verificacion_Contrato, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contrato contratoMod = _context.Contrato.Find(pContratoId);
                contratoMod.FechaModificacion = DateTime.Now;
                contratoMod.UsuarioModificacion = UsuarioModificacion;
                contratoMod.EstadoVerificacionCodigo = pEstadoVerificacionContratoCodigo;
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
