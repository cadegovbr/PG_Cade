-- NivelAvaliacao
INSERT [dbo].[NivelAvaliacao]([DescNivelAvaliacao])
VALUES ('Simplificada');

INSERT [dbo].[NivelAvaliacao]([DescNivelAvaliacao])
VALUES ('Detalhada');

-- Justificativa
INSERT [dbo].[Justificativa]([DescJustificativa])
VALUES ('Produto Não entregue');

INSERT [dbo].[Justificativa]([DescJustificativa])
VALUES ('Entregue com atraso');

INSERT [dbo].[Justificativa]([DescJustificativa])
VALUES ('Atividade Cancelada');

-- SituacaoPacto
INSERT [dbo].[SituacaoPacto]([DescSituacaoPacto])
VALUES ('Pendente de Assinatura');

INSERT [dbo].[SituacaoPacto]([DescSituacaoPacto])
VALUES ('A Iniciar');

INSERT [dbo].[SituacaoPacto]([DescSituacaoPacto])
VALUES ('Em Andamento');

INSERT [dbo].[SituacaoPacto]([DescSituacaoPacto])
VALUES ('Pendente de Avaliação');

INSERT [dbo].[SituacaoPacto]([DescSituacaoPacto])
VALUES ('Avaliado');

INSERT [dbo].[SituacaoPacto]([DescSituacaoPacto])
VALUES ('Negado');

INSERT [dbo].[SituacaoPacto]([DescSituacaoPacto])
VALUES ('Excluído');

INSERT [dbo].[SituacaoPacto]([DescSituacaoPacto])
VALUES ('Interrompido');

INSERT [dbo].[SituacaoPacto]([DescSituacaoPacto])
VALUES ('Suspenso');

INSERT [dbo].[SituacaoPacto]([DescSituacaoPacto])
VALUES ('Avaliado Parcialmente');

-- Unidade
INSERT [dbo].[Unidade]([Nome], [Sigla], [Excluido])
VALUES ('Unidade sem exercício', 'SGL', 0);

-- Perfil
INSERT [dbo].[Perfil]([Nome])
VALUES ('Solicitante');

INSERT [dbo].[Perfil]([Nome])
VALUES ('Dirigente');

INSERT [dbo].[Perfil]([Nome])
VALUES ('Administrador');


-- TipoPacto
INSERT [dbo].[TipoPacto]([DescTipoPacto])
VALUES ('Processo de Trabalho');

INSERT [dbo].[TipoPacto]([DescTipoPacto])
VALUES ('PGD - Projetos');


-- Situacao Produto
INSERT INTO [dbo].[SituacaoProduto]
           ([IdSituacaoProduto]
           ,[DescSituacaoProduto])
     VALUES
           (1
           ,'A Inciar')
GO

INSERT INTO [dbo].[SituacaoProduto]
           ([IdSituacaoProduto]
           ,[DescSituacaoProduto])
     VALUES
           (2
           ,'Iniciado')
GO

INSERT INTO [dbo].[SituacaoProduto]
           ([IdSituacaoProduto]
           ,[DescSituacaoProduto])
     VALUES
           (3
           ,'Concluído')
GO