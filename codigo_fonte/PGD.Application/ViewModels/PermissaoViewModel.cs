namespace PGD.Application.ViewModels
{
    public class PermissaoViewModel
    {
        public int IdPermissao { get; set; }
        public int IdPerfil { get; set; }
        public string Descricao { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}