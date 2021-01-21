using DomainValidation.Validation;
using PGD.Domain.Validations.TiposAtividade;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGD.Domain.Entities
{
    public class Atividade
    {
        public Atividade()
        {
            ValidationResult = new DomainValidation.Validation.ValidationResult();

        }

        public int IdAtividade { get; set; }
        public string NomAtividade { get; set; }
        public int PctMinimoReducao { get; set; }
        public bool Inativo { get; set; }

        [Column("DescLinkAtividade")]
        [MaxLength(300, ErrorMessage = "O tamanho máximo para o campo Link é de 300 caracteres.")]
        public string Link { get; set; }

        public virtual ICollection<GrupoAtividade> Grupos { get; set; }
        public virtual ICollection<TipoAtividade> Tipos { get; set; }

        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            ValidationResult = new DomainValidation.Validation.ValidationResult();
            foreach (var tipo in Tipos)
                ValidationResult.Add(new TipoAtividadeValidation().Validate(tipo));

            return ValidationResult.IsValid;
        }
    }
}
