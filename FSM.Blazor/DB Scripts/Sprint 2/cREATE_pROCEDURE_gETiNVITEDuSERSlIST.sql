alter PROCEDURE [dbo].[GetInvitedUsersList]      
(           
    @SearchValue NVARCHAR(50) = NULL,      
    @PageNo INT = 1,      
    @PageSize INT = 10,      
    @SortColumn NVARCHAR(20) = 'Email',      
    @SortOrder NVARCHAR(20) = 'ASC',    
 @CompanyId INT = NULL    
)      
AS BEGIN      
    SET NOCOUNT ON;      
      
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))      
      
    ; WITH CTE_Results AS       
    (      
        SELECT iu.*,ur.Name As Role from InvitedUsers iu  
  INNER JOIN UserRoles ur  
  on iu.RoleId = ur.Id  
      
        WHERE   
		(iu.IsDeleted = 0 OR  iu.IsDeleted IS NULL) AND      
  ((ISNULL(@CompanyId,0)=0)    
    OR (iu.CompanyId = @CompanyId))    
    and (@SearchValue= '' OR  (       
              Name LIKE '%' + @SearchValue + '%'     
            )  )    
      
            ORDER BY      
            CASE WHEN (@SortColumn = 'Email' AND @SortOrder='ASC')      
                        THEN iu.Email      
            END ASC,    
   CASE WHEN (@SortColumn = 'Email' AND @SortOrder='DESC')      
                        THEN iu.Email     
            END DESC    
    
            OFFSET @PageSize * (@PageNo - 1) ROWS      
            FETCH NEXT @PageSize ROWS ONLY      
    ),      
    CTE_TotalRows AS       
    (      
        select count(ID) as TotalRecords from Companies     
      
        WHERE    
  ((ISNULL(@CompanyId,0)=0)    
    OR (Id = @CompanyId))    
    and (@SearchValue= '' OR  (       
              Name LIKE '%' + @SearchValue + '%'     
            )  )    
    )      
    Select  * from CTE_Results     
 , CTE_TotalRows       
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = ID)      
       
END    
    
/****** Object:  StoredProcedure [dbo].[GetInstructorTypeList]    Script Date: 03-12-2021 16:50:57 ******/    
SET ANSI_NULLS ON 