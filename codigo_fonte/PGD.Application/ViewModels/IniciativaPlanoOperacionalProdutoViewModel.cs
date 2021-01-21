using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class IniciativaPlanoOperacionalProdutoViewModel
    {
        public string IdIniciativaPlanoOperacional { get; set; }
        public IniciativaPlanoOperacionalViewModel IniciativaPlanoOperacional { get; set; }
        public int IdProduto { get; set; }
        public ProdutoViewModel Produto { get; set; }
    }
}
