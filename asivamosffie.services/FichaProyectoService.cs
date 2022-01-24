﻿using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class FichaProyectoService : IFichaProyectoService
    {
        private readonly devAsiVamosFFIEContext _context;


        public FichaProyectoService(devAsiVamosFFIEContext context)
        {
            _context = context;
        }


        public async Task<dynamic> GetProyectoIdByLlaveMen(string pLlaveMen)
        {
            List<VFichaProyectoBusquedaProyecto> ListVFichaProyectoBusquedaProyecto = await _context.VFichaProyectoBusquedaProyecto.Where(f => f.LlaveMen.ToUpper().Contains(pLlaveMen.ToUpper()))
                                                                .ToListAsync();
             
            return ListVFichaProyectoBusquedaProyecto.OrderByDescending(p => p.ProyectoId).ToList();
        }
    }
}
