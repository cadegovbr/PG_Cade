using System.Collections.Generic;
using DomainValidation.Validation;

namespace PGD.Domain.Entities
{
    public class GrupoAtividade
    {
        public GrupoAtividade()
        {
            ValidationResult = new ValidationResult();

        }
        public int IdGrupoAtividade { get; set; }
        public string NomGrupoAtividade { get; set; }
        public bool Inativo { get; set; }

        public virtual ICollection<Atividade> Atividades { get; set; }
        public virtual ICollection<TipoPacto> TiposPacto { get; set; }
        public virtual ICollection<OS_GrupoAtividade> ListaOsGrupoAtividades { get; set; }

        public ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
