using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    class AnexoProdutoConfig : EntityTypeConfiguration<AnexoProduto>
    {
        public AnexoProdutoConfig()
        {
            HasKey(x => x.IdAnexoProduto);
            Property(x => x.Nome).IsRequired();
            Property(x => x.Tipo).IsRequired();
            Property(x => x.Tamanho).IsRequired();
            Property(x => x.IdProduto).IsRequired();
          
            Ignore(c => c.ValidationResult);
        }
    }
}
