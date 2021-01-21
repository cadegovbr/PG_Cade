using PGD.Application.Util;
using PGD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;

namespace PGD.Application.ViewModels
{
    public class PactoViewModel
    {
        public PactoViewModel()
        {
            Produtos = new List<ProdutoViewModel>();
            Historicos = new List<HistoricoViewModel>();
            ValidationResult = new DomainValidation.Validation.ValidationResult();
        }
        [Display(Name = "Código do Plano de Trabalho")]
        public int IdPacto { get; set; }
        public int IdTipoPacto { get; set; }
        public TipoPactoViewModel TipoPacto { get; set; }
        public string CpfUsuario { get; set; }
        [Display(Name ="Nome")]
        [Required(ErrorMessage = "O campo Nome é de preenchimento obrigatório!")]
        public string Nome { get; set; }
        public string MatriculaSIAPE { get; set;}

        [Range(1, Int32.MaxValue, ErrorMessage = "Selecione a unidade de exercício do servidor para este plano de trabalho")]
        public int UnidadeExercicio { get; set; }
        public string UnidadeDescricao { get; set; }
        public string Acao { get; set; }
        public string TelefoneFixoServidor { get; set; }
        public string TelefoneMovelServidor { get; set; }
        public int IdOrdemServico { get; set; }
        public virtual OrdemServicoViewModel OrdemServico { get; set; }
        public int IdSituacaoPacto { get; set; }
        public int IdSituacaoPactoAnteriorAcao { get; set; }
        [Display(Name = "Termo de Abertura do Projeto")]
        public String TAP { get; set; }
        public SituacaoPactoViewModel SituacaoPacto { get; set; }
        [Required(ErrorMessage = "O campo Possui Carga Horária é de preenchimento obrigatório!")]
        public bool? PossuiCargaHoraria { get; set; }
        [Required(ErrorMessage = "O campo Possui Carga Horária é de preenchimento obrigatório!")]
        public bool? PactoExecutadoNoExterior { get; set; }
        public bool? UnidadeUsuarioPermitePactoExecucaoNoExterior { get; set; }
        [Display(Name = "Número do Processo SEI de autorização")]
        public String ProcessoSEI { get; set; }

        [Display(Name = "Data Prevista de Início")]
        public DateTime DataPrevistaInicio { get; set; }
        [Display(Name = "Data Prevista de Término")]
        public DateTime? DataPrevistaTermino { get; set; }
        public DateTime? DataPrevistaTerminoAntesSuspensao { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan CargaHorariaDiaria{ get; set; }
        
        [Display(Name = "Carga Horária Total")]
        public double CargaHorariaTotal { get; set; }

        [Required(ErrorMessage = "É necessário informar o produto que será produzido nesse Plano de Trabalho")]
        public List<ProdutoViewModel> Produtos { get; set; }
        public List<HistoricoViewModel> Historicos { get; set; }        
        public List<AvaliacaoProdutoViewModel> Avaliacoes => Produtos?.SelectMany(p => p.Avaliacoes)?.ToList();
        public int? EntregueNoPrazo { get; set; }
        public string Justificativa { get; set; }            
        
        public DateTime? DataTerminoReal { get; set; }
        public DateTime? DataInterrupcao { get; set; }
        [Display(Name = "Horas do plano de trabalho a serem mantidas para o dia")]
        public TimeSpan HorasMantidasParaDataInterrupcao { get; set; }

        public eAvaliacao eInterromper{ get; set; }
        public eJustificativa eJustificativa { get; set; }
        public List<CronogramaViewModel> Cronogramas { get; set; }

        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }

        public string Motivo { get; set; }
        public DateTime? SuspensoAPartirDe { get; set; }
        public DateTime? SuspensoAte { get; set; }


        public string CpfUsuarioSolicitante { get; set; }
        public int? StatusAprovacaoSolicitante { get; set; }
        public DateTime? DataAprovacaoSolicitante { get; set; }

        public string CpfUsuarioDirigente { get; set; }
        public int? StatusAprovacaoDirigente { get; set; }
        public DateTime? DataAprovacaoDirigente { get; set; }

        public string CpfUsuarioCriador { get; set; }

        public int StatusAssinatura { get; set; }

        [Display(Name = "Visualização Restrita?")]
        public bool IndVisualizacaoRestrita { get; set; }
        [Display(Name = "Justificativa")]
        public string JustificativaVisualizacaoRestrita { get; set; }

        public bool podeEditar { get; set; }
        public bool podeDeletar { get; set; }
        public bool podeAvaliar { get; set; }
        public bool podeInterromper { get; set; }
        public bool podeSuspender { get; set; }
        public bool podeVoltarSuspensao { get; set; }
        public bool podeAssinar { get; set; }
        public bool podeNegar { get; set; }
        public bool semAcao { get; set; }
        public bool podeEditarAndamento { get; set; }
        public bool podeVisualizarPactuadoAvaliado { get; set; }
        public bool podeCancelarAvaliacao { get; set; }
        public bool podeRestringirVisibilidadePacto { get; set; }
        public bool podeVisualizar { get; set; }
        public bool possuiAvaliacoes => Avaliacoes?.Count > 0;
        public bool ehPGDProjeto { get; set; }

        public bool modoSomenteLeitura { get; set; }

        public int IdOrigemAcao { get; set; }

        public string StrCargaHorariaTotal =>  $"{(int)TimeSpan.FromHours((double)CargaHorariaTotal).TotalHours}:{TimeSpan.FromHours((double)CargaHorariaTotal).Minutes.ToString("00")}";
        public string StrDataPrevistaInicio => DataPrevistaInicio.ToString("dd/MM/yyyy");
        public string StrDataPrevistaTermino => DataPrevistaTermino?.ToString("dd/MM/yyyy");

        public string StrCargaHorariaHomologada => Utilitarios.FormatarParaHoras(CargaHorariaTotalHomologada);
        [Display(Name = "Carga Horária Total Avaliada")]
        public double CargaHorariaTotalHomologada => Avaliacoes?.Sum(a => a.CargaHorariaAvaliada) ?? 0;

        public string SituacaoPactoDescricao
        {

            get
            {
                String desc = SituacaoPacto.DescSituacaoPacto;
                if (IdSituacaoPacto != (int)eSituacaoPacto.PendenteDeAssinatura)
                {
                    return desc;
                }
                else
                {
                    if (this.StatusAprovacaoDirigente == null && this.StatusAprovacaoSolicitante == null)
                    {
                        desc += " (Todas)";
                    }
                    else if (this.StatusAprovacaoDirigente == null && this.StatusAprovacaoSolicitante == 1)
                    {
                        desc += " (Chefia)";
                    }
                    else if (this.StatusAprovacaoDirigente == 1 && this.StatusAprovacaoSolicitante == null)
                    {
                        desc += " (Servidor)";
                    }
                }

                return desc;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            PactoViewModel pactoComparado = (PactoViewModel)obj;

            if (pactoComparado.DataPrevistaInicio != DataPrevistaInicio) return false;
            if (pactoComparado.DataPrevistaTermino != DataPrevistaTermino) return false;
            if (pactoComparado.Cronogramas == null && Cronogramas != null) return false;
            if (pactoComparado.Cronogramas != null && Cronogramas == null) return false;
            if (pactoComparado.Cronogramas.Min(c => c.DataCronograma) != Cronogramas.Min(c => c.DataCronograma)) return false;
            if (pactoComparado.Cronogramas.Max(c => c.DataCronograma) != Cronogramas.Max(c => c.DataCronograma)) return false;

            return true;
        }

    }
}


