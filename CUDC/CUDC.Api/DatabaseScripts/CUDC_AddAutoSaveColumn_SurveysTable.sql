USE NCUA
GO

ALTER TABLE [dbo].[CUDC_Surveys] ADD AutoSaveInterval [tinyint] NULL
GO

use ncuarpt2
go

sp_refreshview 'dbo.CUDC_NormalSurveyView'
go
sp_refreshview 'dbo.CUDC_SurveyView'
go
