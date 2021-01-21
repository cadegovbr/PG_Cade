using System.Collections.Generic;

namespace PGD.Domain.Entities.RH
{
    public class Unidade
    {
        #region CSU0008_RN050
        public int IdUnidade { get; set; }
        public string Nome { get; set; }
        #endregion
        public string Sigla { get; set; }
        public string Codigo { get; set; }
        public bool Excluido { get; set; }
        public int? IdUnidadeSuperior { get; set; }
        public virtual Unidade UnidadeSuperior { get; set; }
        public virtual ICollection<Unidade> Unidades { get; set; }
        public virtual ICollection<UsuarioPerfilUnidade> UsuariosPerfisUnidades { get; set; }
        public virtual ICollection<Unidade_TipoPacto> UnidadesTiposPactos { get; set; }
    }
}
