using PGD.Domain.Entities;
using PGD.Domain.Filtros;
using PGD.Domain.Paginacao;

namespace PGD.Domain.Interfaces.Service
{
    public interface IPermissaoService
    {
        Paginacao<Permissao> Buscar(PermissaoFiltro filtro);
    }
}
