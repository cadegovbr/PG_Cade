using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Infra.Data.EntityConfig
{
    public class AvaliacaoProdutoConfig : EntityTypeConfiguration<AvaliacaoProduto>
    {
        public AvaliacaoProdutoConfig()
        {
            HasKey(x => x.IdAvaliacaoProduto);
            Property(x => x.IdProduto).IsRequired();
            Property(x => x.CPFAvaliador).IsRequired().HasMaxLength(11);
            Property(x => x.DataAvaliacao).IsRequired();
            Property(x => x.QuantidadeProdutosAvaliados).IsRequired();
            Property(x => x.Avaliacao).IsRequired();
            Property(x => x.EntregueNoPrazo).IsOptional();
            Property(x => x.LocalizacaoProduto).IsOptional().HasColumnType("varchar(max)");
            Property(x => x.IdNivelAvaliacao).IsRequired();
        }
    }
}
