using PGD.Application.ViewModels;
using System.Collections.Generic;

namespace PGD.Application.Interfaces
{
    public interface ITipoAtividadeAppService
    {
        TipoAtividadeViewModel Adicionar(TipoAtividadeViewModel atividadeViewModel);
        IEnumerable<TipoAtividadeViewModel> ObterTodos();
        TipoAtividadeViewModel Atualizar(TipoAtividadeViewModel atividadeViewModel);
        void Remover(int id);
    }
}
