using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class TipoAtividadeConfig : EntityTypeConfiguration<TipoAtividade>
    {
        public TipoAtividadeConfig()
        {
            HasKey(x => x.IdTipoAtividade);
            Property(x => x.DuracaoFaixa).IsRequired();
            Property(x => x.DuracaoFaixaPresencial).IsRequired();
            Property(x => x.Faixa).IsRequired();
            Property(x => x.IdAtividade).IsRequired();

            Ignore(c => c.ValidationResult);
        }
    }
}
