using PGD.Domain.Entities;
using System.Collections.Generic;

namespace PGD.Domain.Interfaces.Service
{
    public interface IItemAvaliacaoService : IService<ItemAvaliacao>
    {
        IEnumerable<ItemAvaliacao> ObterTodosAtivos();
    }
}
