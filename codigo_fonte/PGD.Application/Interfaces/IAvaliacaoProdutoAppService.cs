using PGD.Application.ViewModels;
using System.Collections.Generic;

namespace PGD.Application.Interfaces
{
    public interface IAvaliacaoProdutoAppService
    { 
        AvaliacaoProdutoViewModel ObterPorId(int idAvaliacaoProduto);
        NotaAvaliacaoViewModel CalcularNotaAvaliacaoDetalhada(List<ItemAvaliadoViewModel> lstItensAvaliados);
        int RetornaQualidadeAvaliacaoDetalhada(NotaAvaliacaoViewModel nota);
    }
}
