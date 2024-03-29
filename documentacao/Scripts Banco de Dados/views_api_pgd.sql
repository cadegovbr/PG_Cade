USE [PGCADE]
GO
/****** Object:  View [dbo].[vw_pacto_horas_homologadas]    Script Date: 11/03/2022 08:37:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_pacto_horas_homologadas]
AS
SELECT        IdPacto, SUM(horas_homologadas) AS HorasHomologadas
FROM            (SELECT        pr.IdPacto, pr.IdProduto, ap.QuantidadeProdutosAvaliados, pr.CargaHorariaProduto, ap.QuantidadeProdutosAvaliados * pr.CargaHorariaProduto AS horas_homologadas
                          FROM            dbo.Produto AS pr INNER JOIN
                                                    dbo.Pacto AS pa ON pr.IdPacto = pa.IdPacto INNER JOIN
                                                    dbo.AvaliacaoProduto AS ap ON ap.IdProduto = pr.IdProduto INNER JOIN
                                                    dbo.Usuario AS us ON pa.CpfUsuario = us.CPF
                          WHERE        (pa.IdSituacaoPacto IN (3, 4, 5, 8, 9, 10)) AND (us.Matricula <> '111111')) AS tab
GROUP BY IdPacto
GO
/****** Object:  View [dbo].[vw_pacto]    Script Date: 11/03/2022 08:37:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_pacto]
AS
SELECT        pa.IdPacto AS id_pacto, CASE WHEN pa.IdSituacaoPacto IN (6, 7) THEN 'cancelado' ELSE NULL END AS situacao, CASE WHEN us.Matricula = '' THEN '0' ELSE us.Matricula END AS matricula_siape, pa.CpfUsuario AS cpf, 
                         pa.Nome AS nome_participante, pa.UnidadeExercicio AS cod_unidade_exercicio, un.Nome AS nome_unidade_exercicio, un.Sigla AS sigla_unidade_exercicio, sitpac.DescSituacaoPacto AS desc_situacao_pacto, 
                         3 AS modalidade_execucao, pa.CargaHoraria AS carga_horaria_semanal, CONVERT(DATE, pa.DataPrevistaInicio) AS data_inicio, CONVERT(DATE, pa.DataPrevistaTermino) AS data_fim, 
                         pa.CargaHorariaTotal AS carga_horaria_total, pa.DataInterrupcao AS data_interrupcao, CASE WHEN pa.EntregueNoPrazo = 1 THEN 'true' ELSE 'false' END AS entregue_no_prazo, ISNULL(phh.HorasHomologadas, 0) 
                         AS horas_homologadas
FROM            dbo.Pacto AS pa INNER JOIN
                         dbo.SituacaoPacto AS sitpac ON sitpac.IdSituacaoPacto = pa.IdSituacaoPacto INNER JOIN
                         dbo.Unidade AS un ON pa.UnidadeExercicio = un.IdUnidade INNER JOIN
                         dbo.Usuario AS us ON pa.CpfUsuario = us.CPF LEFT OUTER JOIN
                         dbo.vw_pacto_horas_homologadas AS phh ON pa.IdPacto = phh.IdPacto
WHERE        (pa.IdSituacaoPacto IN (3, 4, 5, 8, 9, 10)) AND (us.Matricula <> '111111')
GO
/****** Object:  View [dbo].[vw_produto_entregas_efetivas]    Script Date: 11/03/2022 08:37:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_produto_entregas_efetivas]
AS
SELECT        pr.IdPacto, pr.IdProduto, CASE WHEN ap.EntregueNoPrazo = 1 THEN ap.QuantidadeProdutosAvaliados ELSE 0 END AS QuantidadeEntregasEfetivas, ap.Avaliacao, ap.DataAvaliacao, ap.LocalizacaoProduto
FROM            dbo.AvaliacaoProduto AS ap INNER JOIN
                         dbo.Produto AS pr ON ap.IdProduto = pr.IdProduto
GO
/****** Object:  View [dbo].[vw_produto_horas_homologadas]    Script Date: 11/03/2022 08:37:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_produto_horas_homologadas]
AS
SELECT        IdPacto, IdSituacaoPacto, IdProduto, QuantidadeProdutosAvaliados, CargaHorariaProduto, SUM(horas_estimadas) AS horas_estimadas, SUM(horas_programadas) AS horas_programadas, SUM(horas_executadas) 
                         AS horas_executadas
FROM            (SELECT        pr.IdPacto, pa.IdSituacaoPacto, pr.IdProduto, ISNULL(ap.QuantidadeProdutosAvaliados, 0) AS QuantidadeProdutosAvaliados, pr.CargaHorariaProduto, 
                                                    pr.QuantidadeProduto * pr.CargaHorariaProduto AS horas_estimadas, pr.QuantidadeProduto * pr.CargaHorariaProduto AS horas_programadas, ISNULL(ap.QuantidadeProdutosAvaliados * pr.CargaHorariaProduto, 0) 
                                                    AS horas_executadas
                          FROM            dbo.Produto AS pr INNER JOIN
                                                    dbo.Pacto AS pa ON pr.IdPacto = pa.IdPacto LEFT OUTER JOIN
                                                    dbo.AvaliacaoProduto AS ap ON ap.IdProduto = pr.IdProduto INNER JOIN
                                                    dbo.SituacaoPacto AS sp ON sp.IdSituacaoPacto = pa.IdSituacaoPacto INNER JOIN
                                                    dbo.Usuario AS us ON pa.CpfUsuario = us.CPF
                          WHERE        (pa.IdSituacaoPacto IN (3, 4, 5, 8, 9, 10)) AND (us.Matricula <> '111111')) AS tab
GROUP BY IdPacto, IdSituacaoPacto, IdProduto, QuantidadeProdutosAvaliados, CargaHorariaProduto
GO
/****** Object:  View [dbo].[vw_produto]    Script Date: 11/03/2022 08:37:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_produto]
AS
SELECT        ROW_NUMBER() OVER (ORDER BY prod.IdProduto ASC) AS id, prod.IdProduto AS id_produto, prod.IdPacto AS id_pacto, prod.IdAtividade AS id_atividade, grat.NomGrupoAtividade AS nome_grupo_atividade, 
ativ.NomAtividade AS nome_atividade, tpativ.Faixa AS faixa_complexidade, tpativ.DescTextoExplicativo AS parametros_complexidade, 0 AS tempo_presencial_estimado, 0 AS tempo_presencial_programado, 
0 AS tempo_presencial_executado, phh.horas_estimadas AS tempo_teletrabalho_estimado, phh.horas_programadas AS tempo_teletrabalho_programado, phh.horas_executadas AS tempo_teletrabalho_executado, 
prod.Observacoes AS entrega_esperada, prod.QuantidadeProduto AS qtde_entregas, pee.QuantidadeEntregasEfetivas AS qtde_entregas_efetivas, pee.Avaliacao AS avaliacao, CONVERT(DATE, pee.DataAvaliacao) AS data_avaliacao, 
pee.LocalizacaoProduto AS justificativa
FROM            dbo.Produto AS prod INNER JOIN
                         dbo.OS_GrupoAtividade AS grat ON prod.IdGrupoAtividade = grat.IdGrupoAtividade INNER JOIN
                         dbo.OS_Atividade AS ativ ON prod.IdAtividade = ativ.IdAtividade INNER JOIN
                         dbo.OS_TipoAtividade AS tpativ ON prod.IdTipoAtividade = tpativ.IdTipoAtividade INNER JOIN
                         dbo.Pacto AS pact ON prod.IdPacto = pact.IdPacto AND pact.IdOrdemServico = grat.IdOrdemServico INNER JOIN
                         dbo.Usuario AS us ON pact.CpfUsuario = us.CPF LEFT OUTER JOIN
                         dbo.vw_produto_entregas_efetivas AS pee ON pee.IdPacto = pact.IdPacto AND pee.IdProduto = prod.IdProduto LEFT OUTER JOIN
                         dbo.vw_produto_horas_homologadas AS phh ON phh.IdPacto = pact.IdPacto AND phh.IdProduto = prod.IdProduto
WHERE        (pact.IdSituacaoPacto IN (3, 4, 5, 8, 9, 10)) AND (us.Matricula <> '111111')
GO
/****** Object:  View [dbo].[vw_atividade]    Script Date: 11/03/2022 08:37:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_atividade]
AS
SELECT        ROW_NUMBER() OVER (ORDER BY ativ.NomAtividade ASC) AS id_atividade, ativ.NomAtividade AS nome_atividade
FROM            dbo.OS_Atividade AS ativ
GROUP BY ativ.NomAtividade
GO
/****** Object:  View [dbo].[vw_grupo_atividade]    Script Date: 11/03/2022 08:37:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_grupo_atividade]
AS
SELECT        IdGrupoAtividade AS id_grupo_atividade, NomGrupoAtividade AS nome_grupo_atividade
FROM            dbo.OS_GrupoAtividade
WHERE        (IdOrdemServico =
                             (SELECT        MAX(IdOrdemServico) AS Expr1
                               FROM            dbo.OS_GrupoAtividade AS OS_GrupoAtividade_1))
GO
/****** Object:  View [dbo].[vw_situacao_pacto]    Script Date: 11/03/2022 08:37:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_situacao_pacto]
AS
SELECT        IdSituacaoPacto AS id, DescSituacaoPacto AS descricao
FROM            dbo.SituacaoPacto
GO
/****** Object:  View [dbo].[vw_tipo_atividade]    Script Date: 11/03/2022 08:37:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_tipo_atividade]
AS
SELECT        ROW_NUMBER() OVER (ORDER BY tpativ.Faixa ASC) AS id_tipo_atividade, tpativ.Faixa AS faixa_complexidade, tpativ.DescTextoExplicativo AS parametro_complexidade
FROM            dbo.OS_TipoAtividade AS tpativ
GROUP BY tpativ.Faixa, tpativ.DescTextoExplicativo
GO
/****** Object:  View [dbo].[vw_unidade]    Script Date: 11/03/2022 08:37:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_unidade]
AS
SELECT        IdUnidade AS id_unidade, Nome, Sigla, Codigo
FROM            dbo.Unidade
WHERE        (Excluido = 0)
GO
