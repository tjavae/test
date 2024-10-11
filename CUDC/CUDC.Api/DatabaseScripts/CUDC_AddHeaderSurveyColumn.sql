USE [NCUA]
GO

ALTER TABLE dbo.CUDC_Surveys ADD [InformationText] NVARCHAR(500)
GO
ALTER TABLE [dbo].[CUDC_Surveys] ADD  CONSTRAINT [InformationTexttDefault]  DEFAULT (NULL) FOR [InformationText]
GO


