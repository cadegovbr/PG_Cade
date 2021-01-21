using System.ComponentModel.DataAnnotations;

namespace PGD.Domain.Enums
{
    public enum Perfil
    {
        Nenhum = 0,
        Solicitante = 1,
        Dirigente = 2,
        Administrador = 3,
        Consulta = 4,
    }

    #region CSU008_RN051
    public enum eSituacaoPacto
    {
        [Display(Name = "Pendente de Assinatura")]
        PendenteDeAssinatura = 1,
        [Display(Name = "A Iniciar")]
        AIniciar = 2,
        [Display(Name = "Em Andamento")]
        EmAndamento = 3,
        [Display(Name = "Pendente de Avaliação")]
        PendenteDeAvaliacao = 4,
        [Display(Name = "Avaliado")]
        Avaliado = 5,
        [Display(Name = "Negado")]
        Negado = 6,
        [Display(Name = "Excluído")]
        Excluido = 7,
        [Display(Name = "Interrompido")]
        Interrompido = 8,
        [Display(Name = "Suspenso")]
        Suspenso = 9,
        [Display(Name = "Avaliado Parcialmente")]
        AvaliadoParcialmente = 10

        //- Pendente de assinatura: faltam as assinaturas;
        //- A iniciar: plano já assinado, mas a data atual é inferior a data de início;
        //- Em andamento: a data atual está entre a data de início e de término do plano (plano em vigência);
        //- Pendente de avaliação: a data atual é superior a data de término e o plano ainda não foi avaliado ou interrompido;
        //ou o plano foi finalizado por decurso de prazo e está pendente de avaliação do chefe .
        //- Avaliado: planos já avaliados;
        //- Negado: o plano proposto foi negado pelo dirigente;
        //- Excluído: o plano foi excluído – opção possível apenas antes do início do plano;
        //- Interrompido: o plano foi interrompido pelo dirigente;
        //- Suspenso: o plano está suspenso na data atual, pelo dirigente.        
    }
    #endregion

    #region CSU007
    public enum eAssinatura
    {
        AssinadoPorTodos = 1,
        AssinadoPeloSolicitante = 2,
        NaoAssinado = 0
    }
    #endregion

    #region CSU013

    public enum eAvaliacao
    {
        [Display(Name ="Insatisfatório")]
        Insatisfatorio = 1,
        Regular = 2,
        Bom = 3,
        [Display(Name = "Muito Bom")]
        MuitoBom = 4,
        Excelente = 5,
        [Display(Name ="Não Entregue")]
        NaoEntregue = 6
    }

    #endregion

    public enum eAcaoPacto
    {
        Criando = 0,
        Assinando = 1,
        Interrompendo = 2,
        VoltandoSuspensao = 3,
        Suspendendo = 4,
        Avaliando = 5,
        Negando = 6,
        Excluindo = 7,
        Editando = 8,
        SemAcao = 9,
        Iniciando = 10,
        CancelandoAvaliacao = 11,        
        Finalizando = 12,
        AvaliandoParcialmente = 13,
        CancelandoAvaliacaoParcialmente = 14,
        ReativandoAutomaticamente = 15
    }

    public enum eTipoAvaliacao
    {
        Parcial = 1,
        Total = 2
    }

    public enum eNivelAvaliacao
    {
        Simplificada = 1,
        Detalhada = 2
    }

    public enum eJustificativa
    {
        [Display(Name = "Produto Não entregue")]
        NaoEntregue = 1,
        [Display(Name = "Entregue com atraso")]
        ComAtraso = 2,
        [Display(Name = "Atividade Cancelada")]
        Cancelada = 3
    }

    public enum eOrigem
    {
        [Display(Name = "Solicitação de Plano de Trabalho")]
        SolicitacaoPacto = 1,
        [Display(Name = "Listagem de Planos de Trabalho")]
        Listagem = 2
    }

    public enum eParametrosSistema
    {
        [Display(Name = "Bloquear novos planos de trabalho se houver planos de trabalho suspensos para o servidor? (Sim/Não)")]
        BloquearSeHouverPactosSuspensos = 1,

        [Display(Name = "Bloquear novos planos de trabalho se houver planos de trabalho pendentes de assinatura do servidor? (Sim/Não)")]
        BloquearSeHouverPactosPendentesDeAssinatura = 2,

        [Display(Name = "Bloquear novos planos de trabalho se houver planos de trabalho pendentes de avaliação do servidor? (Sim/Não)")]
        BloquearSeHouverPactosPendentesDeAvaliacao = 3,

        [Display(Name = "Tempo máximo de planos de trabalho da unidade em situação 'Pendente de assinatura' para início do bloqueio")]
        TempoMaximoPendenteAssinatura = 4,

        [Display(Name = "Prazo, em dias, para bloqueio de novo planos de trabalho após avaliação parcial (sem conclusão do plano de trabalho)")]
        PrazoBloqueioAvaliacaoParcial = 5,

        [Display(Name = "Prazo, em dias, para bloqueio de novo plano de trabalho após término do plano de trabalho (sem avaliação final)")]
        PrazoBloqueioPactoAposTermino = 6,

        QuantidadesDiaRetroagirInterrupcao = 7,

        [Display(Name = "Prazo, em dias, para bloquear novos planos de trabalho se houver planos de trabalho em andamento sem avaliação parcial ")]
        PrazoBloqueioPactoEmAndamentoSemAvaliacaoParcial = 8,

        QuantidadesDiaConclusaoAntecipada = 9

    }

    public enum eTipoPacto
    {
        [Display(Name = "PGD - Pontual")]
        PGD_Pontual = 1,
        [Display(Name = "PGD - Projetos")]
        PGD_Projeto = 2
    }

    public enum Operacao
    {
        Inclusão,
        Inclusão_Pela_Chefia,
        Alteração,
        Exclusão,
        Suspensão,
        Interrupção,
        Assinatura,
        Avaliacao,
        AvaliacaoParcial,
        VoltandoSuspensão
    }
}
