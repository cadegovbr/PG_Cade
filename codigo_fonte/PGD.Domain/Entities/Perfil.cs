
using System.Collections.Generic;

namespace PGD.Domain.Entities
{
    public class Perfil
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<UsuarioPerfilUnidade> UsuariosPerfisUnidades { get; set; }
        // public virtual ICollection<PermissaoPerfil> PermissoesPerfil { get; set; }
    }
}
