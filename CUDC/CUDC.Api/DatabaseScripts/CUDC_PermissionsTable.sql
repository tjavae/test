USE [NCUA]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CUDC_Permissions](
	[Id] [int] NOT NULL,
	[UserType] [int] NOT NULL,
	[ActingRole] [varchar](5) NOT NULL,
	[Membership] [varchar](100) NOT NULL,
	[RolePermission] [int] NOT NULL,
	[EmployeeNumber] [varchar](500) NULL,
	[EmployeeType] [varchar](50) NULL,
	[EmpDivision] [varchar](20) NULL,
	[EmpRegionCondition] [varchar](50) NULL,
	[EmpSeCondition] [int] NULL,
	[EmpDistrictCondition] [int] NULL,
	[RegionAccess] [varchar](50) NULL,
	[SeAccess] [varchar](100) NULL,
	[DistrictAccess] [varchar](100) NULL,
	[EditSurvey] [varchar](50) NULL,
	[DosViewClaimCUOnly] [bit] NOT NULL,
	[SeViewClaimCUOnly] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](150) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](150) NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_CUDC_Permissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[CUDC_Permissions] ([Id], [UserType], [ActingRole], [Membership], [RolePermission], [EmployeeNumber], [EmployeeType], [EmpDivision], [EmpRegionCondition], [EmpSeCondition], [EmpDistrictCondition], [RegionAccess], [SeAccess], [DistrictAccess], [EditSurvey], [DosViewClaimCUOnly], [SeViewClaimCUOnly], [IsActive], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (1, 1, N'A    ', N'Admin', 1, NULL, NULL, NULL, NULL, 3, 3, N'1,2,3,8', NULL, NULL, NULL, 0, 0, 1, N'sytem', CAST(N'2020-08-31T10:20:51.077' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[CUDC_Permissions] ([Id], [UserType], [ActingRole], [Membership], [RolePermission], [EmployeeNumber], [EmployeeType], [EmpDivision], [EmpRegionCondition], [EmpSeCondition], [EmpDistrictCondition], [RegionAccess], [SeAccess], [DistrictAccess], [EditSurvey], [DosViewClaimCUOnly], [SeViewClaimCUOnly], [IsActive], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (2, 2, N'V    ', N'OceEiObiMember', 2, NULL, N'M', NULL, N'9', 1, 1, N'1,2,3,8', NULL, NULL, NULL, 0, 0, 1, N'sytem', CAST(N'2020-08-31T10:52:34.287' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[CUDC_Permissions] ([Id], [UserType], [ActingRole], [Membership], [RolePermission], [EmployeeNumber], [EmployeeType], [EmpDivision], [EmpRegionCondition], [EmpSeCondition], [EmpDistrictCondition], [RegionAccess], [SeAccess], [DistrictAccess], [EditSurvey], [DosViewClaimCUOnly], [SeViewClaimCUOnly], [IsActive], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (3, 2, N'D    ', N'RegMngmntDosMember', 2, NULL, N'A,B,C,E', NULL, NULL, 3, 1, N'1,2,3,8', NULL, NULL, N'4', 0, 0, 1, N'sytem', CAST(N'2020-08-31T10:53:22.140' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[CUDC_Permissions] ([Id], [UserType], [ActingRole], [Membership], [RolePermission], [EmployeeNumber], [EmployeeType], [EmpDivision], [EmpRegionCondition], [EmpSeCondition], [EmpDistrictCondition], [RegionAccess], [SeAccess], [DistrictAccess], [EditSurvey], [DosViewClaimCUOnly], [SeViewClaimCUOnly], [IsActive], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (4, 2, N'D    ', N'RegMngmntDosMember', 2, NULL, N'I,K', N'DOS', NULL, 3, 1, N'1,2,3,8', NULL, NULL, N'4', 0, 0, 1, N'sytem', CAST(N'2020-08-31T10:53:22.173' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[CUDC_Permissions] ([Id], [UserType], [ActingRole], [Membership], [RolePermission], [EmployeeNumber], [EmployeeType], [EmpDivision], [EmpRegionCondition], [EmpSeCondition], [EmpDistrictCondition], [RegionAccess], [SeAccess], [DistrictAccess], [EditSurvey], [DosViewClaimCUOnly], [SeViewClaimCUOnly], [IsActive], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (5, 2, N'S    ', N'SupervisorSEMember', 2, NULL, N'J,F', NULL, NULL, 2, 3, N'1,2,3,8', NULL, NULL, N'3', 1, 0, 1, N'sytem', CAST(N'2020-08-31T10:53:22.187' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[CUDC_Permissions] ([Id], [UserType], [ActingRole], [Membership], [RolePermission], [EmployeeNumber], [EmployeeType], [EmpDivision], [EmpRegionCondition], [EmpSeCondition], [EmpDistrictCondition], [RegionAccess], [SeAccess], [DistrictAccess], [EditSurvey], [DosViewClaimCUOnly], [SeViewClaimCUOnly], [IsActive], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (6, 2, N'P    ', N'PCOMember', 2, NULL, N'H', NULL, NULL, 2, 3, N'1,2,3,8', NULL, NULL, N'2', 0, 0, 1, N'sytem', CAST(N'2020-08-31T10:53:22.200' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[CUDC_Permissions] ([Id], [UserType], [ActingRole], [Membership], [RolePermission], [EmployeeNumber], [EmployeeType], [EmpDivision], [EmpRegionCondition], [EmpSeCondition], [EmpDistrictCondition], [RegionAccess], [SeAccess], [DistrictAccess], [EditSurvey], [DosViewClaimCUOnly], [SeViewClaimCUOnly], [IsActive], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (7, 2, N'F    ', N'FieldStaff', 2, NULL, N'K', NULL, NULL, 2, 2, N'1,2,3,8', N'SE', NULL, N'2', 1, 1, 1, N'sytem', CAST(N'2020-08-31T10:53:22.217' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[CUDC_Permissions] ([Id], [UserType], [ActingRole], [Membership], [RolePermission], [EmployeeNumber], [EmployeeType], [EmpDivision], [EmpRegionCondition], [EmpSeCondition], [EmpDistrictCondition], [RegionAccess], [SeAccess], [DistrictAccess], [EditSurvey], [DosViewClaimCUOnly], [SeViewClaimCUOnly], [IsActive], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (8, 2, N'S    ', N'Special', 3, N'1998', N'K', NULL, NULL, NULL, NULL, N'1', N'B', N'2,10', N'3', 1, 0, 1, N'sytem', CAST(N'2020-08-31T10:53:22.233' AS DateTime), NULL, NULL)
GO
ALTER TABLE [dbo].[CUDC_Permissions] ADD  CONSTRAINT [DF_CUDC_Permissions_EmpSeCondition]  DEFAULT ((0)) FOR [EmpSeCondition]
GO
ALTER TABLE [dbo].[CUDC_Permissions] ADD  CONSTRAINT [DF_CUDC_Permissions_EmpSeCondition1]  DEFAULT ((0)) FOR [EmpDistrictCondition]
GO
ALTER TABLE [dbo].[CUDC_Permissions] ADD  CONSTRAINT [DF_CUDC_Permissions_DosViewClaimCUOnly]  DEFAULT ((0)) FOR [DosViewClaimCUOnly]
GO
ALTER TABLE [dbo].[CUDC_Permissions] ADD  CONSTRAINT [DF_Table_1_DosViewClaimCUOnly1]  DEFAULT ((0)) FOR [SeViewClaimCUOnly]
GO
ALTER TABLE [dbo].[CUDC_Permissions] ADD  CONSTRAINT [DF_CUDC_Permissions_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
GO
GRANT SELECT, UPDATE, INSERT,DELETE ON [dbo].[CUDC_Permissions] TO  [HQNT\DevWebOperator]  
GO
GRANT SELECT, UPDATE, INSERT,DELETE ON [dbo].[CUDC_Permissions] TO  [HQNT\WebOperator] 
GO