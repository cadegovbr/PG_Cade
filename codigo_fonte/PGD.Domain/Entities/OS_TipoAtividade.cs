using DomainValidation.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Entities
{
    public class OS_TipoAtividade
    {
        public OS_TipoAtividade()
        {
            ValidationResult = new DomainValidation.Validation.ValidationResult();
        }

        public int IdTipoAtividade { get; set; }
        public string Faixa { get; set; }
        public double DuracaoFaixa { get; set; }
        public double DuracaoFaixaPresencial { get; set; }

        public int IdOS_Atividade { get; set; }

        [MaxLength(300, ErrorMessage = "O tamanho máximo para o campo Texto Explicativo é de 300 caracteres.")]
        [Column("DescTextoExplicativo")]
        public string TextoExplicativo { get; set; }


        public virtual OS_Atividade Atividade { get; set; }

        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
