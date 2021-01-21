using PGD.Domain.Entities;

namespace PGD.Domain.Interfaces.Repository
{
    public interface IOrdemServicoRepository : IRepository<OrdemServico>
    {
        void DeletarGrupos(OrdemServico ordemservico);
        OrdemServico AdicionarSave(OrdemServico obj);

        OrdemServico ObterTodosInclude();

        OrdemServico ObterPorIdInclude(int id);
    }
}
