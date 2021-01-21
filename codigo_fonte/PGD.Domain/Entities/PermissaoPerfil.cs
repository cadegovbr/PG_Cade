namespace PGD.Domain.Entities
{
    public class PermissaoPerfil
    {
        public int IdPermissao { get; set; }
        public int IdPerfil { get; set; }
        public virtual Permissao Permissao { get; set; }
        public virtual Perfil Perfil { get; set; }
    }
}