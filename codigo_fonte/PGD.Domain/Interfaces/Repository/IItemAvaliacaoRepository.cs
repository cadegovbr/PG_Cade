using PGD.Domain.Entities;

namespace PGD.Domain.Interfaces.Repository
{
    public interface IItemAvaliacaoRepository : IRepository<ItemAvaliacao>
    {
        ItemAvaliacao AdicionarSave(ItemAvaliacao obj);
    }
}
