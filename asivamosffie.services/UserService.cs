﻿using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
using asivamosffie.model.Models;
namespace asivamosffie.services
{
    public class UserService : IUser
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public UserService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;

            _context = context;
        }

        #region Loggin
        public async Task<Respuesta> RecoverPasswordByEmailAsync(Usuario pUsuario, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            bool blEnvioCorreo = false;
            Respuesta respuesta = new Respuesta();

            //Si no llega Email
            if (string.IsNullOrEmpty(pUsuario.Email))
            {
                respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesUsuarios.EmailObligatorio };
            }
            try
            {
                Usuario usuarioSolicito = _context.Usuario.Where(r => !(bool)r.Eliminado && r.Email.ToUpper().Equals(pUsuario.Email.ToUpper())).FirstOrDefault();

                if (usuarioSolicito != null)
                {
                    if (usuarioSolicito.Activo == false)
                    {
                        respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesUsuarios.UsuarioInactivo };
                    }
                    else
                    {
                        string newPass = Helpers.Helpers.GeneratePassword(true, true, true, true, false, 8);
                        usuarioSolicito.Contrasena = Helpers.Helpers.encryptSha1(newPass.ToString());
                        usuarioSolicito.CambiarContrasena = true;
                        usuarioSolicito.Bloqueado = false;
                        usuarioSolicito.IntentosFallidos = 0;
                        usuarioSolicito.FechaCambioPassword = DateTime.Now;
                        usuarioSolicito.Ip = pUsuario.Ip;
                        //Guardar Usuario
                        await UpdateUser(usuarioSolicito);
                        Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.RecuperarClave);
                        string template = TemplateRecoveryPassword.Contenido;

                        string urlDestino = pDominio;
                        //asent/img/logo
                        //template = template.Replace("_Link_", urlDestino);
                        template = template.Replace("_LinkF_", pDominioFront);
                        template = template.Replace("_Email_", usuarioSolicito.Email);
                        template = template.Replace("_Password_", newPass);

                        blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioSolicito.Email, "Recuperar contraseña", template, pSentender, pPassword, pMailServer, pMailPort);

                        if (blEnvioCorreo)
                            respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesUsuarios.ContrasenaGenerada };
                        else
                            respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesUsuarios.ErrorEnviarCorreo };

                    }
                }
                else
                {
                    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesUsuarios.CorreoNoExiste };

                }
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Usuario, respuesta.Code, (int)enumeratorAccion.SolicitarContraseña, pUsuario.Email, "Recuperar contraseña");
                return respuesta;


            }
            catch (Exception ex)
            {

                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesUsuarios.ErrorGuardarCambios };
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Usuario, respuesta.Code, (int)enumeratorAccion.SolicitarContraseña, pUsuario.Email, "Recuperar contraseña") + ": " + ex.ToString() + ex.InnerException;
                return respuesta;
            }

        }

        public async Task<Usuario> UpdateUser(Usuario pUser)
        {
            pUser.FechaModificacion = DateTime.Now;
            pUser.UsuarioModificacion = pUser.Email;
            await _context.SaveChangesAsync();
            return pUser;
        }

        /**
         *  Método para cambiar la contraseña del usuario
         *  @param pidusuario int id del usuario a actualizar
         *  @param Oldpwd string contraseña anterior encriptada en mayusculas
         *  @param Newpwd string nueva contraseña encriptada en mayusculas
         *  @return Respuesta objeto de respuesta
         * **/

        public async Task<Respuesta> ChangePasswordUser(int pidusuario, string Oldpwd, string Newpwd)
        {
            var user = _context.Usuario.Find(pidusuario);
            Respuesta respuesta = new Respuesta();
            //var UppUser = Helpers.Helpers.ConvertToUpercase(user);
            //var OldpwdEncrypt = Helpers.Helpers.encryptSha1(Newpwd.ToUpper());
            var OldpwdEncrypt = Oldpwd.ToUpper();
            try
            {
                if (user != null)
                {
                    if (user.Contrasena.ToUpper() != OldpwdEncrypt.ToString())
                    {
                        respuesta = new Respuesta()
                        {
                            IsSuccessful = false,
                            IsValidation = false,
                            Code = ConstantMessagesContrasena.ErrorContrasenaAntigua
                        };
                    }
                    else
                    {
                        //user.Contrasena = Helpers.Helpers.encryptSha1(Newpwd.ToUpper());
                        if (user.FechaUltimoIngreso == null)
                        {
                            user.FechaUltimoIngreso = DateTime.Now;
                        }
                        user.Contrasena = Newpwd.ToUpper();
                        user.FechaModificacion = DateTime.Now;
                        user.UsuarioModificacion = user.Email;
                        user.CambiarContrasena = false;
                        await _context.SaveChangesAsync();
                        respuesta = new Respuesta()
                        {
                            IsSuccessful = true,
                            IsValidation = true,
                            Data = user,
                            Code = ConstantMessagesContrasena.OperacionExitosa
                        };
                    }
                }
                else
                {
                    respuesta = new Respuesta()
                    {
                        IsSuccessful = false,
                        IsValidation = false,
                        Code = ConstantMessagesContrasena.ErrorSesion
                    };
                }
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CambioContrasena, respuesta.Code, (int)enumeratorAccion.CambiarContraseña, user.Email, "Cambiar contraseña");
            }
            catch (Exception ex)
            {
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CambioContrasena, respuesta.Code, (int)enumeratorAccion.CambiarContraseña, user.Email, "Cambiar contraseña") + ": " + ex.ToString() + ex.InnerException;
            }
            return respuesta;
        }

        public async Task<Respuesta> ValidateCurrentPassword(int pUserid, string pOldpwd)
        {
            var user = _context.Usuario.Find(pUserid);
            Respuesta respuesta = new Respuesta();
            var OldpwdEncrypt = pOldpwd.ToUpper();
            try
            {
                if (user != null)
                {
                    if (user.Contrasena.ToUpper() != OldpwdEncrypt.ToString())
                    {
                        respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesContrasena.ErrorContrasenaAntigua };
                    }
                    else
                    {
                        respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Data = user, Code = ConstantMessagesContrasena.OperacionExitosa };
                    }
                }
                else
                {
                    respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesContrasena.ErrorSesion };
                }
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Usuario, respuesta.Code, (int)enumeratorAccion.CambiarContraseña, user.Email, "Validación de contraseña");
            }
            catch (Exception ex)
            {
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CambioContrasena, respuesta.Code, (int)enumeratorAccion.CambiarContraseña, user.Email, "Validación de contraseña") + ": " + ex.ToString() + ex.InnerException;
            }
            return respuesta;
        }

        public async Task<Respuesta> CloseSesion(int v)
        {
            var usuario = _context.Usuario.Find(v);
            var respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesUsuarios.OperacionExitosa };
            respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Usuario, respuesta.Code, (int)enumeratorAccion.IniciarSesion, usuario.Email, "Cerrar sesión");
            return respuesta;
        }

        #endregion

        #region Create Edit List

        public async Task<dynamic> GetContratoByTipo(bool EsObra)
        {
            //Envia Interventor
            if (EsObra)
            {

                return await _context.Contrato
                    .Include(c => c.Contratacion)
                    .Where(r => r.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Obra)
                    .Select(c => new
                    {
                        c.ContratoId,
                        c.NumeroContrato
                    })
                    .ToListAsync();
            }
            else
            {
                return await _context.Contrato
                                  .Include(c => c.Contratacion)
                                  .Select(c => new
                                  {
                                      c.ContratoId,
                                      c.NumeroContrato
                                  })
                                  .ToListAsync();
            }
        }

        public async Task<bool> ValidateExistEmail(Usuario pUsuario)
        {
            if (await _context.Usuario.AnyAsync(u => u.Email.ToLower() == pUsuario.Email.ToLower()))
                return false;

            return true;
        }

        public async Task<List<VUsuariosRoles>> GetListUsuario()
        {
            return await _context.VUsuariosRoles.OrderByDescending(ur => ur.UsuarioId).ToListAsync();
        }

        public async Task<Usuario> GetUsuario(int pUsuarioId)
        {
            try
            {
                Usuario usuario = await _context.Usuario.FindAsync(pUsuarioId);
                usuario.Perfil = _context.UsuarioPerfil
                    .Where(u => u.UsuarioId == pUsuarioId)
                    .Include(p => p.Perfil).Select(p => p.Perfil)
                    .FirstOrDefault();

                List<ContratoAsignado> contratoAsignadosInterventor =
                  _context.Contrato.Where(r => r.InterventorId == pUsuarioId)
                                   .Select(c => new ContratoAsignado
                                   {
                                       ContratoId = c.ContratoId,
                                       NumeroContrato = c.NumeroContrato,
                                       TipoAsignacionCodigo = ConstantCodigoTipoAsignacionContrato.Interventor
                                   }).ToList();

                List<ContratoAsignado> contratoAsignadosApoyo =
                    _context.Contrato.Where(r => r.ApoyoId == pUsuarioId)
                                     .Select(c => new ContratoAsignado
                                     {
                                         ContratoId = c.ContratoId,
                                         NumeroContrato = c.NumeroContrato,
                                         TipoAsignacionCodigo = ConstantCodigoTipoAsignacionContrato.Apoyo
                                     }).ToList();


                List<ContratoAsignado> contratoAsignadosSupervisor =
                    _context.Contrato.Where(r => r.SupervisorId == pUsuarioId)
                                     .Select(c => new ContratoAsignado
                                     {
                                         ContratoId = c.ContratoId,
                                         NumeroContrato = c.NumeroContrato,
                                         TipoAsignacionCodigo = ConstantCodigoTipoAsignacionContrato.Supervisor
                                     }).ToList();

                usuario.ContratosAsignados = new List<ContratoAsignado>();

                if (contratoAsignadosInterventor.Count() > 0)
                    usuario.ContratosAsignados.AddRange(contratoAsignadosInterventor);

                if (contratoAsignadosApoyo.Count() > 0)
                    usuario.ContratosAsignados.AddRange(contratoAsignadosApoyo);

                if (contratoAsignadosSupervisor.Count() > 0)
                    usuario.ContratosAsignados.AddRange(contratoAsignadosSupervisor);

                return usuario;
            }
            catch (Exception e)
            {
                return new Usuario();
            }
        }

        public async Task<Respuesta> CreateEditUsuario(Usuario pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Activar_Desactivar_Rol, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pUsuario.UsuarioId == 0)
                {
                    string strPassWordGenerate = Helpers.Helpers.GeneratePassword(true, true, true, true, false, 20);
                    pUsuario.Activo = true;
                    pUsuario.Contrasena = Helpers.Helpers.encryptSha1(strPassWordGenerate);
                    pUsuario.Eliminado = false;

                    _context.Usuario.Add(pUsuario);
                    _context.SaveChanges();

                    CreateEditAsignacionContrato(pUsuario);
                    await CrearEditarUsuarioPerfil(pUsuario, true);
                    await SendEmailWhenCreateUsuario(pUsuario, strPassWordGenerate);
                }
                else
                {
                    _context.Set<Usuario>()
                            .Where(u => u.UsuarioId == pUsuario.UsuarioId)
                            .Update(u => new Usuario
                            {
                                FechaModificacion = DateTime.Now,
                                UsuarioModificacion = pUsuario.UsuarioCreacion,
                                FechaCreacion = pUsuario.FechaCreacion,
                                PrimerNombre = pUsuario.PrimerNombre,
                                SegundoNombre = pUsuario.SegundoNombre,
                                PrimerApellido = pUsuario.PrimerApellido,
                                SegundoApellido = pUsuario.SegundoApellido,
                                TipoDocumentoCodigo = pUsuario.TipoDocumentoCodigo,
                                NumeroIdentificacion = pUsuario.NumeroIdentificacion,
                                Email = pUsuario.Email,
                                TelefonoFijo = pUsuario.TelefonoFijo,
                                TelefonoCelular = pUsuario.TelefonoCelular,
                                MunicipioId = pUsuario.MunicipioId,
                                FechaExpiracion = pUsuario.FechaExpiracion,
                                UrlSoporteDocumentacion = pUsuario.UrlSoporteDocumentacion,
                                Observaciones = pUsuario.Observaciones,
                                DependenciaCodigo = pUsuario.DependenciaCodigo,
                                GrupoCodigo = pUsuario.GrupoCodigo,
                                ProcedenciaCodigo = pUsuario.ProcedenciaCodigo,
                                TipoAsignacionCodigo = pUsuario.TipoAsignacionCodigo,
                                TieneContratoAsignado = pUsuario.TieneContratoAsignado
                            });

                    CreateEditAsignacionContrato(pUsuario);
                    await CrearEditarUsuarioPerfil(pUsuario, false);
                }
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_usuarios, GeneralCodes.OperacionExitosa, idAccion, pUsuario.UsuarioCreacion, "CREAR EDITAR USUARIO")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_usuarios, GeneralCodes.Error, idAccion, pUsuario.UsuarioCreacion, e.InnerException.ToString())
                };
            }
        }

        private void CreateEditAsignacionContrato(Usuario pUsuario)
        {
            if (pUsuario.ContratosAsignados != null)
            {
                foreach (var ContratosAsignados in pUsuario.ContratosAsignados)
                {
                    switch (pUsuario.TipoAsignacionCodigo)
                    {
                        case ConstantCodigoTipoAsignacionContrato.Interventor:
                            _context.Set<Contrato>()
                                    .Where(c => c.ContratoId == ContratosAsignados.ContratoId)
                                    .Update(c => new Contrato
                                    {
                                        InterventorId = pUsuario.UsuarioId,
                                        FechaModificacion = DateTime.Now,
                                        UsuarioModificacion = pUsuario.UsuarioCreacion
                                    });
                            break;

                        case ConstantCodigoTipoAsignacionContrato.Apoyo:
                            _context.Set<Contrato>()
                                  .Where(c => c.ContratoId == ContratosAsignados.ContratoId)
                                  .Update(c => new Contrato
                                  {
                                      ApoyoId = pUsuario.UsuarioId,
                                      FechaModificacion = DateTime.Now,
                                      UsuarioModificacion = pUsuario.UsuarioCreacion
                                  });
                            break;
                        case ConstantCodigoTipoAsignacionContrato.Supervisor:
                            _context.Set<Contrato>()
                                  .Where(c => c.ContratoId == ContratosAsignados.ContratoId)
                                  .Update(c => new Contrato
                                  {
                                      SupervisorId = pUsuario.UsuarioId,
                                      FechaModificacion = DateTime.Now,
                                      UsuarioModificacion = pUsuario.UsuarioCreacion
                                  });
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private async Task CrearEditarUsuarioPerfil(Usuario pUsuario, bool Create)
        {
            try
            {
                if (Create)
                {
                    UsuarioPerfil usuarioPerfil = new UsuarioPerfil
                    {
                        UsuarioId = pUsuario.UsuarioId,
                        PerfilId = pUsuario.PerfilId,
                        Activo = true,
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = pUsuario.UsuarioCreacion
                    };
                    await _context.UsuarioPerfil.AddAsync(usuarioPerfil);
                    _context.SaveChanges();
                }
                else
                {
                    _context.Set<UsuarioPerfil>()
                         .Where(up => up.UsuarioId == pUsuario.UsuarioId)
                         .Update(up => new UsuarioPerfil
                         {
                             PerfilId = pUsuario.PerfilId,
                             FechaModificacion = DateTime.Now,
                             UsuarioModificacion = pUsuario.UsuarioCreacion 
                         });
                }
            }

            catch (Exception e)
            {

            }
        }

        public async Task<bool> SendEmailWhenCreateUsuario(Usuario pUsuario, string pPassWord)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.CreateUserEmail_6_2));
            template.Contenido = ReplaceVariablesUsuarios(template.Contenido, pUsuario, pPassWord);

            List<string> ListString = new List<string>
                 {
                     pUsuario.Email
                 };

            return _commonService.EnviarCorreo(ListString, template);
        }

        private string ReplaceVariablesUsuarios(string template, Usuario pUsuario, string pPassWord)
        {
            template = template
                      .Replace("[EMAIL]", pUsuario.Email)
                      .Replace("[PASSWORD]", pPassWord);

            return template;
        }

        public async Task<dynamic> GetListPerfil()
        {
            return await _context.Perfil
                .Where(p => p.Eliminado != true)
                .OrderByDescending(p => p.PerfilId)
                .Select(p => new
                {
                    p.PerfilId,
                    p.Nombre
                }).ToListAsync();
        }
        #endregion
    }
}
