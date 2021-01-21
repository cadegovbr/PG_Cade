using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PGD.Application.ViewModels
{
    public class OrdemServicoViewModel
    {
        public int? IdOrdemServico { get; set; }

        [Display(Name = "Número OS")]
        public int NumeroOS { get; set; }

        [Required(ErrorMessage = "O campo Descrição é de preenchimento obrigatório!")]
        [Display(Name = "Descrição")]
        [MaxLength(1000, ErrorMessage = "Tamanho inválido para o campo Descrição!")]
        public string DescOrdemServico { get; set; }

        [Required(ErrorMessage = "O campo Data Início é de preenchimento obrigatório!")]
        public DateTime DatInicioSistema { get; set; }

        [Required(ErrorMessage = "O campo Data Início normativo é de preenchimento obrigatório!")]
        public DateTime DatInicioNormativo { get; set; }

        [Required(ErrorMessage = "O campo Grupo de Atividades é de preenchimento obrigatório")]
        public List<int> idsGrupos { get; set; }
        public ICollection<GrupoAtividadeViewModel> Grupos { get; set; }

        [Required(ErrorMessage = "O campo Critério de Avaliação é de preenchimento obrigatório")]
        public List<int> idsCriteriosAvaliacao { get; set; }
        public ICollection<CriterioAvaliacaoViewModel> CriteriosAvaliacao { get; set; }

        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }

        public UsuarioViewModel Usuario { get; set; }
    }
}
