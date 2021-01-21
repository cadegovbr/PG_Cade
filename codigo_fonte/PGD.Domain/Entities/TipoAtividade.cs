using DomainValidation.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGD.Domain.Entities
{
    public class TipoAtividade
    {
        public TipoAtividade()
        {
            ValidationResult = new DomainValidation.Validation.ValidationResult();
        }

        public int IdTipoAtividade { get; set; }
        public string Faixa { get; set; }
        [MaxLength(300, ErrorMessage = "O tamanho máximo para o campo Texto Explicativo é de 300 caracteres.")]
        [Column("DescTextoExplicativo")]
        public string TextoExplicativo { get; set; }
        public double DuracaoFaixa { get; set; }
        public double DuracaoFaixaPresencial { get; set; }
        public int IdAtividade { get; set; }
        public virtual Atividade Atividade { get; set; }

        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
