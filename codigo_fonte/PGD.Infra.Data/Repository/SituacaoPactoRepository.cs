using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;

namespace PGD.Infra.Data.Repository
{
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
    Justification = "False positive.")]
    public class SituacaoPactoRepository : Repository<SituacaoPacto>, ISituacaoPactoRepository
    {

        public SituacaoPactoRepository(PGDDbContext context)
            : base(context)
        {

        }
    }
}
