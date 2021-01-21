using PGD.Domain.Entities;
using System.Collections.Generic;

namespace PGD.Domain.Interfaces.Service
{
    public interface IOrdemServicoService : IService<OrdemServico>
    {
        IEnumerable<OrdemServico> ObterTodosAtivos();
        void DeletarGrupos(OrdemServico ordemservico);
        OrdemServico ObterOrdemVigente();

        OrdemServico ObterPorIdInclude(int id);
    }
}
