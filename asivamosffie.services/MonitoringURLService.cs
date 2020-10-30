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

namespace asivamosffie.services
{
    public class MonitoringURLService: IMonitoringURL
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

            //número de contrato, nombre del contratista y número de proyectos asociados.

            List<VistaContratoProyectos> lstVistaContratoProyectos = new List<VistaContratoProyectos>();
            VistaContratoProyectos vistaContratoProyectos = new VistaContratoProyectos();

            List<Contrato> ListContratos = await _context.Contrato.Where(r => (bool)r.Estado).Distinct().ToListAsync();

            foreach (var contrato in ListContratos)
            {
                try
                {
                    Contratacion contratacion = await _commonService.GetContratacionByContratacionId(contrato.ContratacionId);
                    Contratista contratista =null;
                    string strNombreContratista = string.Empty;

                    if (contratacion != null)
                    {
                        contratista = await _commonService.GetContratistaByContratistaId((Int32)contratacion.ContratistaId);
                        if(contratista!=null)
                        {
                            strNombreContratista = contratista.Nombre;
                        }
                    }                        

                    //TipoContrato = contrato.TipoContratoCodig

                    ContratacionProyecto contratacionProyecto = null;
                    int contratacionProyectoId = 0;


                    if (contratacion != null)
                    {
                        contratacionProyecto =  _context.ContratacionProyecto.Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();


                    }

                    List<ProyectoGrilla> listProyectoGrilla = new List<ProyectoGrilla>();
                    listProyectoGrilla = await GetListProyects( );

                    int NumProyectosAsociados = 0;

                    if (contratacionProyecto != null)
                    {
                        listProyectoGrilla = listProyectoGrilla.Where(r => r.ProyectoId == contratacionProyecto.ProyectoId).ToList();
                        NumProyectosAsociados = listProyectoGrilla.Count();

                        contratacionProyectoId = contratacionProyecto.ProyectoId;
                    }

                    vistaContratoProyectos = new VistaContratoProyectos
                    {
                        NumeroContrato = contrato.NumeroContrato,
                        //NombreContratista = contratista.Nombre,
                        NombreContratista = strNombreContratista,
                        ProyectoId = contratacionProyectoId ,
                        lstProyectoGrilla = listProyectoGrilla,
                        NumeroProyectosAsociados = NumProyectosAsociados
                    };

                    lstVistaContratoProyectos.Add(vistaContratoProyectos);
                }


                catch (Exception e)
                {
                    //VistaGenerarActaInicioContrato actaInicio = new VistaGenerarActaInicioContrato
                    vistaContratoProyectos = new VistaContratoProyectos
                    {
                        NumeroContrato = e.InnerException.ToString(),
                        NombreContratista = e.ToString(),
                        ProyectoId = 0,
                        lstProyectoGrilla = null,
                        NumeroProyectosAsociados = -1

                    };
                    lstVistaContratoProyectos.Add(vistaContratoProyectos);
                }
                
            }
            return lstVistaContratoProyectos;
        }

            public async Task<List<ProyectoGrilla>> GetListProyects(/*int pContratoId*/)
        {
            //Listar Los proyecto segun caso de uso solo trae los ue estado
            //estado de registro “Completo”, que tienen viabilidad jurídica y técnica
            List<ProyectoGrilla> ListProyectoGrilla = new List<ProyectoGrilla>();
            List<Proyecto> ListProyectos = new List<Proyecto>();
            try
            {
                ListProyectos = await
                     _context.Proyecto.Where(
                         r => !(bool)r.Eliminado &&
                         r.EstadoJuridicoCodigo == ConstantCodigoEstadoJuridico.Aprobado
                         &&
                         (bool)r.RegistroCompleto 
                         //Se quitan los proyectos que ya esten vinculados a una contratacion
                       
                         )
                                 .Include(r => r.ContratacionProyecto)
                                   .ThenInclude(r => r.Contratacion)
                                 .Include(r => r.Sede)
                                 .Include(r => r.InstitucionEducativa)
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
                List<Dominio> ListTipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && (bool)r.Activo).ToList();

                List<Localizacion> ListDepartamentos = _context.Localizacion.Where(r => r.Nivel == 1).ToList();

                List<Localizacion> ListRegiones = _context.Localizacion.Where(r => r.Nivel == 3).ToList();
                //departamneto 
                //    Region  
                List<Contratacion> ListContratacion = await _context.Contratacion.Where(r => !(bool)r.Eliminado).ToListAsync();

                string strProyectoUrlMonitoreo= string.Empty;
                

                foreach (var proyecto in ListProyectos)
                {
                    if (!string.IsNullOrEmpty(proyecto.TipoIntervencionCodigo))
                    {
                        Localizacion departamento = ListDepartamentos.Find(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipioNavigation.IdPadre);

                        if(proyecto.UrlMonitoreo!=null)
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
                                item.Contratacion = ListContratacion.Where(r => r.ContratacionId == item.ContratacionId ).FirstOrDefault();
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

            foreach(ProyectoGrilla element in ListProyectoGrilla)
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

        //public async Task<List<VistaContratoGarantiaPoliza>> ListVistaContratoGarantiaPoliza()
        //{
        //    List<VistaContratoGarantiaPoliza> ListContratoGrilla = new List<VistaContratoGarantiaPoliza>();
        //    //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)


        //    //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo          

        //    List<Contrato> ListContratos = new List<Contrato>();
        //    //ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).Include(r => r.NumeroContrato).Include(r => r.Estado).Distinct().ToListAsync();

        //    //return await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado && r.TipoAportanteId == pTipoAportanteID).Include(r => r.Cofinanciacion).ToListAsync();

        //    //ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).ToListAsync();

        //    //item.CofinanciacionAportante = await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado && r.CofinanciacionId == item.CofinanciacionId).IncludeFilter(r => r.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado)).ToListAsync();

        //    ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Distinct()

        //    .ToListAsync();

        //    //ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado)               

        //    // .Select( new
        //    //  {
        //    //      FechaFirmaContrato =r.FechaFirmaContrato.ToString()
        //    //  })
        //    // .ToListAsync();

        //    foreach (var contrato in ListContratos)
        //    {
        //        try
        //        {
        //            ContratoPoliza contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
        //            Contratacion contratacion = await _commonService.GetContratacionByContratacionId(contrato.ContratoId);

        //            Contratista contratista = await _commonService.GetContratistaByContratistaId((Int32)contratacion.ContratistaId);

        //            //TipoContrato = contrato.TipoContratoCodigo   ??? Obra  ????

        //            //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
        //            Int32 plazoDias, plazoMeses;
        //            //25meses / 04 días

        //            if (!string.IsNullOrEmpty(contrato.PlazoFase2ConstruccionDias.ToString()))

        //                plazoDias = Convert.ToInt32(contrato.PlazoFase1PreDias);

        //            else
        //                plazoDias = Convert.ToInt32(contrato.PlazoFase2ConstruccionDias);

        //            if (!string.IsNullOrEmpty(contrato.PlazoFase2ConstruccionMeses.ToString()))

        //                plazoMeses = Convert.ToInt32(contrato.PlazoFase1PreMeses);

        //            else
        //                plazoMeses = Convert.ToInt32(contrato.PlazoFase2ConstruccionMeses);

        //            string PlazoContratoFormat = plazoMeses.ToString("00") + " meses / " + plazoDias.ToString("00") + " dias ";

        //            //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
        //            Dominio TipoContratoCodigoContrato = await _commonService.GetDominioByNombreDominioAndTipoDominio(contrato.TipoContratoCodigo, (int)EnumeratorTipoDominio.Tipo_Contrato);


        //            //Dominio TipoModificacionCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoModificacionCodigo, (int)EnumeratorTipoDominio.Tipo_Modificacion_Contrato_Poliza);
        //            VistaContratoGarantiaPoliza proyectoGrilla = new VistaContratoGarantiaPoliza
        //            {
        //                IdContrato = contrato.ContratoId,
        //                TipoContrato = TipoContratoCodigoContrato.Nombre,
        //                NumeroContrato = contrato.NumeroContrato,
        //                ObjetoContrato = contrato.Objeto,
        //                NombreContratista = contratista.Nombre,

        //                //Nit  
        //                NumeroIdentificacion = contratista.NumeroIdentificacion.ToString(),



        //                ValorContrato = contrato.Valor.ToString(),

        //                PlazoContrato = PlazoContratoFormat,

        //                //EstadoRegistro 

        //                //public bool? RegistroCompleto { get; set; } 



        //                DescripcionModificacion = "resumen", // resumen   TEMPORAL REV

        //                //TipoModificacion = TipoModificacionCodigoContratoPoliza.Nombre
        //                TipoModificacion = "Tipo modificacion"

        //                //TipoSolicitud= contratoPoliza.TipoSolicitudCodigo
        //                //EstadoRegistro { get; set; }

        //                //InstitucionEducativa = _context.InstitucionEducativaSede.Find(contrato.InstitucionEducativaId).Nombre,
        //                //Sede = _context.InstitucionEducativaSede.Find(contrato.SedeId).Nombre,

        //                //Fecha = contrato.FechaCreacion != null ? Convert.ToDateTime(contrato.FechaCreacion).ToString("yyyy-MM-dd") : proyecto.FechaCreacion.ToString(),
        //                //EstadoRegistro = "COMPLETO"
        //            };

        //            //if (!(bool)proyecto.RegistroCompleto)
        //            //{
        //            //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
        //            //}
        //            ListContratoGrilla.Add(proyectoGrilla);
        //        }
        //        catch (Exception e)
        //        {
        //            VistaContratoGarantiaPoliza proyectoGrilla = new VistaContratoGarantiaPoliza
        //            {
        //                IdContrato = 0,
        //                TipoContrato = e.ToString(),
        //                NumeroContrato = e.InnerException.ToString(),
        //                ObjetoContrato = "ERROR",
        //                NombreContratista = "ERROR",

        //                //Nit  
        //                NumeroIdentificacion = "ERROR",
        //                ValorContrato = "ERROR",

        //                PlazoContrato = "ERROR",

        //                //EstadoRegistro 

        //                //public bool? RegistroCompleto { get; set; } 

        //                //TipoSolicitud = contratoPoliza.EstadoPolizaCodigo

        //                DescripcionModificacion = "ERROR",

        //                TipoModificacion = "ERROR"

        //            };
        //            ListContratoGrilla.Add(proyectoGrilla);
        //        }
        //    }
        //    return ListContratoGrilla.OrderByDescending(r => r.TipoSolicitud).ToList();

        //}

        public async Task<Respuesta> EditarURLMonitoreo(Int32 pProyectoId, string pURLMonitoreo, string pUsuarioModificacion)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = "";
            Proyecto proyecto = null;
                       

            proyecto = _context.Proyecto.Where(r => r.ProyectoId == pProyectoId).FirstOrDefault();
            try
            {

                //if (string.IsNullOrEmpty(DP.DisponibilidadPresupuestalId.ToString()) || DP.DisponibilidadPresupuestalId == 0)
                //if (string.IsNullOrEmpty(proyecto.ProyectoId.ToString()) || proyecto.ProyectoId == 0)
                //{
                    //Concecutivo
                    //var LastRegister = _context.DisponibilidadPresupuestal.OrderByDescending(x => x.DisponibilidadPresupuestalId).First().DisponibilidadPresupuestalId;

                    //Auditoria
                    strCrearEditar = "EDITAR URL MONITOREO";
                    proyecto.FechaModificacion = DateTime.Now;
                proyecto.UsuarioModificacion = pUsuarioModificacion;

                proyecto.UrlMonitoreo = pURLMonitoreo;                             

                //proyecto.UrlMonitoreo = pURLMonitoreo;
                //DP.Eliminado = false;

                //DP.NumeroDdp = ""; TODO: traer consecutivo del modulo de proyectos, DDP_PI_autoconsecutivo
                //DP.EstadoSolicitudCodigo = "4"; // Sin registr/*a*/r

                _context.Proyecto.Update(proyecto);
                    return respuesta = new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Data = proyecto,
                        Code = ConstantMessagesCargarEnlaceMonitoreo.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Cargar_enlace_monitoreo, ConstantMessagesCargarEnlaceMonitoreo.OperacionExitosa, idAccion, proyecto.UsuarioModificacion, strCrearEditar)
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Cargar_enlace_monitoreo, ConstantMessagesDisponibilidadPresupuesta.Error, idAccion, proyecto.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }


    }
}
