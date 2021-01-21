using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Entities
{
    public class ParametroSistema
    {
        public int IdParametroSistema { get; set; }

        public string DescParametroSistema { get; set; }

        public string Valor { get; set; }

        public int? IntValue => !string.IsNullOrEmpty(Valor) ? Convert.ToInt32(Valor) : new Nullable<int>();

        public bool? BoolValue => !string.IsNullOrEmpty(Valor) ? ( Valor.Equals("sim") ? true : false ) : new Nullable<bool>();

    }
}
