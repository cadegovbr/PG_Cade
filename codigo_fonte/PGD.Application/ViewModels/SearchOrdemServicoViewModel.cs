using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class SearchOrdemServicoViewModel
    {
        public int? IdOrdemServico { get; set; }
        public string Descricao { get; set; }
        [Display(Name = "Data Inicial")]
        public DateTime DataInicial { get; set; }
        [Display(Name = "Data Final")]
        public DateTime DataFinal { get; set; }

        public SearchOrdemServicoViewModel()
        {
            Descricao = "";
            DataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DataFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        }
    }
}
