using PGD.Domain.Entities.RH;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class RelatorioAlSimultaneaViewModel
    {
        public int? IdPacto { get; set; }
        public string Nome { get; set; }
        public string NomeServidor { get; set; }
        public string NomeUnidade { get; set; }
        public string CpfUsuario { get; set; }
        public int? UnidadeId { get; set; }
        public List<Unidade> lstUnidade { get; set; }
        public List<PactoViewModel> lstPactos { get; set; }
        public List<CronogramaViewModel> listCronogramas { get; set; }
        public List<DateTime> listDates { get; set; }
        public List<QuantidadeServidorViewModel> lisQtdServidor { get; set; }
        public SearchFlPontoViewModel searchSimultanea { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DataInicial { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DataFinal { get; set; }

        public RelatorioAlSimultaneaViewModel()
        {

            Nome = "";
            DataInicial = null;
            DataFinal = null;

        }
    }
}
