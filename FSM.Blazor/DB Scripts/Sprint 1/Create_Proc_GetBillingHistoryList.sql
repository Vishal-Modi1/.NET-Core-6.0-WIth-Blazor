/****** Object:  StoredProcedure [dbo].[GetBillingHistoryList]    Script Date: 06-04-2022 15:33:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetBillingHistoryList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'CreatedOn',  
    @SortOrder NVARCHAR(20) = 'DESC',
	@CompanyId INT = NULL,
	@UserId BIGINT = NULL
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
  print(@sortColumn)
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
      select * from ( select BH.*, u.CompanyId ,temp.ModulesName from (select bh.Id,STRING_AGG(md.displayname,',') 
		as ModulesName from (SELECT BH.id, value  
		FROM BillingHistories bh
		CROSS APPLY STRING_SPLIT(bh.ModuleIds, ',')) as bh
		join ModuleDetails md on BH.value = md.Id group by bh.Id)as temp
		join BillingHistories BH on BH.Id = temp.id
		join Users u on BH.UserId =u.Id
) as tempData
        WHERE
			((ISNULL(@UserId,0)=0)
				OR (tempData.UserId = @UserId))
				AND
			((ISNULL(@CompanyId,0)=0)
				OR (tempData.CompanyId = @CompanyId))
				AND
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
        select count(bh.Id)  as TotalRecords from CTE_Results BH
       WHERE
			((ISNULL(@UserId,0)=0)
				OR (bh.UserId = @UserId))
				AND
			((ISNULL(@CompanyId,0)=0)
				OR (bh.CompanyId = @CompanyId))
				AND
			(@SearchValue= '' OR  (   
              BH.SubscriptionPlanName LIKE '%' + @SearchValue + '%' OR
			  BH.Price LIKE '%' + @SearchValue + '%' OR
			  BH.[Duration] LIKE '%' + @SearchValue + '%' 
            ))  
   
    )  
    Select  TotalRecords, BH.Id, BH.UserId, BH.SubscriptionPlanName, BH.ModuleIds, BH.ModulesName, BH.[Duration]
	,BH.PlanStartDate,BH.IsActive, BH.PlanEndDate, BH.CreatedOn, BH.Iscombo , BH.Price from CTE_Results BH
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = BH.ID)  
   
END
