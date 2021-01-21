using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.Interfaces
{
    public interface ICriterioAvaliacaoAppService
    {
        CriterioAvaliacaoViewModel Adicionar(CriterioAvaliacaoViewModel criterioAvaliacaoViewModel);
        IEnumerable<CriterioAvaliacaoViewModel> ObterTodos();
        CriterioAvaliacaoViewModel ObterPorId(int id);
        CriterioAvaliacaoViewModel Atualizar(CriterioAvaliacaoViewModel criterioAvaliacaoViewModel);
        CriterioAvaliacaoViewModel Remover(CriterioAvaliacaoViewModel criterioAvaliacaoViewModel);
        List<ItemAvaliacaoViewModel> PreencheList(List<int> identificadores);
    }
}
