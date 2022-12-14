using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using Z.EntityFramework.Plus;
using asivamosffie.services.Helpers.Constants;
using System.Net;

namespace asivamosffie.services
{
    public class ProjectContractingService : IProjectContractingService
    {

        private readonly ICommonService _commonService;
        private readonly IRequestBudgetAvailabilityService _requestBudgetAvailabilityService;
        private readonly IProjectService _projectService;
        private readonly devAsiVamosFFIEContext _context;

        public ProjectContractingService(devAsiVamosFFIEContext context, IRequestBudgetAvailabilityService requestBudgetAvailabilityService, ICommonService commonService, IProjectService projectService)
        {
            _requestBudgetAvailabilityService = requestBudgetAvailabilityService;
            _commonService = commonService;
            _projectService = projectService;
            _context = context;
        }
        public async Task<Respuesta> DeleteComponenteUso(int DeleteComponenteUso, string pUsuarioMod)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                await _context.Set<ComponenteUso>().Where(c => c.ComponenteUsoId == DeleteComponenteUso)
                                                   .UpdateAsync(c => new ComponenteUso
                                                   {
                                                       Eliminado = true,
                                                       UsuarioModificacion = pUsuarioMod
                                                   });

                respuesta.IsSuccessful = true;
                respuesta.Code = HttpStatusCode.OK.ToString();
            }
            catch (Exception)
            {
                respuesta.Code = HttpStatusCode.InternalServerError.ToString();
            }

            return new Respuesta();
        }

        public async Task<Respuesta> DeleteComponenteAportante(int pComponenteAportanteId, string pUsuarioMod)
        {
            ComponenteAportante componenteAportanteDelete = _context.ComponenteAportante.Find(pComponenteAportanteId);

            componenteAportanteDelete.Eliminado = true;
            componenteAportanteDelete.FechaModificacion = DateTime.Now;
            componenteAportanteDelete.UsuarioModificacion = pUsuarioMod;
            _context.Update(componenteAportanteDelete);

            await _context.SaveChangesAsync();


            return new Respuesta();
        }

        public async Task<Respuesta> ChangeStateContratacionByIdContratacion(
            int idContratacion,
            string PCodigoEstado,
            string pUsusarioModifico,
            string pDominioFront,
            string pMailServer,
            int pMailPort,
            bool pEnableSSL,
            string pPassword,
            string pSentender
            )
        {
            int idAccionEliminarContratacionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Contratacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {


                Contratacion contratacionOld = _context.Contratacion.Find(idContratacion);
                contratacionOld.FechaTramite = DateTime.Now;
                contratacionOld.UsuarioModificacion = pUsusarioModifico;
                contratacionOld.FechaModificacion = DateTime.Now;
                contratacionOld.EstadoSolicitudCodigo = PCodigoEstado;
                _context.SaveChanges();

                List<Dominio> TipoObraIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar)
                                                                     .ToList();

                string strTipoObraIntervencion = TipoObraIntervencion.Where(r => r.Codigo == contratacionOld.TipoSolicitudCodigo)
                                                                     .Select(r => r.Nombre)
                                                                     .FirstOrDefault();

                if (PCodigoEstado == ConstanCodigoEstadoSolicitudContratacion.En_tramite)
                {
                    var usuariosecretario = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Secretario_Comite)
                                                                  .Select(x => x.Usuario.Email)
                                                                  .ToList();

                    Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjEnviarSolicitudContratacion);

                    foreach (var usuario in usuariosecretario)
                    {

                        string template = TemplateRecoveryPassword.Contenido
                                                                            .Replace("_LinkF_", pDominioFront)
                                                                            .Replace("[NUMERO_SOLICITUD]", contratacionOld.NumeroSolicitud)
                                                                            .Replace("[FECHA_SOLICITUD]", ((DateTime)contratacionOld.FechaTramite).ToString("dd-MM-yyyy"))
                                                                            .Replace("[OBRA_INTERVENTORIA]", strTipoObraIntervencion);

                        bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuario, "Solicitud  de contratación", template, pSentender, pPassword, pMailServer, pMailPort);
                    }
                }

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContratacionProyecto.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto,
                                                                                            ConstantMessagesContratacionProyecto.OperacionExitosa,
                                                                                            idAccionEliminarContratacionProyecto,
                                                                                            pUsusarioModifico,
                                                                                            "CAMBIAR ESTADO CONTRATACION"
                                                                                            )
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContratacionProyecto.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto,
                                                                                            ConstantMessagesContratacionProyecto.Error,
                                                                                            idAccionEliminarContratacionProyecto,
                                                                                            pUsusarioModifico,
                                                                                            ex.InnerException.ToString()
                                                                                            )
                };
            }

        }

        public async Task<Respuesta> DeleteContratacionByIdContratacion(int idContratacion, string pUsusarioElimino)
        {
            int idAccionEliminarContratacionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Contratacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contratacion contratacionOld = _context.Contratacion.Where(r => r.ContratacionId == idContratacion)
                                                                    .Include(r => r.ContratacionProyecto)
                                                                    .FirstOrDefault();

                foreach (var ContratacionProyecto in contratacionOld.ContratacionProyecto)
                {
                    //Eliminar Relacion ContratacionProyecto cuando se elimina
                    ContratacionProyecto contratacionProyectoEliminar = _context.ContratacionProyecto.Find(ContratacionProyecto.ContratacionProyectoId);
                    contratacionProyectoEliminar.Eliminado = true;
                    contratacionProyectoEliminar.UsuarioModificacion = pUsusarioElimino;
                    contratacionProyectoEliminar.FechaModificacion = DateTime.Now;


                    //Cambiar estado Proyecto cuando se elimina
                    //Validar si el proyecto esta en otro contrato si no esta deja el estado no completo para que se pueda editar
                    bool blTieneOtroContratoElProyecto = _context.ContratacionProyecto.Count(r => r.ProyectoId == ContratacionProyecto.ProyectoId
                                                                                        && r.ContratacionId != ContratacionProyecto.ContratacionId
                                                                                        && r.Eliminado != true) > 0;

                    Proyecto proyectoCambiarEstadoEliminado = _context.Proyecto.Find(ContratacionProyecto.ProyectoId);
                    proyectoCambiarEstadoEliminado.UsuarioModificacion = pUsusarioElimino;
                    proyectoCambiarEstadoEliminado.FechaModificacion = DateTime.Now;

                    if (!blTieneOtroContratoElProyecto)
                        proyectoCambiarEstadoEliminado.RegistroCompleto = false;

                    if (contratacionOld.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                        proyectoCambiarEstadoEliminado.EstadoProyectoInterventoriaCodigo = ConstantCodigoEstadoProyecto.Disponible;

                    if (contratacionOld.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                        proyectoCambiarEstadoEliminado.EstadoProyectoObraCodigo = ConstantCodigoEstadoProyecto.Disponible;
                }
                contratacionOld.Eliminado = true;
                contratacionOld.UsuarioModificacion = pUsusarioElimino;
                contratacionOld.FechaModificacion = DateTime.Now;
                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContratacionProyecto.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesContratacionProyecto.OperacionExitosa, idAccionEliminarContratacionProyecto, pUsusarioElimino, "ELIMINAR CONTRATO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContratacionProyecto.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesContratacionProyecto.Error, idAccionEliminarContratacionProyecto, pUsusarioElimino, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        public async Task<Contratacion> GetAllContratacionByContratacionId(int pContratacionId)
        {
            try
            {
                return await _context.Contratacion
                    .Where(r => r.ContratacionId == pContratacionId)
                   //para logica plantilla ficha contratacion
                   .Include(r => r.DisponibilidadPresupuestal)
                   .Include(r => r.Contrato)
                    .Include(r => r.ContratacionProyecto)
                    .ThenInclude(r => r.ContratacionProyectoAportante)
                     .ThenInclude(r => r.CofinanciacionAportante)
                       .ThenInclude(r => r.ProyectoAportante)
                           .Include(r => r.ContratacionProyecto)
                    .ThenInclude(r => r.ContratacionProyectoAportante)
                     .ThenInclude(r => r.ComponenteAportante)
                       .ThenInclude(r => r.ComponenteUso)
                  // 
                  .Include(r => r.Contratista)
                     .Include(r => r.ContratacionProyecto)
                       .ThenInclude(r => r.Proyecto)
                       .ThenInclude(r => r.ProyectoAportante)
                         .ThenInclude(r => r.Aportante)
                           .ThenInclude(r => r.NombreAportante)
                               .Include(r => r.ContratacionProyecto)
                       .ThenInclude(r => r.Proyecto)
                       .ThenInclude(r => r.ProyectoAportante)
                         .ThenInclude(r => r.Aportante)
                           .ThenInclude(r => r.Departamento)
                  .Include(r => r.ContratacionProyecto)
                       .ThenInclude(r => r.Proyecto)
                               .ThenInclude(r => r.ProyectoPredio)
                                    .ThenInclude(r => r.Predio)
                   .Include(r => r.ContratacionProyecto)
                       .ThenInclude(r => r.Proyecto)
                          .ThenInclude(r => r.PredioPrincipal)
                   .Include(r => r.ContratacionProyecto)
                       .ThenInclude(r => r.Proyecto)
                          .ThenInclude(r => r.InfraestructuraIntervenirProyecto)
                   .Include(r => r.ContratacionProyecto)
                     .ThenInclude(r => r.ContratacionProyectoAportante)
                        .ThenInclude(r => r.ComponenteAportante)
                            .ThenInclude(r => r.ComponenteUso).Where(r => !(bool)r.Eliminado)
                   .Include(c => c.PlazoContratacion)
                  .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Contratacion> GetContratacionByContratacionId(int pContratacionId)
        {
            Contratacion contratacion = await _context.Contratacion.Where(r => r.ContratacionId == pContratacionId)
                .Include(r => r.Contratista)
                .Include(r => r.Contrato)

                .Include(r => r.ContratacionProyecto)
                   .ThenInclude(r => r.ContratacionProyectoAportante)
                        .ThenInclude(r => r.CofinanciacionAportante)
               //    .ThenInclude(r => r.ProyectoAportante)

               //.Include(r => r.ContratacionProyecto)
               //  .ThenInclude(r => r.Proyecto)
               //  .ThenInclude(r => r.ProyectoAportante)
               //    .ThenInclude(r => r.Aportante)
               //      .ThenInclude(r => r.NombreAportante)

               //.Include(r => r.ContratacionProyecto)
               //  .ThenInclude(r => r.Proyecto)
               //    .ThenInclude(r => r.ProyectoAportante)
               //      .ThenInclude(r => r.Aportante)
               //        .ThenInclude(r => r.Departamento)

               //   .Include(r => r.ContratacionProyecto)
               //        .ThenInclude(r => r.Proyecto)
               //                .ThenInclude(r => r.ProyectoPredio)
               //                     .ThenInclude(r => r.Predio)
               //.Include(r => r.ContratacionProyecto)
               //    .ThenInclude(r => r.Proyecto)
               //       .ThenInclude(r => r.PredioPrincipal)

               .Include(r => r.ContratacionProyecto)
                    .ThenInclude(r => r.Proyecto)
               .Include(r => r.ContratacionProyecto)
                   .ThenInclude(r => r.Proyecto)
                        .ThenInclude(r => r.Sede)
               .Include(r => r.ContratacionProyecto)
                   .ThenInclude(r => r.Proyecto)
                        .ThenInclude(r => r.InstitucionEducativa)
                .Include(r => r.ContratacionProyecto)
                       .ThenInclude(r => r.Proyecto)
                           .ThenInclude(r => r.LocalizacionIdMunicipioNavigation)

                 //       .ThenInclude(r => r.InfraestructuraIntervenirProyecto)
                 //.Include(r => r.ContratacionProyecto)
                 //    .ThenInclude(r => r.ContratacionProyectoAportante)
                 //      .ThenInclude(r => r.ComponenteAportante)
                 //           .ThenInclude(r => r.ComponenteUso)
                 .Include(p => p.PlazoContratacion)
                           .FirstOrDefaultAsync();


            //foreach (var ContratacionProyecto in contratacion.ContratacionProyecto)
            //{
            //    foreach (var ContratacionProyectoAportante in ContratacionProyecto.ContratacionProyectoAportante)
            //    {
            //        foreach (var ComponenteAportante in ContratacionProyectoAportante.ComponenteAportante)
            //        {
            //            if (ComponenteAportante.ComponenteUso.Count > 0)
            //                ComponenteAportante.ComponenteUso = ComponenteAportante.ComponenteUso.Where(c => c.Eliminado != true).ToList();
            //        }
            //    }
            //}

            contratacion.ContratacionProyecto = contratacion.ContratacionProyecto.Where(r => !(bool)r.Eliminado).ToList();

            List<Dominio> ListTipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && (bool)r.Activo).ToList();
            List<Localizacion> ListDepartamentos = _context.Localizacion.Where(r => r.Nivel == (int)ConstanCodigoTipoNivelLocalizacion.Departamento).ToList();
            List<Localizacion> ListRegiones = _context.Localizacion.Where(r => r.Nivel == (int)ConstanCodigoTipoNivelLocalizacion.Region).ToList();

            //List<Dominio> ListFases = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Fases && (bool)r.Activo).ToList();

            foreach (var item in contratacion.ContratacionProyecto)
            {
                if (!(bool)item.Proyecto.Eliminado)
                {
                    //Proyecto proyectoTem = await _context.Proyecto.Where(r => r.ProyectoId == item.Proyecto.ProyectoId)
                    //       .Include(r => r.Sede)
                    //       .Include(r => r.InstitucionEducativa)
                    //       .Include(r => r.LocalizacionIdMunicipioNavigation).FirstOrDefaultAsync();

                    Localizacion departamento = ListDepartamentos.Find(r => r.LocalizacionId == item.Proyecto.LocalizacionIdMunicipioNavigation.IdPadre);

                    item.Proyecto.LocalizacionIdMunicipio = item.Proyecto.LocalizacionIdMunicipioNavigation.Descripcion + " / " + departamento.Descripcion;
                    item.Proyecto.UsuarioModificacion = ListRegiones.Find(r => r.LocalizacionId == departamento.IdPadre).Descripcion;
                    item.Proyecto.TipoIntervencionCodigo = ListTipoIntervencion.Find(r => r.Codigo == item.Proyecto.TipoIntervencionCodigo).Nombre;

                    item.Proyecto.LocalizacionIdMunicipioNavigation = null;
                    //item.Proyecto.LocalizacionIdMunicipioNavigation.Proyecto = null;
                    //item.Proyecto.LocalizacionIdMunicipioNavigation.CofinanciacionAportanteDepartamento = null;
                    //item.Proyecto.LocalizacionIdMunicipioNavigation.CofinanciacionAportanteMunicipio = null;

                    // item.Proyecto.ProyectoAportante = item.Proyecto.ProyectoAportante.Where(r => r.Eliminado != true).ToList();

                    foreach (var praportante in item.ContratacionProyectoAportante)
                    {

                        praportante.CofinanciacionAportante.NombreAportanteString = _requestBudgetAvailabilityService.getNombreAportante(praportante.CofinanciacionAportante);
                        praportante.CofinanciacionAportante.ContratacionProyectoAportante = null;
                        praportante.CofinanciacionAportante.Departamento = null;
                        praportante.CofinanciacionAportante.Municipio = null;
                        praportante.CofinanciacionAportante.NombreAportante = null;
                        praportante.CofinanciacionAportante.ValorObraInterventoria = _context.ProyectoAportante.Where(c => c.ProyectoId == item.ProyectoId
                                                                                                                   && c.AportanteId == praportante.CofinanciacionAportanteId)
                                                                                                               .Select(c => new
                                                                                                               {
                                                                                                                   ValorInterventoria = c.ValorInterventoria ?? 0,
                                                                                                                   ValorObra = c.ValorObra ?? 0
                                                                                                               }
                                                                                                                      )
                                                                                                              .ToList();

                        praportante.ComponenteAportante = _context.ComponenteAportante.Where(c => c.ContratacionProyectoAportanteId == praportante.ContratacionProyectoAportanteId && c.Eliminado != true).ToList();
                        //OLD
                        //if (praportante.CofinanciacionAportante.TipoAportanteId == ConstanTipoAportante.Ffie)
                        //{
                        //    praportante.CofinanciacionAportante.NombreAportanteString = ConstanStringTipoAportante.Ffie;
                        //}
                        //else if (praportante.CofinanciacionAportante.TipoAportanteId == ConstanTipoAportante.ET)
                        //{
                        //    //verifico si tiene municipio
                        //    if (praportante.CofinanciacionAportante.MunicipioId != null)
                        //    {
                        //        praportante.CofinanciacionAportante.NombreAportanteString = "Alcaldía de " + _context.Localizacion.Find(praportante.CofinanciacionAportante.MunicipioId).Descripcion;
                        //    }
                        //    else//solo departamento
                        //    {
                        //        praportante.CofinanciacionAportante.NombreAportanteString = "Gobernación de " + praportante.CofinanciacionAportante.DepartamentoId == null ? "Error" : _context.Localizacion.Find(praportante.CofinanciacionAportante.DepartamentoId).Descripcion;
                        //    }
                        //}
                        //else
                        //{
                        //    praportante.CofinanciacionAportante.NombreAportanteString = _context.Dominio.Find(praportante.CofinanciacionAportante.NombreAportanteId).Nombre;
                        //}
                        //praportante.CofinanciacionAportante.TipoAportanteString = _context.Dominio.Find(praportante.CofinanciacionAportante.TipoAportanteId).Nombre;

                    }

                    ComiteTecnico comite = _context.SesionComiteSolicitud.Where(x => x.SolicitudId == pContratacionId
                                                                                  && x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion)
                                                                        .Include(x => x.ComiteTecnico)
                                                                        .Select(x => x.ComiteTecnico)
                                                                        .FirstOrDefault();

                    if (comite != null)
                        contratacion.FechaComiteTecnicoNotMapped = Convert.ToDateTime(comite.FechaOrdenDia ?? DateTime.Now);
                }
            }

            return contratacion;
        }

        public async Task<Contratacion> GetContratacionByContratacionIdWithGrillaProyecto(int pContratacionId)
        {
            Contratacion contratacion = await
                _context.Contratacion
                .Where(r => r.ContratacionId == pContratacionId)
                .Include(r => r.ContratacionProyecto).ThenInclude(r => r.SesionSolicitudObservacionProyecto)
                .Include(r => r.ContratacionProyecto).ThenInclude(r => r.ContratacionObservacion)
                .FirstOrDefaultAsync();

            contratacion.ContratacionProyecto = contratacion.ContratacionProyecto.Where(r => !(bool)r.Eliminado).ToList();

            foreach (var ContratacionProyecto in contratacion.ContratacionProyecto)
            {
                ContratacionProyecto.ProyectoGrilla = await _projectService.GetProyectoGrillaByProyectoId(ContratacionProyecto.ProyectoId);
            }

            return contratacion;
        }

        public async Task<List<ContratacionProyecto>> GetListContratacionProyectoByContratacionId(int idContratacion)
        {
            List<ContratacionProyecto> ListContratacionProyecto = new List<ContratacionProyecto>();

            ListContratacionProyecto = await _context.ContratacionProyecto.
                Where(r => !(bool)r.Eliminado && r.ContratacionId == idContratacion).
                IncludeFilter(r => r.Proyecto).Where(r => !(bool)r.Eliminado).
                IncludeFilter(r => r.Contratacion.Contratista).Where(r => !(bool)r.Eliminado).
                IncludeFilter(r => r.ContratacionProyectoAportante.Where(r => !(bool)r.Eliminado)).ToListAsync();
            return ListContratacionProyecto;
        }

        public async Task<ContratacionProyecto> GetContratacionProyectoById(int idContratacionProyecto)
        {
            ContratacionProyecto contratacionProyecto = new ContratacionProyecto();

            contratacionProyecto = await _context.ContratacionProyecto
                .Where(r => !(bool)r.Eliminado && r.ContratacionProyectoId == idContratacionProyecto)

                //Tipo ET DEPARTAMENTO
                //.Include(r => r.Proyecto)
                //     .ThenInclude(r => r.ProyectoAportante)
                //         .ThenInclude(r => r.Aportante)
                //           .ThenInclude(r => r.Departamento)


                ////Tipo ET MUNICIPIO
                //.Include(r => r.Proyecto)
                //   .ThenInclude(r => r.ProyectoAportante)
                //       .ThenInclude(r => r.Aportante)
                //         .ThenInclude(r => r.Municipio)

                ////Tipo TERCERO DOMINIO
                //.Include(r => r.Proyecto)
                //     .ThenInclude(r => r.ProyectoAportante)
                //         .ThenInclude(r => r.Aportante)
                //.ThenInclude(r => r.NombreAportante)

                .Include(r => r.ContratacionProyectoAportante)
                    .ThenInclude(r => r.ComponenteAportante)
                        .ThenInclude(r => r.ComponenteUso)

                 .Include(r => r.Proyecto)
                     .ThenInclude(r => r.InstitucionEducativa)
                  .Include(r => r.Proyecto)
                      .ThenInclude(r => r.Sede)
                  .Include(r => r.Proyecto)
                      .ThenInclude(r => r.LocalizacionIdMunicipioNavigation)


                 .Include(r => r.Contratacion)
                     .ThenInclude(r => r.ContratacionProyecto)
                        .ThenInclude(r => r.ContratacionProyectoAportante)
                            .ThenInclude(r => r.CofinanciacionAportante)
                              .ThenInclude(r => r.FuenteFinanciacion)
                                .ThenInclude(r => r.ControlRecurso)

                .FirstOrDefaultAsync();

            foreach (var ContratacionProyectoAportante in contratacionProyecto.ContratacionProyectoAportante)
            {
                decimal ValorGastado = 0;
                decimal ValorDisponible = (decimal)ContratacionProyectoAportante.CofinanciacionAportante.FuenteFinanciacion.Select(r => r.ValorFuente).Sum();

                if (ContratacionProyectoAportante.ComponenteAportante.Count() > 0)
                {
                    ContratacionProyectoAportante.ComponenteAportante = ContratacionProyectoAportante.ComponenteAportante.Where(r => !r.Eliminado.HasValue || (r.Eliminado.HasValue && !(bool)r.Eliminado)).ToList();
                }
                foreach (var ComponenteAportante in ContratacionProyectoAportante.ComponenteAportante)
                {
                    ComponenteAportante.ComponenteUso = ComponenteAportante.ComponenteUso.Where(c => c.Eliminado != true).ToList();
                    ValorGastado = ComponenteAportante.ComponenteUso.Select(r => r.ValorUso).Sum();
                }
                ContratacionProyectoAportante.CofinanciacionAportante.ValorObraInterventoria = _context.ProyectoAportante.Where(c => c.ProyectoId == contratacionProyecto.ProyectoId
                                                                                                                   && c.AportanteId == ContratacionProyectoAportante.CofinanciacionAportanteId)
                                                                                                               .Select(c => new
                                                                                                               {
                                                                                                                   ValorInterventoria = c.ValorInterventoria ?? 0,
                                                                                                                   ValorObra = c.ValorObra ?? 0
                                                                                                               }
                                                                                                                      )
                                                                                                              .ToList();
                ContratacionProyectoAportante.CofinanciacionAportante.NombreAportanteString = _requestBudgetAvailabilityService.getNombreAportante(ContratacionProyectoAportante.CofinanciacionAportante);
                ContratacionProyectoAportante.CofinanciacionAportante.ContratacionProyectoAportante = null;
                ContratacionProyectoAportante.SaldoDisponible = ValorDisponible - ValorGastado;
            }

            return contratacionProyecto;
        }

        public async Task<Contratacion> GetListContratacionObservacion(int pContratacionId)
        {
            Contratacion Contratacion = new Contratacion();
            try
            {
                Contratacion = await _context.Contratacion.Where(r => r.ContratacionId == pContratacionId)
                    .Where(r => !(bool)r.Eliminado)
                    .Include(r => r.ContratacionObservacion)
                    .Include(r => r.Contratista)
                    .Include(r => r.ContratacionProyecto)
                        .ThenInclude(r => r.SesionSolicitudObservacionProyecto)
                     .Include(r => r.ContratacionProyecto)
                        .ThenInclude(r => r.Proyecto)
                             .ThenInclude(r => r.Sede)
                    .Include(r => r.ContratacionProyecto)
                        .ThenInclude(r => r.Proyecto)
                                 .ThenInclude(r => r.InstitucionEducativa)
                    .FirstOrDefaultAsync();

                List<Dominio> ListParametricas = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar || r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud).ToList();

                if (!string.IsNullOrEmpty(Contratacion.TipoSolicitudCodigo))
                    Contratacion.TipoSolicitudCodigo = ListParametricas.Where(r => r.Codigo == Contratacion.TipoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar).FirstOrDefault().Nombre;

                if (!string.IsNullOrEmpty(Contratacion.EstadoSolicitudCodigo))
                    Contratacion.EstadoSolicitudCodigo = ListParametricas.Where(r => r.Codigo == Contratacion.EstadoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud).FirstOrDefault().Nombre;


                if (!string.IsNullOrEmpty(Contratacion?.ContratacionObservacion?.FirstOrDefault()?.Observacion))
                {
                    Contratacion.ObservacionNotMapped = Contratacion.ContratacionObservacion.FirstOrDefault().Observacion;
                }
                else
                {
                    SesionComiteSolicitud SesionComiteSolicitud = _context.SesionComiteSolicitud
                                        .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion
                                        && r.SolicitudId == Contratacion.ContratacionId).Include(r => r.ComiteTecnico)
                                        .FirstOrDefault();

                    if (SesionComiteSolicitud.ComiteTecnico.EsComiteFiduciario != true)
                        Contratacion.ObservacionNotMapped = SesionComiteSolicitud.Observaciones;
                    else
                        Contratacion.ObservacionNotMapped = SesionComiteSolicitud.ObservacionesFiduciario;
                }

                return Contratacion;
            }
            catch (Exception)
            {
                return Contratacion;
            }
        }

        public async Task<List<Contratacion>> GetListContratacion()
        {
            List<Contratacion> ListContratacion = new List<Contratacion>();

            ListContratacion = await _context.Contratacion
                .Where(r => !(bool)r.Eliminado)
                .Include(r => r.ContratacionProyecto)
                   .ThenInclude(r => r.Proyecto)
                .ToListAsync();

            List<Dominio> ListParametricas =
                _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar
                                    || r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud).ToList();

            foreach (var Contratacion in ListContratacion)
            {
                try
                {
                    Contratacion.EsExpensa = Contratacion.ContratacionProyecto.Any(r => r.Proyecto.TipoIntervencionCodigo == ConstantCodigoTipoIntervencion.Expensas);

                    if (!string.IsNullOrEmpty(Contratacion.TipoSolicitudCodigo))
                        Contratacion.TipoSolicitudCodigo = ListParametricas.Where(r => r.Codigo == Contratacion.TipoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar).FirstOrDefault().Nombre;

                    if (!string.IsNullOrEmpty(Contratacion.EstadoSolicitudCodigo))
                        Contratacion.EstadoSolicitudNombre = ListParametricas.Where(r => r.Codigo == Contratacion.EstadoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud).FirstOrDefault().Nombre;

                    if (
                        Contratacion.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.Enviadas_a_la_Fiduciaria
                     || Contratacion.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.Firmado
                     || Contratacion.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.Registrados
                        )
                        Contratacion.RegistroCompleto = true;

                    Contratacion.ContratacionProyecto = null;
                }
                catch (Exception e)
                {

                }
            }
            return ListContratacion.OrderByDescending(r => r.ContratacionId).ToList();

        }

        public async Task<List<ContratistaGrilla>> GetListContractingByFilters(string pTipoIdentificacionCodigo, string pNumeroIdentidicacion, string pNombre, bool? EsConsorcio)
        {
            List<ContratistaGrilla> ListContratistaGrillas = new List<ContratistaGrilla>();

            List<Contratista> contratistas = _context.Contratista.Where(r => (bool)r.Activo).ToList();

            if (!string.IsNullOrEmpty(pTipoIdentificacionCodigo))
                contratistas = contratistas.Where(r => r.TipoIdentificacionCodigo.Equals(pTipoIdentificacionCodigo)).ToList();

            if (!string.IsNullOrEmpty(pNumeroIdentidicacion))
                contratistas = contratistas.Where(r => r.NumeroIdentificacion.Contains(pNumeroIdentidicacion)).ToList();

            if (!string.IsNullOrEmpty(pNombre))
                contratistas = contratistas.Where(r => r.Nombre.ToUpper().Contains(pNombre.ToUpper())).ToList();

            if ((bool)EsConsorcio)
                contratistas = contratistas.Where(r => r.TipoProponenteCodigo == ConstanCodigoTipoProponente.Persona_Juridica_Union_Temporal_o_Consorcio).ToList();
            else
                contratistas = contratistas.Where(r => r.TipoProponenteCodigo == ConstanCodigoTipoProponente.Personal_Natural || r.TipoProponenteCodigo == ConstanCodigoTipoProponente.Persona_Juridica_Individual).ToList();

            foreach (var contratista in contratistas)
            {
                ContratistaGrilla contratistaGrilla = new ContratistaGrilla
                {
                    IdContratista = contratista.ContratistaId,
                    Nombre = contratista.Nombre,
                    NumeroIdentificacion = contratista.NumeroIdentificacion == "0" ? contratista.RepresentanteLegalNumeroIdentificacion : contratista.NumeroIdentificacion,
                    NumeroInvitacion = contratista.NumeroInvitacion,
                    RepresentanteLegal = contratista.RepresentanteLegal
                };
                ListContratistaGrillas.Add(contratistaGrilla);
            }
            return ListContratistaGrillas;
        }

        public async Task<List<ProyectoGrilla>> GetListProyectsByFilters(
          string pTipoIntervencion,
          string pLlaveMen,
          string pRegion,
          string pDepartamento,
          string pMunicipio,
          int pIdInstitucionEducativa,
          int pIdSede)
        {
            //estado de registro “Completo”, que tienen viabilidad jurídica y técnica
            List<ProyectoGrilla> ListProyectoGrilla = new List<ProyectoGrilla>();
            List<Proyecto> ListProyectos = new List<Proyecto>();

            ListProyectos =
                     _context.Proyecto.Where(
                                                    r => !(bool)r.Eliminado
                                                 && r.EstadoJuridicoCodigo == ConstantCodigoEstadoJuridico.Aprobado
                                                 && (bool)r.RegistroCompleto
                                                && r.TipoIntervencionCodigo == (string.IsNullOrEmpty(pTipoIntervencion) ? r.TipoIntervencionCodigo : pTipoIntervencion)
                                                && r.LlaveMen.Contains((string.IsNullOrEmpty(pLlaveMen) ? r.LlaveMen : pLlaveMen))
                                                && r.LocalizacionIdMunicipio == (string.IsNullOrEmpty(pMunicipio) ? r.LocalizacionIdMunicipio : pMunicipio)
                                                && r.InstitucionEducativaId == (pIdInstitucionEducativa > 0 ? pIdInstitucionEducativa : r.InstitucionEducativaId)
                                                && r.SedeId == (pIdSede > 0 ? pIdSede : r.SedeId)
                                           )
                                             .Include(r => r.ContratacionProyecto)
                                                .ThenInclude(r => r.Contratacion)
                                             .Include(r => r.Sede)
                                             .Include(r => r.InstitucionEducativa)
                                             .Include(r => r.LocalizacionIdMunicipioNavigation)
                                             .Distinct()
                                                       .ToList();

            List<Localicacion> Municipios = new List<Localicacion>();

            if (!string.IsNullOrEmpty(pDepartamento) && string.IsNullOrEmpty(pRegion) && string.IsNullOrEmpty(pMunicipio))
                Municipios = await _commonService.GetListMunicipioByIdDepartamento(pDepartamento);

            if (!string.IsNullOrEmpty(pRegion) && string.IsNullOrEmpty(pDepartamento) && string.IsNullOrEmpty(pMunicipio))
            {
                List<Localizacion> Departamentos = _context.Localizacion.Where(r => r.IdPadre == pRegion).ToList();
                foreach (var dep in Departamentos)
                {
                    Municipios.AddRange(await _commonService.GetListMunicipioByIdDepartamento(dep.LocalizacionId));
                }
            }
            if (Municipios.Count() > 0)
                ListProyectos.RemoveAll(item => !Municipios.Select(r => r.LocalizacionId).Contains(item.LocalizacionIdMunicipio));

            List<Dominio> ListTipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && (bool)r.Activo).ToList();

            List<Localizacion> ListDepartamentos = _context.Localizacion.Where(r => r.Nivel == (int)ConstanCodigoTipoNivelLocalizacion.Departamento).ToList();

            List<Localizacion> ListRegiones = _context.Localizacion.Where(r => r.Nivel == (int)ConstanCodigoTipoNivelLocalizacion.Region).ToList();

            foreach (var proyecto in ListProyectos)
            {
                if (proyecto.ContratacionProyecto.Count() > 0)
                {
                    //Quitar las contrataciones que estan eliminadas 
                    proyecto.ContratacionProyecto = proyecto.ContratacionProyecto.Where(r => r.Eliminado != true).ToList();


                    //Quitar las contrataciones que estan rechazadas por comite tecnico o comite fiduciario
                    List<int> idsContratacionProyecto = proyecto.ContratacionProyecto.Where(r =>
                                                                                                 r.Contratacion.EstadoSolicitudCodigo ==  ConstanCodigoEstadoSolicitudContratacion.RechazadoComiteTecnico 
                                                                                              || r.Contratacion.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.RechazadoComiteFiduciario
                                                                                           )
                                                                                     .Select(r => r.ContratacionProyectoId)
                                                                                     .ToList();
        
                    proyecto.ContratacionProyecto = proyecto.ContratacionProyecto.Where(r => !idsContratacionProyecto.Contains(r.ContratacionProyectoId))
                                                                                  .ToList(); 
                }

                bool cumpleCondicionTai = false;

                if (!string.IsNullOrEmpty(proyecto.TipoIntervencionCodigo))
                {
                    Localizacion departamento = ListDepartamentos.Find(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipioNavigation.IdPadre);

                    string strNumeroSolicitud = string.Empty;

                    if (proyecto.ContratacionProyecto.Count() > 0)
                    {
                        foreach (var ContratacionProyecto in proyecto?.ContratacionProyecto)
                        {
                            strNumeroSolicitud += ContratacionProyecto.Contratacion.TipoSolicitudCodigo == "1" ? "Obra: " + ContratacionProyecto.Contratacion.NumeroSolicitud + " " : "Interventoria: " + ContratacionProyecto.Contratacion.NumeroSolicitud;
                        }
                    }

                    ProyectoGrilla proyectoGrilla = new ProyectoGrilla
                    {
                        TipoIntervencion = ListTipoIntervencion.Find(r => r.Codigo == proyecto.TipoIntervencionCodigo).Nombre,
                        LlaveMen = proyecto.LlaveMen,
                        Departamento = departamento.Descripcion,
                        Region = ListRegiones.Find(r => r.LocalizacionId == departamento.IdPadre).Descripcion,
                        Municipio = proyecto.LocalizacionIdMunicipioNavigation.Descripcion,
                        InstitucionEducativa = proyecto.InstitucionEducativa.Nombre,
                        Sede = proyecto.Sede.Nombre,
                        ProyectoId = proyecto.ProyectoId,
                        ContratacionId = proyecto?.ContratacionProyecto?.FirstOrDefault()?.ContratacionId,
                        NumeroSolicitud = !string.IsNullOrEmpty(strNumeroSolicitud) ? strNumeroSolicitud : "No asignado"
                    };

                    cumpleCondicionTai = ValidarCumpleTaiContratistaxProyectoId(proyecto.ProyectoId);


                    if (proyecto.EstadoProyectoObraCodigo != ConstantCodigoEstadoProyecto.Disponible 
                     && proyecto.EstadoProyectoObraCodigo != ConstantCodigoEstadoProyecto.RechazadoComiteTecnico
                     && proyecto.EstadoProyectoObraCodigo != ConstantCodigoEstadoProyecto.RechazadoComiteFiduciario
                        )
                        proyectoGrilla.TieneObra = true;

                    if (proyecto.EstadoProyectoInterventoriaCodigo != ConstantCodigoEstadoProyecto.Disponible 
                     && proyecto.EstadoProyectoInterventoriaCodigo != ConstantCodigoEstadoProyecto.RechazadoComiteTecnico  
                     && proyecto.EstadoProyectoInterventoriaCodigo != ConstantCodigoEstadoProyecto.RechazadoComiteFiduciario 
                     )
                        proyectoGrilla.TieneInterventoria = true;
                     
                    if (cumpleCondicionTai)
                    {
                        proyectoGrilla.NumeroSolicitud = "No asignado";
                        proyectoGrilla.ContratacionId = null;

                        if (proyecto?.ContratacionProyecto?.FirstOrDefault()?.Contratacion?.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria
                            )
                        {
                            proyectoGrilla.TieneInterventoria = false;

                        }
                        else
                        {
                            proyectoGrilla.TieneObra = false;
                        }
                    }

                    if (proyecto.EstadoProyectoObraCodigo == ConstantCodigoEstadoProyecto.Disponible ||
                        proyecto.EstadoProyectoObraCodigo == ConstantCodigoEstadoProyecto.RechazadoComiteTecnico ||
                        proyecto.EstadoProyectoObraCodigo == ConstantCodigoEstadoProyecto.RechazadoComiteFiduciario ||
                        proyecto.EstadoProyectoObraCodigo == ConstantCodigoEstadoProyecto.Liberado_por_comunicacion_decision_TAI_al_contratista ||
                        proyecto.EstadoProyectoInterventoriaCodigo == ConstantCodigoEstadoProyecto.Disponible ||
                        proyecto.EstadoProyectoInterventoriaCodigo == ConstantCodigoEstadoProyecto.RechazadoComiteTecnico ||
                        proyecto.EstadoProyectoInterventoriaCodigo == ConstantCodigoEstadoProyecto.RechazadoComiteFiduciario ||
                        proyecto.EstadoProyectoInterventoriaCodigo == ConstantCodigoEstadoProyecto.Liberado_por_comunicacion_decision_TAI_al_contratista ||
                        cumpleCondicionTai
                        )
                    {
                        ListProyectoGrilla.Add(proyectoGrilla);
                    }

                }
            }
            return ListProyectoGrilla.OrderByDescending(r => r.ProyectoId).ToList();
        }


        public async Task<Respuesta> CreateEditContratacionTermLimit(int contratacionId, TermLimit termLimit)
        {
            var response = new Respuesta();
            int idAccionCrearContratacionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (termLimit.PlazoDias > 30)
                    return response = new Respuesta { IsSuccessful = false, Data = null, Code = ConstantMessagesSesionComiteTema.Error, Message = "El valor ingresado en dias no puede superior a 30" };

                var contratacion = _context.Contratacion.Find(contratacionId);

                if (contratacion.PlazoContratacion != null)
                {
                    await _context.Set<PlazoContratacion>()
                         .Where(order => order.PlazoContratacionId == contratacion.PlazoContratacion.PlazoContratacionId)
                         .UpdateAsync(o => new PlazoContratacion()
                         {
                             UsuarioModificacion = termLimit.Usuario,
                             FechaModificacion = DateTime.Now,
                             PlazoDias = termLimit.PlazoDias,
                             PlazoMeses = termLimit.PlazoMeses
                         });
                }
                else
                {
                    contratacion.PlazoContratacion = new PlazoContratacion
                    {
                        UsuarioCreacion = termLimit.Usuario,
                        FechaCreacion = DateTime.Now,
                        PlazoDias = termLimit.PlazoDias,
                        PlazoMeses = termLimit.PlazoMeses
                    };

                    _context.SaveChanges();
                }


                Contratacion contratacionValidarRegistro = _context.Contratacion.Where(r => r.ContratacionId == contratacion.ContratacionId).Include(r => r.ContratacionProyecto).FirstOrDefault();
                bool blEsCompleto = ValidarEstado(contratacionValidarRegistro);
                contratacionValidarRegistro.RegistroCompleto = ValidarEstado(contratacionValidarRegistro);
                if (blEsCompleto)
                    contratacionValidarRegistro.EstadoSolicitudCodigo = ConstanCodigoEstadoSolicitudContratacion.Sin_Registro;
                contratacionValidarRegistro.UsuarioModificacion = termLimit.Usuario;
                contratacionValidarRegistro.FechaModificacion = DateTime.Now;

                _context.SaveChanges();

                response.Data = contratacion.PlazoContratacion.PlazoContratacionId;
                response.IsSuccessful = true;
                response.Code = ConstantMessagesProyecto.OperacionExitosa;
                response.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearContratacionProyecto, contratacion.UsuarioCreacion, (string.IsNullOrEmpty(contratacion.UsuarioModificacion) ? "CREAR" : "EDITAR") + " CONTRATACION PROYECTO");
                return response;

            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesProyecto.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.Error, idAccionCrearContratacionProyecto, termLimit.Usuario, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> CreateEditContratacion(Contratacion Pcontratacion)
        {
            int idAccionCrearContratacionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //Contratista 
                /* if (Pcontratacion.Contratista != null)
                     await CreateEditContratista(Pcontratacion.Contratista, true);
                     */
                //ContratacionProyecto  

                foreach (var ContratacionProyecto in Pcontratacion.ContratacionProyecto)
                {
                    ContratacionProyecto.UsuarioCreacion = Pcontratacion.UsuarioCreacion;
                    await CreateEditContratacionProyecto(ContratacionProyecto, true);

                    //ContratacionAportante
                    foreach (var ContratacionProyectoAportante in ContratacionProyecto.ContratacionProyectoAportante)
                    {
                        ContratacionProyectoAportante.UsuarioCreacion = Pcontratacion.UsuarioCreacion;
                        await CreateEditContratacionProyectoAportante(ContratacionProyectoAportante, true);

                        //ComponenteAportante
                        foreach (var ComponenteAportante in ContratacionProyectoAportante.ComponenteAportante)
                        {
                            //Componente Uso
                            foreach (var ComponenteUso in ComponenteAportante.ComponenteUso)
                            {
                                ComponenteUso.UsuarioCreacion = Pcontratacion.UsuarioCreacion;
                                await CreateEditComponenteUso(ComponenteUso, true);
                            }
                            ComponenteAportante.UsuarioCreacion = Pcontratacion.UsuarioCreacion;
                            await CreateEditComponenteAportante(ComponenteAportante, true);
                        }
                    }
                }
                //Contratacion
                if (Pcontratacion.ContratacionId == 0)
                {
                    Pcontratacion.Eliminado = false;
                    Pcontratacion.FechaCreacion = DateTime.Now;

                    Pcontratacion.FechaTramite = DateTime.Now;
                    Pcontratacion.RegistroCompleto = false;
                    Pcontratacion.NumeroSolicitud = await _commonService.EnumeradorContratacion();
                    Pcontratacion.RegistroCompleto = false;
                    _context.Contratacion.Add(Pcontratacion);
                }
                else
                {
                    Contratacion contratacionVieja = await _context.Contratacion.Where(r => r.ContratacionId == Pcontratacion.ContratacionId).Include(r => r.Contratista).FirstOrDefaultAsync();
                    contratacionVieja.UsuarioModificacion = Pcontratacion.UsuarioCreacion;
                    contratacionVieja.FechaModificacion = DateTime.Now;

                    contratacionVieja.TipoSolicitudCodigo = Pcontratacion.TipoSolicitudCodigo;
                    contratacionVieja.EsObligacionEspecial = Pcontratacion.EsObligacionEspecial;
                    contratacionVieja.ConsideracionDescripcion = Pcontratacion.ConsideracionDescripcion;
                    contratacionVieja.EstadoSolicitudCodigo = Pcontratacion.EstadoSolicitudCodigo;
                    contratacionVieja.ContratistaId = Pcontratacion.ContratistaId;
                }
                _context.SaveChanges();

                Contratacion contratacionValidarRegistro = _context.Contratacion.Where(r => r.ContratacionId == Pcontratacion.ContratacionId).Include(r => r.ContratacionProyecto).FirstOrDefault();
                contratacionValidarRegistro.RegistroCompleto = ValidarEstado(contratacionValidarRegistro);

                if ((bool)contratacionValidarRegistro.RegistroCompleto
                 && (contratacionValidarRegistro.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.DevueltaProcesoContractual
                     || contratacionValidarRegistro.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.DevueltoComiteFiduciario
                     || contratacionValidarRegistro.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.DevueltoComiteTecnico
                     ))
                    contratacionValidarRegistro.EstadoSolicitudCodigo = ConstanCodigoEstadoSolicitudContratacion.Sin_Registro;



                _context.SaveChanges();
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesProyecto.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearContratacionProyecto, Pcontratacion.UsuarioCreacion, (string.IsNullOrEmpty(Pcontratacion.UsuarioModificacion) ? "CREAR" : "EDITAR") + " CONTRATACION PROYECTO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesProyecto.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.Error, idAccionCrearContratacionProyecto, Pcontratacion.UsuarioCreacion, ex.InnerException.ToString())
                };
            }

        }

        public async Task<Respuesta> CreateEditContratista(Contratista pContratista, bool esTransaccion)
        {
            Respuesta Respuesta = new Respuesta();

            try
            {
                if (pContratista.ContratistaId == 0)
                {
                    pContratista.FechaCreacion = DateTime.Now;
                    pContratista.Activo = true;
                    _context.Contratista.Add(pContratista);
                }

                else
                {
                    Contratista contratistaOld = await _context.Contratista.FindAsync(pContratista.ContratistaId);
                    contratistaOld.UsuarioModificacion = pContratista.UsuarioCreacion;
                    contratistaOld.FechaModificacion = DateTime.Now;


                    contratistaOld.TipoIdentificacionCodigo = pContratista.TipoIdentificacionCodigo;
                    contratistaOld.NumeroIdentificacion = pContratista.NumeroInvitacion;
                    contratistaOld.Nombre = pContratista.Nombre;
                    contratistaOld.RepresentanteLegal = pContratista.RepresentanteLegal;
                    contratistaOld.NumeroInvitacion = pContratista.NumeroInvitacion;
                }
                return Respuesta;
            }
            catch (Exception ex)
            {
                Respuesta.Data = ex;
                return Respuesta;
            }
        }

        public async Task<Respuesta> CreateEditComponenteAportante(ComponenteAportante pComponenteAportante, bool esTransaccion)
        {
            Respuesta Respuesta = new Respuesta();

            try
            {
                if (pComponenteAportante.ComponenteAportanteId == 0)
                {
                    pComponenteAportante.FechaCreacion = DateTime.Now;
                    pComponenteAportante.Eliminado = false;
                    pComponenteAportante.RegistroCompleto = ValidarRegistroCompletoComponenteAportante(pComponenteAportante);
                    _context.ComponenteAportante.Add(pComponenteAportante);
                }
                else
                {
                    ComponenteAportante componenteAportanteOld = await _context.ComponenteAportante.FindAsync(pComponenteAportante.ComponenteAportanteId);
                    componenteAportanteOld.UsuarioModificacion = pComponenteAportante.UsuarioCreacion;
                    componenteAportanteOld.FechaModificacion = DateTime.Now;

                    componenteAportanteOld.TipoComponenteCodigo = pComponenteAportante.TipoComponenteCodigo;
                    componenteAportanteOld.FaseCodigo = pComponenteAportante.FaseCodigo;
                    componenteAportanteOld.RegistroCompleto = ValidarRegistroCompletoComponenteAportante(pComponenteAportante);

                }
                return Respuesta;
            }
            catch (Exception ex)
            {
                Respuesta.Data = ex;
                return Respuesta;
            }
        }

        private bool? ValidarRegistroCompletoComponenteAportante(ComponenteAportante pComponenteAportante)
        {
            bool RegistroCompletoHijo = true;

            foreach (var ComponenteUso in pComponenteAportante.ComponenteUso)
            {
                if (ComponenteUso.ValorUso == 0 || string.IsNullOrEmpty(ComponenteUso.TipoUsoCodigo))
                    return false;
            }
            if (!string.IsNullOrEmpty(pComponenteAportante.TipoComponenteCodigo) && !string.IsNullOrEmpty(pComponenteAportante.FaseCodigo) && RegistroCompletoHijo)
                return true;
            return false;

        }

        public async Task<Respuesta> CreateEditComponenteUso(ComponenteUso pComponenteUso, bool esTransaccion)
        {
            Respuesta Respuesta = new Respuesta();

            try
            {
                if (pComponenteUso.ComponenteUsoId == 0)
                {
                    pComponenteUso.FechaCreacion = DateTime.Now;
                    pComponenteUso.Eliminado = false;
                    pComponenteUso.RegistroCompleto = ValidarRegistroCompletoComponenteUso(pComponenteUso);
                    _context.ComponenteUso.Add(pComponenteUso);
                }
                else
                {
                    ComponenteUso pComponenteUsoOld = await _context.ComponenteUso.FindAsync(pComponenteUso.ComponenteUsoId);
                    pComponenteUsoOld.UsuarioModificacion = pComponenteUso.UsuarioCreacion;
                    pComponenteUsoOld.FechaModificacion = DateTime.Now;
                    pComponenteUsoOld.FuenteFinanciacionId = pComponenteUso.FuenteFinanciacionId;
                    pComponenteUsoOld.TipoUsoCodigo = pComponenteUso.TipoUsoCodigo;
                    pComponenteUsoOld.ValorUso = pComponenteUso.ValorUso;
                    pComponenteUsoOld.RegistroCompleto = ValidarRegistroCompletoComponenteUso(pComponenteUsoOld);
                }

                return Respuesta;
            }
            catch (Exception ex)
            {
                Respuesta.Data = ex;
                return Respuesta;
            }
        }

        private bool? ValidarRegistroCompletoComponenteUso(ComponenteUso pComponenteUsoOld)
        {
            if (pComponenteUsoOld.TipoUsoCodigo == null
                || pComponenteUsoOld.ValorUso == 0
                || pComponenteUsoOld.FuenteFinanciacionId == 0
                )
                return false;
            return true;
        }

        public async Task<Respuesta> CreateEditContratacionProyecto(ContratacionProyecto pContratacionProyecto, bool esTransaccion)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearContratacionContrataicionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Contratacion_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pContratacionProyecto.ContratacionProyectoId == 0)
                {
                    pContratacionProyecto.FechaCreacion = DateTime.Now;
                    pContratacionProyecto.Eliminado = false;
                    pContratacionProyecto.RegistroCompleto = ValidarRegistroCompletoContratacionProyecto(pContratacionProyecto);
                    _context.ContratacionProyecto.Add(pContratacionProyecto);
                }

                else
                {
                    ContratacionProyecto contratacionProyectoAntiguo = _context.ContratacionProyecto.Find(pContratacionProyecto.ContratacionProyectoId);
                    //Auditoria 
                    contratacionProyectoAntiguo.FechaModificacion = DateTime.Now;
                    contratacionProyectoAntiguo.UsuarioModificacion = pContratacionProyecto.UsuarioCreacion;

                    //registros
                    contratacionProyectoAntiguo.ContratacionId = pContratacionProyecto.ContratacionId;
                    contratacionProyectoAntiguo.ProyectoId = pContratacionProyecto.ProyectoId;
                    contratacionProyectoAntiguo.EsReasignacion = pContratacionProyecto.EsReasignacion;
                    contratacionProyectoAntiguo.EsAvanceobra = pContratacionProyecto.EsAvanceobra;
                    contratacionProyectoAntiguo.PorcentajeAvanceObra = pContratacionProyecto.PorcentajeAvanceObra;

                    contratacionProyectoAntiguo.RequiereLicencia = pContratacionProyecto.RequiereLicencia;
                    contratacionProyectoAntiguo.TieneMonitoreoWeb = pContratacionProyecto.TieneMonitoreoWeb;
                    contratacionProyectoAntiguo.LicenciaVigente = pContratacionProyecto.LicenciaVigente;
                    contratacionProyectoAntiguo.NumeroLicencia = pContratacionProyecto.NumeroLicencia;
                    contratacionProyectoAntiguo.FechaVigencia = pContratacionProyecto.FechaVigencia;
                    contratacionProyectoAntiguo.RegistroCompleto = ValidarRegistroCompletoContratacionProyecto(contratacionProyectoAntiguo);
                }
                if (esTransaccion)
                {
                    return respuesta;
                }
                Contratacion contratacion = _context.Contratacion
                    .Where(r => r.ContratacionId == pContratacionProyecto.ContratacionId)
                    .Include(r => r.ContratacionProyecto)
                    .FirstOrDefault();

                contratacion.RegistroCompleto = ValidarEstado(contratacion);
                contratacion.FechaModificacion = DateTime.Now;
                contratacion.UsuarioModificacion = pContratacionProyecto.UsuarioCreacion;
                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesProyecto.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearContratacionContrataicionProyecto, pContratacionProyecto.UsuarioCreacion, (string.IsNullOrEmpty(pContratacionProyecto.UsuarioModificacion) ? "CREAR" : "EDITAR") + " CONTRATACION PROYECTO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesProyecto.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesProyecto.Error, idAccionCrearContratacionContrataicionProyecto, pContratacionProyecto.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }
        /*
         * jflorez 20201127
         * edit: tiene monitoreo web es obligatorio
             */
        private bool? ValidarRegistroCompletoContratacionProyecto(ContratacionProyecto pContratacionProyectoAntiguo)
        {
            if (
                 pContratacionProyectoAntiguo.TieneMonitoreoWeb.HasValue    //Pregunta 0?
                 && pContratacionProyectoAntiguo.EsReasignacion.HasValue    //Pregunta 1
                 && pContratacionProyectoAntiguo.EsAvanceobra.HasValue      //Pregunta 2
                 && pContratacionProyectoAntiguo.RequiereLicencia.HasValue  //Pregunta 4
                 && pContratacionProyectoAntiguo.LicenciaVigente.HasValue   //Pregunta 5
              )
                return true;

            if (
                 pContratacionProyectoAntiguo.TieneMonitoreoWeb.HasValue    //Pregunta 0?
                 && pContratacionProyectoAntiguo.EsReasignacion.HasValue    //Pregunta 1
              && pContratacionProyectoAntiguo.EsAvanceobra.HasValue         //Pregunta 2
              && pContratacionProyectoAntiguo.RequiereLicencia.HasValue     //Pregunta 4
               )
                return true;


            if (
               pContratacionProyectoAntiguo.TieneMonitoreoWeb.HasValue     //Pregunta 0?
                 && pContratacionProyectoAntiguo.EsReasignacion.HasValue   //Pregunta 1
               && pContratacionProyectoAntiguo.EsAvanceobra.HasValue       //Pregunta 2
               && pContratacionProyectoAntiguo.LicenciaVigente.HasValue    //Pregunta 5
            )
                return true;

            if (
                pContratacionProyectoAntiguo.TieneMonitoreoWeb.HasValue    //Pregunta 0?
                 && pContratacionProyectoAntiguo.EsReasignacion.HasValue   //Pregunta 1 
                && pContratacionProyectoAntiguo.RequiereLicencia.HasValue  //Pregunta 4 
              )
            {
                if (pContratacionProyectoAntiguo.RequiereLicencia == false)
                    return true;
            }
            if (
                  pContratacionProyectoAntiguo.TieneMonitoreoWeb.HasValue //Pregunta 0?
               && pContratacionProyectoAntiguo.EsReasignacion.HasValue    //Pregunta 1 
               && pContratacionProyectoAntiguo.RequiereLicencia.HasValue  //Pregunta 4
               && pContratacionProyectoAntiguo.LicenciaVigente.HasValue   //Pregunta 5
            )
            {
                if (pContratacionProyectoAntiguo.LicenciaVigente == true)   //Pregunta 5
                {
                    if (pContratacionProyectoAntiguo.NumeroLicencia != null
                      && pContratacionProyectoAntiguo.FechaVigencia != null)   //Pregunta 5
                        return true;

                }
                else
                    return true;
            }
            return false;
        }

        public async Task<Respuesta> CreateEditContratacionProyectoAportanteByContratacionproyecto(ContratacionProyecto pContratacionProyecto, bool esTransaccion)
        {
            int idAccionCrearContratacionContrataicionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Contratacion_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pContratacionProyecto.ContratacionId > 0)
                {
                    ContratacionProyecto contratacionProyectoOld = _context.ContratacionProyecto.Find(pContratacionProyecto.ContratacionProyectoId);
                    contratacionProyectoOld.UsuarioModificacion = pContratacionProyecto.UsuarioCreacion;
                    contratacionProyectoOld.FechaModificacion = DateTime.Now;
                    contratacionProyectoOld.RegistroValido = pContratacionProyecto.RegistroValido;
                }

                foreach (var ContratacionProyectoAportante in pContratacionProyecto.ContratacionProyectoAportante)
                {
                    ContratacionProyectoAportante.UsuarioCreacion = pContratacionProyecto.UsuarioCreacion;
                    await CreateEditContratacionProyectoAportante(ContratacionProyectoAportante, true);

                    foreach (var ComponenteAportante in ContratacionProyectoAportante.ComponenteAportante)
                    {
                        //Componente Uso
                        foreach (var ComponenteUso in ComponenteAportante.ComponenteUso)
                        {
                            ComponenteUso.UsuarioCreacion = pContratacionProyecto.UsuarioCreacion;
                            await CreateEditComponenteUso(ComponenteUso, true);
                        }
                        //ComponenteAportante
                        ComponenteAportante.UsuarioCreacion = pContratacionProyecto.UsuarioCreacion;
                        await CreateEditComponenteAportante(ComponenteAportante, true);
                    }
                }

                //Validar Registro Completo
                Contratacion contratacion = _context.Contratacion
                       .Where(r => r.ContratacionId == pContratacionProyecto.ContratacionId)
                      .Include(r => r.ContratacionProyecto)
                        .ThenInclude(r => r.ContratacionProyectoAportante)
                            .ThenInclude(r => r.ComponenteAportante)
                                .ThenInclude(r => r.ComponenteUso).FirstOrDefault();

                contratacion.RegistroCompleto = ValidarEstado(contratacion);
                contratacion.UsuarioModificacion = pContratacionProyecto.UsuarioCreacion;
                contratacion.FechaModificacion = DateTime.Now;
                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesProyecto.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearContratacionContrataicionProyecto, pContratacionProyecto.UsuarioCreacion, "CREAR CONTRATACION PROYECTO APORTANTE")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesProyecto.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesProyecto.Error, idAccionCrearContratacionContrataicionProyecto, pContratacionProyecto.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        public async Task<Respuesta> CreateEditContratacionProyectoAportante(ContratacionProyectoAportante pContratacionProyectoAportante, bool esTransaccion)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearContratacionContrataicionProyectoAportante = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Contratacion_Proyecto_Aportante, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                string strAccion;
                if (pContratacionProyectoAportante.ContratacionProyectoAportanteId == 0)
                {
                    strAccion = "CREAR CONTRATACION PROYECTO APORTANTE";

                    //Auditoria No guardo usuario Creacion Porque ya viene desde el controller

                    pContratacionProyectoAportante.FechaCreacion = DateTime.Now;
                    pContratacionProyectoAportante.Eliminado = false;

                    _context.ContratacionProyectoAportante.Add(pContratacionProyectoAportante);

                }

                else
                {
                    strAccion = "EDITAR CONTRATACION  PROYECTO APORTANTE";

                    ContratacionProyectoAportante contratacionProyectoAportanteAntiguo = _context.ContratacionProyectoAportante.Find(pContratacionProyectoAportante.ContratacionProyectoAportanteId);
                    //Auditoria 
                    contratacionProyectoAportanteAntiguo.FechaModificacion = DateTime.Now;
                    contratacionProyectoAportanteAntiguo.UsuarioModificacion = contratacionProyectoAportanteAntiguo.UsuarioCreacion;

                    //registros
                    contratacionProyectoAportanteAntiguo.ContratacionProyectoId = pContratacionProyectoAportante.ContratacionProyectoId;
                    contratacionProyectoAportanteAntiguo.ContratacionProyectoAportanteId = pContratacionProyectoAportante.ContratacionProyectoAportanteId;
                    contratacionProyectoAportanteAntiguo.ValorAporte = pContratacionProyectoAportante.ValorAporte;

                }

                if (esTransaccion)
                {
                    return respuesta;
                }


                _context.SaveChanges();
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesProyecto.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearContratacionContrataicionProyectoAportante, pContratacionProyectoAportante.UsuarioCreacion, strAccion)
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesProyecto.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesProyecto.Error, idAccionCrearContratacionContrataicionProyectoAportante, pContratacionProyectoAportante.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        public bool ValidarEstado(Contratacion contratacion)
        {
            if (contratacion is null)
                return false;
            if (string.IsNullOrEmpty(contratacion.TipoSolicitudCodigo)
             || string.IsNullOrEmpty(contratacion.NumeroSolicitud)
             || string.IsNullOrEmpty(contratacion.EstadoSolicitudCodigo)
             || contratacion.ContratacionId == 0
             || !contratacion.EsObligacionEspecial.HasValue
             || contratacion.ContratistaId == null)
                return false;

            // (contratacion.PlazoContratacion == null || (contratacion.PlazoContratacion.PlazoDias == 0 && contratacion.PlazoContratacion.PlazoMeses == 0)

            PlazoContratacion plazoContratacion = _context.PlazoContratacion.Where(r => r.PlazoContratacionId == contratacion.PlazoContratacionId).FirstOrDefault();

            if (plazoContratacion is null)
                return false;

            if (plazoContratacion.PlazoDias + plazoContratacion.PlazoMeses == 0)
                return false;

            foreach (var ContratacionProyecto in contratacion.ContratacionProyecto)
            {
                if (ContratacionProyecto.RegistroValido != true)
                    return false;

                if (!ContratacionProyecto.RegistroCompleto.HasValue)
                    return false;
                else if (!(bool)ContratacionProyecto.RegistroCompleto)
                    return false;
            }
            foreach (var ContratacionProyecto in contratacion.ContratacionProyecto)
            {
                foreach (var ContratacionProyectoAportante in ContratacionProyecto.ContratacionProyectoAportante)
                {
                    if (ContratacionProyectoAportante.ComponenteAportante.Count() == 0)
                        return false;

                    foreach (var ComponenteAportante in ContratacionProyectoAportante.ComponenteAportante.Where(r => r.Eliminado != true))
                    {
                        if (
                              string.IsNullOrEmpty(ComponenteAportante.TipoComponenteCodigo)
                           || string.IsNullOrEmpty(ComponenteAportante.FaseCodigo))
                            return false;

                        foreach (var ComponenteUso in ComponenteAportante.ComponenteUso)
                        {
                            if (ComponenteUso.TipoUsoCodigo == null || string.IsNullOrEmpty(ComponenteUso.TipoUsoCodigo.ToString()) || ComponenteUso.ValorUso == 0)
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        public async Task<Contratacion> CreateContratacion(Contratacion pContratacion, string pUsuarioCreacion)
        {
            try
            {
                Contratacion contratacion = new Contratacion
                {
                    //Auditoria
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacion = pUsuarioCreacion,
                    Eliminado = false,

                    //Registros 
                    TipoSolicitudCodigo = pContratacion.TipoSolicitudCodigo,
                    FechaTramite = DateTime.Now,
                    NumeroSolicitud = await _commonService.EnumeradorContratacion(),
                    EstadoSolicitudCodigo = ConstanCodigoEstadoSolicitudContratacion.Sin_Registro,

                };
                contratacion.RegistroCompleto = ValidarEstado(contratacion);

                foreach (ContratacionProyecto c in pContratacion.ContratacionProyecto)
                {
                    //Crear contratacionProyecto
                    ContratacionProyecto contratacionProyecto = new ContratacionProyecto
                    {
                        //Auditoria
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = pUsuarioCreacion,
                        Eliminado = false,
                        Activo = true,
                        //Registros
                        ContratacionId = contratacion.ContratacionId,
                        ProyectoId = c.ProyectoId,
                    };

                    //Se cambia el estado del proyecto cuando se asigna a una contratación

                    Proyecto proyectoCambiarEstado = _context.Proyecto.Find(c.ProyectoId);
                    if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                        proyectoCambiarEstado.EstadoProyectoInterventoriaCodigo = ConstantCodigoEstadoProyecto.AsignadoSolicitudContratacion;
                    if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                        proyectoCambiarEstado.EstadoProyectoObraCodigo = ConstantCodigoEstadoProyecto.AsignadoSolicitudContratacion;


                    proyectoCambiarEstado.FechaModificacion = DateTime.Now;
                    proyectoCambiarEstado.UsuarioModificacion = pUsuarioCreacion;

                    List<ProyectoAportante> listaAportantes = _context.ProyectoAportante.Where(a => !(bool)a.Eliminado && a.ProyectoId == c.ProyectoId).ToList();

                    listaAportantes.ForEach(apo =>
                    {
                        ContratacionProyectoAportante contratacionProyectoAportante = new ContratacionProyectoAportante
                        {
                            CofinanciacionAportanteId = apo.AportanteId,
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = pUsuarioCreacion
                        };
                        contratacionProyecto.ContratacionProyectoAportante.Add(contratacionProyectoAportante);
                    });
                    contratacion.ContratacionProyecto.Add(contratacionProyecto);
                }
                _context.Contratacion.Add(contratacion);
                await _context.SaveChangesAsync();

                return contratacion;
            }
            catch (Exception)
            {
                return new Contratacion();
            }

        }

        public async Task<Respuesta> CreateContratacionProyecto(Contratacion pContratacion, string usuarioCreacion)
        {
            int idAccionCrearContratacionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pContratacion.TipoSolicitudCodigo != ConstanCodigoTipoContratacion.Obra_Interventoria.ToString())
                {
                    Contratacion contratacion = await CreateContratacion(pContratacion, usuarioCreacion);

                    return new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesContratacionProyecto.OperacionExitosa,
                        Data = contratacion,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesContratacionProyecto.OperacionExitosa, idAccionCrearContratacionProyecto, usuarioCreacion, "CREAR CONTRATACION PROYECTO")
                    };

                }
                else
                {
                    //Si se seleccionan obra o interventoria se crean Dos solicitudes
                    pContratacion.TipoSolicitudCodigo = ConstanCodigoTipoContratacion.Obra.ToString();
                    Contratacion contratacionObra = await CreateContratacion(pContratacion, usuarioCreacion);

                    pContratacion.TipoSolicitudCodigo = ConstanCodigoTipoContratacion.Interventoria.ToString();
                    Contratacion contratacionInterventoria = await CreateContratacion(pContratacion, usuarioCreacion);


                    return
                              new Respuesta
                              {
                                  IsSuccessful = true,
                                  IsException = false,
                                  IsValidation = true,
                                  Code = ConstantMessagesContratacionProyecto.OperacionExitosa,
                                  Data = contratacionInterventoria,
                                  Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesContratacionProyecto.OperacionExitosa, idAccionCrearContratacionProyecto, usuarioCreacion, "CREAR CONTRATACION PROYECTO")
                              };
                }
            }
            catch (Exception ex)
            {
                return
                         new Respuesta
                         {
                             IsSuccessful = false,
                             IsException = true,
                             IsValidation = false,
                             Code = ConstantMessagesProyecto.Error,
                             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.Error, idAccionCrearContratacionProyecto, usuarioCreacion, ex.InnerException.ToString())
                         };
            }
        }

        public async Task<List<FaseComponenteUso>> GetListFaseComponenteUso()
        {
            return _context.FaseComponenteUso.ToList();
        }

        private bool ValidarCumpleTaiContratistaxProyectoId(int pProyectoId)
        {
            //Nueva restricción control de cambios
            bool cumpleCondicionesTai = false;
            ContratacionProyecto cp = _context.ContratacionProyecto.Where(r => r.ProyectoId == pProyectoId && r.Eliminado != true).FirstOrDefault();
            if (cp != null)
            {
                Contrato contrato = _context.Contrato.Where(r => r.ContratacionId == cp.ContratacionId).FirstOrDefault();
                if (contrato != null)
                {
                    List<ControversiaContractual> controversiaContractualList = _context.ControversiaContractual.Where(r => r.ContratoId == contrato.ContratoId && r.Eliminado != true).ToList();
                    foreach (var controversiaContractual in controversiaContractualList)
                    {
                        SesionComiteSolicitud sesionComiteSolicitud = _context.SesionComiteSolicitud
                                    .Where(r => r.SolicitudId == controversiaContractual.ControversiaContractualId && r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.ControversiasContractuales && (r.Eliminado == false || r.Eliminado == null))
                                    .FirstOrDefault();

                        if (sesionComiteSolicitud != null)
                        {
                            if (sesionComiteSolicitud.ComiteTecnicoFiduciarioId != null)
                            {
                                ComiteTecnico comiteFiduciario = _context.ComiteTecnico.Find(sesionComiteSolicitud.ComiteTecnicoFiduciarioId);

                                if (comiteFiduciario != null)
                                {
                                    //cumple la primera condición de ser aprobada por el comite fiduciario
                                    if (comiteFiduciario.EstadoComiteCodigo == ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Aprobada)
                                    {
                                        //validar que esa controversia tenga actuaciones finalizadas y con actuación "Comunicacion_decision_TAI_al_contratista"
                                        ControversiaActuacion controversiaActuacion = _context.ControversiaActuacion.Where(
                                            r => r.ControversiaContractualId == controversiaContractual.ControversiaContractualId
                                            && r.Eliminado != true
                                            && r.EstadoCodigo == ConstantCodigoEstadoControversiaActuacion.Finalizada
                                            && r.ActuacionAdelantadaCodigo == ConstanCodigoActuacionAdelantada.Comunicacion_decision_TAI_al_contratista
                                            && r.FechaActuacion < DateTime.Now).FirstOrDefault();

                                        if (controversiaActuacion != null)
                                        {
                                            cumpleCondicionesTai = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            return cumpleCondicionesTai;
        }

    }
}
