﻿using System;
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

namespace asivamosffie.services
{
    public class ProjectContractingService : IProjectContractingService
    {

        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        private readonly devAsiVamosFFIEContext _context;

        public async Task<Respuesta> ChangeStateContratacionByIdContratacion(int idContratacion,string PCodigoEstado, string pUsusarioModifico)
        {
            int idAccionEliminarContratacionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Contratacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contratacion contratacionOld = _context.Contratacion.Find(idContratacion); 
                contratacionOld.EstadoSolicitudCodigo = PCodigoEstado;
                contratacionOld.FechaSolicitud = DateTime.Now;
                contratacionOld.UsuarioModificacion = pUsusarioModifico;
                contratacionOld.FechaModificacion = DateTime.Now;
                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContratacionProyecto.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesContratacionProyecto.OperacionExitosa, idAccionEliminarContratacionProyecto, pUsusarioModifico, "CAMBIAR ESTADO CONTRATACIÓN")
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


        public async Task<Respuesta> DeleteContratacionByIdContratacion(int idContratacion , string pUsusarioElimino) 
        {
            int idAccionEliminarContratacionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Contratacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contratacion contratacionOld = _context.Contratacion.Find(idContratacion);
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesContratacionProyecto.Error, idAccionEliminarContratacionProyecto, pUsusarioElimino, ex.InnerException.ToString().Substring(0,500))
                }; 
            }

        }


        public async Task<Contratacion> GetAllContratacionByContratacionId(int pContratacionId)
        {
            return  await _context.Contratacion.Where(r => r.ContratacionId == pContratacionId)
              .Include(r => r.Contratista)
              .Include(r => r.ContratacionProyecto)
                   .ThenInclude(r => r.Proyecto)
              .Include(r => r.ContratacionProyecto).FirstOrDefaultAsync();
        }

        public async Task<Contratacion> GetContratacionByContratacionId(int pContratacionId)
        {

            Contratacion contratacion = await _context.Contratacion.Where(r => r.ContratacionId == pContratacionId)
                .Include(r => r.Contratista)
                .Include(r => r.ContratacionProyecto)
                     .ThenInclude(r => r.Proyecto)
                .Include(r => r.ContratacionProyecto)
                    .ThenInclude(r => r.ContratacionProyectoAportante)
                      .ThenInclude(r => r.ComponenteAportante)
                           .ThenInclude(r => r.ComponenteUso).FirstOrDefaultAsync();

            contratacion.ContratacionProyecto = contratacion.ContratacionProyecto.Where(r => !(bool)r.Eliminado).ToList();

            List<Dominio> ListTipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && (bool)r.Activo).ToList();
            List<Localizacion> ListDepartamentos = _context.Localizacion.Where(r => r.Nivel == 1).ToList();
            List<Localizacion> ListRegiones = _context.Localizacion.Where(r => r.Nivel == 3).ToList();


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

            }

            return contratacion;
        }

        public ProjectContractingService(devAsiVamosFFIEContext context, ICommonService commonService, IDocumentService documentService)
        {
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
        }

        public async Task<List<ContratacionProyecto>> GetListContratacionProyectoByContratacionId(int idContratacion)
        {

            //devuelto = array ContratacionProyecto  + proyecto + contratista + ProyectoAportante + CofinanciacionAportante

            List<ContratacionProyecto> ListContratacionProyecto = new List<ContratacionProyecto>();

            ListContratacionProyecto = await _context.ContratacionProyecto.
                Where(r => !(bool)r.Eliminado && r.ContratacionId == idContratacion).
                IncludeFilter(r => r.Proyecto).Where(r => !(bool)r.Eliminado).
                IncludeFilter(r => r.Contratacion.Contratista).Where(r => !(bool)r.Eliminado).
                IncludeFilter(r => r.ContratacionProyectoAportante.Where(r => !(bool)r.Eliminado)).ToListAsync();

            foreach (var item in ListContratacionProyecto)
            {
                foreach (var ContratacionProyectoAportante in item.ContratacionProyectoAportante)
                {
                    ContratacionProyectoAportante.ProyectoAportante = await _context.ProyectoAportante.Where(r => !(bool)r.Eliminado && r.ProyectoAportanteId == ContratacionProyectoAportante.ProyectoAportanteId ).FirstOrDefaultAsync();
                }
            }
            return ListContratacionProyecto;


        }

        public async Task<ContratacionProyecto> GetContratacionProyectoById(int idContratacionProyecto)
        {
            ContratacionProyecto contratacionProyecto = new ContratacionProyecto();

            contratacionProyecto = await _context.ContratacionProyecto
                .Where(r => !(bool)r.Eliminado && r.ContratacionProyectoId == idContratacionProyecto)
                .Include(r => r.Proyecto).Where(r => !(bool)r.Eliminado)
                .Include(r => r.Contratacion).Where(r => !(bool)r.Eliminado)
                .Include(r => r.ContratacionProyectoAportante).Where(r => !(bool)r.Eliminado)
                .Include(r => r.Proyecto)
                    .ThenInclude(r => r.ProyectoAportante).Where(r => !(bool)r.Eliminado)
                .Include(r => r.Proyecto)
                     .ThenInclude(r => r.ProyectoAportante)
                         .ThenInclude( r => r.Aportante )
                .Include(r => r.ContratacionProyectoAportante)
                    .ThenInclude(r => r.ComponenteAportante).Where(r => !(bool)r.Eliminado)
                .Include(r => r.ContratacionProyectoAportante)
                    .ThenInclude(r => r.ComponenteAportante)
                        .ThenInclude(r => r.ComponenteUso).Where(r => !(bool)r.Eliminado)
                .Include(r => r.Proyecto)
                    .ThenInclude( r => r.InstitucionEducativa )        
                 .Include(r => r.Proyecto)
                     .ThenInclude( r => r.Sede )        
                 .Include(r => r.Proyecto)
                     .ThenInclude( r => r.LocalizacionIdMunicipioNavigation )        
                .FirstOrDefaultAsync();
                

            // foreach (var item in ListContratacionProyecto)
            // {
            //     foreach (var ContratacionProyectoAportante in item.ContratacionProyectoAportante)
            //     {
            //         ContratacionProyectoAportante.Aportante = await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado && r.CofinanciacionAportanteId == ContratacionProyectoAportante.AportanteId).FirstOrDefaultAsync();
            //     }
            // }

            return contratacionProyecto;
        }

        public async Task<List<Contratacion>> GetListContratacion()
        {

            List<Contratacion> ListContratacion = await _context.Contratacion.Where( r => !(bool)r.Eliminado ).ToListAsync();

            foreach (var Contratacion in ListContratacion)
            {
                if (!string.IsNullOrEmpty(Contratacion.TipoSolicitudCodigo))
                {
                    Contratacion.TipoSolicitudCodigo = await _commonService.GetNombreDominioByCodigoAndTipoDominio(Contratacion.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_de_Solicitud);
                }
                if (!string.IsNullOrEmpty(Contratacion.EstadoSolicitudCodigo))
                {
                    Contratacion.EstadoSolicitudCodigo = await _commonService.GetNombreDominioByCodigoAndTipoDominio(Contratacion.EstadoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Solicitud);
                }
            }
            return ListContratacion;
        }

        public async Task<List<ContratistaGrilla>> GetListContractingByFilters(string pTipoIdentificacionCodigo, string pNumeroIdentidicacion, string pNombre, bool? EsConsorcio)
        {
            List<ContratistaGrilla> ListContratistaGrillas = new List<ContratistaGrilla>();

            IQueryable<Contratista> contratistas = _context.Contratista.Where(
                r => (bool)r.Activo);

            if (!string.IsNullOrEmpty(pTipoIdentificacionCodigo))
            {
                contratistas = contratistas.Where(r => r.TipoIdentificacionCodigo.Equals(pTipoIdentificacionCodigo));
            }
            if (!string.IsNullOrEmpty(pNumeroIdentidicacion))
            {
                contratistas = contratistas.Where(r => r.NumeroIdentificacion.Contains(pNumeroIdentidicacion));
            }
            if (!string.IsNullOrEmpty(pNombre))
            {
                contratistas = contratistas.Where(r => r.Nombre.ToUpper().Contains(pNombre.ToUpper()));
            }
            //TODO: Validar si se compara asi los bool
            if (EsConsorcio != null)
            {
                contratistas = contratistas.Where(r => r.EsConsorcio == EsConsorcio);
            }

            foreach (var contratista in contratistas)
            {
                ContratistaGrilla contratistaGrilla = new ContratistaGrilla
                {
                    IdContratista = contratista.ContratistaId,
                    Nombre = contratista.Nombre,
                    NumeroIdentificacion = contratista.NumeroIdentificacion,
                    EsConsorcio = (bool)contratista.EsConsorcio,
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
            string pMunicipio,
            int pIdInstitucionEducativa,
            int pIdSede)
        {
            //Listar Los proyecto segun caso de uso solo trae los ue estado
            //estado de registro “Completo”, que tienen viabilidad jurídica y técnica

            string strCodigoEstadoJuridicoAprobado = ConstantCodigoEstadoJuridico.Aprobado;

            List<Proyecto> ListProyectos =
                  _context.Proyecto.Where(
                      r => !(bool)r.Eliminado &&
                      r.EstadoJuridicoCodigo
                      .Equals(strCodigoEstadoJuridicoAprobado) &&
                      (bool)r.RegistroCompleto &&
                      r.TipoIntervencionCodigo == (string.IsNullOrEmpty(pTipoIntervencion) ? r.TipoIntervencionCodigo : pTipoIntervencion) &&
                      r.LlaveMen == (string.IsNullOrEmpty(pLlaveMen) ? r.LlaveMen : pLlaveMen) &&
                      r.LocalizacionIdMunicipio == (string.IsNullOrEmpty(pMunicipio) ? r.LocalizacionIdMunicipio : pMunicipio) &&
                      r.InstitucionEducativaId == (pIdInstitucionEducativa > 0 ? pIdInstitucionEducativa : r.InstitucionEducativaId) &&
                      r.SedeId == (pIdSede > 0 ? pIdSede : r.SedeId)
                      )
                              .Distinct()
                              .Include(r => r.ContratacionProyecto)
                              .Include(r => r.Sede)
                              .Include(r => r.InstitucionEducativa)
                              .Include(r => r.LocalizacionIdMunicipioNavigation).ToList();


            List<ProyectoGrilla> ListProyectoGrilla = new List<ProyectoGrilla>();
            List<Dominio> ListTipoSolicitud = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Tipo_de_Solicitud_Obra_Interventorias);

            //Lista para Dominio intervencio
            List<Dominio> ListTipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && (bool)r.Activo).ToList();

            List<Localizacion> ListDepartamentos = _context.Localizacion.Where(r => r.Nivel == 1).ToList();

            List<Localizacion> ListRegiones = _context.Localizacion.Where(r => r.Nivel == 3).ToList();
            //departamneto 
            //    Region  
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
                            //  Departamento = _commonService.GetNombreDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio),
                            // Municipio = _commonService.GetNombreLocalizacionByLocalizacionId(proyecto.LocalizacionIdMunicipio),
                            Municipio = proyecto.LocalizacionIdMunicipioNavigation.Descripcion,
                            //InstitucionEducativa = _context.InstitucionEducativaSede.Find(proyecto.InstitucionEducativaId).Nombre,
                            //Sede = _context.InstitucionEducativaSede.Find(proyecto.SedeId).Nombre,
                            InstitucionEducativa = proyecto.InstitucionEducativa.Nombre,
                            Sede = proyecto.Sede.Nombre,
                            ProyectoId = proyecto.ProyectoId, 
                        };
   
      //r.TipoIntervencionCodigo == (string.IsNullOrEmpty(pTipoIntervencion) ? r.TipoIntervencionCodigo : pTipoIntervencion) &&

                        foreach (var item in proyecto.ContratacionProyecto)
                        {
                            item.Contratacion = ListContratacion.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefault();
                            if (!string.IsNullOrEmpty(item.Contratacion.TipoSolicitudCodigo))
                            {
                                if (item.Contratacion.TipoSolicitudCodigo == "1")
                                {
                                    proyectoGrilla.TieneObra = true; 
                                }
                                if(item.Contratacion.TipoSolicitudCodigo == "2") 
                                {
                                    proyectoGrilla.TieneInterventoria = true;
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

            return ListProyectoGrilla.OrderByDescending(r => r.ProyectoId).ToList();
        }



        //CrearContratacion
        public async Task<Respuesta> CreateEditContratacion(Contratacion Pcontratacion)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearContratacionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            try
            {

                //Contratacion
                if (Pcontratacion.ContratacionId == 0)
                {
                    Pcontratacion.Eliminado = false;
                    Pcontratacion.FechaCreacion = DateTime.Now;
                    Pcontratacion.FechaSolicitud = DateTime.Now;
                    //Metodo que valida si todos los registros estan completos retorna true si completos
                    Pcontratacion.RegistroCompleto = ValidarEstado(Pcontratacion);
                    Pcontratacion.NumeroSolicitud = await _commonService.EnumeradorContratacion();
                    _context.Contratacion.Add(Pcontratacion);
                }
                else
                {
                    Contratacion contratacionVieja = await _context.Contratacion.Where(r => r.ContratacionId == Pcontratacion.ContratacionId).Include(r => r.Contratista).FirstOrDefaultAsync();
                    contratacionVieja.TipoSolicitudCodigo = Pcontratacion.TipoSolicitudCodigo;
                    contratacionVieja.EsObligacionEspecial = Pcontratacion.EsObligacionEspecial;
                    contratacionVieja.ConsideracionDescripcion = Pcontratacion.ConsideracionDescripcion;
                    contratacionVieja.EstadoSolicitudCodigo = Pcontratacion.EstadoSolicitudCodigo;
                    contratacionVieja.ContratistaId = Pcontratacion.ContratistaId;    

                    contratacionVieja.RegistroCompleto = ValidarEstado(contratacionVieja);
                    _context.Update(contratacionVieja);
                }

                //Contratista 
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
                            ComponenteAportante.UsuarioCreacion = Pcontratacion.UsuarioCreacion;
                            await CreateEditComponenteAportante(ComponenteAportante, true);


                            //Componente Uso
                            foreach (var ComponenteUso in ComponenteAportante.ComponenteUso)
                            {
                                ComponenteUso.UsuarioCreacion = Pcontratacion.UsuarioCreacion;
                                await CreateEditComponenteUso(ComponenteUso, true);
                            }
                        }
                    }
                }

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesProyecto.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearContratacionProyecto, Pcontratacion.UsuarioCreacion, "")
                };
            }
            catch (Exception ex)
            {
                return respuesta =
                           new Respuesta
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
                    //Registros

                    contratistaOld.TipoIdentificacionCodigo = pContratista.TipoIdentificacionCodigo;
                    contratistaOld.NumeroIdentificacion = pContratista.NumeroInvitacion;
                    contratistaOld.Nombre = pContratista.Nombre;
                    contratistaOld.RepresentanteLegal = pContratista.RepresentanteLegal;
                    contratistaOld.NumeroInvitacion = pContratista.NumeroInvitacion;
                }

                if (esTransaccion)
                {
                    return Respuesta;
                }
                ///
                //TODO:Armar Respuesta Creacion Componente Aportante
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
                    _context.ComponenteAportante.Add(pComponenteAportante);
                }
                else
                {
                    ComponenteAportante componenteAportanteOld = await _context.ComponenteAportante.FindAsync(pComponenteAportante.ComponenteAportanteId);
                    componenteAportanteOld.UsuarioModificacion = pComponenteAportante.UsuarioCreacion;
                    componenteAportanteOld.FechaModificacion = DateTime.Now;
                    //Esto es lo unico que puede cambiar en esta tabla
                    componenteAportanteOld.TipoComponenteCodigo = pComponenteAportante.TipoComponenteCodigo;
                    componenteAportanteOld.FaseCodigo = pComponenteAportante.FaseCodigo;
                }
                if (esTransaccion)
                {
                    return Respuesta;
                }
                ///
                //TODO:Armar Respuesta Creacion Componente Aportante
                return Respuesta;
            }
            catch (Exception ex)
            {
                Respuesta.Data = ex;
                return Respuesta;
            }
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
                    _context.ComponenteUso.Add(pComponenteUso);
                }
                else
                {
                    ComponenteUso pComponenteUsoOld = await _context.ComponenteUso.FindAsync(pComponenteUso.ComponenteUsoId);
                    pComponenteUsoOld.UsuarioModificacion = pComponenteUso.UsuarioCreacion;
                    pComponenteUsoOld.FechaModificacion = DateTime.Now;
                    //Esto es lo unico que puede cambiar en esta tabla
                    pComponenteUsoOld.TipoUsoCodigo = pComponenteUso.TipoUsoCodigo;
                    pComponenteUsoOld.ValorUso = pComponenteUso.ValorUso;
                }
                if (esTransaccion)
                {
                    return Respuesta;
                }
                ///
                //TODO:Armar Respuesta Creacion Componente Uso
                return Respuesta;
            }
            catch (Exception ex)
            {
                Respuesta.Data = ex;
                return Respuesta;
            }
        }

        public async Task<Respuesta> CreateEditContratacionProyecto(ContratacionProyecto pContratacionProyecto, bool esTransaccion)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearContratacionContrataicionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Contratacion_Proyecto, (int)EnumeratorTipoDominio.Acciones);
            string strAccion = " ";

            try
            {
                if (pContratacionProyecto.ContratacionProyectoId == 0)
                {
                    strAccion = "CREAR CONTRATACION PROYECTO";
                    pContratacionProyecto.FechaCreacion = DateTime.Now;
                    pContratacionProyecto.Eliminado = false;

                    _context.ContratacionProyecto.Add(pContratacionProyecto);

                }

                else
                {
                    strAccion = "EDITAR CONTRATACION PROYECTO";

                    ContratacionProyecto contratacionProyectoAntiguo = _context.ContratacionProyecto.Find(pContratacionProyecto.ContratacionProyectoId);
                    //Auditoria 
                    contratacionProyectoAntiguo.FechaModificacion = DateTime.Now;
                    contratacionProyectoAntiguo.UsuarioModificacion = pContratacionProyecto.UsuarioCreacion;

                    //registros
                    contratacionProyectoAntiguo.ContratacionId = pContratacionProyecto.ContratacionId;
                    contratacionProyectoAntiguo.ProyectoId = pContratacionProyecto.ProyectoId;
                    contratacionProyectoAntiguo.EsReasignacion = pContratacionProyecto.EsReasignacion;
                    contratacionProyectoAntiguo.EsAvanceObra = pContratacionProyecto.EsAvanceObra;

                    contratacionProyectoAntiguo.RequiereLicencia = pContratacionProyecto.RequiereLicencia;
                    contratacionProyectoAntiguo.LicenciaVigente = pContratacionProyecto.LicenciaVigente;
                    contratacionProyectoAntiguo.NumeroLicencia = pContratacionProyecto.NumeroLicencia;
                    contratacionProyectoAntiguo.FechaVigencia = pContratacionProyecto.FechaVigencia;
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
                    Code = ConstantMessagesProyecto.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearContratacionContrataicionProyecto, pContratacionProyecto.UsuarioCreacion, strAccion)
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

        public async Task<Respuesta> CreateEditContratacionProyectoAportanteByContratacionproyecto(ContratacionProyecto pContratacionProyecto, bool esTransaccion)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearContratacionContrataicionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Contratacion_Proyecto, (int)EnumeratorTipoDominio.Acciones);
            string strAccion = " ";

            try
            {
                foreach (var ContratacionProyectoAportante in pContratacionProyecto.ContratacionProyectoAportante)
                    {
                        ContratacionProyectoAportante.UsuarioCreacion = pContratacionProyecto.UsuarioCreacion;
                        await CreateEditContratacionProyectoAportante(ContratacionProyectoAportante, true);

                        //ComponenteAportante
                        foreach (var ComponenteAportante in ContratacionProyectoAportante.ComponenteAportante)
                        {
                            ComponenteAportante.UsuarioCreacion = pContratacionProyecto.UsuarioCreacion;
                            await CreateEditComponenteAportante(ComponenteAportante, true);


                            //Componente Uso
                            foreach (var ComponenteUso in ComponenteAportante.ComponenteUso)
                            {
                                ComponenteUso.UsuarioCreacion = pContratacionProyecto.UsuarioCreacion;
                                await CreateEditComponenteUso(ComponenteUso, true);
                            }
                        }
                    }
                _context.SaveChanges();
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesProyecto.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearContratacionContrataicionProyecto, pContratacionProyecto.UsuarioCreacion, strAccion)
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
            string strAccion = " ";

            try
            {

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
                    contratacionProyectoAportanteAntiguo.ProyectoAportanteId = pContratacionProyectoAportante.ProyectoAportanteId;
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
            //TODO: VALIDAR HIJOS
            if (!string.IsNullOrEmpty(contratacion.TipoSolicitudCodigo)
             || !string.IsNullOrEmpty(contratacion.NumeroSolicitud)
             || !string.IsNullOrEmpty(contratacion.EstadoSolicitudCodigo)
             || !string.IsNullOrEmpty(contratacion.ContratacionId.ToString())
             || (contratacion.EsObligacionEspecial != null)
             || !string.IsNullOrEmpty(contratacion.ConsideracionDescripcion))
            {
                return true;
            }
            return false;
        }
       
        public async Task<Respuesta> CreateContratacionProyecto(Contratacion pContratacion, string usuarioCreacion)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearContratacionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contratacion contratacion = new Contratacion
                {
                    //Auditoria
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacion = usuarioCreacion,
                    Eliminado = false,

                    //Registros

                    //TODO: Poner contratistaID y los demas campos
                    TipoSolicitudCodigo = pContratacion.TipoSolicitudCodigo,
                    FechaSolicitud = DateTime.Now,
                    NumeroSolicitud = await _commonService.EnumeradorContratacion()
                    //Contratista = ContratistaId 
                    //EsObligacionEspecial = (bool),
                    //ConsideracionDescripcion = "" 
                };
                contratacion.RegistroCompleto = ValidarEstado(contratacion);
                //Se guarda para tener idContratacion y relacionarlo con la tabla contratacionProyecto


                foreach (ContratacionProyecto c in pContratacion.ContratacionProyecto)
                {
                    //Crear contratacionProyecto
                    ContratacionProyecto contratacionProyecto = new ContratacionProyecto
                    {
                        //Auditoria
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = usuarioCreacion,
                        Eliminado = false,
                        //Registros
                        ContratacionId = contratacion.ContratacionId,
                        ProyectoId = c.ProyectoId,

                    };

                    List<ProyectoAportante> listaAportantes = _context.ProyectoAportante.Where( a => !(bool)a.Eliminado && a.ProyectoId == c.ProyectoId ).ToList();

                    listaAportantes.ForEach( apo => {
                        ContratacionProyectoAportante contratacionProyectoAportante = new ContratacionProyectoAportante();

                        contratacionProyectoAportante.ProyectoAportanteId = apo.ProyectoAportanteId;
                        contratacionProyectoAportante.FechaCreacion = DateTime.Now;
                        contratacionProyectoAportante.UsuarioCreacion = usuarioCreacion;

                        contratacionProyecto.ContratacionProyectoAportante.Add( contratacionProyectoAportante );

                    });

                    contratacion.ContratacionProyecto.Add(contratacionProyecto);
                    //_context.ContratacionProyecto.Add(contratacionProyecto);

                }

                _context.Contratacion.Add(contratacion);
                _context.SaveChanges();

                return respuesta =
                 new Respuesta
                 {
                     IsSuccessful = true,
                     IsException = false,
                     IsValidation = true,
                     Code = ConstantMessagesContratacionProyecto.OperacionExitosa,
                     Data = contratacion,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesContratacionProyecto.OperacionExitosa, idAccionCrearContratacionProyecto, usuarioCreacion, "")
                 };
            }
            catch (Exception ex)
            {
                return respuesta =
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
