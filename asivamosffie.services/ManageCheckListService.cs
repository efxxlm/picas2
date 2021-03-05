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
    public class ManageCheckListService : IManageCheckListService
    {
        private readonly devAsiVamosFFIEContext _context;

        public ManageCheckListService(devAsiVamosFFIEContext context)
        {
            _context = context;
        }

        public async Task<List<ListaChequeoItem>> GetListItem()
        {
            return await _context.ListaChequeoItem.Where(l => l.Eliminado != true).OrderByDescending(o => o.ListaChequeoItemId).ToListAsync();
        }

        public async Task<List<ListaChequeo>> GetCheckList()
        {
            return await _context.ListaChequeo.Where(c => c.Eliminado != true).OrderByDescending(o => o.ListaChequeoId).ToListAsync();
        }

        public async Task<Respuesta> CreateEditItem(ListaChequeoItem pListaChequeoItem)
        {
            if (pListaChequeoItem.ListaChequeoItemId == 0)
            {
                pListaChequeoItem.FechaCreacion = DateTime.Now;
                pListaChequeoItem.Eliminado = false;
                await _context.ListaChequeoItem.AddAsync(pListaChequeoItem);
            }
            else
            {
                await _context.Set<ListaChequeoItem>()
                          .Where(l => l.ListaChequeoItemId == pListaChequeoItem.ListaChequeoItemId)
                          .UpdateAsync(l => new ListaChequeoItem
                          {
                              Estado = pListaChequeoItem.Estado,
                              Nombre = pListaChequeoItem.Nombre,
                              FechaModificacion = DateTime.Now,
                              UsuarioModificacion = pListaChequeoItem.UsuarioCreacion
                          });

            }
            return new Respuesta();
        }

        public async Task<Respuesta> CreateEditCheckList(ListaChequeo pListaChequeo)
        {
            if (pListaChequeo.ListaChequeoId == 0)
            {
                pListaChequeo.FechaCreacion = DateTime.Now;
                pListaChequeo.Eliminado = false;
                await _context.ListaChequeo.AddAsync(pListaChequeo);
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
            }
            return new Respuesta();
        }

        public async Task<ListaChequeoItem> GetListaChequeoItemByListaChequeoItemId(int ListaChequeoItemId)
        {
              return await _context.ListaChequeoItem.FindAsync(ListaChequeoItemId);
        }

        public async Task<ListaChequeo> GetListaChequeoItemByListaChequeoId(int ListaChequeoId)
        {
            return await _context.ListaChequeo.FindAsync(ListaChequeoId);
        }

    }
}
