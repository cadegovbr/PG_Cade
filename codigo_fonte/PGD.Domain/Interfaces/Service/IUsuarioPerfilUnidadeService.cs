using PGD.Domain.Entities;
using PGD.Domain.Filtros;
using PGD.Domain.Paginacao;

namespace PGD.Domain.Interfaces.Service
{
    public interface IUsuarioPerfilUnidadeService
    {
        Paginacao<UsuarioPerfilUnidade> Buscar(UsuarioPerfilUnidadeFiltro filtro);
        UsuarioPerfilUnidade Adicionar(UsuarioPerfilUnidade usuarioPerfilUnidade);
        UsuarioPerfilUnidade Remover(long id);
    }
}
