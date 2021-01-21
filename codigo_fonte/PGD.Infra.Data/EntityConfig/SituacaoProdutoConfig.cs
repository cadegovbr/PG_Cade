using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using PGD.Domain.Entities;

namespace PGD.Infra.Data.EntityConfig
{
    public class SituacaoProdutoConfig : EntityTypeConfiguration<SituacaoProduto>
    {

        public SituacaoProdutoConfig()
        {
            HasKey(x => x.IdSituacaoProduto);
            Property(x => x.DescSituacaoProduto).IsRequired();

            Ignore(c => c.ValidationResult);
        }
    }
}

