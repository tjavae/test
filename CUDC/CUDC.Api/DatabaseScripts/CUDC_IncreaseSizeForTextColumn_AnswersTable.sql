USE NCUA
GO

ALTER TABLE CUDC_Answers ALTER COLUMN Text NVARCHAR(2000)
GO

ALTER TABLE CUDC_Answers ALTER COLUMN Longtext NVARCHAR(1000)
GO

ALTER TABLE CUDC_QuestionRevisions ALTER COLUMN Text NVARCHAR(1500)
GO

-- Fixed bug for Static Label type for question.
insert into [CUDC_QuestionTypes](UniqueId, Text, IsActive, CreatedOn, CreatedBy)
values(NEWID(), 'Label', 1, GETDATE(), 'system') 

use ncuarpt2
go

sp_refreshview 'dbo.CUDC_NormalSurveyView'
go
sp_refreshview 'dbo.CUDC_SurveyView'
go
