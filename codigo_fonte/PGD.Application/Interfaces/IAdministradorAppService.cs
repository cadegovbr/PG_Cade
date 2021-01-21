using PGD.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.Interfaces
{
    public interface IAdministradorAppService
    {
        AdministradorViewModel Adicionar(AdministradorViewModel administradorViewModel);
        IEnumerable<AdministradorViewModel> ObterTodos();
        //AdministradorViewModel ObterPorId(int id);
        AdministradorViewModel Atualizar(AdministradorViewModel administradorViewModel);
        AdministradorViewModel Remover(AdministradorViewModel administradorViewModel);
    }
}
