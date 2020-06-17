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

namespace asivamosffie.services
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly devAsiVamosFFIEContext _context;
        public AutenticacionService(devAsiVamosFFIEContext context)
        {
            _context = context;            
        }

        public async Task<Object> IniciarSesion(Usuario pUsuario, string prmSecret, string prmIssuer, string prmAudience)
        {
            Object mensaje = null;
            Task<Usuario> result = this.GetUserByMail(pUsuario.Email);

                Usuario usuario = await result;
                
                // Usuario no existe
                if (usuario == null)
                {
                    mensaje = new { codigo = "OK", validation = true, validationmessage = "El usuario no existe en el sistema. Contacte al administrador." };
                }else if (!(usuario.Activo.HasValue?usuario.Activo.Value:true))
                {
                    mensaje = new { codigo = "OK", validation = true, validationmessage = "El usuario se encuentra inactivo. Contacte al administrador." };
                }else if (usuario.IntentosFallidos >= 3 || usuario.Bloqueado.Value)
                {
                    this.BlockUser(usuario.UsuarioId);
                    mensaje = new { codigo = "OK", validation = true, validationmessage = "El usuario se encuentra bloqueado, debe remitirse a la opción “Recordar Contraseña”"};
                }else if (usuario.Contrasena != pUsuario.Contrasena)
                {
                    this.AddFailedAttempt(usuario.UsuarioId);
                    mensaje = new { codigo = "OK", validation = true,validationmessage = "La contraseña es incorrecta." };
                }else if (usuario.FechaUltimoIngreso == null)
                {
                    mensaje = new { codigo = "PV", validation = true, validationmessage = "PrimeraVez",token=this.GenerateToken(prmSecret, prmIssuer, prmAudience,usuario) };
                }else
                {
                    this.ResetFailedAttempts(usuario.UsuarioId);
                    mensaje = new { codigo = "OK", validation = false, data = usuario, token = this.GenerateToken(prmSecret, prmIssuer, prmAudience, usuario) };
                }


                return mensaje;
        }

        private JwtToken GenerateToken(string prmSecret, string prmIssuer, string prmAudience, Usuario prmUser)
        {
            var token = new JwtTokenBuilder()
            .AddSecurityKey(JwtSecurityKey.Create(prmSecret))
            .AddIssuer(prmIssuer)
            .AddAudience(prmAudience)
            .AddExpiry(1)
            //.AddClaim("Name", result.Primernombre+" "+result.Primerapellido)
            .AddClaim("User", prmUser.Email)
            .AddClaim("UserId", prmUser.UsuarioId.ToString())
            //.AddClaim("Title", result.Cargo)
            //.AddRole(result.IdrolNavigation.Nombre)
            .Build();
            return token;
        }
        public async Task<Usuario>  GetUserByMail(string pMail)
        {
            try {
                   Usuario usuario = await _context.Usuario.Where(u => u.Email == pMail  
                                                    && u.Eliminado.Value == false).SingleOrDefaultAsync();
                    return usuario;

            }catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task ResetFailedAttempts(int pUserId)
        {
            Usuario usuario = _context.Usuario.Where(u => u.UsuarioId == pUserId).SingleOrDefault();

            usuario.IntentosFallidos = 0;

            _context.SaveChanges();
        }

        public async Task AddFailedAttempt(int pUserId)
        {
            try
            {
                Usuario usuario = _context.Usuario.Where(u => u.UsuarioId == pUserId).SingleOrDefault();

                usuario.IntentosFallidos ++;

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
