using PGD.Application.ViewModels;
using System.Collections.Generic;

namespace PGD.Application.Interfaces
{
    public interface IOS_CriterioAvaliacaoAppService
    {
        OS_CriterioAvaliacaoViewModel Adicionar(OS_CriterioAvaliacaoViewModel oS_CriterioAvaliacaoViewModel, UsuarioViewModel user);
        IEnumerable<OS_CriterioAvaliacaoViewModel> ObterTodos();
        IEnumerable<OS_CriterioAvaliacaoViewModel> BuscarPorIdOS(int idOS);
        OS_CriterioAvaliacaoViewModel Atualizar(OS_CriterioAvaliacaoViewModel oS_CriterioAvaliacaoViewModel, UsuarioViewModel user);
        void Remover(int id, UsuarioViewModel user);        
    }
}
