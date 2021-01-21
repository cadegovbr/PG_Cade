using PGD.Domain.Entities;
using PGD.Domain.Entities.RH;
using PGD.Domain.Enums;
using PGD.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class SearchPactoViewModel
    {
        
        [Display(Name = "Código do Plano de Trabalho")]
        public int? IdPacto { get; set; }
        public string NomeServidor { get; set; }
        public int? UnidadeId { get; set; }
        public bool ObterPactosUnidadesSubordinadas { get; set; }
        public List<Unidade> lstUnidade { get; set; }
        public List<SituacaoPacto> lstSituacaoPacto { get; set; }
        public int? idSituacao { get; set; }
        public int? idTipoPacto { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DataInicial { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DataFinal { get; set; }

        public SearchPactoViewModel()
        {

            DataInicial =null;
            DataFinal = null;

        }
    }
}
