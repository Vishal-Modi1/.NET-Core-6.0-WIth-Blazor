/****** Object:  StoredProcedure [dbo].[GetDocumentList]    Script Date: 16-03-2022 09:34:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetDocumentList]      
(           
    @SearchValue NVARCHAR(50) = NULL,      
    @PageNo INT = 1,      
    @PageSize INT = 10,      
    @SortColumn NVARCHAR(20) = 'DisplayName',      
    @SortOrder NVARCHAR(20) = 'ASC',    
	@CompanyId INT = NULL,    
	@ModuleId INT = NULL,
	@UserId bigint = NULL
)      
AS BEGIN      
    SET NOCOUNT ON;      
      
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))      
      
    ; WITH CTE_Results AS       
    (      
        SELECT d.Id, d.Name,d.UserId ,d.DisplayName, d.ExpirationDate, d.CompanyId,
		d.Type,ISNULL(d.[Size(InKB)], 0) as Size, m.Name as ModuleName,
		CP.Name as CompanyName, u.FirstName + ' ' + u.LastName as UserName   from Documents d    
		LEFT JOIN  Companies CP on CP.Id = d.CompanyId    
		LEFT JOIN ModuleDetails m on d.ModuleId = m.Id
		LEFT JOIN Users u on d.UserId = u.Id
    
        WHERE    
   (CP.IsDeleted = 0 OR  CP.IsDeleted IS NULL) AND    
   1 = 1 AND     
        (    
    ((ISNULL(@CompanyId,0)=0)    
    OR (d.CompanyId = @CompanyId))    
        
        ) 
		AND     
        (    
    ((ISNULL(@ModuleId,0)=0)    
    OR (d.ModuleId = @ModuleId))    
        
        ) 

		AND     
        (    
    ((ISNULL(@UserId,0)=0)    
    OR (d.UserId = @UserId))    
        
        ) 
   AND     
   d.IsDeleted = 0 AND    
   (@SearchValue= '' OR  (       
              d.DisplayName LIKE '%' + @SearchValue + '%' OR    
     Type LIKE '%' + @SearchValue + '%' OR    
     CP.Name LIKE '%' + @SearchValue + '%'    
            ))      
      
            ORDER BY      
            CASE WHEN (@SortColumn = 'DisplayName' AND @SortOrder='ASC')      
                        THEN d.DisplayName      
            END ASC,      
            CASE WHEN (@SortColumn = 'DisplayName' AND @SortOrder='DESC')      
                        THEN d.DisplayName      
            END DESC,     
   CASE WHEN (@SortColumn = 'Type' AND @SortOrder='ASC')      
                        THEN [Type]    
            END ASC,      
            CASE WHEN (@SortColumn = 'Type' AND @SortOrder='DESC')      
                        THEN [Type]     
            END DESC,     
   CASE WHEN (@SortColumn = 'Size' AND @SortOrder='ASC')      
                        THEN [Size(InKB)]      
            END ASC,      
            CASE WHEN (@SortColumn = 'Size' AND @SortOrder='DESC')      
                        THEN [Size(InKB)]      
            END DESC,  
	CASE WHEN (@SortColumn = 'ModuleName' AND @SortOrder='ASC')      
                        THEN m.Name    
            END ASC,      
            CASE WHEN (@SortColumn = 'ModuleName' AND @SortOrder='DESC')      
                        THEN m.Name     
            END DESC,
   CASE WHEN (@SortColumn = 'CompanyName' AND @SortOrder='ASC')      
                        THEN CP.Name    
            END ASC,      
            CASE WHEN (@SortColumn = 'CompanyName' AND @SortOrder='DESC')      
                        THEN CP.Name     
            END DESC,
	CASE WHEN (@SortColumn = 'UserName' AND @SortOrder='ASC')      
                        THEN u.FirstName    
            END ASC,      
            CASE WHEN (@SortColumn = 'UserName' AND @SortOrder='DESC')      
                        THEN u.FirstName     
            END DESC
            OFFSET @PageSize * (@PageNo - 1) ROWS      
            FETCH NEXT @PageSize ROWS ONLY      
    ),      
    CTE_TotalRows AS       
    (      
        select count(d.Id) as TotalRecords from Documents d    
  LEFT JOIN  Companies CP on CP.Id = d.CompanyId    
  LEFT JOIN ModuleDetails m on d.ModuleId = m.Id
    LEFT JOIN Users u on d.UserId = u.Id
        WHERE    
   (CP.IsDeleted = 0 OR  CP.IsDeleted IS NULL) AND    
   1 = 1 AND     
        (    
    ((ISNULL(@CompanyId,0)=0)    
    OR (d.CompanyId = @CompanyId))    
        )
		AND     
        (    
    ((ISNULL(@ModuleId,0)=0)    
    OR (d.ModuleId = @ModuleId))    
        
        ) 
		AND     
        (    
    ((ISNULL(@UserId,0)=0)    
    OR (d.UserId = @UserId))    
        
        ) 
   AND     
   d.IsDeleted = 0 AND    
   (@SearchValue= '' OR  (       
              d.DisplayName LIKE '%' + @SearchValue + '%' OR    
     Type LIKE '%' + @SearchValue + '%' OR    
     CP.Name LIKE '%' + @SearchValue + '%'    
            ))      
       
    )      
    Select TotalRecords, d.Id,d.UserId, d.Name, d.DisplayName, d.ExpirationDate, d.CompanyId,
	d.Type,CONCAT(ISNULL(d.[Size(InKB)], 0), ' KB') as Size, m.Name as ModuleName, 
	CP.Name as CompanyName, u.FirstName + ' ' + u.LastName as UserName from Documents d    
	LEFT JOIN  Companies CP on CP.Id = d.CompanyId  
	LEFT JOIN ModuleDetails m on d.ModuleId = m.Id
	LEFT JOIN Users u on d.UserId = u.Id
 , CTE_TotalRows       
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = d.Id)      
       
END    
    