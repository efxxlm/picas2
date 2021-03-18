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
        Task<Respuesta> DeleteListaChequeo(int pListaChequeoId, string pAutor);

        Task<bool> GetValidateExistNameCheckList(ListaChequeo pListaChequeo);

        Task<Respuesta> DeleteListaChequeoItem(int pListaChequeoListaChequeoItemId, string pAutor);

        Task<bool> SendEmailWhenDesactiveListaChequeo(int pListaChequeoId);

        Task<Respuesta> ActivateDeactivateListaChequeo(ListaChequeo pListaChequeo);

        Task<List<ListaChequeoItem>> GetListItem();

        Task<List<ListaChequeo>> GetCheckList();

        Task<Respuesta> CreateEditItem(ListaChequeoItem pListaChequeoItem);

        Task<Respuesta> CreateEditCheckList(ListaChequeo pListaChequeo);

        Task<Respuesta> ActivateDeactivateListaChequeoItem(ListaChequeoItem pListaChequeoItem);

        Task<ListaChequeo> GetListaChequeoItemByListaChequeoId(int ListaChequeoId);

        Task<ListaChequeoItem> GetListaChequeoItemByListaChequeoItemId(int ListaChequeoItemId);
    }
}
