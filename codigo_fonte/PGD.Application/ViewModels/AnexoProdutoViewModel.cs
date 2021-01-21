using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class AnexoProdutoViewModel
    {
        public AnexoProdutoViewModel()
        {

        }

        public int IdAnexoProduto { get; set; }

        public string Nome { get; set; }

        public string Tamanho { get; set; }

        public string Tipo { get; set; }

        public int IdProduto { get; set; }

    }
}
