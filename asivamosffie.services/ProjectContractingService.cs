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
using Microsoft.AspNetCore.StaticFiles;

namespace asivamosffie.services
{
    public class ProjectContractingService : IProjectContractingService
    {

        private readonly ICommonService _commonService;
        private readonly IProjectService _projectService;
        private readonly devAsiVamosFFIEContext _context;

        public ProjectContractingService(devAsiVamosFFIEContext context, ICommonService commonService, IProjectService projectService)
        {
            _commonService = commonService;
            _projectService = projectService;
            _context = context;
        }

        public async Task<Respuesta> ChangeStateContratacionByIdContratacion(int idContratacion, string PCodigoEstado, string pUsusarioModifico
            , string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender
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

                List<Dominio> TipoObraIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar).ToList();
                string strTipoObraIntervencion = TipoObraIntervencion.Where(r => r.Codigo == contratacionOld.TipoSolicitudCodigo).Select(r => r.Nombre).FirstOrDefault();

                if (PCodigoEstado == ConstanCodigoEstadoSolicitudContratacion.En_tramite.ToString())
                {
                    var usuariosecretario = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Secretario_Comite).Select(x => x.Usuario.Email).ToList();
                    foreach (var usuario in usuariosecretario)
                    {
                        Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjEnviarSolicitudContratacion);
                        string template =
                            TemplateRecoveryPassword.Contenido
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesContratacionProyecto.OperacionExitosa, idAccionEliminarContratacionProyecto, pUsusarioModifico, "CAMBIAR ESTADO CONTRATACION")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesContratacionProyecto.Error, idAccionEliminarContratacionProyecto, pUsusarioModifico, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        public async Task<Respuesta> DeleteContratacionByIdContratacion(int idContratacion, string pUsusarioElimino)
        {
            int idAccionEliminarContratacionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Contratacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contratacion contratacionOld = _context.Contratacion.Where(r => r.ContratacionId == idContratacion)
                    .Include(r => r.ContratacionProyecto).FirstOrDefault();

                foreach (var ContratacionProyecto in contratacionOld.ContratacionProyecto)
                {
                    //Eliminar Relacion ContratacionProyecto cuando se elimina
                    ContratacionProyecto contratacionProyectoEliminar = _context.ContratacionProyecto.Find(ContratacionProyecto.ContratacionProyectoId);
                    contratacionProyectoEliminar.Eliminado = true;
                    contratacionProyectoEliminar.UsuarioModificacion = pUsusarioElimino;
                    contratacionProyectoEliminar.FechaModificacion = DateTime.Now;


                    //Cambiar estado Proyecto cuando se elimina
                    Proyecto proyectoCambiarEstadoEliminado = _context.Proyecto.Find(ContratacionProyecto.ProyectoId);
                    proyectoCambiarEstadoEliminado.UsuarioModificacion = pUsusarioElimino;
                    proyectoCambiarEstadoEliminado.FechaModificacion = DateTime.Now;

                    proyectoCambiarEstadoEliminado.EstadoProyectoCodigo = ConstantCodigoEstadoProyecto.Disponible;
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
            try{
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
                 .Include(r => r.ContratacionProyecto)
                 .ThenInclude(r => r.ContratacionProyectoAportante)
                    .ThenInclude(r => r.ComponenteAportante)
                        .ThenInclude(r => r.ComponenteUso).Where(r => !(bool)r.Eliminado)
              .FirstOrDefaultAsync();
            }catch(Exception ex ){
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
                   .ThenInclude(r => r.ProyectoAportante)
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
                           .ThenInclude(r => r.ComponenteUso).FirstOrDefaultAsync();

            contratacion.ContratacionProyecto = contratacion.ContratacionProyecto.Where(r => !(bool)r.Eliminado).ToList();

            List<Dominio> ListTipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && (bool)r.Activo).ToList();
            List<Localizacion> ListDepartamentos = _context.Localizacion.Where(r => r.Nivel == (int)ConstanCodigoTipoNivelLocalizacion.Departamento).ToList();
            List<Localizacion> ListRegiones = _context.Localizacion.Where(r => r.Nivel == (int)ConstanCodigoTipoNivelLocalizacion.Region).ToList();

            List<Dominio> ListFases = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Fases && (bool)r.Activo).ToList();

            foreach (var item in contratacion.ContratacionProyecto)
            {
                if (!(bool)item.Proyecto.Eliminado)
                {
                    Proyecto proyectoTem = await _context.Proyecto.Where(r => r.ProyectoId == item.Proyecto.ProyectoId)
                           .Include(r => r.Sede)
                           .Include(r => r.InstitucionEducativa)
                           .Include(r => r.LocalizacionIdMunicipioNavigation).FirstOrDefaultAsync();

                    Localizacion departamento = ListDepartamentos.Find(r => r.LocalizacionId == proyectoTem.LocalizacionIdMunicipioNavigation.IdPadre);

                    item.Proyecto.LocalizacionIdMunicipio = item.Proyecto.LocalizacionIdMunicipioNavigation.Descripcion + " / " + departamento.Descripcion;
                    item.Proyecto.UsuarioModificacion = ListRegiones.Find(r => r.LocalizacionId == departamento.IdPadre).Descripcion;
                    item.Proyecto.TipoIntervencionCodigo = ListTipoIntervencion.Find(r => r.Codigo == item.Proyecto.TipoIntervencionCodigo).Nombre;
                }
                foreach(var praportante in item.ContratacionProyectoAportante)
                {                    
                    if (praportante.CofinanciacionAportante.TipoAportanteId == ConstanTipoAportante.Ffie)
                    {
                        praportante.CofinanciacionAportante.NombreAportanteString = ConstanStringTipoAportante.Ffie;
                    }
                    else if (praportante.CofinanciacionAportante.TipoAportanteId == ConstanTipoAportante.ET)
                    {
                        //verifico si tiene municipio
                        if (praportante.CofinanciacionAportante.MunicipioId != null)
                        {
                            praportante.CofinanciacionAportante.NombreAportanteString = _context.Localizacion.Find(praportante.CofinanciacionAportante.MunicipioId).Descripcion;
                        }
                        else//solo departamento
                        {
                            praportante.CofinanciacionAportante.NombreAportanteString = praportante.CofinanciacionAportante.DepartamentoId == null ? "Error" : _context.Localizacion.Find(praportante.CofinanciacionAportante.DepartamentoId).Descripcion;
                        }
                    }
                    else
                    {
                        praportante.CofinanciacionAportante.NombreAportanteString = _context.Dominio.Find(praportante.CofinanciacionAportante.NombreAportanteId).Nombre;
                    }                     
                    praportante.CofinanciacionAportante.TipoAportanteString = _context.Dominio.Find(praportante.CofinanciacionAportante.TipoAportanteId).Nombre;

                }
                DateTime? fechaComitetecnico = null;
                string numerocomietetecnico = "";
                var comite= _context.SesionComiteSolicitud.Where(x => x.SolicitudId == contratacion.ContratacionId && x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion).
                    Include(x => x.ComiteTecnico).ToList();
                if (comite.Count() > 0)
                {
                    fechaComitetecnico = Convert.ToDateTime(comite.FirstOrDefault().ComiteTecnico.FechaOrdenDia);
                }

                contratacion.FechaComiteTecnicoNotMapped = fechaComitetecnico;
            }

            return contratacion;
        }

        public async Task<Contratacion> GetContratacionByContratacionIdWithGrillaProyecto(int pContratacionId)
        {
            Contratacion contratacion = await _context.Contratacion.Where(r => r.ContratacionId == pContratacionId)
                .Include(r => r.ContratacionProyecto)
                   .ThenInclude(r => r.SesionSolicitudObservacionProyecto)
                .Include(r => r.ContratacionProyecto)
                    .ThenInclude(r => r.ContratacionObservacion)
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
                .Include(r => r.Proyecto)
                     .ThenInclude(r => r.ProyectoAportante)
                         .ThenInclude(r => r.Aportante)
                           .ThenInclude(r => r.Departamento)
                 .Include(r => r.Proyecto)
                
                 //Tipo ET MUNICIPIO
                     .ThenInclude(r => r.ProyectoAportante)
                         .ThenInclude(r => r.Aportante)
                           .ThenInclude(r => r.Municipio)
                
                 //Tipo TERCERO DOMINIO
                .Include(r => r.Proyecto)
                     .ThenInclude(r => r.ProyectoAportante)
                         .ThenInclude(r => r.Aportante)
                             .ThenInclude(r => r.NombreAportante)


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
                .FirstOrDefaultAsync();

            foreach (var ContratacionProyectoAportante in contratacionProyecto.ContratacionProyectoAportante)
            {

                decimal ValorGastado = 0;
                decimal ValorDisponible = (decimal)ContratacionProyectoAportante.CofinanciacionAportante.FuenteFinanciacion.Select(r => r.ValorFuente).Sum();

                foreach (var ComponenteAportante in ContratacionProyectoAportante.ComponenteAportante)
                {
                    ValorGastado = ComponenteAportante.ComponenteUso.Select(r => r.ValorUso).Sum();
                }

                ContratacionProyectoAportante.SaldoDisponible = ValorDisponible - ValorGastado;

            }

            return contratacionProyecto;
        }

        public async Task<List<Contratacion>> GetListContratacion()
        {
            List<Contratacion> ListContratacion = new List<Contratacion>();

            try
            {
                ListContratacion = await _context.Contratacion
                    .Where(r => !(bool)r.Eliminado)
                    //.Include(r => r.Contratista)
                    //.Include(r => r.ContratacionProyecto)
                    //    .ThenInclude(r => r.SesionSolicitudObservacionProyecto)
                    // .Include(r => r.ContratacionProyecto)
                    //    .ThenInclude(r => r.Proyecto)
                    //         .ThenInclude(r => r.Sede)
                    //.Include(r => r.ContratacionProyecto)
                    //    .ThenInclude(r => r.Proyecto)
                    //             .ThenInclude(r => r.InstitucionEducativa)
                    .ToListAsync();

                List<Dominio> ListParametricas = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar || r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud).ToList();

                foreach (var Contratacion in ListContratacion)
                {
                    if (!string.IsNullOrEmpty(Contratacion.TipoSolicitudCodigo))
                        Contratacion.TipoSolicitudCodigo = ListParametricas.Where(r => r.Codigo == Contratacion.TipoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar).FirstOrDefault().Nombre;

                    if (!string.IsNullOrEmpty(Contratacion.EstadoSolicitudCodigo))
                        Contratacion.EstadoSolicitudCodigo = ListParametricas.Where(r => r.Codigo == Contratacion.EstadoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud).FirstOrDefault().Nombre;
                }
                return ListContratacion.OrderByDescending(r => r.ContratacionId).ToList();
            }
            catch (Exception ex)
            {
                return ListContratacion;
            }
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
                    NumeroIdentificacion = contratista.NumeroIdentificacion,
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
            //Listar Los proyecto segun caso de uso solo trae los ue estado
            //estado de registro “Completo”, que tienen viabilidad jurídica y técnica
            List<ProyectoGrilla> ListProyectoGrilla = new List<ProyectoGrilla>();
            List<Proyecto> ListProyectos = new List<Proyecto>();
            try
            {

                ListProyectos =
                     _context.Proyecto.Where(
                         r => !(bool)r.Eliminado &&
                         r.EstadoJuridicoCodigo == ConstantCodigoEstadoJuridico.Aprobado
                         &&
                         (bool)r.RegistroCompleto &&
                         //Se quitan los proyectos que ya esten vinculados a una contratacion 
                         r.TipoIntervencionCodigo == (string.IsNullOrEmpty(pTipoIntervencion) ? r.TipoIntervencionCodigo : pTipoIntervencion) &&
                         r.LlaveMen == (string.IsNullOrEmpty(pLlaveMen) ? r.LlaveMen : pLlaveMen) &&
                         r.LocalizacionIdMunicipio == (string.IsNullOrEmpty(pMunicipio) ? r.LocalizacionIdMunicipio : pMunicipio) &&
                         r.InstitucionEducativaId == (pIdInstitucionEducativa > 0 ? pIdInstitucionEducativa : r.InstitucionEducativaId) &&
                         r.SedeId == (pIdSede > 0 ? pIdSede : r.SedeId)
                         )
                                 .Include(r => r.ContratacionProyecto)
                                   .ThenInclude(r => r.Contratacion)
                                 .Include(r => r.Sede)
                                 .Include(r => r.InstitucionEducativa)
                                 .Include(r => r.LocalizacionIdMunicipioNavigation).Distinct().ToList();

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

                List<Proyecto> ListaProyectosRemover = new List<Proyecto>();
                foreach (var Proyecto in ListProyectos)
                {
                    foreach (var ContratacionProyecto in Proyecto.ContratacionProyecto.Where(r => !(bool)r.Eliminado))
                    {
                        if (ContratacionProyecto.Contratacion.EstadoSolicitudCodigo != ConstanCodigoEstadoSolicitudContratacion.Rechazado)
                            ListaProyectosRemover.Add(Proyecto);
                        else
                        {
                            if (Proyecto.ContratacionProyecto.Where(r => r.ProyectoId == Proyecto.ProyectoId).Count() > 1)
                                ListaProyectosRemover.Add(Proyecto);
                        }
                    }
                }
                foreach (var proyecto in ListaProyectosRemover.Distinct())
                {
                    ListProyectos.Remove(proyecto);
                }

                List<Dominio> ListTipoSolicitud = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Tipo_de_Solicitud_Obra_Interventorias);

                List<Dominio> ListTipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && (bool)r.Activo).ToList();

                List<Localizacion> ListDepartamentos = _context.Localizacion.Where(r => r.Nivel == (int)ConstanCodigoTipoNivelLocalizacion.Departamento).ToList();

                List<Localizacion> ListRegiones = _context.Localizacion.Where(r => r.Nivel == (int)ConstanCodigoTipoNivelLocalizacion.Region).ToList();

                List<Contratacion> ListContratacion = _context.Contratacion.Where(r => !(bool)r.Eliminado).ToList();

                foreach (var proyecto in ListProyectos)
                {
                    if (!string.IsNullOrEmpty(proyecto.TipoIntervencionCodigo))
                    {
                        Localizacion departamento = ListDepartamentos.Find(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipioNavigation.IdPadre);

                        try
                        {
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
                            };
                            foreach (var item in proyecto.ContratacionProyecto.Where(r => !(bool)r.Eliminado))
                            {
                                item.Contratacion = ListContratacion.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefault();
                                if (!string.IsNullOrEmpty(item.Contratacion.TipoSolicitudCodigo))
                                {
                                    if (item.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                                        proyectoGrilla.TieneObra = true;

                                    if (item.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                                        proyectoGrilla.TieneInterventoria = true;
                                }
                            }
                            ListProyectoGrilla.Add(proyectoGrilla);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return ListProyectoGrilla.OrderByDescending(r => r.ProyectoId).ToList();
            }
            return ListProyectoGrilla.OrderByDescending(r => r.ProyectoId).ToList();
        }

        public async Task<Respuesta> CreateEditContratacion(Contratacion Pcontratacion)
        {
            int idAccionCrearContratacionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            try
            {

                //Contratista 
                if (Pcontratacion.Contratista != null)
                    await CreateEditContratista(Pcontratacion.Contratista, true);

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
            if (pComponenteUsoOld.TipoUsoCodigo == null && pComponenteUsoOld.ValorUso == 0)
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
            {
                return true;
            }

            if (
                 pContratacionProyectoAntiguo.TieneMonitoreoWeb.HasValue    //Pregunta 0?
                 && pContratacionProyectoAntiguo.EsReasignacion.HasValue     //Pregunta 1
              && pContratacionProyectoAntiguo.EsAvanceobra.HasValue       //Pregunta 2
              && pContratacionProyectoAntiguo.RequiereLicencia.HasValue   //Pregunta 4
               )
            {
                return true;
            }

            if (
               pContratacionProyectoAntiguo.TieneMonitoreoWeb.HasValue    //Pregunta 0?
                 && pContratacionProyectoAntiguo.EsReasignacion.HasValue     //Pregunta 1
               && pContratacionProyectoAntiguo.EsAvanceobra.HasValue       //Pregunta 2
               && pContratacionProyectoAntiguo.LicenciaVigente.HasValue    //Pregunta 5
            )
            {
                return true;
            }
            if (
                pContratacionProyectoAntiguo.TieneMonitoreoWeb.HasValue    //Pregunta 0?
                 && pContratacionProyectoAntiguo.EsReasignacion.HasValue    //Pregunta 1 
                && pContratacionProyectoAntiguo.RequiereLicencia.HasValue  //Pregunta 4 
              )
            {
                return true;
            }


            if (
                  pContratacionProyectoAntiguo.TieneMonitoreoWeb.HasValue    //Pregunta 0?
               && pContratacionProyectoAntiguo.EsReasignacion.HasValue    //Pregunta 1 
               && pContratacionProyectoAntiguo.RequiereLicencia.HasValue  //Pregunta 4
               && pContratacionProyectoAntiguo.LicenciaVigente.HasValue   //Pregunta 5
            )
            {
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
                    pContratacionProyecto.UsuarioModificacion = pContratacionProyecto.UsuarioCreacion;
                    pContratacionProyecto.FechaModificacion = DateTime.Now;
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

        public static bool ValidarEstado(Contratacion contratacion)
        {
            if (string.IsNullOrEmpty(contratacion.TipoSolicitudCodigo)
             || string.IsNullOrEmpty(contratacion.NumeroSolicitud)
             || string.IsNullOrEmpty(contratacion.EstadoSolicitudCodigo)
             || string.IsNullOrEmpty(contratacion.ContratacionId.ToString())
             || !contratacion.EsObligacionEspecial.HasValue
             || contratacion.ContratistaId == null
             )
                return false;
            else
            {
                foreach (var ContratacionProyecto in contratacion.ContratacionProyecto)
                {
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

                        foreach (var ComponenteAportante in ContratacionProyectoAportante.ComponenteAportante)
                        {
                            if (!(bool)ComponenteAportante.Eliminado)
                            {
                                if (string.IsNullOrEmpty(ComponenteAportante.TipoComponenteCodigo)
                                   || string.IsNullOrEmpty(ComponenteAportante.FaseCodigo))
                                    return false;
                                foreach (var ComponenteUso in ComponenteAportante.ComponenteUso)
                                {
                                    if (string.IsNullOrEmpty(ComponenteUso.TipoUsoCodigo.ToString()) || ComponenteUso.ValorUso == 0)
                                        return false;
                                }
                            }
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
                    proyectoCambiarEstado.EstadoProyectoCodigo = ConstantCodigoEstadoProyecto.AsignadoSolicitudContratacion;
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
            Respuesta respuesta = new Respuesta();
            int idAccionCrearContratacionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proyecto, (int)EnumeratorTipoDominio.Acciones);


            try
            {

                if (pContratacion.TipoSolicitudCodigo != ConstanCodigoTipoContratacion.Obra_Interventoria.ToString())
                {
                    Contratacion contratacion = await CreateContratacion(pContratacion, usuarioCreacion);

                    return respuesta =
                     new Respuesta
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
                    //Si se seleccionan obra o interventoria se creas nos solicitudes
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

    }
}
