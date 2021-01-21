using PGD.Application.ViewModels;
using System.Collections.Generic;

namespace PGD.Application.Interfaces
{
    public interface INivelAvaliacaoAppService
    {
        NivelAvaliacaoViewModel Adicionar(NivelAvaliacaoViewModel nivelAvaliacaoViewModel);
        IEnumerable<NivelAvaliacaoViewModel> ObterTodos();
        NivelAvaliacaoViewModel Atualizar(NivelAvaliacaoViewModel nivelAvaliacaoViewModel);
        void Remover(int id);
    }
}
