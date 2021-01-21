using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;

namespace PGD.Infra.Data.Repository
{
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
    Justification = "False positive.")]
    public class AtividadeRepository : Repository<Atividade>, IAtividadeRepository
    {

        public AtividadeRepository(PGDDbContext context)
            : base(context)
        {

        }

        //public override void Remover(int id)
        //{
        //    var atividade = ObterPorId(id);
        //    if(atividade!= null)
        //    {
        //        atividade.Excluido = true;
        //        Atualizar(atividade);
        //    }
        //}
    }
}
