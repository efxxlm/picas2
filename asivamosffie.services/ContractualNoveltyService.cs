﻿using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace asivamosffie.services
{
    //CU 4_4_1 Registrar actuaciones de controversias contractuales

    public class ContractualNoveltyService : IContractualNoveltyService
    {

        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;


        public ContractualNoveltyService(devAsiVamosFFIEContext context, ICommonService commonService)
        {

            _commonService = commonService;
            _context = context;

            //_settings = settings;
        }



        #region Grids

        /// <summary>
        /// CU 4.1.3 - get list of information about contractual modification 
        /// </summary>
        /// <returns></returns>
        public async Task<List<VNovedadContractual>> GetListGrillaNovedadContractualObra()
        {
            List<VNovedadContractual> ListNovedades = new List<VNovedadContractual>();

            try
            {
                ListNovedades = await _context.VNovedadContractual
                                                    .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                                                    .ToListAsync();

                return ListNovedades.OrderByDescending(r => r.FechaSolictud).ToList();
            }
            catch (Exception ex)
            {
                return ListNovedades;
            }
        }

        public async Task<List<VNovedadContractual>> GetListGrillaNovedadContractualInterventoria()
        {
            List<VNovedadContractual> ListNovedades = new List<VNovedadContractual>();

            try
            {
                ListNovedades = await _context.VNovedadContractual
                                                    .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                                                    .ToListAsync();

                return ListNovedades.OrderByDescending(r => r.FechaSolictud).ToList();
            }
            catch (Exception ex)
            {
                return ListNovedades;
            }
        }

        public async Task<List<VNovedadContractual>> GetListGrillaNovedadContractualGestionar()
        {
            List<VNovedadContractual> ListNovedades = new List<VNovedadContractual>();

            try
            {
                ListNovedades = await _context.VNovedadContractual
                                                    .Where(
                                                            r => r.EstadoCodigo != ConstanCodigoEstadoNovedadContractual.En_proceso_de_registro &&
                                                            r.EstadoCodigo != ConstanCodigoEstadoNovedadContractual.Con_novedad_aprobada_por_interventor &&
                                                            r.EstadoCodigo != ConstanCodigoEstadoNovedadContractual.En_proceso_de_verificacion   &&
                                                            r.EstadoCodigo != ConstanCodigoEstadoNovedadContractual.Sin_validar &&
                                                            r.EstadoCodigo != ConstanCodigoEstadoNovedadContractual.En_proceso_de_validacion &&
                                                            r.EstadoCodigo != ConstanCodigoEstadoNovedadContractual.Con_novedad_rechazada_por_supervisor &&
                                                            r.EstadoCodigo != ConstanCodigoEstadoNovedadContractual.Con_novedad_rechazada_por_interventor
                                                           )
                                                    .ToListAsync();

                foreach (var novedad in ListNovedades)
                {
                    NovedadContractual novedadContractual = _context.NovedadContractual
                                                                        .Where(x => x.NovedadContractualId == novedad.NovedadContractualId)
                                                                        .Include(x => x.NovedadContractualObservaciones)
                                                                        .FirstOrDefault();

                    if (novedadContractual.EstadoProcesoCodigo == "3")
                    {
                        novedadContractual.RegistroCompletoDevolucionTramite = true;

                        novedadContractual.ObservacionTramite = getObservacion(novedadContractual, null, true);

                        if (
                                novedadContractual.ObservacionTramite == null ||
                                string.IsNullOrEmpty(novedadContractual?.ObservacionTramite.Observaciones)
                            )
                        {
                            novedadContractual.RegistroCompletoDevolucionTramite = false;
                        }
                    }

                    novedad.novedadContractual = novedadContractual;
                }



                return ListNovedades.OrderByDescending(r => r.FechaSolictud).ToList();
            }
            catch (Exception ex)
            {
                return ListNovedades;
            }
        }

        #endregion Grids

        #region Gets

        /// <summary>
        /// CU 4.1.3 - get list of contracts assign to logged user for autocomplete
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<List<Contrato>> GetListContract(int userID)
        {
            List<Contrato> listaContratos = new List<Contrato>();

            List<Contrato> contratos = _context.Contrato
                                        .Where(x =>/*x.UsuarioInterventoria==userID*/ !(bool)x.Eliminado)
                                        .Include(x => x.Contratacion)
                                            .ThenInclude(x => x.DisponibilidadPresupuestal)
                                        .Include(x => x.Contratacion)
                                            .ThenInclude(x => x.Contratista)
                                        .Include(r => r.ContratoPoliza)
                                        .ToList();

            List<NovedadContractual> listaNovedadesActivas = _context.NovedadContractual
                                                                        .Where(x => (
                                                                                        x.EstadoCodigo != ConstanCodigoEstadoNovedadContractual.Con_novedad_aprobada_tecnica_y_juridicamente ||
                                                                                        x.EstadoCodigo != ConstanCodigoEstadoNovedadContractual.Con_novedad_rechazada_por_interventor
                                                                                    ) &&
                                                                               x.Eliminado != true)
                                                                        .ToList();

            List<Dominio> listDominioTipoDocumento = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Documento).ToList();

            foreach (var contrato in contratos)
            {
                if (
                        contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault()?.FechaDrp != null &&
                        contrato.ContratoPoliza?.OrderByDescending(r => r.FechaAprobacion)?.FirstOrDefault()?.FechaAprobacion != null &&

                        listaNovedadesActivas
                                    .Where(x => x.ContratoId == contrato.ContratoId)
                                    .Count() == 0
                    )
                {
                    contrato.Contratacion.Contratista.Contratacion = null;//para bajar el peso del consumo
                    contrato.Contratacion.Contratista.TipoIdentificacionNotMapped = listDominioTipoDocumento.Where(x => x.Codigo == contrato?.Contratacion?.Contratista?.TipoIdentificacionCodigo)?.FirstOrDefault()?.Nombre;
                    //contrato.TipoIntervencion no se de donde sale, preguntar, porque si es del proyecto, cuando sea multiproyecto cual traigo?
                    listaContratos.Add(contrato);
                }

            }
            return listaContratos;
        }

        public async Task<List<VProyectosXcontrato>> GetProyectsByContract(int pContratoId)
        {
            List<VProyectosXcontrato> listProyectos = _context.VProyectosXcontrato
                                        .Where(x => x.ContratoId == pContratoId)
                                        .ToList();

            return listProyectos;
        }

        public async Task<NovedadContractual> GetNovedadContractualById(int pId)
        {
            List<Dominio> listDominioTipoDocumento = _context.Dominio
                                                                .Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Documento)
                                                                .ToList();

            List<Dominio> listDominioTipoNovedad = _context.Dominio
                                                                .Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Novedad_Modificacion_Contractual)
                                                                .ToList();

            List<Dominio> listDominioMotivos = _context.Dominio
                                                                .Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Motivos_Novedad_contractual)
                                                                .ToList();

            List<Dominio> listDominioInstancias = _context.Dominio
                                                                .Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Instancias_de_seguimiento_tecnico)
                                                                .ToList();

            List<Dominio> listDominioComponente = _context.Dominio
                                                                .Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Componentes)
                                                                .ToList();

            List<Dominio> listDominioFase = _context.Dominio
                                                                .Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Fases)
                                                                .ToList();

            List<Dominio> listDominioUso = _context.Dominio
                                                                .Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Usos)
                                                                .ToList();


            NovedadContractual novedadContractual = _context.NovedadContractual
                                                                .Where(r => r.NovedadContractualId == pId)
                                                                .Include(r => r.NovedadContractualObservaciones)
                                                                .Include(r => r.Contrato)
                                                                    .ThenInclude(r => r.Contratacion)
                                                                        .ThenInclude(r => r.Contratista)
                                                                .Include(r => r.NovedadContractualDescripcion)
                                                                    .ThenInclude(r => r.NovedadContractualClausula)
                                                                .Include(r => r.NovedadContractualDescripcion)
                                                                    .ThenInclude(r => r.NovedadContractualDescripcionMotivo)
                                                                .Include(r => r.Contrato)
                                                                    .ThenInclude(r => r.Contratacion)
                                                                        .ThenInclude(r => r.DisponibilidadPresupuestal)
                                                                .Include(r => r.NovedadContractualAportante)
                                                                    .ThenInclude(r => r.ComponenteAportanteNovedad)
                                                                        .ThenInclude(r => r.ComponenteFuenteNovedad)
                                                                            .ThenInclude(r => r.ComponenteUsoNovedad)
                                                                .FirstOrDefault();

            if (novedadContractual != null)
            {
                novedadContractual.ProyectosContrato = _context.VProyectosXcontrato
                                                                .Where(r => r.ContratoId == novedadContractual.ContratoId)
                                                                .ToList();

                novedadContractual.ProyectosSeleccionado = _context.VProyectosXcontrato
                                                                        .Where(r => r.ProyectoId == novedadContractual.ProyectoId && r.ContratoId == novedadContractual.ContratoId)
                                                                        .FirstOrDefault();

                novedadContractual.InstanciaNombre = listDominioInstancias
                                                            .Where(r => r.Codigo == novedadContractual.InstanciaCodigo)
                                                            ?.FirstOrDefault()
                                                            ?.Nombre;

                novedadContractual.RegistroCompletoInformacionBasica = RegistroCompletoInformacionBasica(novedadContractual);
                novedadContractual.RegistroCompletoSoporte = RegistroCompletoSoporte(novedadContractual);
                novedadContractual.RegistroCompletoDescripcion = RegistroCompletoDescripcion(novedadContractual);

                novedadContractual.RegistroCompletoRevisionJuridica = RegistrocompletoRevisionJuridica(novedadContractual);
                novedadContractual.RegistroCompletoFirmas = RegistrocompletoFirmas(novedadContractual);
                novedadContractual.RegistroCompletoDetallar = RegistrocompletoDetallar(novedadContractual);

                novedadContractual.NovedadContractualDescripcion = novedadContractual.NovedadContractualDescripcion.Where(x => x.Eliminado != true).ToList();

                foreach (NovedadContractualDescripcion novedadContractualDescripcion in novedadContractual.NovedadContractualDescripcion)
                {
                    novedadContractualDescripcion.NombreTipoNovedad = listDominioTipoNovedad
                                                                            .Where(r => r.Codigo == novedadContractualDescripcion.TipoNovedadCodigo)
                                                                            ?.FirstOrDefault()
                                                                            ?.Nombre;
                    foreach (NovedadContractualDescripcionMotivo motivo in novedadContractualDescripcion.NovedadContractualDescripcionMotivo)
                    {
                        motivo.NombreMotivo = listDominioMotivos.Where(r => r.Codigo == motivo.MotivoNovedadCodigo)?.FirstOrDefault()?.Nombre;
                    }

                    novedadContractualDescripcion.NovedadContractualClausula = novedadContractualDescripcion.NovedadContractualClausula
                                                                                                                .Where(x => x.Eliminado != true)
                                                                                                                .ToList();

                }

                foreach (NovedadContractualAportante novedadContractualAportante in novedadContractual.NovedadContractualAportante)
                {
                    //novedadContractualAportante.NombreAportante = 

                    foreach (ComponenteAportanteNovedad componenteAportanteNovedad in novedadContractualAportante.ComponenteAportanteNovedad)
                    {
                        componenteAportanteNovedad.NombreTipoComponente = listDominioComponente
                                                                                .Where(r => r.Codigo == componenteAportanteNovedad.TipoComponenteCodigo)
                                                                                ?.FirstOrDefault()
                                                                                ?.Nombre;

                        componenteAportanteNovedad.Nombrefase = listDominioFase
                                                                                .Where(r => r.Codigo == componenteAportanteNovedad.FaseCodigo)
                                                                                ?.FirstOrDefault()
                                                                                ?.Nombre;

                        foreach (ComponenteFuenteNovedad componenteFuenteNovedad in componenteAportanteNovedad.ComponenteFuenteNovedad)
                        {

                            foreach (ComponenteUsoNovedad componenteUsoNovedad in componenteFuenteNovedad.ComponenteUsoNovedad)
                            {
                                componenteUsoNovedad.NombreUso = listDominioUso
                                                                            .Where(r => r.Codigo == componenteUsoNovedad.TipoUsoCodigo)
                                                                            ?.FirstOrDefault()
                                                                            ?.Nombre;
                            }
                        }


                    }
                }

                if (novedadContractual?.Contrato?.Contratacion?.Contratista != null)
                {
                    novedadContractual.Contrato.Contratacion.Contratista.Contratacion = null;//para bajar el peso del consumo
                    novedadContractual.Contrato.Contratacion.Contratista.TipoIdentificacionNotMapped = novedadContractual.Contrato.Contratacion.Contratista.TipoIdentificacionCodigo == null ? "" : listDominioTipoDocumento.Where(x => x.Codigo == novedadContractual.Contrato.Contratacion.Contratista.TipoIdentificacionCodigo)?.FirstOrDefault()?.Nombre;
                    //contrato.TipoIntervencion no se de donde sale, preguntar, porque si es del proyecto, cuando sea multiproyecto cual traigo?
                }

                novedadContractual.ObservacionApoyo = getObservacion(novedadContractual, false, null);
                novedadContractual.ObservacionSupervisor = getObservacion(novedadContractual, true, null);
                novedadContractual.ObservacionTramite = getObservacion(novedadContractual, null, true);

                novedadContractual.ObservacionDevolucion = _context.NovedadContractualObservaciones.Find(novedadContractual.ObervacionSupervisorId);
                novedadContractual.ObservacionDevolucionTramite = _context.NovedadContractualObservaciones.Find(novedadContractual.ObservacionesDevolucionId);

                if (novedadContractual.EstadoProcesoCodigo == "3")
                {
                    novedadContractual.RegistroCompletoDevolucionTramite = true;
                    if (
                            novedadContractual.ObservacionTramite == null ||
                            string.IsNullOrEmpty(novedadContractual?.ObservacionTramite.Observaciones)
                        )
                    {
                        novedadContractual.RegistroCompletoDevolucionTramite = false;
                    }
                }
            }
            else
            {
                novedadContractual = new NovedadContractual();
            }

            return novedadContractual;
        }

        public async Task<List<CofinanciacionAportante>> GetAportanteByContratacion(int pId)
        {
            List<CofinanciacionAportante> listaAportantes = new List<CofinanciacionAportante>();

            Contratacion contratacion = _context.Contratacion
                                                    .Include(x => x.ContratacionProyecto)
                                                        .ThenInclude(x => x.ContratacionProyectoAportante)
                                                            .ThenInclude(x => x.CofinanciacionAportante)
                                                    .FirstOrDefault(x => x.ContratacionId == pId);

            contratacion.ContratacionProyecto.ToList().ForEach(cp =>
           {
               cp.ContratacionProyectoAportante.ToList().ForEach(cpa =>
              {
                  CofinanciacionAportante cofinanciacionAportante = new CofinanciacionAportante();
                  cofinanciacionAportante.CofinanciacionAportanteId = cpa.CofinanciacionAportante.CofinanciacionAportanteId;

                  if (cpa.CofinanciacionAportante.TipoAportanteId == ConstanTipoAportante.Ffie)
                  {
                      cofinanciacionAportante.NombreAportanteString = ConstanStringTipoAportante.Ffie;
                  }
                  else if (cpa.CofinanciacionAportante.TipoAportanteId == ConstanTipoAportante.ET)
                  {
                      //verifico si tiene municipio
                      if (cpa.CofinanciacionAportante.MunicipioId != null)
                      {
                          cofinanciacionAportante.NombreAportanteString = _context.Localizacion.Find(cpa.CofinanciacionAportante.MunicipioId).Descripcion;
                      }
                      else//solo departamento
                      {
                          cofinanciacionAportante.NombreAportanteString = cpa.CofinanciacionAportante.DepartamentoId == null ? "" : _context.Localizacion.Find(cpa.CofinanciacionAportante.DepartamentoId).Descripcion;

                      }

                  }
                  else
                  {
                      cofinanciacionAportante.NombreAportanteString = _context.Dominio.Find(cpa.CofinanciacionAportante.NombreAportanteId).Nombre;
                  }
                  listaAportantes.Add(cofinanciacionAportante);
              });
           });

            return listaAportantes;
        }

        public async Task<List<FuenteFinanciacion>> GetFuentesByAportante(int pConfinanciacioAportanteId)
        {
            List<Dominio> listaDominio = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).ToList();

            List<FuenteFinanciacion> listaFuentes = _context.FuenteFinanciacion
                                                                .Where(x => x.AportanteId == pConfinanciacioAportanteId && 
                                                                       x.Eliminado != true)
                                                                .Include(x => x.Aportante)
                                                                .ToList();

            foreach (var fuente in listaFuentes)
            {
                fuente.FuenteRecursosString = listaDominio.Where(x => x.Codigo == fuente.FuenteRecursosCodigo)?.FirstOrDefault()?.Nombre;
                fuente.FuenteRecursosString = string.Concat(fuente.FuenteRecursosString, "-", fuente?.Aportante?.CofinanciacionId);

            }

            return listaFuentes;
        }

        #endregion Gets

        #region CreateEdit 

        public async Task<Respuesta> CreateEditNovedadContractual(NovedadContractual novedadContractual)
        {
            Respuesta _response = new Respuesta(); /*NovedadContractual novedadContractual*/

            Contrato contrato = _context.Contrato
                                            .Where(r => r.ContratoId == novedadContractual.ContratoId)
                                            .Include(r => r.Contratacion)
                                            .FirstOrDefault();

            int idAccionCrearEditarNovedadContractual = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;

            try
            {
                if (novedadContractual != null)
                {

                    if (string.IsNullOrEmpty(novedadContractual.NovedadContractualId.ToString()) || novedadContractual.NovedadContractualId == 0)
                    {

                        int cantidadNovedades = _context.NovedadContractual.ToList().Count();

                        //Auditoria
                        strCrearEditar = "REGISTRAR NOVEDAD CONTRACTUAL";
                        novedadContractual.FechaCreacion = DateTime.Now;
                        novedadContractual.UsuarioCreacion = novedadContractual.UsuarioCreacion;
                        novedadContractual.NumeroSolicitud = "NOV-" + cantidadNovedades.ToString("000");

                        if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                        {
                            novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.En_proceso_de_registro;
                        }
                        if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                        {
                            novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.En_proceso_de_verificacion;
                        }


                        novedadContractual.Eliminado = false;

                        if (novedadContractual.NovedadContractualDescripcion != null)
                        {
                            foreach (NovedadContractualDescripcion descripcion in novedadContractual.NovedadContractualDescripcion)
                            {
                                descripcion.UsuarioCreacion = novedadContractual.UsuarioCreacion;
                                descripcion.NovedadContractualId = novedadContractual.NovedadContractualId;

                                await CreateEditNovedadContractualDescripcion(descripcion);
                            }
                        }

                        novedadContractual.RegistroCompleto = Registrocompleto(novedadContractual);
                        _context.NovedadContractual.Add(novedadContractual);

                    }

                    else
                    {
                        strCrearEditar = "EDIT NOVEDAD CONTRACTUAL";

                        NovedadContractual novedadContractualOld = _context.NovedadContractual.Find(novedadContractual.NovedadContractualId);

                        novedadContractualOld.FechaModificacion = DateTime.Now;
                        novedadContractualOld.UsuarioModificacion = novedadContractual.UsuarioCreacion;

                        novedadContractualOld.ProyectoId = novedadContractual.ProyectoId;
                        novedadContractualOld.EsAplicadaAcontrato = novedadContractual.EsAplicadaAcontrato;
                        novedadContractualOld.FechaSolictud = novedadContractual.FechaSolictud;
                        novedadContractualOld.InstanciaCodigo = novedadContractual.InstanciaCodigo;
                        novedadContractualOld.FechaSesionInstancia = novedadContractual.FechaSesionInstancia;
                        novedadContractualOld.UrlSoporte = novedadContractual.UrlSoporte;

                        if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                        {
                            novedadContractualOld.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.En_proceso_de_verificacion;
                        }

                        

                        if (novedadContractual.NovedadContractualDescripcion != null)
                        {
                            List<NovedadContractualDescripcion> listaDescripciones = _context.NovedadContractualDescripcion
                                                                                                .Where(x => x.NovedadContractualId == novedadContractual.NovedadContractualId &&
                                                                                                        x.Eliminado != true)
                                                                                                .ToList();

                            foreach (NovedadContractualDescripcion novedadContractualDescripcion in listaDescripciones)
                            {
                                if (novedadContractual.NovedadContractualDescripcion.Where( x => x.NovedadContractualDescripcionId == novedadContractualDescripcion.NovedadContractualDescripcionId ).Count() == 0)
                                {
                                    novedadContractualDescripcion.Eliminado = true;
                                }
                            }

                            foreach (NovedadContractualDescripcion descripcion in novedadContractual.NovedadContractualDescripcion)
                            {
                                descripcion.UsuarioCreacion = novedadContractual.UsuarioCreacion;
                                descripcion.NovedadContractualId = novedadContractual.NovedadContractualId;

                                await CreateEditNovedadContractualDescripcion(descripcion);
                            }
                        }

                        novedadContractualOld.RegistroCompleto = Registrocompleto(novedadContractualOld);
                    }

                    _context.SaveChanges();

                    return
                        new Respuesta
                        {
                            Data = novedadContractual,
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContractualNovelty.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_solicitud_novedad_contractual,
                            ConstantMessagesContractualNovelty.OperacionExitosa,
                            idAccionCrearEditarNovedadContractual
                            , novedadContractual.UsuarioModificacion
                            , "EDITAR NOVEDAD CONTRACTUAL"
                            )
                        };

                }
                else
                {
                    return _response = new Respuesta
                    {
                        IsSuccessful = false,
                        IsValidation = false,
                        Data = null,
                        Code = ConstantMessagesContractualNovelty.RecursoNoEncontrado,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_solicitud_novedad_contractual, ConstantMessagesContractualNovelty.RecursoNoEncontrado, idAccionCrearEditarNovedadContractual, novedadContractual.UsuarioCreacion, strCrearEditar)
                    };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_solicitud_novedad_contractual, GeneralCodes.Error, idAccionCrearEditarNovedadContractual, novedadContractual.UsuarioCreacion, strCrearEditar)
                };
            }
        }

        public async Task<Respuesta> CreateEditNovedadContractualDescripcion(NovedadContractualDescripcion novedadContractualDescripcion)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);//ERROR VALIDAR ACCIONES

            string strCrearEditar = "";
            try
            {

                if (string.IsNullOrEmpty(novedadContractualDescripcion.NovedadContractualDescripcionId.ToString()) || novedadContractualDescripcion.NovedadContractualDescripcionId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR NOVEDAD CONTRACTUAL DESCRIPCION";
                    novedadContractualDescripcion.FechaCreacion = DateTime.Now;
                    novedadContractualDescripcion.Eliminado = false;

                    _context.NovedadContractualDescripcion.Add(novedadContractualDescripcion);

                }
                else
                {
                    strCrearEditar = "EDIT NOVEDAD CONTRACTUAL DESCRIPCION";
                    NovedadContractualDescripcion novedadDescripcionOld = _context.NovedadContractualDescripcion.Find(novedadContractualDescripcion.NovedadContractualDescripcionId);

                    //Auditoria
                    novedadDescripcionOld.FechaModificacion = DateTime.Now;
                    novedadDescripcionOld.UsuarioCreacion = novedadContractualDescripcion.UsuarioCreacion;

                    //Registros

                    novedadDescripcionOld.MotivoNovedadCodigo = novedadContractualDescripcion.MotivoNovedadCodigo;
                    novedadDescripcionOld.ResumenJustificacion = novedadContractualDescripcion.ResumenJustificacion;
                    novedadDescripcionOld.EsDocumentacionSoporte = novedadContractualDescripcion.EsDocumentacionSoporte;
                    novedadDescripcionOld.ConceptoTecnico = novedadContractualDescripcion.ConceptoTecnico;
                    novedadDescripcionOld.FechaConcepto = novedadContractualDescripcion.FechaConcepto;
                    novedadDescripcionOld.FechaInicioSuspension = novedadContractualDescripcion.FechaInicioSuspension;
                    novedadDescripcionOld.FechaFinSuspension = novedadContractualDescripcion.FechaFinSuspension;
                    novedadDescripcionOld.PresupuestoAdicionalSolicitado = novedadContractualDescripcion.PresupuestoAdicionalSolicitado;
                    novedadDescripcionOld.PlazoAdicionalDias = novedadContractualDescripcion.PlazoAdicionalDias;
                    novedadDescripcionOld.PlazoAdicionalMeses = novedadContractualDescripcion.PlazoAdicionalMeses;
                    novedadDescripcionOld.ClausulaModificar = novedadContractualDescripcion.ClausulaModificar;
                    novedadDescripcionOld.AjusteClausula = novedadContractualDescripcion.AjusteClausula;
                    novedadDescripcionOld.RegistroCompleto = novedadContractualDescripcion.RegistroCompleto;
                    novedadDescripcionOld.NumeroRadicado = novedadContractualDescripcion.NumeroRadicado;

                    _context.NovedadContractualDescripcion.Update(novedadDescripcionOld);
                }

                if (novedadContractualDescripcion.TipoNovedadCodigo == ConstanTiposNovedades.Modificación_de_Condiciones_Contractuales &&
                    (novedadContractualDescripcion.NovedadContractualClausula == null || novedadContractualDescripcion.NovedadContractualClausula.Count() == 0)
                    )
                {
                    NovedadContractualClausula novedadContractualClausula = new NovedadContractualClausula();
                    novedadContractualDescripcion.NovedadContractualClausula.Add(novedadContractualClausula);
                }

                foreach (NovedadContractualClausula clausula in novedadContractualDescripcion.NovedadContractualClausula)
                {
                    clausula.UsuarioCreacion = novedadContractualDescripcion.UsuarioCreacion;
                    //clausula.NovedadContractualDescripcionId = novedadContractualDescripcion.NovedadContractualDescripcionId;

                    await CreateEditNovedadContractualDescripcionClausula(clausula);
                }

                //las borro los motivos para crearlos de nuevo
                List<NovedadContractualDescripcionMotivo> listaMotivos = _context.NovedadContractualDescripcionMotivo
                                                                                       .Where(r => r.NovedadContractualDescripcionId == novedadContractualDescripcion.NovedadContractualDescripcionId)
                                                                                       .ToList();

                _context.NovedadContractualDescripcionMotivo.RemoveRange(listaMotivos);

                // creo los motivos de nuevo
                foreach (NovedadContractualDescripcionMotivo motivo in novedadContractualDescripcion.NovedadContractualDescripcionMotivo)
                {
                    motivo.UsuarioCreacion = novedadContractualDescripcion.UsuarioCreacion;
                    motivo.FechaCreacion = DateTime.Now;

                    _context.NovedadContractualDescripcionMotivo.Add(motivo);
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Grupo, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, novedadContractualDescripcion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> CreateEditNovedadContractualUso(ComponenteUsoNovedad componenteUsoNovedad)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual_Tramite, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = "";
            try
            {

                if (string.IsNullOrEmpty(componenteUsoNovedad.ComponenteUsoNovedadId.ToString()) || componenteUsoNovedad.ComponenteUsoNovedadId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR NOVEDAD CONTRACTUAL USO";
                    componenteUsoNovedad.FechaCreacion = DateTime.Now;
                    componenteUsoNovedad.Eliminado = false;

                    _context.ComponenteUsoNovedad.Add(componenteUsoNovedad);

                }
                else
                {
                    strCrearEditar = "EDIT NOVEDAD CONTRACTUAL USO";
                    ComponenteUsoNovedad componenteUsoNovedadOld = _context.ComponenteUsoNovedad.Find(componenteUsoNovedad.ComponenteUsoNovedadId);

                    //Auditoria
                    componenteUsoNovedadOld.FechaModificacion = DateTime.Now;
                    componenteUsoNovedadOld.UsuarioCreacion = componenteUsoNovedad.UsuarioCreacion;

                    //Registros

                    componenteUsoNovedadOld.TipoUsoCodigo = componenteUsoNovedad.TipoUsoCodigo;
                    componenteUsoNovedadOld.ValorUso = componenteUsoNovedad.ValorUso;

                    _context.ComponenteUsoNovedad.Update(componenteUsoNovedadOld);
                }


                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Grupo, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, componenteUsoNovedad.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> CreateEditNovedadContractualFuente(ComponenteFuenteNovedad componenteFuenteNovedad)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual_Tramite, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = "";
            try
            {

                if (string.IsNullOrEmpty(componenteFuenteNovedad.ComponenteFuenteNovedadId.ToString()) || componenteFuenteNovedad.ComponenteFuenteNovedadId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR NOVEDAD CONTRACTUAL FUENTE";
                    componenteFuenteNovedad.FechaCreacion = DateTime.Now;
                    componenteFuenteNovedad.Eliminado = false;

                    _context.ComponenteFuenteNovedad.Add(componenteFuenteNovedad);

                }
                else
                {
                    strCrearEditar = "EDIT NOVEDAD CONTRACTUAL FUENTE";
                    ComponenteFuenteNovedad componenteFuenteNovedadOld = _context.ComponenteFuenteNovedad.Find(componenteFuenteNovedad.ComponenteFuenteNovedadId);

                    //Auditoria
                    componenteFuenteNovedadOld.FechaModificacion = DateTime.Now;
                    componenteFuenteNovedadOld.UsuarioCreacion = componenteFuenteNovedadOld.UsuarioCreacion;

                    //Registros

                    componenteFuenteNovedadOld.FuenteRecursosCodigo = componenteFuenteNovedad.FuenteRecursosCodigo;


                    _context.ComponenteFuenteNovedad.Update(componenteFuenteNovedadOld);
                }

                foreach (ComponenteUsoNovedad uso in componenteFuenteNovedad.ComponenteUsoNovedad)
                {
                    uso.UsuarioCreacion = componenteFuenteNovedad.UsuarioCreacion;

                    await CreateEditNovedadContractualUso(uso);
                }



                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Grupo, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, componenteFuenteNovedad.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> CreateEditNovedadContractualComponente(ComponenteAportanteNovedad componenteAportanteNovedad)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual_Tramite, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = "";
            try
            {

                if (string.IsNullOrEmpty(componenteAportanteNovedad.ComponenteAportanteNovedadId.ToString()) || componenteAportanteNovedad.ComponenteAportanteNovedadId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR NOVEDAD CONTRACTUAL COMPONENTE";
                    componenteAportanteNovedad.FechaCreacion = DateTime.Now;
                    componenteAportanteNovedad.Eliminado = false;

                    _context.ComponenteAportanteNovedad.Add(componenteAportanteNovedad);

                }
                else
                {
                    strCrearEditar = "EDIT NOVEDAD CONTRACTUAL COMPONENTE";
                    ComponenteAportanteNovedad componenteAportanteNovedadOld = _context.ComponenteAportanteNovedad.Find(componenteAportanteNovedad.ComponenteAportanteNovedadId);

                    //Auditoria
                    componenteAportanteNovedadOld.FechaModificacion = DateTime.Now;
                    componenteAportanteNovedadOld.UsuarioCreacion = componenteAportanteNovedad.UsuarioCreacion;

                    //Registros

                    componenteAportanteNovedadOld.TipoComponenteCodigo = componenteAportanteNovedad.TipoComponenteCodigo;
                    componenteAportanteNovedadOld.FaseCodigo = componenteAportanteNovedad.FaseCodigo;

                    _context.ComponenteAportanteNovedad.Update(componenteAportanteNovedadOld);
                }

                foreach (ComponenteFuenteNovedad fuente in componenteAportanteNovedad.ComponenteFuenteNovedad)
                {
                    fuente.UsuarioCreacion = componenteAportanteNovedad.UsuarioCreacion;

                    await CreateEditNovedadContractualFuente(fuente);
                }



                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Grupo, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, componenteAportanteNovedad.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }


        public async Task<Respuesta> CreateEditNovedadContractualAportante(NovedadContractualAportante novedadContractualAportante)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual_Tramite, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = "";
            try
            {

                if (string.IsNullOrEmpty(novedadContractualAportante.NovedadContractualAportanteId.ToString()) || novedadContractualAportante.NovedadContractualAportanteId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR NOVEDAD CONTRACTUAL APORTANTE";
                    novedadContractualAportante.FechaCreacion = DateTime.Now;
                    novedadContractualAportante.Eliminado = false;

                    _context.NovedadContractualAportante.Add(novedadContractualAportante);

                }
                else
                {
                    strCrearEditar = "EDIT NOVEDAD CONTRACTUAL APORTANTE";
                    NovedadContractualAportante novedadContractualAportanteOld = _context.NovedadContractualAportante.Find(novedadContractualAportante.NovedadContractualAportanteId);

                    //Auditoria
                    novedadContractualAportanteOld.FechaModificacion = DateTime.Now;
                    novedadContractualAportanteOld.UsuarioCreacion = novedadContractualAportante.UsuarioCreacion;

                    //Registros

                    novedadContractualAportanteOld.CofinanciacionAportanteId = novedadContractualAportante.CofinanciacionAportanteId;
                    novedadContractualAportanteOld.ValorAporte = novedadContractualAportante.ValorAporte;

                    _context.NovedadContractualAportante.Update(novedadContractualAportanteOld);
                }

                foreach (ComponenteAportanteNovedad componente in novedadContractualAportante.ComponenteAportanteNovedad)
                {
                    componente.UsuarioCreacion = novedadContractualAportante.UsuarioCreacion;

                    await CreateEditNovedadContractualComponente(componente);
                }



                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Grupo, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, novedadContractualAportante.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> CreateEditNovedadContractualDescripcionClausula(NovedadContractualClausula novedadContractualClausula)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);//ERROR VALIDAR ACCIONES

            string strCrearEditar = "";
            try
            {

                if (string.IsNullOrEmpty(novedadContractualClausula.NovedadContractualClausulaId.ToString()) || novedadContractualClausula.NovedadContractualClausulaId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR NOVEDAD CONTRACTUAL CLAUSULA";
                    novedadContractualClausula.FechaCreacion = DateTime.Now;
                    novedadContractualClausula.Eliminado = false;

                    _context.NovedadContractualClausula.Add(novedadContractualClausula);
                }
                else
                {
                    strCrearEditar = "EDIT NOVEDAD CONTRACTUAL CLAUSULA";
                    NovedadContractualClausula clausulaOld = _context.NovedadContractualClausula.Find(novedadContractualClausula.NovedadContractualClausulaId);

                    //Auditoria
                    clausulaOld.FechaModificacion = DateTime.Now;
                    clausulaOld.UsuarioCreacion = novedadContractualClausula.UsuarioCreacion;

                    //Registros

                    clausulaOld.AjusteSolicitadoAclausula = novedadContractualClausula.AjusteSolicitadoAclausula;
                    clausulaOld.ClausulaAmodificar = novedadContractualClausula.ClausulaAmodificar;

                    _context.NovedadContractualClausula.Update(clausulaOld);
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Grupo, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, novedadContractualClausula.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> CreateEditObservacion(NovedadContractual pNovedadContractual, bool? esSupervisor, bool? esTramite)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";

            try
            {
                CreateEdit = "EDIT NOVEDAD CONTRACTUAL";
                int idObservacion = 0;

                if (pNovedadContractual.NovedadContractualObservaciones.Count() > 0)
                    idObservacion = pNovedadContractual.NovedadContractualObservaciones.FirstOrDefault().NovedadContractualObservacionesId;

                NovedadContractual novedadContractual = _context.NovedadContractual.Find(pNovedadContractual.NovedadContractualId);

                novedadContractual.UsuarioModificacion = pNovedadContractual.UsuarioCreacion;
                novedadContractual.FechaModificacion = DateTime.Now;

                if (esTramite == true)
                {
                    if (novedadContractual.EstadoProcesoCodigo == "3")
                    {

                        await CreateEditObservacionSeguimientoDiario(pNovedadContractual.NovedadContractualObservaciones.FirstOrDefault(), pNovedadContractual.UsuarioCreacion);
                    }
                    else
                    {
                        NovedadContractualObservaciones observacionDelete = _context.NovedadContractualObservaciones.Find(idObservacion);

                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }
                }
                else
                {
                    if (esSupervisor == true)
                    {
                        novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.En_proceso_de_validacion;

                        novedadContractual.TieneObservacionesSupervisor = pNovedadContractual.TieneObservacionesSupervisor;

                        if (novedadContractual.TieneObservacionesSupervisor.HasValue ? novedadContractual.TieneObservacionesSupervisor.Value : false)
                        {

                            await CreateEditObservacionSeguimientoDiario(pNovedadContractual.NovedadContractualObservaciones.FirstOrDefault(), pNovedadContractual.UsuarioCreacion);
                        }
                        else
                        {
                            NovedadContractualObservaciones observacionDelete = _context.NovedadContractualObservaciones.Find(idObservacion);

                            if (observacionDelete != null)
                                observacionDelete.Eliminado = true;
                        }

                        novedadContractual.RegistroCompletoValidacion = await ValidarRegistroCompletoValidacion(novedadContractual.NovedadContractualId, esSupervisor, esTramite);
                        if (novedadContractual.RegistroCompletoValidacion.Value)
                        {
                            novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.En_proceso_de_validacion;
                        }
                        else
                        {
                            novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Sin_validar;
                        }


                    }
                    else
                    {
                        novedadContractual.TieneObservacionesApoyo = pNovedadContractual.TieneObservacionesApoyo;

                        if (novedadContractual.TieneObservacionesApoyo.HasValue ? novedadContractual.TieneObservacionesApoyo.Value : true)
                        {
                            await CreateEditObservacionSeguimientoDiario(pNovedadContractual.NovedadContractualObservaciones.FirstOrDefault(), pNovedadContractual.UsuarioCreacion);
                        }
                        else
                        {
                            NovedadContractualObservaciones observacionDelete = _context.NovedadContractualObservaciones.Find(idObservacion);

                            if (observacionDelete != null)
                                observacionDelete.Eliminado = true;
                        }

                        novedadContractual.RegistroCompletoVerificacion = await ValidarRegistroCompletoVerificacion(novedadContractual.NovedadContractualId, esSupervisor, esTramite);
                        if (novedadContractual.RegistroCompletoVerificacion.Value)
                        {
                            novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.En_proceso_de_verificacion;
                        }
                        else
                        {
                            novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Con_novedad_aprobada_por_interventor;
                        }
                    }
                }







                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_solicitud_novedad_contractual, GeneralCodes.OperacionExitosa, idAccion, pNovedadContractual.UsuarioCreacion, CreateEdit)
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
                        Code = GeneralCodes.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_seguimiento_diario, GeneralCodes.Error, idAccion, pNovedadContractual.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> CreateEditNovedadContractualTramite(NovedadContractual novedadContractual)
        {
            Respuesta _response = new Respuesta(); /*NovedadContractual novedadContractual*/

            Contrato contrato = _context.Contrato
                                            .Where(r => r.ContratoId == novedadContractual.ContratoId)
                                            .Include(r => r.Contratacion)
                                            .FirstOrDefault();

            int idAccionCrearEditarNovedadContractual = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual_Tramite, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;

            try
            {
                strCrearEditar = "EDIT NOVEDAD CONTRACTUAL TRAMITE";

                NovedadContractual novedadContractualOld = _context.NovedadContractual.Find(novedadContractual.NovedadContractualId);

                novedadContractualOld.FechaModificacion = DateTime.Now;
                novedadContractualOld.UsuarioModificacion = novedadContractual.UsuarioCreacion;

                novedadContractualOld.FechaEnvioGestionContractual = novedadContractual.FechaEnvioGestionContractual;
                novedadContractualOld.EstadoProcesoCodigo = novedadContractual.EstadoProcesoCodigo;
                novedadContractualOld.FechaAprobacionGestionContractual = novedadContractual.FechaAprobacionGestionContractual;
                novedadContractualOld.AbogadoRevisionId = novedadContractual.AbogadoRevisionId;

                novedadContractualOld.DeseaContinuar = novedadContractual.DeseaContinuar;
                novedadContractualOld.FechaEnvioActaContratistaObra = novedadContractual.FechaEnvioActaContratistaObra;
                novedadContractualOld.FechaFirmaActaContratistaObra = novedadContractual.FechaFirmaActaContratistaObra;
                novedadContractualOld.FechaEnvioActaContratistaInterventoria = novedadContractual.FechaEnvioActaContratistaInterventoria;
                novedadContractualOld.FechaFirmaContratistaInterventoria = novedadContractual.FechaFirmaContratistaInterventoria;
                novedadContractualOld.FechaEnvioActaApoyo = novedadContractual.FechaEnvioActaApoyo;
                novedadContractualOld.FechaFirmaApoyo = novedadContractual.FechaFirmaApoyo;
                novedadContractualOld.FechaEnvioActaSupervisor = novedadContractual.FechaEnvioActaSupervisor;
                novedadContractualOld.FechaFirmaSupervisor = novedadContractual.FechaFirmaSupervisor;
                novedadContractualOld.UrlSoporteFirmas = novedadContractual.UrlSoporteFirmas;

                novedadContractualOld.RazonesNoContinuaProceso = novedadContractual.RazonesNoContinuaProceso;

                switch (novedadContractualOld.EstadoProcesoCodigo)
                {
                    // aprobada
                    case "1":
                        {
                            if (
                                //(
                                //  novedadContractualOld.EstadoCodigo == ConstanCodigoEstadoNovedadContractual.Sin_tramite ||
                                //  novedadContractualOld.EstadoCodigo == ConstanCodigoEstadoNovedadContractual.Sin_tramite
                                //) &&
                                novedadContractualOld.FechaEnvioGestionContractual != null &&
                                novedadContractualOld.FechaAprobacionGestionContractual != null &&
                                novedadContractualOld.AbogadoRevisionId != null
                            )
                            {
                                novedadContractualOld.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.En_registro_de_firmas;
                            }
                            break;
                        }
                    // en revision
                    case "2":
                        {
                            novedadContractualOld.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.En_proceso_de_aprobacion;
                            break;
                        }

                }


                foreach (NovedadContractualAportante aportante in novedadContractual.NovedadContractualAportante)
                {
                    aportante.UsuarioCreacion = novedadContractual.UsuarioCreacion;

                    await CreateEditNovedadContractualAportante(aportante);
                }

                novedadContractualOld.RegistroCompletoTramiteNovedades = RegistrocompletoTramite(novedadContractualOld);

                _context.SaveChanges();

                await CreateEditObservacion(novedadContractual, null, true);

                return
                    new Respuesta
                    {
                        Data = novedadContractual,
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantMessagesContractualNovelty.OperacionExitosa,
                        Message =
                        await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_solicitud_novedad_contractual,
                        ConstantMessagesContractualNovelty.OperacionExitosa,
                        idAccionCrearEditarNovedadContractual
                        , novedadContractual.UsuarioModificacion
                        , "EDITAR NOVEDAD CONTRACTUAL"
                        )
                    };


            }
            catch (Exception ex)
            {
                return _response = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_solicitud_novedad_contractual, GeneralCodes.Error, idAccionCrearEditarNovedadContractual, novedadContractual.UsuarioCreacion, strCrearEditar)
                };
            }
        }

        #endregion CreateEdit 

        #region business

        /// <summary>
        /// CU 4.1.3 - Delete a contractual modification 
        /// </summary>
        /// <param name="pNovedadContractualId"></param>
        /// <param name="pUsuario"></param>
        /// <returns></returns>
        public async Task<Respuesta> EliminarNovedadContractual(int pNovedadContractualId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.EliminarNovedadContractual, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            NovedadContractual novedadContractual = _context.NovedadContractual.Find(pNovedadContractualId);

            try
            {

                if (novedadContractual != null)
                {
                    strCrearEditar = "Eliminar NOVEDAD CONTRACTUAL";
                    novedadContractual.FechaModificacion = DateTime.Now;
                    novedadContractual.UsuarioModificacion = pUsuario;
                    novedadContractual.Eliminado = true;
                    _context.NovedadContractual.Update(novedadContractual);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_solicitud_novedad_contractual,
                    ConstantMessagesContractualControversy.EliminacionExitosa,
                    idAccion,
                    pUsuario,
                    strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.Error,
                    idAccion,
                    pUsuario,
                    ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> EliminarNovedadClausula(int pNovedadContractuaClausulalId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.EliminarNovedadContractualClausula, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            NovedadContractualClausula novedadContractualClausula = _context.NovedadContractualClausula.Find(pNovedadContractuaClausulalId);

            try
            {

                if (novedadContractualClausula != null)
                {
                    strCrearEditar = "Eliminar NOVEDAD CONTRACTUAL";
                    novedadContractualClausula.FechaModificacion = DateTime.Now;
                    novedadContractualClausula.UsuarioModificacion = pUsuario;
                    novedadContractualClausula.Eliminado = true;
                    _context.NovedadContractualClausula.Update(novedadContractualClausula);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = novedadContractualClausula,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_solicitud_novedad_contractual,
                    ConstantMessagesContractualControversy.EliminacionExitosa,
                    idAccion,
                    pUsuario,
                    strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = novedadContractualClausula,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.Error,
                    idAccion,
                    pUsuario,
                    ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> AprobarSolicitud(int pNovedadContractualId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.AprobarNovedadContractual, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            NovedadContractual novedadContractual = _context.NovedadContractual.Find(pNovedadContractualId);

            try
            {

                if (novedadContractual != null)
                {
                    strCrearEditar = "APROBAR NOVEDAD CONTRACTUAL";
                    novedadContractual.FechaModificacion = DateTime.Now;
                    novedadContractual.UsuarioModificacion = pUsuario;
                    novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Con_novedad_aprobada_por_interventor;
                    _context.NovedadContractual.Update(novedadContractual);

                    _context.SaveChanges();

                    EnviarNotificacionAApoyo(novedadContractual);
                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.OperacionExitosa,
                    idAccion,
                    pUsuario,
                    strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.Error,
                    idAccion,
                    pUsuario,
                    ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> EnviarAlSupervisor(int pNovedadContractualId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.VerificarNovedadContractual, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            NovedadContractual novedadContractual = _context.NovedadContractual.Find(pNovedadContractualId);

            try
            {

                if (novedadContractual != null)
                {
                    strCrearEditar = "VERIFICAR NOVEDAD CONTRACTUAL";
                    novedadContractual.FechaModificacion = DateTime.Now;
                    novedadContractual.UsuarioModificacion = pUsuario;
                    novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Sin_validar;
                    novedadContractual.FechaVerificacion = DateTime.Now;


                    _context.NovedadContractual.Update(novedadContractual);

                    _context.SaveChanges();

                    EnviarNotificacionASupervisor(novedadContractual);

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.OperacionExitosa,
                    idAccion,
                    pUsuario,//controversiaActuacion.UsuarioCreacion,
                    strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.Error,
                    idAccion,
                    pUsuario,
                    ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> RechazarPorInterventor(NovedadContractual pNovedadContractual, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.RechazarNovedadPorInterventor, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            NovedadContractual novedadContractual = _context.NovedadContractual.Find(pNovedadContractual.NovedadContractualId);

            try
            {

                if (novedadContractual != null)
                {
                    strCrearEditar = "RECHAZAR NOVEDAD POR INTERVENTOR";
                    novedadContractual.FechaModificacion = DateTime.Now;
                    novedadContractual.UsuarioModificacion = pUsuario;
                    novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Con_novedad_rechazada_por_interventor;
                    novedadContractual.CausaRechazo = pNovedadContractual.CausaRechazo;
                    _context.NovedadContractual.Update(novedadContractual);

                    _context.SaveChanges();

                    EnviarNotificacionRechazo(novedadContractual);
                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.OperacionExitosa,
                    idAccion,
                    pUsuario,
                    strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.Error,
                    idAccion,
                    pUsuario,
                    ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> RechazarPorSupervisor(NovedadContractual pNovedadContractual, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.RechazarNovedadPorInterventor, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            NovedadContractual novedadContractual = _context.NovedadContractual.Find(pNovedadContractual.NovedadContractualId);

            try
            {

                if (novedadContractual != null)
                {
                    strCrearEditar = "RECHAZAR NOVEDAD POR SUPERVISOR";
                    novedadContractual.FechaModificacion = DateTime.Now;
                    novedadContractual.UsuarioModificacion = pUsuario;
                    novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Con_novedad_rechazada_por_supervisor;
                    novedadContractual.CausaRechazo = pNovedadContractual.CausaRechazo;
                    _context.NovedadContractual.Update(novedadContractual);

                    _context.SaveChanges();

                    EnviarNotificacionRechazoSupervisor(novedadContractual);

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.OperacionExitosa,
                    idAccion,
                    pUsuario,
                    strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.Error,
                    idAccion,
                    pUsuario,
                    ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> TramitarSolicitud(int pNovedadContractualId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Tramitar_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            NovedadContractual novedadContractual = _context.NovedadContractual.Find(pNovedadContractualId);

            try
            {

                if (novedadContractual != null)
                {
                    strCrearEditar = "TRAMITAR NOVEDAD CONTRACTUAL";
                    novedadContractual.FechaModificacion = DateTime.Now;
                    novedadContractual.UsuarioModificacion = pUsuario;
                    novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Sin_tramite;
                    novedadContractual.FechaValidacion = DateTime.Now;

                    _context.NovedadContractual.Update(novedadContractual);

                    List<NovedadContractualObservaciones> listaObservaciones = _context.NovedadContractualObservaciones
                                                                                            .Where(x => x.EsTramiteNovedades == true &&
                                                                                                   x.Eliminado != true)
                                                                                            .ToList();

                    foreach( NovedadContractualObservaciones observacion in listaObservaciones)
                    {
                        observacion.Archivado = true;

                        _context.NovedadContractualObservaciones.Update(observacion);
                    }

                    _context.SaveChanges();

                    EnviarNotificacionATramitador(novedadContractual);

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.OperacionExitosa,
                    idAccion,
                    pUsuario,
                    strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.Error,
                    idAccion,
                    pUsuario,
                    ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> DevolverSolicitud(int pNovedadContractualId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Devolver_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = "DEVOLVER NOVEDAD CONTRACTUAL";

            try
            {

                NovedadContractual novedadContractual = _context.NovedadContractual
                                                                    .Where(r => r.NovedadContractualId == pNovedadContractualId)
                                                                    .Include(r => r.NovedadContractualObservaciones)
                                                                    .FirstOrDefault();

                novedadContractual.UsuarioModificacion = pUsuario;
                novedadContractual.FechaModificacion = DateTime.Now;

                if (novedadContractual.TieneObservacionesApoyo == true)
                {

                    NovedadContractualObservaciones observacionesApoyo = getObservacion(novedadContractual, false, null);

                    if (observacionesApoyo != null)
                        observacionesApoyo.Archivado = true;

                }

                if (novedadContractual.TieneObservacionesSupervisor == true)
                {

                    NovedadContractualObservaciones observacionesSupervisor = getObservacion(novedadContractual, true, null);

                    if (observacionesSupervisor != null)
                    {
                        observacionesSupervisor.Archivado = true;
                        novedadContractual.ObervacionSupervisorId = observacionesSupervisor.NovedadContractualObservacionesId;
                    }

                }

                novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Con_observaciones_del_supervisor;
                novedadContractual.TieneObservacionesApoyo = null;
                novedadContractual.TieneObservacionesSupervisor = null;
                novedadContractual.RegistroCompleto = null;
                novedadContractual.RegistroCompletoValidacion = null;
                novedadContractual.RegistroCompletoVerificacion = null;

                _context.SaveChanges();

                EnviarNotificacionDevolucion(novedadContractual);

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.OperacionExitosa,
                    idAccion,
                    pUsuario,
                    strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = ex,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.Error,
                    idAccion,
                    pUsuario,
                    ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> DevolverSolicitudASupervisor(int pNovedadContractualId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Devolver_Novedad_Contractual_a_supervisor, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = "DEVOLVER NOVEDAD CONTRACTUAL A SUPERVISOR";

            try
            {

                NovedadContractual novedadContractual = _context.NovedadContractual
                                                                    .Where(r => r.NovedadContractualId == pNovedadContractualId)
                                                                    .Include(r => r.NovedadContractualObservaciones)
                                                                    .FirstOrDefault();

                novedadContractual.UsuarioModificacion = pUsuario;
                novedadContractual.FechaModificacion = DateTime.Now;


                NovedadContractualObservaciones observaciones = getObservacion(novedadContractual, null, true);

                if (observaciones != null)
                {
                    observaciones.Archivado = true;
                    novedadContractual.ObservacionesDevolucionId = observaciones.NovedadContractualObservacionesId;
                }


                novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Devuelta_para_ajustes_de_supervisión;
                novedadContractual.TieneObservacionesApoyo = null;
                novedadContractual.TieneObservacionesSupervisor = null;
                novedadContractual.RegistroCompletoValidacion = null;

                _context.SaveChanges();

                //EnviarNotificacionDevolucion(novedadContractual);

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.OperacionExitosa,
                    idAccion,
                    pUsuario,
                    strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = ex,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.Error,
                    idAccion,
                    pUsuario,
                    ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> EnviarAComite(int pNovedadContractualId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_A_Comite_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = "ENVIAR A COMITE NOVEDAD CONTRACTUAL";

            try
            {

                NovedadContractual novedadContractual = _context.NovedadContractual
                                                                    .Where(r => r.NovedadContractualId == pNovedadContractualId)
                                                                    .Include(r => r.NovedadContractualObservaciones)
                                                                    .FirstOrDefault();

                novedadContractual.UsuarioModificacion = pUsuario;
                novedadContractual.FechaModificacion = DateTime.Now;

                novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Enviada_a_comite_tecnico;

                _context.SaveChanges();



                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.OperacionExitosa,
                    idAccion,
                    pUsuario,
                    strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = ex,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.Error,
                    idAccion,
                    pUsuario,
                    ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> CancelarNovedad(int pNovedadContractualId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.CancelarNovedadContractual, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            NovedadContractual novedadContractual = _context.NovedadContractual.Find(pNovedadContractualId);

            try
            {

                if (novedadContractual != null)
                {
                    strCrearEditar = "CANCELAR NOVEDAD";
                    novedadContractual.FechaModificacion = DateTime.Now;
                    novedadContractual.UsuarioModificacion = pUsuario;

                    novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Novedad_Cancelada;

                    _context.NovedadContractual.Update(novedadContractual);

                    _context.SaveChanges();

                    EnviarNotificacionCancelar(novedadContractual);
                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.OperacionExitosa,
                    idAccion,
                    pUsuario,
                    strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.Error,
                    idAccion,
                    pUsuario,
                    ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        #endregion business

        #region private 

        private async Task<Respuesta> CreateEditObservacionSeguimientoDiario(NovedadContractualObservaciones pObservacion, string pUsuarioCreacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);

            Respuesta respuesta = new Respuesta();
            try
            {
                string strCrearEditar = "";
                if (pObservacion.NovedadContractualObservacionesId > 0)
                {
                    strCrearEditar = "EDITAR OBSERVACION NOVEDAD CONTRACTUAL";
                    NovedadContractualObservaciones novedadContractualObservaciones = _context.NovedadContractualObservaciones.Find(pObservacion.NovedadContractualObservacionesId);

                    novedadContractualObservaciones.FechaModificacion = DateTime.Now;
                    novedadContractualObservaciones.UsuarioModificacion = pUsuarioCreacion;

                    novedadContractualObservaciones.Observaciones = pObservacion.Observaciones;

                }
                else
                {
                    strCrearEditar = "CREAR OBSERVACION NOVEDAD CONTRACTUAL";

                    NovedadContractualObservaciones novedadContractualObservaciones = new NovedadContractualObservaciones
                    {
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = pUsuarioCreacion,

                        NovedadContractualId = pObservacion.NovedadContractualId,
                        Observaciones = pObservacion.Observaciones,
                        EsSupervision = pObservacion.EsSupervision,
                        EsTramiteNovedades = pObservacion.EsTramiteNovedades,
                    };

                    _context.NovedadContractualObservaciones.Add(novedadContractualObservaciones);
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                return
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = GeneralCodes.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_seguimiento_diario, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }


        private NovedadContractualObservaciones getObservacion(NovedadContractual pNovedadContractual, bool? pEsSupervicion, bool? pEsTramite)
        {
            NovedadContractualObservaciones novedadContractualObservaciones = pNovedadContractual.NovedadContractualObservaciones.ToList()
                        .Where(r => r.EsSupervision == pEsSupervicion &&
                                    r.Archivado != true &&
                                    r.Eliminado != true &&
                                    r.EsTramiteNovedades == pEsTramite
                              )
                        .FirstOrDefault();

            return novedadContractualObservaciones;
        }

        private bool? RegistrocompletoRevisionJuridica(NovedadContractual pNovedadContractual)
        {
            bool? esCompleto = true;

            if (
                    pNovedadContractual.FechaEnvioGestionContractual == null &&
                    string.IsNullOrEmpty(pNovedadContractual.EstadoProcesoCodigo) &&
                    pNovedadContractual.FechaAprobacionGestionContractual == null &&
                    pNovedadContractual.AbogadoRevisionId == null
                )
            {
                esCompleto = null;
            }
            else if (
                   pNovedadContractual.FechaEnvioGestionContractual == null ||
                   string.IsNullOrEmpty(pNovedadContractual.EstadoProcesoCodigo) ||
                   (pNovedadContractual.EstadoProcesoCodigo == "1" && pNovedadContractual.FechaAprobacionGestionContractual == null) ||
                   (pNovedadContractual.EstadoProcesoCodigo == "1" && pNovedadContractual.AbogadoRevisionId == null)
               )
            {
                esCompleto = false;
            }


            return esCompleto;
        }

        private bool? RegistrocompletoDetallar(NovedadContractual pNovedadContractual)
        {
            bool? esCompleto = true;

            foreach (NovedadContractualDescripcion descripcion in pNovedadContractual.NovedadContractualDescripcion)
            {
                // adicion
                if (descripcion.TipoNovedadCodigo == "3")
                {
                    if (
                            pNovedadContractual.NovedadContractualAportante == null ||
                            pNovedadContractual.NovedadContractualAportante.Count() == 0
                       )
                    {
                        esCompleto = null;
                    }
                    else
                    {
                        pNovedadContractual.NovedadContractualAportante.ToList().ForEach(na =>
                        {
                            if (
                                 na.CofinanciacionAportanteId == null ||
                                 na.ValorAporte == null
                             )
                            {
                                esCompleto = false;
                            }

                            na.ComponenteAportanteNovedad.ToList().ForEach(ca =>
                            {
                                if (
                                  ca.FaseCodigo == null ||
                                  ca.TipoComponenteCodigo == null
                              )
                                {
                                    esCompleto = false;
                                }

                                ca.ComponenteFuenteNovedad.ToList().ForEach(cf =>
                                {
                                    if (
                                           cf.FuenteFinanciacionId == null
                                       )
                                    {
                                        esCompleto = false;
                                    }

                                    cf.ComponenteUsoNovedad.ToList().ForEach(cu =>
                                    {
                                        if (
                                               cu.TipoUsoCodigo == null ||
                                               cu.ValorUso == null
                                           )
                                        {
                                            esCompleto = false;
                                        }
                                    });
                                });

                            });

                        });
                    }
                }
            }

            return esCompleto;
        }

        private bool? RegistrocompletoFirmas(NovedadContractual pNovedadContractual)
        {
            bool? esCompleto = true;

            if (
                    pNovedadContractual.DeseaContinuar == null &&
                    pNovedadContractual.FechaEnvioActaContratistaObra == null &&
                    pNovedadContractual.FechaFirmaActaContratistaObra == null &&
                    pNovedadContractual.FechaEnvioActaContratistaInterventoria == null &&
                    pNovedadContractual.FechaFirmaContratistaInterventoria == null &&
                    pNovedadContractual.FechaEnvioActaApoyo == null &&
                    pNovedadContractual.FechaFirmaApoyo == null &&
                    pNovedadContractual.FechaEnvioActaSupervisor == null &&
                    pNovedadContractual.FechaFirmaSupervisor == null &&
                    string.IsNullOrEmpty(pNovedadContractual.UrlSoporteFirmas)
                )
            {
                esCompleto = null;
            }
            else
                if (
                   pNovedadContractual.DeseaContinuar == null ||
                   (pNovedadContractual.DeseaContinuar == true && pNovedadContractual.FechaEnvioActaContratistaObra == null) ||
                   (pNovedadContractual.DeseaContinuar == true && pNovedadContractual.FechaFirmaActaContratistaObra == null) ||
                   (pNovedadContractual.DeseaContinuar == true && pNovedadContractual.FechaEnvioActaContratistaInterventoria == null) ||
                   (pNovedadContractual.DeseaContinuar == true && pNovedadContractual.FechaFirmaContratistaInterventoria == null) ||
                   (pNovedadContractual.DeseaContinuar == true && pNovedadContractual.FechaEnvioActaApoyo == null) ||
                   (pNovedadContractual.DeseaContinuar == true && pNovedadContractual.FechaFirmaApoyo == null) ||
                   (pNovedadContractual.DeseaContinuar == true && pNovedadContractual.FechaEnvioActaSupervisor == null) ||
                   (pNovedadContractual.DeseaContinuar == true && pNovedadContractual.FechaFirmaSupervisor == null) ||
                   (pNovedadContractual.DeseaContinuar == true && string.IsNullOrEmpty(pNovedadContractual.UrlSoporteFirmas)) ||

                   (pNovedadContractual.DeseaContinuar == false && string.IsNullOrEmpty(pNovedadContractual.RazonesNoContinuaProceso)) 

               )
            {
                esCompleto = false;
            }

            return esCompleto;
        }

        private bool? RegistroCompletoInformacionBasica(NovedadContractual pNovedadContractual)
        {
            bool? esCompleto = true;

            if (
                   pNovedadContractual.FechaSolictud == null &&
                    string.IsNullOrEmpty(pNovedadContractual.InstanciaCodigo) &&
                    pNovedadContractual.FechaSesionInstancia == null &&
                    //pNovedadContractual.NovedadContractualDescripcion == null
                    pNovedadContractual.NovedadContractualDescripcion.Count() == 0
                )
            {
                esCompleto = null;
            }
            else
            {
                if (
                    pNovedadContractual.FechaSolictud == null ||
                    string.IsNullOrEmpty(pNovedadContractual.InstanciaCodigo) ||
                    (pNovedadContractual.InstanciaCodigo != "3" && pNovedadContractual.FechaSesionInstancia == null) || // opcion no aplica
                    pNovedadContractual.NovedadContractualDescripcion == null ||
                    pNovedadContractual.NovedadContractualDescripcion.Count() == 0

                )
                {
                    esCompleto = false;
                }
            }

            return esCompleto;
        }

        private bool RegistroCompletoSoporte(NovedadContractual pNovedadContractual)
        {
            bool esCompleto = true;

            if (
                    string.IsNullOrEmpty(pNovedadContractual.UrlSoporte)
                )
            {
                esCompleto = false;
            }

            return esCompleto;
        }

        private bool? RegistroCompletoDescripcion(NovedadContractual pNovedadContractual)
        {
            bool? esCompleto = true;

            if (pNovedadContractual.NovedadContractualDescripcion == null || pNovedadContractual.NovedadContractualDescripcion.Count() == 0)
            {
                esCompleto = null;
            }

            foreach (NovedadContractualDescripcion descripcion in pNovedadContractual.NovedadContractualDescripcion.Where( x => x.Eliminado != true ))
            {
                // Suspension - Prórroga a la Suspensión -Reinicio

                descripcion.RegistroCompleto = true;

                if (
                        descripcion.NovedadContractualDescripcionMotivo.Count() == 0 &&
                        string.IsNullOrEmpty(descripcion.ResumenJustificacion) &&
                        descripcion.EsDocumentacionSoporte == null &&
                        string.IsNullOrEmpty(descripcion.ConceptoTecnico) &&
                        descripcion.FechaConcepto == null &&
                        string.IsNullOrEmpty(descripcion.NumeroRadicado)

                    )
                {
                    descripcion.RegistroCompleto = null;
                    esCompleto = null;
                }
                else
                {
                    if (
                        descripcion.NovedadContractualDescripcionMotivo == null ||
                        descripcion.NovedadContractualDescripcionMotivo.Count() == 0 ||
                        string.IsNullOrEmpty(descripcion.ResumenJustificacion) ||
                        descripcion.EsDocumentacionSoporte == null ||
                        //string.IsNullOrEmpty(descripcion.ConceptoTecnico) ||
                        //descripcion.FechaConcepto == null ||
                        string.IsNullOrEmpty(descripcion.NumeroRadicado)

                    )
                    {
                        descripcion.RegistroCompleto = false;
                        esCompleto = false;
                    }

                    switch (descripcion.TipoNovedadCodigo)
                    {
                        case ConstanTiposNovedades.Suspensión:
                        case ConstanTiposNovedades.Prórroga_a_las_Suspensión:
                        case ConstanTiposNovedades.Reinicio:
                            if (
                                descripcion.FechaInicioSuspension == null ||
                                descripcion.FechaFinSuspension == null
                            )
                            {
                                descripcion.RegistroCompleto = false;
                                esCompleto = false;
                            }
                            break;

                        case ConstanTiposNovedades.Adición:
                            if (
                                   descripcion.PresupuestoAdicionalSolicitado == null
                               )
                            {
                                descripcion.RegistroCompleto = false;
                                esCompleto = false;
                            }
                            break;
                        case ConstanTiposNovedades.Prórroga:
                            if (
                                descripcion.PlazoAdicionalDias == null ||
                                descripcion.PlazoAdicionalMeses == null
                            )
                            {
                                descripcion.RegistroCompleto = false;
                                esCompleto = false;
                            }
                            break;
                        case ConstanTiposNovedades.Modificación_de_Condiciones_Contractuales:
                            descripcion.NovedadContractualClausula.ToList().ForEach(c =>
                            {
                                if (
                                     string.IsNullOrEmpty(c.ClausulaAmodificar) ||
                                     string.IsNullOrEmpty(c.AjusteSolicitadoAclausula)
                                )
                                {
                                    descripcion.RegistroCompleto = false;
                                    esCompleto = false;
                                }
                            });
                            break;
                    }
                }
            }

            return esCompleto;
        }

        private bool Registrocompleto(NovedadContractual pNovedadContractual)
        {
            bool esCompleto = true;

            if (
                    RegistroCompletoInformacionBasica(pNovedadContractual) == false ||
                    RegistroCompletoInformacionBasica(pNovedadContractual) == null ||
                    RegistroCompletoSoporte(pNovedadContractual) == false ||
                    RegistroCompletoDescripcion(pNovedadContractual) == false ||
                    RegistroCompletoDescripcion(pNovedadContractual) == null
                )
            {
                esCompleto = false;
            }

            return esCompleto;
        }

        private bool? RegistrocompletoTramite(NovedadContractual pNovedadContractual)
        {
            bool? esCompleto = true;

            if (
                RegistrocompletoRevisionJuridica(pNovedadContractual) == null ||
                RegistrocompletoRevisionJuridica(pNovedadContractual) == false ||
                RegistrocompletoFirmas(pNovedadContractual) == null ||
                RegistrocompletoFirmas(pNovedadContractual) == false ||
                RegistrocompletoDetallar(pNovedadContractual) == null ||
                RegistrocompletoDetallar(pNovedadContractual) == false 
                )
            {
                esCompleto = false;
            }

            return esCompleto;
        }

        private async Task<bool> ValidarRegistroCompletoVerificacion(int id, bool? pEsSupervicion, bool? pEsTramite)
        {
            bool esCompleto = true;

            NovedadContractual nc = await _context.NovedadContractual.Where(cc => cc.NovedadContractualId == id)
                                                                .FirstOrDefaultAsync();


            nc.ObservacionApoyo = getObservacion(nc, pEsSupervicion, pEsTramite);

            if (nc.TieneObservacionesApoyo == null ||
                 (nc.TieneObservacionesApoyo == true && string.IsNullOrEmpty(nc.ObservacionApoyo != null ? nc.ObservacionApoyo.Observaciones : null))
               )
            {
                esCompleto = false;
            }

            return esCompleto;
        }

        private async Task<bool> ValidarRegistroCompletoValidacion(int id, bool? pEsSupervicion, bool? pEsTramite)
        {
            bool esCompleto = true;

            NovedadContractual nc = await _context.NovedadContractual.Where(cc => cc.NovedadContractualId == id)
                                                                .FirstOrDefaultAsync();


            nc.ObservacionSupervisor = getObservacion(nc, pEsSupervicion, pEsTramite);

            if (nc.TieneObservacionesSupervisor == null ||
                 (nc.TieneObservacionesSupervisor == true && string.IsNullOrEmpty(nc.ObservacionSupervisor != null ? nc.ObservacionSupervisor.Observaciones : null))
               )
            {
                esCompleto = false;
            }

            return esCompleto;
        }

        private async void EnviarNotificacionAApoyo(NovedadContractual novedadContractual)
        {
            VProyectosXcontrato proyecto = _context.VProyectosXcontrato
                                                        .Where(x => x.ContratoId == novedadContractual.ContratoId)
                                                        .AsNoTracking()
                                                        .FirstOrDefault();

            Contrato contrato = _context.Contrato
                                            .Where(x => x.ContratoId == novedadContractual.ContratoId)
                                            .Include(x => x.Apoyo)
                                            .FirstOrDefault();

            Template templateEnviar = _context.Template
                                                .Where(r => r.TemplateId == (int)enumeratorTemplate.EnviarNovedadAApoyo && r.Activo == true)
                                                .FirstOrDefault();

            string template = templateEnviar.Contenido
                                                      .Replace("[NUMERO_CONTRATO]", proyecto.NumeroContrato)
                                                      .Replace("[NUMERO_SOLICITUD]", novedadContractual.NumeroSolicitud)

            //.Replace("[INSTITUCION_EDUCATIVA]", proyecto.InstitucionEducativa)
            //.Replace("[SEDE]", proyecto.Sede)
            //.Replace("[TIPO_INTERVENCION]", proyecto.TipoIntervencion)

            ;
            //List<Usuario> ListUsuarios = await _commonService.GetUsuariosByPerfil((int)EnumeratorPerfil.Miembros_Comite);
            List<string> listaMails = new List<string> { contrato?.Apoyo?.Email };
            _commonService.EnviarCorreo(listaMails, template, "Novedad Contractual enviada");


        }

        private async void EnviarNotificacionASupervisor(NovedadContractual novedadContractual)
        {
            VProyectosXcontrato proyecto = _context.VProyectosXcontrato
                                                        .Where(x => x.ContratoId == novedadContractual.ContratoId)
                                                        .AsNoTracking()
                                                        .FirstOrDefault();

            Contrato contrato = _context.Contrato
                                            .Where(x => x.ContratoId == novedadContractual.ContratoId)
                                            .Include(x => x.Supervisor)
                                            .FirstOrDefault();

            Template templateEnviar = _context.Template
                                                .Where(r => r.TemplateId == (int)enumeratorTemplate.EnviarNovedadAApoyo && r.Activo == true)
                                                .FirstOrDefault();

            string template = templateEnviar.Contenido
                                                      .Replace("[NUMERO_CONTRATO]", proyecto.NumeroContrato)
                                                      .Replace("[NUMERO_SOLICITUD]", novedadContractual.NumeroSolicitud)

            //.Replace("[INSTITUCION_EDUCATIVA]", proyecto.InstitucionEducativa)
            //.Replace("[SEDE]", proyecto.Sede)
            //.Replace("[TIPO_INTERVENCION]", proyecto.TipoIntervencion)

            ;
            //List<Usuario> ListUsuarios = await _commonService.GetUsuariosByPerfil((int)EnumeratorPerfil.Miembros_Comite);
            List<string> listaMails = new List<string> { contrato?.Supervisor?.Email };
            _commonService.EnviarCorreo(listaMails, template, "Novedad Contractual enviada");


        }

        private async void EnviarNotificacionRechazo(NovedadContractual novedadContractual)
        {
            VProyectosXcontrato proyecto = _context.VProyectosXcontrato
                                                        .Where(x => x.ContratoId == novedadContractual.ContratoId)
                                                        .AsNoTracking()
                                                        .FirstOrDefault();

            Contrato contrato = _context.Contrato
                                            .Where(x => x.ContratoId == novedadContractual.ContratoId)
                                            .Include(x => x.Contratacion)
                                                .ThenInclude(x => x.Contratista)
                                                    .ThenInclude(x => x.ProcesoSeleccionProponente)
                                            .FirstOrDefault();

            Template templateEnviar = _context.Template
                                                .Where(r => r.TemplateId == (int)enumeratorTemplate.EnviarNovedadRechazo && r.Activo == true)
                                                .FirstOrDefault();

            string template = templateEnviar.Contenido
                                                      .Replace("[NUMERO_CONTRATO]", proyecto.NumeroContrato)
                                                      .Replace("[NUMERO_SOLICITUD]", novedadContractual.NumeroSolicitud)

            //.Replace("[INSTITUCION_EDUCATIVA]", proyecto.InstitucionEducativa)
            //.Replace("[SEDE]", proyecto.Sede)
            //.Replace("[TIPO_INTERVENCION]", proyecto.TipoIntervencion)

            ;
            //List<Usuario> ListUsuarios = await _commonService.GetUsuariosByPerfil((int)EnumeratorPerfil.Miembros_Comite);
            List<string> listaMails = new List<string> { contrato?.Contratacion?.Contratista?.ProcesoSeleccionProponente?.EmailProponente };
            _commonService.EnviarCorreo(listaMails, template, "Novedad Contractual Rechazada");


        }

        private async void EnviarNotificacionRechazoSupervisor(NovedadContractual novedadContractual)
        {
            VProyectosXcontrato proyecto = _context.VProyectosXcontrato
                                                        .Where(x => x.ContratoId == novedadContractual.ContratoId)
                                                        .AsNoTracking()
                                                        .FirstOrDefault();

            Contrato contrato = _context.Contrato
                                            .Where(x => x.ContratoId == novedadContractual.ContratoId)
                                            .Include(x => x.Apoyo)
                                            .Include(x => x.Interventor)
                                            .FirstOrDefault();

            Template templateEnviar = _context.Template
                                                .Where(r => r.TemplateId == (int)enumeratorTemplate.EnviarNovedadRechazoSupervisor && r.Activo == true)
                                                .FirstOrDefault();

            string template = templateEnviar.Contenido
                                                      .Replace("[NUMERO_CONTRATO]", proyecto.NumeroContrato)
                                                      .Replace("[NUMERO_SOLICITUD]", novedadContractual.NumeroSolicitud)
                                                      ;


            List<string> listaMails = new List<string> { contrato?.Apoyo?.Email, contrato?.Interventor?.Email };
            _commonService.EnviarCorreo(listaMails, template, "Novedad Contractual Rechazada por supervisor");


        }

        private async void EnviarNotificacionDevolucion(NovedadContractual novedadContractual)
        {
            VProyectosXcontrato proyecto = _context.VProyectosXcontrato
                                                        .Where(x => x.ContratoId == novedadContractual.ContratoId)
                                                        .AsNoTracking()
                                                        .FirstOrDefault();

            Contrato contrato = _context.Contrato
                                            .Where(x => x.ContratoId == novedadContractual.ContratoId)
                                            .Include(x => x.Apoyo)
                                            .Include(x => x.Interventor)
                                            .FirstOrDefault();

            Template templateEnviar = _context.Template
                                                .Where(r => r.TemplateId == (int)enumeratorTemplate.EnviarNovedadDevuelta && r.Activo == true)
                                                .FirstOrDefault();

            string template = templateEnviar.Contenido
                                                      .Replace("[NUMERO_CONTRATO]", proyecto.NumeroContrato)
                                                      .Replace("[NUMERO_SOLICITUD]", novedadContractual.NumeroSolicitud)
                                                      ;

            List<string> listaMails = new List<string>();

            if (proyecto.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
            {
                listaMails.Add(contrato?.Interventor?.Email);
            }

            if (proyecto.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
            {
                listaMails.Add(contrato?.Apoyo?.Email);
            }

            _commonService.EnviarCorreo(listaMails, template, "Novedad Contractual devuelta por supervisor");


        }

        private async void EnviarNotificacionATramitador(NovedadContractual novedadContractual)
        {
            VProyectosXcontrato proyecto = _context.VProyectosXcontrato
                                                        .Where(x => x.ContratoId == novedadContractual.ContratoId)
                                                        .AsNoTracking()
                                                        .FirstOrDefault();

            Contrato contrato = _context.Contrato
                                            .Where(x => x.ContratoId == novedadContractual.ContratoId)
                                            .Include(x => x.Apoyo)
                                            .FirstOrDefault();

            Template templateEnviar = _context.Template
                                                .Where(r => r.TemplateId == (int)enumeratorTemplate.EnviarNovedadTramitar && r.Activo == true)
                                                .FirstOrDefault();

            string template = templateEnviar.Contenido
                                                      .Replace("[NUMERO_CONTRATO]", proyecto.NumeroContrato)
                                                      .Replace("[NUMERO_SOLICITUD]", novedadContractual.NumeroSolicitud)

            //.Replace("[INSTITUCION_EDUCATIVA]", proyecto.InstitucionEducativa)
            //.Replace("[SEDE]", proyecto.Sede)
            //.Replace("[TIPO_INTERVENCION]", proyecto.TipoIntervencion)

            ;

            List<UsuarioPerfil> ListUsuarios = _context.UsuarioPerfil
                                                            .Where(x => x.PerfilId == (int)EnumeratorPerfil.Seguimiento_y_control)
                                                            .ToList();

            List<string> listaMails = new List<string>();

            ListUsuarios.ForEach(u =>
           {
               Usuario usuario = _context.Usuario.Find(u.UsuarioId);

               if (usuario != null)
               {
                   listaMails.Add(usuario.Email);
               }

           });

            if (listaMails.Count() > 0)
                _commonService.EnviarCorreo(listaMails, template, "Novedad Contractual enviada");


        }

        private async void EnviarNotificacionCancelar(NovedadContractual novedadContractual)
        {
            VProyectosXcontrato proyecto = _context.VProyectosXcontrato
                                                        .Where(x => x.ContratoId == novedadContractual.ContratoId)
                                                        .AsNoTracking()
                                                        .FirstOrDefault();

            Contrato contrato = _context.Contrato
                                            .Where(x => x.ContratoId == novedadContractual.ContratoId)
                                            .Include( x => x.Supervisor )
                                            .Include(x => x.Apoyo)
                                            .Include(x => x.Contratacion)
                                                .ThenInclude(x => x.Contratista)
                                                    .ThenInclude(x => x.ProcesoSeleccionProponente)
                                            .FirstOrDefault();

            Template templateEnviar = _context.Template
                                                .Where(r => r.TemplateId == (int)enumeratorTemplate.EnviarNovedadCancelar && r.Activo == true)
                                                .FirstOrDefault();

            string template = templateEnviar.Contenido
                                                      .Replace("[NUMERO_CONTRATO]", proyecto.NumeroContrato)
                                                      .Replace("[NUMERO_SOLICITUD]", novedadContractual.NumeroSolicitud)
                                                      .Replace("[JUSTIFICACION]", novedadContractual.NumeroSolicitud)

            ;

            List<string> listaMails = new List<string>();


            if ( !string.IsNullOrEmpty(contrato?.Supervisor?.Email) )
            {
                listaMails.Add(contrato?.Supervisor?.Email);
            }

            if ( !string.IsNullOrEmpty( contrato?.Apoyo?.Email ) )
            {
                listaMails.Add(contrato?.Apoyo?.Email);
            }

            if (!string.IsNullOrEmpty(contrato?.Contratacion?.Contratista?.ProcesoSeleccionProponente?.EmailProponente))
            {
                listaMails.Add(contrato?.Contratacion?.Contratista?.ProcesoSeleccionProponente?.EmailProponente);
            }

            if (listaMails.Count() > 0)
                _commonService.EnviarCorreo(listaMails, template, "Novedad Contractual Canceladad");


        }

        #endregion private 

    }


}
