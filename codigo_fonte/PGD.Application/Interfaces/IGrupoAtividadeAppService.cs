using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.Interfaces
{
    public interface IGrupoAtividadeAppService
    {
        GrupoAtividadeViewModel Adicionar(GrupoAtividadeViewModel grupoatividadeViewModel);
        IEnumerable<GrupoAtividadeViewModel> ObterTodos();
        GrupoAtividadeViewModel ObterPorId(int id);
        GrupoAtividadeViewModel Atualizar(GrupoAtividadeViewModel grupoatividadeViewModel);
        GrupoAtividadeViewModel Remover(GrupoAtividadeViewModel grupoatividadeViewModel);
        List<AtividadeViewModel> PreencheList(List<int> identificadores);
        List<TipoPactoViewModel> PreencheListTipoPacto(List<int> identificadores);
    }
}
