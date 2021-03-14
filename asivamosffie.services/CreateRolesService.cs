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

    }
}
