------------- PGD - RELATÓRIO DE DADOS BRUTOS

-- SQL para gerar arquivo .csv para relatório do PGD
select 
pac.IdPacto, pac.Nome, pac.MatriculaSIAPE, pac.UnidadeExercicio, unid.nome as NomeUnidade, pac.TelefoneFixoServidor, pac.TelefoneMovelServidor, 

--Tratamento do campo --> pac.PossuiCargaHoraria, 
case when pac.PossuiCargaHoraria=1 then 'Sim' 
     when pac.PossuiCargaHoraria=0 then 'Não' end as PossuiCargaHoraria,

pac.CargaHoraria,
pac.DataPrevistaInicio, pac.DataPrevistaTermino, 
pac.CargaHorariaTotal, 

sit.DescSituacaoPacto as SituacaoPacto,

pac.IdOrdemServico, 
pac.OrdemServico_IdOrdemServico, 
pac.SuspensoAPartirDe, replace(replace(replace(pac.Motivo,Char(13),''),char(10),''),';','-') as Motivo, 

pac.DataAprovacaoSolicitante, pac.DataAprovacaoDirigente, 
osga.NomGrupoAtividade, osa.NomAtividade,

pro.CargaHoraria, pro.QuantidadeProduto, pro.CargaHorariaProduto, 

--- Observações da Atividade (Produtos)
replace(replace(replace(replace(pro.Observacoes,Char(13),''),char(10),''),';','-'),',',' ') as Observacoes,

pro.IdTipoAtividade,ta.Faixa, ta.DuracaoFaixa, ta.DuracaoFaixaPresencial, 
pro.Motivo

------- DADOS DAS AVALIACOES REALIZADAS PARA OS PRODUTOS DAS ATIVIDADES DO PACTO
, avp.QuantidadeProdutosAvaliados, avp.DataAvaliacao as 'DataAvaliacao', 

replace(replace(replace(replace(avp.LocalizacaoProduto,Char(13),''),char(10),''),';','-'),',',' ') as LocalizacaoProduto, 

--Tratamento do campo --> pac.EntregueNoPrazo (1 - Sim / 0 - Não), 
case when avp.EntregueNoPrazo=0 then 'Não' 
     when avp.EntregueNoPrazo=1 then 'Sim' 
     else 'Pacto não avaliado' 
	 end as EntregueNoPrazo,
jus.DescJustificativa, 

--Tratamento do campo --> avp.Avaliacao (1 - Sim / 0 - Não), 
case when avp.Avaliacao = 1 then '1 - Insatisfatório' 
     when avp.Avaliacao = 2 then '2 - Regular' 
	 when avp.Avaliacao = 3 then '3 - Bom' 
	 when avp.Avaliacao = 4 then '4 - Muito Bom' 
	 when avp.Avaliacao = 5 then '5 - Excelente' 
	 when avp.Avaliacao = 6 then '6 - Não Entregue' 
     else 'Pacto não avaliado' 
	 end as Avaliacao
, (select avp.QuantidadeProdutosAvaliados * pro.CargaHorariaProduto) as 'HorasHomologadas'
, avp.DataTerminoReal

from 
AvaliacaoProduto avp			--> INCLUÍDA NOVA TABELA AVALIACAO, POIS AGORA PODEM OCORRER VARIAS AVALIAÇÕES PARCIAIS PARA O PACTO
LEFT join dbo.Produto as pro on avp.IdProduto = pro.IdProduto
left join dbo.Os_tipoAtividade ta on ta.IdTipoAtividade = pro.IdTipoAtividade
left join dbo.Pacto pac on pac.IdPacto = pro.IdPacto
left join dbo.OS_Atividade osa on osa.IdAtividade = pro.IdAtividade
left join dbo.OS_GrupoAtividade osga on osga.IdGrupoAtividade = pro.IdGrupoAtividade
left join dbo.Unidade unid on unid.id_unidade = pac.UnidadeExercicio
left join dbo.SituacaoPacto sit on pac.IdSituacaoPacto = sit.IdSituacaoPacto
LEFT JOIN DBO.Justificativa jus on jus.IdJustificativa = AVP.IdJustificativa;

