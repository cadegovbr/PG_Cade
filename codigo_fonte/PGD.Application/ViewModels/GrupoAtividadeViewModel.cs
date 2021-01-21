using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class GrupoAtividadeViewModel
    {

        public int IdGrupoAtividade { get; set; }
        public int IdGrupoAtividadeOriginal { get; set; }

        [Required(ErrorMessage = "O campo Nome Grupo é de preenchimento obrigatório!")]
        [StringLength(500)]
        [Display(Name = "Nome do Grupo")]
        public string NomGrupoAtividade { get; set; }

        [Required(ErrorMessage = "O campo Atividades é de preenchimento obrigatório!")]
        public List<int> idsAtividades { get; set; }
        public ICollection<AtividadeViewModel> Atividades { get; set; }
        public List<TipoPactoViewModel> TiposPacto { get; set; }

        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }

        public UsuarioViewModel Usuario { get; set; }
        [Required(ErrorMessage = "É necessário selecionar pelo menos um tipo de plano para o grupo de atividades.")]
        public List<int> IdsTipoPacto { get; set; }
    }
}
