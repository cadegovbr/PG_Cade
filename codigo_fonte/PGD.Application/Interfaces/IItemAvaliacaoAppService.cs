using PGD.Application.ViewModels;
using System.Collections.Generic;

namespace PGD.Application.Interfaces
{
    public interface IItemAvaliacaoAppService
    {
        ItemAvaliacaoViewModel Adicionar(ItemAvaliacaoViewModel itemAvaliacaoViewModel);
        IEnumerable<ItemAvaliacaoViewModel> ObterTodos();
        ItemAvaliacaoViewModel ObterPorId(int id);
        ItemAvaliacaoViewModel Atualizar(ItemAvaliacaoViewModel itemAvaliacaoViewModel);
        ItemAvaliacaoViewModel Remover(ItemAvaliacaoViewModel itemAvaliacaoViewModel);
    }
}
