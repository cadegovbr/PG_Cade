using PGD.Application.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class CronogramaPactoViewModel
    {
        public CronogramaPactoViewModel()
        {
            Cronogramas = new List<CronogramaViewModel>();
            Mensagens = new List<string>();
            ValidarHorasADistribuir = true;
        }

        public int IdPacto { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime? DataInicioSuspensao { get; set; }
        public DateTime? DataFimSuspensao { get; set; }
        public DateTime? DataInicioImpedimento { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan HorasDiarias { get; set; }
        public double HorasTotais { get; set; }
        public bool PodeEditar { get; set; }
        public bool PodeRemoverDias { get; set; }
        public string SalvarCallbackFunction { get; set; }
        public string FecharCallbackFunction { get; set; }
        public string CPFUsuario { get; set; }

        public bool ValidarHorasADistribuir { get; set; }

        

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan CargaHorariaTotal => TimeSpan.FromHours(HorasTotais);

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan CargaHorariaDistribuir => TimeSpan.FromHours( CargaHorariaTotal.TotalHours - (Cronogramas.Sum(c => c.HorasCronograma.TotalHours)));

        public List<CronogramaViewModel> Cronogramas { get; set; }

        public string StrQuantidadeHorasDiasSuspensao { get; set; }

        public TimeSpan QuantidadeHorasDiasSuspensao => !string.IsNullOrEmpty(StrQuantidadeHorasDiasSuspensao) ? TimeSpan.Parse(StrQuantidadeHorasDiasSuspensao) : TimeSpan.Zero;

        public bool CalcularCronogramaAPartirBanco { get; set; }

        public List<string> Mensagens { get; set; }

        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
    }
}
