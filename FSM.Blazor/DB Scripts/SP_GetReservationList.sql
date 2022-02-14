/****** Object:  StoredProcedure [dbo].[GetUserList]    Script Date: 10-02-2022 15:19:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[GetReservationList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'StartDateTime',  
    @SortOrder NVARCHAR(20) = 'ASC',
	@CompanyId INT = NULL
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
        Select acs.Id, acs.ScheduleTitle, acs.CreatedOn, 
		acs.StartDateTime, acs.EndDateTime, 
		u.FirstName + '' + u.LastName as Member1,
		acs.ReservationId, a.TailNo, cp.Name as CompanyName
		From AircraftSchedules acs
		LEFT JOIN Users u ON ACS.Member1Id = u.Id
		LEFT JOIN Aircrafts a ON acs.AircraftId = a.Id
		LEFT JOIN  Companies cp on cp.Id = u.CompanyId

        WHERE
			(cp.IsDeleted = 0 OR  cp.IsDeleted IS NULL) AND
			1 = 1 AND 
		      (
				((ISNULL(@CompanyId,0)=0)
				OR (U.CompanyId = @CompanyId))
		      )
			AND 
			acs.IsDeleted = 0 AND
			(@SearchValue= '' OR  (   
              ScheduleTitle LIKE '%' + @SearchValue + '%' OR
			  ReservationId LIKE '%' + @SearchValue + '%' OR
			  TailNo LIKE '%' + @SearchValue + '%' 
            ))  
  
            ORDER BY  
            CASE WHEN (@SortColumn = 'StartDateTime' AND @SortOrder='ASC')  
                        THEN FirstName  
            END ASC,  
            CASE WHEN (@SortColumn = 'StartDateTime' AND @SortOrder='DESC')  
                        THEN FirstName  
            END DESC
            OFFSET @PageSize * (@PageNo - 1) ROWS  
            FETCH NEXT @PageSize ROWS ONLY  
    ),  
    CTE_TotalRows AS   
    (  
        select count(acs.Id) as TotalRecords 
		From AircraftSchedules acs
		LEFT JOIN Users u ON ACS.Member1Id = u.Id
		LEFT JOIN Aircrafts a ON acs.AircraftId = a.Id
		LEFT JOIN  Companies cp on cp.Id = u.CompanyId

        WHERE
			(cp.IsDeleted = 0 OR  cp.IsDeleted IS NULL) AND
			1 = 1 AND 
		      (
				((ISNULL(@CompanyId,0)=0)
				OR (U.CompanyId = @CompanyId))
		      )
			AND 
			acs.IsDeleted = 0 AND
			(@SearchValue= '' OR  (   
              ScheduleTitle LIKE '%' + @SearchValue + '%' OR
			  ReservationId LIKE '%' + @SearchValue + '%' OR
			  TailNo LIKE '%' + @SearchValue + '%' 
            ))  
   
    )  
    Select TotalRecords, acs.Id, acs.ScheduleTitle, acs.CreatedOn, acs.StartDateTime, acs.EndDateTime, 
	u.FirstName + '' + u.LastName as Member1, acs.ReservationId, a.TailNo, 
	cp.Name as CompanyName From AircraftSchedules acs LEFT JOIN Users u ON ACS.Member1Id = u.Id
	LEFT JOIN Aircrafts a ON acs.AircraftId = a.Id 
	LEFT JOIN  Companies cp on cp.Id = u.CompanyId
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = acs.Id)  
   
END



/****** Object:  StoredProcedure [dbo].[GetUserRolePermissionList]    Script Date: 07-12-2021 15:12:50 ******/
SET ANSI_NULLS ON
