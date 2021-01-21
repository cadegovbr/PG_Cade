namespace PGD.Application.ViewModels
{
    public class AvaliacaoDetalhadaProdutoViewModel
    {
        public int IdAvaliacaoDetalhadaProduto { get; set; }
        public int IdAvaliacaoProduto { get; set; }
        public virtual AvaliacaoProdutoViewModel AvaliacaoProduto { get; set; }        
        public int IdOS_ItemAvaliacao { get; set; }
        public virtual OS_ItemAvaliacaoViewModel OS_ItemAvaliacao { get; set; }

        public int IdOS_CriterioAvaliacao { get; set; }
        public virtual OS_CriterioAvaliacaoViewModel OS_CriterioAvaliacao { get; set; }
    }
}
