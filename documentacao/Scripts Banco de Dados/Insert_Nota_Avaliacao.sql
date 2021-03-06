USE [PGDNOVO]
GO
SET IDENTITY_INSERT [dbo].[NotaAvaliacao] ON 

INSERT [dbo].[NotaAvaliacao] ([IdNotaAvaliacao], [DescNotaAvaliacao], [IndAtivoAvaliacaoSimplificada], [IndAtivoAvaliacaoDetalhada], [LimiteSuperiorFaixa], [LimiteInferiorFaixa], [Conceito]) VALUES (1, N'Excelente', 0, 1, CAST(7.90 AS Decimal(18, 2)), CAST(7.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[NotaAvaliacao] ([IdNotaAvaliacao], [DescNotaAvaliacao], [IndAtivoAvaliacaoSimplificada], [IndAtivoAvaliacaoDetalhada], [LimiteSuperiorFaixa], [LimiteInferiorFaixa], [Conceito]) VALUES (2, N'Muito Bom', 0, 1, CAST(7.90 AS Decimal(18, 2)), CAST(7.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[NotaAvaliacao] ([IdNotaAvaliacao], [DescNotaAvaliacao], [IndAtivoAvaliacaoSimplificada], [IndAtivoAvaliacaoDetalhada], [LimiteSuperiorFaixa], [LimiteInferiorFaixa], [Conceito]) VALUES (3, N'Bom', 0, 1, CAST(7.90 AS Decimal(18, 2)), CAST(7.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[NotaAvaliacao] ([IdNotaAvaliacao], [DescNotaAvaliacao], [IndAtivoAvaliacaoSimplificada], [IndAtivoAvaliacaoDetalhada], [LimiteSuperiorFaixa], [LimiteInferiorFaixa], [Conceito]) VALUES (4, N'Regular', 0, 1, CAST(7.90 AS Decimal(18, 2)), CAST(7.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[NotaAvaliacao] ([IdNotaAvaliacao], [DescNotaAvaliacao], [IndAtivoAvaliacaoSimplificada], [IndAtivoAvaliacaoDetalhada], [LimiteSuperiorFaixa], [LimiteInferiorFaixa], [Conceito]) VALUES (5, N'Insatisfatório', 0, 1, CAST(7.90 AS Decimal(18, 2)), CAST(7.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[NotaAvaliacao] ([IdNotaAvaliacao], [DescNotaAvaliacao], [IndAtivoAvaliacaoSimplificada], [IndAtivoAvaliacaoDetalhada], [LimiteSuperiorFaixa], [LimiteInferiorFaixa], [Conceito]) VALUES (6, N'Inaproveitável', 0, 0, CAST(7.90 AS Decimal(18, 2)), CAST(7.00 AS Decimal(18, 2)), NULL)
SET IDENTITY_INSERT [dbo].[NotaAvaliacao] OFF
GO
