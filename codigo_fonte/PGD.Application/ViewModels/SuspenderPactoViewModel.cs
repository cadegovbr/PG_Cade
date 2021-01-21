using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class SuspenderPactoViewModel
    {

        public int IdPacto { get; set; }
        public DateTime DataInicioPacto { get; set; }
        public double CargaHorariaTotalPacto { get; set; }
        public TimeSpan CargaHorariaDiaria { get; set; }
        public string Motivo { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de início da suspensão")]
        public DateTime? SuspensoAPartirDe { get; set; }
        [Display(Name = "Data de Reinício:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SuspensoAte { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        [Display(Name = "Horas do plano de trabalho a serem mantidas para o dia:")]
        public TimeSpan HorasMantidasParaDataSuspensao { get; set; }

        public string CPFUsuario { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data prevista para término do plano de trabalho")]
        public DateTime DataTerminoPacto { get; set; }
        public bool PodeEditar { get; set; }

        public string Mensagem { get; set; }
        public string ClasseMensagem { get; set; }

    }
}
