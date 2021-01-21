using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class RelatorioConsolidadoViewModel
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "O período é de preenchimento obrigatório!")]
        public DateTime? DataInicial { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "O período é de preenchimento obrigatório!")]
        public DateTime? DataFinal { get; set; }

        public string TextoInicialData { get; set; }
        public List<RelatorioAtividadesPgdViewModel> listaAtividadesPgd { get; set; }
        public List<ProdutoViewModel> listProdutosPgd { get; set; }
        public List<GrupoAtividadeViewModel> listGrupoAtividades { get; set; }

        public RelatorioConsolidadoViewModel()
        {
            DataInicial = null;
            DataFinal = null;
        }
    }
}
