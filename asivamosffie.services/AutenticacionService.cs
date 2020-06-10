using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace asivamosffie.services
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly devAsiVamosFFIEContext _context;
        public AutenticacionService(devAsiVamosFFIEContext context)
        {
            _context = context;            
        }
        public async Task<Usuario>  IniciarSesion(string pMail, string pContrasena)
        {
            Usuario usuario = _context.Usuario.Where(u => u.Email == pMail 
                                                    && u.Contrasena == pContrasena).Single();
            
            return usuario;
        }
    }
}
