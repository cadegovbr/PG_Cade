using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class RelatorioAtividadesPgdViewModel
    {
        public int IdRltAtividadePgd { get; set; }
        public string nomeUnidade { get; set; }
        public int QtdServPgd { get; set; }
        public int QtdPctCelebrados { get; set; }
        public int QtdPctEntreguePrazo { get; set; }
        public decimal PercentEntreguePz { get; set; }
    }
}
