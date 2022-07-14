
/****** Object:  StoredProcedure [dbo].[GetAircraftModelsList]    Script Date: 08-07-2022 10:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAircraftModelsList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'Name',  
    @SortOrder NVARCHAR(20) = 'ASC'  
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
        SELECT * from AircraftModels
  
        WHERE  (@SearchValue= '' OR  (   
              [Name] LIKE '%' + @SearchValue + '%' 
            ))
  
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
        select count(ID) as TotalRecords from AircraftModels 
		
        WHERE  (@SearchValue= '' OR  (   
              [Name] LIKE '%' + @SearchValue + '%' 
            ))
    )  

    Select   * from CTE_Results 
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = ID)  
   
END
