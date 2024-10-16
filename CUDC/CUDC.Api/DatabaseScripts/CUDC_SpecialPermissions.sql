USE [NCUA]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CUDC_SpecialPermissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeNumber] [int] NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Region] [int] NULL,
	[SE] [nvarchar](50) NULL,
	[District] [int] NULL,
	[Position] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CUDC_SpecialPermissions] ON 
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (195, 3147, N'PATRICIA', N'OUELLETTE', 1, N'Z', 14, N'RLS', CAST(N'2020-09-28T08:54:28.530' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (196, 364, N'MANUEL', N'PACHECO', 3, N'R', 10, N'RLS', CAST(N'2020-09-28T08:54:28.680' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (197, 2334, N'KIMBERLY', N'PAIGE', 2, N'P', NULL, N'DSA', CAST(N'2020-09-28T08:54:28.830' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (198, 310, N'MARK', N'PAINTER', 2, N'O', 8, N'RLS', CAST(N'2020-09-28T08:54:28.990' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (199, 1087, N'MARIA', N'PALMA', 3, N'R', 11, N'RLS', CAST(N'2020-09-28T08:54:29.143' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (200, 2129, N'JOSEPH', N'PARKER', 1, N'X', 11, N'RCMS', CAST(N'2020-09-28T08:54:29.283' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (201, 2859, N'MATTHEW', N'PAVLICK', 1, N'Z', 3, N'PCO', CAST(N'2020-09-28T08:54:29.433' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (202, 1649, N'CORRADO', N'PERILLI', 1, N'Y', 4, N'RISO', CAST(N'2020-09-28T08:54:29.580' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (203, 1448, N'DANIEL', N'POPP', 1, N'Y', 9, N'RISO', CAST(N'2020-09-28T08:54:29.720' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (204, 434, N'ALICIA', N'POTTER', 2, N'P', 4, N'PCO', CAST(N'2020-09-28T08:54:29.880' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (205, 1613, N'RAYMOND', N'QUINTANA', 3, N'S', 8, N'RLS', CAST(N'2020-09-28T08:54:30.023' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (206, 2060, N'JUSTIN', N'RAEF', 2, N'O', 3, N'PCO', CAST(N'2020-09-28T08:54:30.160' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (207, 2467, N'ISMAEL', N'RAMOS', 2, N'O', 6, N'RLS', CAST(N'2020-09-28T08:54:30.320' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (208, 397, N'DORINDA', N'ROCHE', 3, N'U', 92, N'RISO', CAST(N'2020-09-28T08:54:30.480' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (209, 2921, N'SHAWN', N'RUEHLE', 1, N'Z', 13, N'RLS', CAST(N'2020-09-28T08:54:30.627' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (210, 538, N'SCOTT', N'SAND', 2, N'O', 7, N'RLS', CAST(N'2020-09-28T08:54:30.780' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (211, 1335, N'AMANDA', N'SAUNDERS', 3, N'U', 95, N'RISO', CAST(N'2020-09-28T08:54:30.920' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (212, 2205, N'PATRICK', N'SCHWARTZ', 3, N'T', 1, N'PCO', CAST(N'2020-09-28T08:54:31.067' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (213, 1488, N'ROBERT', N'SCOTT', 3, N'S', 9, N'RLS', CAST(N'2020-09-28T08:54:31.213' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (214, 1716, N'DANIEL', N'SEARLES', 2, N'P', 9, N'RCMS', CAST(N'2020-09-28T08:54:31.360' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (215, 593, N'PETER', N'SEIDL', 3, N'U', 99, N'RISO', CAST(N'2020-09-28T08:54:31.507' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (216, 2117, N'DAVID', N'SHAFFER', 3, N'U', 94, N'RISO', CAST(N'2020-09-28T08:54:31.643' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (217, 2070, N'LAURA', N'SMITH', 2, N'O', 12, N'RLS', CAST(N'2020-09-28T08:54:31.813' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (218, 2548, N'PAULA', N'SPENDLOVE', 3, N'R', 12, N'RLS', CAST(N'2020-09-28T08:54:31.963' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (219, 1085, N'KEITH', N'STEIER', 3, N'S', 10, N'RLS', CAST(N'2020-09-28T08:54:32.107' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (220, 1146, N'ALAN', N'STRAYER', 3, N'R', 1, N'PCO', CAST(N'2020-09-28T08:54:32.250' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (221, 2138, N'RYAN', N'SULLIVAN', 1, N'X', 2, N'PCO', CAST(N'2020-09-28T08:54:32.397' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (222, 1305, N'DAVID', N'THEORET', 1, N'Z', 12, N'RLS', CAST(N'2020-09-28T08:54:32.543' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (223, 2715, N'WAYNE', N'TROUT', 2, N'Q', 8, N'RISO', CAST(N'2020-09-28T08:54:32.683' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (224, 44, N'PATRICK', N'TRUETT', 2, N'Q', 9, N'RISO', CAST(N'2020-09-28T08:54:32.830' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (225, 2227, N'KENNETH', N'TRUJILLO', 3, N'T', 17, N'RCMS', CAST(N'2020-09-28T08:54:32.990' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (226, 3024, N'EDWARD', N'VALLEJO', 2, N'O', 11, N'RLS', CAST(N'2020-09-28T08:54:33.147' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (227, 685, N'JAMES', N'VANDENBERG', 1, N'Y', 6, N'PCO', CAST(N'2020-09-28T08:54:33.303' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (228, 1305, N'CHARLES', N'VOYTAN', 3, N'U', 91, N'RISO', CAST(N'2020-09-28T08:54:33.450' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (229, 2715, N'SHERRY', N'WELLS', 2, N'O', 15, N'RLS', CAST(N'2020-09-28T08:54:33.607' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (230, 44, N'GERALD', N'WYLAND', 1, N'Y', 10, N'RISO', CAST(N'2020-09-28T08:54:33.753' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (231, 2227, N'SHAWN', N'ZENON', 2, N'O', 2, N'PCO', CAST(N'2020-09-28T08:54:33.967' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (232, 3024, N'DAVID', N'ZWEIGART', 3, N'U', 98, N'RISO', CAST(N'2020-09-28T08:54:34.153' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (117, 350, N'LADANA', N'ACERS', 3, N'T', 11, N'RCMS', CAST(N'2020-09-28T08:54:15.540' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (118, 317, N'THOMAS', N'ANDREA', 1, N'Y', 8, N'PCO', CAST(N'2020-09-28T08:54:15.713' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (119, 326, N'RICHARD', N'ARCIA', 1, N'Y', 7, N'PCO', CAST(N'2020-09-28T08:54:15.867' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (120, 1230, N'GEORGE', N'ARNOLD', 1, N'X', 13, N'RCMS', CAST(N'2020-09-28T08:54:16.020' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (121, 1926, N'GARY', N'ARTZ', 2, N'P', 5, N'RCMS', CAST(N'2020-09-28T08:54:16.180' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (122, 1447, N'JAMES', N'ASHMAN', 2, N'Q', 2, N'PCO', CAST(N'2020-09-28T08:54:16.330' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (123, 2547, N'KEVIN', N'AXELROOD', 3, N'S', 3, N'RLS', CAST(N'2020-09-28T08:54:16.497' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (124, 2705, N'EDWARD', N'BAILEY', 2, N'Q', 5, N'RISO', CAST(N'2020-09-28T08:54:16.663' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (125, 1720, N'RUSSELL', N'BARLOW', 3, N'T', 2, N'PCO', CAST(N'2020-09-28T08:54:16.820' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (126, 1287, N'SHEILA', N'BEAUDETTE', 3, N'U', NULL, N'DSA', CAST(N'2020-09-28T08:54:16.990' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (127, 2761, N'ANDREW', N'BECHINSKY', 2, N'P', 6, N'RCMS', CAST(N'2020-09-28T08:54:17.163' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (128, 935, N'JEFFREY', N'BEDIGIAN', 1, N'Z', 1, N'PCO', CAST(N'2020-09-28T08:54:17.330' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (129, 1804, N'JOHN', N'BELFIELD', 1, N'X', 5, N'RCMS', CAST(N'2020-09-28T08:54:17.493' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (130, 1408, N'PAUL', N'BELLAIR', 1, N'Y', 1, N'RISO', CAST(N'2020-09-28T08:54:17.657' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (131, 35, N'VICTORIA', N'BENNETT', 3, N'R', 7, N'RLS', CAST(N'2020-09-28T08:54:17.807' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (132, 2603, N'JOHN', N'BISHOP', 3, N'R', 2, N'PCO', CAST(N'2020-09-28T08:54:17.967' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (133, 2403, N'DAVID', N'BLANCHARD', 1, N'Z', 4, N'PCO', CAST(N'2020-09-28T08:54:18.127' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (134, 2432, N'RYAN', N'BLANSCETT', 2, N'P', 1, N'PCO', CAST(N'2020-09-28T08:54:18.290' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (135, 1998, N'DAVID', N'BOLVIN', 1, N'Y', 12, N'PCO', CAST(N'2020-09-28T08:54:18.440' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (136, 2348, N'WILLIAM', N'BRODIE', 1, N'Z', 6, N'RLS', CAST(N'2020-09-28T08:54:18.603' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (137, 253, N'KIM', N'BROWN', 2, N'Q', 3, N'PCO', CAST(N'2020-09-28T08:54:18.753' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (138, 108, N'CHRISTINE', N'BRYANT', 2, N'O', NULL, N'DSA', CAST(N'2020-09-28T08:54:18.907' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (139, 3134, N'TROD', N'BUGGS', 3, N'S', 4, N'RLS', CAST(N'2020-09-28T08:54:19.073' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (140, 1060, N'LILY', N'BUI', 3, N'T', 12, N'RCMS', CAST(N'2020-09-28T08:54:19.237' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (141, 3131, N'TIMOTHY', N'CANJAR', 1, N'Z', 7, N'RLS', CAST(N'2020-09-28T08:54:19.390' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (142, 675, N'MARK', N'CANTOR', 2, N'Q', NULL, N'DSA', CAST(N'2020-09-28T08:54:19.570' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (143, 3191, N'JOSEPH', N'CARPER', 3, N'T', 13, N'RCMS', CAST(N'2020-09-28T08:54:19.767' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (144, 246, N'DAVID', N'CLEMENTE', 1, N'X', 6, N'RCMS', CAST(N'2020-09-28T08:54:19.930' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (145, 2973, N'DAVID', N'COMBS', 3, N'R', 14, N'RLS', CAST(N'2020-09-28T08:54:20.080' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (146, 755, N'JOSEPH', N'D''AMBRA', 2, N'P', 7, N'RCMS', CAST(N'2020-09-28T08:54:20.240' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (147, 944, N'ELIZABETH', N'DINAPOLI', 3, N'U', 1, N'PCO', CAST(N'2020-09-28T08:54:20.420' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (148, 757, N'JAMES', N'DUPRE', 2, N'P', 8, N'RCMS', CAST(N'2020-09-28T08:54:20.610' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (149, 2552, N'PAMELA', N'LYNN', 1, N'Z', 8, N'RLS', CAST(N'2020-09-28T08:54:20.767' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (150, 3524, N'JONATHAN', N'FAIRALL', 3, N'T', 14, N'RCMS', CAST(N'2020-09-28T08:54:20.940' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (151, 279, N'FEDERICO', N'FERNANDEZ', 2, N'Q', 6, N'RISO', CAST(N'2020-09-28T08:54:21.103' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (152, 2975, N'ANDREW', N'FIELDS', 3, N'T', 16, N'RCMS', CAST(N'2020-09-28T08:54:21.283' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (153, 3089, N'NORMAN', N'FREY', 3, N'T', 18, N'RCMS', CAST(N'2020-09-28T08:54:21.457' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (154, 710, N'ISAAC', N'FULWOOD', 1, N'X', 7, N'RCMS', CAST(N'2020-09-28T08:54:21.630' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (155, 1418, N'FRANCISCO', N'GARCIA', 2, N'O', 4, N'PCO', CAST(N'2020-09-28T08:54:21.780' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (156, 1752, N'WILLIAM', N'GROTHKOPP', 1, N'Z', 2, N'PCO', CAST(N'2020-09-28T08:54:21.940' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (157, 2655, N'HAMBY', N'HANEY', 2, N'Q', 10, N'RISO', CAST(N'2020-09-28T08:54:22.107' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (158, 1937, N'KATHLEEN', N'HEALY', 1, N'Z', NULL, N'DSA', CAST(N'2020-09-28T08:54:22.253' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (159, 2947, N'CYNTHIA', N'HEGGEMEIER', 3, N'S', 5, N'RLS', CAST(N'2020-09-28T08:54:22.407' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (160, 3606, N'SIMON', N'HERMANN', 3, N'R', 8, N'RLS', CAST(N'2020-09-28T08:54:22.567' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (161, 2706, N'SAMUEL', N'HILL', 2, N'O', 14, N'RLS', CAST(N'2020-09-28T08:54:22.723' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (162, 2311, N'LEIGH', N'HODECKER', 1, N'X', NULL, N'PCO', CAST(N'2020-09-28T08:54:22.870' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (163, 637, N'JACK', N'HOLLISTER', 2, N'O', 1, N'PCO', CAST(N'2020-09-28T08:54:23.027' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (164, 1894, N'JAVIER', N'IBANEZ', 3, N'U', 2, N'PCO', CAST(N'2020-09-28T08:54:23.167' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (165, 1864, N'PETER', N'JENSEN', 3, N'T', 15, N'RCMS', CAST(N'2020-09-28T08:54:23.320' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (166, 862, N'KAREN', N'KALLIS', 2, N'P', 2, N'PCO', CAST(N'2020-09-28T08:54:23.480' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (167, 1102, N'JOHN', N'KARISNY', 1, N'Z', 9, N'RLS', CAST(N'2020-09-28T08:54:23.630' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (168, 2703, N'JAMAL', N'KINLAW', 2, N'O', 9, N'RLS', CAST(N'2020-09-28T08:54:23.810' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (169, 3305, N'PAUL', N'KISSEL', 3, N'U', 97, N'RISO', CAST(N'2020-09-28T08:54:23.953' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (170, 420, N'RICHARD', N'KLECUN', 3, N'S', NULL, N'DSA', CAST(N'2020-09-28T08:54:24.107' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (171, 1985, N'KELLY', N'KNIGHTON', 3, N'R', 9, N'RLS', CAST(N'2020-09-28T08:54:24.250' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (172, 2128, N'BRENT', N'KREISER', 1, N'X', 9, N'RCMS', CAST(N'2020-09-28T08:54:24.393' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (173, 1709, N'ADAM', N'KRUPP', 2, N'P', 10, N'RCMS', CAST(N'2020-09-28T08:54:24.550' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (174, 1855, N'NICOLE', N'LECHNER', 1, N'Y', 5, N'PCO', CAST(N'2020-09-28T08:54:24.710' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (175, 2282, N'MARINA', N'LEVCHUK', 1, N'Y', 2, N'RISO', CAST(N'2020-09-28T08:54:24.860' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (176, 2372, N'MICHAEL', N'LICITRA', 3, N'U', 96, N'RISO', CAST(N'2020-09-28T08:54:25.010' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (177, 1073, N'ADAM', N'LORANGER', 1, N'Y', 3, N'RISO', CAST(N'2020-09-28T08:54:25.160' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (178, 2556, N'GARY', N'LUVERA', 1, N'X', NULL, N'DSA', CAST(N'2020-09-28T08:54:25.303' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (179, 2549, N'THOMAS', N'LUX', 2, N'Q', 7, N'RISO', CAST(N'2020-09-28T08:54:25.457' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (180, 1741, N'GREGORY', N'MACK', 2, N'P', 3, N'PCO', CAST(N'2020-09-28T08:54:25.613' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (181, 2524, N'RIPPY', N'MADAN', 1, N'Z', 10, N'RLS', CAST(N'2020-09-28T08:54:25.763' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (182, 2445, N'BENJAMIN', N'MALOWSKI', 1, N'X', 10, N'RCMS', CAST(N'2020-09-28T08:54:25.920' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (183, 2197, N'RUTH', N'MARTIN', 1, N'Y', 11, N'RISO', CAST(N'2020-09-28T08:54:26.087' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (184, 309, N'JOSEPH', N'MCCLELLAN', 3, N'S', 1, N'PCO', CAST(N'2020-09-28T08:54:26.260' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (185, 2984, N'TODD', N'MILLER', 3, N'T', NULL, N'DSA', CAST(N'2020-09-28T08:54:26.420' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (186, 2965, N'JEFFREY', N'MILNE', 3, N'S', 7, N'RLS', CAST(N'2020-09-28T08:54:26.563' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (187, 1470, N'MICHAEL', N'MULLIS', 3, N'T', 3, N'PCO', CAST(N'2020-09-28T08:54:26.703' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (188, 73, N'KIM', N'MYERS', 3, N'R', NULL, N'DSA', CAST(N'2020-09-28T08:54:26.857' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (189, 2329, N'OLEN', N'NOE', 1, N'Y', NULL, N'DSA', CAST(N'2020-09-28T08:54:27.010' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (190, 542, N'HECTOR', N'NORIEGA', 2, N'Q', 4, N'PCO', CAST(N'2020-09-28T08:54:27.460' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (191, 1602, N'JOSEPH', N'NORTON', 1, N'X', 1, N'PCO', CAST(N'2020-09-28T08:54:27.610' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (192, 2926, N'MAUREEN', N'O''SULLIVAN', 1, N'Z', 11, N'RLS', CAST(N'2020-09-28T08:54:27.767' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (193, 1567, N'BRIAN', N'OKKEN', 3, N'S', 2, N'PCO', CAST(N'2020-09-28T08:54:28.203' AS DateTime), N'System', NULL, NULL)
GO
INSERT [dbo].[CUDC_SpecialPermissions] ([Id], [EmployeeNumber], [FirstName], [LastName], [Region], [SE], [District], [Position], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (194, 1041, N'KAREN', N'ORTH', 1, N'X', 4, N'PCO', CAST(N'2020-09-28T08:54:28.367' AS DateTime), N'System', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[CUDC_SpecialPermissions] OFF
GO
ALTER TABLE [dbo].[CUDC_SpecialPermissions] ADD  CONSTRAINT [DF_CUDC_SpecialPermissions_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
