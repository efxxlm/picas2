using System;
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

namespace asivamosffie.services
{
    public class ProjectContractingService : IProjectContractingService
    {

        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        private readonly devAsiVamosFFIEContext _context;

        public ProjectContractingService(devAsiVamosFFIEContext context, ICommonService commonService, IDocumentService documentService)
        {
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
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

            List<ProyectoGrilla> ListProyectoGrilla = new List<ProyectoGrilla>();
            try
            {
                //Listar Los proyecto segun caso de uso solo trae los ue estado
                //estado de registro “Completo”, que tienen viabilidad jurídica y técnica

                string strCodigoEstadoJuridicoAprobado = ConstantCodigoEstadoJuridico.Aprobado;
                string strCodigoEstadoProyectoCompleto = ConstantCodigoEstadoProyecto.Completo;

                IQueryable<Proyecto> ListProyectos =
                    _context.Proyecto.Where(
                        r => !(bool)r.Eliminado &&
                        r.EstadoJuridicoCodigo.Equals(strCodigoEstadoJuridicoAprobado) &&
                        r.EstadoProyectoCodigo.Equals(strCodigoEstadoJuridicoAprobado) &&
                        r.EstadoProyectoCodigo.Equals(strCodigoEstadoProyectoCompleto));

                if (!string.IsNullOrEmpty(pTipoIntervencion))
                {
                    ListProyectos = ListProyectos.Where(r => r.TipoIntervencionCodigo.Equals(pTipoIntervencion));
                }
                if (!string.IsNullOrEmpty(pLlaveMen))
                {
                    ListProyectos = ListProyectos.Where(r => r.LlaveMen.Equals(pLlaveMen));
                }
                if (!string.IsNullOrEmpty(pMunicipio))
                {
                    ListProyectos = ListProyectos.Where(r => r.LocalizacionIdMunicipio.Equals(pMunicipio));
                }
                if (pIdInstitucionEducativa != null || pIdInstitucionEducativa > 0)
                {
                    ListProyectos = ListProyectos.Where(r => r.InstitucionEducativaId.Equals(pIdInstitucionEducativa));
                }
                if (pIdSede != null || pIdSede > 0)
                {
                    ListProyectos = ListProyectos.Where(r => r.SedeId.Equals(pIdSede));
                }


                foreach (var proyecto in ListProyectos)
                {
                    Localizacion municipio = await _commonService.GetLocalizacionByLocalizacionId(proyecto.LocalizacionIdMunicipio);
                    Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio estadoRegistro = await _commonService.GetDominioByNombreDominioAndTipoDominio(proyecto.EstadoProyectoCodigo, (int)EnumeratorTipoDominio.Estado_Registro);
                    Dominio EstadoJuridicoPredios = await _commonService.GetDominioByNombreDominioAndTipoDominio(proyecto.EstadoJuridicoCodigo, (int)EnumeratorTipoDominio.Estado_Registro);

                    ProyectoGrilla proyectoGrilla = new ProyectoGrilla
                    {
                        ProyectoId = proyecto.ProyectoId,
                        Departamento = departamento.Descripcion,
                        Municipio = municipio.Descripcion,
                        InstitucionEducativa = _context.InstitucionEducativaSede.Find(proyecto.InstitucionEducativaId).Nombre,
                        Sede = _context.InstitucionEducativaSede.Find(proyecto.SedeId).Nombre,
                        EstadoRegistro = estadoRegistro.Nombre,
                        EstadoJuridicoPredios = EstadoJuridicoPredios.Nombre
                    };
                    ListProyectoGrilla.Add(proyectoGrilla);
                }

                return ListProyectoGrilla;


            }
            catch (Exception ex)
            {
                return ListProyectoGrilla;

            }
        }


    }
}
