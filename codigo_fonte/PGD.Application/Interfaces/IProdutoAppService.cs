using PGD.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.Interfaces
{
    public interface IProdutoAppService
    {
        ProdutoViewModel Adicionar(ProdutoViewModel produtoViewModel);
        IEnumerable<ProdutoViewModel> ObterTodos(int idpacto);
        IEnumerable<ProdutoViewModel> ObterTodos(ProdutoViewModel objFiltro);
        ProdutoViewModel Atualizar(ProdutoViewModel produtoViewModel);
        ProdutoViewModel Remover(ProdutoViewModel produtoViewModel);
        ProdutoViewModel BuscarPorId(int idpacto, int idproduto);
    }
}
