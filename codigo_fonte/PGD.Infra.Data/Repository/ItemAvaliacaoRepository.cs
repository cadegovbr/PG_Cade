using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;

namespace PGD.Infra.Data.Repository
{
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
    Justification = "False positive.")]
    public class ItemAvaliacaoRepository : Repository<ItemAvaliacao>, IItemAvaliacaoRepository
    {

        public ItemAvaliacaoRepository(PGDDbContext context)
            : base(context)
        {

        }
    }
}
