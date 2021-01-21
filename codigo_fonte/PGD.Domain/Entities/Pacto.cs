using DomainValidation.Validation;
using PGD.Domain.Validations.Pactos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PGD.Domain.Entities
{
    public class Pacto
    { 
        public int IdPacto { get; set; }
        public string CpfUsuario { get; set; }
        public string Nome { get; set; } 
        public string MatriculaSIAPE { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Selecione a unidade de exercício do servidor para este plano de trabalho")]
        public int UnidadeExercicio { get; set; }//RN050
        public string TelefoneFixoServidor { get; set; }
        public string TelefoneMovelServidor { get; set; }
        public int IdOrdemServico { get; set; }
        public OrdemServico OrdemServico { get; set; }
        public bool PactoExecutadoNoExterior { get; set; }        
        public String ProcessoSEI { get; set; }
        public bool PossuiCargaHoraria { get; set; }
        public DateTime DataPrevistaInicio { get; set; }
        public DateTime DataPrevistaTermino { get; set; }
        public double CargaHoraria { get; set; }
        public double CargaHorariaTotal { get; set; }
        //public int SituacaoPacto { get; set; }//RN051 // TODO - RETIRAR APÓS COMPLETAR
        [ForeignKey("SituacaoPacto")]
        public int IdSituacaoPacto { get; set; }
        public virtual SituacaoPacto SituacaoPacto { get; set; }

        public virtual ICollection<Produto> Produtos { get; set; }
        public virtual ICollection<Cronograma> Cronogramas { get; set; }

        [MaxLength]
        [Column(TypeName = "varchar(MAX)")]
        public string Motivo { get; set; }//CSU007

        public DateTime? SuspensoAPartirDe { get; set; }//CSU007
        public DateTime? SuspensoAte { get; set; }//CSU007

        public int? EntregueNoPrazo { get; set; }
        
        public DateTime? DataTerminoReal { get; set; }

        public DateTime? DataInterrupcao { get; set; }
        public int IdTipoPacto { get; set;}
        public virtual TipoPacto TipoPacto { get; set; }

        [MaxLength(500)]        
        public String TAP { get; set; }
        public virtual ICollection<Historico> Historicos { get; set; }

        public string CpfUsuarioSolicitante { get; set; }
        public int? StatusAprovacaoSolicitante { get; set; }
        public DateTime? DataAprovacaoSolicitante { get; set; }

        public string CpfUsuarioDirigente { get; set; }
        public int? StatusAprovacaoDirigente { get; set; }
        public DateTime? DataAprovacaoDirigente { get; set; }

        public string CpfUsuarioCriador { get; set; }

        public bool IndVisualizacaoRestrita { get; set; }
        public string JustificativaVisualizacaoRestrita { get; set; }

        [NotMapped]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            ValidationResult = new PactoValidation().Validate(this);

            return ValidationResult.IsValid;
        }

        [NotMapped]
        public DateTime? DataUltimaAvaliacaoParcial => Produtos?.SelectMany(p => p.Avaliacoes)?.OrderBy(p => p.DataAvaliacao).LastOrDefault()?.DataAvaliacao;

        public Pacto()
        {
            ValidationResult = new DomainValidation.Validation.ValidationResult();
        }
         
    }
}

