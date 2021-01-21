using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PGD.Infra.Data.Repository
{
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
    Justification = "False positive.")]
    public class AnexoProdutoRepository : Repository<AnexoProduto>, IAnexoProdutoRepository
    {
        public AnexoProdutoRepository(PGDDbContext context)
           : base(context)
        {

        }
    }
}
