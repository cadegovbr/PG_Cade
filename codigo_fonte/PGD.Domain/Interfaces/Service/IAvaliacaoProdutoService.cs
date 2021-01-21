using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Interfaces.Service
{
    public interface IAvaliacaoProdutoService : IService<AvaliacaoProduto>
    {
        IEnumerable<AvaliacaoProduto> ObterAvaliacoesPorProduto(int idProduto);
    }
}
