using PGD.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.Interfaces
{
    public interface IHistoricoAppService
    {
        HistoricoViewModel Adicionar(HistoricoViewModel historicoViewModel);
        IEnumerable<HistoricoViewModel> ObterTodos(int idpacto);
        IEnumerable<HistoricoViewModel> ObterTodos(HistoricoViewModel objFiltro);
        HistoricoViewModel Atualizar(HistoricoViewModel historicoViewModel);
        HistoricoViewModel Remover(HistoricoViewModel historicoViewModel);
        HistoricoViewModel BuscarPorId(int idpacto, int idhistorico);
    }
}
