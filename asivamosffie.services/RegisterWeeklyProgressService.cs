﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using System.IO;
using Z.EntityFramework.Plus;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Runtime.InteropServices.WindowsRuntime;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Microsoft.EntityFrameworkCore.Internal;

namespace asivamosffie.services
{
    public class RegisterWeeklyProgressService : IRegisterWeeklyProgressService
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public RegisterWeeklyProgressService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        #region Get
        public async Task<List<VRegistrarAvanceSemanal>> GetVRegistrarAvanceSemanal()
        {
            return await _context.VRegistrarAvanceSemanal.ToListAsync();
        }

        public async Task<SeguimientoSemanal> GetLastSeguimientoSemanalByContratacionProyectoIdOrSeguimientoSemanalId(int pContratacionProyectoId, int pSeguimientoSemanalId)
        {
            if (pContratacionProyectoId > 0)
            {
                return await _context.SeguimientoSemanal.Where(r => r.ContratacionProyectoId == pContratacionProyectoId && !(bool)r.Eliminado && !(bool)r.RegistroCompleto)
                    //Informacion Proyecto
                    .Include(r => r.ContratacionProyecto)
                       .ThenInclude(r => r.Contratacion)
                           .ThenInclude(r => r.Contrato)
                    .Include(r => r.ContratacionProyecto)
                       .ThenInclude(r => r.Proyecto)
                           .ThenInclude(r => r.InstitucionEducativa)
                    .Include(r => r.ContratacionProyecto)
                       .ThenInclude(r => r.Proyecto)
                  

                    .Include(r => r.SeguimientoDiario)
                           .ThenInclude(r => r.SeguimientoDiarioObservaciones)
                    //mAP SUMA DEL AVANCE POR CAPITULO DESDE SEMANA ACTUAL MIRAR TABLA ?

                    .Include(r => r.SeguimientoSemanalAvanceFinanciero)

                    .Include(r => r.SeguimientoSemanalAvanceFisico)

                    //Gestion Obra 
                    //Gestion Obra Ambiental
                    .Include(r => r.SeguimientoSemanalGestionObra)
                       .ThenInclude(r => r.SeguimientoSemanalGestionObraAmbiental)
                           .ThenInclude(r => r.ManejoMaterialesInsumo)
                                .ThenInclude(r => r.ManejoMaterialesInsumosProveedor)

                    .Include(r => r.SeguimientoSemanalGestionObra)
                       .ThenInclude(r => r.SeguimientoSemanalGestionObraAmbiental)
                           .ThenInclude(r => r.ManejoResiduosConstruccionDemolicion)
                               .ThenInclude(r => r.ManejoResiduosConstruccionDemolicionGestor)

                    .Include(r => r.SeguimientoSemanalGestionObra)
                       .ThenInclude(r => r.SeguimientoSemanalGestionObraAmbiental)
                           .ThenInclude(r => r.ManejoResiduosPeligrososEspeciales)

                     .Include(r => r.SeguimientoSemanalGestionObra)
                        .ThenInclude(r => r.SeguimientoSemanalGestionObraAmbiental)
                            .ThenInclude(r => r.ManejoResiduosPeligrososEspeciales)

                     .Include(r => r.SeguimientoSemanalGestionObra)
                        .ThenInclude(r => r.SeguimientoSemanalGestionObraAmbiental)
                            .ThenInclude(r => r.ManejoOtro)

                     //Gestion Obra Calidad
                     .Include(r => r.SeguimientoSemanalGestionObra)
                        .ThenInclude(r => r.SeguimientoSemanalGestionObraCalidad)
                            .ThenInclude(r => r.GestionObraCalidadEnsayoLaboratorio)
                                .ThenInclude(r => r.EnsayoLaboratorioMuestra)

                     //Gestion Obra Calidad
                     .Include(r => r.SeguimientoSemanalGestionObra)
                        .ThenInclude(r => r.SeguimientoSemanalGestionObraSeguridadSalud)
                            .ThenInclude(r => r.SeguridadSaludCausaAccidente)

                     //Gestion Obra Social
                     .Include(r => r.SeguimientoSemanalGestionObra)
                        .ThenInclude(r => r.SeguimientoSemanalGestionObraSocial)

                    .Include(r => r.SeguimientoSemanalGestionObra)
                        .ThenInclude(r => r.SeguimientoSemanalGestionObraAlerta)


                    .Include(r => r.SeguimientoSemanalReporteActividad)

                    .Include(r => r.SeguimientoSemanalRegistroFotografico)

                    .Include(r => r.SeguimientoSemanalRegistrarComiteObra)

                    .FirstOrDefaultAsync();
            }
            else
            {
                return await _context.SeguimientoSemanal.Where(r => r.SeguimientoSemanalId == pSeguimientoSemanalId)
                      //Informacion Proyecto
                      .Include(r => r.ContratacionProyecto)
                         .ThenInclude(r => r.Contratacion)
                             .ThenInclude(r => r.Contrato)
                      .Include(r => r.ContratacionProyecto)
                         .ThenInclude(r => r.Proyecto)
                             .ThenInclude(r => r.InstitucionEducativa)
                      .Include(r => r.ContratacionProyecto)
                         .ThenInclude(r => r.Proyecto)
                             .ThenInclude(r => r.LocalizacionIdMunicipio)

                      .Include(r => r.SeguimientoDiario)
                             .ThenInclude(r => r.SeguimientoDiarioObservaciones)
                      //mAP SUMA DEL AVANCE POR CAPITULO DESDE SEMANA ACTUAL MIRAR TABLA ?

                      .Include(r => r.SeguimientoSemanalAvanceFinanciero)

                      .Include(r => r.SeguimientoSemanalAvanceFisico)

                      //Gestion Obra 
                      //Gestion Obra Ambiental
                      .Include(r => r.SeguimientoSemanalGestionObra)
                         .ThenInclude(r => r.SeguimientoSemanalGestionObraAmbiental)
                             .ThenInclude(r => r.ManejoMaterialesInsumo)
                                  .ThenInclude(r => r.ManejoMaterialesInsumosProveedor)

                      .Include(r => r.SeguimientoSemanalGestionObra)
                         .ThenInclude(r => r.SeguimientoSemanalGestionObraAmbiental)
                             .ThenInclude(r => r.ManejoResiduosConstruccionDemolicion)
                                 .ThenInclude(r => r.ManejoResiduosConstruccionDemolicionGestor)

                      .Include(r => r.SeguimientoSemanalGestionObra)
                         .ThenInclude(r => r.SeguimientoSemanalGestionObraAmbiental)
                             .ThenInclude(r => r.ManejoResiduosPeligrososEspeciales)

                       .Include(r => r.SeguimientoSemanalGestionObra)
                          .ThenInclude(r => r.SeguimientoSemanalGestionObraAmbiental)
                              .ThenInclude(r => r.ManejoResiduosPeligrososEspeciales)

                       .Include(r => r.SeguimientoSemanalGestionObra)
                          .ThenInclude(r => r.SeguimientoSemanalGestionObraAmbiental)
                              .ThenInclude(r => r.ManejoOtro)

                       //Gestion Obra Calidad
                       .Include(r => r.SeguimientoSemanalGestionObra)
                          .ThenInclude(r => r.SeguimientoSemanalGestionObraCalidad)
                              .ThenInclude(r => r.GestionObraCalidadEnsayoLaboratorio)
                                  .ThenInclude(r => r.EnsayoLaboratorioMuestra)

                       //Gestion Obra Calidad
                       .Include(r => r.SeguimientoSemanalGestionObra)
                          .ThenInclude(r => r.SeguimientoSemanalGestionObraSeguridadSalud)
                              .ThenInclude(r => r.SeguridadSaludCausaAccidente)

                       //Gestion Obra Social
                       .Include(r => r.SeguimientoSemanalGestionObra)
                          .ThenInclude(r => r.SeguimientoSemanalGestionObraSocial)

                      .Include(r => r.SeguimientoSemanalGestionObra)
                          .ThenInclude(r => r.SeguimientoSemanalGestionObraAlerta)


                      .Include(r => r.SeguimientoSemanalReporteActividad)

                      .Include(r => r.SeguimientoSemanalRegistroFotografico)

                      .Include(r => r.SeguimientoSemanalRegistrarComiteObra)

                      .FirstOrDefaultAsync();



            }
        }

        public async Task<List<dynamic>> GetListSeguimientoSemanalBypContratacionProyectoId(int pContratacionProyectoId)
        {

            List<SeguimientoSemanal> ListseguimientoSemanal = await _context.SeguimientoSemanal.Where(r => r.ContratacionProyectoId == pContratacionProyectoId)
                .Include(r => r.ContratacionProyecto)
                .ThenInclude(r => r.Proyecto)
                   .Include(r => r.ContratacionProyecto)
                .ThenInclude(r => r.Contratacion)
                 .ThenInclude(r => r.Contrato)
                .ToListAsync();

            List<dynamic> ListBitaCora = new List<dynamic>();

            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            

            foreach (var item in ListseguimientoSemanal)
            {
                ListBitaCora.Add(new {
                    item.ContratacionProyecto?.Proyecto?.LlaveMen,
                    item.ContratacionProyecto?.Contratacion?.Contrato?.FirstOrDefault().NumeroContrato,



                });


            } 
            return ListBitaCora;
        }
        #endregion
        public async Task<Respuesta> SaveUpdateSeguimientoSemanal(SeguimientoSemanal pSeguimientoSemanal)
        {

            bool registroCompleto = false;
            try
            {
                SeguimientoSemanal seguimientoSemanalMod = await _context.SeguimientoSemanal.FindAsync(pSeguimientoSemanal.SeguimientoSemanalId);

                SaveUpdateAvanceFisico(pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault());

                SaveUpdateAvanceFinanciero(pSeguimientoSemanal.SeguimientoSemanalAvanceFinanciero.FirstOrDefault());

                SaveUpdateGestionObra(pSeguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault());

                SaveUpdateReporteActividades(pSeguimientoSemanal.SeguimientoSemanalReporteActividad.FirstOrDefault());

                SaveUpdateRegistroFotografico(pSeguimientoSemanal.SeguimientoSemanalRegistroFotografico.FirstOrDefault());

                SaveUpdateComiteObra(pSeguimientoSemanal.SeguimientoSemanalRegistrarComiteObra.FirstOrDefault());



                return new Respuesta();
            }
            catch (Exception)
            {

                return new Respuesta();
            }


        }

        private bool SaveUpdateComiteObra(SeguimientoSemanalRegistrarComiteObra seguimientoSemanalRegistrarComiteObra)
        {
            throw new NotImplementedException();
        }

        private bool SaveUpdateRegistroFotografico(SeguimientoSemanalRegistroFotografico seguimientoSemanalRegistroFotografico)
        {
            throw new NotImplementedException();
        }

        private bool SaveUpdateReporteActividades(SeguimientoSemanalReporteActividad seguimientoSemanalReporteActividad)
        {
            throw new NotImplementedException();
        }

        private bool SaveUpdateGestionObra(SeguimientoSemanalGestionObra seguimientoSemanalGestionObra)
        {

            if (seguimientoSemanalGestionObra.SeguimientoSemanalGestionObraId == 0)
            {

            }

            return false;
        }

        private bool SaveUpdateAvanceFinanciero(SeguimientoSemanalAvanceFinanciero seguimientoSemanalAvanceFinanciero)
        {
            throw new NotImplementedException();
        }

        private bool SaveUpdateAvanceFisico(SeguimientoSemanalAvanceFisico seguimientoSemanalAvanceFisico)
        {
            throw new NotImplementedException();
        }
    }
}
