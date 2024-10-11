USE [NCUA]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--**********************************************************************************************************************
-- Get CUDC permission access
-- exec [dbo].[CUDC_GetUserPermissions] 'HQNT\FJIANG'
-- exec [dbo].[CUDC_GetUserPermissions] 'HQNT\NTONSIC'
-- exec [dbo].[CUDC_GetUserPermissions] 'HQNT\CBILOTTA'
-- exec [dbo].[CUDC_GetUserPermissions] 'HQNT\ASAUNDERS'
-- exec [dbo].[CUDC_GetUserPermissions] 'HQNT\PSPENDLOVE'
--**********************************************************************************************************************
-- Modified By: Rocio Borda Modified On: 10/02/2020	Modification: Added Other Special Permissions 
-- Modified By: Rocio Borda Modified On: 9/28/2020	Modification: Added Special Permissions 
-- Modified By: Rocio Borda Modified On: 9/24/2020	Modification: Added DOT division to RegMngmntDosMember 
--**********************************************************************************************************************
ALTER PROCEDURE [dbo].[CUDC_GetUserPermissions]
 @UserID varchar(100)  
AS 
BEGIN
-- **Left for testing purposes 
-- declare  @UserID varchar(100)  
--set @UserID ='HQNT\AOSTester2'

DECLARE @TempDosException varchar(10) = '1319'						-- INC0089186
DECLARE @SEException varchar(10) = '2553'							-- INC0088623

		IF EXISTS(select * from dbo.EmpSAPID s  inner join dbo.employee e  on s.EMPLOYEE_NUM = e.EMPLOYEE_NUM and s.LANLoginId = @UserID and s.Employee_NUM = @SEException) 
		BEGIN
			select top 1  1 sort, 'FieldStaff'  UserType, s.LANLoginId, e.EMPLOYEE_NUM,e.EMPLOYEE_TYPE, 1 as CanAccess, e.REGION, e.se, d.DISTRICT, 1 as EditCat, 0 as EditSe, 0 as EditDos, e.OFFICE, e.DIVISION   FROM DBO.EMPLOYEE E 
				join dbo.EmpSAPID s on s.Employee_NUM = e.EMPLOYEE_NUM
				left join EmployeeDistrict d on d.EMPLOYEE_NUM = e.EMPLOYEE_NUM
		    WHERE e.EMPLOYEE_NUM = @SEException			-- INC0088623	
		END ELSE 
		IF EXISTS(select * from dbo.EmpSAPID s  inner join dbo.employee e  on s.EMPLOYEE_NUM = e.EMPLOYEE_NUM and s.LANLoginId = @UserID and s.Employee_NUM in ('2593','2370','0899','2465','2656','1858','1871','2454','2932')) 
		BEGIN
			select top 1 3 sort, 'FieldStaff' UserType, s.LANLoginId, e.EMPLOYEE_NUM,e.EMPLOYEE_TYPE, 1 as CanAccess, e.REGION, 'D' se, 0 DISTRICT, 0 as EditCat, 1 as EditSe, 0 as EditDos, e.OFFICE, e.DIVISION FROM DBO.EMPLOYEE E 
				join dbo.EmpSAPID s on s.Employee_NUM = e.EMPLOYEE_NUM and LANLoginId = ISNULL(@UserID, LANLoginId) 
			where e.EMPLOYEE_NUM in ('2593','2370','0899','2465','2656','1858','1871','2454','2932')			--TASK0033313
		END ELSE IF EXISTS(select * from dbo.EmpSAPID s  inner join dbo.employee e  on s.EMPLOYEE_NUM = e.EMPLOYEE_NUM and s.LANLoginId = @UserID and s.Employee_NUM = @TempDosException) 
		BEGIN
			select top 1 3 sort, 'RegMngmntDosMember' UserType, s.LANLoginId, e.EMPLOYEE_NUM,e.EMPLOYEE_TYPE, 1 as CanAccess, e.REGION, e.se, 0 DISTRICT, 0 as EditCat, 1 as EditSe, 0 as EditDos, e.OFFICE, e.DIVISION FROM DBO.EMPLOYEE E 
				join dbo.EmpSAPID s on s.Employee_NUM = e.EMPLOYEE_NUM and LANLoginId = ISNULL(@UserID, LANLoginId) 
			where e.EMPLOYEE_NUM = 	@TempDosException	-- INC0089186
		END ELSE IF EXISTS(select * from dbo.EmpSAPID s  inner join dbo.CUDC_SpecialPermissions p  on s.EMPLOYEE_NUM = p.EmployeeNumber and s.LANLoginId = @UserID) 
		BEGIN
			select top 1 3 sort, 'SupervisorSEMember' UserType, s.LANLoginId, e.EMPLOYEE_NUM,e.EMPLOYEE_TYPE, 1 as CanAccess, e.REGION, e.se, 0 DISTRICT, 0 as EditCat, 1 as EditSe, 0 as EditDos, e.OFFICE, e.DIVISION FROM DBO.EMPLOYEE E 
				join dbo.EmpSAPID s on s.Employee_NUM = e.EMPLOYEE_NUM
				inner join dbo.CUDC_SpecialPermissions p  on e.EMPLOYEE_NUM = p.EmployeeNumber   AND LANLoginId = ISNULL(@UserID, LANLoginId) 			
		END ELSE
		BEGIN
			select top 1  1 sort, 'FieldStaff'  UserType, s.LANLoginId, e.EMPLOYEE_NUM,e.EMPLOYEE_TYPE, 1 as CanAccess, e.REGION, e.se, d.DISTRICT, 1 as EditCat, 0 as EditSe, 0 as EditDos, e.OFFICE, e.DIVISION   FROM DBO.EMPLOYEE E 
				join dbo.EmpSAPID s on s.Employee_NUM = e.EMPLOYEE_NUM
				left join EmployeeDistrict d on d.EMPLOYEE_NUM = e.EMPLOYEE_NUM
			WHERE e.EMPLOYEE_TYPE = 'K' and e.REGION in (1,2,3,8) and e.se is not null and d.DISTRICT is not null  AND LANLoginId = ISNULL(@UserID, LANLoginId)
			union 
			select top 1 3 sort, 'SupervisorSEMember' UserType, s.LANLoginId, e.EMPLOYEE_NUM,e.EMPLOYEE_TYPE, 1 as CanAccess, e.REGION, e.se, 0 DISTRICT, 0 as EditCat, 1 as EditSe, 0 as EditDos, e.OFFICE, e.DIVISION FROM DBO.EMPLOYEE E 
				join dbo.EmpSAPID s on s.Employee_NUM = e.EMPLOYEE_NUM
			--left join EmployeeDistrict d on d.EMPLOYEE_NUM = e.REGION     ** no district check
			WHERE e.EMPLOYEE_TYPE in ('J', 'F')  and e.REGION in (1,2,3,8)  AND LANLoginId = ISNULL(@UserID, LANLoginId) 
			union 
			select top 1 4 sort, 'RegMngmntDosMember' UserType, s.LANLoginId, e.EMPLOYEE_NUM,e.EMPLOYEE_TYPE, 1 as CanAccess, e.REGION, e.se, d.DISTRICT, 0 as EditCat, 0 as EditSe, 1 as EditDos, e.OFFICE, e.DIVISION FROM DBO.EMPLOYEE E 
				join dbo.EmpSAPID s on s.Employee_NUM = e.EMPLOYEE_NUM
			left join EmployeeDistrict d on d.EMPLOYEE_NUM = e.REGION
			WHERE (((e.DIVISION = 'DOS' OR e.DIVISION = 'DOT') and  e.EMPLOYEE_TYPE in ('I', 'K') ) or ( e.EMPLOYEE_TYPE in('A','B','C','E') ) )  and e.REGION in (1,2,3,8) and d.DISTRICT is null AND LANLoginId =  ISNULL(@UserID, LANLoginId) 

			union  
			select top 1 5 sort, 'OceEiObiMember' UserType, s.LANLoginId, e.EMPLOYEE_NUM,e.EMPLOYEE_TYPE, 1 as CanAccess, e.REGION, e.se, d.DISTRICT, 0 as EditCat, 0 as EditSe, 0 as EditDos, e.OFFICE, e.DIVISION FROM DBO.EMPLOYEE E 
				join dbo.EmpSAPID s on s.Employee_NUM = e.EMPLOYEE_NUM
			left join EmployeeDistrict d on d.EMPLOYEE_NUM = e.REGION
			WHERE (e.REGION in (9) and  e.EMPLOYEE_TYPE in ('M') ) AND e.SE IS NOT NULL AND D.DISTRICT IS NULL AND LANLoginId = ISNULL(@UserID, LANLoginId) 
			union
			select top 1 2 sort, 'PCOMember' UserType, s.LANLoginId, e.EMPLOYEE_NUM,e.EMPLOYEE_TYPE, 1 as CanAccess, e.REGION, e.se, d.DISTRICT, 1 as EditCat, 0 as EditSe, 0 as EditDos, e.OFFICE, e.DIVISION   FROM DBO.EMPLOYEE E 
				join dbo.EmpSAPID s on s.Employee_NUM = e.EMPLOYEE_NUM
				left join EmployeeDistrict d on d.EMPLOYEE_NUM = e.EMPLOYEE_NUM
			WHERE e.EMPLOYEE_TYPE = 'H' and e.REGION in (1,2,3,8) and e.se is not null  AND LANLoginId = ISNULL(@UserID, LANLoginId)

			order by sort
		END
END
