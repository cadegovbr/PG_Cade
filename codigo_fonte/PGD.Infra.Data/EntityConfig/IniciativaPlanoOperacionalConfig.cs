using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Infra.Data.EntityConfig
{
    public class IniciativaPlanoOperacionalConfig : EntityTypeConfiguration<IniciativaPlanoOperacional>
    {

        public IniciativaPlanoOperacionalConfig()
        {
            HasKey(x => x.IdIniciativaPlanoOperacional);
        }

    }
}
