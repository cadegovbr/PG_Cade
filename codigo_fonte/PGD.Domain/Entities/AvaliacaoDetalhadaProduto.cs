using System.ComponentModel.DataAnnotations.Schema;

namespace PGD.Domain.Entities
{
    public class AvaliacaoDetalhadaProduto
    {
        public int IdAvaliacaoDetalhadaProduto { get; set; }
        public int IdAvaliacaoProduto { get; set; }
        public virtual AvaliacaoProduto AvaliacaoProduto { get; set; }
        [ForeignKey("OS_ItemAvaliacao")]
        public int IdOS_ItemAvaliacao { get; set; }
        public virtual OS_ItemAvaliacao OS_ItemAvaliacao { get; set; }

        [ForeignKey("OS_CriterioAvaliacao")]
        public int IdOS_CriterioAvaliacao { get; set; }
        public virtual OS_CriterioAvaliacao OS_CriterioAvaliacao { get; set; }
    }
}
