using System;
using System.ComponentModel.DataAnnotations;

namespace PGD.Application.ViewModels
{
    public class RetornoSuspensaoViewModel2
    {
        public RetornoSuspensaoViewModel2()
        {
        }

        public int IdPacto { get; set; }

        public DateTime DataInicioPacto { get; set; }

        public double CargaHorariaTotalPacto { get; set; }

        public int CargaHorariaDiaria { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de início da suspensão")]
        public DateTime DataInicioSuspensao { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de fim da suspensão")]
        public DateTime? DataFimSuspensao { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data prevista para término do plano de trabalho")]
        public DateTime? DataTerminoPacto { get; set; }

        public bool PodeEditar { get; set; }
        public DateTime? DataInicioImpedimento { get; set; }
        public string CPFUsuario { get; set; }

    }
}
