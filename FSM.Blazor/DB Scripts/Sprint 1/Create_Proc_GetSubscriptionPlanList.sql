/****** Object:  StoredProcedure [dbo].[GetSubscriptionPlanList]    Script Date: 29-03-2022 16:03:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetSubscriptionPlanList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'Name',  
    @SortOrder NVARCHAR(20) = 'ASC'
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
  print(@sortColumn)
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
      select * from ( select SP.*,temp.ModulesName from (select sp.Id,STRING_AGG(md.displayname,',') 
		as ModulesName from (SELECT SP.id, value  
		FROM SubscriptionPlans sp
		CROSS APPLY STRING_SPLIT(sp.ModuleIds, ',')) as sp
		join ModuleDetails md on SP.value = md.Id group by sp.Id)as temp
		join SubscriptionPlans SP on SP.Id = temp.id
) as tempData
        WHERE
			(tempData.IsDeleted = 0 OR  tempData.IsDeleted IS NULL) AND
			
			(@SearchValue= '' OR  (   
              tempData.Name LIKE '%' + @SearchValue + '%' OR
			  tempData.Price LIKE '%' + @SearchValue + '%' OR
			  tempData.[Duration(InMonths)] LIKE '%' + @SearchValue + '%' 
            ))  
  
            ORDER BY  
            CASE WHEN (@SortColumn = 'Name' AND @SortOrder='ASC')  
                        THEN [Name]  
            END ASC,  
            CASE WHEN (@SortColumn = 'Name' AND @SortOrder='DESC')  
                        THEN [Name]  
            END DESC, 
			CASE WHEN (@SortColumn = 'Price' AND @SortOrder='ASC')  
					
                        THEN Price  
            END ASC,  
            CASE WHEN (@SortColumn = 'Price' AND @SortOrder='DESC')  
                        THEN Price  
            END DESC, 
			CASE WHEN (@SortColumn = 'Duration' AND @SortOrder='ASC')  
                        THEN [Duration(InMonths)]  
            END ASC,  
            CASE WHEN (@SortColumn = 'Duration' AND @SortOrder='DESC')  
                        THEN [Duration(InMonths)]  
            END DESC
           
            OFFSET @PageSize * (@PageNo - 1) ROWS  
            FETCH NEXT @PageSize ROWS ONLY  
    ),  
    CTE_TotalRows AS   
    (  
        select count(sp.Id)  as TotalRecords from SubscriptionPlans SP
       WHERE
		(SP.IsDeleted = 0 OR  SP.IsDeleted IS NULL) AND
			
			(@SearchValue= '' OR  (   
              SP.Name LIKE '%' + @SearchValue + '%' OR
			  SP.Price LIKE '%' + @SearchValue + '%' OR
			  SP.[Duration(InMonths)] LIKE '%' + @SearchValue + '%' 
            ))  
   
    )  
    Select  TotalRecords, SP.Id, SP.Name, SP.ModuleIds, SP.ModulesName, SP.[Duration(InMonths)]
	, SP.IsActive, SP.IsCombo, SP.Price from CTE_Results SP
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = SP.ID)  
   
END
