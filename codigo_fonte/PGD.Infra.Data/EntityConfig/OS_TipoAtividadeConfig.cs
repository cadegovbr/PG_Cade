using PGD.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PGD.Infra.Data.EntityConfig
{
    public class OS_TipoAtividadeConfig : EntityTypeConfiguration<OS_TipoAtividade>
    {
        public OS_TipoAtividadeConfig()
        {
            HasKey(x => x.IdTipoAtividade);
            Property(x => x.DuracaoFaixa).IsRequired();
            Property(x => x.DuracaoFaixaPresencial).IsRequired();
            Property(x => x.Faixa).IsRequired();
            Property(x => x.IdOS_Atividade).IsRequired();

            Ignore(c => c.ValidationResult);
        }
    }
}
