using PGD.Application.ViewModels;
using System.Collections.Generic;

namespace PGD.Application.Interfaces
{
    public interface IOS_ItemAvaliacaoAppService
    {
        OS_ItemAvaliacaoViewModel Adicionar(OS_ItemAvaliacaoViewModel oS_ItemAvaliacaoViewModel, UsuarioViewModel user);
        IEnumerable<OS_ItemAvaliacaoViewModel> ObterTodos();
        OS_ItemAvaliacaoViewModel BuscarPorId(int idItemAvaliacao);
        OS_ItemAvaliacaoViewModel Atualizar(OS_ItemAvaliacaoViewModel oS_ItemAvaliacaoViewModel, UsuarioViewModel user);
        void Remover(int id, UsuarioViewModel user);        
    }
}
