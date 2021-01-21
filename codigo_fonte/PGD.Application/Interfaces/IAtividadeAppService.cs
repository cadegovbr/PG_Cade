using PGD.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.Interfaces
{
    public interface IAtividadeAppService
    {
        AtividadeViewModel Adicionar(AtividadeViewModel atividadeViewModel);
        IEnumerable<AtividadeViewModel> ObterTodos();
        AtividadeViewModel ObterPorId(int id);
        AtividadeViewModel Atualizar(AtividadeViewModel atividadeViewModel);
        AtividadeViewModel Remover(AtividadeViewModel atividadeViewModel);
    }
}
