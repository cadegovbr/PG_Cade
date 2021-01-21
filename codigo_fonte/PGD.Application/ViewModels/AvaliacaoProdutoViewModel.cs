using PGD.Application.Util;
using PGD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PGD.Application.ViewModels
{
    public class AvaliacaoProdutoViewModel
    {
        public AvaliacaoProdutoViewModel()
        {
            CriteriosAvaliacao = new List<OS_CriterioAvaliacaoViewModel>();
        }

        public int IdPacto { get; set; }
        public int IdProduto { get; set; }
        public ProdutoViewModel Produto { get; set; }
        public int IdAvaliacaoProduto { get; set; }
        public string CPFAvaliador { get; set; }
        public string NomeAvaliador { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public int QuantidadeProdutosAvaliados { get; set; }
        public bool? EntregueNoPrazo { get; set; }
        public int? IdJustificativa { get; set; }
        public DateTime? DataTerminoReal { get; set; }
        public int Avaliacao { get; set; }
        [AllowHtml]
        public string LocalizacaoProduto { get; set; }
        public int TipoAvaliacao { get; set; }

        //controle de tela
        public bool HabilitarCampos { get; set; }

        public bool ModoSomenteLeitura { get; set; }

        public int IdOrigemAcao { get; set; }

        public int IdNivelAvaliacao { get; set; }

        public List<OS_CriterioAvaliacaoViewModel> CriteriosAvaliacao { get; set; }

        public List<AvaliacaoDetalhadaProdutoViewModel> AvaliacoesDetalhadas { get; set; }

        public decimal? NotaFinalAvaliacaoDetalhada { get; set; }

        public string CargaHorariaPendenteFormatada => Utilitarios.FormatarParaHoras((Produto.QuantidadeProduto - Produto.QuantidadeProdutoAvaliado) * Produto.CargaHorariaProduto);
        public string CargaHorariaAvaliadaFormatada => Utilitarios.FormatarParaHoras(QuantidadeProdutosAvaliados * Produto.CargaHorariaProduto);
        public string CargaHorariaAvaliadaAnteriormenteFormatada => Utilitarios.FormatarParaHoras(Produto.QuantidadeProdutoAvaliado * Produto.CargaHorariaProduto);
        public double CargaHorariaAvaliada => QuantidadeProdutosAvaliados * Produto?.CargaHorariaProduto ?? 0;

        public string DescricaoAvaliacao
        {
            get
            {
                if (Avaliacao == (int)eAvaliacao.Bom) return "Bom";
                else if (Avaliacao == (int)eAvaliacao.Excelente) return "Excelente";
                else if (Avaliacao == (int)eAvaliacao.Insatisfatorio) return "Insatisfatório";
                else if (Avaliacao == (int)eAvaliacao.MuitoBom) return "Muito Bom";
                else if (Avaliacao == (int)eAvaliacao.NaoEntregue) return "Não Entregue";
                else if (Avaliacao == (int)eAvaliacao.Regular) return "Regular";
                else return null;

            }
        }

        public string[] ItensAvaliados
        {
            get;
            set;
        }
        public bool ehAvaliacaoSimplificada => IdNivelAvaliacao == (int)eNivelAvaliacao.Simplificada;
        
        public bool ehAvaliacaoDetalhada => IdNivelAvaliacao == (int) eNivelAvaliacao.Detalhada;
        
        public bool podeTerAvaliacaoDetalhada
        {
            get; set;
        }

    }
}
