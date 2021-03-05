using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.services.Interfaces
{
    public interface IManageCheckListService
    { 
        Task<List<ListaChequeoItem>> GetListItem();

        Task<List<ListaChequeo>> GetCheckList();

        Task<Respuesta> CreateEditItem(ListaChequeoItem pListaChequeoItem);

        Task<Respuesta> CreateEditCheckList(ListaChequeo pListaChequeo);

        Task<ListaChequeo> GetListaChequeoItemByListaChequeoId(int ListaChequeoId);

        Task<ListaChequeoItem> GetListaChequeoItemByListaChequeoItemId(int ListaChequeoItemId);
    }
}
