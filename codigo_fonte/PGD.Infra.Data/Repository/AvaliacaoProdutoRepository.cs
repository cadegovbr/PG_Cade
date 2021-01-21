using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Infra.Data.Repository
{
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
    Justification = "False positive.")]
    public class AvaliacaoProdutoRepository : Repository<AvaliacaoProduto>, IAvaliacaoProdutoRepository
    {
        public AvaliacaoProdutoRepository(PGDDbContext context)
            : base(context)
        {

        }
    }
}
