CREATE PROCEDURE [dbo].[GetAircraftStatusLogsList]    
(         
    @SearchValue NVARCHAR(50) = NULL,    
    @PageNo INT = 1,    
    @PageSize INT = 10,    
    @SortColumn NVARCHAR(20) = 'CreatedOn',    
    @SortOrder NVARCHAR(20) = 'DESC',
	@AircraftId bigint
)    
AS BEGIN    
    SET NOCOUNT ON;    
    
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))    
    
    ; WITH CTE_Results AS     
    (    
        SELECT asl.Id, asl.AircraftId, asl.AircraftStatusId,
		ASL.Reason,ASS.Status as AircraftStatus from AircraftStatusLogs asl
		INNER JOIN AircraftStatuses ass	ON ASL.AircraftStatusId = ass.Id
        WHERE  asl.IsDeleted = 0 and   
		asl.AircraftId = @AircraftId and
		 (@SearchValue= '' OR  (     
              ass.Status LIKE '%' + @SearchValue + '%'   
            )  )  
    
            ORDER BY    
            CASE WHEN (@SortColumn = 'CreatedOn' AND @SortOrder='ASC')    
                        THEN asl.CreatedOn    
            END ASC,  
   CASE WHEN (@SortColumn = 'CreatedOn' AND @SortOrder='DESC')    
                        THEN asl.CreatedOn
            END DESC  
  
            OFFSET @PageSize * (@PageNo - 1) ROWS    
            FETCH NEXT @PageSize ROWS ONLY    
    ),    
    CTE_TotalRows AS     
    (    
     SELECT  count(asl.Id) as TotalRecords from AircraftStatusLogs asl
		INNER JOIN AircraftStatuses ass	ON ASL.AircraftStatusId = ass.Id
        WHERE  asl.IsDeleted = 0 and   
		asl.AircraftId = @AircraftId and
		 (@SearchValue= '' OR  (     
              ass.Status LIKE '%' + @SearchValue + '%'   
            )  )  
    )    
    Select  * from CTE_Results   
 , CTE_TotalRows     
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = ID)    
     
END  
  
