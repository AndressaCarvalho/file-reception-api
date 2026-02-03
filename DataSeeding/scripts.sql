/* ==================== Company ==================== */

USE [FileReception]
GO

INSERT INTO [dbo].[Company] ([Name]) VALUES ('FagammonCard')
GO

INSERT INTO [dbo].[Company] ([Name]) VALUES ('UfCard')
GO


/* ==================== FileLayout ==================== */

USE [FileReception]
GO

INSERT INTO [dbo].[FileLayout] ([CompanyId], [Name]) VALUES (1, 'FagammonCard Layout 1')
GO

INSERT INTO [dbo].[FileLayout] ([CompanyId], [Name]) VALUES (2, 'UfCard Layout 1')
GO


/* ==================== FileLayoutField ==================== */

USE [FileReception]
GO

INSERT INTO [dbo].[FileLayoutField] ([FileLayoutId], [Description], [Start], [End], [FileLayoutFieldTypeId]) VALUES (1, 'TipoRegistro', 1, 1, 2)
GO

INSERT INTO [dbo].[FileLayoutField] ([FileLayoutId], [Description], [Start], [End], [FileLayoutFieldTypeId]) VALUES (1, 'DataProcessamento', 2, 9, 2)
GO

INSERT INTO [dbo].[FileLayoutField] ([FileLayoutId], [Description], [Start], [End], [FileLayoutFieldTypeId]) VALUES (1, 'Estabelecimento', 10, 17, 2)
GO

INSERT INTO [dbo].[FileLayoutField] ([FileLayoutId], [Description], [Start], [End], [FileLayoutFieldTypeId]) VALUES (1, 'Empresa', 18, 29, 1)
GO

INSERT INTO [dbo].[FileLayoutField] ([FileLayoutId], [Description], [Start], [End], [FileLayoutFieldTypeId]) VALUES (1, 'Sequencia', 30, 36, 2)
GO

INSERT INTO [dbo].[FileLayoutField] ([FileLayoutId], [Description], [Start], [End], [FileLayoutFieldTypeId]) VALUES (2, 'TipoRegistro', 1, 1, 2)
GO

INSERT INTO [dbo].[FileLayoutField] ([FileLayoutId], [Description], [Start], [End], [FileLayoutFieldTypeId]) VALUES (2, 'Estabelecimento', 2, 11, 2)
GO

INSERT INTO [dbo].[FileLayoutField] ([FileLayoutId], [Description], [Start], [End], [FileLayoutFieldTypeId]) VALUES (2, 'DataProcessamento', 12, 19, 2)
GO

INSERT INTO [dbo].[FileLayoutField] ([FileLayoutId], [Description], [Start], [End], [FileLayoutFieldTypeId]) VALUES (2, 'PeriodoInicial', 20, 27, 2)
GO

INSERT INTO [dbo].[FileLayoutField] ([FileLayoutId], [Description], [Start], [End], [FileLayoutFieldTypeId]) VALUES (2, 'PeriodoFinal', 28, 35, 2)
GO

INSERT INTO [dbo].[FileLayoutField] ([FileLayoutId], [Description], [Start], [End], [FileLayoutFieldTypeId]) VALUES (2, 'Sequencia', 36, 42, 2)
GO

INSERT INTO [dbo].[FileLayoutField] ([FileLayoutId], [Description], [Start], [End], [FileLayoutFieldTypeId]) VALUES (2, 'Empresa', 43, 50, 1)
GO


/* ==================== File ==================== */

USE [FileReception]
GO

INSERT INTO [dbo].[File] ([FileLayoutId], [Name], [ExpectedDate], [StatusId]) VALUES (1, 'FagammonCard_Teste_1.txt', '2026-02-04 10:00:00.0000000', 1)
GO

INSERT INTO [dbo].[File] ([FileLayoutId], [Name], [ExpectedDate], [StatusId]) VALUES (2, 'UfCard_Teste_1.txt', '2026-02-04 10:00:00.0000000', 1)
GO
