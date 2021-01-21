using DomainValidation.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Entities
{
    public class OS_GrupoAtividade
    {
        public OS_GrupoAtividade()
        {
            ValidationResult = new ValidationResult();
        }

        public int IdGrupoAtividade { get; set; }
        public int IdGrupoAtividadeOriginal { get; set; }

        public string NomGrupoAtividade { get; set; }
        public bool Inativo { get; set; }

        public int IdOrdemServico { get; set; }
        public virtual OrdemServico OrdemServico { get; set; }
        public virtual GrupoAtividade GrupoAtividade { get; set; }

        public virtual ICollection<OS_Atividade> Atividades { get; set; }
        public virtual ICollection<TipoPacto> TiposPacto { get; set; }

        public ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
}
