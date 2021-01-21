using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Infra.Data.EntityConfig
{
    public class HistoricoConfig : EntityTypeConfiguration<Historico>
    {
        public HistoricoConfig()
        {
            HasKey(x => x.IdHistorico);
            Property(x => x.IdPacto).IsRequired();
            Property(x => x.Descricao).IsRequired().HasMaxLength(300);
            Ignore(c => c.ValidationResult);
        }
    }
}
