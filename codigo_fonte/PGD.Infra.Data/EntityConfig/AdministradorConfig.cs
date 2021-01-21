using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Infra.Data.EntityConfig
{
    public class AdminstradorConfig : EntityTypeConfiguration<Administrador>
    {
        public AdminstradorConfig()
        {
            HasKey(x => x.IdAdministrador);
            Property(x => x.CPF).IsRequired().HasMaxLength(14);
            Ignore(c => c.ValidationResult);
        }
    }
}