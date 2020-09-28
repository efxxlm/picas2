using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using DinkToPdf;
using System.IO;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{

    /*
       PARAMETRICAS

     Tipo tema = 1. Solicitudes contractuales, 2. Tema nuevo
     TipoDominioId = 42
    ------------------------------------

     MiembrosComiteTecnico = 1.	Dirección técnica, 2.	Dirección financiera, 3.	Dirección jurídica, 4.	Fiduciaria, 5.	Dirección administrativa
     TipoDominioId = 46
    ------------------------------------

    */

    public class CommitteeSessionFiduciarioService : ICommitteeSessionFiduciarioService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IProjectContractingService _IProjectContractingService;
        public readonly IConverter _converter;

        public CommitteeSessionFiduciarioService(devAsiVamosFFIEContext context, ICommonService commonService, IProjectContractingService projectContractingService, IConverter converter)
        {
            _IProjectContractingService = projectContractingService;
            _context = context;
            _commonService = commonService;
            _converter = converter;
        }


        #region "ORDEN DEL DIA";

        //todas las solicitudes que fueron aprobadas por el comite tecnico.
        //TipoDominioId = 38, Codigo = 2, Nombre = Convocada
        public async Task<List<dynamic>> GetCommitteeSessionFiduciario()
        {
            List<dynamic> listaSolicitudesGrilla = new List<dynamic>();
            List<dynamic> listaComitesGrilla = new List<dynamic>();

            try
            {
                List<ComiteTecnico> listaComites = await _context.ComiteTecnico.Where(ct => ct.EsComiteFiduciario == null || ct.EsComiteFiduciario == false)
                                                                                .Include(r => r.SesionComiteSolicitudComiteTecnico)
                                                                                .ToListAsync(); //Aprobadas

                List<Dominio> ListTipoSolicitud = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud).ToList();

                listaComites.ForEach(c =>
                {
                    dynamic comite = new { nombreSesion = c.NumeroComite, fecha = c.FechaOrdenDia, data = new List<dynamic>() };
                    foreach (var ss in c.SesionComiteSolicitudComiteTecnico)
                    {
                        if (ss.EstadoCodigo == "1")
                            switch (ss.TipoSolicitudCodigo)
                            {
                                case ConstanCodigoTipoSolicitud.Contratacion:
                                    {
                                        Contratacion contratacion = _context.Contratacion.Find(ss.SolicitudId);

                                        if (contratacion != null)
                                            comite.data.Add(new
                                            {
                                                Id = contratacion.ContratacionId,
                                                IdSolicitud = ss.SesionComiteSolicitudId,
                                                FechaSolicitud = contratacion.FechaTramite.HasValue ? (DateTime?)contratacion.FechaTramite.Value : null,
                                                NumeroSolicitud = contratacion.NumeroSolicitud,
                                                TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion).FirstOrDefault().Nombre,
                                                tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Contratacion
                                            });
                                        break;
                                    }

                                case ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion:
                                    {
                                        ProcesoSeleccion procesoSeleccion = _context.ProcesoSeleccion.Find(ss.SolicitudId);

                                        if (procesoSeleccion != null)
                                            comite.data.Add(new
                                            {
                                                Id = procesoSeleccion.ProcesoSeleccionId,
                                                FechaSolicitud = (DateTime?)(procesoSeleccion.FechaCreacion),
                                                NumeroSolicitud = procesoSeleccion.NumeroProceso,
                                                TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion).FirstOrDefault().Nombre,
                                                tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion
                                            });

                                        break;
                                    }
                            }
                    }
                    if (comite.data.Count > 0)
                        listaComitesGrilla.Add(comite);

                });



                return listaComitesGrilla;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Respuesta> CreateEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud(ComiteTecnico pComiteTecnico)
        {
            int idAccionCrearComiteTecnico = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Comite_Fiduciario_SesionComiteSolicitud_SesionComiteTema, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                string strCreateEdit;
                if (pComiteTecnico.ComiteTecnicoId == 0)
                {
                    //Agregar Tema Proposiciones y Varios
                    pComiteTecnico.SesionComiteTema.Add(
                           new SesionComiteTema
                           {
                               Eliminado = false,
                               UsuarioCreacion = pComiteTecnico.UsuarioCreacion,
                               FechaCreacion = DateTime.Now,
                               EsProposicionesVarios = true,
                               Tema = "",

                           });

                    strCreateEdit = "CREAR COMITE FIDUCIARIO  + SESIÓN COMITE SOLICITUD + SESIÓN COMITE TEMA";
                    //Auditoria
                    pComiteTecnico.FechaCreacion = DateTime.Now;
                    pComiteTecnico.Eliminado = false;
                    pComiteTecnico.EsComiteFiduciario = true;
                    //Registros
                    pComiteTecnico.EsCompleto = ValidarCamposComiteTecnico(pComiteTecnico);
                    pComiteTecnico.EsComiteFiduciario = true;

                    pComiteTecnico.EstadoComiteCodigo = ConstanCodigoEstadoComite.Sin_Convocatoria;
                    pComiteTecnico.NumeroComite = await _commonService.EnumeradorComiteFiduciario();


                    foreach (var SesionComiteTema in pComiteTecnico.SesionComiteTema)
                    {
                        //Auditoria
                        SesionComiteTema.FechaCreacion = DateTime.Now;
                        SesionComiteTema.UsuarioCreacion = pComiteTecnico.UsuarioCreacion;
                        SesionComiteTema.RegistroCompleto = ValidarRegistroCompletoSesionComiteTema(SesionComiteTema);
                        SesionComiteTema.Eliminado = false;
                    }

                    List<int> listaSolicitudes = new List<int>();

                    pComiteTecnico.SesionComiteSolicitudComiteTecnico.ToList().ForEach(ct => { listaSolicitudes.Add(ct.SesionComiteSolicitudId); });
                    pComiteTecnico.SesionComiteSolicitudComiteTecnico = new List<SesionComiteSolicitud>();

                    _context.ComiteTecnico.Add(pComiteTecnico);
                    _context.SaveChanges();

                    foreach (var id in listaSolicitudes)
                    {
                        SesionComiteSolicitud SesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(id);
                        //Auditoria 
                        SesionComiteSolicitudOld.UsuarioComiteFiduciario = pComiteTecnico.UsuarioCreacion;
                        SesionComiteSolicitudOld.FechaComiteFiduciario = DateTime.Now;

                        //Registros
                        SesionComiteSolicitudOld.ComiteTecnicoFiduciarioId = pComiteTecnico.ComiteTecnicoId;
                        SesionComiteSolicitudOld.RegistroCompleto = ValidarRegistroCompletoSesionComiteSolicitud(SesionComiteSolicitudOld);

                        //_context.SesionComiteSolicitud.Update( SesionComiteSolicitudOld );
                    }

                }
                else
                {
                    strCreateEdit = "EDITAR COMITE TECNICO  + SESIÓN COMITE SOLICITUD + SESIÓN COMITE TEMA";

                    ComiteTecnico comiteTecnicoOld = _context.ComiteTecnico
                        .Where(r => r.ComiteTecnicoId == pComiteTecnico.ComiteTecnicoId)
                            .Include(r => r.SesionComiteSolicitudComiteTecnico)
                            .Include(r => r.SesionComiteTema).FirstOrDefault();

                    //Auditoria 
                    comiteTecnicoOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                    comiteTecnicoOld.FechaModificacion = DateTime.Now;

                    //Registros
                    comiteTecnicoOld.EsCompleto = ValidarCamposComiteTecnico(comiteTecnicoOld);
                    comiteTecnicoOld.RequiereVotacion = comiteTecnicoOld.RequiereVotacion;
                    comiteTecnicoOld.Justificacion = comiteTecnicoOld.Justificacion;
                    comiteTecnicoOld.EsAprobado = comiteTecnicoOld.EsAprobado;
                    comiteTecnicoOld.FechaAplazamiento = comiteTecnicoOld.FechaAplazamiento;
                    comiteTecnicoOld.Observaciones = comiteTecnicoOld.Observaciones;
                    comiteTecnicoOld.RutaSoporteVotacion = comiteTecnicoOld.RutaSoporteVotacion;
                    comiteTecnicoOld.TieneCompromisos = comiteTecnicoOld.TieneCompromisos;
                    comiteTecnicoOld.CantCompromisos = comiteTecnicoOld.CantCompromisos;
                    comiteTecnicoOld.RutaActaSesion = comiteTecnicoOld.RutaActaSesion;
                    comiteTecnicoOld.FechaOrdenDia = comiteTecnicoOld.FechaOrdenDia;
                    comiteTecnicoOld.NumeroComite = comiteTecnicoOld.NumeroComite;
                    comiteTecnicoOld.EstadoComiteCodigo = comiteTecnicoOld.EstadoComiteCodigo;
                    comiteTecnicoOld.TipoTemaFiduciarioCodigo = pComiteTecnico.TipoTemaFiduciarioCodigo;

                    foreach (var SesionComiteTema in pComiteTecnico.SesionComiteTema)
                    {
                        if (SesionComiteTema.SesionTemaId == 0)
                        {

                            //Auditoria 
                            SesionComiteTema.UsuarioCreacion = pComiteTecnico.UsuarioCreacion;
                            SesionComiteTema.FechaCreacion = DateTime.Now;
                            SesionComiteTema.Eliminado = false;
                            SesionComiteTema.RegistroCompleto = ValidarRegistroCompletoSesionComiteTema(SesionComiteTema);
                            //Registros
                            SesionComiteTema.ComiteTecnicoId = pComiteTecnico.ComiteTecnicoId;
                            _context.SesionComiteTema.Add(SesionComiteTema);
                        }
                        else
                        {
                            SesionComiteTema sesionComiteTemaOld = _context.SesionComiteTema.Find(SesionComiteTema.SesionTemaId);
                            //Auditoria 
                            sesionComiteTemaOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                            sesionComiteTemaOld.FechaModificacion = DateTime.Now;

                            //Registros
                            sesionComiteTemaOld.Tema = SesionComiteTema.Tema;
                            sesionComiteTemaOld.ResponsableCodigo = SesionComiteTema.ResponsableCodigo;
                            sesionComiteTemaOld.TiempoIntervencion = SesionComiteTema.TiempoIntervencion;
                            sesionComiteTemaOld.RutaSoporte = SesionComiteTema.RutaSoporte;
                            sesionComiteTemaOld.Observaciones = SesionComiteTema.Observaciones;
                            sesionComiteTemaOld.EsAprobado = SesionComiteTema.EsAprobado;
                            sesionComiteTemaOld.ObservacionesDecision = SesionComiteTema.Observaciones;
                            sesionComiteTemaOld.EsProposicionesVarios = SesionComiteTema.EsProposicionesVarios;
                            sesionComiteTemaOld.RegistroCompleto = ValidarRegistroCompletoSesionComiteTema(sesionComiteTemaOld);
                        }
                    }

                    foreach (var SesionComiteSolicitud in pComiteTecnico.SesionComiteSolicitudComiteTecnico)
                    {
                        {
                            SesionComiteSolicitud SesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(SesionComiteSolicitud.SesionComiteSolicitudId);
                            //Auditoria 
                            SesionComiteSolicitudOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                            SesionComiteSolicitudOld.FechaModificacion = DateTime.Now;

                            //Registros
                            SesionComiteSolicitudOld.DesarrolloSolicitudFiduciario = SesionComiteSolicitud.DesarrolloSolicitudFiduciario;
                            SesionComiteSolicitudOld.EstadoActaCodigoFiduciario = SesionComiteSolicitud.EstadoActaCodigoFiduciario;
                            SesionComiteSolicitudOld.ComiteTecnicoFiduciarioId = pComiteTecnico.ComiteTecnicoId;
                            SesionComiteSolicitudOld.ObservacionesFiduciario = SesionComiteSolicitud.ObservacionesFiduciario;
                            SesionComiteSolicitudOld.RutaSoporteVotacionFiduciario = SesionComiteSolicitud.RutaSoporteVotacionFiduciario;
                            SesionComiteSolicitudOld.GeneraCompromisoFiduciario = SesionComiteSolicitud.GeneraCompromisoFiduciario;
                            SesionComiteSolicitudOld.CantCompromisosFiduciario = SesionComiteSolicitud.CantCompromisosFiduciario;
                            SesionComiteSolicitudOld.RequiereVotacionFiduciario = SesionComiteSolicitud.RequiereVotacionFiduciario;
                            SesionComiteSolicitudOld.RegistroCompleto = ValidarRegistroCompletoSesionComiteSolicitud(SesionComiteSolicitudOld);
                        }
                    }
                }
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteFiduciario.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccionCrearComiteTecnico, pComiteTecnico.UsuarioCreacion, strCreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccionCrearComiteTecnico, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        //Get all seseion comite tecnico fiduciario
        public async Task<List<ComiteGrilla>> GetCommitteeSession()
        {
            List<ComiteGrilla> ListComiteGrilla = new List<ComiteGrilla>();

            List<Dominio> ListaEstadoComite = await _context.Dominio
                .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Comite && (bool)r.Activo)
                .ToListAsync();

            List<Dominio> ListaEstadoActa = await _context.Dominio
                .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Acta && (bool)r.Activo)
                .ToListAsync();

            try
            {
                var ListComiteTecnico = await _context.ComiteTecnico.Where(r => !(bool)r.Eliminado && (bool)r.EsComiteFiduciario)
                                                                    .Include(r => r.SesionComiteTecnicoCompromiso)
                                                                    .Select(x => new
                                                                    {
                                                                        Id = x.ComiteTecnicoId,
                                                                        FechaComite = x.FechaOrdenDia,
                                                                        EstadoComite = x.EstadoComiteCodigo,
                                                                        x.NumeroComite,
                                                                        x.EsComiteFiduciario,
                                                                        x.EstadoActaCodigo,
                                                                        x.EsCompleto

                                                                    }).Distinct().OrderByDescending(r => r.Id).ToListAsync();

                foreach (var comite in ListComiteTecnico)
                {
                    ComiteGrilla comiteGrilla = new ComiteGrilla
                    {
                        Id = comite.Id,
                        FechaComite = comite.FechaComite.Value,
                        EstadoComiteCodigo = comite.EstadoComite,
                        EstadoComite = !string.IsNullOrEmpty(comite.EstadoComite) ? ListaEstadoComite.Where(r => r.Codigo == comite.EstadoComite).FirstOrDefault().Nombre : "",
                        NumeroComite = comite.NumeroComite,
                        EstadoActa = !string.IsNullOrEmpty(comite.EstadoActaCodigo) ? ListaEstadoActa.Where(r => r.Codigo == comite.EstadoActaCodigo).FirstOrDefault().Nombre : "",
                        EstadoActaCodigo = comite.EstadoActaCodigo,
                        RegistroCompletoNombre = (bool)comite.EsCompleto ? "Completo" : "Incompleto",
                        RegistroCompleto = comite.EsCompleto,
                        //NumeroCompromisos = numeroCompromisos(comite.Id, false),
                        //NumeroCompromisosCumplidos = numeroCompromisos(comite.Id, true),
                        EsComiteFiduciario = comite.EsComiteFiduciario,
                    };

                    ListComiteGrilla.Add(comiteGrilla);
                }
            }
            catch (Exception ex)
            {
            }
            return ListComiteGrilla;
        }

        //Solicitudes acordeon => Para seleccion de solicitudes contractuales, temas y solicitudes
        public async Task<ComiteTecnico> GetRequestCommitteeSessionById(int comiteTecnicoId)
        {
            try
            {
                ComiteTecnico comite = await _context.ComiteTecnico
                                .Where(sc => sc.ComiteTecnicoId == comiteTecnicoId && !(bool)sc.Eliminado)
                                .Include(cm => cm.SesionComiteTema)
                                .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                                .FirstOrDefaultAsync();

                comite.SesionComiteTema = comite.SesionComiteTema.Where(te => !(bool)te.Eliminado).ToList();

                return comite;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Respuesta> ConvocarComiteTecnico(ComiteTecnico pComiteTecnico, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Convocar_Comite_Fiduciario, (int)EnumeratorTipoDominio.Acciones);
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();

            try
            {
                ComiteTecnico comiteTecnico = await _context.ComiteTecnico
                    .Where(r => r.ComiteTecnicoId == pComiteTecnico.ComiteTecnicoId)
                    .Include(r => r.SesionComiteTema)
                    .Include(r => r.SesionParticipante)
                    .ThenInclude(r => r.Usuario).FirstOrDefaultAsync();

                comiteTecnico.SesionParticipante = comiteTecnico.SesionParticipante.Where(r => !(bool)r.Eliminado).ToList();
                comiteTecnico.SesionComiteTema = comiteTecnico.SesionComiteTema.Where(r => !(bool)r.Eliminado).ToList();

                comiteTecnico.EstadoComiteCodigo = ConstanCodigoEstadoComite.Convocada;
                comiteTecnico.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                comiteTecnico.FechaModificacion = DateTime.Now;


                //Plantilla
                string TipoPlantilla = ((int)ConstanCodigoPlantillas.Convocar_Comite_Tecnico).ToString();
                Plantilla plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).FirstOrDefault();


                string TipoPlantilla2 = ((int)ConstanCodigoPlantillas.Tabla_Orden_Del_Dia).ToString();
                Plantilla TablaTemasRegistro = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla2).Include(r => r.Encabezado).FirstOrDefault();
                string strRegistros = "";

                List<Dominio> ListaParametricas = _context.Dominio.ToList();

                foreach (var item in comiteTecnico.SesionComiteTema)
                {
                    strRegistros += TablaTemasRegistro.Contenido;

                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {
                            case ConstanCodigoVariablesPlaceHolders.TEMAS_ORDEN_DIA:
                                plantilla.Contenido = plantilla.Contenido.Replace(placeholderDominio.Nombre, item.Tema);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.RESPONSABLE_TEMA_ORDEN_DIA:
                                plantilla.Contenido = plantilla.Contenido.Replace(placeholderDominio.Nombre,
                                !string.IsNullOrEmpty(item.ResponsableCodigo) ? ListaParametricas.Where(r => r.Codigo == item.ResponsableCodigo
                                && r.TipoDominioId == (int)EnumeratorTipoDominio.Miembros_Comite_Tecnico).FirstOrDefault().Nombre : ""
                                );
                                break;

                            case ConstanCodigoVariablesPlaceHolders.TIEMPO_TEMA_ORDEN_DIA:
                                plantilla.Contenido = plantilla.Contenido.Replace(placeholderDominio.Nombre, item.TiempoIntervencion.ToString());
                                break;
                        }
                    }

                }

                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {
                        case ConstanCodigoVariablesPlaceHolders.FECHA_SESION_CONVOCAR_COMITE:
                            plantilla.Contenido = plantilla.Contenido.Replace(placeholderDominio.Nombre, comiteTecnico.FechaCreacion.ToString("yyyy-MM-dd"));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.ORDEN_DEL_DIA_CONVOCAR_COMITE:
                            plantilla.Contenido = plantilla.Contenido.Replace(placeholderDominio.Nombre, strRegistros);
                            break;
                    }
                }
                //Notificar a los participantes
                bool blEnvioCorreo = false;

                //TODO: esta lista debe ser parametrizada de acuerdo a los perfiles Directore de las 4 areas :
                //Director financiero, Director Juridico , Director técnico, y Director administrativo
                List<Usuario> ListMiembrosComite = await _commonService.GetUsuariosByPerfil((int)EnumeratorPerfil.Miembros_Comite);


                foreach (var Usuario in ListMiembrosComite)
                {
                    if (!string.IsNullOrEmpty(Usuario.Email))
                    {

                        blEnvioCorreo = Helpers.Helpers.EnviarCorreo(Usuario.Email, "Convocatoria sesión de comité técnico", plantilla.Contenido, pSentender, pPassword, pMailServer, pMailPort);
                    }
                }

                _context.SaveChanges();
                return
                   new Respuesta
                   {
                       //Data = await GetComiteTecnicoByComiteTecnicoId(pComiteTecnico.ComiteTecnicoId),
                       IsSuccessful = true,
                       IsException = false,
                       IsValidation = false,
                       Code = ConstantSesionComiteTecnico.OperacionExitosa,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pComiteTecnico.UsuarioCreacion, "CONVOCAR COMITE TECNICO")
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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }

        public async Task<Respuesta> CambiarEstadoComiteTecnico(ComiteTecnico pComiteTecnico)
        {
            int idAccionCambiarEstadoSesion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Comite_Fiduciario, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ComiteTecnico ComiteTecnicoOld = _context.ComiteTecnico.Find(pComiteTecnico.ComiteTecnicoId);

                string NombreEstado = await _commonService.GetNombreDominioByCodigoAndTipoDominio(pComiteTecnico.EstadoComiteCodigo, (int)EnumeratorTipoDominio.Estado_Comite);

                ComiteTecnicoOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                ComiteTecnicoOld.FechaModificacion = DateTime.Now;

                ComiteTecnicoOld.EstadoComiteCodigo = pComiteTecnico.EstadoComiteCodigo;

                if (ComiteTecnicoOld.EstadoComiteCodigo == ConstanCodigoEstadoComite.Desarrollada_Sin_Acta)
                {
                    ComiteTecnicoOld.EstadoActaCodigo = "1"; //Sin Acta
                }

                if (ComiteTecnicoOld.EstadoComiteCodigo == ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Enviada)
                {
                    ComiteTecnicoOld.EstadoActaCodigo = "2"; //En proceso de aprobación
                }

                _context.SaveChanges();
                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccionCambiarEstadoSesion, pComiteTecnico.UsuarioCreacion, "ESTADO COMITE CAMBIADO A " + NombreEstado.ToUpper())
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccionCambiarEstadoSesion, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                };
            }

        }

        public async Task<ComiteTecnico> GetComiteTecnicoByComiteTecnicoId(int pComiteTecnicoId)
        {
            if (pComiteTecnicoId == 0)
            {

                return new ComiteTecnico();
            }
            //  List<SesionParticipante> sesionParticipantes = _context.SesionParticipante.Where(r=> r.ComiteTecnicoId == pComiteTecnicoId).ToList();

            ComiteTecnico comiteTecnico = await _context.ComiteTecnico
                 .Where(r => r.ComiteTecnicoId == pComiteTecnicoId)

                  .Include(r => r.SesionInvitado)
                  .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                     .ThenInclude(r => r.SesionSolicitudVoto)
                  .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                     .ThenInclude(r => r.SesionSolicitudCompromiso)
                  .Include(r => r.SesionComiteTema)
                     .ThenInclude(r => r.SesionTemaVoto)
                  .Include(r => r.SesionComiteTema)
                     .ThenInclude(r => r.TemaCompromiso)
                 .FirstOrDefaultAsync();

            //    comiteTecnico.SesionParticipante = sesionParticipantes;

            comiteTecnico.SesionComiteTema = comiteTecnico.SesionComiteTema.Where(r => !(bool)r.Eliminado).ToList();
            comiteTecnico.SesionInvitado = comiteTecnico.SesionInvitado.Where(r => !(bool)r.Eliminado).ToList();


            foreach (var SesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
            {
                SesionComiteSolicitud.SesionSolicitudVoto = SesionComiteSolicitud.SesionSolicitudVoto.Where(r => !(bool)r.Eliminado).ToList();
                SesionComiteSolicitud.SesionSolicitudCompromiso = SesionComiteSolicitud.SesionSolicitudCompromiso.Where( r => r.EsFiduciario == true && !(bool)r.Eliminado ).ToList();
            }

            List<SesionSolicitudVoto> ListSesionSolicitudVotos = _context.SesionSolicitudVoto.Where(r => !(bool)r.Eliminado).ToList();
            foreach (var SesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
            {
                SesionComiteSolicitud.SesionSolicitudVoto = ListSesionSolicitudVotos.Where(r => r.SesionComiteSolicitudId == SesionComiteSolicitud.SesionComiteSolicitudId).ToList();
            }
            List<Dominio> TipoComiteSolicitud = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud).ToList();

            List<ProcesoSeleccion> ListProcesoSeleccion =
                _context.ProcesoSeleccion
                .Where(r => !(bool)r.Eliminado).ToList();

            List<Contratacion> ListContratacion = _context.Contratacion.ToList();

            foreach (SesionComiteSolicitud sesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
            {

                switch (sesionComiteSolicitud.TipoSolicitudCodigo)
                {
                    case ConstanCodigoTipoSolicitud.Contratacion:

                        Contratacion contratacion = ListContratacion.Where(r => r.ContratacionId == sesionComiteSolicitud.SolicitudId).FirstOrDefault();

                        sesionComiteSolicitud.FechaSolicitud = (DateTime)contratacion.FechaCreacion;

                        sesionComiteSolicitud.NumeroSolicitud = contratacion.NumeroSolicitud;

                        sesionComiteSolicitud.Contratacion = contratacion;

                        break;

                    case ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion:
                        sesionComiteSolicitud.FechaSolicitud = ListProcesoSeleccion
                          .Where(r => r.ProcesoSeleccionId == sesionComiteSolicitud.SolicitudId)
                          .FirstOrDefault()
                          .FechaCreacion;

                        sesionComiteSolicitud.NumeroSolicitud = ListProcesoSeleccion
                          .Where(r => r.ProcesoSeleccionId == sesionComiteSolicitud.SolicitudId)
                          .FirstOrDefault()
                          .NumeroProceso;

                        sesionComiteSolicitud.ProcesoSeleccion = ListProcesoSeleccion.Where(r => r.ProcesoSeleccionId == sesionComiteSolicitud.SolicitudId).FirstOrDefault();

                        break;
                }

                sesionComiteSolicitud.TipoSolicitud = TipoComiteSolicitud.Where(r => r.Codigo == sesionComiteSolicitud.TipoSolicitudCodigo).FirstOrDefault().Nombre;
            }

            return comiteTecnico;
        }

        private string ReemplazarDatosPlantillaContratacion(string pPlantilla, Contratacion pContratacion)
        {
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();

            string TipoPlantillaDetalleProyecto = ((int)ConstanCodigoPlantillas.Detalle_Proyecto).ToString();
            string DetalleProyecto = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaDetalleProyecto).Select(r => r.Contenido).FirstOrDefault();
            string DetallesProyectos = "";

            string TipoPlantillaRegistrosAlcance = ((int)ConstanCodigoPlantillas.Registros_Tabla_Alcance).ToString();
            string RegistroAlcance = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaRegistrosAlcance).Select(r => r.Contenido).FirstOrDefault();

            List<Dominio> ListaParametricas = _context.Dominio.ToList();
            List<Localizacion> ListaLocalizaciones = _context.Localizacion.ToList();
            List<InstitucionEducativaSede> ListaInstitucionEducativaSedes = _context.InstitucionEducativaSede.ToList();
            //Se crea el detalle de los proyectos asociado a contratacion - contratacionProyecto 
            int enumProyecto = 1;

            foreach (var proyecto in pContratacion.ContratacionProyecto)
            {
                //Se crear una nueva plantilla por cada vez que entra
                DetallesProyectos += DetalleProyecto;
                string RegistrosAlcance = "";

                Localizacion Municipio = ListaLocalizaciones.Where(r => r.LocalizacionId == proyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                Localizacion Departamento = ListaLocalizaciones.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                Localizacion Region = ListaLocalizaciones.Where(r => r.LocalizacionId == Departamento.IdPadre).FirstOrDefault();

                InstitucionEducativaSede IntitucionEducativa = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == proyecto.Proyecto.InstitucionEducativaId).FirstOrDefault();
                InstitucionEducativaSede Sede = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == proyecto.Proyecto.SedeId).FirstOrDefault();



                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {

                        case ConstanCodigoVariablesPlaceHolders.NOMBRE_PROYECTO:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, (enumProyecto++).ToString());
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TIPO_DE_INTERVENCION:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, ListaParametricas.Where(r => r.Codigo == proyecto.Proyecto.TipoIntervencionCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).FirstOrDefault().Nombre);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.LLAVE_MEN:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.LlaveMen);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.REGION:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Region.Descripcion);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.DEPARTAMENTO:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Departamento.Descripcion);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.MUNICIPIO:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Municipio.Descripcion);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.INSTITUCION_EDUCATIVA:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, IntitucionEducativa.Nombre);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.CODIGO_DANE_IE:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, IntitucionEducativa.CodigoDane.ToString());
                            break;

                        case ConstanCodigoVariablesPlaceHolders.SEDE:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Sede.Nombre);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.CODIGO_DANE_SEDE:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Sede.CodigoDane.ToString());
                            break;

                        case ConstanCodigoVariablesPlaceHolders.REGISTROS_ALCANCE:
                            //Predio Principal

                            //List<Predio> ListPredios = proyecto.Proyecto.ProyectoPredio.Select(r => r.Predio).ToList();
                            //ListPredios.Add(proyecto.Proyecto.PredioPrincipal); 
                            //var PrediosOrdenadosPorTipoPredio = ListPredios.GroupBy(x => x.TipoPredioCodigo)
                            //           .Select(x => new {
                            //               Espacio = x.Key,
                            //               Cantidad = x.Count()});

                            foreach (var infraestructura in proyecto.Proyecto.InfraestructuraIntervenirProyecto)
                            {
                                RegistrosAlcance += RegistroAlcance;

                                RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_ESPACIOS_A_INTERVENIR]", ListaParametricas
                                    .Where(r => r.Codigo == infraestructura.InfraestructuraCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Espacios_Intervenir)
                                    .FirstOrDefault().Nombre);
                                RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_CANTIDAD]", infraestructura.Cantidad.ToString());
                            }

                            //Dictionary<string, int> DictionaryRegistrosAlcance = new Dictionary<string, int>();

                            //foreach (var ListRegistrosAlcance in proyecto.Proyecto.ProyectoPredio.GroupBy(predio => predio.Predio.TipoPredioCodigo)
                            //       .Select(group => new
                            //       {
                            //           Espacio = group.Key,
                            //           Cantidad = group.Count()
                            //       })
                            //       .OrderBy(x => x.Cantidad)) ;
                            //          DictionaryRegistrosAlcance.Add(ListRegistrosAlcance.Espacio, ListRegistrosAlcance.Cantidad);

                            //Agregar el predio principal a los otros predios relacionados con el proyecto 
                            //RegistrosAlcance += RegistroAlcance;

                            //RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_ESPACIOS_A_INTERVENIR]", ListaParametricas.Where(r => r.Codigo == proyecto.Proyecto.PredioPrincipal.TipoPredioCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Espacios_Intervenir).FirstOrDefault().Nombre);
                            //RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_CANTIDAD]", "1");

                            //// Lista Predios 
                            //foreach (var predio in proyecto.Proyecto.ProyectoPredio)
                            //{
                            //    RegistrosAlcance += RegistroAlcance;

                            //    RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_ESPACIOS_A_INTERVENIR]", ListaParametricas.Where(r => r.Codigo == predio.Predio.TipoPredioCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Espacios_Intervenir).FirstOrDefault().Nombre);
                            //    RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_CANTIDAD]", "1");

                            //}
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, RegistrosAlcance);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.PLAZO_OBRA_MESES:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.InfraestructuraIntervenirProyecto.Sum(r => r.PlazoMesesObra).ToString());
                            break;

                        case ConstanCodigoVariablesPlaceHolders.PLAZO_OBRA_DIAS:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.InfraestructuraIntervenirProyecto.Sum(r => r.PlazoDiasObra).ToString());
                            break;

                        case ConstanCodigoVariablesPlaceHolders.VALOR_OBRA:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, "$" + String.Format("{0:n0}", proyecto.Proyecto.ValorObra));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.VALOR_INTERVENTORIA:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, "$" + String.Format("{0:n0}", proyecto.Proyecto.ValorInterventoria));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.VALOR_TOTAL_PROYECTO:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, "$" + String.Format("{0:n0}", proyecto.Proyecto.ValorTotal));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.REGISTROS_FUENTES_USO:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, " ");
                            break;
                    }
                }
            }


            foreach (Dominio placeholderDominio in placeholders)
            {
                //ConstanCodigoVariablesPlaceHolders placeholder = (ConstanCodigoVariablesPlaceHolders)placeholderDominio.Codigo.ToString();

                switch (placeholderDominio.Codigo)
                {
                    case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.NumeroSolicitud);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.FechaTramite != null ? ((DateTime)pContratacion.FechaTramite).ToString("yyyy-MM-dd") : " ");
                        break;

                    case ConstanCodigoVariablesPlaceHolders.OPCION_POR_CONTRATAR:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, ListaParametricas.Where(r => r.Codigo == pContratacion.TipoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud_Obra_Interventorias).FirstOrDefault().Nombre);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.VALOR_TOTAL_DE_LA_SOLICITUD:
                        decimal? ValorTotal = pContratacion.ContratacionProyecto.Sum(r => r.Proyecto.ValorTotal);
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, "$" + String.Format("{0:n0}", ValorTotal));
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CANTIDAD_DE_PROYECTOS_ASOCIADOS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.ContratacionProyecto.Count().ToString());
                        break;
                    //Datos Contratista 
                    case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NOMBRE:

                        if (pContratacion.Contratista != null)
                        {
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.Contratista.Nombre);
                        }
                        else
                        {
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, " ");
                        }
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NUMERO_IDENTIFICACION:
                        if (pContratacion.Contratista != null)
                        {
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.Contratista.NumeroIdentificacion);
                        }
                        else
                        {
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, " ");
                        }

                        break;

                    case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NOMBRE_RE_LEGAL:
                        if (pContratacion.Contratista != null)
                        {
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.Contratista.RepresentanteLegal);
                        }
                        else
                        {
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, " ");
                        }
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NUMERO_IDENTIFICACION_RE_LEGAL:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, " ");
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NUMERO_INVITACION:
                        if (pContratacion.Contratista != null)
                        {

                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.Contratista.NumeroInvitacion);
                        }
                        else
                        {
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, "");
                        }

                        break;
                    //
                    case ConstanCodigoVariablesPlaceHolders.DETALLES_PROYECTOS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, DetallesProyectos);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.NUMERO_DE_LICENCIA:
                        string numeroLicencia = "";
                        if (pContratacion.ContratacionProyecto.Count() > 0)
                        {
                            numeroLicencia = pContratacion.ContratacionProyecto.FirstOrDefault().NumeroLicencia;
                        }
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, numeroLicencia);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.FECHA_DE_VIGENCIA:
                        string fechaVigencia = "";
                        if (pContratacion.ContratacionProyecto.Count() > 0)
                        {
                            if (pContratacion.ContratacionProyecto.FirstOrDefault().FechaVigencia != null)
                            {
                                fechaVigencia = ((DateTime)pContratacion.ContratacionProyecto.FirstOrDefault().FechaVigencia).ToString("yy-MM-dd");

                            }
                        }
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, fechaVigencia);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CONSIDERACIONES_ESPECIALES:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.ConsideracionDescripcion);
                        break;


                }
            }
            //Preguntas    //Preguntas 
            //Preguntas    //Preguntas 
            string strPregunta_1 = " ";

            string strPregunta_2 = "";
            string ContenidoPregunta2 = _context.Plantilla.Where(r => r.Codigo.Equals(((int)ConstanCodigoPlantillas.dos_pregunta_tecnica_o_juridica).ToString())).FirstOrDefault().Contenido;

            string strPregunta_3 = "";
            string ContenidoPregunta3 = _context.Plantilla.Where(r => r.Codigo.Equals(((int)ConstanCodigoPlantillas.tres_pregunta_tecnica_o_juridica).ToString())).FirstOrDefault().Contenido;

            string strPregunta_4 = "";
            string ContenidoPregunta4 = _context.Plantilla.Where(r => r.Codigo.Equals(((int)ConstanCodigoPlantillas.cuatro_pregunta_tecnica_o_juridica).ToString())).FirstOrDefault().Contenido;

            string strPregunta_5 = "";
            string ContenidoPregunta5 = _context.Plantilla.Where(r => r.Codigo.Equals(((int)ConstanCodigoPlantillas.cinco_pregunta_tecnica_o_juridica).ToString())).FirstOrDefault().Contenido;

            string strPregunta_6 = "";
            string ContenidoPregunta6 = _context.Plantilla.Where(r => r.Codigo.Equals(((int)ConstanCodigoPlantillas.seis_pregunta_tecnica_o_juridica).ToString())).FirstOrDefault().Contenido;


            if (pContratacion.ContratacionProyecto.Count() > 0)
            {

                //Pregunta 1 
                if (pContratacion.ContratacionProyecto.FirstOrDefault().TieneMonitoreWeb == null ||
                    !(bool)pContratacion.ContratacionProyecto.FirstOrDefault().TieneMonitoreWeb)
                {
                    //Si la respuesta a la pregunta 1, fue “No”, el sistema mostrará la pregunta 4
                    strPregunta_1 = " no";
                    // strPregunta_4 = ContenidoPregunta4 + " " + (pContratacion.ContratacionProyecto.FirstOrDefault().PorcentajeAvanceObra).ToString() + "%";
                }
                else
                {
                    //Si la respuesta fue “Si”, el sistema mostrará la pregunta 2.  
                    strPregunta_1 = " si";

                    //pregunta 2
                    if (pContratacion.ContratacionProyecto.FirstOrDefault().EsReasignacion == null
                        || !(bool)pContratacion.ContratacionProyecto.FirstOrDefault().EsReasignacion)
                    {
                        //pregunta 5
                        if (pContratacion.ContratacionProyecto.FirstOrDefault().RequiereLicencia == null
                            || !(bool)pContratacion.ContratacionProyecto.FirstOrDefault().RequiereLicencia)
                        {

                        }
                        else
                        {
                            strPregunta_5 = ContenidoPregunta5 + " si";
                            strPregunta_6 = ContenidoPregunta6 + " " + pContratacion.ContratacionProyecto.FirstOrDefault().LicenciaVigente;
                        }
                    }
                    else
                    {
                        strPregunta_2 = ContenidoPregunta2 + " si";
                        //pregunta 3
                        if (pContratacion.ContratacionProyecto.FirstOrDefault().EsAvanceobra == null
                           || !(bool)pContratacion.ContratacionProyecto.FirstOrDefault().EsAvanceobra)
                        {

                        }
                        else
                        {

                            strPregunta_3 = ContenidoPregunta3 + " si";
                            strPregunta_4 = ContenidoPregunta4 + " " + (pContratacion.ContratacionProyecto.FirstOrDefault().PorcentajeAvanceObra).ToString() + "%";
                            //pregunta 5
                            if (pContratacion.ContratacionProyecto.FirstOrDefault().RequiereLicencia == null
                                || !(bool)pContratacion.ContratacionProyecto.FirstOrDefault().RequiereLicencia)
                            {

                            }
                            else
                            {
                                strPregunta_5 = ContenidoPregunta5 + " si";
                                if (pContratacion.ContratacionProyecto.FirstOrDefault().LicenciaVigente == null || !(bool)pContratacion.ContratacionProyecto.FirstOrDefault().LicenciaVigente)
                                {
                                    strPregunta_6 = ContenidoPregunta6 + " no";
                                }
                                {
                                    strPregunta_6 = ContenidoPregunta6 + " si";
                                }

                            }

                        }
                    }

                }
            }




            pPlantilla = pPlantilla.Replace("[PREGUNTA_1]", strPregunta_1);
            pPlantilla = pPlantilla.Replace("[PREGUNTA_2]", strPregunta_2);
            pPlantilla = pPlantilla.Replace("[PREGUNTA_3]", strPregunta_3);
            pPlantilla = pPlantilla.Replace("[PREGUNTA_4]", strPregunta_4);
            pPlantilla = pPlantilla.Replace("[PREGUNTA_5]", strPregunta_5);
            pPlantilla = pPlantilla.Replace("[PREGUNTA_6]", strPregunta_6);
            return pPlantilla;
        }

        private byte[] ConvertirPDF(Plantilla pPlantilla)
        {
            string strEncabezado = "";
            if (!string.IsNullOrEmpty(pPlantilla.Encabezado.Contenido))
            {
                strEncabezado = Helpers.Helpers.HtmlStringLimpio(pPlantilla.Encabezado.Contenido);
            }

            var globalSettings = new GlobalSettings
            {
                ImageQuality = 1080,
                PageOffset = 0,
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings
                {
                    Top = pPlantilla.MargenArriba,
                    Left = pPlantilla.MargenIzquierda,
                    Right = pPlantilla.MargenDerecha,
                    Bottom = pPlantilla.MargenAbajo
                },
                DocumentTitle = DateTime.Now.ToString(),
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = pPlantilla.Contenido,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "pdf-styles.css") },
                HeaderSettings = { FontName = "Roboto", FontSize = 8, Center = strEncabezado, Line = false, Spacing = 18 },
                FooterSettings = { FontName = "Ariel", FontSize = 10, Center = "[page]" },
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            return _converter.Convert(pdf);
        }

        private async Task<byte[]> ReplacePlantillaFichaContratacion(int pContratacionId)
        {
            Contratacion contratacion = await _IProjectContractingService.GetAllContratacionByContratacionId(pContratacionId);

            if (contratacion == null)
            {
                return Array.Empty<byte>();
            }

            string TipoPlantilla = ((int)ConstanCodigoPlantillas.Ficha_De_Contratacion).ToString();

            Plantilla Plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            Plantilla.Contenido = ReemplazarDatosPlantillaContratacion(Plantilla.Contenido, contratacion);
            return ConvertirPDF(Plantilla);

        }

        private string ReemplazarDatosPlantillaProcesosSeleccion(string pPlantilla, ProcesoSeleccion pProcesoSeleccion)
        {
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();

            string TipoPlantillaRegistrosGruposProcesoSeleccion = ((int)ConstanCodigoPlantillas.Registros_Grupos_Proceso_Seleccion).ToString();
            string DetalleGrupoProcesosSeleccion = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaRegistrosGruposProcesoSeleccion).Select(r => r.Contenido).FirstOrDefault();
            string DetallesGrupoProcesosSeleccion = "";

            string TipoPlantillaRegistrosCronograma = ((int)ConstanCodigoPlantillas.Registros_Cronograma_Proceso_seleccion).ToString();
            string RegistroCronograma = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaRegistrosCronograma).Select(r => r.Contenido).FirstOrDefault();
            string RegistrosCronogramas = "";

            string TipoPlantillaProcesoSeleccionPrivada = ((int)ConstanCodigoPlantillas.Proceso_de_seleccion_Privada).ToString();
            string ProcesoSeleccionPrivada = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaProcesoSeleccionPrivada).Select(r => r.Contenido).FirstOrDefault();
            string ProcesosSeleccionPrivada = "";

            string TipoPlantillaProcesoSeleccionCerrada = ((int)ConstanCodigoPlantillas.Proceso_de_seleccion_Cerrada).ToString();
            string ProcesoSeleccionCerrada = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaProcesoSeleccionCerrada).Select(r => r.Contenido).FirstOrDefault();
            string ProcesosSeleccionCerrada = "";

            string TipoPlantillaProcesoSeleccionAbierta = ((int)ConstanCodigoPlantillas.Proceso_de_seleccion_Abierta).ToString();
            string ProcesoSeleccionAbierta = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaProcesoSeleccionAbierta).Select(r => r.Contenido).FirstOrDefault();
            string ProcesosSeleccionAbierta = " ";

            List<Dominio> ListaParametricas = _context.Dominio.ToList();

            //Plantilla Grupos de seleccion
            foreach (var ProcesoSeleccionGrupo in pProcesoSeleccion.ProcesoSeleccionGrupo)
            {
                DetallesGrupoProcesosSeleccion += DetalleGrupoProcesosSeleccion;

                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {
                        case ConstanCodigoVariablesPlaceHolders.NOMBRE_GRUPO_PS:
                            DetallesGrupoProcesosSeleccion = DetallesGrupoProcesosSeleccion
                                .Replace(placeholderDominio.Nombre, ProcesoSeleccionGrupo.NombreGrupo);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.PRESUPUESTO_OFICIAL_PS:
                            DetallesGrupoProcesosSeleccion = DetallesGrupoProcesosSeleccion
                                 .Replace(placeholderDominio.Nombre,
                                 !string.IsNullOrEmpty(ProcesoSeleccionGrupo.TipoPresupuestoCodigo) ?
                            ListaParametricas.Where(r => r.Codigo == ProcesoSeleccionGrupo.TipoPresupuestoCodigo
                            && r.TipoDominioId == (int)EnumeratorTipoDominio.Presupuesto_Proceso_de_Selección)
                            .FirstOrDefault().Nombre
                            : "");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.PLAZO_EN_MESES_PS:
                            DetallesGrupoProcesosSeleccion = DetallesGrupoProcesosSeleccion
                                .Replace(placeholderDominio.Nombre, ProcesoSeleccionGrupo.PlazoMeses.ToString());
                            break;
                    }
                }
            }

            //Plantilla Cronograma 
            foreach (var ProcesoSeleccionCronograma in pProcesoSeleccion.ProcesoSeleccionCronograma)
            {
                RegistrosCronogramas += RegistroCronograma;

                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {
                        case ConstanCodigoVariablesPlaceHolders.ACTIVIDAD_CRONOGRAMA_PS:
                            RegistrosCronogramas = RegistrosCronogramas.Replace(placeholderDominio.Nombre,
                            !string.IsNullOrEmpty(ProcesoSeleccionCronograma.EstadoActividadCodigo) ?
                            ListaParametricas
                            .Where(r => r.Codigo == ProcesoSeleccionCronograma.EstadoActividadCodigo
                            && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_alcance)
                            .FirstOrDefault().Nombre : " ");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.FECHA_CRONOGRAMA_PS:
                            RegistrosCronogramas = RegistrosCronogramas.Replace(placeholderDominio.Nombre,
                             ProcesoSeleccionCronograma.FechaCreacion.ToString("yyy-MM-dd"));
                            break;
                    }
                }
            }

            //Plantilla que Depende del Tipo de proceso de solicitud

            switch (pProcesoSeleccion.TipoProcesoCodigo)
            {
                case ConstanCodigoTipoProcesoSeleccion.Invitacion_Abierta:
                    ProcesosSeleccionAbierta = ProcesoSeleccionAbierta;
                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {
                            case ConstanCodigoVariablesPlaceHolders.CONDICIONES_JURIDICAS_HABILITANTES_ABIERTA_PS:
                                ProcesosSeleccionAbierta = ProcesosSeleccionAbierta.
                                  Replace(placeholderDominio.Nombre, pProcesoSeleccion.CondicionesJuridicasHabilitantes);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.CONDICIONES_FINANCIERAS_HABILITANTES_ABIERTA_PS:
                                ProcesosSeleccionAbierta = ProcesosSeleccionAbierta.
                                  Replace(placeholderDominio.Nombre, pProcesoSeleccion.CondicionesFinancierasHabilitantes);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.CONDICIONES_TECNICAS_HABILITANTES_ABIERTA_PS:
                                ProcesosSeleccionAbierta = ProcesosSeleccionAbierta.
                                  Replace(placeholderDominio.Nombre, pProcesoSeleccion.CondicionesTecnicasHabilitantes);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.CONDICIONES_ASIGNACION_PUNTAJE_ABIERTA_PS:
                                ProcesosSeleccionAbierta = ProcesosSeleccionAbierta.
                                  Replace(placeholderDominio.Nombre, pProcesoSeleccion.CondicionesAsignacionPuntaje);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.RESPONSABLES_ABIERTA_PS:
                                string NombresPreponente = "";
                                foreach (var ProcesoSeleccionProponente in pProcesoSeleccion.ProcesoSeleccionProponente)
                                {
                                    NombresPreponente += ProcesoSeleccionProponente.NombreProponente + " - ";
                                }
                                ProcesosSeleccionAbierta = ProcesosSeleccionAbierta.
                                  Replace(placeholderDominio.Nombre, NombresPreponente);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.EVALUACION_DESCRIPCION_ABIERTA_PS:
                                ProcesosSeleccionAbierta = ProcesosSeleccionAbierta.
                                  Replace(placeholderDominio.Nombre, pProcesoSeleccion.EvaluacionDescripcion);
                                break;
                        }
                    }

                    break;
                case ConstanCodigoTipoProcesoSeleccion.Invitacion_Cerrada:
                    ProcesosSeleccionCerrada = ProcesoSeleccionCerrada;
                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {
                            case ConstanCodigoVariablesPlaceHolders.CRITERIOS_SELECCION_CERRADA_PS:
                                ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                                  Replace(placeholderDominio.Nombre, pProcesoSeleccion.CriteriosSeleccion);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.RESPONSABLES_CERRADA_PS:
                                string NombresPreponente = "";
                                foreach (var ProcesoSeleccionProponente in pProcesoSeleccion.ProcesoSeleccionProponente)
                                {
                                    NombresPreponente += ProcesoSeleccionProponente.NombreProponente + " - ";
                                }
                                ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                                Replace(placeholderDominio.Nombre, NombresPreponente);
                                break;

                            //[4:02 PM, 8/26/2020] Faber Ivolucion: se campo no tiene descripción
                            //[4:03 PM, 8 / 26 / 2020] Faber Ivolucion: no se si lo quitaron o ya en aparece algo en el control de cambios
                            //    [4:04 PM, 8 / 26 / 2020] JULIÁN MARTÍNEZ C: y el VALOR_CONTIZACION_CERRADA
                            //        [4:12 PM, 8 / 26 / 2020] Faber Ivolucion: Tampoco aparece en CU

                            //case ConstanCodigoVariablesPlaceHolders.NOMBRE_ORGANIZACION_CERRADA_PS:
                            //    ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                            //      Replace(placeholderDominio.Nombre, pProcesoSeleccion.);
                            //    break; 

                            //case ConstanCodigoVariablesPlaceHolders.VALOR_CONTIZACION_CERRADA_PS:
                            //    ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                            //      Replace(placeholderDominio.Nombre, pProcesoSeleccion.);
                            //    break;

                            case ConstanCodigoVariablesPlaceHolders.EVALUACION_DESCRIPCION_CERRADA_PS:
                                ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                                  Replace(placeholderDominio.Nombre, pProcesoSeleccion.EvaluacionDescripcion);
                                break;
                        }
                    }
                    break;

                case ConstanCodigoTipoProcesoSeleccion.Invitacion_Privada:
                    ProcesosSeleccionPrivada = ProcesoSeleccionPrivada;
                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {
                            case ConstanCodigoVariablesPlaceHolders.TIPO_PROPONENTE_PRIVADA_PS:

                                ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                                  Replace(placeholderDominio.Nombre, (pProcesoSeleccion.ProcesoSeleccionProponente.Count() > 0) ? ListaParametricas
                                  .Where(r => r.Codigo == pProcesoSeleccion.ProcesoSeleccionProponente.FirstOrDefault().TipoProponenteCodigo
                                  && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Proponente
                                  ).FirstOrDefault().Nombre : " ");
                                break;

                            case ConstanCodigoVariablesPlaceHolders.NOMBRE_PRIVADA_PS:

                                ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                                  Replace(placeholderDominio.Nombre, (pProcesoSeleccion.ProcesoSeleccionProponente.Count() > 0) ? pProcesoSeleccion.ProcesoSeleccionProponente.FirstOrDefault().NombreProponente : "");
                                break;

                            case ConstanCodigoVariablesPlaceHolders.TIPO_DOCUMENTO_PRIVADA_PS:
                                ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                                Replace(placeholderDominio.Nombre, (pProcesoSeleccion.ProcesoSeleccionProponente.Count() > 0)
                                ? ListaParametricas.Where(r => r.Codigo == pProcesoSeleccion.ProcesoSeleccionProponente.FirstOrDefault().TipoIdentificacionCodigo
                                && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Documento
                                ).FirstOrDefault().Nombre : " ");
                                break;

                            case ConstanCodigoVariablesPlaceHolders.NOMBRE_REPRESENTANTE_LEGAL_PRIVADA_PS:
                                ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                               Replace(placeholderDominio.Nombre, (pProcesoSeleccion.ProcesoSeleccionProponente.Count() > 0) ? pProcesoSeleccion.ProcesoSeleccionProponente.FirstOrDefault().NombreRepresentanteLegal : "");

                                break;
                        }
                    }
                    break;
            }

            //Plantilla Principal
            foreach (Dominio placeholderDominio in placeholders)
            {
                switch (placeholderDominio.Codigo)
                {
                    case ConstanCodigoVariablesPlaceHolders.NUMERO_PROCESO_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pProcesoSeleccion.NumeroProceso);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD_PS:
                        //TODO: DOnde se guarda la fecha de solicitud = fecha creacion
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pProcesoSeleccion.FechaCreacion.ToString("yyyy-MM-dd"));
                        break;

                    case ConstanCodigoVariablesPlaceHolders.TIPO_SOLICITUD_PS:
                        pPlantilla = pPlantilla.Replace
                            (placeholderDominio.Nombre,
                            !string.IsNullOrEmpty(pProcesoSeleccion.TipoProcesoCodigo) ?
                            ListaParametricas
                            .Where(r => r.Codigo == pProcesoSeleccion.TipoProcesoCodigo
                            && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Proceso_Seleccion)
                            .FirstOrDefault().Nombre : "");
                        break;

                    case ConstanCodigoVariablesPlaceHolders.OBJETO_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pProcesoSeleccion.Objeto);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.ALCANCE_PARTICULAR_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pProcesoSeleccion.AlcanceParticular);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.JUSTIFICACION_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pProcesoSeleccion.Justificacion);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.TIPO_INTERVENCION_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre,
                            !string.IsNullOrEmpty(pProcesoSeleccion.TipoIntervencionCodigo) ?
                            ListaParametricas.Where(r => r.Codigo == pProcesoSeleccion.TipoIntervencionCodigo
                            && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion
                            ).FirstOrDefault().Nombre
                            : "");
                        break;

                    case ConstanCodigoVariablesPlaceHolders.TIPO_ALCANCE_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre,
                             !string.IsNullOrEmpty(pProcesoSeleccion.TipoAlcanceCodigo) ?
                             ListaParametricas
                             .Where(r => r.Codigo == pProcesoSeleccion.TipoAlcanceCodigo
                             && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_alcance)
                             .FirstOrDefault().Nombre
                             : ""); break;

                    case ConstanCodigoVariablesPlaceHolders.DISTRIBUCION_TERRITORIO_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pProcesoSeleccion.EsDistribucionGrupos != null ?
                            (bool)pProcesoSeleccion.EsDistribucionGrupos ? "Si" : "No" : " ");
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CUANTOS_GRUPOS_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pProcesoSeleccion.CantGrupos.ToString());
                        break;

                    ///Plantillas dinamicas
                    ///
                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_GRUPOS_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, DetallesGrupoProcesosSeleccion);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_CRONOGRAMA_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, RegistrosCronogramas);
                        break;
                    case ConstanCodigoVariablesPlaceHolders.PROCESO_PRIVADA_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, ProcesosSeleccionPrivada);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.PROCESO_CERRADA_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, ProcesosSeleccionCerrada);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.PROCESO_ABIERTA_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, ProcesosSeleccionAbierta);
                        break;
                }
            }


            return pPlantilla;

        }

        private async Task<byte[]> ReplacePlantillaProcesosSeleccion(int pProcesoSeleccionId)
        {
            ProcesoSeleccion procesoSeleccion = await _context.ProcesoSeleccion
                .Where(r => r.ProcesoSeleccionId == pProcesoSeleccionId)
                .IncludeFilter(r => r.ProcesoSeleccionCronograma.Where(r => !(bool)r.Eliminado))
                .IncludeFilter(r => r.ProcesoSeleccionGrupo.Where(r => !(bool)r.Eliminado))
                //Aqui falta filtrarlos proponentes ya que en model y en codigo no de guarda eliminado
                .Include(r => r.ProcesoSeleccionProponente)
                .FirstOrDefaultAsync();

            if (procesoSeleccion == null)
            {
                return Array.Empty<byte>();
            }

            string TipoPlantilla = ((int)ConstanCodigoPlantillas.Ficha_De_Procesos_De_Seleccion).ToString();

            Plantilla Plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            Plantilla.Contenido = ReemplazarDatosPlantillaProcesosSeleccion(Plantilla.Contenido, procesoSeleccion);
            return ConvertirPDF(Plantilla);

        }

        public async Task<byte[]> GetPlantillaByTablaIdRegistroId(string pTablaId, int pRegistroId)
        {
            return pTablaId switch
            {
                ConstanCodigoTipoSolicitud.Contratacion => await ReplacePlantillaFichaContratacion(pRegistroId),
                ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion => await ReplacePlantillaProcesosSeleccion(pRegistroId),
                _ => Array.Empty<byte>(),
            };
        }

        public async Task<Respuesta> CreateEditSesionComiteTema(List<SesionComiteTema> ListSesionComiteTemas)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_Comite_Tema, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            try
            {
                foreach (var SesionComiteTema in ListSesionComiteTemas)
                {
                    if (SesionComiteTema.SesionTemaId == 0)
                    {
                        CreateEdit = "CREAR SESIÓN COMITE TEMA";
                        SesionComiteTema.FechaCreacion = DateTime.Now;
                        SesionComiteTema.UsuarioCreacion = ListSesionComiteTemas.FirstOrDefault().UsuarioCreacion;
                        SesionComiteTema.Eliminado = false;
                        _context.SesionComiteTema.Add(SesionComiteTema);
                    }
                    else
                    {
                        CreateEdit = "EDITAR SESIÓN COMITE TEMA";
                        SesionComiteTema sesionComiteTemaOld = _context.SesionComiteTema.Find(SesionComiteTema.SesionTemaId);
                        sesionComiteTemaOld.UsuarioModificacion = ListSesionComiteTemas.FirstOrDefault().UsuarioCreacion;
                        sesionComiteTemaOld.FechaModificacion = DateTime.Now;

                        sesionComiteTemaOld.Tema = SesionComiteTema.Tema;
                        sesionComiteTemaOld.ResponsableCodigo = SesionComiteTema.ResponsableCodigo;
                        sesionComiteTemaOld.TiempoIntervencion = SesionComiteTema.TiempoIntervencion;
                        sesionComiteTemaOld.RutaSoporte = SesionComiteTema.RutaSoporte;
                        sesionComiteTemaOld.Observaciones = SesionComiteTema.Observaciones;
                        sesionComiteTemaOld.EsAprobado = SesionComiteTema.EsAprobado;
                        sesionComiteTemaOld.ObservacionesDecision = SesionComiteTema.ObservacionesDecision;
                        sesionComiteTemaOld.ComiteTecnicoId = SesionComiteTema.ComiteTecnicoId;
                        //sesionComiteTemaOld.EsProposicionesVarios = SesionComiteTema.EsProposicionesVarios;
                    }
                    _context.SaveChanges();
                }

                return
                new Respuesta
                {
                    Data = await GetComiteTecnicoByComiteTecnicoId((int)ListSesionComiteTemas.FirstOrDefault().ComiteTecnicoId),
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteFiduciario.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, ListSesionComiteTemas.FirstOrDefault().UsuarioCreacion, CreateEdit)
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
                      Code = ConstantSesionComiteFiduciario.Error,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, ListSesionComiteTemas.FirstOrDefault().UsuarioCreacion, ex.InnerException.ToString())
                  };
            }

        }

        public async Task<Respuesta> AplazarSesionComite(ComiteTecnico pComiteTecnico, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Aplazar_Sesion_De_Comite_Fiduciario, (int)EnumeratorTipoDominio.Acciones);
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();

            try
            {
                ComiteTecnico comiteTecnicoOld = _context.ComiteTecnico.Find(pComiteTecnico.ComiteTecnicoId);
                comiteTecnicoOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                comiteTecnicoOld.FechaModificacion = DateTime.Now;
                comiteTecnicoOld.FechaOrdenDia = pComiteTecnico.FechaAplazamiento;
                comiteTecnicoOld.FechaAplazamiento = pComiteTecnico.FechaAplazamiento;
                comiteTecnicoOld.EstadoComiteCodigo = ConstanCodigoEstadoComite.Convocada;

                _context.SaveChanges();
                //Plantilla
                string TipoPlantilla = ((int)ConstanCodigoPlantillas.Aplazar_Comite_Tecnico).ToString();
                Plantilla plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).FirstOrDefault();

                List<Dominio> ListaParametricas = _context.Dominio.ToList();

                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {
                        case ConstanCodigoVariablesPlaceHolders.COMITE_NUMERO:
                            plantilla.Contenido = plantilla.Contenido.Replace(placeholderDominio.Nombre, comiteTecnicoOld.NumeroComite);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.COMITE_FECHA:
                            plantilla.Contenido = plantilla.Contenido.Replace(placeholderDominio.Nombre, comiteTecnicoOld.FechaCreacion.ToString("yyyy-MM-dd"));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.COMITE_FECHA_APLAZAMIENTO:
                            plantilla.Contenido = plantilla.Contenido.Replace(placeholderDominio.Nombre, ((DateTime)comiteTecnicoOld.FechaAplazamiento).ToString("yyyy-MM-dd"));
                            break;
                    }
                }


                //Notificar a los participantes 
                //TODO: esta lista debe ser parametrizada de acuerdo a los perfiles Directore de las 4 areas :
                //Director financiero, Director Juridico , Director técnico, y Director administrativo
                List<Usuario> ListMiembrosComite = await _commonService.GetUsuariosByPerfil((int)EnumeratorPerfil.Miembros_Comite);

                List<Usuario> UsuarioNoNotificados = new List<Usuario>();
                foreach (var Usuario in ListMiembrosComite)
                {
                    if (!string.IsNullOrEmpty(Usuario.Email))
                    {
                        if (!(bool)Helpers.Helpers.EnviarCorreo(Usuario.Email, "Aplazar sesión comité técnico", plantilla.Contenido, pSentender, pPassword, pMailServer, pMailPort))
                        {

                            UsuarioNoNotificados.Add(Usuario);
                        }
                    }
                }
                return
                  new Respuesta
                  {
                      Data = new List<dynamic>{
                         UsuarioNoNotificados
                      },
                      IsSuccessful = true,
                      IsException = false,
                      IsValidation = false,
                      Code = ConstantSesionComiteTecnico.OperacionExitosa,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pComiteTecnico.UsuarioCreacion, "APLAZAR SESIÓN COMITE")
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
                   Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
               };
            }

        }


        public async Task<List<SesionParticipante>> GetSesionParticipantesByIdComite(int pComiteId)
        {
            List<SesionParticipante> ListaParticipantes = new List<SesionParticipante>();
            try
            {

                ListaParticipantes = await _context.SesionParticipante
                .Where(r => r.ComiteTecnicoId == pComiteId && !(bool)r.Eliminado)
                .Include(r => r.SesionSolicitudObservacionProyecto)
                .ToListAsync();

                return ListaParticipantes;

            }
            catch (Exception)
            {
                return ListaParticipantes;
            }
        }

        public async Task<Respuesta> DeleteSesionInvitado(int pSesionInvitadoId, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Sesion_Invitado, (int)EnumeratorTipoDominio.Acciones);
            try
            {

                if (pSesionInvitadoId == 0)
                {
                    return
                     new Respuesta
                     {
                         IsSuccessful = false,
                         IsException = false,
                         IsValidation = true,
                         Code = ConstantSesionComiteTecnico.Error,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pUsuarioModificacion, "NO SE ENCONTRO SESION INVITADO")
                     };

                }
                SesionInvitado sesionInvitadoOld = await _context.SesionInvitado.FindAsync(pSesionInvitadoId);

                if (sesionInvitadoOld == null)
                {
                    return
                     new Respuesta
                     {
                         IsSuccessful = false,
                         IsException = false,
                         IsValidation = true,
                         Code = ConstantSesionComiteTecnico.Error,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pUsuarioModificacion, "NO SE ENCONTRO SESION INVITADO")
                     };
                }
                sesionInvitadoOld.UsuarioModificacion = pUsuarioModificacion;
                sesionInvitadoOld.FechaModificacion = DateTime.Now;
                sesionInvitadoOld.Eliminado = true;
                _context.SaveChanges();
                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pUsuarioModificacion, "ELIMINAR SESIÓN INVITADO")
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString())
                  };
            }

        }

        public async Task<Respuesta> CreateEditSesionInvitadoAndParticipante(ComiteTecnico pComiteTecnico)
        {
            int idAccionCrearSesionParticipante = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Participantes_Sesion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                foreach (var SesionParticipante in pComiteTecnico.SesionParticipante)
                {
                    if (SesionParticipante.SesionParticipanteId == 0)
                    {
                        _context.SesionParticipante.Add(new SesionParticipante
                        {
                            FechaCreacion = DateTime.Now,
                            Eliminado = false,
                            UsuarioCreacion = pComiteTecnico.UsuarioCreacion,
                            UsuarioId = SesionParticipante.UsuarioId,
                            ComiteTecnicoId = pComiteTecnico.ComiteTecnicoId,
                        });
                    }
                    else
                    {
                        SesionParticipante sesionParticipanteOld = _context.SesionParticipante.Find(SesionParticipante.SesionParticipanteId);
                        sesionParticipanteOld.UsuarioId = SesionParticipante.UsuarioId;
                        sesionParticipanteOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                        sesionParticipanteOld.FechaModificacion = DateTime.Now;
                    }
                }
                foreach (var SesionInvitado in pComiteTecnico.SesionInvitado)
                {

                    if (SesionInvitado.SesionInvitadoId == 0)
                    {
                        _context.SesionInvitado.Add(new SesionInvitado
                        {
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = pComiteTecnico.UsuarioCreacion,
                            Eliminado = false,
                            Cargo = SesionInvitado.Cargo,
                            ComiteTecnicoId = pComiteTecnico.ComiteTecnicoId,
                            Entidad = SesionInvitado.Entidad,
                            Nombre = SesionInvitado.Nombre,
                        });
                    }
                    else
                    {
                        SesionInvitado SesionInvitadoOld = _context.SesionInvitado.Find(SesionInvitado.SesionInvitadoId);
                        SesionInvitadoOld.FechaModificacion = DateTime.Now;
                        SesionInvitadoOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                        SesionInvitadoOld.Nombre = SesionInvitado.Nombre;
                        SesionInvitadoOld.Cargo = SesionInvitado.Cargo;
                        SesionInvitadoOld.Entidad = SesionInvitado.Entidad;
                    }
                }
                _context.SaveChanges();
                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccionCrearSesionParticipante, pComiteTecnico.UsuarioCreacion, "REGISTRAR PARTICIPANTES SESIÓN")
                    };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccionCrearSesionParticipante, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                };
            }

        }

        public async Task<Respuesta> GetNoRequiereVotacionSesionComiteSolicitud(SesionComiteSolicitud pSesionComiteSolicitud)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.No_Requiere_Votacion_Sesion_Comite_Solicitud, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SesionComiteSolicitud sesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(pSesionComiteSolicitud.SesionComiteSolicitudId);

                sesionComiteSolicitudOld.RequiereVotacionFiduciario = pSesionComiteSolicitud.RequiereVotacionFiduciario;
                sesionComiteSolicitudOld.UsuarioModificacion = pSesionComiteSolicitud.UsuarioCreacion;
                sesionComiteSolicitudOld.FechaModificacion = DateTime.Now;

                _context.SaveChanges();
                return

                new Respuesta
                {
                    //Data = await GetComiteTecnicoByComiteTecnicoId((int)pSesionComiteSolicitud.ComiteTecnicoId),
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pSesionComiteSolicitud.UsuarioCreacion, "NO REQUIERE VOTACIÓN")
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pSesionComiteSolicitud.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }

        }

        public async Task<Respuesta> NoRequiereVotacionSesionComiteTema(int idSesionComiteTema, bool pRequiereVotacion, string pUsuarioCreacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.No_Requiere_Votacion_Sesion_Comite_Tema, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                SesionComiteTema sesionComiteTemaOld = _context.SesionComiteTema.Find(idSesionComiteTema);
                sesionComiteTemaOld.RequiereVotacion = pRequiereVotacion;
                sesionComiteTemaOld.UsuarioModificacion = pUsuarioCreacion;
                sesionComiteTemaOld.FechaModificacion = DateTime.Now;

                _context.SaveChanges();
                return
                  new Respuesta
                  {
                      IsSuccessful = true,
                      IsException = false,
                      IsValidation = false,
                      Code = ConstantSesionComiteTecnico.OperacionExitosa,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pUsuarioCreacion, "NO REQUIERE VOTACIÓN SESIÓN COMITE TEMA")
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
                   Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pUsuarioCreacion, ex.InnerException.ToString())
               };
            }

        }

        public async Task<Respuesta> CreateEditSesionSolicitudVoto(SesionComiteSolicitud pSesionComiteSolicitud)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_Solicitud_Voto, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            try
            {
                SesionComiteSolicitud sesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(pSesionComiteSolicitud.SesionComiteSolicitudId);
                sesionComiteSolicitudOld.EstadoCodigo = pSesionComiteSolicitud.EstadoCodigo;
                sesionComiteSolicitudOld.RequiereVotacionFiduciario = true;
                sesionComiteSolicitudOld.UsuarioModificacion = pSesionComiteSolicitud.UsuarioCreacion;
                sesionComiteSolicitudOld.FechaModificacion = DateTime.Now;

                foreach (var sesionSolicitudVoto in pSesionComiteSolicitud.SesionSolicitudVoto)
                {
                    if (sesionSolicitudVoto.SesionSolicitudVotoId == 0)
                    {
                        CreateEdit = "CREAR SOLICITUD VOTO";
                        sesionSolicitudVoto.UsuarioCreacion = pSesionComiteSolicitud.UsuarioCreacion;
                        sesionSolicitudVoto.Eliminado = false;
                        sesionSolicitudVoto.FechaCreacion = DateTime.Now;
                        _context.SesionSolicitudVoto.Add(sesionSolicitudVoto);
                        //sesionComiteSolicitudOld.SesionSolicitudVoto.Add(sesionSolicitudVoto);
                    }
                    else
                    {
                        CreateEdit = "EDITAR SOLICITUD VOTO";
                        SesionSolicitudVoto sesionSolicitudVotoOld = _context.SesionSolicitudVoto.Find(sesionSolicitudVoto.SesionSolicitudVotoId);

                        sesionSolicitudVotoOld.UsuarioModificacion = pSesionComiteSolicitud.UsuarioCreacion;
                        sesionSolicitudVotoOld.FechaModificacion = DateTime.Now;

                        sesionSolicitudVotoOld.EsAprobado = sesionSolicitudVoto.EsAprobado;
                        sesionSolicitudVotoOld.Observacion = sesionSolicitudVoto.Observacion;
                    }
                }

                //
                foreach (var SesionSolicitudObservacionProyecto in pSesionComiteSolicitud.SesionSolicitudObservacionProyecto)
                {
                    if (SesionSolicitudObservacionProyecto.SesionSolicitudObservacionProyectoId == 0)
                    {
                        SesionSolicitudObservacionProyecto.UsuarioCreacion = pSesionComiteSolicitud.UsuarioCreacion;
                        SesionSolicitudObservacionProyecto.FechaCreacion = DateTime.Now;
                        SesionSolicitudObservacionProyecto.Eliminado = false;
                        _context.SesionSolicitudObservacionProyecto.Add(SesionSolicitudObservacionProyecto);
                        //sesionComiteSolicitudOld.SesionSolicitudObservacionProyecto.Add( SesionSolicitudObservacionProyecto );
                    }
                    else
                    {
                        SesionSolicitudObservacionProyecto SesionSolicitudObservacionProyectoOld = _context.SesionSolicitudObservacionProyecto.Find(SesionSolicitudObservacionProyecto.SesionSolicitudObservacionProyectoId);
                        SesionSolicitudObservacionProyectoOld.Observacion = SesionSolicitudObservacionProyecto.Observacion;
                        SesionSolicitudObservacionProyectoOld.UsuarioModificacion = pSesionComiteSolicitud.UsuarioCreacion;
                        SesionSolicitudObservacionProyectoOld.FechaModificacion = DateTime.Now;
                    }
                }
                _context.SaveChanges();
                return
                new Respuesta
                {
                    //Data = await GetComiteTecnicoByComiteTecnicoId((int)pSesionComiteSolicitud.ComiteTecnicoId),
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pSesionComiteSolicitud.UsuarioCreacion, CreateEdit)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pSesionComiteSolicitud.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }

        }

        public async Task<byte[]> GetPlantillaActaIdComite(int ComiteId)
        {
            if (ComiteId == 0)
            {
                return Array.Empty<byte>();
            }
            ComiteTecnico comiteTecnico = await _context.ComiteTecnico
                .Where(r => r.ComiteTecnicoId == ComiteId)
                    .Include(r => r.SesionComiteTema).FirstOrDefaultAsync();

            if (comiteTecnico == null)
            {
                return Array.Empty<byte>();
            }
            Plantilla plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Descargar_Acta).ToString()).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();

            plantilla.Contenido = ReemplazarDatosPlantillaActa(plantilla.Contenido, comiteTecnico);
            return ConvertirPDF(plantilla);
        }

        private string ReemplazarDatosPlantillaActa(string strContenido, ComiteTecnico pComiteTecnico)
        {
            List<Contratacion> ListContratacion =
                _context.Contratacion
                .Include(r => r.Contrato)
                .Include(r => r.Contratista)
                .Include(r => r.ContratacionProyecto)
                    .ThenInclude(r => r.Proyecto).ToList();

            List<Localizacion> localizacions = _context.Localizacion.ToList();
            List<Dominio> ListParametricas = _context.Dominio.Where(r => r.TipoDominioId != (int)EnumeratorTipoDominio.PlaceHolder).ToList();
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();

            List<Usuario> MiembrosParticipantes = _context.SesionParticipante
                .Where(r => r.ComiteTecnicoId == pComiteTecnico.ComiteTecnicoId)
                .Include(r => r.Usuario)
                .Select(r => r.Usuario)
                .ToList();

            List<SesionInvitado> ListInvitados = _context.SesionInvitado
                .Where(r => r.ComiteTecnicoId == pComiteTecnico.ComiteTecnicoId).ToList();
            //Tablas Dinamicas

            //Logica Invitados
            string PlantillaInvitados = _context.Plantilla
                .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Registros_Tabla_Invitados)
                   .ToString()).FirstOrDefault()
                .Contenido;

            string RegistrosInvitados = string.Empty;

            string PlantillaContratacion = _context.Plantilla
                .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Tabla_Solicitud_Contratacion)
                   .ToString()).FirstOrDefault()
                .Contenido;

            string PlantillaRegistrosProyectos = _context.Plantilla
                .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Registros_Tabla_proyectos)
                   .ToString()).FirstOrDefault()
                .Contenido;

            string RegistrosProyectos = string.Empty;

            string registrosContratacion = string.Empty;
            //Logica Orden Del Dia
            foreach (var SesionComiteSolicitud in pComiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
            {
                switch (SesionComiteSolicitud.TipoSolicitudCodigo)
                {
                    case ConstanCodigoTipoSolicitud.Contratacion:
                        Contratacion contratacion = ListContratacion.Where(r => r.ContratacionId == SesionComiteSolicitud.SolicitudId).FirstOrDefault();

                        foreach (Dominio placeholderDominio in placeholders)
                        {
                            switch (placeholderDominio.Codigo)
                            {
                                case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD_CONTRATACION:
                                    strContenido = strContenido
                                        .Replace(placeholderDominio.Nombre, SesionComiteSolicitud.NumeroSolicitud);
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD_CONTRATACION:
                                    strContenido = strContenido
                                        .Replace(placeholderDominio.Nombre, ((DateTime)SesionComiteSolicitud.FechaSolicitud).ToString("dd-MM-yyy"));
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TIPO_SOLICITUD_CONTRATACION:
                                    strContenido = strContenido
                                        .Replace(placeholderDominio.Nombre,
                                        ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud
                                        && r.Codigo == SesionComiteSolicitud.TipoSolicitudCodigo).FirstOrDefault().Nombre);
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TIPO_CONTRATO_CONTRATACION:
                                    strContenido = strContenido
                                        .Replace(placeholderDominio.Nombre,
                                        ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar
                                        && r.Codigo == contratacion.Contrato.FirstOrDefault().TipoContratoCodigo).FirstOrDefault().Nombre);
                                    break;


                                case ConstanCodigoVariablesPlaceHolders.REGISTROS_TABLA_PROYECTO:

                                    foreach (var ContratacionProyecto in contratacion.ContratacionProyecto)
                                    {
                                        RegistrosProyectos += PlantillaRegistrosProyectos;

                                        foreach (Dominio placeholderDominio2 in placeholders)
                                        {
                                            switch (placeholderDominio2.Codigo)
                                            {




                                            }
                                        }




                                    }
                                    strContenido = strContenido
                                        .Replace(placeholderDominio.Nombre,
                                        ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar
                                        && r.Codigo == contratacion.Contrato.FirstOrDefault().TipoContratoCodigo).FirstOrDefault().Nombre);
                                    break;
                            }
                        }

                        break;


                    case ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion:

                        break;



                    default:
                        break;
                }

            }

            foreach (var invitado in ListInvitados)
            {
                RegistrosInvitados += PlantillaInvitados;
                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {
                        case ConstanCodigoVariablesPlaceHolders.INVITADO_NOMBRE:
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, invitado.Nombre);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.INVITADO_CARGO:
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, invitado.Cargo);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.INVITADO_ENTIDAD:
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, invitado.Entidad);
                            break;
                    }
                }
            }




            foreach (Dominio placeholderDominio in placeholders)
            {
                switch (placeholderDominio.Codigo)
                {
                    //Tablas dinamicas

                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_TABLA_INVITADOS:
                        strContenido = strContenido
                            .Replace(placeholderDominio.Nombre, RegistrosInvitados);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.NUMERO_COMITE:
                        strContenido = strContenido
                            .Replace(placeholderDominio.Nombre, pComiteTecnico.NumeroComite);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.FECHA_COMITE:
                        strContenido = strContenido
                            .Replace(placeholderDominio.Nombre, ((DateTime)pComiteTecnico.FechaOrdenDia).ToString("dd-MM-yyyy"));
                        break;

                    case ConstanCodigoVariablesPlaceHolders.MIEMBROS_PARTICIPANTES:
                        string strUsuariosParticipantes = string.Empty;
                        MiembrosParticipantes.ForEach(user =>
                        {
                            strUsuariosParticipantes += user.Nombres + " " + user.Apellidos + " ";
                        });
                        strContenido = strContenido.Replace(placeholderDominio.Nombre, strUsuariosParticipantes);
                        break;


                }
            }


            return strContenido;
        }

        private bool validarcompletosActa(int pComiteTecnicoId)
        {
            bool estaCompleto = true;

            ComiteTecnico comite = _context.ComiteTecnico.Where(ct => ct.ComiteTecnicoId == pComiteTecnicoId)
                                                         .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                                                         .Include(r => r.SesionComiteTema)
                                                        .FirstOrDefault();

            comite.SesionComiteSolicitudComiteTecnicoFiduciario.ToList().ForEach(cs =>
            {
                if ((cs.RegistroCompletoFiduciaria.HasValue ? cs.RegistroCompletoFiduciaria.Value : false) == false)
                    estaCompleto = false;
            });

            comite.SesionComiteTema.ToList().ForEach(ct =>
            {
                if ((ct.RegistroCompleto.HasValue ? ct.RegistroCompleto.Value : false) == false)
                    estaCompleto = false;
            });

            if (estaCompleto)
            {
                comite.EstadoComiteCodigo = ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Enviada;
                comite.EsCompleto = true;
                _context.SaveChanges();
            }

            return estaCompleto;
        }

        public async Task<Respuesta> CreateEditActasSesionSolicitudCompromiso(SesionComiteSolicitud pSesionComiteSolicitud)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Edit_Sesion_Solicitud_Compromisos_ACTAS, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            try
            {
                SesionComiteSolicitud sesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(pSesionComiteSolicitud.SesionComiteSolicitudId);

                sesionComiteSolicitudOld.FechaModificacion = DateTime.Now;
                sesionComiteSolicitudOld.UsuarioModificacion = pSesionComiteSolicitud.UsuarioModificacion;
                sesionComiteSolicitudOld.EstadoCodigo = pSesionComiteSolicitud.EstadoCodigo;
                sesionComiteSolicitudOld.GeneraCompromisoFiduciario = pSesionComiteSolicitud.GeneraCompromisoFiduciario;
                sesionComiteSolicitudOld.DesarrolloSolicitudFiduciario = pSesionComiteSolicitud.DesarrolloSolicitudFiduciario;
                sesionComiteSolicitudOld.CantCompromisosFiduciario = pSesionComiteSolicitud.CantCompromisosFiduciario;
                sesionComiteSolicitudOld.ObservacionesFiduciario = pSesionComiteSolicitud.ObservacionesFiduciario;
                sesionComiteSolicitudOld.RutaSoporteVotacionFiduciario = pSesionComiteSolicitud.RutaSoporteVotacionFiduciario;
                sesionComiteSolicitudOld.RegistroCompletoFiduciaria = ValidarRegistroCompletoSesionComiteSolicitud(sesionComiteSolicitudOld);


                foreach (var SesionSolicitudCompromiso in pSesionComiteSolicitud.SesionSolicitudCompromiso)
                {
                    if (SesionSolicitudCompromiso.SesionSolicitudCompromisoId == 0)
                    {
                        CreateEdit = "CREAR SOLICITUD COMPROMISO";
                        SesionSolicitudCompromiso.UsuarioCreacion = pSesionComiteSolicitud.UsuarioCreacion;
                        SesionSolicitudCompromiso.FechaCreacion = DateTime.Now;
                        SesionSolicitudCompromiso.Eliminado = false;
                        SesionSolicitudCompromiso.EsFiduciario = true;

                        _context.SesionSolicitudCompromiso.Add(SesionSolicitudCompromiso);
                    }
                    else
                    {
                        CreateEdit = "EDITAR SOLICITUD COMPROMISO";
                        SesionSolicitudCompromiso sesionSolicitudCompromisoOld = _context.SesionSolicitudCompromiso.Find(SesionSolicitudCompromiso.SesionSolicitudCompromisoId);

                        SesionSolicitudCompromiso.FechaModificacion = SesionSolicitudCompromiso.FechaModificacion;
                        SesionSolicitudCompromiso.UsuarioModificacion = pSesionComiteSolicitud.UsuarioModificacion;

                        SesionSolicitudCompromiso.Tarea = SesionSolicitudCompromiso.Tarea;
                        SesionSolicitudCompromiso.FechaCumplimiento = SesionSolicitudCompromiso.FechaCumplimiento;

                        SesionSolicitudCompromiso.ResponsableSesionParticipanteId = SesionSolicitudCompromiso.ResponsableSesionParticipanteId;

                    }
                }
                _context.SaveChanges();
                return
                   new Respuesta
                   {
                       Data = validarcompletosActa(pSesionComiteSolicitud.ComiteTecnicoFiduciarioId.Value),
                       IsSuccessful = true,
                       IsException = false,
                       IsValidation = false,
                       Code = ConstantSesionComiteTecnico.OperacionExitosa,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pSesionComiteSolicitud.UsuarioCreacion, CreateEdit)
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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pSesionComiteSolicitud.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }

        public async Task<Respuesta> CreateEditTemasCompromiso(SesionComiteTema pSesionComiteTema)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Temas_Compromiso, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            try
            {
                SesionComiteTema SesionComiteTemadOld = _context.SesionComiteTema.Find(pSesionComiteTema.SesionTemaId);

                SesionComiteTemadOld.FechaModificacion = DateTime.Now;
                SesionComiteTemadOld.UsuarioModificacion = pSesionComiteTema.UsuarioCreacion;

                SesionComiteTemadOld.EstadoTemaCodigo = pSesionComiteTema.EstadoTemaCodigo;
                SesionComiteTemadOld.CantCompromisos = pSesionComiteTema.CantCompromisos;
                SesionComiteTemadOld.GeneraCompromiso = pSesionComiteTema.GeneraCompromiso;
                SesionComiteTemadOld.Observaciones = pSesionComiteTema.Observaciones;
                SesionComiteTemadOld.ObservacionesDecision = pSesionComiteTema.ObservacionesDecision;
                SesionComiteTemadOld.RegistroCompleto = ValidarRegistroCompletoSesionComiteTema(SesionComiteTemadOld);

                foreach (var TemaCompromiso in pSesionComiteTema.TemaCompromiso)
                {
                    if (TemaCompromiso.TemaCompromisoId == 0)
                    {
                        CreateEdit = "CREAR TEMA COMPROMISO";
                        TemaCompromiso.UsuarioCreacion = pSesionComiteTema.UsuarioCreacion;
                        TemaCompromiso.FechaCreacion = DateTime.Now;
                        TemaCompromiso.Eliminado = true;

                        _context.TemaCompromiso.Add(TemaCompromiso);
                    }
                    else
                    {
                        CreateEdit = "EDITAR TEMA COMPROMISO";
                        TemaCompromiso temaCompromisoOld = _context.TemaCompromiso.Find(TemaCompromiso.TemaCompromisoId);

                        temaCompromisoOld.Tarea = TemaCompromiso.Tarea;
                        temaCompromisoOld.Responsable = TemaCompromiso.Responsable;
                        temaCompromisoOld.FechaCumplimiento = TemaCompromiso.FechaCumplimiento;

                        temaCompromisoOld.FechaModificacion = TemaCompromiso.FechaModificacion;
                        temaCompromisoOld.UsuarioModificacion = TemaCompromiso.UsuarioModificacion;

                    }
                }
                _context.SaveChanges();
                return
                   new Respuesta
                   {
                       Data = validarcompletosActa(pSesionComiteTema.ComiteTecnicoId.Value),
                       IsSuccessful = true,
                       IsException = false,
                       IsValidation = false,
                       Code = ConstantSesionComiteTecnico.OperacionExitosa,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pSesionComiteTema.UsuarioCreacion, CreateEdit)
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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pSesionComiteTema.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }

        public async Task<Respuesta> CreateEditSesionTemaVoto(SesionComiteTema pSesionComiteTema)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Comite_Tema_Voto, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                SesionComiteTema sesionComiteTemaOld = _context.SesionComiteTema.Find(pSesionComiteTema.SesionTemaId);
                string CrearEditar = "";
                sesionComiteTemaOld.RequiereVotacion = true;
                sesionComiteTemaOld.EstadoTemaCodigo = pSesionComiteTema.EstadoTemaCodigo;
                sesionComiteTemaOld.EsAprobado = (pSesionComiteTema.EstadoTemaCodigo == "1") ? true : false;
                sesionComiteTemaOld.UsuarioModificacion = pSesionComiteTema.UsuarioCreacion;
                sesionComiteTemaOld.FechaModificacion = DateTime.Now;
                sesionComiteTemaOld.RegistroCompleto = ValidarRegistroCompletoSesionComiteTema(sesionComiteTemaOld);
                foreach (var SesionTemaVoto in pSesionComiteTema.SesionTemaVoto)
                {
                    if (SesionTemaVoto.SesionTemaVotoId == 0)
                    {
                        CrearEditar = "CREAR SESIÓN TEMA VOTO";
                        SesionTemaVoto.UsuarioCreacion = pSesionComiteTema.UsuarioCreacion;
                        SesionTemaVoto.FechaCreacion = DateTime.Now;
                        //SesionTemaVoto.Eliminado = false;
                        _context.SesionTemaVoto.Add(SesionTemaVoto);
                    }
                    else
                    {
                        CrearEditar = "EDITAR SESIÓN TEMA VOTO";
                        SesionTemaVoto SesionTemaVotoOld = _context.SesionTemaVoto.Find(SesionTemaVoto.SesionTemaVotoId);
                        //SesionTemaVotoOld.FechaModificacion = DateTime.Now;
                        //SesionTemaVotoOld.UsuarioModificacion = pSesionComiteTema.UsuarioCreacion;

                        SesionTemaVotoOld.EsAprobado = SesionTemaVoto.EsAprobado;
                        SesionTemaVotoOld.Observacion = SesionTemaVoto.Observacion;
                    }
                }
                _context.SaveChanges();
                return
                  new Respuesta
                  {
                      IsSuccessful = true,
                      IsException = false,
                      IsValidation = false,
                      Code = ConstantSesionComiteTecnico.OperacionExitosa,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pSesionComiteTema.UsuarioCreacion, CrearEditar)
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
                   Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pSesionComiteTema.UsuarioCreacion, ex.InnerException.ToString())
               };
            }

        }

        public async Task<ComiteTecnico> GetCompromisosByComiteTecnicoId(int ComiteTecnicoId)
        {
            //Dominio estado reportado 48 
            ComiteTecnico comiteTecnico = await _context.ComiteTecnico.Where(r => r.ComiteTecnicoId == ComiteTecnicoId)
                .Include(r => r.SesionComiteTema)
                   .ThenInclude(r => r.TemaCompromiso)
               .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                   .ThenInclude(r => r.SesionSolicitudCompromiso)
                      .FirstOrDefaultAsync();
            


            List<Dominio> ListEstadoReportado = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Compromisos).ToList();
            List<SesionParticipante> ListSesionParticipantes = _context.SesionParticipante.Where(r => !(bool)r.Eliminado).Include(r => r.Usuario).ToList();
            comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario.ToList().ForEach( s => {
                s.SesionSolicitudCompromiso = s.SesionSolicitudCompromiso.Where( c => c.EsFiduciario == true ).ToList();
            } );
                
            comiteTecnico.NumeroCompromisos = 0;
            foreach (var SesionComiteTema in comiteTecnico.SesionComiteTema)
            {
                foreach (var TemaCompromiso in SesionComiteTema.TemaCompromiso)
                {
                    if (!string.IsNullOrEmpty(TemaCompromiso.EstadoCodigo))
                    {
                        TemaCompromiso.EstadoCodigo = ListEstadoReportado.Where(r => r.Codigo == TemaCompromiso.EstadoCodigo).FirstOrDefault().Nombre;
                    }
                    if (TemaCompromiso.Responsable != null)
                    {
                        TemaCompromiso.ResponsableNavigation = ListSesionParticipantes.Where(r => r.SesionParticipanteId == TemaCompromiso.Responsable).FirstOrDefault();
                    }
                    if (TemaCompromiso.EstadoCodigo == "3")
                        comiteTecnico.NumeroCompromisosCumplidos++;
                    comiteTecnico.NumeroCompromisos++;
                }
            }

            foreach (var SesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
            {
                foreach (var SesionSolicitudCompromiso in SesionComiteSolicitud.SesionSolicitudCompromiso.Where( c => c.EsFiduciario == true ))
                {
                    if (!string.IsNullOrEmpty(SesionSolicitudCompromiso.EstadoCodigo))
                    {
                        SesionSolicitudCompromiso.EstadoCodigo = ListEstadoReportado.Where(r => r.Codigo == SesionSolicitudCompromiso.EstadoCodigo).FirstOrDefault().Nombre;
                    }

                    if (SesionSolicitudCompromiso.ResponsableSesionParticipanteId != null)
                    {
                        SesionSolicitudCompromiso.ResponsableSesionParticipante = ListSesionParticipantes.Where(r => r.SesionParticipanteId == SesionSolicitudCompromiso.ResponsableSesionParticipanteId).FirstOrDefault();
                    }
                    if (SesionSolicitudCompromiso.EstadoCodigo == "3")
                        comiteTecnico.NumeroCompromisosCumplidos++;
                    comiteTecnico.NumeroCompromisos++;
                }
            }



            return comiteTecnico;
        }

        public async Task<Respuesta> VerificarTemasCompromisos(ComiteTecnico pComiteTecnico)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Vertificar_Tema_Compromisos, (int)EnumeratorTipoDominio.Acciones);

            try
            {

                foreach (var SesionComiteTema in pComiteTecnico.SesionComiteTema)
                {

                    foreach (var temaCompromiso in SesionComiteTema.TemaCompromiso)
                    {
                        TemaCompromiso temaCompromisoOld = _context.TemaCompromiso.Find(temaCompromiso.TemaCompromisoId);
                        temaCompromisoOld.FechaModificacion = DateTime.Now;
                        temaCompromisoOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                        temaCompromisoOld.EstadoCodigo = temaCompromiso.EstadoCodigo;
                    }
                }

                foreach (var SesionComiteSolicitud in pComiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
                {

                    foreach (var pSesionSolicitudCompromiso in SesionComiteSolicitud.SesionSolicitudCompromiso)
                    {
                        SesionSolicitudCompromiso SesionSolicitudCompromisoOld = _context.SesionSolicitudCompromiso.Find(pSesionSolicitudCompromiso.SesionSolicitudCompromisoId);
                        SesionSolicitudCompromisoOld.FechaModificacion = DateTime.Now;
                        SesionSolicitudCompromisoOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                        SesionSolicitudCompromisoOld.EstadoCodigo = pSesionSolicitudCompromiso.EstadoCodigo;
                    }
                }


                _context.SaveChanges();
                return
                   new Respuesta
                   {
                       IsSuccessful = true,
                       IsException = false,
                       IsValidation = false,
                       Code = ConstantSesionComiteTecnico.OperacionExitosa,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pComiteTecnico.UsuarioCreacion, "VERIFICAR TEMA COMPROMISO")
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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }

        // //Crear un nuevo tema
        // public async Task<Respuesta> CreateOrEditTema(SesionComiteTema sesionComiteTema, DateTime fechaComite)
        // {
        //     Respuesta respuesta = new Respuesta();
        //     int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_Comite_Tema, (int)EnumeratorTipoDominio.Acciones);

        //     string strCrearEditar = string.Empty;
        //     SesionComiteTema sesionComiteTemaAntiguo = null;
        //     var newComiteTecnicoId = 0;
        //     try
        //     {

        //         if (string.IsNullOrEmpty(sesionComiteTema.SesionTemaId.ToString()) || sesionComiteTema.SesionTemaId == 0)
        //         {

        //             //Auditoria
        //             strCrearEditar = "CREAR COMITE TECNICO FIDUCIARIO@#CREAR NUEVO TEMA";

        //             //Crear Comite tecnico fiduciario inicial
        //             int countMaxId = _context.ComiteTecnico.Max(cm => cm.ComiteTecnicoId);

        //             ComiteTecnico comiteTecnico = new ComiteTecnico();
        //             comiteTecnico.FechaCreacion = fechaComite;
        //             comiteTecnico.UsuarioCreacion = sesionComiteTema.UsuarioCreacion;
        //             comiteTecnico.EsComiteFiduciario = true;
        //             comiteTecnico.EstadoComiteCodigo = "1";
        //             comiteTecnico.EsCompleto = false;
        //             comiteTecnico.EsAprobado = false;
        //             comiteTecnico.Eliminado = false;
        //             comiteTecnico.NumeroComite = Helpers.Helpers.Consecutive("CF", countMaxId);

        //             _context.ComiteTecnico.Add(comiteTecnico);
        //             var result = await _context.SaveChangesAsync();

        //             if (result > 0)
        //             {
        //                 newComiteTecnicoId = comiteTecnico.ComiteTecnicoId;
        //                 sesionComiteTema.ComiteTecnicoId = newComiteTecnicoId;
        //                 sesionComiteTema.FechaCreacion = DateTime.Now;
        //                 sesionComiteTema.UsuarioCreacion = sesionComiteTema.UsuarioCreacion;
        //                 sesionComiteTema.Eliminado = false;

        //                 _context.SesionComiteTema.Add(sesionComiteTema);
        //             }

        //         }
        //         else
        //         {
        //             strCrearEditar = "EDIT TEMA";
        //             sesionComiteTemaAntiguo = _context.SesionComiteTema.Find(sesionComiteTema.SesionTemaId);

        //             //Auditoria
        //             sesionComiteTemaAntiguo.UsuarioModificacion = sesionComiteTema.UsuarioModificacion;
        //             sesionComiteTemaAntiguo.Eliminado = false;


        //             //Registros
        //             sesionComiteTemaAntiguo.Tema = sesionComiteTema.Tema;
        //             sesionComiteTemaAntiguo.ResponsableCodigo = sesionComiteTema.ResponsableCodigo;
        //             sesionComiteTemaAntiguo.TiempoIntervencion = sesionComiteTema.TiempoIntervencion;
        //             _context.SesionComiteTema.Add(sesionComiteTema);

        //         }

        //         return respuesta = new Respuesta
        //         {
        //             IsSuccessful = true,
        //             IsException = false,
        //             IsValidation = false,
        //             Data = sesionComiteTema,
        //             Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
        //             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, sesionComiteTema.UsuarioCreacion, strCrearEditar)

        //         };
        //     }

        //     catch (Exception ex)
        //     {
        //         return respuesta = new Respuesta
        //         {
        //             IsSuccessful = false,
        //             IsException = true,
        //             IsValidation = false,
        //             Data = null,
        //             Code = ConstantMessagesSesionComiteTema.Error,
        //             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, sesionComiteTema.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
        //         };
        //     }

        // }


        // //Ver detalle grilla comite tecnico
        // public async Task<List<SesionComiteTema>> GetCommitteeSessionByComiteTecnicoId(int comiteTecnicoId)
        // {
        //     try
        //     {
        //         return await _context.SesionComiteTema.Include(st => st.ComiteTecnico).Where(cm => !(bool)cm.Eliminado && cm.ComiteTecnicoId == comiteTecnicoId).ToListAsync();
        //     }
        //     catch (Exception)
        //     {
        //         throw;
        //     }
        // }





        // //Grilla vefificar complimiento de compromisos
        // public async Task<List<GridComiteTecnicoCompromiso>> GetCompromisosSolicitud()
        // {
        //     try
        //     {
        //         return await (from n in _context.SesionComiteTecnicoCompromiso
        //                       where !(bool)n.Eliminado
        //                       select new GridComiteTecnicoCompromiso
        //                       {
        //                           Tarea = n.Tarea,
        //                           Responzable = n.Responsable,
        //                           FechaCumplimiento = n.FechaCumplimiento,
        //                           FechaReporte = n.FechaCreacion,
        //                           EstadoReporte = n.EstadoCodigo
        //                       }).OrderByDescending(n => n.FechaCumplimiento).ToListAsync();
        //     }
        //     catch (Exception)
        //     {

        //         throw;
        //     }
        // }

        // //Convocar sesión de comité
        // public async Task<Respuesta> CallCommitteeSession(int comiteTecnicoId, string user)
        // {
        //     Respuesta respuesta = new Respuesta();
        //     int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Convocar_Sesion_Comite, (int)EnumeratorTipoDominio.Acciones);

        //     string strCrearEditar = string.Empty;
        //     ComiteTecnico comiteTecnico = null;
        //     try
        //     {


        //         strCrearEditar = "CONVOCAR SESION COMITE";
        //         comiteTecnico = await _context.ComiteTecnico.FindAsync(comiteTecnicoId);

        //         //Auditoria
        //         comiteTecnico.UsuarioModificacion = user;
        //         comiteTecnico.FechaModificacion = DateTime.Now;


        //         //Registros
        //         comiteTecnico.EstadoComiteCodigo = "2";
        //         _context.ComiteTecnico.Update(comiteTecnico);
        //         var result = await _context.SaveChangesAsync();
        //         if (result > 0)
        //         {
        //             //Enviar notificacion
        //         }


        //         return respuesta = new Respuesta
        //         {
        //             IsSuccessful = true,
        //             IsException = false,
        //             IsValidation = false,
        //             Data = comiteTecnico,
        //             Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
        //             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, user, strCrearEditar)

        //         };
        //     }

        //     catch (Exception ex)
        //     {
        //         return respuesta = new Respuesta
        //         {
        //             IsSuccessful = false,
        //             IsException = true,
        //             IsValidation = false,
        //             Data = null,
        //             Code = ConstantMessagesSesionComiteTema.Error,
        //             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, user, ex.InnerException.ToString().Substring(0, 500))
        //         };
        //     }

        // }



        // //Eliminar Tema
        // public async Task<bool> DeleteTema(int sesionTemaId, string user)
        // {
        //     SesionComiteTema sesionComiteTema = await _context.SesionComiteTema.FindAsync(sesionTemaId);
        //     bool status = false;
        //     try
        //     {
        //         status = sesionComiteTema.GeneraCompromiso.HasValue ? sesionComiteTema.GeneraCompromiso.Value : false;
        //         Console.Write(status);
        //         if (!status)
        //         {
        //             // Se puede eliminar
        //             sesionComiteTema.Eliminado = true;
        //             sesionComiteTema.UsuarioCreacion = user;
        //             sesionComiteTema.FechaModificacion = DateTime.Now;

        //             _context.Remove(sesionComiteTema);
        //             var resultado = await _context.SaveChangesAsync();
        //             if (resultado > 0)
        //                 return true;
        //         }

        //         return false;

        //     }
        //     catch (Exception ex)
        //     {
        //         return false;
        //     }

        // }



        #endregion



        // #region "SESIONES DE COMITE FIDUCIARIO";

        // //Grilla sesiones programadas en estado Convocada.
        // public async Task<List<ComiteTecnico>> GetConvokeSessionFiduciario(int? estadoComiteCodigo)
        // {
        //     try
        //     {
        //         if (estadoComiteCodigo != null)
        //             return await _context.ComiteTecnico.Include(st => st.SesionComiteTema).Where(cm => !(bool)cm.Eliminado && cm.EstadoComiteCodigo == Convert.ToString(estadoComiteCodigo) && cm.EsComiteFiduciario == true).ToListAsync();
        //         else
        //             return await _context.ComiteTecnico.Include(st => st.SesionComiteTema).Where(cm => !(bool)cm.Eliminado && cm.EsComiteFiduciario == true).ToListAsync();

        //     }
        //     catch (Exception)
        //     {

        //         throw;
        //     }
        // }


        // //Grilla validacion de solicitudes contractuales
        // public async Task<List<GridValidationRequests>> GetValidationRequests()
        // {
        //     //sc.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion
        //     return await
        //         (from sc in _context.SesionComiteSolicitud
        //          join ct in _context.ComiteTecnico on sc.ComiteTecnicoId equals ct.ComiteTecnicoId
        //          join ps in _context.ProcesoSeleccion on sc.SolicitudId equals ps.SolicitudId
        //          where !(bool)ct.Eliminado && !(bool)ps.Eliminado && !(bool)sc.Eliminado

        //          select new GridValidationRequests
        //          {
        //              ComiteTecnicoId = ct.ComiteTecnicoId,
        //              SesionComiteSolicitudId = sc.SesionComiteSolicitudId,
        //              FechaSolicitud = ps.FechaCreacion,
        //              NumeroSolicitud = ps.NumeroProceso,
        //              ProcesoSeleccionId = ps.ProcesoSeleccionId,
        //              TipoSolicitudCodigo = sc.TipoSolicitudCodigo,
        //              TipoSolicitudText = _context.Dominio.Where(r => (bool)r.Activo && r.DominioId.Equals(sc.TipoSolicitudCodigo) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud).Select(r => r.Nombre).FirstOrDefault(),
        //              FechaComiteTecnico = ct.FechaCreacion,
        //              NumeroComite = ct.NumeroComite,
        //              TemaRequiereVotacion = sc.RequiereVotacion,
        //              sesionParticipanteVoto = (List<SesionParticipanteVoto>)(from v in _context.SesionParticipanteVoto
        //                                                                      where v.ComiteTecnicoId == ct.ComiteTecnicoId
        //                                                                      select new SesionParticipanteVoto
        //                                                                      {
        //                                                                          SesionParticipanteVotoId = v.SesionParticipanteId,
        //                                                                          SesionParticipante =
        //                                                                          (SesionParticipante)(from sp in _context.SesionParticipante
        //                                                                                               where sp.ComiteTecnicoId == v.ComiteTecnicoId && !(bool)sp.Eliminado
        //                                                                                               select new SesionParticipante
        //                                                                                               {
        //                                                                                                   ComiteTecnicoId = sp.ComiteTecnicoId,
        //                                                                                                   UsuarioId = sp.UsuarioId,
        //                                                                                                   Nombres = _context.Usuario.Where(u => (bool)u.Activo && u.UsuarioId.Equals(sp.UsuarioId)).Select(u => string.Concat(u.Nombres, " ", u.Apellidos)).FirstOrDefault(),
        //                                                                                               }),
        //                                                                          SesionParticipanteId = v.SesionParticipanteId,
        //                                                                          EsAprobado = v.EsAprobado,
        //                                                                          Observaciones = v.Observaciones,
        //                                                                          ObservacionesDevolucion = v.ObservacionesDevolucion

        //                                                                      })


        //          }).ToListAsync();

        // }

        // public async Task<Respuesta> CreateOrEditVotacionSolicitud(List<SesionSolicitudVoto> listSolicitudVoto)
        // {

        //     Respuesta respuesta = new Respuesta();
        //     int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Votacion_Solicitud_Participante, (int)EnumeratorTipoDominio.Acciones);

        //     string strCrearEditar = string.Empty;
        //     SesionSolicitudVoto solicitudVoto = null; ;

        //     try
        //     {



        //         //Auditoria
        //         strCrearEditar = "VOTACION SOLICITUD PARTICIPANTE";
        //         if (listSolicitudVoto.Count > 0)
        //         {

        //             foreach (var list in listSolicitudVoto)
        //             {

        //                 if (string.IsNullOrEmpty(list.SesionSolicitudVotoId.ToString()) || list.SesionSolicitudVotoId == 0)
        //                 {
        //                     listSolicitudVoto = new List<SesionSolicitudVoto>();
        //                     solicitudVoto.SesionComiteSolicitudId = list.SesionComiteSolicitudId;
        //                     solicitudVoto.SesionParticipanteId = list.SesionParticipanteId;
        //                     solicitudVoto.EsAprobado = list.EsAprobado;
        //                     solicitudVoto.Observacion = list.Observacion;


        //                     solicitudVoto.FechaCreacion = DateTime.Now;
        //                     solicitudVoto.UsuarioCreacion = list.UsuarioCreacion;
        //                     _context.SesionSolicitudVoto.Add(solicitudVoto);
        //                 }
        //             }

        //         }




        //         return respuesta = new Respuesta
        //         {
        //             IsSuccessful = true,
        //             IsException = false,
        //             IsValidation = false,
        //             Data = listSolicitudVoto,
        //             Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
        //             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, listSolicitudVoto.FirstOrDefault().UsuarioCreacion, strCrearEditar)

        //         };
        //     }

        //     catch (Exception ex)
        //     {
        //         return respuesta = new Respuesta
        //         {
        //             IsSuccessful = false,
        //             IsException = true,
        //             IsValidation = false,
        //             Data = null,
        //             Code = ConstantMessagesSesionComiteTema.Error,
        //             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, listSolicitudVoto.FirstOrDefault().UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
        //         };
        //     }
        // }
        // //Ver soporte
        // public async Task<List<SesionComiteSolicitud>> StartDownloadResumenFichaSolicitud()
        // {

        //     try
        //     {
        //         return null;

        //     }
        //     catch (Exception)
        //     {

        //         throw;
        //     }
        // }


        // //Lista participantes
        // public async Task<List<Usuario>> GetListParticipantes()
        // {

        //     try
        //     {
        //         return await (from u in _context.Usuario
        //                       where u.Eliminado == false && u.Activo == true
        //                       select new Usuario
        //                       {
        //                           UsuarioId = u.UsuarioId,
        //                           Email = u.Email,
        //                           Nombres = string.Concat(u.Nombres, " ", u.Apellidos)
        //                       }).ToListAsync();

        //     }
        //     catch (Exception)
        //     {

        //         throw;
        //     }
        // }


        // //Registrar Mienbros invitados.
        // public async Task<Respuesta> CreateOrEditInvitedMembers(SesionParticipante sesionParticipante)
        // {
        //     Respuesta respuesta = new Respuesta();
        //     int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Mienbros_Invitados, (int)EnumeratorTipoDominio.Acciones);

        //     string strCrearEditar = string.Empty;
        //     SesionParticipante _sesionParticipante = null;

        //     try
        //     {

        //         if (string.IsNullOrEmpty(sesionParticipante.SesionParticipanteId.ToString()) || sesionParticipante.SesionParticipanteId == 0)
        //         {

        //             //Auditoria
        //             strCrearEditar = "CREAR MIENBROS INVITADOS";
        //             // if (sesionParticipante.UsersIds.Count > 0)
        //             // {
        //             //     foreach (var list in sesionParticipante.UsersIds)
        //             //     {
        //             //         _sesionParticipante = new SesionParticipante();
        //             //         _sesionParticipante.ComiteTecnicoId = sesionParticipante.ComiteTecnicoId;
        //             //         _sesionParticipante.UsuarioId = list.UsuarioId;
        //             //         _sesionParticipante.FechaCreacion = DateTime.Now;
        //             //         _sesionParticipante.UsuarioCreacion = sesionParticipante.UsuarioCreacion;
        //             //         _sesionParticipante.Eliminado = false;
        //             //         _context.SesionParticipante.Add(_sesionParticipante);
        //             //     }
        //             // }
        //         }


        //         return respuesta = new Respuesta
        //         {
        //             IsSuccessful = true,
        //             IsException = false,
        //             IsValidation = false,
        //             Data = _sesionParticipante,
        //             Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
        //             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, sesionParticipante.UsuarioCreacion, strCrearEditar)

        //         };
        //     }

        //     catch (Exception ex)
        //     {
        //         return respuesta = new Respuesta
        //         {
        //             IsSuccessful = false,
        //             IsException = true,
        //             IsValidation = false,
        //             Data = null,
        //             Code = ConstantMessagesSesionComiteTema.Error,
        //             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, sesionParticipante.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
        //         };
        //     }

        // }




        // #endregion

        // #region "Gestion de actas";
        // //Grilla sesion comite, => sesiones desarrolladas sin actas.
        // public async Task<List<ComiteTecnico>> GetSesionSinActa()
        // {
        //     try
        //     {
        //         return await (from n in _context.ComiteTecnico
        //                       where n.EstadoComiteCodigo == "3" && (bool)!n.Eliminado
        //                       select new ComiteTecnico
        //                       {
        //                           ComiteTecnicoId = n.ComiteTecnicoId,
        //                           NumeroComite = n.NumeroComite,
        //                           FechaOrdenDia = n.FechaOrdenDia,
        //                           //TipoDominioId = 38 -> Estado comite
        //                           EstadoComiteCodigo = n.EstadoComiteCodigo == "3" ? "Sin acta" : n.EstadoComiteCodigo == "4" ? "En proceso de aprobación" : n.EstadoComiteCodigo == "5" ? "Aprobada" : "Devuelta",
        //                           EsCompleto = n.EsCompleto
        //                       }).OrderByDescending(s => s.FechaOrdenDia).ToListAsync();


        //     }

        //     catch (Exception)
        //     {

        //         throw;
        //     }
        // }



        // #endregion



        // #region "Sesion";


        // //Grilla sesion comite
        // //public async Task<ActionResult<IEnumerable<GridCommitteeSession>>> GetCommitteeSession(int? comiteTecnicoId)
        // //{

        // //    List<ComiteTecnico> ListSesion = (comiteTecnicoId != null ? await _context.ComiteTecnico.Where(s => s.ComiteTecnicoId == comiteTecnicoId && (bool)!s.Eliminado).ToListAsync() : await _context.ComiteTecnico.Where(s => (bool)!s.Eliminado).ToListAsync());

        // //    List<GridCommitteeSession> ListGridCommitteeSession = new List<GridCommitteeSession>();


        // //    foreach (var ss in ListSesion)
        // //    {
        // //        GridCommitteeSession SesionGrid = new GridCommitteeSession
        // //        {
        // //            ComiteTecnicoId = ss.ComiteTecnicoId,
        // //            FechaDeComite = ss.FechaOrdenDia,
        // //            EstadoComiteCodigo = ss.EstadoComiteCodigo,
        // //            NumeroComite = ss.NumeroComite,
        // //            EstadoComiteText = ss.EstadoComiteCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(ss.EstadoComiteCodigo, (int)EnumeratorTipoDominio.Estado_Comite) : "",
        // //        };
        // //        ListGridCommitteeSession.Add(SesionGrid);
        // //    }

        // //    return ListGridCommitteeSession;
        // //}


        // // Ver detalle
        // public async Task<IEnumerable<GridCommitteeSession>> GetCommitteeSessionTemaById(int sessionTemaId)
        // {

        //     List<SesionComiteTema> ListSesionComiteTema = await _context.SesionComiteTema.Where(s => s.ComiteTecnicoId == sessionTemaId && (bool)!s.Eliminado).ToListAsync();

        //     List<GridCommitteeSession> ListGridsesionComiteTema = new List<GridCommitteeSession>();


        //     foreach (var st in ListSesionComiteTema)
        //     {
        //         GridCommitteeSession sesionComiteTemaGrid = new GridCommitteeSession
        //         {
        //             ComiteTecnicoId = st.ComiteTecnicoId,
        //             SesionComiteTemaId = st.SesionTemaId,
        //             Tema = st.Tema,
        //             Responzable = st.ResponsableCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(st.ResponsableCodigo, (int)EnumeratorTipoDominio.Estado_Comite) : "",
        //             TiempoIntervencion = st.TiempoIntervencion,
        //             UrlSoporte = st.RutaSoporte,
        //             Observaciones = st.Observaciones,
        //             ObservacionesDecision = st.ObservacionesDecision
        //         };

        //         ListGridsesionComiteTema.Add(sesionComiteTemaGrid);
        //     }

        //     return ListGridsesionComiteTema;
        // }







        // //Aplazar sesion
        // public async Task<bool> SessionPostpone(int ComiteTecnicoId, DateTime newDate, string usuarioModifico)
        // {

        //     try
        //     {
        //         int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_Comite_Tema, (int)EnumeratorTipoDominio.Acciones);
        //         if (string.IsNullOrEmpty(Convert.ToString(ComiteTecnicoId)) || newDate != null)
        //         {

        //             var comiteTecnico = await _context.ComiteTecnico.FindAsync(ComiteTecnicoId);
        //             if (comiteTecnico == null)
        //             {
        //                 throw new Exception("No se encontro la sesion");
        //             }

        //             comiteTecnico.FechaOrdenDia = newDate;
        //             comiteTecnico.EstadoComiteCodigo = "6"; // Aplazada. TipoDominioId = 38, 
        //             comiteTecnico.UsuarioModificacion = usuarioModifico;
        //             _context.ComiteTecnico.Update(comiteTecnico);

        //             var resultado = await _context.SaveChangesAsync();
        //             if (resultado > 0) //TODO: Enviar notificación a los miembros del comite indicando la nueva fecha se sesion.
        //                 return true;
        //             else
        //                 return false;
        //         }

        //         return false;
        //     }

        //     catch (Exception)
        //     {
        //         return false;
        //     }
        // }



        // //Declarar fallida
        // public async Task<bool> SessionDeclaredFailed(int ComiteTecnicoId, string usuarioModifico)
        // {

        //     try
        //     {
        //         int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_Comite_Tema, (int)EnumeratorTipoDominio.Acciones);
        //         if (string.IsNullOrEmpty(Convert.ToString(ComiteTecnicoId)))
        //         {

        //             var sesion = await _context.ComiteTecnico.FindAsync(ComiteTecnicoId);
        //             if (sesion == null)
        //             {
        //                 throw new Exception("No se encontro la sesion");
        //             }
        //             sesion.EstadoComiteCodigo = "7"; // Fallida. TipoDominioId = 38, 
        //             sesion.UsuarioModificacion = usuarioModifico;
        //             _context.ComiteTecnico.Update(sesion);

        //             var resultado = await _context.SaveChangesAsync();
        //             if (resultado > 0)
        //                 return true;
        //             else
        //                 return false;
        //         }

        //         return false;
        //     }

        //     catch (Exception)
        //     {
        //         return false;
        //     }
        // }



        // //Registrar invitado
        // public async Task<Respuesta> CreateOrEditGuest(SesionInvitado sesionInvitado)
        // {
        //     Respuesta respuesta = new Respuesta();
        //     int idAccion = 0; //await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_invitado, (int)EnumeratorTipoDominio.Acciones);

        //     string strCrearEditar = string.Empty;
        //     SesionInvitado sesionInvitadoAntiguo = null;
        //     try
        //     {

        //         if (string.IsNullOrEmpty(sesionInvitado.SesionInvitadoId.ToString()) || sesionInvitado.SesionInvitadoId == 0)
        //         {
        //             //Auditoria
        //             strCrearEditar = "CREAR SESION COMITE INVITADO";
        //             sesionInvitado.FechaCreacion = DateTime.Now;
        //             sesionInvitado.Eliminado = false;
        //             _context.SesionInvitado.Add(sesionInvitado);
        //         }
        //         else
        //         {
        //             strCrearEditar = "EDIT SESION COMITE INVITADO";
        //             sesionInvitadoAntiguo = _context.SesionInvitado.Find(sesionInvitado.SesionInvitadoId);
        //             //Auditoria
        //             sesionInvitadoAntiguo.UsuarioModificacion = sesionInvitado.UsuarioModificacion;
        //             sesionInvitadoAntiguo.FechaModificacion = DateTime.Now;
        //             sesionInvitadoAntiguo.Eliminado = false;


        //             //Registros
        //             sesionInvitadoAntiguo.SesionInvitadoId = sesionInvitado.SesionInvitadoId;
        //             sesionInvitadoAntiguo.Nombre = sesionInvitado.Nombre;
        //             sesionInvitadoAntiguo.Cargo = sesionInvitado.Cargo;
        //             sesionInvitadoAntiguo.Entidad = sesionInvitado.Entidad;

        //             _context.SesionInvitado.Update(sesionInvitadoAntiguo);

        //         }

        //         return respuesta = new Respuesta
        //         {
        //             IsSuccessful = true,
        //             IsException = false,
        //             IsValidation = false,
        //             Data = sesionInvitado,
        //             Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
        //             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, sesionInvitado.UsuarioCreacion, strCrearEditar)

        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         return respuesta = new Respuesta
        //         {
        //             IsSuccessful = false,
        //             IsException = true,
        //             IsValidation = false,
        //             Data = null,
        //             Code = ConstantMessagesSesionComiteTema.Error,
        //             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, sesionInvitado.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
        //         };
        //     }

        // }



        // //Registrar Sesion Comentario ->  // No guarda desde este caso de uso
        // public async Task<Respuesta> CreateOrEditSesioncomment(SesionComentario sesionComentario)
        // {
        //     Respuesta respuesta = new Respuesta();
        //     int idAccion = 0; //await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_comentario, (int)EnumeratorTipoDominio.Acciones);

        //     string strCrearEditar = string.Empty;
        //     SesionComentario sesionComentarioAntiguo = null;
        //     try
        //     {

        //         if (string.IsNullOrEmpty(sesionComentario.SesionComentarioId.ToString()) || sesionComentario.SesionComentarioId == 0)
        //         {
        //             //Auditoria
        //             strCrearEditar = "CREAR SESION COMENTARIO";
        //             sesionComentario.FechaCreacion = DateTime.Now;
        //             _context.SesionComentario.Add(sesionComentario);
        //         }
        //         else
        //         {
        //             strCrearEditar = "EDIT SESION COMENTARIO";
        //             sesionComentarioAntiguo = _context.SesionComentario.Find(sesionComentario.SesionComentarioId);
        //             //Auditoria
        //             sesionComentarioAntiguo.UsuarioModificacion = sesionComentario.UsuarioModificacion;
        //             sesionComentarioAntiguo.FechaModificacion = DateTime.Now;


        //             //Registros
        //             sesionComentarioAntiguo.ComiteTecnicoId = sesionComentario.ComiteTecnicoId;
        //             sesionComentarioAntiguo.Fecha = sesionComentario.Fecha;
        //             sesionComentarioAntiguo.MiembroSesionParticipante = sesionComentario.MiembroSesionParticipante;
        //             sesionComentarioAntiguo.Observacion = sesionComentario.Observacion;

        //             _context.SesionComentario.Update(sesionComentarioAntiguo);

        //         }

        //         return respuesta = new Respuesta
        //         {
        //             IsSuccessful = true,
        //             IsException = false,
        //             IsValidation = false,
        //             Data = sesionComentario,
        //             Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
        //             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, sesionComentario.UsuarioCreacion, strCrearEditar)

        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         return respuesta = new Respuesta
        //         {
        //             IsSuccessful = false,
        //             IsException = true,
        //             IsValidation = false,
        //             Data = null,
        //             Code = ConstantMessagesSesionComiteTema.Error,
        //             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, sesionComentario.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
        //         };
        //     }

        // }



        // //Registrar temas compromisos
        // public async Task<Respuesta> CreateOrEditSubjects(TemaCompromiso temaCompromiso)
        // {
        //     Respuesta respuesta = new Respuesta();
        //     int idAccion = 0; //await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Tema_Compromiso, (int)EnumeratorTipoDominio.Acciones);

        //     string strCrearEditar = string.Empty;
        //     TemaCompromiso temaCompromisoAntiguo = null;
        //     try
        //     {

        //         if (string.IsNullOrEmpty(temaCompromiso.TemaCompromisoId.ToString()) || temaCompromiso.TemaCompromisoId == 0)
        //         {
        //             //Auditoria
        //             strCrearEditar = "CREAR TEMA COMPROMISO";
        //             temaCompromiso.UsuarioCreacion = temaCompromiso.UsuarioCreacion;
        //             temaCompromiso.FechaCreacion = DateTime.Now;
        //             _context.TemaCompromiso.Add(temaCompromiso);
        //         }
        //         else
        //         {
        //             strCrearEditar = "EDIT TEMA COMPROMISO";
        //             temaCompromisoAntiguo = _context.TemaCompromiso.Find(temaCompromiso.TemaCompromisoId);
        //             //Auditoria
        //             temaCompromisoAntiguo.UsuarioModificacion = temaCompromiso.UsuarioModificacion;
        //             temaCompromisoAntiguo.FechaModificacion = DateTime.Now;

        //             //Registros
        //             temaCompromisoAntiguo.SesionTemaId = temaCompromiso.SesionTemaId;
        //             temaCompromisoAntiguo.Tarea = temaCompromiso.Tarea;
        //             temaCompromisoAntiguo.Responsable = temaCompromiso.Responsable;
        //             temaCompromisoAntiguo.FechaCumplimiento = temaCompromiso.FechaCumplimiento;

        //             _context.TemaCompromiso.Update(temaCompromisoAntiguo);

        //         }

        //         return respuesta = new Respuesta
        //         {
        //             IsSuccessful = true,
        //             IsException = false,
        //             IsValidation = false,
        //             Data = temaCompromiso,
        //             Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
        //             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, temaCompromiso.UsuarioCreacion, strCrearEditar)

        //         };
        //     }

        //     catch (Exception ex)
        //     {
        //         return respuesta = new Respuesta
        //         {
        //             IsSuccessful = false,
        //             IsException = true,
        //             IsValidation = false,
        //             Data = null,
        //             Code = ConstantMessagesSesionComiteTema.Error,
        //             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, temaCompromiso.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
        //         };
        //     }

        // }


        // //Detalle de registro sesion invitado
        // public async Task<SesionInvitado> GetSesionGuesById(int sesionInvitadoId)
        // {
        //     try
        //     {
        //         return await _context.SesionInvitado.FindAsync(sesionInvitadoId);
        //     }
        //     catch (Exception)
        //     {

        //         throw;
        //     }
        // }

        private bool ValidarRegistroCompletoSesionComiteSolicitud(SesionComiteSolicitud sesionComiteSolicitud)
        {
            if (
               sesionComiteSolicitud.RutaSoporteVotacionFiduciario == null ||
               sesionComiteSolicitud.GeneraCompromisoFiduciario == null ||
               sesionComiteSolicitud.RequiereVotacionFiduciario == null ||
               sesionComiteSolicitud.EstadoCodigo == null ||
               string.IsNullOrEmpty(sesionComiteSolicitud.ObservacionesFiduciario) ||
               string.IsNullOrEmpty(sesionComiteSolicitud.RutaSoporteVotacionFiduciario)
                )
            {
                return false;
            }
            return true;
        }

        private bool ValidarRegistroCompletoSesionComiteTema(SesionComiteTema sesionComiteTemaOld)
        {
            if (string.IsNullOrEmpty(sesionComiteTemaOld.Tema)
                || string.IsNullOrEmpty(sesionComiteTemaOld.ResponsableCodigo)
                || string.IsNullOrEmpty(sesionComiteTemaOld.TiempoIntervencion.ToString())
                //|| !string.IsNullOrEmpty(sesionComiteTemaOld.RutaSoporte)
                || string.IsNullOrEmpty(sesionComiteTemaOld.Observaciones)
                || sesionComiteTemaOld.EsAprobado == null
                || sesionComiteTemaOld.RequiereVotacion == null
                //|| sesionComiteTemaOld.EsProposicionesVarios == null
                || sesionComiteTemaOld.GeneraCompromiso == null
                || string.IsNullOrEmpty(sesionComiteTemaOld.ObservacionesDecision)
                || sesionComiteTemaOld.EstadoTemaCodigo == null

                )
            {

                return false;
            }

            return true;

        }

        public static bool ValidarCamposComiteTecnico(ComiteTecnico pComiteTecnico)
        {
            if (
                    pComiteTecnico.RequiereVotacion == null ||
                    pComiteTecnico.RequiereVotacion == null ||
                    string.IsNullOrEmpty(pComiteTecnico.Justificacion) ||
                    pComiteTecnico.EsAprobado == null ||
                    string.IsNullOrEmpty(pComiteTecnico.FechaAplazamiento.ToString()) ||
                    string.IsNullOrEmpty(pComiteTecnico.Observaciones) ||
                    string.IsNullOrEmpty(pComiteTecnico.RutaSoporteVotacion) ||
                    pComiteTecnico.TieneCompromisos == null ||
                    string.IsNullOrEmpty(pComiteTecnico.CantCompromisos.ToString()) ||
                    string.IsNullOrEmpty(pComiteTecnico.RutaActaSesion) ||
                    string.IsNullOrEmpty(pComiteTecnico.FechaOrdenDia.ToString()) ||
                    string.IsNullOrEmpty(pComiteTecnico.NumeroComite) ||
                    string.IsNullOrEmpty(pComiteTecnico.EstadoComiteCodigo)
                )
            {
                return false;
            }
            return true;
        }

        // //Crear orden del dia de comité fiduciario



        // //Registrar participantes
        // public async Task<Respuesta> CreateOrEditParticipantes(SesionComiteTema sesionComiteTema)
        // {
        //     Respuesta respuesta = new Respuesta();
        //     int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_Comite_Tema, (int)EnumeratorTipoDominio.Acciones);

        //     string strCrearEditar = string.Empty;
        //     SesionComiteTema sesionComiteTemaAntiguo = null;
        //     try
        //     {

        //         if (string.IsNullOrEmpty(sesionComiteTema.SesionTemaId.ToString()) || sesionComiteTema.SesionTemaId == 0)
        //         {
        //             //TODO: recorrer objeto SesionComiteTema, Se puede guardar uno o varios temas.

        //             //Auditoria
        //             strCrearEditar = "CREAR SESION COMITÉ TEMA";
        //             sesionComiteTema.FechaCreacion = DateTime.Now;
        //             sesionComiteTema.Eliminado = false;

        //             //Registros
        //             sesionComiteTema.EsAprobado = false;
        //             sesionComiteTema.ResponsableCodigo = string.IsNullOrEmpty(sesionComiteTema.ResponsableCodigo) ? string.Empty : sesionComiteTema.ResponsableCodigo;
        //             _context.SesionComiteTema.Add(sesionComiteTema);
        //         }
        //         else
        //         {
        //             strCrearEditar = "EDIT SESION COMITÉ TEMA";
        //             sesionComiteTemaAntiguo = _context.SesionComiteTema.Find(sesionComiteTema.SesionTemaId);
        //             //Auditoria
        //             sesionComiteTemaAntiguo.UsuarioModificacion = sesionComiteTema.UsuarioModificacion;
        //             sesionComiteTemaAntiguo.FechaModificacion = DateTime.Now;


        //             //Registros

        //             sesionComiteTemaAntiguo.SesionTemaId = sesionComiteTema.SesionTemaId;
        //             sesionComiteTemaAntiguo.Tema = sesionComiteTema.Tema;
        //             sesionComiteTemaAntiguo.ResponsableCodigo = sesionComiteTema.ResponsableCodigo;
        //             sesionComiteTemaAntiguo.TiempoIntervencion = sesionComiteTema.TiempoIntervencion; // En minutos

        //             _context.SesionComiteTema.Update(sesionComiteTemaAntiguo);

        //         }

        //         return respuesta = new Respuesta
        //         {
        //             IsSuccessful = true,
        //             IsException = false,
        //             IsValidation = false,
        //             Data = sesionComiteTema,
        //             Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
        //             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, sesionComiteTema.UsuarioCreacion, strCrearEditar)

        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         return respuesta = new Respuesta
        //         {
        //             IsSuccessful = false,
        //             IsException = true,
        //             IsValidation = false,
        //             Data = null,
        //             Code = ConstantMessagesSesionComiteTema.Error,
        //             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, sesionComiteTemaAntiguo.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
        //         };
        //     }

        // }

        // #endregion





    }
}
