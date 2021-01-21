using PGD.Application.ViewModels;
using System.Collections.Generic;

namespace PGD.Application.Interfaces
{
    public interface ISituacaoPactoAppService
    {
        SituacaoPactoViewModel Adicionar(SituacaoPactoViewModel situacaoPactoViewModel);
        IEnumerable<SituacaoPactoViewModel> ObterTodos();
        SituacaoPactoViewModel Atualizar(SituacaoPactoViewModel situacaoPactoViewModel);
        void Remover(int id);
    }
}
