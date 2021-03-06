/****** Object:  StoredProcedure [dbo].[GetCompanyList]    Script Date: 26-04-2022 16:22:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetCompanyList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'Name',  
    @SortOrder NVARCHAR(20) = 'ASC',
	@CompanyId INT = NULL
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
        SELECT * from Companies 
		
        WHERE  IsDeleted = 0 and 
		((ISNULL(@CompanyId,0)=0)
				OR (Id = @CompanyId))
				and (@SearchValue= '' OR  (   
              Name LIKE '%' + @SearchValue + '%' 
            )  )
  
            ORDER BY  
            CASE WHEN (@SortColumn = 'Name' AND @SortOrder='ASC')  
                        THEN [Name]  
            END ASC,
			CASE WHEN (@SortColumn = 'Name' AND @SortOrder='DESC')  
                        THEN [Name]  
            END DESC

            OFFSET @PageSize * (@PageNo - 1) ROWS  
            FETCH NEXT @PageSize ROWS ONLY  
    ),  
    CTE_TotalRows AS   
    (  
        select count(ID) as TotalRecords from CTE_Results 
    )  
    Select  * from CTE_Results 
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = ID)  
   
END

/****** Object:  StoredProcedure [dbo].[GetInstructorTypeList]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
