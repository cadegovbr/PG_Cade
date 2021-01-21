using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;

namespace PGD.Infra.Data.Repository
{
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
    Justification = "False positive.")]
    public class OS_CriterioAvaliacaoRepository : Repository<OS_CriterioAvaliacao>, IOS_CriterioAvaliacaoRepository
    {

        public OS_CriterioAvaliacaoRepository(PGDDbContext context)
            : base(context)
        {

        }
    }
}
