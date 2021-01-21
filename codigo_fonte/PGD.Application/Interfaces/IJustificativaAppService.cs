using PGD.Application.ViewModels;
using System.Collections.Generic;

namespace PGD.Application.Interfaces
{
    public interface IJustificativaAppService
    {
        JustificativaViewModel Adicionar(JustificativaViewModel justificativaViewModel);
        IEnumerable<JustificativaViewModel> ObterTodos();
        JustificativaViewModel Atualizar(JustificativaViewModel justificativaViewModel);
        void Remover(int id);
    }
}
