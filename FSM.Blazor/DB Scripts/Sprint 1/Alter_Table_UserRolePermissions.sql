ALTER TABLE UserRolePermissions 
ADD IsAllowedForMobileApp bit NOT NULL DEFAULT (0)
GO

ALTER PROCEDURE [dbo].[GetUserRolePermissionList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'UserRole',  
    @SortOrder NVARCHAR(20) = 'ASC'  ,
	@ModuleId INT = NULL,
	@RoleId INT = NULL,
	@CompanyId INT = NULL
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
        SELECT URP.*,UR.Name,PR.PermissionType,MD.Name AS ModuleName,
		MD.OrderNo from UserRolePermissions URP
		LEFT JOIN  UserRoles UR on UR.Id = URP.RoleId
		LEFT JOIN  Permissions PR on PR.Id = URP.PermissionId 
		LEFT JOIN  ModuleDetails MD on MD.Id = URP.ModuleId
		LEFT JOIN  Companies CP on CP.Id = URP.CompanyId
		
        WHERE 
			(CP.IsDeleted = 0 OR CP.IsDeleted IS NULL) AND
			1 = 1 AND 
		      (
				((ISNULL(@ModuleId,0)=0)
				OR (URP.ModuleId = @ModuleId))
				AND ((ISNULL(@RoleId,0)=0) 
				OR (URP.RoleId = @RoleId))
				AND ((ISNULL(@CompanyId,0)=0)
				OR (URP.CompanyId = @CompanyId))
		      )
			  
			  AND
			  (@SearchValue= '' OR  (   
              UR.Name LIKE '%' + @SearchValue + '%' OR
			  MD.Name LIKE '%' + @SearchValue + '%' OR
			  PR.PermissionType LIKE '%' + @SearchValue + '%' OR
			  URP.IsAllowed  LIKE '%' + @SearchValue + '%'
            ) )
            ORDER BY  
            CASE WHEN (@SortColumn = 'RoleName' AND @SortOrder='ASC')  
                        THEN UR.Name  
            END ASC,  
            CASE WHEN (@SortColumn = 'RoleName' AND @SortOrder='DESC')  
                        THEN UR.Name  
            END DESC, 
			CASE WHEN (@SortColumn = 'DisplayName' AND @SortOrder='ASC')  
                        THEN MD.Name  
            END ASC,  
            CASE WHEN (@SortColumn = 'DisplayName' AND @SortOrder='DESC')  
                        THEN MD.Name  
            END DESC, 
			CASE WHEN (@SortColumn = 'PermissionType' AND @SortOrder='ASC')  
                        THEN PR.PermissionType  
            END ASC,  
            CASE WHEN (@SortColumn = 'PermissionType' AND @SortOrder='DESC')  
                        THEN PR.PermissionType  
            END DESC, 
			CASE WHEN (@SortColumn = 'IsAllowed' AND @SortOrder='ASC')  
                        THEN URP.IsAllowed
            END ASC,  
            CASE WHEN (@SortColumn = 'IsAllowed' AND @SortOrder='DESC')  
                         THEN URP.IsAllowed
            END DESC, 
			CASE WHEN (@SortColumn = 'IsAllowedForMobileApp' AND @SortOrder='ASC')  
                        THEN URP.IsAllowed
            END ASC,  
            CASE WHEN (@SortColumn = 'IsAllowedForMobileApp' AND @SortOrder='DESC')  
                         THEN URP.IsAllowed
            END DESC 
            OFFSET @PageSize * (@PageNo - 1) ROWS  
            FETCH NEXT @PageSize ROWS ONLY  
    ),  
    CTE_TotalRows AS   
    (  
        select count(URP.ID) as TotalRecords  from UserRolePermissions URP
		LEFT JOIN  UserRoles UR on UR.Id = URP.RoleId
		LEFT JOIN  Permissions PR on PR.Id = URP.PermissionId 
		LEFT JOIN  ModuleDetails MD on MD.Id = URp.ModuleId
		LEFT JOIN  Companies CP on CP.Id = URP.CompanyId

        WHERE 
			(CP.IsDeleted = 0 OR CP.IsDeleted IS NULL) AND
			1 = 1 AND 
		      (
				((ISNULL(@ModuleId,0)=0)
				OR (URP.ModuleId = @ModuleId))
				AND ((ISNULL(@RoleId,0)=0) 
				OR (URP.RoleId = @RoleId))
				AND ((ISNULL(@CompanyId,0)=0)
				OR (URP.CompanyId = @CompanyId))
		      ) AND
			  (@SearchValue= '' OR  (   
              UR.Name LIKE '%' + @SearchValue + '%' OR
			  MD.Name LIKE '%' + @SearchValue + '%' OR
			  PR.PermissionType LIKE '%' + @SearchValue + '%' OR
			  URP.IsAllowed  LIKE '%' + @SearchValue + '%'
            ))   
    )  

    Select TotalRecords, URP.*,UR.Name  AS RoleName,PR.PermissionType,
		   MD.Name ModuleName,MD.ControllerName, MD.ActionName, MD.Icon, MD.DisplayName,
		   MD.OrderNo from UserRolePermissions URP
		LEFT JOIN  UserRoles UR on UR.Id = URP.RoleId
		LEFT JOIN  Permissions PR on PR.Id = URP.PermissionId 
		LEFT JOIN  ModuleDetails MD on MD.Id = URp.ModuleId
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = URP.ID)  
   
END
GO