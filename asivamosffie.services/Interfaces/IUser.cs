using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;

namespace asivamosffie.services.Interfaces
{
    public interface IUser
    {
        Task<Usuario> RecoverPasswordByEmailAsync(string userMail ,string ipClient);

    }
}
