USE [NCUA]
GO

/****** Object:  Table [dbo].[CUDC_QuestionReferences]    Script Date: 11/18/2020 6:34:40 PM ******/
DROP TABLE [dbo].[CUDC_QuestionReferences]
GO

/****** Object:  Table [dbo].[CUDC_QuestionReferences]    Script Date: 11/18/2020 6:34:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CUDC_QuestionReferences](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UniqueId] [uniqueidentifier] NOT NULL,
	[ReferenceQuestionId] [uniqueidentifier] NOT NULL,
	[ReferenceOptionId] [uniqueidentifier] NOT NULL,
	[QuestionId] [uniqueidentifier] NOT NULL,
	[QuestionOptionId] [uniqueidentifier] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [nchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nchar](50) NULL,
 CONSTRAINT [PK_CUDC_QuestionReferences] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Grant access permission on CUDC_QuestionReferences ******/
GRANT SELECT ON [dbo].[CUDC_QuestionReferences] TO  [CUDCLINK]  
GO
GRANT SELECT ON [dbo].[CUDC_QuestionReferences] TO  [foc310704]  
GO
GRANT SELECT, UPDATE, INSERT, DELETE ON [dbo].[CUDC_QuestionReferences] TO  [HQNT\DevWebOperator]  
GO
GRANT SELECT, UPDATE, INSERT, DELETE ON [dbo].[CUDC_QuestionReferences] TO  [HQNT\WebOperator] 
GO
GRANT SELECT, UPDATE, INSERT, DELETE ON [dbo].[CUDC_QuestionReferences] TO  [HQNT\svcDevMissionInfoSys] 
GO