using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PGD.Application.ViewModels
{
    public class SearchUsuarioViewModel : IValidatableObject
    {
        public string MatriculaUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public int? IdUnidade { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(MatriculaUsuario) && string.IsNullOrWhiteSpace(NomeUsuario) && (!IdUnidade.HasValue || IdUnidade == 0))
                yield return new ValidationResult("Ao menos um filtro deve ser informado", new[] { "" });
        }
    }
}