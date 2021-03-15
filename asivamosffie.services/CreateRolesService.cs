using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.api;
using asivamosffie.services.Helpers.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class CreateRolesService : ICreateRolesService
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;
        public readonly IConverter _converter;

        public CreateRolesService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        public async Task<bool> ValidateExistNamePerfil(string pNamePerfil)
        {
            pNamePerfil = pNamePerfil.Replace("%", "");
            bool blExisteNombre = false;
            if (await _context.Perfil.AnyAsync(p => p.Nombre.Trim().ToLower() == pNamePerfil.Trim().ToLower()))
                blExisteNombre = true;

            return blExisteNombre;
        }

        public async Task<dynamic> GetMenu()
        {
            return await _context.Menu.Select(m =>
                                                new
                                                {
                                                    m.FaseCodigo,
                                                    m.MenuId,
                                                    m.Nombre
                                                }
                                              ).ToListAsync();
        }

        public async Task<Perfil> GetPerfilByPerfilId(int pPerfilId)
        {
            return await
                _context.Perfil
                .Where(p => p.PerfilId == pPerfilId)
                .Include(mp => mp.MenuPerfil).FirstOrDefaultAsync();

        }

        public async Task<Respuesta> CreateEditRolesPermisos(Perfil pPerfil)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Create_Edit_Menu_Perfil, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pPerfil.PerfilId == 0)
                {
                    pPerfil.FechaCreacion = DateTime.Now;
                    pPerfil.Eliminado = false;
                    _context.Perfil.Add(pPerfil);
                }
                else
                {
                    _context.Set<Perfil>()
                             .Where(p => p.PerfilId == pPerfil.PerfilId)
                             .Update(p => new Perfil
                             {
                                 Nombre = pPerfil.Nombre,
                                 FechaModificacion = DateTime.Now,
                                 UsuarioModificacion = pPerfil.UsuarioCreacion
                             });
                }
                foreach (var MenuPerfil in pPerfil.MenuPerfil)
                {

                    if (MenuPerfil.MenuPerfilId == 0)
                    {
                        MenuPerfil.UsuarioCreacion = pPerfil.UsuarioCreacion;
                        MenuPerfil.FechaCreacion = DateTime.Now;
                        MenuPerfil.Activo = true;
                        _context.MenuPerfil.Add(MenuPerfil);
                    }
                    else
                    {
                        _context.Set<MenuPerfil>()
                              .Where(p => p.MenuPerfilId == MenuPerfil.MenuPerfilId)
                              .Update(p => new MenuPerfil
                              {
                                  TienePermisoCrear = MenuPerfil.TienePermisoCrear,
                                  TienePermisoEliminar = MenuPerfil.TienePermisoEliminar,
                                  TienePermisoEditar = MenuPerfil.TienePermisoEditar,
                                  TienePermisoLeer = MenuPerfil.TienePermisoLeer,
                              });
                    }

                }
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Crear_Roles, GeneralCodes.OperacionExitosa, idAccion, pPerfil.UsuarioCreacion, "CREAR EDITAR ROLES Y PERMISOS")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Crear_Roles, GeneralCodes.Error, idAccion, pPerfil.UsuarioCreacion, e.InnerException.ToString())
                };
            }


        }

        public async Task<dynamic> GetListPerfil()
        {
            return await _context.Perfil
                .Select(p => new
                {
                    p.PerfilId,
                    p.FechaCreacion,
                    p.Nombre,
                    p.Eliminado
                })
                .OrderByDescending(p => p.PerfilId)
                .ToListAsync();
        }

        public async Task<Respuesta> ActivateDeactivatePerfil(Perfil pPerfil)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Activar_Desactivar_Rol, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                List<Usuario> ListUsuario = _context.UsuarioPerfil
                                                                .Where(r => r.PerfilId == pPerfil.PerfilId)
                                                                .Include(u => u.Usuario)
                                                                .Select(u => u.Usuario)
                                                                .ToList();

                _context.Set<Perfil>()
                        .Where(p => p.PerfilId == pPerfil.PerfilId)
                        .Update(u => new Perfil
                        {
                            Eliminado = pPerfil.Eliminado,
                            UsuarioModificacion = pPerfil.UsuarioCreacion,
                            FechaModificacion = DateTime.Now
                        });

                foreach (var User in ListUsuario)
                {
                    _context.Set<Usuario>()
                        .Where(u => u.UsuarioId == User.UsuarioId)
                        .Update(u => new Usuario
                        {
                            Bloqueado = pPerfil.Eliminado,
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pPerfil.UsuarioCreacion,
                        });
                }

                await SendEmailWhenDesactivateRol(pPerfil);

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.EliminacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Crear_Roles, GeneralCodes.EliminacionExitosa, idAccion, pPerfil.UsuarioCreacion, "ACTIVAR DESACTIVAR ROL")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Crear_Roles, GeneralCodes.Error, idAccion, pPerfil.UsuarioCreacion, e.InnerException.ToString())
                };
            }


        }

        private async Task<bool> SendEmailWhenDesactivateRol(Perfil pPerfil)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.MensajeDesactivarRol_6_3));
            template.Contenido = ReplaceVariablesPerfil(template.Contenido, pPerfil.Nombre);

            List<string> ListEmails = _context.UsuarioPerfil
                .Include(u => u.Usuario)
                .Where(r => r.PerfilId == pPerfil.PerfilId)
                .Select(r => r.Usuario.Email)
                                            .ToList();

            return _commonService.EnviarCorreo(ListEmails, template);
        }

        private string ReplaceVariablesPerfil(string template, string pNombreRol)
        {
            template = template
                      .Replace("[NOMBRE_ROL]", pNombreRol);
            return template;
        }
    }
}
