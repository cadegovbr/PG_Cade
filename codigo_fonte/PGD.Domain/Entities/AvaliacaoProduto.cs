using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PGD.Domain.Entities
{
    public class AvaliacaoProduto
    {
        public int IdAvaliacaoProduto { get; set; }
        public int IdProduto { get; set; }
        public virtual Produto Produto { get; set; }
        public string CPFAvaliador { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public int QuantidadeProdutosAvaliados { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A qualidade da atividade é obrigatória")]
        public int Avaliacao { get; set; }
        [Required(ErrorMessage ="Informe se foi entregue no prazo")]
        public bool? EntregueNoPrazo { get; set; }
        [Required(ErrorMessage = "A descrição da entrega é obrigatória")]
        [MaxLength(8000)]
        public string LocalizacaoProduto { get; set; }
        public DateTime? DataTerminoReal { get; set; }
        public int? IdJustificativa { get; set; }
        public virtual Justificativa Justificativa { get; set; }
        public int TipoAvaliacao { get; set; }
        public int IdNivelAvaliacao { get; set; }
        public decimal? NotaFinalAvaliacaoDetalhada { get; set; }
        public virtual NivelAvaliacao NivelAvaliacao { get; set; }
        public virtual List<AvaliacaoDetalhadaProduto> AvaliacoesDetalhadas { get; set; }
    }
}
