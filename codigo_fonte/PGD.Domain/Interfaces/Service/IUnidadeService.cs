using PGD.Domain.Entities.RH;
using PGD.Domain.Filtros;
using PGD.Domain.Paginacao;
using System.Collections.Generic;

namespace PGD.Domain.Interfaces.Service
{
    public interface IUnidadeService : IService<Unidade>
    {
        IEnumerable<Unidade> ObterUnidades(int idTipoPacto = 0);
        IEnumerable<Unidade> ObterUnidadesSubordinadas(int idUnidadePai);
        Unidade ObterUnidade(int idUnidade);
        Paginacao<Unidade> Buscar(UnidadeFiltro filtro);
    }
}
