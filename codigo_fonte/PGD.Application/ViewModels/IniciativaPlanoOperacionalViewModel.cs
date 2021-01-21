using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class IniciativaPlanoOperacionalViewModel
    {
        public string IdIniciativaPlanoOperacional { get; set; }
        public string DescIniciativaPlanoOperacional { get; set; }

        public string CodigoDescricao => IdIniciativaPlanoOperacional + " - " + DescIniciativaPlanoOperacional;
        public decimal CodigoDecimal => Convert.ToDecimal(IdIniciativaPlanoOperacional.Replace('.', ','));
    }
}
