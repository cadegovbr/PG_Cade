using PGD.Application.ViewModels;
using PGD.Application.ViewModels.Filtros;
using PGD.Application.ViewModels.Paginacao;
using System.Collections.Generic;

namespace PGD.Application.Interfaces
{
    public interface IUsuarioAppService
    {
        UsuarioViewModel Adicionar(UsuarioViewModel usuarioViewModel);
        UsuarioViewModel ObterPorId(int id);
        UsuarioViewModel Atualizar(UsuarioViewModel usuarioViewModel);
        UsuarioViewModel Remover(UsuarioViewModel usuarioViewModel);

        UsuarioViewModel ObterPorNome(string nome);
        UsuarioViewModel ObterPorEmail(string email);
        UsuarioViewModel ObterPorCPF(string cpf);
        IEnumerable<UsuarioViewModel> ObterTodos(int idUnidade, bool incluirSubordinados = false);
        IEnumerable<UsuarioViewModel> ObterTodos();
        UsuarioViewModel TornarRemoverAdministrador(UsuarioViewModel usuario, bool admin);

        List<PGD.Domain.Enums.Perfil> ObterPerfis(UsuarioViewModel usuario);
        IEnumerable<UsuarioViewModel> ObterTodosAdministradores();

        bool PodeSelecionarPerfil(UsuarioViewModel usuario);
        bool PodeSelecionarUnidade(UsuarioViewModel usuario);
        PaginacaoViewModel<UsuarioViewModel> Buscar(UsuarioFiltroViewModel model);
        PaginacaoViewModel<UnidadeViewModel> BuscarUnidades(UnidadeFiltroViewModel filtro);
        PaginacaoViewModel<UsuarioPerfilUnidadeViewModel> BuscarPerfilUnidade(UsuarioPerfilUnidadeFiltroViewModel filtro);
        VincularPerfilUsuarioViewModel VincularUnidadePerfil(VincularPerfilUsuarioViewModel model, string cpfUsuarioLogado);
        VincularPerfilUsuarioViewModel RemoverVinculoUnidadePerfil(long idUsuarioPerfilUnidade, string cpfUsuarioLogado);
        ICollection<PermissaoViewModel> BuscarPermissoes(int? idPerfil);
    }
}
