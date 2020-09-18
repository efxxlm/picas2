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
using Newtonsoft.Json;

namespace asivamosffie.services
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;

        public AutenticacionService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<Respuesta> IniciarSesion(Usuario pUsuario, string prmSecret, string prmIssuer, string prmAudience)
        {
            Respuesta respuesta = new Respuesta();

            try
            {

                Task<Usuario> result = this.GetUserByMail(pUsuario.Email);
                Usuario usuario = await result;
                List<UsuarioPerfil> perfiles = await _context.UsuarioPerfil.Where(y => y.UsuarioId == usuario.UsuarioId).Include(y=>y.Perfil).ToListAsync();

               

                // User doesn't exist
                if (usuario == null)
                {
                    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesUsuarios.UsuarioNoExiste };
                }
                else if (!(usuario.Activo.HasValue ? usuario.Activo.Value : true)) // User inactive
                {
                    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesUsuarios.UsuarioInactivo };
                }
                else if (usuario.IntentosFallidos >= 3 || usuario.Bloqueado.Value) // Failed Attempt or User blocked
                {
                    this.BlockUser(usuario.UsuarioId);
                    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesUsuarios.UsuarioBloqueado };
                }
                else if (usuario.Contrasena != pUsuario.Contrasena) // incorrect password
                {
                    this.AddFailedAttempt(usuario.UsuarioId);
                    respuesta = new Respuesta { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesUsuarios.ContrasenaIncorrecta };
                }
                else if (usuario.FechaUltimoIngreso == null || usuario.CambiarContrasena.Value) // first time to log in
                {                                   
                    respuesta = new Respuesta { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesUsuarios.DirecCambioContrasena, Data = new { datausuario=usuario, dataperfiles=perfiles }, Token = this.GenerateToken(prmSecret, prmIssuer, prmAudience, usuario, perfiles) };                    
                }
                else // successful
                {
                    this.ResetFailedAttempts(usuario.UsuarioId);
                    respuesta = new Respuesta { IsSuccessful = true, IsValidation = false, Code = ConstantMessagesUsuarios.OperacionExitosa, Data = new { datausuario = usuario, dataperfiles = perfiles }, Token = this.GenerateToken(prmSecret, prmIssuer, prmAudience, usuario, perfiles) };
                  
                }

                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Usuario, respuesta.Code, (int)enumeratorAccion.IniciarSesion, pUsuario.Email, "Inicio de sesión");    

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        private JwtToken GenerateToken(string prmSecret, string prmIssuer, string prmAudience, Usuario prmUser, List<UsuarioPerfil> prmPerfiles)
        {
            var token = new JwtTokenBuilder()
            .AddSecurityKey(JwtSecurityKey.Create(prmSecret))
            .AddIssuer(prmIssuer)
            .AddAudience(prmAudience)
            .AddExpiryinMinute(15)
            //.AddClaim("Name", result.Primernombre+" "+result.Primerapellido)
            .AddClaim("User", prmUser.Email)
            .AddClaim("UserId", prmUser.UsuarioId.ToString())
            .AddClaim("Rol", JsonConvert.SerializeObject(prmPerfiles))
            //.AddRole(result.IdrolNavigation.Nombre)
            .Build();
            return token;
        }
        public async Task<Usuario> GetUserByMail(string pMail)
        {
            try
            {
                Usuario usuario = await _context.Usuario.Where(u => u.Email == pMail
                                                 && u.Eliminado.Value == false).SingleOrDefaultAsync();              
                return usuario;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task ResetFailedAttempts(int pUserId)
        {
            try
            {
                Usuario usuario = _context.Usuario.Where(u => u.UsuarioId == pUserId).SingleOrDefault();

                usuario.IntentosFallidos = 0;
                usuario.FechaUltimoIngreso = DateTime.Now;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddFailedAttempt(int pUserId)
        {
            try
            {
                Usuario usuario = _context.Usuario.Where(u => u.UsuarioId == pUserId).SingleOrDefault();

                usuario.IntentosFallidos++;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task BlockUser(int pUserId)
        {
            try
            {
                Usuario usuario = _context.Usuario.Where(u => u.UsuarioId == pUserId).SingleOrDefault();

                usuario.Bloqueado = true;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
