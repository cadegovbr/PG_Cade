using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;

namespace PGD.Infra.Data.Repository
{
    public class SituacaoProdutoRepository : Repository<SituacaoProduto>, ISituacaoProdutoRepository
    {

        public SituacaoProdutoRepository(PGDDbContext context)
            : base(context)
        {
        }
    }
}
