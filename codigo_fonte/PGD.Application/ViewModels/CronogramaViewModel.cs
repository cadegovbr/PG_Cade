using PGD.Application.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class CronogramaViewModel
    {
        public CronogramaViewModel()
        {
            //ValidationResult = new DomainValidation.Validation.ValidationResult();
        }

        public int IdCronograma { get; set; }
        public int IdPacto { get; set; }
        public DateTime DataCronograma { get; set; }
        public string DataString => DataCronograma.ToString("dd/MM/yyyy");

        public string DiaCronograma {
            get
            {
                var culture = new System.Globalization.CultureInfo("pt-BR");
                var day = culture.TextInfo.ToTitleCase( culture.DateTimeFormat.GetAbbreviatedDayName(DataCronograma.DayOfWeek));
                return day;
            }
        }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        [Display(Name ="Carga Horária Diária")]
        [Required(ErrorMessage = "O campo Carga Horária Diária é obrigatório. Para dias sem carga horária, favor inserir o valor 00:00.")]
        public TimeSpan HorasCronograma { get; set; }

        public bool DiaUtil { get; set; }
        public bool Feriado { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan DuracaoFeriado { get; set; }
 
        public bool Suspenso { get; set; }

        public bool PodeEditar { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan HorasUsadasPorOutroPacto { get; set; }

    }
}
