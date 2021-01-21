namespace PGD.Infra.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class criacao_banco : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrador",
                c => new
                    {
                        IdAdministrador = c.Int(nullable: false, identity: true),
                        CPF = c.String(nullable: false, maxLength: 14, unicode: false),
                    })
                .PrimaryKey(t => t.IdAdministrador);
            
            CreateTable(
                "dbo.Atividade",
                c => new
                    {
                        IdAtividade = c.Int(nullable: false, identity: true),
                        NomAtividade = c.String(nullable: false, maxLength: 1000, unicode: false),
                        PctMinimoReducao = c.Int(nullable: false),
                        Inativo = c.Boolean(nullable: false),
                        DescLinkAtividade = c.String(maxLength: 300, unicode: false),
                    })
                .PrimaryKey(t => t.IdAtividade);
            
            CreateTable(
                "dbo.GrupoAtividade",
                c => new
                    {
                        IdGrupoAtividade = c.Int(nullable: false, identity: true),
                        NomGrupoAtividade = c.String(nullable: false, maxLength: 500, unicode: false),
                        Inativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdGrupoAtividade);
            
            CreateTable(
                "dbo.TipoPacto",
                c => new
                    {
                        IdTipoPacto = c.Int(nullable: false, identity: true),
                        DescTipoPacto = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.IdTipoPacto);
            
            CreateTable(
                "dbo.OS_GrupoAtividade",
                c => new
                    {
                        IdGrupoAtividade = c.Int(nullable: false, identity: true),
                        IdGrupoAtividadeOriginal = c.Int(nullable: false),
                        NomGrupoAtividade = c.String(nullable: false, maxLength: 500, unicode: false),
                        Inativo = c.Boolean(nullable: false),
                        IdOrdemServico = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdGrupoAtividade)
                .ForeignKey("dbo.OrdemServico", t => t.IdOrdemServico)
                .Index(t => t.IdOrdemServico);
            
            CreateTable(
                "dbo.OS_Atividade",
                c => new
                    {
                        IdAtividade = c.Int(nullable: false, identity: true),
                        NomAtividade = c.String(nullable: false, maxLength: 1000, unicode: false),
                        PctMinimoReducao = c.Int(nullable: false),
                        Inativo = c.Boolean(nullable: false),
                        IdOS_GrupoAtividade = c.Int(nullable: false),
                        DescLinkAtividade = c.String(maxLength: 300, unicode: false),
                        Grupo_IdGrupoAtividade = c.Int(),
                    })
                .PrimaryKey(t => t.IdAtividade)
                .ForeignKey("dbo.OS_GrupoAtividade", t => t.Grupo_IdGrupoAtividade)
                .Index(t => t.Grupo_IdGrupoAtividade);
            
            CreateTable(
                "dbo.OS_TipoAtividade",
                c => new
                    {
                        IdTipoAtividade = c.Int(nullable: false, identity: true),
                        Faixa = c.String(nullable: false, maxLength: 100, unicode: false),
                        DuracaoFaixa = c.Double(nullable: false),
                        DuracaoFaixaPresencial = c.Double(nullable: false),
                        IdOS_Atividade = c.Int(nullable: false),
                        DescTextoExplicativo = c.String(maxLength: 300, unicode: false),
                        Atividade_IdAtividade = c.Int(),
                    })
                .PrimaryKey(t => t.IdTipoAtividade)
                .ForeignKey("dbo.OS_Atividade", t => t.Atividade_IdAtividade)
                .Index(t => t.Atividade_IdAtividade);
            
            CreateTable(
                "dbo.OrdemServico",
                c => new
                    {
                        IdOrdemServico = c.Int(nullable: false, identity: true),
                        DatInicioSistema = c.DateTime(nullable: false),
                        DatInicioNormativo = c.DateTime(nullable: false),
                        DescOrdemServico = c.String(nullable: false, maxLength: 1000, unicode: false),
                        Inativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdOrdemServico);
            
            CreateTable(
                "dbo.OS_CriterioAvaliacao",
                c => new
                    {
                        IdCriterioAvaliacao = c.Int(nullable: false, identity: true),
                        IdCriterioAvaliacaoOriginal = c.Int(nullable: false),
                        DescCriterioAvaliacao = c.String(nullable: false, maxLength: 100, unicode: false),
                        StrTextoExplicativo = c.String(nullable: false, maxLength: 1000, unicode: false),
                        IdOrdemServico = c.Int(nullable: false),
                        Inativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdCriterioAvaliacao)
                .ForeignKey("dbo.OrdemServico", t => t.IdOrdemServico)
                .Index(t => t.IdOrdemServico);
            
            CreateTable(
                "dbo.OS_ItemAvaliacao",
                c => new
                    {
                        IdItemAvaliacao = c.Int(nullable: false, identity: true),
                        DescItemAvaliacao = c.String(nullable: false, maxLength: 100, unicode: false),
                        ImpactoNota = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IdNotaMaxima = c.Int(nullable: false),
                        IdCriterioAvaliacao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdItemAvaliacao)
                .ForeignKey("dbo.OS_CriterioAvaliacao", t => t.IdCriterioAvaliacao)
                .ForeignKey("dbo.NotaAvaliacao", t => t.IdNotaMaxima)
                .Index(t => t.IdNotaMaxima)
                .Index(t => t.IdCriterioAvaliacao);
            
            CreateTable(
                "dbo.NotaAvaliacao",
                c => new
                    {
                        IdNotaAvaliacao = c.Int(nullable: false, identity: true),
                        DescNotaAvaliacao = c.String(nullable: false, maxLength: 20, unicode: false),
                        IndAtivoAvaliacaoSimplificada = c.Boolean(nullable: false),
                        IndAtivoAvaliacaoDetalhada = c.Boolean(nullable: false),
                        LimiteSuperiorFaixa = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LimiteInferiorFaixa = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Conceito = c.Int(),
                    })
                .PrimaryKey(t => t.IdNotaAvaliacao);
            
            CreateTable(
                "dbo.TipoAtividade",
                c => new
                    {
                        IdTipoAtividade = c.Int(nullable: false, identity: true),
                        Faixa = c.String(nullable: false, maxLength: 100, unicode: false),
                        DescTextoExplicativo = c.String(maxLength: 300, unicode: false),
                        DuracaoFaixa = c.Double(nullable: false),
                        DuracaoFaixaPresencial = c.Double(nullable: false),
                        IdAtividade = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdTipoAtividade)
                .ForeignKey("dbo.Atividade", t => t.IdAtividade)
                .Index(t => t.IdAtividade);
            
            CreateTable(
                "dbo.CriterioAvaliacao",
                c => new
                    {
                        IdCriterioAvaliacao = c.Int(nullable: false, identity: true),
                        DescCriterioAvaliacao = c.String(nullable: false, maxLength: 100, unicode: false),
                        StrTextoExplicativo = c.String(nullable: false, maxLength: 1000, unicode: false),
                        Inativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdCriterioAvaliacao);
            
            CreateTable(
                "dbo.ItemAvaliacao",
                c => new
                    {
                        IdItemAvaliacao = c.Int(nullable: false, identity: true),
                        DescItemAvaliacao = c.String(nullable: false, maxLength: 500, unicode: false),
                        ImpactoNota = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IdNotaMaxima = c.Int(nullable: false),
                        IdCriterioAvaliacao = c.Int(nullable: false),
                        Inativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdItemAvaliacao)
                .ForeignKey("dbo.CriterioAvaliacao", t => t.IdCriterioAvaliacao)
                .ForeignKey("dbo.NotaAvaliacao", t => t.IdNotaMaxima)
                .Index(t => t.IdNotaMaxima)
                .Index(t => t.IdCriterioAvaliacao);
            
            CreateTable(
                "dbo.Cronograma",
                c => new
                    {
                        IdCronograma = c.Int(nullable: false, identity: true),
                        IdPacto = c.Int(nullable: false),
                        DataCronograma = c.DateTime(nullable: false),
                        HorasCronograma = c.Double(nullable: false),
                        DiaUtil = c.Boolean(nullable: false),
                        Feriado = c.Boolean(nullable: false),
                        DuracaoFeriado = c.Double(),
                        Suspenso = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdCronograma)
                .ForeignKey("dbo.Pacto", t => t.IdPacto)
                .Index(t => t.IdPacto);
            
            CreateTable(
                "dbo.Feriado",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        data_feriado = c.DateTime(),
                        descricao = c.String(maxLength: 100, unicode: false),
                        id_localidade = c.Int(),
                        id_unidade_federativa = c.Int(),
                        categoria = c.String(maxLength: 100, unicode: false),
                        id_municipio = c.Int(),
                        duracao = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Historico",
                c => new
                    {
                        IdHistorico = c.Int(nullable: false, identity: true),
                        IdPacto = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.IdHistorico)
                .ForeignKey("dbo.Pacto", t => t.IdPacto)
                .Index(t => t.IdPacto);
            
            CreateTable(
                "dbo.Justificativa",
                c => new
                    {
                        IdJustificativa = c.Int(nullable: false, identity: true),
                        DescJustificativa = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.IdJustificativa);
            
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        IdLog = c.Int(nullable: false, identity: true),
                        CpfUsuario = c.String(nullable: false, maxLength: 100, unicode: false),
                        Data = c.DateTime(nullable: false),
                        Operacao = c.String(nullable: false, maxLength: 20, unicode: false),
                        Tabela = c.String(nullable: false, maxLength: 50, unicode: false),
                        IdTabela = c.Int(nullable: false),
                        Valores = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.IdLog);
            
            CreateTable(
                "dbo.NivelAvaliacao",
                c => new
                    {
                        IdNivelAvaliacao = c.Int(nullable: false, identity: true),
                        DescNivelAvaliacao = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.IdNivelAvaliacao);
            
            CreateTable(
                "dbo.Pacto",
                c => new
                    {
                        IdPacto = c.Int(nullable: false, identity: true),
                        CpfUsuario = c.String(maxLength: 100, unicode: false),
                        Nome = c.String(nullable: false, maxLength: 100, unicode: false),
                        MatriculaSIAPE = c.String(maxLength: 100, unicode: false),
                        UnidadeExercicio = c.Int(nullable: false),
                        TelefoneFixoServidor = c.String(maxLength: 100, unicode: false),
                        TelefoneMovelServidor = c.String(maxLength: 100, unicode: false),
                        IdOrdemServico = c.Int(nullable: false),
                        PactoExecutadoNoExterior = c.Boolean(nullable: false),
                        ProcessoSEI = c.String(maxLength: 100, unicode: false),
                        PossuiCargaHoraria = c.Boolean(nullable: false),
                        DataPrevistaInicio = c.DateTime(nullable: false),
                        DataPrevistaTermino = c.DateTime(nullable: false),
                        CargaHoraria = c.Double(nullable: false),
                        CargaHorariaTotal = c.Double(nullable: false),
                        IdSituacaoPacto = c.Int(nullable: false),
                        Motivo = c.String(unicode: false),
                        SuspensoAPartirDe = c.DateTime(),
                        SuspensoAte = c.DateTime(),
                        EntregueNoPrazo = c.Int(),
                        DataTerminoReal = c.DateTime(),
                        DataInterrupcao = c.DateTime(),
                        IdTipoPacto = c.Int(nullable: false),
                        TAP = c.String(maxLength: 500, unicode: false),
                        CpfUsuarioSolicitante = c.String(maxLength: 100, unicode: false),
                        StatusAprovacaoSolicitante = c.Int(),
                        DataAprovacaoSolicitante = c.DateTime(),
                        CpfUsuarioDirigente = c.String(maxLength: 100, unicode: false),
                        StatusAprovacaoDirigente = c.Int(),
                        DataAprovacaoDirigente = c.DateTime(),
                        CpfUsuarioCriador = c.String(maxLength: 100, unicode: false),
                        IndVisualizacaoRestrita = c.Boolean(nullable: false),
                        JustificativaVisualizacaoRestrita = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.IdPacto)
                .ForeignKey("dbo.OrdemServico", t => t.IdOrdemServico)
                .ForeignKey("dbo.SituacaoPacto", t => t.IdSituacaoPacto)
                .ForeignKey("dbo.TipoPacto", t => t.IdTipoPacto)
                .Index(t => t.IdOrdemServico)
                .Index(t => t.IdSituacaoPacto)
                .Index(t => t.IdTipoPacto);
            
            CreateTable(
                "dbo.Produto",
                c => new
                    {
                        IdProduto = c.Int(nullable: false, identity: true),
                        IdGrupoAtividade = c.Int(nullable: false),
                        IdAtividade = c.Int(nullable: false),
                        IdTipoAtividade = c.Int(nullable: false),
                        CargaHoraria = c.Int(nullable: false),
                        QuantidadeProduto = c.Int(nullable: false),
                        CargaHorariaProduto = c.Double(nullable: false),
                        Observacoes = c.String(unicode: false),
                        ObservacoesAdicionais = c.String(unicode: false),
                        IdPacto = c.Int(nullable: false),
                        Motivo = c.String(unicode: false),
                        Avaliacao = c.Int(nullable: false),
                        EntregueNoPrazo = c.Boolean(),
                        IdJustificativa = c.Int(),
                        DataTerminoReal = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdProduto)
                .ForeignKey("dbo.OS_Atividade", t => t.IdAtividade)
                .ForeignKey("dbo.OS_GrupoAtividade", t => t.IdGrupoAtividade)
                .ForeignKey("dbo.Justificativa", t => t.IdJustificativa)
                .ForeignKey("dbo.OS_TipoAtividade", t => t.IdTipoAtividade)
                .ForeignKey("dbo.Pacto", t => t.IdPacto)
                .Index(t => t.IdGrupoAtividade)
                .Index(t => t.IdAtividade)
                .Index(t => t.IdTipoAtividade)
                .Index(t => t.IdPacto)
                .Index(t => t.IdJustificativa);
            
            CreateTable(
                "dbo.AvaliacaoProduto",
                c => new
                    {
                        IdAvaliacaoProduto = c.Int(nullable: false, identity: true),
                        IdProduto = c.Int(nullable: false),
                        CPFAvaliador = c.String(nullable: false, maxLength: 11, unicode: false),
                        DataAvaliacao = c.DateTime(nullable: false),
                        QuantidadeProdutosAvaliados = c.Int(nullable: false),
                        Avaliacao = c.Int(nullable: false),
                        EntregueNoPrazo = c.Boolean(),
                        LocalizacaoProduto = c.String(unicode: false),
                        DataTerminoReal = c.DateTime(),
                        IdJustificativa = c.Int(),
                        TipoAvaliacao = c.Int(nullable: false),
                        IdNivelAvaliacao = c.Int(nullable: false),
                        NotaFinalAvaliacaoDetalhada = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IdAvaliacaoProduto)
                .ForeignKey("dbo.Justificativa", t => t.IdJustificativa)
                .ForeignKey("dbo.NivelAvaliacao", t => t.IdNivelAvaliacao)
                .ForeignKey("dbo.Produto", t => t.IdProduto)
                .Index(t => t.IdProduto)
                .Index(t => t.IdJustificativa)
                .Index(t => t.IdNivelAvaliacao);
            
            CreateTable(
                "dbo.AvaliacaoDetalhadaProduto",
                c => new
                    {
                        IdAvaliacaoDetalhadaProduto = c.Int(nullable: false, identity: true),
                        IdAvaliacaoProduto = c.Int(nullable: false),
                        IdOS_ItemAvaliacao = c.Int(nullable: false),
                        IdOS_CriterioAvaliacao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdAvaliacaoDetalhadaProduto)
                .ForeignKey("dbo.AvaliacaoProduto", t => t.IdAvaliacaoProduto)
                .ForeignKey("dbo.OS_CriterioAvaliacao", t => t.IdOS_CriterioAvaliacao)
                .ForeignKey("dbo.OS_ItemAvaliacao", t => t.IdOS_ItemAvaliacao)
                .Index(t => t.IdAvaliacaoProduto)
                .Index(t => t.IdOS_ItemAvaliacao)
                .Index(t => t.IdOS_CriterioAvaliacao);
            
            CreateTable(
                "dbo.IniciativaPlanoOperacionalProduto",
                c => new
                    {
                        IdIniciativaPlanoOperacional = c.String(nullable: false, maxLength: 5, unicode: false),
                        IdProduto = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdIniciativaPlanoOperacional, t.IdProduto })
                .ForeignKey("dbo.Produto", t => t.IdProduto)
                .Index(t => t.IdProduto);
            
            CreateTable(
                "dbo.SituacaoPacto",
                c => new
                    {
                        IdSituacaoPacto = c.Int(nullable: false, identity: true),
                        DescSituacaoPacto = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.IdSituacaoPacto);
            
            CreateTable(
                "dbo.ParametroSistema",
                c => new
                    {
                        IdParametroSistema = c.Int(nullable: false, identity: true),
                        DescParametroSistema = c.String(nullable: false, maxLength: 150, unicode: false),
                        Valor = c.String(nullable: false, maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => t.IdParametroSistema);
            
            CreateTable(
                "dbo.RecursosHumanos",
                c => new
                    {
                        IdRecursosHumanos = c.Int(nullable: false, identity: true),
                        IdUnidade = c.Int(),
                        IdPerfil = c.Int(),
                        Perfil = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdRecursosHumanos)
                .ForeignKey("dbo.Unidade", t => t.IdUnidade)
                .Index(t => t.IdUnidade);
            
            CreateTable(
                "dbo.Unidade",
                c => new
                    {
                        IdUnidade = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 100, unicode: false),
                        Codigo = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.IdUnidade);
            
            CreateTable(
                "dbo.Unidade_TipoPacto",
                c => new
                    {
                        IdUnidade_TipoPacto = c.Int(nullable: false, identity: true),
                        IdUnidade = c.Int(nullable: false),
                        IdTipoPacto = c.Int(nullable: false),
                        IndPermitePactoExterior = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdUnidade_TipoPacto)
                .ForeignKey("dbo.TipoPacto", t => t.IdTipoPacto)
                .Index(t => t.IdTipoPacto);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        IdUsuario = c.Int(nullable: false, identity: true),
                        Matricula = c.String(maxLength: 100, unicode: false),
                        CPF = c.String(maxLength: 100, unicode: false),
                        Nome = c.String(maxLength: 100, unicode: false),
                        Email = c.String(maxLength: 100, unicode: false),
                        Unidade = c.Int(),
                        NomeUnidade = c.String(maxLength: 100, unicode: false),
                        Administrador = c.Boolean(),
                        Inativo = c.Boolean(),
                    })
                .PrimaryKey(t => t.IdUsuario);
            
            CreateTable(
                "dbo.IniciativaPlanoOperacional",
                c => new
                    {
                        IdIniciativaPlanoOperacional = c.String(nullable: false, maxLength: 5, unicode: false),
                        DescIniciativaPlanoOperacional = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.IdIniciativaPlanoOperacional);
            
            CreateTable(
                "dbo.GrupoAtividade_Atividade",
                c => new
                    {
                        IdGrupoAtividade = c.Int(nullable: false),
                        IdAtividade = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdGrupoAtividade, t.IdAtividade })
                .ForeignKey("dbo.GrupoAtividade", t => t.IdGrupoAtividade)
                .ForeignKey("dbo.Atividade", t => t.IdAtividade)
                .Index(t => t.IdGrupoAtividade)
                .Index(t => t.IdAtividade);
            
            CreateTable(
                "dbo.OS_TipoPacto_GrupoAtividade",
                c => new
                    {
                        IdGrupoAtividade = c.Int(nullable: false),
                        IdTipoPacto = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdGrupoAtividade, t.IdTipoPacto })
                .ForeignKey("dbo.OS_GrupoAtividade", t => t.IdGrupoAtividade)
                .ForeignKey("dbo.TipoPacto", t => t.IdTipoPacto)
                .Index(t => t.IdGrupoAtividade)
                .Index(t => t.IdTipoPacto);
            
            CreateTable(
                "dbo.TipoPactoGrupoAtividade",
                c => new
                    {
                        IdGrupoAtividade = c.Int(nullable: false),
                        IdTipoPacto = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdGrupoAtividade, t.IdTipoPacto })
                .ForeignKey("dbo.GrupoAtividade", t => t.IdGrupoAtividade)
                .ForeignKey("dbo.TipoPacto", t => t.IdTipoPacto)
                .Index(t => t.IdGrupoAtividade)
                .Index(t => t.IdTipoPacto);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Unidade_TipoPacto", "IdTipoPacto", "dbo.TipoPacto");
            DropForeignKey("dbo.RecursosHumanos", "IdUnidade", "dbo.Unidade");
            DropForeignKey("dbo.Pacto", "IdTipoPacto", "dbo.TipoPacto");
            DropForeignKey("dbo.Pacto", "IdSituacaoPacto", "dbo.SituacaoPacto");
            DropForeignKey("dbo.Produto", "IdPacto", "dbo.Pacto");
            DropForeignKey("dbo.Produto", "IdTipoAtividade", "dbo.OS_TipoAtividade");
            DropForeignKey("dbo.Produto", "IdJustificativa", "dbo.Justificativa");
            DropForeignKey("dbo.IniciativaPlanoOperacionalProduto", "IdProduto", "dbo.Produto");
            DropForeignKey("dbo.Produto", "IdGrupoAtividade", "dbo.OS_GrupoAtividade");
            DropForeignKey("dbo.AvaliacaoProduto", "IdProduto", "dbo.Produto");
            DropForeignKey("dbo.AvaliacaoProduto", "IdNivelAvaliacao", "dbo.NivelAvaliacao");
            DropForeignKey("dbo.AvaliacaoProduto", "IdJustificativa", "dbo.Justificativa");
            DropForeignKey("dbo.AvaliacaoDetalhadaProduto", "IdOS_ItemAvaliacao", "dbo.OS_ItemAvaliacao");
            DropForeignKey("dbo.AvaliacaoDetalhadaProduto", "IdOS_CriterioAvaliacao", "dbo.OS_CriterioAvaliacao");
            DropForeignKey("dbo.AvaliacaoDetalhadaProduto", "IdAvaliacaoProduto", "dbo.AvaliacaoProduto");
            DropForeignKey("dbo.Produto", "IdAtividade", "dbo.OS_Atividade");
            DropForeignKey("dbo.Pacto", "IdOrdemServico", "dbo.OrdemServico");
            DropForeignKey("dbo.Historico", "IdPacto", "dbo.Pacto");
            DropForeignKey("dbo.Cronograma", "IdPacto", "dbo.Pacto");
            DropForeignKey("dbo.ItemAvaliacao", "IdNotaMaxima", "dbo.NotaAvaliacao");
            DropForeignKey("dbo.ItemAvaliacao", "IdCriterioAvaliacao", "dbo.CriterioAvaliacao");
            DropForeignKey("dbo.TipoAtividade", "IdAtividade", "dbo.Atividade");
            DropForeignKey("dbo.TipoPactoGrupoAtividade", "IdTipoPacto", "dbo.TipoPacto");
            DropForeignKey("dbo.TipoPactoGrupoAtividade", "IdGrupoAtividade", "dbo.GrupoAtividade");
            DropForeignKey("dbo.OS_TipoPacto_GrupoAtividade", "IdTipoPacto", "dbo.TipoPacto");
            DropForeignKey("dbo.OS_TipoPacto_GrupoAtividade", "IdGrupoAtividade", "dbo.OS_GrupoAtividade");
            DropForeignKey("dbo.OS_GrupoAtividade", "IdOrdemServico", "dbo.OrdemServico");
            DropForeignKey("dbo.OS_CriterioAvaliacao", "IdOrdemServico", "dbo.OrdemServico");
            DropForeignKey("dbo.OS_ItemAvaliacao", "IdNotaMaxima", "dbo.NotaAvaliacao");
            DropForeignKey("dbo.OS_ItemAvaliacao", "IdCriterioAvaliacao", "dbo.OS_CriterioAvaliacao");
            DropForeignKey("dbo.OS_TipoAtividade", "Atividade_IdAtividade", "dbo.OS_Atividade");
            DropForeignKey("dbo.OS_Atividade", "Grupo_IdGrupoAtividade", "dbo.OS_GrupoAtividade");
            DropForeignKey("dbo.GrupoAtividade_Atividade", "IdAtividade", "dbo.Atividade");
            DropForeignKey("dbo.GrupoAtividade_Atividade", "IdGrupoAtividade", "dbo.GrupoAtividade");
            DropIndex("dbo.TipoPactoGrupoAtividade", new[] { "IdTipoPacto" });
            DropIndex("dbo.TipoPactoGrupoAtividade", new[] { "IdGrupoAtividade" });
            DropIndex("dbo.OS_TipoPacto_GrupoAtividade", new[] { "IdTipoPacto" });
            DropIndex("dbo.OS_TipoPacto_GrupoAtividade", new[] { "IdGrupoAtividade" });
            DropIndex("dbo.GrupoAtividade_Atividade", new[] { "IdAtividade" });
            DropIndex("dbo.GrupoAtividade_Atividade", new[] { "IdGrupoAtividade" });
            DropIndex("dbo.Unidade_TipoPacto", new[] { "IdTipoPacto" });
            DropIndex("dbo.RecursosHumanos", new[] { "IdUnidade" });
            DropIndex("dbo.IniciativaPlanoOperacionalProduto", new[] { "IdProduto" });
            DropIndex("dbo.AvaliacaoDetalhadaProduto", new[] { "IdOS_CriterioAvaliacao" });
            DropIndex("dbo.AvaliacaoDetalhadaProduto", new[] { "IdOS_ItemAvaliacao" });
            DropIndex("dbo.AvaliacaoDetalhadaProduto", new[] { "IdAvaliacaoProduto" });
            DropIndex("dbo.AvaliacaoProduto", new[] { "IdNivelAvaliacao" });
            DropIndex("dbo.AvaliacaoProduto", new[] { "IdJustificativa" });
            DropIndex("dbo.AvaliacaoProduto", new[] { "IdProduto" });
            DropIndex("dbo.Produto", new[] { "IdJustificativa" });
            DropIndex("dbo.Produto", new[] { "IdPacto" });
            DropIndex("dbo.Produto", new[] { "IdTipoAtividade" });
            DropIndex("dbo.Produto", new[] { "IdAtividade" });
            DropIndex("dbo.Produto", new[] { "IdGrupoAtividade" });
            DropIndex("dbo.Pacto", new[] { "IdTipoPacto" });
            DropIndex("dbo.Pacto", new[] { "IdSituacaoPacto" });
            DropIndex("dbo.Pacto", new[] { "IdOrdemServico" });
            DropIndex("dbo.Historico", new[] { "IdPacto" });
            DropIndex("dbo.Cronograma", new[] { "IdPacto" });
            DropIndex("dbo.ItemAvaliacao", new[] { "IdCriterioAvaliacao" });
            DropIndex("dbo.ItemAvaliacao", new[] { "IdNotaMaxima" });
            DropIndex("dbo.TipoAtividade", new[] { "IdAtividade" });
            DropIndex("dbo.OS_ItemAvaliacao", new[] { "IdCriterioAvaliacao" });
            DropIndex("dbo.OS_ItemAvaliacao", new[] { "IdNotaMaxima" });
            DropIndex("dbo.OS_CriterioAvaliacao", new[] { "IdOrdemServico" });
            DropIndex("dbo.OS_TipoAtividade", new[] { "Atividade_IdAtividade" });
            DropIndex("dbo.OS_Atividade", new[] { "Grupo_IdGrupoAtividade" });
            DropIndex("dbo.OS_GrupoAtividade", new[] { "IdOrdemServico" });
            DropTable("dbo.TipoPactoGrupoAtividade");
            DropTable("dbo.OS_TipoPacto_GrupoAtividade");
            DropTable("dbo.GrupoAtividade_Atividade");
            DropTable("dbo.IniciativaPlanoOperacional");
            DropTable("dbo.Usuario");
            DropTable("dbo.Unidade_TipoPacto");
            DropTable("dbo.Unidade");
            DropTable("dbo.RecursosHumanos");
            DropTable("dbo.ParametroSistema");
            DropTable("dbo.SituacaoPacto");
            DropTable("dbo.IniciativaPlanoOperacionalProduto");
            DropTable("dbo.AvaliacaoDetalhadaProduto");
            DropTable("dbo.AvaliacaoProduto");
            DropTable("dbo.Produto");
            DropTable("dbo.Pacto");
            DropTable("dbo.NivelAvaliacao");
            DropTable("dbo.Log");
            DropTable("dbo.Justificativa");
            DropTable("dbo.Historico");
            DropTable("dbo.Feriado");
            DropTable("dbo.Cronograma");
            DropTable("dbo.ItemAvaliacao");
            DropTable("dbo.CriterioAvaliacao");
            DropTable("dbo.TipoAtividade");
            DropTable("dbo.NotaAvaliacao");
            DropTable("dbo.OS_ItemAvaliacao");
            DropTable("dbo.OS_CriterioAvaliacao");
            DropTable("dbo.OrdemServico");
            DropTable("dbo.OS_TipoAtividade");
            DropTable("dbo.OS_Atividade");
            DropTable("dbo.OS_GrupoAtividade");
            DropTable("dbo.TipoPacto");
            DropTable("dbo.GrupoAtividade");
            DropTable("dbo.Atividade");
            DropTable("dbo.Administrador");
        }
    }
}
