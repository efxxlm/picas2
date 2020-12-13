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

                foreach (var FlujoInversion in seguimientoSemanal.FlujoInversion)
                {
                    FlujoInversion.Programacion.RangoDias = (FlujoInversion.Programacion.FechaInicio - FlujoInversion.Programacion.FechaFin).TotalDays;
                }

                foreach (var SeguimientoSemanalGestionObra in seguimientoSemanal.SeguimientoSemanalGestionObra)
                {
                    foreach (var SeguimientoSemanalGestionObraAmbiental in SeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraAmbiental)
                    {
                        if (SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor.Count() > 0)
                            SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor.Where(r => !(bool)r.Eliminado).ToList();
                    }
                }
                 
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

                foreach (var FlujoInversion in seguimientoSemanal.FlujoInversion)
                {
                    FlujoInversion.Programacion.RangoDias = (FlujoInversion.Programacion.FechaInicio - FlujoInversion.Programacion.FechaFin).TotalDays;
                }

                foreach (var SeguimientoSemanalGestionObra in seguimientoSemanal.SeguimientoSemanalGestionObra)
                {
                    foreach (var SeguimientoSemanalGestionObraAmbiental in SeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraAmbiental)
                    {
                        if (SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor.Count() > 0)
                            SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor.Where(r => !(bool)r.Eliminado).ToList();
                    }
                }
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
            List<SeguimientoSemanal> ListseguimientoSemanal = await _context.SeguimientoSemanal.Where(r => r.ContratacionProyectoId == pContratacionProyectoId && r.RegistroCompleto == true)
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

        public async Task<Respuesta> DeleteManejoMaterialesInsumosProveedor(int ManejoMaterialesInsumosProveedorId, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Manejo_Materiales_Insumo_Proveedor, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ManejoMaterialesInsumosProveedor manejoMaterialesInsumosProveedorDelete = _context.ManejoMaterialesInsumosProveedor.Find(ManejoMaterialesInsumosProveedorId);
 
                if (manejoMaterialesInsumosProveedorDelete == null)
                {
                    return new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstantSesionComiteTecnico.Error, idAccion, pUsuarioModificacion, "ManejoMaterialesInsumosProveedor no encontrado".ToUpper())
                    };
                }
                manejoMaterialesInsumosProveedorDelete.UsuarioModificacion = pUsuarioModificacion;
                manejoMaterialesInsumosProveedorDelete.FechaModificacion = DateTime.Now;
                manejoMaterialesInsumosProveedorDelete.Eliminado = true;

                await _context.SaveChangesAsync();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pUsuarioModificacion, "Eliminar Manejo Materiales Insumo Proveedor".ToUpper())
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstantSesionComiteTecnico.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString())
                };
            }


        }

        public async Task<Respuesta> SaveUpdateSeguimientoSemanal(SeguimientoSemanal pSeguimientoSemanal)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Seguimiento_Semanal, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SeguimientoSemanal seguimientoSemanalMod = await _context.SeguimientoSemanal.FindAsync(pSeguimientoSemanal.SeguimientoSemanalId);

                if (pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.Count() > 0)
                    SaveUpdateAvanceFisico(pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault(), pSeguimientoSemanal.FlujoInversion, pSeguimientoSemanal.UsuarioCreacion);

                if (pSeguimientoSemanal.SeguimientoSemanalAvanceFinanciero.Count() > 0)
                    SaveUpdateAvanceFinanciero(pSeguimientoSemanal.SeguimientoSemanalAvanceFinanciero.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                if (pSeguimientoSemanal.SeguimientoSemanalGestionObra.Count() > 0)
                    SaveUpdateGestionObra(pSeguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                if (pSeguimientoSemanal.SeguimientoSemanalReporteActividad.Count() > 0)
                    SaveUpdateReporteActividades(pSeguimientoSemanal.SeguimientoSemanalReporteActividad.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                if (pSeguimientoSemanal.SeguimientoSemanalRegistroFotografico.Count() > 0)
                    SaveUpdateRegistroFotografico(pSeguimientoSemanal.SeguimientoSemanalRegistroFotografico.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                if (pSeguimientoSemanal.SeguimientoSemanalRegistrarComiteObra.Count() > 0)
                    SaveUpdateComiteObra(pSeguimientoSemanal.SeguimientoSemanalRegistrarComiteObra.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                await _context.SaveChangesAsync();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pSeguimientoSemanal.UsuarioCreacion, pSeguimientoSemanal.FechaModificacion.HasValue ? "CREAR SEGUIMIENTO SEMANAL" : "EDITAR SEGUIMIENTO SEMANAL")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstantSesionComiteTecnico.Error, idAccion, pSeguimientoSemanal.UsuarioCreacion, ex.InnerException.ToString())
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
            //New
            if (pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraId == 0)
            {
                //Auditoria
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

                    _context.SeguimientoSemanalGestionObraAmbiental.Add(SeguimientoSemanalGestionObraAmbiental);

                    //Manejo Materiales e Insumos
                    if (SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo != null)
                    {
                        SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.UsuarioCreacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.Eliminado = false;
                        SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.FechaCreacion = DateTime.Now;
                        SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumo(SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo);

                        _context.ManejoMaterialesInsumos.Add(SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo);

                        foreach (var ManejoMaterialesInsumosProveedor in SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor)
                        {
                            ManejoMaterialesInsumosProveedor.UsuarioCreacion = pUsuarioCreacion;
                            ManejoMaterialesInsumosProveedor.Eliminado = false;
                            ManejoMaterialesInsumosProveedor.FechaCreacion = DateTime.Now;
                            ManejoMaterialesInsumosProveedor.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumosProveedor(ManejoMaterialesInsumosProveedor);

                            _context.ManejoMaterialesInsumosProveedor.Add(ManejoMaterialesInsumosProveedor);
                        }
                    }

                    //Manejo Residuos Construccion Demolicion
                    if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion != null)
                    {

                        SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.UsuarioCreacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.Eliminado = false;
                        SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.FechaCreacion = DateTime.Now;
                        SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.RegistroCompleto = ValidarRegistroCompletoManejoResiduosConstruccionDemolicion(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion);

                        _context.ManejoResiduosConstruccionDemolicion.Add(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion);

                        foreach (var ManejoResiduosConstruccionDemolicionGestor in SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionGestor)
                        {
                            ManejoResiduosConstruccionDemolicionGestor.UsuarioCreacion = pUsuarioCreacion;
                            ManejoResiduosConstruccionDemolicionGestor.Eliminado = false;
                            ManejoResiduosConstruccionDemolicionGestor.FechaCreacion = DateTime.Now;
                            ManejoResiduosConstruccionDemolicionGestor.RegistroCompleto = ValidarRegistroCompletoManejoResiduosConstruccionDemolicionGestor(ManejoResiduosConstruccionDemolicionGestor);

                            _context.ManejoResiduosConstruccionDemolicionGestor.Add(ManejoResiduosConstruccionDemolicionGestor);
                        }
                    }

                    //Manejo Residuo Peligrosos Especiales
                    if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales != null)
                    {

                        SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.UsuarioCreacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.Eliminado = false;
                        SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.FechaCreacion = DateTime.Now;
                        SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.RegistroCompleto = ValidarRegistroCompletoManejoResiduosPeligrososEspeciales(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales);

                        _context.ManejoResiduosPeligrososEspeciales.Add(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales);

                    }

                    //Manejo Otro
                    if (SeguimientoSemanalGestionObraAmbiental.ManejoOtro != null)
                    {
                        SeguimientoSemanalGestionObraAmbiental.ManejoOtro.UsuarioCreacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraAmbiental.ManejoOtro.Eliminado = false;
                        SeguimientoSemanalGestionObraAmbiental.ManejoOtro.FechaCreacion = DateTime.Now;
                        SeguimientoSemanalGestionObraAmbiental.ManejoOtro.RegistroCompleto = ValidarRegistroCompletoManejoOtro(SeguimientoSemanalGestionObraAmbiental.ManejoOtro);

                        _context.ManejoOtro.Add(SeguimientoSemanalGestionObraAmbiental.ManejoOtro);
                    }

                }

                pSeguimientoSemanalGestionObra.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObra(pSeguimientoSemanalGestionObra);
                _context.SeguimientoSemanalGestionObra.Add(pSeguimientoSemanalGestionObra);
            }
            //Update
            else
            {

                SeguimientoSemanalGestionObra seguimientoSemanalGestionObraOld = _context.SeguimientoSemanalGestionObra.Find(pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraId);
                //Auditoria
                seguimientoSemanalGestionObraOld.UsuarioModificacion = pUsuarioCreacion;
                seguimientoSemanalGestionObraOld.FechaModificacion = DateTime.Now;


                //Gestion Ambiental
                foreach (var SeguimientoSemanalGestionObraAmbiental in pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraAmbiental)
                {
                    SeguimientoSemanalGestionObraAmbiental SeguimientoSemanalGestionObraAmbientalOld = _context.SeguimientoSemanalGestionObraAmbiental.Find(SeguimientoSemanalGestionObraAmbiental.SeguimientoSemanalGestionObraAmbientalId);

                    SeguimientoSemanalGestionObraAmbientalOld.UsuarioModificacion = pUsuarioCreacion;
                    SeguimientoSemanalGestionObraAmbientalOld.FechaModificacion = DateTime.Now;

                    SeguimientoSemanalGestionObraAmbientalOld.SeEjecutoGestionAmbiental = SeguimientoSemanalGestionObraAmbiental.SeEjecutoGestionAmbiental;

                    //Manejo Materiales e Insumos
                    if (SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo != null)
                    {
                        //New
                        if (SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosId == 0)
                        {
                            SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.UsuarioCreacion = pUsuarioCreacion;
                            SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.Eliminado = false;
                            SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.FechaCreacion = DateTime.Now;
                            SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumo(SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo);

                            _context.ManejoMaterialesInsumos.Add(SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo);
                        }
                        //Update
                        else
                        {
                            ManejoMaterialesInsumos manejoMaterialesInsumosOld = _context.ManejoMaterialesInsumos.Find(SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosId);

                            manejoMaterialesInsumosOld.FechaModificacion = DateTime.Now;
                            manejoMaterialesInsumosOld.UsuarioModificacion = pUsuarioCreacion;

                            manejoMaterialesInsumosOld.EstanProtegidosDemarcadosMateriales = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.EstanProtegidosDemarcadosMateriales;
                            manejoMaterialesInsumosOld.RequiereObservacion = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.RequiereObservacion;
                            manejoMaterialesInsumosOld.Observacion = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.Observacion;
                            manejoMaterialesInsumosOld.Url = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.Url;
                            manejoMaterialesInsumosOld.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumo(manejoMaterialesInsumosOld);
                        }
                        foreach (var ManejoMaterialesInsumosProveedor in SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor)
                        {
                            if (ManejoMaterialesInsumosProveedor.ManejoMaterialesInsumosProveedorId == 0)
                            {
                                ManejoMaterialesInsumosProveedor.UsuarioCreacion = pUsuarioCreacion;
                                ManejoMaterialesInsumosProveedor.Eliminado = false;
                                ManejoMaterialesInsumosProveedor.FechaCreacion = DateTime.Now;
                                ManejoMaterialesInsumosProveedor.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumosProveedor(ManejoMaterialesInsumosProveedor);

                                _context.ManejoMaterialesInsumosProveedor.Add(ManejoMaterialesInsumosProveedor);
                            }
                            else
                            {
                                ManejoMaterialesInsumosProveedor manejoMaterialesInsumosProveedorOld = _context.ManejoMaterialesInsumosProveedor.Find(ManejoMaterialesInsumosProveedor.ManejoMaterialesInsumosProveedorId);
                                manejoMaterialesInsumosProveedorOld.UsuarioModificacion = pUsuarioCreacion;
                                ManejoMaterialesInsumosProveedor.FechaModificacion = DateTime.Now;

                                manejoMaterialesInsumosProveedorOld.Proveedor = ManejoMaterialesInsumosProveedor.Proveedor;
                                manejoMaterialesInsumosProveedorOld.RequierePermisosAmbientalesMineros = ManejoMaterialesInsumosProveedor.RequierePermisosAmbientalesMineros;
                                manejoMaterialesInsumosProveedorOld.UrlRegistroFotografico = ManejoMaterialesInsumosProveedor.UrlRegistroFotografico;
                                manejoMaterialesInsumosProveedorOld.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumosProveedor(manejoMaterialesInsumosProveedorOld);
                            }
                        }
                    }

                    //Manejo Residuos Construccion Demolicion
                    if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion != null)
                    {
                        if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionId == 0)
                        {
                            SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.UsuarioCreacion = pUsuarioCreacion;
                            SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.Eliminado = false;
                            SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.FechaCreacion = DateTime.Now;
                            SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.RegistroCompleto = ValidarRegistroCompletoManejoResiduosConstruccionDemolicion(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion);

                            _context.ManejoResiduosConstruccionDemolicion.Add(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion);

                        }
                        else
                        {
                            ManejoResiduosConstruccionDemolicion manejoResiduosConstruccionDemolicionOld = _context.ManejoResiduosConstruccionDemolicion.Find(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionId);

                            manejoResiduosConstruccionDemolicionOld.RegistroCompleto = ValidarRegistroCompletoManejoResiduosConstruccionDemolicion(manejoResiduosConstruccionDemolicionOld);
                            manejoResiduosConstruccionDemolicionOld.UsuarioModificacion = pUsuarioCreacion;
                            manejoResiduosConstruccionDemolicionOld.FechaModificacion = DateTime.Now;

                            manejoResiduosConstruccionDemolicionOld.EstaCuantificadoRcd = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.EstaCuantificadoRcd;
                            manejoResiduosConstruccionDemolicionOld.RequiereObservacion = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.RequiereObservacion;
                            manejoResiduosConstruccionDemolicionOld.Observacion = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.Observacion;
                            manejoResiduosConstruccionDemolicionOld.SeReutilizadorResiduos = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.SeReutilizadorResiduos;
                            manejoResiduosConstruccionDemolicionOld.CantidadToneladas = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.CantidadToneladas;


                        }

                        foreach (var ManejoResiduosConstruccionDemolicionGestor in SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionGestor)
                        {

                            if (ManejoResiduosConstruccionDemolicionGestor.ManejoResiduosConstruccionDemolicionGestorId == 0)
                            {
                                ManejoResiduosConstruccionDemolicionGestor.UsuarioCreacion = pUsuarioCreacion;
                                ManejoResiduosConstruccionDemolicionGestor.Eliminado = false;
                                ManejoResiduosConstruccionDemolicionGestor.FechaCreacion = DateTime.Now;
                                ManejoResiduosConstruccionDemolicionGestor.RegistroCompleto = ValidarRegistroCompletoManejoResiduosConstruccionDemolicionGestor(ManejoResiduosConstruccionDemolicionGestor);

                                _context.ManejoResiduosConstruccionDemolicionGestor.Add(ManejoResiduosConstruccionDemolicionGestor);
                            }
                            else
                            {
                                ManejoResiduosConstruccionDemolicionGestor ManejoResiduosConstruccionDemolicionGestorOld = _context.ManejoResiduosConstruccionDemolicionGestor.Find(ManejoResiduosConstruccionDemolicionGestor.ManejoResiduosConstruccionDemolicionGestorId);
                                ManejoResiduosConstruccionDemolicionGestorOld.RegistroCompleto = ValidarRegistroCompletoManejoResiduosConstruccionDemolicionGestor(ManejoResiduosConstruccionDemolicionGestor);
                                ManejoResiduosConstruccionDemolicionGestorOld.UsuarioModificacion = pUsuarioCreacion;
                                ManejoResiduosConstruccionDemolicionGestorOld.FechaModificacion = DateTime.Now;

                                ManejoResiduosConstruccionDemolicionGestorOld.NombreGestorResiduos = ManejoResiduosConstruccionDemolicionGestor.NombreGestorResiduos;
                                ManejoResiduosConstruccionDemolicionGestorOld.TienePermisoAmbiental = ManejoResiduosConstruccionDemolicionGestor.TienePermisoAmbiental;
                                ManejoResiduosConstruccionDemolicionGestorOld.Url = ManejoResiduosConstruccionDemolicionGestor.Url;
                            }
                        }
                    }

                    //Manejo Residuo Peligrosos Especiales
                    if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales != null)
                    {
                        if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.ManejoResiduosPeligrososEspecialesId == 0)
                        {
                            SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.UsuarioCreacion = pUsuarioCreacion;
                            SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.Eliminado = false;
                            SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.FechaCreacion = DateTime.Now;
                            SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.RegistroCompleto = ValidarRegistroCompletoManejoResiduosPeligrososEspeciales(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales);
                        }
                        else
                        {
                            ManejoResiduosPeligrososEspeciales ManejoResiduosPeligrososEspecialesOld = _context.ManejoResiduosPeligrososEspeciales.Find(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.ManejoResiduosPeligrososEspecialesId);

                            ManejoResiduosPeligrososEspecialesOld.RegistroCompleto = ValidarRegistroCompletoManejoResiduosPeligrososEspeciales(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales);
                            ManejoResiduosPeligrososEspecialesOld.UsuarioModificacion = pUsuarioCreacion;
                            ManejoResiduosPeligrososEspecialesOld.FechaModificacion = DateTime.Now;

                            ManejoResiduosPeligrososEspecialesOld.UrlRegistroFotografico = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.UrlRegistroFotografico;
                            ManejoResiduosPeligrososEspecialesOld.EstanClasificados = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.EstanClasificados;
                            ManejoResiduosPeligrososEspecialesOld.RequiereObservacion = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.RequiereObservacion;
                            ManejoResiduosPeligrososEspecialesOld.Observacion = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.Observacion;
                        }
                    }

                    //Manejo Otro
                    if (SeguimientoSemanalGestionObraAmbiental.ManejoOtro != null)
                    {
                        if (SeguimientoSemanalGestionObraAmbiental.ManejoOtro.ManejoOtroId == 0)
                        {
                            SeguimientoSemanalGestionObraAmbiental.ManejoOtro.UsuarioCreacion = pUsuarioCreacion;
                            SeguimientoSemanalGestionObraAmbiental.ManejoOtro.Eliminado = false;
                            SeguimientoSemanalGestionObraAmbiental.ManejoOtro.FechaCreacion = DateTime.Now;
                            SeguimientoSemanalGestionObraAmbiental.ManejoOtro.RegistroCompleto = ValidarRegistroCompletoManejoOtro(SeguimientoSemanalGestionObraAmbiental.ManejoOtro);
                            _context.ManejoOtro.Add(SeguimientoSemanalGestionObraAmbiental.ManejoOtro);
                        }
                        else
                        {
                            ManejoOtro manejoOtroOld = _context.ManejoOtro.Find(SeguimientoSemanalGestionObraAmbiental.ManejoOtro.ManejoOtroId);

                            manejoOtroOld.RegistroCompleto = ValidarRegistroCompletoManejoOtro(SeguimientoSemanalGestionObraAmbiental.ManejoOtro);
                            manejoOtroOld.UsuarioModificacion = pUsuarioCreacion;
                            manejoOtroOld.FechaModificacion = DateTime.Now;

                            manejoOtroOld.FechaActividad = SeguimientoSemanalGestionObraAmbiental.ManejoOtro.FechaActividad;
                            manejoOtroOld.Actividad = SeguimientoSemanalGestionObraAmbiental.ManejoOtro.Actividad;
                            manejoOtroOld.UrlSoporteGestion = SeguimientoSemanalGestionObraAmbiental.ManejoOtro.UrlSoporteGestion;
                        }
                    }
                }

                seguimientoSemanalGestionObraOld.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObra(pSeguimientoSemanalGestionObra);
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

        private bool ValidarRegistroCompletoSeguimientoSemanalGestionObra(SeguimientoSemanalGestionObra pSeguimientoSemanalGestionObra)
        {
            return false;
        }

        #region Gestion Obra Ambiental

        private bool ValidarRegistroCompletoSeguimientoSemanalGestionObraAmbiental(SeguimientoSemanalGestionObraAmbiental pSeguimientoSemanalGestionObraAmbiental)
        {
            return false;
        }

        private bool ValidarRegistroCompletoManejoMaterialesInsumo(ManejoMaterialesInsumos pManejoMaterialesInsumo)
        {
            return false;
        }

        private bool ValidarRegistroCompletoManejoMaterialesInsumosProveedor(ManejoMaterialesInsumosProveedor pManejoMaterialesInsumosProveedor)
        {
            return false;
        }

        private bool ValidarRegistroCompletoManejoResiduosConstruccionDemolicion(ManejoResiduosConstruccionDemolicion pManejoResiduosConstruccionDemolicion)
        {
            return false;
        }

        private bool ValidarRegistroCompletoManejoResiduosConstruccionDemolicionGestor(ManejoResiduosConstruccionDemolicionGestor pManejoResiduosConstruccionDemolicionGestor)
        {
            return false;
        }

        private bool ValidarRegistroCompletoManejoResiduosPeligrososEspeciales(ManejoResiduosPeligrososEspeciales pManejoResiduosPeligrososEspeciales)
        {
            if (
                !pManejoResiduosPeligrososEspeciales.EstanClasificados.HasValue
                || !pManejoResiduosPeligrososEspeciales.RequiereObservacion.HasValue
                || pManejoResiduosPeligrososEspeciales.RequiereObservacion.HasValue && (bool)pManejoResiduosPeligrososEspeciales.RequiereObservacion && string.IsNullOrEmpty(pManejoResiduosPeligrososEspeciales.Observacion)
               )
            {
                return false;
            }
            return true;
        }

        private bool ValidarRegistroCompletoManejoOtro(ManejoOtro pManejoOtro)
        {

            if (
                !pManejoOtro.FechaActividad.HasValue
                || string.IsNullOrEmpty(pManejoOtro.Actividad)
                || string.IsNullOrEmpty(pManejoOtro.UrlSoporteGestion)
                )
            {
                return false;
            }

            return true;
        }

        #endregion

        #endregion
    }
}
