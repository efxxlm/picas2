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
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();

            if (pContratacionProyectoId > 0)
            {
                SeguimientoSemanal seguimientoSemanal = await _context.SeguimientoSemanal.Where(r => r.ContratacionProyectoId == pContratacionProyectoId && !(bool)r.Eliminado && !(bool)r.RegistroCompleto)
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

                   .Include(r => r.SeguimientoSemanalAvanceFinanciero)
                   .Include(r => r.FlujoInversion)
                       .ThenInclude(r => r.Programacion)
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


                Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == seguimientoSemanal.ContratacionProyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == seguimientoSemanal.ContratacionProyecto.Proyecto.SedeId).FirstOrDefault();
                InstitucionEducativaSede institucionEducativa = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == Sede.PadreId).FirstOrDefault();

                seguimientoSemanal.ContratacionProyecto.Proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == seguimientoSemanal.ContratacionProyecto.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                seguimientoSemanal.ContratacionProyecto.Proyecto.MunicipioObj = Municipio;
                seguimientoSemanal.ContratacionProyecto.Proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                seguimientoSemanal.ContratacionProyecto.Proyecto.Sede = Sede;
                seguimientoSemanal.ContratacionProyecto.Proyecto.InstitucionEducativa = institucionEducativa;
                return seguimientoSemanal;
            }
            else
            {
                SeguimientoSemanal seguimientoSemanal = await _context.SeguimientoSemanal.Where(r => r.SeguimientoSemanalId == pSeguimientoSemanalId)
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
                 .Include(r => r.FlujoInversion)
                       .ThenInclude(r => r.Programacion)
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


                Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == seguimientoSemanal.ContratacionProyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == seguimientoSemanal.ContratacionProyecto.Proyecto.SedeId).FirstOrDefault();
                InstitucionEducativaSede institucionEducativa = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == Sede.PadreId).FirstOrDefault();


                seguimientoSemanal.ContratacionProyecto.Proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == seguimientoSemanal.ContratacionProyecto.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                seguimientoSemanal.ContratacionProyecto.Proyecto.MunicipioObj = Municipio;
                seguimientoSemanal.ContratacionProyecto.Proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                seguimientoSemanal.ContratacionProyecto.Proyecto.Sede = Sede;
                seguimientoSemanal.ContratacionProyecto.Proyecto.InstitucionEducativa = institucionEducativa;
                return seguimientoSemanal;



            }
        }

        public async Task<List<dynamic>> GetListSeguimientoSemanalByContratacionProyectoId(int pContratacionProyectoId)
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
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();

            foreach (var item in ListseguimientoSemanal)
            {
                InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == item.ContratacionProyecto?.Proyecto?.SedeId).FirstOrDefault();
                InstitucionEducativaSede institucionEducativa = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == Sede.PadreId).FirstOrDefault();

                ListBitaCora.Add(new
                {
                    item.ContratacionProyecto?.Proyecto?.LlaveMen,
                    item.ContratacionProyecto?.Contratacion?.Contrato?.FirstOrDefault().NumeroContrato,
                    TipoIntervencion = !string.IsNullOrEmpty(item.ContratacionProyecto?.Proyecto.TipoIntervencionCodigo) ? TipoIntervencion.Where(r => r.Codigo == item.ContratacionProyecto?.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre : " ",
                    institucionEducativa.Nombre,
                    Sede = Sede.Nombre,
                    item.FechaModificacion,
                    EstadoObra = item.EstadoObraCodigo
                });


            }
            return ListBitaCora;
        }
        #endregion

        #region Save Edit
        public async Task<Respuesta> SaveUpdateSeguimientoSemanal(SeguimientoSemanal pSeguimientoSemanal)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Seguimiento_Semanal, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SeguimientoSemanal seguimientoSemanalMod = await _context.SeguimientoSemanal.FindAsync(pSeguimientoSemanal.SeguimientoSemanalId);

                SaveUpdateAvanceFisico(pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault(), pSeguimientoSemanal.FlujoInversion, pSeguimientoSemanal.UsuarioCreacion);

                SaveUpdateAvanceFinanciero(pSeguimientoSemanal.SeguimientoSemanalAvanceFinanciero.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                SaveUpdateGestionObra(pSeguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                SaveUpdateReporteActividades(pSeguimientoSemanal.SeguimientoSemanalReporteActividad.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                SaveUpdateRegistroFotografico(pSeguimientoSemanal.SeguimientoSemanalRegistroFotografico.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                SaveUpdateComiteObra(pSeguimientoSemanal.SeguimientoSemanalRegistrarComiteObra.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                await _context.SaveChangesAsync();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pSeguimientoSemanal.UsuarioCreacion, pSeguimientoSemanal.FechaModificacion.HasValue ? "CREAR SEGUIMIENTO SEMANAL" : "EDITAR SEGUIMIENTO SEMANAL")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pSeguimientoSemanal.UsuarioCreacion, ex.InnerException.ToString())
                };
            }


        }

        private void SaveUpdateAvanceFisico(SeguimientoSemanalAvanceFisico pSeguimientoSemanalAvanceFisico, ICollection<FlujoInversion> pListFlujoInversion, string usuarioCreacion)
        {
            bool RegistroCompleto = true;

            foreach (var FlujoInversion in pListFlujoInversion)
            {
                Programacion programacionOld = _context.Programacion.Where(r => r.ProgramacionId == FlujoInversion.ProgramacionId).FirstOrDefault();
                programacionOld.AvanceFisicoCapitulo = FlujoInversion.Programacion.AvanceFisicoCapitulo;

                if (!programacionOld.AvanceFisicoCapitulo.HasValue)
                {
                    RegistroCompleto = false;
                }
            }

            if (pSeguimientoSemanalAvanceFisico.SeguimientoSemanalAvanceFisicoId == 0)
            {
                pSeguimientoSemanalAvanceFisico.RegistroCompleto = RegistroCompleto;
                pSeguimientoSemanalAvanceFisico.UsuarioCreacion = usuarioCreacion;
                pSeguimientoSemanalAvanceFisico.FechaCreacion = DateTime.Now;
                pSeguimientoSemanalAvanceFisico.Eliminado = false;
            }
            {
                SeguimientoSemanalAvanceFisico seguimientoSemanalAvanceFisicoOld = _context.SeguimientoSemanalAvanceFisico.Find();

                seguimientoSemanalAvanceFisicoOld.RegistroCompleto = RegistroCompleto;
                pSeguimientoSemanalAvanceFisico.UsuarioModificacion = usuarioCreacion;
                pSeguimientoSemanalAvanceFisico.FechaModificacion = DateTime.Now;
            }

        }

        private void SaveUpdateAvanceFinanciero(SeguimientoSemanalAvanceFinanciero pSeguimientoSemanalAvanceFinanciero, string pUsuarioCreacion)
        {
            throw new NotImplementedException();
        }

        private void SaveUpdateGestionObra(SeguimientoSemanalGestionObra pSeguimientoSemanalGestionObra, string pUsuarioCreacion)
        {
            if (pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraId == 0)
            {
                pSeguimientoSemanalGestionObra.UsuarioCreacion = pUsuarioCreacion;
                pSeguimientoSemanalGestionObra.Eliminado = false;
                pSeguimientoSemanalGestionObra.FechaCreacion = DateTime.Now;

                //Gestion Ambiental
                foreach (var SeguimientoSemanalGestionObraAmbiental in pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraAmbiental)
                {
                    SeguimientoSemanalGestionObraAmbiental.UsuarioCreacion = pUsuarioCreacion;
                    SeguimientoSemanalGestionObraAmbiental.Eliminado = false;
                    SeguimientoSemanalGestionObraAmbiental.FechaCreacion = DateTime.Now;
                    SeguimientoSemanalGestionObraAmbiental.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraAmbiental(SeguimientoSemanalGestionObraAmbiental);
                    //Manejo Materiales Insumo
                    SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.UsuarioCreacion = pUsuarioCreacion;
                    SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.Eliminado = false;
                    SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.FechaCreacion = DateTime.Now;
                    SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumo(SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo);

                    foreach (var ManejoMaterialesInsumosProveedor in SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor)
                    {
                        ManejoMaterialesInsumosProveedor.UsuarioCreacion = pUsuarioCreacion;
                        ManejoMaterialesInsumosProveedor.Eliminado = false;
                        ManejoMaterialesInsumosProveedor.FechaCreacion = DateTime.Now;
                        ManejoMaterialesInsumosProveedor.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumosProveedor(ManejoMaterialesInsumosProveedor);
                    }
                }
                _context.SeguimientoSemanalGestionObra.Add(pSeguimientoSemanalGestionObra);
            }
        }

        private void SaveUpdateReporteActividades(SeguimientoSemanalReporteActividad pSeguimientoSemanalReporteActividad, string pUsuarioCreacion)
        {
            throw new NotImplementedException();
        }

        private void SaveUpdateRegistroFotografico(SeguimientoSemanalRegistroFotografico pSeguimientoSemanalRegistroFotografico, string pUsuarioCreacion)
        {
            throw new NotImplementedException();
        }

        private void SaveUpdateComiteObra(SeguimientoSemanalRegistrarComiteObra pSeguimientoSemanalRegistrarComiteObra, string pUsuarioCreacion)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Validar Registros Completos
        private bool? ValidarRegistroCompletoManejoMaterialesInsumosProveedor(ManejoMaterialesInsumosProveedor manejoMaterialesInsumosProveedor)
        {
            return false;
        }

        private bool? ValidarRegistroCompletoManejoMaterialesInsumo(ManejoMaterialesInsumos manejoMaterialesInsumo)
        {
            return false;
        }

        private bool? ValidarRegistroCompletoSeguimientoSemanalGestionObraAmbiental(SeguimientoSemanalGestionObraAmbiental seguimientoSemanalGestionObraAmbiental)
        {
            return false;
        }

        #endregion
    }
}
