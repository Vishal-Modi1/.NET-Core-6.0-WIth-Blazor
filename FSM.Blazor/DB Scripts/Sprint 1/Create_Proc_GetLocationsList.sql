/****** Object:  StoredProcedure [dbo].[GetLocationsList]    Script Date: 10-05-2022 11:10:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetLocationsList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'AirportCode',  
    @SortOrder NVARCHAR(20) = 'ASC'  
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
        SELECT l.*, tz.Offset, tz.Timezone from Locations l
		INNER JOIN Timezones  tz
		ON l.TimezoneId = tz.Id

  
        WHERE  (@SearchValue= '' OR  (   
              AirportCode LIKE '%' + @SearchValue + '%' 
            )) AND l.DeletedBy Is  null  
  
            ORDER BY  
            CASE WHEN (@SortColumn = 'AirportCode' AND @SortOrder='ASC')  
                        THEN [AirportCode]  
            END ASC,
			CASE WHEN (@SortColumn = 'AirportCode' AND @SortOrder='DESC')  
                        THEN [AirportCode]  
            END DESC

            OFFSET @PageSize * (@PageNo - 1) ROWS  
            FETCH NEXT @PageSize ROWS ONLY  
    ),  
    CTE_TotalRows AS   
    (  
        select count(ID) as TotalRecords from Locations 
		
        WHERE  (@SearchValue= '' OR  (   
              AirportCode LIKE '%' + @SearchValue + '%' 
            ))   AND DeletedBy Is  null
    )  
    Select   * from CTE_Results 
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = ID)  
   
END
