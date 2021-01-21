using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Infra.Data.EntityConfig
{
    public class ParametroSistemaConfig : EntityTypeConfiguration<ParametroSistema>
    {
        public ParametroSistemaConfig() {
            HasKey(p => p.IdParametroSistema);
            Property(p => p.DescParametroSistema).IsRequired().HasMaxLength(150);
            Property(p => p.Valor).IsRequired().HasMaxLength(25);
        }
    }
}
