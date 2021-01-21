using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class AvaliacaoDetalhadaProdutoConfig : EntityTypeConfiguration<AvaliacaoDetalhadaProduto>
    {
        public AvaliacaoDetalhadaProdutoConfig()
        {
            HasKey(x => x.IdAvaliacaoDetalhadaProduto);
            Property(x => x.IdAvaliacaoProduto).IsRequired();
            Property(x => x.IdOS_ItemAvaliacao).IsRequired();
        }
    }
}
