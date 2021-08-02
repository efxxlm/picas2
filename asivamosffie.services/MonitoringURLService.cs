using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class MonitoringURLService : IMonitoringURL
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public MonitoringURLService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;

        }

        public async Task<List<VistaContratoProyectos>> GetListContratoProyectos()
        {
            List<VistaContratoProyectos> lstVistaContratoProyectos = new List<VistaContratoProyectos>();
            VistaContratoProyectos vistaContratoProyectos = new VistaContratoProyectos();
            List<Contrato> ListContratos =
                await _context.Contrato
                                      .Where(r => (r.EstadoActaFase2 == ConstanCodigoEstadoActaContrato.Con_acta_suscrita_y_cargada
                                          || r.EstadoActaFase2 == "20"))
                                      .Distinct()
                                      .ToListAsync();

            foreach (var contrato in ListContratos)
            {
                try
                {
                    Contratacion contratacion = await _commonService.GetContratacionByContratacionId(contrato.ContratacionId);
                    Contratista contratista = null;
                    string strNombreContratista = string.Empty;

                    if (contratacion != null)
                    {
                        contratista = await _commonService.GetContratistaByContratistaId((Int32)contratacion.ContratistaId);
                        if (contratista != null)
                            strNombreContratista = contratista.Nombre; 
                    }

                    //TipoContrato = contrato.TipoContratoCodig

                    ContratacionProyecto contratacionProyecto = null;
                    int contratacionProyectoId = 0;


                    if (contratacion != null)
                    {
                        contratacionProyecto = _context.ContratacionProyecto.Where(r => r.ContratacionId == contratacion.ContratacionId && (bool)r.TieneMonitoreoWeb).FirstOrDefault();


                    }


                    if (contratacionProyecto != null)
                    {
                        //solicitar contratacion - si requiere monitoreo
                        if (contratacionProyecto.TieneMonitoreoWeb == true)
                        {
                            List<ProyectoGrilla> listProyectoGrilla = new List<ProyectoGrilla>();
                            listProyectoGrilla = await GetListProyects(contratacionProyecto.ProyectoId);

                            int NumProyectosAsociados = 0;
                            listProyectoGrilla = listProyectoGrilla.Where(r => r.ProyectoId == contratacionProyecto.ProyectoId).ToList();
                            NumProyectosAsociados = listProyectoGrilla.Count();

                            contratacionProyectoId = contratacionProyecto.ProyectoId;

                            var semaforo = 0;
                            foreach (var proy in listProyectoGrilla)
                            {
                                semaforo += String.IsNullOrEmpty(proy.URLMonitoreo) ? 0 : 1;
                            }
                            vistaContratoProyectos = new VistaContratoProyectos
                            {
                                NumeroContrato = contrato.NumeroContrato,
                                //NombreContratista = contratista.Nombre,
                                NombreContratista = strNombreContratista,
                                ProyectoId = contratacionProyectoId,
                                lstProyectoGrilla = listProyectoGrilla,
                                NumeroProyectosAsociados = NumProyectosAsociados,
                                Semaforo = (semaforo == listProyectoGrilla.Count() ? 1 : semaforo == 0 ? 0 : 2),
                            };

                            lstVistaContratoProyectos.Add(vistaContratoProyectos);
                        }

                    }


                }


                catch (Exception e)
                {
                    //VistaGenerarActaInicioContrato actaInicio = new VistaGenerarActaInicioContrato
                    vistaContratoProyectos = new VistaContratoProyectos
                    {
                        NumeroContrato = "Error",
                        NombreContratista = "Error",
                        ProyectoId = 0,
                        lstProyectoGrilla = null,
                        NumeroProyectosAsociados = -1

                    };
                    lstVistaContratoProyectos.Add(vistaContratoProyectos);
                }

            }
            return lstVistaContratoProyectos;
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


                                URLMonitoreo = strProyectoUrlMonitoreo,
                                ContratoId = 0, //await getContratoIdByProyectoId(proyecto.ProyectoId),


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
                element.ContratoId = await getContratoIdByProyectoId(element.ProyectoId);
            }

            return ListProyectoGrilla.OrderByDescending(r => r.ProyectoId).ToList();
        }

        public async Task<int> getContratoIdByProyectoId(int pProyectoId)
        //private int? getContratoIdByProyectoId(int pProyectoId)
        {
            //Proyecto proyecto = null;
            //proyecto = await _context.Proyecto.Where(r => r.ProyectoId == pProyectoId).FirstOrDefaultAsync();

            ContratacionProyecto contratacionProyecto = null;
            contratacionProyecto = await _context.ContratacionProyecto.Where(r => r.ProyectoId == pProyectoId && (bool)r.Activo == true).FirstOrDefaultAsync();

            Contratacion contratacion = null;
            if (contratacionProyecto != null)
            {
                contratacion = await _context.Contratacion.Where(r => r.ContratacionId == contratacionProyecto.ContratacionId && r.Eliminado == false).FirstOrDefaultAsync();

            }

            Contrato contrato = null;
            if (contratacion != null)
            {
                contrato = await _context.Contrato.Where(r => r.ContratacionId == contratacion.ContratacionId && r.Eliminado == false).FirstOrDefaultAsync();
            }

            if (contrato != null)
                return contrato.ContratoId;
            else
                return 0;

        }

        public async Task<Respuesta> EditarURLMonitoreo(Proyecto pProyecto)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_crear_url, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                _context.Set<Proyecto>()
                        .Where(p => p.ProyectoId == pProyecto.ProyectoId)
                        .Update(p => new Proyecto
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pProyecto.UsuarioModificacion,
                            UrlMonitoreo = pProyecto.UrlMonitoreo,
                        });

                await SendMailCargarEnlace(pProyecto.ProyectoId);

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = _context.Proyecto.Find(pProyecto.ProyectoId),
                    Code = ConstantMessagesCargarEnlaceMonitoreo.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Cargar_enlace_monitoreo, ConstantMessagesCargarEnlaceMonitoreo.OperacionExitosa, idAccion, pProyecto.UsuarioModificacion, "EDITAR MONITOREO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesCargarEnlaceMonitoreo.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Cargar_enlace_monitoreo, ConstantMessagesDisponibilidadPresupuesta.Error, idAccion, pProyecto.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> VisitaURLMonitoreo(string uRLMonitoreo, string usuarioModificacion)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Visita_url, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = "";
            try
            {
                strCrearEditar = "VISITA URL MONITOREO " + uRLMonitoreo;
                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = uRLMonitoreo,
                    Code = ConstantMessagesCargarEnlaceMonitoreo.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Cargar_enlace_monitoreo, ConstantMessagesCargarEnlaceMonitoreo.OperacionExitosa, idAccion, usuarioModificacion, strCrearEditar)
                };

                //}


            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesCargarEnlaceMonitoreo.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Cargar_enlace_monitoreo, ConstantMessagesDisponibilidadPresupuesta.Error, idAccion, usuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        #region correos
        private async Task<bool> SendMailCargarEnlace(int proyectoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Cargar_enlace_sistema_monitoreo_linea_4_1_1));

            Proyecto proyecto = _context.Proyecto.Where(r => r.ProyectoId == proyectoId)
                 .Include(r => r.ContratacionProyecto)
                    .ThenInclude(r => r.Contratacion)
                        .ThenInclude(r => r.Contrato)
                 .Include(r => r.Sede)
                 .Include(r => r.InstitucionEducativa)
                 .Include(r => r.LocalizacionIdMunicipioNavigation)
                 .FirstOrDefault();
            List<Dominio> ListTipoIntervencion = await _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && (bool)r.Activo).ToListAsync();

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Supervisor
                                          };

            if (proyecto != null)
            {
                String strContenido = template.Contenido
                             .Replace("[LLAVE_MEN]", proyecto.LlaveMen)
                             .Replace("[NUM_CONTRATO]", proyecto.ContratacionProyecto.FirstOrDefault().Contratacion.Contrato.FirstOrDefault().NumeroContrato)
                             .Replace("[INSTITUCION_EDUCATIVA]", proyecto.InstitucionEducativa.Nombre)
                             .Replace("[SEDE]", proyecto.Sede.Nombre)
                             .Replace("[TIPO_INTERVENCION]", ListTipoIntervencion.Find(r => r.Codigo == proyecto.TipoIntervencionCodigo).Nombre)
                             .Replace("[FECHA_ACTA_INICIO]", proyecto.ContratacionProyecto.FirstOrDefault().Contratacion.Contrato.FirstOrDefault().FechaActaInicioFase1.HasValue ? ((DateTime)proyecto.ContratacionProyecto.FirstOrDefault().Contratacion.Contrato.FirstOrDefault().FechaActaInicioFase1).ToString("dd-MM-yy") : " ");


                return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
            }

            return false;
        }
        #endregion
    }
}
