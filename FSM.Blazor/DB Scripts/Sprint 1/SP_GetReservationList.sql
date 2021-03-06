/****** Object:  StoredProcedure [dbo].[GetReservationList]    Script Date: 05-04-2022 09:11:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetReservationList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'StartDateTime',  
    @SortOrder NVARCHAR(20) = 'ASC',
	@CompanyId INT = NULL,
	@StartDate DATETIME = NULL,
	@EndDate DATETIME = NULL,
	@UserId BIGINT = NULL,
	@AircraftId BIGINT = NULL
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
        Select acs.Id, acs.ScheduleTitle, acs.CreatedOn, 
		acs.StartDateTime, acs.EndDateTime, acd.FlightStatus,
		u.FirstName + '' + u.LastName as Member1, acd.IsCheckOut,
		acs.ReservationId, a.TailNo, cp.Name as CompanyName, ah.TotalTime as AirFrameTime
		From AircraftSchedules acs
		LEFT JOIN AircraftScheduleDetails acd ON acs.Id = acd.AircraftScheduleId
		LEFT JOIN Users u ON ACS.Member1Id = u.Id
		LEFT JOIN Aircrafts a ON acs.AircraftId = a.Id
		LEFT JOIN  Companies cp on a.CompanyId = cp.Id   
		LEFT JOIN AircraftScheduleHobbsTimes ah
		ON acs.Id = ah.AircraftScheduleId
		LEFT JOIN AircraftEquipmentTimes at
		ON ah.AircraftEquipmentTimeId = at.Id
        WHERE
			(cp.IsDeleted = 0 OR  cp.IsDeleted IS NULL) AND
			(at.IsDeleted = 0 or at.IsDeleted Is NULL) and
			(at.EquipmentName = 'Air Frame' or at.EquipmentName IS NULL)
			AND
			1 = 1 AND 
		      (
				((ISNULL(@UserId,0)=0)
				OR (acs.CreatedBy = @UserId))
				AND
				((ISNULL(@AircraftId,0)=0)
				OR (acs.AircraftId = @AircraftId))
				AND
				((ISNULL(@CompanyId,0)=0)
				OR (cp.Id = @CompanyId))
				AND
				((ISNULL(@StartDate,0)= '1900-01-01 00:00:00.000')
				OR (acs.StartDateTime   >= @StartDate ))
				AND
				((ISNULL(@EndDate,0)='1900-01-01 00:00:00.000')
				OR (acs.EndDateTime  <= @EndDate ))
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
                        THEN acs.StartDateTime  
            END ASC,  
            CASE WHEN (@SortColumn = 'StartDateTime' AND @SortOrder='DESC')  
                        THEN acs.StartDateTime  
            END DESC,
			 CASE WHEN (@SortColumn = 'EndDateTime' AND @SortOrder='ASC')  
                        THEN acs.EndDateTime  
            END ASC,  
            CASE WHEN (@SortColumn = 'EndDateTime' AND @SortOrder='DESC')  
                        THEN acs.EndDateTime  
            END DESC,
			CASE WHEN (@SortColumn = 'ScheduleTitle' AND @SortOrder='ASC')  
                        THEN acs.ScheduleTitle  
            END ASC,  
            CASE WHEN (@SortColumn = 'ScheduleTitle' AND @SortOrder='DESC')  
                        THEN acs.ScheduleTitle   
            END DESC,
			CASE WHEN (@SortColumn = 'TailNo' AND @SortOrder='ASC')  
                        THEN a.TailNo  
            END ASC,  
            CASE WHEN (@SortColumn = 'TailNo' AND @SortOrder='DESC')  
                        THEN a.TailNo   
            END DESC,
			CASE WHEN (@SortColumn = 'FlightStatus' AND @SortOrder='ASC')  
                        THEN acd.FlightStatus  
            END ASC,  
            CASE WHEN (@SortColumn = 'FlightStatus' AND @SortOrder='DESC')  
                        THEN acd.FlightStatus 
            END DESC
            OFFSET @PageSize * (@PageNo - 1) ROWS  
            FETCH NEXT @PageSize ROWS ONLY  
    ),  
    CTE_TotalRows AS   
    (  
        select count(acs.Id) as TotalRecords 
		From AircraftSchedules acs
		LEFT JOIN AircraftScheduleDetails acd ON acs.Id = acd.AircraftScheduleId
		LEFT JOIN Users u ON ACS.Member1Id = u.Id
		LEFT JOIN Aircrafts a ON acs.AircraftId = a.Id
		LEFT JOIN  Companies cp on a.CompanyId = cp.Id  
		LEFT JOIN AircraftScheduleHobbsTimes ah
		ON acs.Id = ah.AircraftScheduleId
		LEFT JOIN AircraftEquipmentTimes at
		ON ah.AircraftEquipmentTimeId = at.Id

        WHERE
			(cp.IsDeleted = 0 OR  cp.IsDeleted IS NULL) AND
			(at.IsDeleted = 0 or at.IsDeleted Is NULL) and
			(at.EquipmentName = 'Air Frame' or at.EquipmentName IS NULL)
			AND
			1 = 1 AND 
		      (
			   ((ISNULL(@UserId,0)=0)
				OR (acs.CreatedBy = @UserId))
				AND
				((ISNULL(@AircraftId,0)=0)
				OR (acs.AircraftId = @AircraftId))
				AND
				((ISNULL(@CompanyId,0)=0)
				OR (cp.Id = @CompanyId))
				AND
				((ISNULL(@StartDate,0)= '1900-01-01 00:00:00.000')
				OR (acs.StartDateTime   >= @StartDate ))
				AND
				((ISNULL(@EndDate,0)='1900-01-01 00:00:00.000')
				OR (acs.EndDateTime <= @EndDate ))
		      )
			AND 
			acs.IsDeleted = 0 AND
			(@SearchValue= '' OR  (   
              ScheduleTitle LIKE '%' + @SearchValue + '%' OR
			  ReservationId LIKE '%' + @SearchValue + '%' OR
			  TailNo LIKE '%' + @SearchValue + '%' 
            ))  
    )
	
    SELECT TotalRecords, CTE_Results.* From CTE_Results,CTE_TotalRows

END
