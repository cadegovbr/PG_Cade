using PGD.Application.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class TipoAtividadeViewModel
    {
        public TipoAtividadeViewModel()
        {
            //ValidationResult = new DomainValidation.Validation.ValidationResult();
        }
        public int IdTipoAtividade { get; set; }

        [Display(Name = "Tipo de Faixa")]
        [StringLength(100,ErrorMessage ="O Campo Tipo de Faixa deve conter no máximo 100 caracteres.")]
        public string Faixa { get; set; }

        [Required(ErrorMessage = "O campo Duração da Faixa no PGD é de preenchimento obrigatório!")]
        [Display(Name = "Duração da Faixa no PGD")]
        [DataType(DataType.Currency, ErrorMessage = "Duração da Faixa no PGD deve ser um número!")]
        [Decimal(ErrorMessage = "Duração da Faixa no PGD deve ser um número!")]
        public string DuracaoFaixa { get; set; }

        [Required(ErrorMessage = "O campo Duração da Faixa Presencial é de preenchimento obrigatório!")]
        [Display(Name = "Duração da Faixa Presencial")]
        [DataType(DataType.Currency, ErrorMessage = "Duração da Faixa Presencial deve ser um número!")]
        [Decimal(ErrorMessage = "Duração da Faixa Presencial deve ser um número!")]
        public string DuracaoFaixaPresencial { get; set; }

        [Display(Name = "Texto explicativo")]
        public string TextoExplicativo { get; set; }

        public bool Excluir { get; set; }

        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }

        public string FaixaTextoExplicativo => string.IsNullOrEmpty(TextoExplicativo) ? Faixa : $"{Faixa} - {TextoExplicativo}";
    }
}
