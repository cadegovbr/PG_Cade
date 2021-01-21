
using PGD.Application.ViewModels;
using PGD.Application.ViewModels.Filtros;
using PGD.Domain.Entities.RH;
using PGD.Domain.Filtros;
using PGD.Domain.Paginacao;

namespace PGD.Application.Interfaces
{
    public interface IUnidadeAppService
    {
        Paginacao<UnidadeViewModel> Buscar(UnidadeFiltroViewModel filtro);
    }
}
