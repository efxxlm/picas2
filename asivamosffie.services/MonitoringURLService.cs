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

        public async Task<List<ProyectoGrilla>> GetListProyects()
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
                         (bool)r.RegistroCompleto 
                         //Se quitan los proyectos que ya esten vinculados a una contratacion
                       
                         )
                                 .Include(r => r.ContratacionProyecto)
                                   .ThenInclude(r => r.Contratacion)
                                 .Include(r => r.Sede)
                                 .Include(r => r.InstitucionEducativa)
                                 .Include(r => r.LocalizacionIdMunicipioNavigation).Distinct().ToList();

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
                                    if (item.Contratacion.TipoSolicitudCodigo == "1" )
                                    {
                                        proyectoGrilla.TieneObra = true;
                                    }
                                    if (item.Contratacion.TipoSolicitudCodigo == "2")
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

            }
            catch (Exception ex)
            {
                return ListProyectoGrilla.OrderByDescending(r => r.ProyectoId).ToList();
            }
            return ListProyectoGrilla.OrderByDescending(r => r.ProyectoId).ToList();
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



    }
}
