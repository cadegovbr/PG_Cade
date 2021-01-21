using PGD.Application.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class NotaAvaliacaoViewModel
    {
        public NotaAvaliacaoViewModel()
        {
            
        }
        public int IdNotaAvaliacao { get; set; }

        [Display(Name = "Tipo do Plano de Trabalho")]
        [StringLength(20,ErrorMessage ="O campo descrição da nota deve conter no máximo 20 caracteres.")]
        public string DescNotaAvaliacao { get; set; }

        public bool IndAtivoAvaliacaoSimplificada { get; set; }

        public bool IndAtivoAvaliacaoDetalhada { get; set; }

        public decimal ValorNotaFinal { get; set; }

        public int Conceito { get; set; }

        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
    }
}
