using PGD.Application.ViewModels;
using System.Collections.Generic;

namespace PGD.Application.Interfaces
{
    public interface ITipoPactoAppService
    {
        TipoPactoViewModel Adicionar(TipoPactoViewModel tipoPactoViewModel);
        IEnumerable<TipoPactoViewModel> ObterTodos();
        TipoPactoViewModel Atualizar(TipoPactoViewModel tipoPactoViewModel);
        void Remover(int id);
    }
}
