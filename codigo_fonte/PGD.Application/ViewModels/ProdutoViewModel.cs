using PGD.Application.Util;
using PGD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PGD.Application.ViewModels
{
    public class ProdutoViewModel
    {
        public ProdutoViewModel()
        {
            this.IniciativasPlanoOperacionalSelecionadas = new List<string>();
            this.Avaliacoes = new List<AvaliacaoProdutoViewModel>();
        }
        public int IdProduto { get; set; }
        public int IdPacto { get; set; }
        public int IdTipoPacto { get; set; }
        public virtual TipoPactoViewModel TipoPacto { get; set; }
        public int IdOrdemServico { get; set; }
        public int CargaHoraria { get; set; }
        public int QtdFaixas { get; set; }
        [Required(ErrorMessage = "O grupo de atividade é obrigatório."), Range(1, int.MaxValue, ErrorMessage = "O grupo de atividade selecionado é inválido.")]
        [Display(Name = "Grupo de Atividade")]
        public int IdGrupoAtividade { get; set; }
        public virtual GrupoAtividadeViewModel GrupoAtividade { get; set; }
        [Required(ErrorMessage = "A atividade é obrigatória."), Range(1, int.MaxValue, ErrorMessage = "A atividade selecionada é inválida.")]
        [Display(Name = "Atividade")]
        public int IdAtividade { get; set; }
        public virtual AtividadeViewModel Atividade { get; set; }
        [Required(ErrorMessage = "A informação da faixa é obrigatória."), Range(1, int.MaxValue, ErrorMessage = "A faixa informada é inválida.")]
        [Display(Name = "Faixa")]
        public int IdTipoAtividade { get; set; }
        public virtual TipoAtividadeViewModel TipoAtividade { get; set; }
        [Required(ErrorMessage = "A quantidade de produtos é obrigatória."), Range(1, int.MaxValue, ErrorMessage = "Quantidade de produtos inválida.")]
        public int QuantidadeProduto { get; set; }
        public double CargaHorariaProduto { get; set; }
        public string CargaHorariaTotalProdutoFormatada { get; set; }
        [AllowHtml]
        public string Observacoes { get; set; }
        public string ObservacoesAdicionais { get; set; }
        public string NomeGrupo { get; set; }
        public string NomeFaixa { get; set; }
        public string NomeAtividade { get; set; }
        public int Avaliacao { get; set; }
        public bool? EntregueNoPrazo { get; set; }        
        public int? IdJustificativa { get; set; }
        public DateTime? DataTerminoReal { get; set; }
        public virtual JustificativaViewModel Justificativa { get; set; }
        public int QuantidadeProdutoAvaliado { get; set; }        
        public string CargaHorariaHomologada { get; set; }
        public List<AvaliacaoProdutoViewModel> Avaliacoes { get; set; }

        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }

        public bool Excluir { get; set; }

        public bool PodeEditar { get; set; }

        public bool PodeEditarAndamento { get; set; }

        public int Index { get; set; }

        public double CargaHorariaTotal { get; set; }

        public bool PodeEditarObservacaoProduto { get; set; }

        public bool PodeVisualizarAvaliacaoProduto { get; set; }

        public bool PodeVisualizarPactuadoAvaliado { get; set; }

        public List<IniciativaPlanoOperacionalProdutoViewModel> IniciativasPlanoOperacionalProduto { get; set; }
        public List<string> IniciativasPlanoOperacionalSelecionadas { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "Ao menos uma iniciativa deve ser selecionada")]
        public int TotIniciativas => IniciativasPlanoOperacionalSelecionadas?.Count() ?? 0;
        public string CargaHorariaProdutoFormatada => Utilitarios.FormatarParaHoras(CargaHorariaProduto);

        public bool PossuiAvaliacoes => Avaliacoes?.Count > 0;

        public int QuantidadeProdutosAAvaliar => QuantidadeProduto - (Avaliacoes?.Sum(a => a.QuantidadeProdutosAvaliados)).GetValueOrDefault();
        public double CargaHorariaAAvaliar => QuantidadeProdutosAAvaliar * CargaHorariaProduto;
        public string CargaHorariaAAvaliarFormatada => Utilitarios.FormatarParaHoras(CargaHorariaAAvaliar);

        public int IdAnexoProduto { get; set; }

        public virtual List<AnexoProdutoViewModel> AnexoProduto { get; set; }

        public virtual SituacaoProdutoViewModel SituacaoProduto { get; set; }

        public int IdSituacaoProduto { get; set; }





    }
}
