USE [NCUA]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--****************************************************************************
-- Get CUDC permission access
-- Modified By: Rocio Borda   Modified On: 9/2/2020
-- exec [dbo].[CUDC_GetMemberPermissions] 'HQNT\GPAGE', 2, 65111, 'User'
--****************************************************************************
ALTER PROCEDURE [dbo].[CUDC_GetMemberPermissions]
   @UserID varchar(100) = null
 , @SurveyTypeId int = null
 , @CharterNumber int = null
 , @UserType varchar(20) = null
AS 
BEGIN
-- **Left for testing purposes 
--DECLARE  @UserID varchar(100), @SurveyTypeId int = 2, @CharterNumber int = 68709, @UserType varchar(20) = 'Admin'
--SET @UserID =	'HQNT\DANP'  

	DECLARE @BitFalse bit = 0, @BitTrue bit = 1, @Found bit = 0, @EmployeeNumber varchar(10) = null, @PermissionId int = null, @CATExistsClaimUser bit = 0
			, @SEExistsClaimUser bit = 0, @CATSubmittedForCurrentCU bit = 0, @SESubmittedForCurrentCU bit = 0, @rowCount int = 0

	-- Get employee number
	--SELECT e.REGION, e.SE, d.DISTRICT, e.DIVISION, e.EMPLOYEE_NUM, e.EMPLOYEE_TYPE, es.LANLoginId, e.*
	--from dbo.employee e 																
	--	INNER JOIN dbo.EmpSAPID es on e.EMPLOYEE_NUM = es.Employee_NUM AND e.STATUS = 3 AND es.LANLoginId = @UserID 
	--	left join dbo.EmployeeDistrict d on d.EMPLOYEE_NUM = e.EMPLOYEE_NUM									--DEBUG ONLY CODE
	--select c.region, c.se, c.district, * from dbo.cu030 c where c.CU_NUMBER = @CharterNumber				--DEBUG ONLY CODE
	--select @EmployeeNumber = e.EMPLOYEE_NUM from dbo.employee e 															
	--	INNER JOIN dbo.EmpSAPID es on e.EMPLOYEE_NUM = es.Employee_NUM AND e.STATUS = 3 AND es.LANLoginId = @UserID						--DEBUG ONLY CODE

	-- Determine if member has Special permissions
    select @EmployeeNumber = e.EMPLOYEE_NUM, @PermissionId = p.Id from dbo.employee e 															
		INNER JOIN dbo.EmpSAPID es on e.EMPLOYEE_NUM = es.Employee_NUM AND es.LANLoginId = @UserID
		INNER JOIN dbo.CUDC_Permissions p ON p.EmployeeNumber IS NOT NULL AND e.EMPLOYEE_NUM in (select * from SplitList(',', p.EmployeeNumber))	
	--select @EmployeeNumber , @PermissionId             						--DEBUG ONLY CODE

	IF @UserID IS NULL
	BEGIN
		SELECT top 1 1 Sort, 'NoAccess' AS Membership, CAST(3 as INT) as ErrorMessage, @BitFalse as IsCreated, @BitFalse as HasOwner, @BitFalse as IsOwner, @BitFalse as IsSubmitted, @BitFalse as CanEdit, @BitFalse as CanView
		FROM dbo.employee e where e.STATUS = 3
		select @rowCount = @@ROWCOUNT
	END ELSE  IF @UserType = 'Admin'
	BEGIN
		SELECT top 1 2 Sort, 'Admin' AS Membership, CAST(0 as INT) as ErrorMessage, @BitFalse as IsCreated, @BitFalse as HasOwner, @BitFalse as IsOwner, @BitFalse as IsSubmitted, @BitFalse as CanEdit, @BitTrue as CanView
		FROM dbo.employee e where e.STATUS = 3
		select @rowCount = @@ROWCOUNT
	END IF NOT EXISTS(SELECT * FROM DBO.CU030 C WHERE C.CU_NUMBER = @CharterNumber AND C.STATUS = 'A')
	BEGIN
		SELECT top 1 3 Sort, 'CUDoesNotExists' AS Membership, CAST(7 as INT) as ErrorMessage, @BitFalse as IsCreated, @BitFalse as HasOwner, @BitFalse as IsOwner, @BitFalse as IsSubmitted, @BitFalse as CanEdit, @BitFalse as CanView
		FROM dbo.employee e where e.STATUS = 3	
		select @rowCount = @@ROWCOUNT	
	END ELSE IF @UserType = 'User'
	BEGIN	
		-- Check if Survey Submitted
		select @CATSubmittedForCurrentCU = CAST((case when r.CuNumber is not null then 1 else 0 end) as bit) from dbo.CUDC_Responses r inner join dbo.CUDC_Surveys s on s.id = r.SurveyId where r.CuNumber = @CharterNumber and r.IsRejected is null and r.UserId IS NOT NULL AND r.SubmittedOn IS NOT NULL AND s.SurveyTypeId = 2	
		select @SESubmittedForCurrentCU = CAST((case when r.CuNumber is not null then 1 else 0 end) as bit) from dbo.CUDC_Responses r inner join dbo.CUDC_Surveys s on s.id = r.SurveyId where r.CuNumber = @CharterNumber and r.IsRejected is null and r.UserId IS NOT NULL AND r.SubmittedOn IS NOT NULL AND s.SurveyTypeId = 3		
		--select 'submitted', @CATSubmittedForCurrentCU, @SESubmittedForCurrentCU, @SurveyTypeId, @CharterNumber						--DEBUG ONLY CODE
		-- SEViewClaimCUOnly
		select @CATExistsClaimUser = CAST((case when r.CuNumber is not null then 1 else 0 end) as bit) from dbo.CUDC_Responses r inner join dbo.CUDC_Surveys s on s.id = r.SurveyId where r.CuNumber = @CharterNumber and r.IsRejected is null and r.UserId = @UserID AND s.SurveyTypeId = 2
		-- DOSViewClaimCUOnly
		select @SEExistsClaimUser = CAST((case when r.CuNumber is not null then 1 else 0 end) as bit) from dbo.CUDC_Responses r inner join dbo.CUDC_Surveys s on s.id = r.SurveyId where r.CuNumber = @CharterNumber and r.IsRejected is null and r.UserId = @UserID AND s.SurveyTypeId = 3
		--select 'claimed', @CATExistsClaimUser, @SEExistsClaimUser, @SurveyTypeId, @CharterNumber					--DEBUG ONLY CODE

		-- Field Staff		
		SELECT DISTINCT   4 Sort, p.Membership
			, CAST((CASE WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 2 AND ((@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId != p.EditSurvey AND (@CATExistsClaimUser = p.DOSViewClaimCUOnly OR @CATExistsClaimUser = p.SEViewClaimCUOnly))	OR (@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId = 2)) --DOSViewClaimCUOnly and SEViewClaimCUOnly
							AND ((e.REGION in (select * from SplitList(',', p.RegionAccess))) AND e.REGION = c.REGION) THEN 0									-- Regular Role Permission
						WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 3 AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 0		-- Special Role Permission
						WHEN (s.SurveyTypeId IS NULL AND c.region IS NOT NULL) THEN 0 
						ELSE 5			--Member missing permission to the Credit Union based on member se access.
						END 
				) AS INT) as ErrorMessage
			, CAST((CASE WHEN s.SurveyTypeId IS NOT NULL AND r.CuNumber IS NOT NULL THEN 1 ELSE 0 END) AS BIT) as IsCreated	 
			, CAST((CASE WHEN s.SurveyTypeId IS NOT NULL AND r.UserId IS NOT NULL THEN 1 ELSE 0 END) AS BIT) as HasOwner	 	    
			, CAST((CASE WHEN @CharterNumber = r.CuNumber and r.UserId = @UserID THEN 1 ELSE 0 END) AS BIT) as IsOwner	 	    
			, CAST((CASE WHEN @CharterNumber = r.CuNumber and r.SubmittedOn IS NOT NULL THEN 1 ELSE 0 END) AS BIT) as IsSubmitted
			, CAST((CASE WHEN (@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId  = p.EditSurvey AND @CharterNumber = r.CuNumber AND r.UserId = @UserID AND r.SubmittedOn IS NULL) THEN 1 ELSE 0 END) AS BIT) as CanEdit
			, CAST((CASE WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 2 AND ((@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId != p.EditSurvey AND (@CATExistsClaimUser = p.DOSViewClaimCUOnly OR @CATExistsClaimUser = p.SEViewClaimCUOnly))			--DOSViewClaimCUOnly and SEViewClaimCUOnly
						OR (@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId = 2)) AND ((e.REGION in (select * from SplitList(',', p.RegionAccess))) AND e.REGION = c.REGION) THEN 1									-- Regular Role Permission
						WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 3 AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 1		-- Special Role Permission
						WHEN (r.Id IS NULL AND c.region IS NOT NULL) THEN 1 
						ELSE 0
						END ) AS BIT) as CanView
			--, p.Id BRid,s.SurveyTypeId, p.DosViewClaimCUOnly, p.SeViewClaimCUOnly, r.SubmittedOn  --, r.Id
			--, r.UserId, r.IsRejected, p.RegionAccess, r.CuNumber, e.REGION EmpRegion, c.REGION as Region, e.SE EmpSE, c.SE as Se, d.DISTRICT EmpDistrict, c.DISTRICT AS District
		FROM dbo.employee e 															
			INNER JOIN dbo.EmpSAPID es on e.EMPLOYEE_NUM = es.Employee_NUM AND es.LANLoginId = @UserID
			LEFT JOIN dbo.CUDC_Responses r on r.CuNumber = @CharterNumber and r.IsRejected IS NULL 	
			INNER JOIN dbo.CUDC_Surveys s on s.id = r.SurveyId AND s.SurveyTypeId = @SurveyTypeId
			LEFT JOIN dbo.EmployeeDistrict d on e.EMPLOYEE_NUM = d.EMPLOYEE_NUM
			INNER JOIN [dbo].[CUDC_Permissions] p on p.ActingRole = 'F' 
				AND ((p.id = ISNULL(@PermissionId, p.id) and p.EmployeeNumber IS NULL) OR p.id = @PermissionId)					
				AND CASE WHEN p.EmpSeCondition = 2 AND p.RolePermission = 2 AND e.SE IS NOT NULL THEN 1										-- Regular Role Permission
							WHEN p.EmpSeCondition IS NULL AND p.RolePermission = 3 AND  e.SE in (select * from SplitList(',', p.SeAccess)) THEN 1			-- Special Role Permission
							WHEN p.EmpSeCondition IS NULL AND p.RolePermission = 3 AND  e.SE = ISNULL(p.SeAccess, e.SE) THEN 1								-- Special Role Permission
							ELSE 0
							END = 1					
				AND CASE WHEN p.EmpDistrictCondition = 2 AND p.RolePermission = 2 AND d.DISTRICT IS NOT NULL THEN 1										-- Regular Role Permission
							WHEN p.EmpDistrictCondition IS NULL AND p.RolePermission = 3 AND  d.DISTRICT in (select * from SplitList(',', p.DistrictAccess)) THEN 1	-- Special Role Permission
							WHEN p.EmpDistrictCondition IS NULL AND p.RolePermission = 3 AND  d.DISTRICT = ISNULL(p.DistrictAccess, d.DISTRICT) THEN 1				-- Special Role Permission
							ELSE 0
							END = 1	
			INNER JOIN dbo.CU030 c on c.CU_NUMBER = @CharterNumber 
			   AND CASE WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 2 
							 AND e.REGION in (select * from SplitList(',', p.RegionAccess))
							 AND e.REGION = c.REGION
							 THEN 1										-- Regular Role Permission
						WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 3 AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 1			-- Special Role Permission
						ELSE 0
						END = 1
			  AND c.SE = e.SE							
		WHERE   e.STATUS = 3
			AND e.EMPLOYEE_TYPE in (select * from SplitList(',', p.EmployeeType))
			AND p.IsActive = 1 	AND @UserType != 'Admin'

	UNION
	-- PCOMember						
		SELECT DISTINCT   5 Sort, p.Membership
			, CAST((CASE WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 2 AND ((@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId != p.EditSurvey AND (@CATExistsClaimUser = p.DOSViewClaimCUOnly OR @CATExistsClaimUser = p.SEViewClaimCUOnly))	OR (@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId = 2)) --DOSViewClaimCUOnly and SEViewClaimCUOnly
							AND ((e.REGION in (select * from SplitList(',', p.RegionAccess))) AND e.REGION = c.REGION) THEN 0									-- Regular Role Permission
						WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 3 AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 0		-- Special Role Permission
						WHEN (s.SurveyTypeId IS NULL AND c.region IS NOT NULL) THEN 0 
						ELSE 5			--Member missing permission to the Credit Union based on member se access.
						END 
				) AS INT) as ErrorMessage
			, CAST((CASE WHEN s.SurveyTypeId IS NOT NULL AND r.CuNumber IS NOT NULL THEN 1 ELSE 0 END) AS BIT) as IsCreated	 
			, CAST((CASE WHEN s.SurveyTypeId IS NOT NULL AND r.UserId IS NOT NULL THEN 1 ELSE 0 END) AS BIT) as HasOwner	 	    
			, CAST((CASE WHEN @CharterNumber = r.CuNumber and r.UserId = @UserID THEN 1 ELSE 0 END) AS BIT) as IsOwner	 	    
			, CAST((CASE WHEN @CharterNumber = r.CuNumber and r.SubmittedOn IS NOT NULL THEN 1 ELSE 0 END) AS BIT) as IsSubmitted
			, CAST((CASE WHEN (@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId  = p.EditSurvey AND @CharterNumber = r.CuNumber AND r.UserId = @UserID AND r.SubmittedOn IS NULL) THEN 1 ELSE 0 END) AS BIT) as CanEdit
			, CAST((CASE WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 2 AND ((@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId != p.EditSurvey AND (@CATExistsClaimUser = p.DOSViewClaimCUOnly OR @CATExistsClaimUser = p.SEViewClaimCUOnly))			--DOSViewClaimCUOnly and SEViewClaimCUOnly
						OR (@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId = 2)) AND ((e.REGION in (select * from SplitList(',', p.RegionAccess))) AND e.REGION = c.REGION) THEN 1									-- Regular Role Permission
						WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 3 AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 1		-- Special Role Permission
						WHEN (r.Id IS NULL AND c.region IS NOT NULL) THEN 1 
						ELSE 0
						END ) AS BIT) as CanView
			--, p.Id BRid,s.SurveyTypeId, p.DosViewClaimCUOnly, p.SeViewClaimCUOnly, r.SubmittedOn  --, r.Id
			--, r.UserId, r.IsRejected, p.RegionAccess, r.CuNumber, e.REGION EmpRegion, c.REGION as Region, e.SE EmpSE, c.SE as Se, d.DISTRICT EmpDistrict, c.DISTRICT AS District
		FROM dbo.employee e 															
			INNER JOIN dbo.EmpSAPID es on e.EMPLOYEE_NUM = es.Employee_NUM AND es.LANLoginId = @UserID
			LEFT JOIN dbo.CUDC_Responses r on r.CuNumber = @CharterNumber and r.IsRejected IS NULL 	
			INNER JOIN dbo.CUDC_Surveys s on s.id = r.SurveyId AND s.SurveyTypeId = @SurveyTypeId
			LEFT JOIN dbo.EmployeeDistrict d on e.EMPLOYEE_NUM = d.EMPLOYEE_NUM
			INNER JOIN [dbo].[CUDC_Permissions] p on p.ActingRole = 'P' 
				AND ((p.id = ISNULL(@PermissionId, p.id) and p.EmployeeNumber IS NULL) OR p.id = @PermissionId)					
				AND CASE WHEN p.RolePermission = 2 AND p.EmpSeCondition = 2 AND e.SE IS NOT NULL THEN 1										-- Regular Role Permission
							WHEN p.EmpSeCondition IS NULL AND p.RolePermission = 3 AND  e.SE in (select * from SplitList(',', p.SeAccess)) THEN 1			-- Special Role Permission
							WHEN p.EmpSeCondition IS NULL AND p.RolePermission = 3 AND  e.SE = ISNULL(p.SeAccess, e.SE) THEN 1								-- Special Role Permission
							ELSE 0
							END = 1					
				AND CASE WHEN p.EmpDistrictCondition = 3 AND p.RolePermission = 2  THEN 1										-- Regular Role Permission
							WHEN p.EmpDistrictCondition IS NULL AND p.RolePermission = 3 AND  d.DISTRICT in (select * from SplitList(',', p.DistrictAccess)) THEN 1	-- Special Role Permission
							WHEN p.EmpDistrictCondition IS NULL AND p.RolePermission = 3 AND  d.DISTRICT = ISNULL(p.DistrictAccess, d.DISTRICT) THEN 1				-- Special Role Permission
							ELSE 0
							END = 1	
			INNER JOIN dbo.CU030 c on c.CU_NUMBER = @CharterNumber 
			   AND CASE WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 2 
							 AND e.REGION in (select * from SplitList(',', p.RegionAccess))
							 AND e.REGION = c.REGION
							 THEN 1										-- Regular Role Permission
						WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 3 AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 1			-- Special Role Permission
						ELSE 0
						END = 1	
			  AND c.SE = e.SE					
		WHERE   e.STATUS = 1
			AND e.EMPLOYEE_TYPE in (select * from SplitList(',', p.EmployeeType))
			AND p.IsActive = 1 	AND @UserType != 'Admin'
	UNION				
		-- SupervisorSEMember
		SELECT DISTINCT   6 Sort, p.Membership
			, CAST((CASE WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 2  AND ((@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId = p.EditSurvey AND (@CATExistsClaimUser = p.DOSViewClaimCUOnly OR @CATExistsClaimUser = p.SEViewClaimCUOnly)) OR (@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId in(2,3)))
							AND ((e.REGION in (select * from SplitList(',', p.RegionAccess))) AND e.REGION = c.REGION) THEN 0
						WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 3 AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 0		-- Special Role Permission
						WHEN (s.SurveyTypeId IS NULL AND c.region IS NOT NULL) THEN 0 
						WHEN s.SurveyTypeId IS NULL THEN 2
						ELSE 4	--Member missing permission to the Credit Union based on member region access.
						END 
				) AS INT) as ErrorMessage
			, CAST((CASE WHEN s.SurveyTypeId IS NOT NULL AND r.CuNumber IS NOT NULL THEN 1 ELSE 0 END) AS BIT) as IsCreated	 
			, CAST((CASE WHEN  s.SurveyTypeId IS NOT NULL AND r.UserId IS NOT NULL THEN 1 ELSE 0 END) AS BIT) as HasOwner	 	    
			, CAST((CASE WHEN  @CharterNumber = r.CuNumber and r.UserId = @UserID THEN 1 ELSE 0 END) AS BIT) as IsOwner	 	    
			, CAST((CASE WHEN @CharterNumber = r.CuNumber and r.SubmittedOn IS NOT NULL THEN 1 ELSE 0 END) AS BIT) as IsSubmitted
			-- SE Claimed Survey exists and Survey is allowed to be edited within the charter number; and current user is the owner, and it has not been submitted yet
			, CAST((CASE WHEN @CATSubmittedForCurrentCU = 1 
			AND (@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId  = p.EditSurvey)	
			AND @CharterNumber = r.CuNumber	AND r.UserId = @UserID AND r.SubmittedOn IS NULL 
						THEN 1 ELSE 0 END) AS BIT) as CanEdit
				-- Survey within employee's parameters
			, CAST((CASE WHEN @CATSubmittedForCurrentCU = 1 AND p.RolePermission = 2 AND p.EmpRegionCondition IS NULL AND ((e.REGION in (select * from SplitList(',', p.RegionAccess))) AND e.REGION = c.REGION) 
				-- Survey is CAT/SE or Suvery DOS has an SE Claimed CU
				AND (@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId in(2,3) OR (s.SurveyTypeId = 4 AND p.DOSViewClaimCUOnly = @SEExistsClaimUser ))	THEN 1
						WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 3 AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 1		-- Special Role Permission
						WHEN (r.Id IS NULL AND c.region IS NOT NULL) THEN 1 
						WHEN s.SurveyTypeId IS NULL THEN 1
						ELSE 0
						END ) AS BIT) as CanView						
			--, p.Id BRid, s.SurveyTypeId, p.DosViewClaimCUOnly, p.SeViewClaimCUOnly, r.SubmittedOn  --, r.Id
			--, r.UserId, r.IsRejected, p.RegionAccess, r.CuNumber, e.REGION EmpRegion, c.REGION as Region, e.SE EmpSE, c.SE as Se, d.DISTRICT EmpDistrict, c.DISTRICT AS District
		FROM dbo.employee e 															
			INNER JOIN dbo.EmpSAPID es on e.EMPLOYEE_NUM = es.Employee_NUM AND es.LANLoginId = @UserID
			LEFT JOIN dbo.CUDC_Responses r on r.CuNumber = @CharterNumber and r.IsRejected IS NULL 	--r.UserId = es.LANLoginId AND 
			INNER JOIN dbo.CUDC_Surveys s on s.id = r.SurveyId AND s.SurveyTypeId = @SurveyTypeId
			LEFT JOIN dbo.EmployeeDistrict d on e.EMPLOYEE_NUM = d.EMPLOYEE_NUM
			INNER JOIN [dbo].[CUDC_Permissions] p on p.ActingRole = 'S' 
				AND ((p.id = ISNULL(@PermissionId, p.id) and p.EmployeeNumber IS NULL) OR p.id = @PermissionId)					
				AND CASE WHEN p.EmpSeCondition = 2 AND p.RolePermission = 2 AND e.SE IS NOT NULL THEN 1										-- Regular Role Permission
							WHEN p.EmpSeCondition IS NULL AND p.RolePermission = 3 AND  e.SE in (select * from SplitList(',', p.SeAccess)) 	THEN 1			-- Special Role Permission
							WHEN p.EmpSeCondition IS NULL AND p.RolePermission = 3 AND  e.SE = ISNULL(p.SeAccess, e.SE) THEN 1								-- Special Role Permission
							ELSE 0
							END = 1					
				AND CASE WHEN p.EmpDistrictCondition = 3 AND p.RolePermission = 2 THEN 1										-- Regular Role Permission
							WHEN p.EmpDistrictCondition IS NULL AND p.RolePermission = 3 AND  d.DISTRICT in (select * from SplitList(',', p.DistrictAccess)) THEN 1	-- Special Role Permission
							WHEN p.EmpDistrictCondition IS NULL AND p.RolePermission = 3 AND  d.DISTRICT = ISNULL(p.DistrictAccess, d.DISTRICT) THEN 1				-- Special Role Permission
							ELSE 0
							END = 1	
			INNER JOIN dbo.CU030 c on c.CU_NUMBER = @CharterNumber 
			   AND CASE WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 2 
							 AND e.REGION in (select * from SplitList(',', p.RegionAccess))
							 AND e.REGION = c.REGION
							 THEN 1
						WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 3 AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 1
						ELSE 0
						END = 1
		WHERE   e.STATUS = 3
			AND e.EMPLOYEE_TYPE in (select * from SplitList(',', p.EmployeeType))
			AND p.IsActive = 1 	AND @UserType != 'Admin'
	UNION
	-- RegMngmntDosMember
		SELECT DISTINCT   7 Sort, p.Membership
			, CAST((CASE WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 2  AND ((@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId = p.EditSurvey AND (@CATExistsClaimUser = p.DOSViewClaimCUOnly OR @CATExistsClaimUser = p.SEViewClaimCUOnly)) OR (@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId in(2,3)))
							AND ((e.REGION in (select * from SplitList(',', p.RegionAccess))) AND e.REGION = c.REGION) THEN 0
						WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 3 AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 0		-- Special Role Permission
						WHEN (s.SurveyTypeId IS NULL AND c.region IS NOT NULL) THEN 0 
						WHEN s.SurveyTypeId IS NULL THEN 2
						ELSE 6	--Member missing permission to the Credit Union based on member region access.
						END 
				) AS INT) as ErrorMessage
			, CAST((CASE WHEN s.SurveyTypeId IS NOT NULL AND r.CuNumber IS NOT NULL THEN 1 ELSE 0 END) AS BIT) as IsCreated	 
			, CAST((CASE WHEN s.SurveyTypeId IS NOT NULL AND r.UserId IS NOT NULL THEN 1 ELSE 0 END) AS BIT) as HasOwner	 	    
			, CAST((CASE WHEN @CharterNumber = r.CuNumber and r.UserId = @UserID THEN 1 ELSE 0 END) AS BIT) as IsOwner	 	    
			, CAST((CASE WHEN @CharterNumber = r.CuNumber and r.SubmittedOn IS NOT NULL THEN 1 ELSE 0 END) AS BIT) as IsSubmitted
			, CAST((CASE WHEN (@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId  = p.EditSurvey) AND @CharterNumber = r.CuNumber AND r.UserId = @UserID AND r.SubmittedOn IS NULL THEN 1 ELSE 0 END) AS BIT) as CanEdit
			, CAST((CASE WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 2  AND ((@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId = p.EditSurvey AND (@CATExistsClaimUser = p.DOSViewClaimCUOnly OR @CATExistsClaimUser = p.SEViewClaimCUOnly)) OR (@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId in(2,3)))
							AND ((e.REGION in (select * from SplitList(',', p.RegionAccess))) AND e.REGION = c.REGION) THEN 1
						WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 3 AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 1		-- Special Role Permission
						WHEN (r.Id IS NULL AND c.region IS NOT NULL) THEN 1 
						ELSE 0
						END ) AS BIT) as CanView						
			--, p.Id BRid,s.SurveyTypeId, p.DosViewClaimCUOnly, p.SeViewClaimCUOnly, r.SubmittedOn  --, r.Id
			--, r.UserId, r.IsRejected, p.RegionAccess, r.CuNumber, e.REGION EmpRegion, c.REGION as Region, e.SE EmpSE, c.SE as Se, d.DISTRICT EmpDistrict, c.DISTRICT AS District
		FROM dbo.employee e 															
			INNER JOIN dbo.EmpSAPID es on e.EMPLOYEE_NUM = es.Employee_NUM AND es.LANLoginId = @UserID
			LEFT JOIN dbo.CUDC_Responses r on r.CuNumber = @CharterNumber and r.IsRejected IS NULL 	--r.UserId = es.LANLoginId AND 
			INNER JOIN dbo.CUDC_Surveys s on s.id = r.SurveyId AND s.SurveyTypeId = @SurveyTypeId
			LEFT JOIN dbo.EmployeeDistrict d on e.EMPLOYEE_NUM = d.EMPLOYEE_NUM
			INNER JOIN [dbo].[CUDC_Permissions] p on p.ActingRole = 'D' 
				AND ((p.id = ISNULL(@PermissionId, p.id) and p.EmployeeNumber IS NULL) OR p.id = @PermissionId)					
				AND CASE WHEN p.EmpSeCondition = 3 AND p.RolePermission = 2 THEN 1										-- Regular Role Permission
							WHEN p.EmpSeCondition IS NULL AND p.RolePermission = 3 AND  e.SE in (select * from SplitList(',', p.SeAccess)) 	THEN 1			-- Special Role Permission
							WHEN p.EmpSeCondition IS NULL AND p.RolePermission = 3 AND  e.SE = ISNULL(p.SeAccess, e.SE) THEN 1								-- Special Role Permission
							ELSE 0
							END = 1					
				AND CASE WHEN  p.RolePermission = 2 AND p.EmpDistrictCondition = 1 AND (d.DISTRICT IS NULL OR d.DISTRICT = '') THEN 1										-- Regular Role Permission
							WHEN p.EmpDistrictCondition IS NULL AND p.RolePermission = 3 AND  d.DISTRICT in (select * from SplitList(',', p.DistrictAccess)) THEN 1	-- Special Role Permission
							WHEN p.EmpDistrictCondition IS NULL AND p.RolePermission = 3 AND  d.DISTRICT = ISNULL(p.DistrictAccess, d.DISTRICT) THEN 1				-- Special Role Permission
							ELSE 0
							END = 1	
			INNER JOIN dbo.CU030 c on c.CU_NUMBER = @CharterNumber 
			   AND CASE WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 2 
							 AND e.REGION in (select * from SplitList(',', p.RegionAccess))
							 AND e.REGION = c.REGION
							 THEN 1
						WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 3 AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 1
						ELSE 0
						END = 1
		WHERE   e.STATUS = 3
			AND e.EMPLOYEE_TYPE in (select * from SplitList(',', p.EmployeeType))
			AND p.IsActive = 1 	AND @UserType != 'Admin'
	UNION
	-- OceEiObiMember
		SELECT DISTINCT   8 Sort, p.Membership
			, CAST((CASE WHEN p.RolePermission = 2  AND p.EmpRegionCondition = 9 AND (@SurveyTypeId = s.SurveyTypeId) AND (c.REGION in (select * from SplitList(',', p.RegionAccess)))  THEN 0	 -- Regular Role Permission
						WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 3 AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 0		-- Special Role Permission
						WHEN (s.SurveyTypeId IS NULL AND c.region IS NOT NULL) THEN 0 
						ELSE 3	--Member missing permission to the Credit Union based on member region access.
						END 
				) AS INT) as ErrorMessage
			, CAST((CASE WHEN s.SurveyTypeId IS NOT NULL AND r.CuNumber IS NOT NULL THEN 1 ELSE 0 END) AS BIT) as IsCreated	 
			, CAST((CASE WHEN s.SurveyTypeId IS NOT NULL AND r.UserId IS NOT NULL THEN 1 ELSE 0 END) AS BIT) as HasOwner	 	    
			, CAST((CASE WHEN @CharterNumber = r.CuNumber and r.UserId = @UserID THEN 1 ELSE 0 END) AS BIT) as IsOwner	 	    
			, CAST((CASE WHEN @CharterNumber = r.CuNumber and r.SubmittedOn IS NOT NULL THEN 1 ELSE 0 END) AS BIT) as IsSubmitted
			, CAST((CASE WHEN (@SurveyTypeId = s.SurveyTypeId AND s.SurveyTypeId  = p.EditSurvey) AND @CharterNumber = r.CuNumber AND r.UserId = @UserID AND r.SubmittedOn IS NULL THEN 1 ELSE 0 END) AS BIT) as CanEdit
			, CAST((CASE WHEN p.RolePermission = 2  AND p.EmpRegionCondition = e.REGION AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 1
						WHEN p.RolePermission = 3 AND p.EmpRegionCondition IS NULL AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 1		-- Special Role Permission
						WHEN (r.Id IS NULL AND c.region IS NOT NULL) THEN 1 
						ELSE 0
						END ) AS BIT) as CanView						
			--, p.Id BRid,s.SurveyTypeId, p.DosViewClaimCUOnly, p.SeViewClaimCUOnly, r.SubmittedOn  --, r.Id
			--, r.UserId, r.IsRejected, p.RegionAccess, r.CuNumber, e.REGION EmpRegion, c.REGION as Region, e.SE EmpSE, c.SE as Se, d.DISTRICT EmpDistrict, c.DISTRICT AS District
		FROM dbo.employee e 															
			INNER JOIN dbo.EmpSAPID es on e.EMPLOYEE_NUM = es.Employee_NUM AND es.LANLoginId = @UserID
			LEFT JOIN dbo.CUDC_Responses r on r.CuNumber = @CharterNumber and r.IsRejected IS NULL 	--r.UserId = es.LANLoginId AND 
			INNER JOIN dbo.CUDC_Surveys s on s.id = r.SurveyId AND s.SurveyTypeId = @SurveyTypeId
			LEFT JOIN dbo.EmployeeDistrict d on e.EMPLOYEE_NUM = d.EMPLOYEE_NUM
			INNER JOIN [dbo].[CUDC_Permissions] p on p.ActingRole = 'V' 
				AND ((p.id = ISNULL(@PermissionId, p.id) and p.EmployeeNumber IS NULL) OR p.id = @PermissionId)					
				AND CASE WHEN  p.RolePermission = 2 AND p.EmpSeCondition = 1 AND (e.SE IS NULL OR e.SE = '') THEN 1										-- Regular Role Permission
							WHEN p.EmpSeCondition IS NULL AND p.RolePermission = 3 AND  e.SE in (select * from SplitList(',', p.SeAccess)) 	THEN 1			-- Special Role Permission
							WHEN p.EmpSeCondition IS NULL AND p.RolePermission = 3 AND  e.SE = ISNULL(p.SeAccess, e.SE) THEN 1								-- Special Role Permission
							ELSE 0
							END = 1					
				AND CASE WHEN  p.RolePermission = 2 AND p.EmpDistrictCondition = 1 AND (d.DISTRICT IS NULL OR d.DISTRICT = '') THEN 1										-- Regular Role Permission
							WHEN p.EmpDistrictCondition IS NULL AND p.RolePermission = 3 AND  d.DISTRICT in (select * from SplitList(',', p.DistrictAccess)) THEN 1	-- Special Role Permission
							WHEN p.EmpDistrictCondition IS NULL AND p.RolePermission = 3 AND  d.DISTRICT = ISNULL(p.DistrictAccess, d.DISTRICT) THEN 1				-- Special Role Permission
							ELSE 0
							END = 1	
			INNER JOIN dbo.CU030 c on c.CU_NUMBER = @CharterNumber 
			   AND CASE WHEN p.RolePermission = 2 AND p.EmpRegionCondition = e.REGION 
							 AND c.REGION in (select * from SplitList(',', p.RegionAccess))
							 THEN 1
						WHEN p.RolePermission = 3 AND p.EmpRegionCondition IS NULL AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 1
						ELSE 0
						END = 1
		WHERE   e.STATUS = 3
			--AND e.EMPLOYEE_TYPE in (select * from SplitList(',', p.EmployeeType)) 
			AND e.REGION = p.EmpRegionCondition
			AND p.IsActive = 1	AND @UserType != 'Admin'

		select @rowCount = @@ROWCOUNT
	END

	--select @rowCount			--DEBUG CODE
	IF @rowCount = 0 
	BEGIN
		IF (@CATSubmittedForCurrentCU = 0)
		BEGIN			
			SELECT top 1 13 Sort, 'SurveyNoCreated' AS Membership, CAST(2 as INT) as ErrorMessage, @BitFalse as IsCreated, @BitFalse as HasOwner, @BitFalse as IsOwner, @BitFalse as IsSubmitted, @BitFalse as CanEdit, @BitFalse as CanView
			FROM dbo.employee e where e.STATUS = 3
		END ELSE
		IF (@SurveyTypeId = 2  and @UserType != 'Admin' AND (@CATExistsClaimUser != 1 AND @CATSubmittedForCurrentCU != 1))
		BEGIN
			SELECT DISTINCT 9 Sort, 'NewCAT' AS Membership, CAST(0 as INT) as ErrorMessage, @BitFalse as IsCreated, @BitFalse as HasOwner, @BitFalse as IsOwner, @BitFalse as IsSubmitted, @BitTrue as CanEdit, @BitTrue as CanView
			FROM dbo.employee e 
			INNER JOIN dbo.EmpSAPID es on es.Employee_NUM = e.EMPLOYEE_NUM and es.LANLoginId = @UserID
			INNER JOIN [dbo].[CUDC_Permissions] p on p.ActingRole IN('F','P')
			INNER JOIN dbo.CU030 c on c.CU_NUMBER = @CharterNumber 
			   AND CASE WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 2 
							 AND e.REGION in (select * from SplitList(',', p.RegionAccess))
							 AND e.REGION = c.REGION
							 THEN 1
						WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 3 AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 1
						ELSE 0
						END = 1
			  AND c.SE = e.SE	
			where e.STATUS = 3 AND c.STATUS = 'A'
		END ELSE IF ( @SurveyTypeId = 3  and @UserType != 'Admin' )--EXISTS(SELECT * FROM dbo.cu030 c where c.CU_NUMBER = @CharterNumber)
		BEGIN
			SELECT DISTINCT 10 Sort, 'NewSE' AS Membership, CAST(0 as INT) as ErrorMessage, @BitFalse as IsCreated, @BitFalse as HasOwner, @BitFalse as IsOwner, @BitFalse as IsSubmitted, @BitTrue as CanEdit, @BitTrue as CanView
			FROM dbo.employee e 
			INNER JOIN dbo.EmpSAPID es on es.Employee_NUM = e.EMPLOYEE_NUM and es.LANLoginId = @UserID
			INNER JOIN [dbo].[CUDC_Permissions] p on p.ActingRole = 'S' 
			INNER JOIN dbo.CU030 c on c.CU_NUMBER = @CharterNumber 
			   AND CASE WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 2 
							 AND e.REGION in (select * from SplitList(',', p.RegionAccess))
							 AND e.REGION = c.REGION
							 THEN 1
						WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 3 AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 1
						ELSE 0
						END = 1
			where e.STATUS = 3 AND c.STATUS = 'A'
		END ELSE IF ( @SurveyTypeId = 4 and @UserType != 'Admin')
		BEGIN
			SELECT DISTINCT 11 Sort, 'NewDOS' AS Membership, CAST(0 as INT) as ErrorMessage, @BitFalse as IsCreated, @BitFalse as HasOwner, @BitFalse as IsOwner, @BitFalse as IsSubmitted, @BitTrue as CanEdit, @BitTrue as CanView
			FROM dbo.employee e 
			INNER JOIN dbo.EmpSAPID es on es.Employee_NUM = e.EMPLOYEE_NUM and es.LANLoginId = @UserID
			INNER JOIN [dbo].[CUDC_Permissions] p on p.ActingRole = 'D' 
			INNER JOIN dbo.CU030 c on c.CU_NUMBER = @CharterNumber 
			   AND CASE WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 2 
							 AND e.REGION in (select * from SplitList(',', p.RegionAccess))
							 AND e.REGION = c.REGION
							 THEN 1
						WHEN p.EmpRegionCondition IS NULL AND p.RolePermission = 3 AND c.REGION in (select * from SplitList(',', p.RegionAccess)) THEN 1
						ELSE 0
						END = 1
			where e.STATUS = 3 AND c.STATUS = 'A'
		END 

		IF @@ROWCOUNT = 0 
		BEGIN
			SELECT top 1 12 Sort, 'NoAccess' AS Membership, CAST(3 as INT) as ErrorMessage, @BitFalse as IsCreated, @BitFalse as HasOwner, @BitFalse as IsOwner, @BitFalse as IsSubmitted, @BitFalse as CanEdit, @BitFalse as CanView
			FROM dbo.employee e where e.STATUS = 3
		END
	END

	
END
GO
GRANT EXECUTE ON [dbo].[CUDC_GetMemberPermissions] TO  [HQNT\DevWebOperator]  
GO
GRANT EXECUTE ON [dbo].[CUDC_GetMemberPermissions] TO  [HQNT\WebOperator] 
GO 