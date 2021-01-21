using System.Collections.Generic;

namespace PGD.Domain.Entities
{
    public class Permissao
    {
        public int IdPermissao { get; set; }
        public string Descricao { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public virtual ICollection<PermissaoPerfil> PermissoesPerfil { get; set; }
    }
}