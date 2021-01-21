using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Interfaces.Service
{
    public interface IAnexoProdutoService :IService<AnexoProduto>
    {

        IEnumerable<AnexoProduto> ObterTodos(AnexoProduto objFiltro);
        IEnumerable<AnexoProduto> ObterTodos(int idProduto);
        AnexoProduto BuscarPorId(int idProduto, int idAnexoProduto);
        AnexoProduto AtualizarIdAnexoProduto(AnexoProduto obj, int idAnexoProduto);
        
    }
}
