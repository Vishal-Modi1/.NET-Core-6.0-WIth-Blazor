/****** Object:  StoredProcedure [dbo].[GetBillingHistoryList]    Script Date: 31-03-2022 17:00:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetBillingHistoryList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'CreatedOn',  
    @SortOrder NVARCHAR(20) = 'DESC'
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
  print(@sortColumn)
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
      select * from ( select BH.*,temp.ModulesName from (select bh.Id,STRING_AGG(md.displayname,',') 
		as ModulesName from (SELECT BH.id, value  
		FROM BillingHistories bh
		CROSS APPLY STRING_SPLIT(bh.ModuleIds, ',')) as bh
		join ModuleDetails md on BH.value = md.Id group by bh.Id)as temp
		join BillingHistories BH on BH.Id = temp.id
) as tempData
        WHERE
			
			(@SearchValue= '' OR  (   
              tempData.SubscriptionPlanName LIKE '%' + @SearchValue + '%' OR
			  tempData.Price LIKE '%' + @SearchValue + '%' OR
			  tempData.[Duration] LIKE '%' + @SearchValue + '%' 
            ))  
  
            ORDER BY  
            CASE WHEN (@SortColumn = 'SubscriptionPlanName' AND @SortOrder='ASC')  
                        THEN [SubscriptionPlanName]  
            END ASC,  
            CASE WHEN (@SortColumn = 'SubscriptionPlanName' AND @SortOrder='DESC')  
                        THEN [SubscriptionPlanName]  
            END DESC, 
			CASE WHEN (@SortColumn = 'Price' AND @SortOrder='ASC')  
                        THEN Price  
            END ASC,  
            CASE WHEN (@SortColumn = 'Price' AND @SortOrder='DESC')  
                        THEN Price  
            END DESC, 
			CASE WHEN (@SortColumn = 'Duration' AND @SortOrder='ASC')  
                        THEN [Duration]  
            END ASC,  
            CASE WHEN (@SortColumn = 'Duration' AND @SortOrder='DESC')  
                        THEN [Duration]  
            END DESC,
			   
            CASE WHEN (@SortColumn = 'CreatedOn' AND @SortOrder='ASC')  
                        THEN [CreatedOn]  
            END ASC,  
            CASE WHEN (@SortColumn = 'CreatedOn' AND @SortOrder='DESC')  
                        THEN [CreatedOn]  
            END DESC
           
            OFFSET @PageSize * (@PageNo - 1) ROWS  
            FETCH NEXT @PageSize ROWS ONLY  
    ),  
    CTE_TotalRows AS   
    (  
        select count(bh.Id)  as TotalRecords from BillingHistories BH
       WHERE
			
			(@SearchValue= '' OR  (   
              BH.SubscriptionPlanName LIKE '%' + @SearchValue + '%' OR
			  BH.Price LIKE '%' + @SearchValue + '%' OR
			  BH.[Duration] LIKE '%' + @SearchValue + '%' 
            ))  
   
    )  
    Select  TotalRecords, BH.Id, BH.UserId, BH.SubscriptionPlanName, BH.ModuleIds, BH.ModulesName, BH.[Duration]
	,BH.PlanStartDate, BH.PlanEndDate, BH.CreatedOn, BH.Iscombo , BH.Price from CTE_Results BH
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = BH.ID)  
   
END
