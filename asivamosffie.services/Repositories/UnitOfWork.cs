using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly IRepository<Usuario> _userRepository;

        public UnitOfWork(devAsiVamosFFIEContext context)
        {
            _context = context;
        }

        public IRepository<Usuario> UserRepository => _userRepository ?? new BaseRepository<Usuario>(_context);

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }


      
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


    }
}
