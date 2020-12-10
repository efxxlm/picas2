using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Helpers.Constant;

namespace asivamosffie.services
{
    public class JudicialDefenseService /*: IGuaranteePolicyService*/
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public JudicialDefenseService(devAsiVamosFFIEContext context, ICommonService commonService)
        {

            _commonService = commonService;
            _context = context;
            //_settings = settings;
        }

        public async Task<string> GetNombreContratistaByContratoId(int pContratoId)
        {
            Contrato contrato = null;
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            Contratacion contratacion = null;
            if (contrato != null)
            {
                contratacion = await _commonService.GetContratacionByContratacionId(contrato.ContratacionId);

            }            

            Contratista contratista = null;
            if (contratacion != null)
            {
                if (contratacion.ContratistaId != null)
                    contratista = await _commonService.GetContratistaByContratistaId((Int32)contratacion.ContratistaId);

                if (contratista != null)
                {
                    return contratista.Nombre;

                }
            }
            return null;
        }

        public async Task<Respuesta> CreateOrEditDemandadoConvocado(DemandadoConvocado demandadoConvocado)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Demandado_Convocado, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            DemandadoConvocado demandadoConvocadoBD = null;
            try
            {

                if (string.IsNullOrEmpty(demandadoConvocado.DemandadoConvocadoId.ToString()) || demandadoConvocado.DemandadoConvocadoId == 0)
                {
                    //Auditoria
                    strCrearEditar = "REGISTRAR DEMANDADO CONVOCADO";
                    demandadoConvocado.FechaCreacion = DateTime.Now;
                    demandadoConvocado.UsuarioCreacion = demandadoConvocado.UsuarioCreacion;
                    //fichaEstudio.DefensaJudicialId = fichaEstudio.DefensaJudicialId;
                    demandadoConvocado.Eliminado = false;
                    _context.DemandadoConvocado.Add(demandadoConvocado);
                }
                else
                {
                    strCrearEditar = "EDIT DEMANDADO CONVOCADO";
                    demandadoConvocadoBD = _context.DemandadoConvocado.Find(demandadoConvocado.DemandadoConvocadoId);

                    //Auditoria
                    demandadoConvocadoBD.UsuarioModificacion = demandadoConvocado.UsuarioModificacion;
                    demandadoConvocadoBD.Eliminado = false;

                    //Registros
                    demandadoConvocadoBD.Nombre = demandadoConvocado.Nombre;
                    demandadoConvocadoBD.TipoIdentificacionCodigo = demandadoConvocado.TipoIdentificacionCodigo;
                    demandadoConvocadoBD.NumeroIdentificacion = demandadoConvocado.NumeroIdentificacion;
                    demandadoConvocadoBD.DefensaJudicial = demandadoConvocado.DefensaJudicial;
                    demandadoConvocadoBD.Direccion = demandadoConvocado.Direccion;
                    demandadoConvocadoBD.UsuarioModificacion = demandadoConvocado.UsuarioModificacion;
                    demandadoConvocadoBD.Email = demandadoConvocado.Email;
                    
                    _context.DemandadoConvocado.Update(demandadoConvocado);

                }

                _context.SaveChanges();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = demandadoConvocado,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, demandadoConvocado.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = demandadoConvocado,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, demandadoConvocado.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        public async Task<Respuesta> CreateOrEditFichaEstudio(FichaEstudio fichaEstudio)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Ficha_Estudio, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            FichaEstudio fichaEstudioBD = null;
            try
            {

                if (string.IsNullOrEmpty(fichaEstudio.FichaEstudioId.ToString()) || fichaEstudio.FichaEstudioId == 0)
                {
                    //Auditoria
                    strCrearEditar = "REGISTRAR FICHA ESTUDIO";
                    fichaEstudio.FechaCreacion = DateTime.Now;
                    fichaEstudio.UsuarioCreacion = fichaEstudio.UsuarioCreacion;
                    //fichaEstudio.DefensaJudicialId = fichaEstudio.DefensaJudicialId;
                    fichaEstudio.EsCompleto = ValidarRegistroCompletoFichaEstudio(fichaEstudio);
                    fichaEstudio.Eliminado = false;
                    _context.FichaEstudio.Add(fichaEstudio);
                }
                else
                {
                    strCrearEditar = "EDIT FICHA ESTUDIO";
                    fichaEstudioBD = _context.FichaEstudio.Find(fichaEstudio.FichaEstudioId);

                    //Auditoria
                    fichaEstudioBD.UsuarioModificacion = fichaEstudio.UsuarioModificacion;
                    fichaEstudioBD.Eliminado = false;

                    //Registros
                    fichaEstudioBD.AnalisisJuridico = fichaEstudio.AnalisisJuridico;
                    fichaEstudioBD.Antecedentes = fichaEstudio.Antecedentes;
                    fichaEstudioBD.DecisionComiteDirectrices = fichaEstudio.DecisionComiteDirectrices;
                    fichaEstudioBD.DefensaJudicial = fichaEstudio.DefensaJudicial;
                    fichaEstudioBD.TipoActuacionCodigo = fichaEstudio.TipoActuacionCodigo;
                    fichaEstudioBD.UsuarioModificacion = fichaEstudio.UsuarioModificacion;
                    fichaEstudioBD.Abogado = fichaEstudio.Abogado;
                    fichaEstudioBD.RutaSoporte = fichaEstudio.RutaSoporte;
                    fichaEstudioBD.Recomendaciones = fichaEstudio.Recomendaciones;
                    fichaEstudioBD.RecomendacionFinalComite = fichaEstudio.RecomendacionFinalComite;
                    fichaEstudioBD.JurisprudenciaDoctrina = fichaEstudio.JurisprudenciaDoctrina;

                    fichaEstudioBD.EsAprobadoAperturaProceso = fichaEstudio.EsAprobadoAperturaProceso;
                    fichaEstudioBD.EsPresentadoAnteComiteFfie = fichaEstudio.EsPresentadoAnteComiteFfie;

                    fichaEstudio.EsCompleto = ValidarRegistroCompletoFichaEstudio(fichaEstudio);

                    _context.FichaEstudio.Update(fichaEstudio);

                }

                _context.SaveChanges();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = fichaEstudio,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, fichaEstudio.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = fichaEstudio,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, fichaEstudio.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        private bool ValidarRegistroCompletoFichaEstudio(FichaEstudio fichaEstudio)
        {
            if (string.IsNullOrEmpty(fichaEstudio.Antecedentes)
             || string.IsNullOrEmpty(fichaEstudio.HechosRelevantes)
            || string.IsNullOrEmpty(fichaEstudio.JurisprudenciaDoctrina)
            || string.IsNullOrEmpty(fichaEstudio.DecisionComiteDirectrices)
            || string.IsNullOrEmpty(fichaEstudio.AnalisisJuridico)
            || string.IsNullOrEmpty(fichaEstudio.Recomendaciones)
            || string.IsNullOrEmpty(fichaEstudio.TipoActuacionCodigo)
                || (fichaEstudio.EsPresentadoAnteComiteFfie == null)
                || (fichaEstudio.EsAprobadoAperturaProceso == null)
               || (fichaEstudio.RecomendacionFinalComite == null)
               || string.IsNullOrEmpty(fichaEstudio.RutaSoporte))

            {
                return false;
            }

            return true;
        }
        public async Task<List<ProyectoGrilla>> GetListProyects(/*int pContratoId*/ int pProyectoId)
        {
            //Listar Los proyecto segun caso de uso solo trae los ue estado
            //estado de registro “Completo”, que tienen viabilidad jurídica y técnica
            List<ProyectoGrilla> ListProyectoGrilla = new List<ProyectoGrilla>();
            List<Proyecto> ListProyectos = new List<Proyecto>();
            try
            {
                ListProyectos = await _context.Proyecto.Where(
                         r => !(bool)r.Eliminado &&
                         r.EstadoJuridicoCodigo == ConstantCodigoEstadoJuridico.Aprobado
                         && (bool)r.RegistroCompleto
                         && r.ProyectoId == pProyectoId
                         //Se quitan los proyectos que ya esten vinculados a una contratacion
                         )
                                 .Include(r => r.ContratacionProyecto).ThenInclude(r => r.Contratacion)
                                 .Include(r => r.Sede).Include(r => r.InstitucionEducativa)
                                 .Include(r => r.LocalizacionIdMunicipioNavigation).Distinct().ToListAsync();

                //List<Localicacion> Municipios = new List<Localicacion>();

                //if (!string.IsNullOrEmpty(pDepartamento) && string.IsNullOrEmpty(pRegion) && string.IsNullOrEmpty(pMunicipio))
                //{
                //    Municipios = await _commonService.GetListMunicipioByIdDepartamento(pDepartamento);
                //}

                //if (!string.IsNullOrEmpty(pRegion) && string.IsNullOrEmpty(pDepartamento) && string.IsNullOrEmpty(pMunicipio))
                //{
                //    List<Localizacion> Departamentos = _context.Localizacion.Where(r => r.IdPadre == pRegion).ToList();
                //    foreach (var dep in Departamentos)
                //    {
                //        Municipios.AddRange(await _commonService.GetListMunicipioByIdDepartamento(dep.LocalizacionId));
                //    }
                //}
                //if (Municipios.Count() > 0)
                //{
                //    //ListContratacion.RemoveAll(item => LisIdContratacion.Contains(item.ContratacionId));
                //    ListProyectos.RemoveAll(item => !Municipios.Select(r => r.LocalizacionId).Contains(item.LocalizacionIdMunicipio));
                //}

                //List<Proyecto> ListaProyectosRemover = new List<Proyecto>();
                //foreach (var Proyecto in ListProyectos)
                //{
                //    foreach (var ContratacionProyecto in Proyecto.ContratacionProyecto)
                //    {
                //        if (ContratacionProyecto.Contratacion.EstadoSolicitudCodigo
                //            != ConstanCodigoEstadoSolicitudContratacion.Rechazado)
                //        {
                //            ListaProyectosRemover.Add(Proyecto);
                //        }
                //        else
                //        {
                //            if (Proyecto.ContratacionProyecto.Where(r => r.ProyectoId == Proyecto.ProyectoId).Count() > 1)
                //            {
                //                ListaProyectosRemover.Add(Proyecto);
                //            }
                //        }
                //    }
                //}

                //foreach (var proyecto in ListaProyectosRemover.Distinct())
                //{
                //    ListProyectos.Remove(proyecto);
                //}

                List<Dominio> ListTipoSolicitud = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Tipo_de_Solicitud_Obra_Interventorias);

                //Lista para Dominio intervencio
                List<Dominio> ListTipoIntervencion = await _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && (bool)r.Activo).ToListAsync();

                List<Localizacion> ListDepartamentos = await _context.Localizacion.Where(r => r.Nivel == 1).ToListAsync();

                List<Localizacion> ListRegiones = await _context.Localizacion.Where(r => r.Nivel == 3).ToListAsync();
                //departamneto 
                //    Region  
                List<Contratacion> ListContratacion = await _context.Contratacion.Where(r => !(bool)r.Eliminado).ToListAsync();

                string strProyectoUrlMonitoreo = string.Empty;


                foreach (var proyecto in ListProyectos)
                {
                    if (!string.IsNullOrEmpty(proyecto.TipoIntervencionCodigo))
                    {
                        Localizacion departamento = ListDepartamentos.Find(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipioNavigation.IdPadre);

                        if (proyecto.UrlMonitoreo != null)
                            strProyectoUrlMonitoreo = proyecto.UrlMonitoreo;

                        try
                        {
                            ProyectoGrilla proyectoGrilla = new ProyectoGrilla
                            {
                                TipoIntervencion = ListTipoIntervencion.Find(r => r.Codigo == proyecto.TipoIntervencionCodigo).Nombre,
                                LlaveMen = proyecto.LlaveMen,
                                Departamento = departamento.Descripcion,
                                Region = ListRegiones.Find(r => r.LocalizacionId == departamento.IdPadre).Descripcion,
                                //  Departamento = _commonService.GetNombreDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio),
                                // Municipio = _commonService.GetNombreLocalizacionByLocalizacionId(proyecto.LocalizacionIdMunicipio),
                                Municipio = proyecto.LocalizacionIdMunicipioNavigation.Descripcion,
                                //InstitucionEducativa = _context.InstitucionEducativaSede.Find(proyecto.InstitucionEducativaId).Nombre,
                                //Sede = _context.InstitucionEducativaSede.Find(proyecto.SedeId).Nombre,
                                InstitucionEducativa = proyecto.InstitucionEducativa.Nombre,
                                Sede = proyecto.Sede.Nombre,
                                ProyectoId = proyecto.ProyectoId,


                                //URLMonitoreo = strProyectoUrlMonitoreo,
                                //ContratoId = 0, //await getContratoIdByProyectoId(proyecto.ProyectoId),


                            };

                            //r.TipoIntervencionCodigo == (string.IsNullOrEmpty(pTipoIntervencion) ? r.TipoIntervencionCodigo : pTipoIntervencion) &&
                            //List<Contrato> lstContratos = _context.Contrato.Where(r => r.ContratoId == pContratoId).ToList();


                            foreach (var item in proyecto.ContratacionProyecto)
                            {
                                item.Contratacion = ListContratacion.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefault();
                                //item.Contratacion = ListContratacion.Where(r => r.ContratacionId == contrato.ContratacionId ).FirstOrDefault();

                                //item.Contratacion= item.Contratacion.wh(r => r.ContratacionId == item.ContratacionId ).FirstOrDefault();

                                if (item.Contratacion != null)
                                {

                                    if (!string.IsNullOrEmpty(item.Contratacion.TipoSolicitudCodigo))
                                    {
                                        if (item.Contratacion.TipoSolicitudCodigo == "1")
                                        {
                                            proyectoGrilla.TieneObra = true;
                                        }
                                        if (item.Contratacion.TipoSolicitudCodigo == "2")
                                        {
                                            proyectoGrilla.TieneInterventoria = true;
                                        }
                                    }
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
            //ListProyectoGrilla = ListProyectoGrilla.Where(r => r.ContratoId != 0).ToList();

            foreach (ProyectoGrilla element in ListProyectoGrilla)
            {
                //element.ContratoId = await getContratoIdByProyectoId(element.ProyectoId);
            }

            return ListProyectoGrilla.OrderByDescending(r => r.ProyectoId).ToList();
        }

        public async Task<List<GrillaProcesoDefensaJudicial>> ListGrillaProcesosDefensaJudicial()
        {
            //await AprobarContratoByIdContrato(1);

            List<GrillaProcesoDefensaJudicial> ListDefensaJudicialGrilla = new List<GrillaProcesoDefensaJudicial>();
            //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)

            //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo        

            //List <Contrato> ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).Include(r => r.NumeroContrato).Include(r => r.Estado).Distinct().ToListAsync();
            List<DefensaJudicial> ListDefensaJudicial  = await _context.DefensaJudicial.Where(r => (bool)r.Eliminado==false).Distinct().ToListAsync();

            foreach (var defensaJudicial in ListDefensaJudicial)
            {
                try
                {     
                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    string strTipoSolicitudCodigoContratoPoliza = "sin definir";
                    string strEstadoSolicitudCodigoContratoPoliza = "sin definir";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio TipoSolicitudCodigoContratoPoliza;
                    Dominio EstadoSolicitudCodigoContratoPoliza;

                    //if (contratoPoliza != null)
                    //{
                        //TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Modificacion_Contrato_Poliza);
                        //TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo.Trim(), (int)EnumeratorTipoDominio.Tipo_de_Solicitud);
                        //if (TipoSolicitudCodigoContratoPoliza != null)
                        //    strTipoSolicitudCodigoContratoPoliza = TipoSolicitudCodigoContratoPoliza.Nombre;

                        //EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.EstadoPolizaCodigo.Trim(), (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                        //if (EstadoSolicitudCodigoContratoPoliza != null)
                        //    strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

                    //}
                    bool bRegistroCompleto = false;
                    string strRegistroCompleto = "Incompleto";

                    //if (defensaJudicial.EsCompleto != null)
                    //{
                        strRegistroCompleto = (bool)defensaJudicial.EsCompleto ? "Completo" : "Incompleto";
                    //}

                    //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                    GrillaProcesoDefensaJudicial defensaJudicialGrilla = new GrillaProcesoDefensaJudicial
                    {
                        DefensaJudicialId = defensaJudicial.DefensaJudicialId,
                        FechaRegistro = defensaJudicial.FechaCreacion.ToString("dd/MM/yyyy"),
                        LegitimacionPasivaActiva= (bool)defensaJudicial.EsLegitimacionActiva ? "Activa" : "Pasiva",
                        NumeroProceso="DJ"+ defensaJudicial.DefensaJudicialId.ToString() + defensaJudicial.FechaCreacion.ToString("yyyy"),
                        TipoAccionCodigo= defensaJudicial.TipoAccionCodigo,
                        TipoAccion = "PENDIENTE",
                        EstadoProceso= "PENDIENTE",
                        EstadoProcesoCodigo =defensaJudicial.EstadoProcesoCodigo,
                        
                        RegistroCompletoNombre =strRegistroCompleto,
                        TipoProceso="PENDIENTE",
                        TipoProcesoCodigo = defensaJudicial.TipoProcesoCodigo,                       
                                          
                    };

                    //if (!(bool)proyecto.RegistroCompleto)
                    //{
                    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    //}
                    ListDefensaJudicialGrilla.Add(defensaJudicialGrilla);
                }
                catch (Exception e)
                {
                    GrillaProcesoDefensaJudicial defensaJudicialGrilla = new GrillaProcesoDefensaJudicial
                    {
                        DefensaJudicialId = defensaJudicial.DefensaJudicialId,
                        FechaRegistro = e.ToString(),
                        LegitimacionPasivaActiva = e.InnerException.ToString(),
                        NumeroProceso = "ERROR",
                        TipoAccionCodigo = "ERROR",
                        TipoAccion = "ERROR",
                        EstadoProceso = "ERROR",
                        EstadoProcesoCodigo = "ERROR",

                        RegistroCompletoNombre = "ERROR",
                        TipoProceso = "ERROR",
                        TipoProcesoCodigo = "ERROR",                  
    
                    };
                    ListDefensaJudicialGrilla.Add(defensaJudicialGrilla);
                }
            }
            return ListDefensaJudicialGrilla.ToList();

        }
    }
}
