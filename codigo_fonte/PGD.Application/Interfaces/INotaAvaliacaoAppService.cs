using PGD.Application.ViewModels;
using System.Collections.Generic;

namespace PGD.Application.Interfaces
{
    public interface INotaAvaliacaoAppService
    {
        NotaAvaliacaoViewModel Adicionar(NotaAvaliacaoViewModel notaAvaliacaoViewModel);
        IEnumerable<NotaAvaliacaoViewModel> ObterTodos();
        NotaAvaliacaoViewModel ObterPorId(int id);
        IEnumerable<NotaAvaliacaoViewModel> ObterTodosPorNivelAvaliacao(int idNivelAvaliacao);
        NotaAvaliacaoViewModel Atualizar(NotaAvaliacaoViewModel notaAvaliacaoViewModel);
        void Remover(int id);
    }
}
