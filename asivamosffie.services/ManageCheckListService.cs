using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Helpers.Constant;
using Z.EntityFramework.Plus;
using System.Text.RegularExpressions;


namespace asivamosffie.services
{
    public class ManageCheckListService : IManageCheckListService
    {
        #region Construcctor
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public ManageCheckListService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }
        #endregion

        #region Get

        public async Task<bool> GetValidateExistNameCheckList(ListaChequeo pListaChequeo)
        {  
            if (await _context.ListaChequeo
                .AnyAsync(lc => lc.Eliminado != true
                && lc.Nombre.Trim().ToLower().Equals(pListaChequeo.Nombre.Trim().ToLower())))
                return true;
            return false;
        }

        public async Task<List<ListaChequeoItem>> GetListItem()
        {
            return await _context.ListaChequeoItem
                .Where(l => l.Eliminado != true)
                .OrderByDescending(o => o.ListaChequeoItemId)
                .ToListAsync();
        }

        public async Task<List<ListaChequeo>> GetCheckList()
        {
            return await _context.ListaChequeo
                .Where(c => c.Eliminado != true)
                .IncludeFilter(lclci => lclci.ListaChequeoListaChequeoItem.Where(lc => lc.Eliminado != null))
                .OrderByDescending(o => o.ListaChequeoId)
                .ToListAsync();
        }

        public async Task<ListaChequeoItem> GetListaChequeoItemByListaChequeoItemId(int ListaChequeoItemId)
        {
            return await _context.ListaChequeoItem.FindAsync(ListaChequeoItemId);
        }

        public async Task<ListaChequeo> GetListaChequeoItemByListaChequeoId(int ListaChequeoId)
        {
            return await _context.ListaChequeo
                    .Where(r => r.ListaChequeoId == ListaChequeoId)
                    .Include(lci => lci.ListaChequeoListaChequeoItem)
                    .FirstOrDefaultAsync();
        }
        #endregion 

        #region Create Edit Business
        public async Task<Respuesta> DeleteListaChequeoItem(int pListaChequeoListaChequeoItemId, string pAutor)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Elemento_Lista_Chequeo, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                await _context.Set<ListaChequeoListaChequeoItem>()
                              .Where(l => l.ListaChequeoListaChequeoItemId == pListaChequeoListaChequeoItemId)
                              .UpdateAsync(l => new ListaChequeoListaChequeoItem
                              {
                                  UsuarioModificacion = pAutor,
                                  FechaModificacion = DateTime.Now,
                                  Eliminado = true
                              });

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Lista_Chequeo, GeneralCodes.OperacionExitosa, idAccion, pAutor, "ELIMINAR ITEM DE LISTA DE CHEQUEO")
                };
            }
            catch (Exception e)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = GeneralCodes.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Lista_Chequeo, GeneralCodes.Error, idAccion, pAutor, e.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> ActivateDeactivateListaChequeo(ListaChequeo pListaChequeo)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Activar_Desactivar_Lista_Chequeo, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                await _context.Set<ListaChequeo>()
                              .Where(l => l.ListaChequeoId == pListaChequeo.ListaChequeoId)
                              .UpdateAsync(l => new ListaChequeo
                              {
                                  Activo = pListaChequeo.Activo,
                                  FechaModificacion = DateTime.Now,
                                  UsuarioModificacion = pListaChequeo.UsuarioCreacion
                              });

                //Enviar Correo si se desactiva la lista de chequeo
                if (pListaChequeo.Activo == false)
                    await SendEmailWhenDesactiveListaChequeo(pListaChequeo.ListaChequeoId);

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Lista_Chequeo, GeneralCodes.OperacionExitosa, idAccion, pListaChequeo.UsuarioCreacion, "ACTIVAR DESACTIVAR LISTA CHEQUEO")
                };
            }
            catch (Exception e)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = GeneralCodes.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Lista_Chequeo, GeneralCodes.Error, idAccion, pListaChequeo.UsuarioCreacion, e.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> ActivateDeactivateListaChequeoItem(ListaChequeoItem pListaChequeoItem)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Activar_Desactivar_Item_Lista_Chequeo, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ListaChequeoItem ListaChequeoItem = _context.ListaChequeoItem.Where(r => r.ListaChequeoItemId == pListaChequeoItem.ListaChequeoItemId)
                    .Include(r => r.ListaChequeoListaChequeoItem)
                       .ThenInclude(l => l.ListaChequeo)
                                .FirstOrDefault();

                if (ListaChequeoItem.ListaChequeoListaChequeoItem
                    .Any(l => l.ListaChequeo.EstadoCodigo == ConstanCodigoEstadoListaChequeo.Activo_En_proceso
                          || l.ListaChequeo.EstadoCodigo == ConstanCodigoEstadoListaChequeo.Activo_Terminado))
                {
                    return new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.InformacionDependiente,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Lista_Chequeo, GeneralCodes.InformacionDependiente, idAccion, pListaChequeoItem.UsuarioCreacion, "El registro del banco de requerimientos solo se puede desactivar si no esta asociada a una lista de chequeo activa.".ToUpper())
                    };
                }

                await _context.Set<ListaChequeoItem>()
                           .Where(l => l.ListaChequeoItemId == pListaChequeoItem.ListaChequeoItemId)
                               .UpdateAsync(l => new ListaChequeoItem
                               {
                                   Activo = pListaChequeoItem.Activo,
                                   FechaModificacion = DateTime.Now,
                                   UsuarioModificacion = pListaChequeoItem.UsuarioCreacion
                               });

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Lista_Chequeo, GeneralCodes.OperacionExitosa, idAccion, pListaChequeoItem.UsuarioCreacion, "ACTIVAR DESACTIVAR LISTA CHEQUEO ITEM")
                };
            }
            catch (Exception e)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = GeneralCodes.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Lista_Chequeo, GeneralCodes.Error, idAccion, pListaChequeoItem.UsuarioCreacion, e.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> CreateEditItem(ListaChequeoItem pListaChequeoItem)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Create_Edit_Item_Lista_Chequeo, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //Validar si el ya existe Nombre
                if (_context.ListaChequeoItem.Where(lc => lc.Nombre.Trim().ToUpper() == pListaChequeoItem.Nombre.Trim().ToUpper()).Count() > 0)
                {
                    return new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.RegistroDuplicado,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Lista_Chequeo, GeneralCodes.RegistroDuplicado, idAccion, pListaChequeoItem.UsuarioCreacion, "REGISTRO DUPLICADO")
                    };
                }

                if (pListaChequeoItem.ListaChequeoItemId == 0)
                {
                    await _context.ListaChequeoItem.AddAsync(pListaChequeoItem);
                }
                else
                {
                    await _context.Set<ListaChequeoItem>()
                              .Where(l => l.ListaChequeoItemId == pListaChequeoItem.ListaChequeoItemId)
                              .UpdateAsync(l => new ListaChequeoItem
                              {
                                  Activo = pListaChequeoItem.Activo,
                                  Nombre = pListaChequeoItem.Nombre,
                                  FechaModificacion = DateTime.Now,
                                  UsuarioModificacion = pListaChequeoItem.UsuarioCreacion
                              });
                }
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Lista_Chequeo, GeneralCodes.OperacionExitosa, idAccion, pListaChequeoItem.UsuarioCreacion, "CREAR EDITAR ITEM LISTA CHEQUEO")
                };
            }
            catch (Exception e)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = GeneralCodes.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Lista_Chequeo, GeneralCodes.Error, idAccion, pListaChequeoItem.UsuarioCreacion, e.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> CreateEditCheckList(ListaChequeo pListaChequeo)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Create_Edit_Item_Lista_Chequeo, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pListaChequeo.ListaChequeoId == 0)
                {
                    pListaChequeo.FechaCreacion = DateTime.Now;
                    pListaChequeo.Eliminado = false;
                    await _context.ListaChequeo.AddAsync(pListaChequeo);

                    CreateEditListaChequeoListaChequeoItem(pListaChequeo);
                }
                else
                {
                    await _context.Set<ListaChequeo>()
                            .Where(l => l.ListaChequeoId == pListaChequeo.ListaChequeoId)
                            .UpdateAsync(l => new ListaChequeo
                            {
                                Nombre = pListaChequeo.Nombre,
                                EstadoCodigo = pListaChequeo.EstadoCodigo,
                                FechaModificacion = DateTime.Now,
                                UsuarioModificacion = pListaChequeo.UsuarioCreacion
                            });
                    CreateEditListaChequeoListaChequeoItem(pListaChequeo);
                }
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Lista_Chequeo, GeneralCodes.OperacionExitosa, idAccion, pListaChequeo.UsuarioCreacion, "CREAR EDITAR LISTA CHEQUEO")
                };
            }
            catch (Exception e)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = GeneralCodes.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Lista_Chequeo, GeneralCodes.Error, idAccion, pListaChequeo.UsuarioCreacion, e.InnerException.ToString())
                };
            }
        }

        private void CreateEditListaChequeoListaChequeoItem(ListaChequeo pListaChequeo)
        {
            pListaChequeo.ListaChequeoListaChequeoItem
                .ToList().ForEach(lclci =>
                        {
                            if (lclci.ListaChequeoListaChequeoItemId == 0)
                            {
                                lclci.UsuarioCreacion = pListaChequeo.UsuarioCreacion;
                                lclci.FechaCreacion = DateTime.Now;
                                lclci.Eliminado = false;
                                _context.ListaChequeoListaChequeoItem.Add(lclci);
                            }
                            else
                            {
                                _context.Set<ListaChequeoListaChequeoItem>()
                                        .Where(l => l.ListaChequeoListaChequeoItemId == lclci.ListaChequeoListaChequeoItemId)
                                        .Update(lc => new ListaChequeoListaChequeoItem()
                                        {
                                            FechaModificacion = DateTime.Now,
                                            Orden = lclci.Orden,
                                            UsuarioModificacion = pListaChequeo.UsuarioCreacion,
                                            Eliminado = lclci.Eliminado
                                        });
                            }
                        });
        }
        #endregion

        #region  Emails 
        public async Task<bool> SendEmailWhenDesactiveListaChequeo(int pListaChequeoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.ListaChequeoDesactivada));
            template.Contenido = ReplaceVariablesListaChequeo(template.Contenido, pListaChequeoId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Tecnica
                                          };

            return _commonService.EnviarCorreo(perfilsEnviarCorreo, template);
        }

        private string ReplaceVariablesListaChequeo(string template, int pListaChequeoId)
        {
            ListaChequeo listaChequeo = _context.ListaChequeo.Find(pListaChequeoId);

            template = template
                      .Replace("[NOMBRE_LISTA_CHEQUEO]", listaChequeo.Nombre);
            return template;
        }

        #endregion 
    }
}
