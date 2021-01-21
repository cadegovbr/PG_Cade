using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Interfaces.Repository
{
    public interface IProdutoRepository:IRepository<Produto>
    {
        Produto BuscarPorId(int idpacto, int idproduto);
        Produto Atualizar(Produto produto, int idProduto);
    }
}
