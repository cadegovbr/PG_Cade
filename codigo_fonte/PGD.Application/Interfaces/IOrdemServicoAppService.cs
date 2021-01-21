using PGD.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.Interfaces
{
    public interface IOrdemServicoAppService
    {
        OrdemServicoViewModel Adicionar(OrdemServicoViewModel ordemServicoViewModel);
        IEnumerable<OrdemServicoViewModel> ObterTodos();
        OrdemServicoViewModel ObterPorId(int id);
        OrdemServicoViewModel Atualizar(OrdemServicoViewModel ordemServicoViewModel);
        OrdemServicoViewModel Remover(OrdemServicoViewModel ordemServicoViewModel);
        List<GrupoAtividadeViewModel> PreencheListGrupoAtividade(List<int> identificadores);
        List<CriterioAvaliacaoViewModel> PreencheListCriterioAvaliacao(List<int> identificadores);

        OrdemServicoViewModel GetOrdemVigente();
    }
}
