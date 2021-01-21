using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class ProdutoConfig : EntityTypeConfiguration<Produto>
    {
        public ProdutoConfig()
        {
            HasKey(x => x.IdProduto);
            Property(x => x.IdAtividade).IsRequired();
            Property(x => x.IdGrupoAtividade).IsRequired();
            Property(x => x.IdTipoAtividade).IsRequired();
            Property(x => x.CargaHorariaProduto).IsRequired();
            Property(x => x.QuantidadeProduto).IsRequired();
            Property(x => x.IdPacto).IsRequired();
            Property(x => x.Avaliacao).IsRequired();
            Property(x => x.EntregueNoPrazo).IsOptional();
            Property(x => x.IdJustificativa).IsOptional();

            Ignore(c => c.ValidationResult);
        }
    }
}
