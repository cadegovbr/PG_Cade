using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class FeriadoViewModel
    {
        public DateTime data_feriado { get; set; }
        public string duracao { get; set; }
        public bool EhFeriado { get; set; }
        public string descricao { get; set; }
    }
}
