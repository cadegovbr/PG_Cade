using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class RelatorioFolhaPontoSearchViewModel
    {
        public bool IsDirigente { get; set; }
        public string NomeServidor { get; set; }
        public string CpfServidor { get; set; }
        public string NomeUnidade { get; set; }
        public int IdUnidade { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }


    }
}
