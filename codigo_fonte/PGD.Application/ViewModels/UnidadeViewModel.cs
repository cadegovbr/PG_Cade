using System.Collections.Generic;

namespace PGD.Application.ViewModels
{
    public class UnidadeViewModel
    {
        public int IdUnidade { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public bool Excluido { get; set; }
        public int? IdUnidadeSuperior { get; set; }
        public virtual UnidadeViewModel UnidadeSuperior { get; set; }
        public virtual ICollection<UnidadeViewModel> Unidades { get; set; }
        public virtual ICollection<UsuarioPerfilUnidadeViewModel> UsuariosPerfisUnidades { get; set; }
        public virtual ICollection<Unidade_TipoPactoViewModel> UnidadesTiposPactos { get; set; }
    }
}