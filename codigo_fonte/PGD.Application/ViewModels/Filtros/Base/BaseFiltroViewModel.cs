namespace PGD.Application.ViewModels.Filtros.Base
{
    public abstract class BaseFiltroViewModel
    {
        public int? Id { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public bool OrdenarDescendente { get; set; }
        public string OrdenarPor { get; set; }
    }
}