using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;

namespace PGD.Infra.Data.Repository
{
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
    Justification = "False positive.")]
    public class OS_ItemAvaliacaoRepository : Repository<OS_ItemAvaliacao>, IOS_ItemAvaliacaoRepository
    {

        public OS_ItemAvaliacaoRepository(PGDDbContext context)
            : base(context)
        {

        }
    }
}
