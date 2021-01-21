using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Entities.RH
{
    public class Feriado
    {
        public int ID { get; set; }
        public DateTime data_feriado { get; set; }
        public string descricao { get; set; }
        public int? id_localidade { get; set; }
        public int? id_unidade_federativa { get; set; }
        public string categoria { get; set; }
        public int? id_municipio { get; set; }
        public string duracao { get; set; }
    }
}
