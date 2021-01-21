using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Interfaces.Service
{
    public interface IProdutoService : IService<Produto>
    {
        IEnumerable<Produto> ObterTodos(Produto objFiltro);
        IEnumerable<Produto> ObterTodos(int idpacto);
        Produto BuscarPorId(int idpacto, int idproduto);
        Produto AtualizarIdProduto(Produto obj, int idProduto);
        Produto AtualizarObservacaoProduto(int idProduto, string observacoes);
        Produto AtualizarSituacaoProduto(int idProduto, int idSituacaoProduto);
    }
}
