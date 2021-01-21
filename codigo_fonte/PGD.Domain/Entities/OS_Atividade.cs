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
    public class OS_Atividade
    {
        public OS_Atividade()
        {
            ValidationResult = new DomainValidation.Validation.ValidationResult();
        }

        public int IdAtividade { get; set; }
        public string NomAtividade { get; set; }
        public int PctMinimoReducao { get; set; }
        public bool Inativo { get; set; }
        public int IdOS_GrupoAtividade { get; set; }
        public virtual OS_GrupoAtividade Grupo { get; set; }
        public virtual ICollection<OS_TipoAtividade> Tipos { get; set; }
        [Column("DescLinkAtividade")]
        [MaxLength(300, ErrorMessage = "O tamanho máximo para o campo Link é de 300 caracteres.")]
        public string Link { get; set; }
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            ValidationResult = new DomainValidation.Validation.ValidationResult();
            return true;
        }
    }
}
