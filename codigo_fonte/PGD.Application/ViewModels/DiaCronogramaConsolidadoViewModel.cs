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
    public class DiaCronogramaConsolidadoViewModel
    {
        public DiaCronogramaConsolidadoViewModel()
        {
        }

        public string IdsPacto { get; set; }
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

        public TimeSpan HorasCronograma { get; set; }



    }
}
