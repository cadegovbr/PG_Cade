using PGD.Domain.Entities.RH;

namespace PGD.Domain.Entities
{
    public class UsuarioPerfilUnidade
    {
        public UsuarioPerfilUnidade()
        {
            ValidationResult = new DomainValidation.Validation.ValidationResult();
        }

        public long Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }
        public int IdUnidade { get; set; }
        public bool Excluido { get; set; }
        public virtual Usuario.Usuario Usuario { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual Unidade Unidade { get; set; }

        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
    }
}