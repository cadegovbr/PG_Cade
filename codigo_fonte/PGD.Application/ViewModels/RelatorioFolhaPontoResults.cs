using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class RelatorioFolhaPontoResultsViewModel
    {
        public RelatorioFolhaPontoSearchViewModel Servidor { get; set; }
        public IEnumerable<DiaCronogramaConsolidadoViewModel> ListCronogramas { get; set; }
    }
}
