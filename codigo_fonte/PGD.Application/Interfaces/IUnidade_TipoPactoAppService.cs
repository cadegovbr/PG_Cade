using PGD.Application.ViewModels;
using PGD.Application.ViewModels.Filtros;
using PGD.Application.ViewModels.Paginacao;
using System.Collections.Generic;

namespace PGD.Application.Interfaces
{
    public interface IUnidade_TipoPactoAppService
    {
        Unidade_TipoPactoViewModel Adicionar(Unidade_TipoPactoViewModel unidade_tipoPactoViewModel, UsuarioViewModel user);
        IEnumerable<Unidade_TipoPactoViewModel> ObterTodos();
        Unidade_TipoPactoViewModel BuscarPorIdUnidadeTipoPacto(int idUnidade, int idTipoPacto);        
        Unidade_TipoPactoViewModel Atualizar(Unidade_TipoPactoViewModel unidade_tipoPactoViewModel, UsuarioViewModel user);
        void Remover(int id, UsuarioViewModel user);
        PaginacaoViewModel<Unidade_TipoPactoViewModel> Buscar(UnidadeTipoPactoFiltroViewModel filtro);
    }
}
