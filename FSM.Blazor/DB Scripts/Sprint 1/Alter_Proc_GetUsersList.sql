/****** Object:  StoredProcedure [dbo].[GetUsersList]    Script Date: 10-05-2022 17:53:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetUsersList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'FirstName',  
    @SortOrder NVARCHAR(20) = 'ASC',
	@CompanyId INT = NULL,
	@RoleId INT = NULL
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
        SELECT U.*,UR.Name, CP.Name CompanyName from Users U
		LEFT JOIN  UserRoles UR on UR.Id= U.RoleId
		LEFT JOIN  Companies CP on CP.Id = U.CompanyId

        WHERE
			(CP.IsDeleted = 0 OR  CP.IsDeleted IS NULL) AND
			1 = 1 AND 
		      (
				((ISNULL(@CompanyId,0)=0)
				OR (U.CompanyId = @CompanyId))
				AND ((ISNULL(@RoleId,0)=0) 
				OR (UR.Id = @RoleId))
		      )
			AND 
			U.IsDeleted = 0 AND
			(@SearchValue= '' OR  (   
              FirstName LIKE '%' + @SearchValue + '%' OR
			  LastName LIKE '%' + @SearchValue + '%' OR
			  Email LIKE '%' + @SearchValue + '%' OR
			  UR.Name LIKE '%' + @SearchValue + '%' OR
			  CP.Name LIKE '%' + @SearchValue + '%'
            ))  
  
            ORDER BY  
            CASE WHEN (@SortColumn = 'FirstName' AND @SortOrder='ASC')  
                        THEN FirstName  
            END ASC,  
            CASE WHEN (@SortColumn = 'FirstName' AND @SortOrder='DESC')  
                        THEN FirstName  
            END DESC, 
			CASE WHEN (@SortColumn = 'LastName' AND @SortOrder='ASC')  
                        THEN LastName  
            END ASC,  
            CASE WHEN (@SortColumn = 'LastName' AND @SortOrder='DESC')  
                        THEN LastName  
            END DESC, 
			CASE WHEN (@SortColumn = 'Email' AND @SortOrder='ASC')  
                        THEN Email  
            END ASC,  
            CASE WHEN (@SortColumn = 'Email' AND @SortOrder='DESC')  
                        THEN Email  
            END DESC, 
            CASE WHEN (@SortColumn = 'UserRole' AND @SortOrder='ASC')  
                        THEN UR.Name  
            END ASC,  
            CASE WHEN (@SortColumn = 'UserRole' AND @SortOrder='DESC')  
                        THEN UR.Name  
            END DESC,
			CASE WHEN (@SortColumn = 'IsInstructor' AND @SortOrder='ASC')  
                        THEN U.IsInstructor 
            END ASC,  
            CASE WHEN (@SortColumn = 'IsInstructor' AND @SortOrder='DESC')  
                        THEN U.IsInstructor  
            END DESC, 
			CASE WHEN (@SortColumn = 'IsActive' AND @SortOrder='ASC')  
                        THEN U.IsActive  
            END ASC,  
            CASE WHEN (@SortColumn = 'IsActive' AND @SortOrder='DESC')  
                        THEN U.IsActive 
            END DESC,
			CASE WHEN (@SortColumn = 'CompanyName' AND @SortOrder='ASC')  
                        THEN CP.Name
            END ASC,  
            CASE WHEN (@SortColumn = 'CompanyName' AND @SortOrder='DESC')  
                        THEN CP.Name 
            END DESC
            OFFSET @PageSize * (@PageNo - 1) ROWS  
            FETCH NEXT @PageSize ROWS ONLY  
    ),  
    CTE_TotalRows AS   
    (  
        select count(U.ID) as TotalRecords from Users U
		LEFT JOIN  UserRoles UR on UR.Id =U.RoleId
		LEFT JOIN  Companies CP on CP.Id = U.CompanyId
       WHERE
		(CP.IsDeleted = 0 OR  CP.IsDeleted IS NULL) AND
			1 = 1 AND 
		      (
				((ISNULL(@CompanyId,0)=0)
				OR (U.CompanyId = @CompanyId))
				AND ((ISNULL(@RoleId,0)=0) 
				OR (UR.Id = @RoleId))
		      )
			AND 
			U.IsDeleted = 0 AND
			(@SearchValue= '' OR  (   
              FirstName LIKE '%' + @SearchValue + '%' OR
			  LastName LIKE '%' + @SearchValue + '%' OR
			  Email LIKE '%' + @SearchValue + '%' OR
			  UR.Name LIKE '%' + @SearchValue + '%' OR
			  CP.Name LIKE '%' + @SearchValue + '%'
            ))  
   
    )  
    Select TotalRecords, U.Id, U.FirstName, U.LastName,U.Email,ISNULL(U.IsInstructor, 0 ) 
	AS IsInstructor,ISNULL(U.IsActive, 0 ) AS IsActive,Ur.Name as UserRole, CP.Id AS CompanyId,
	CP.Name AS CompanyName,	U.ImageName as ProfileImage from Users U
	LEFT JOIN  UserRoles UR on UR.Id=U.RoleId
	LEFT JOIN  Companies CP on CP.Id = U.CompanyId
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = U.ID)  
   
END
