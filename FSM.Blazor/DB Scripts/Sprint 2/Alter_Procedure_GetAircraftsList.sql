    
ALTER PROCEDURE [dbo].[GetAircraftsList]      
(           
    @SearchValue NVARCHAR(50) = NULL,      
    @PageNo INT = 1,      
    @PageSize INT = 10,      
    @SortColumn NVARCHAR(20) = 'TailNo',      
    @SortOrder NVARCHAR(20) = 'ASC',    
 @CompanyId INT = NULL,    
 @IsActvie BIT = 1    
)      
AS BEGIN      
    SET NOCOUNT ON;      
      
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))      
      
    ; WITH CTE_Results AS       
    (      
        SELECT A.*, CP.Name CompanyName, AM.Name MakeName,AMD.Name ModelName,  
  AC.Name Category, ACS.CanFly, ACS.Indicator, ACS.Status AS AircraftStatus from Aircrafts A    
  LEFT JOIN  Companies CP on CP.Id = A.CompanyId    
  LEFT JOIN  AircraftMakes AM on AM.Id = A.AircraftMakeId    
  LEFT JOIN  AircraftModels AMD on AMD.Id = A.AircraftModelId    
  LEFT JOIN  AircraftCategories AC on AC.Id = A.AircraftCategoryId    
  LEFT JOIN  AircraftStatuses ACS on ACS.Id = A.AircraftStatusId   
    
        WHERE    
    CP.IsDeleted = 0 AND    
    1 = 1 AND     
         (    
     ((ISNULL(@CompanyId,0)=0)    
     OR (A.CompanyId = @CompanyId))    
         
         )    
    AND     
    A.IsDeleted = 0 AND    
    (@SearchValue= '' OR  (       
               TailNo LIKE '%' + @SearchValue + '%' OR    
      AM.Name LIKE '%' + @SearchValue + '%' OR    
      AMD.Name LIKE '%' + @SearchValue + '%' OR    
      AC.Name LIKE '%' + @SearchValue + '%' OR    
      CP.Name LIKE '%' + @SearchValue + '%'    
             ))      
    
   ORDER BY      
            CASE WHEN (@SortColumn = 'TailNo' AND @SortOrder='ASC')      
                        THEN TailNo    
            END ASC,    
   CASE WHEN (@SortColumn = 'TailNo' AND @SortOrder='DESC')      
                        THEN TailNo      
            END DESC    
    
   OFFSET @PageSize * (@PageNo - 1) ROWS      
            FETCH NEXT @PageSize ROWS ONLY     
       
    ),      
    CTE_TotalRows AS       
    (      
        select count(A.ID) as TotalRecords from Aircrafts A    
  LEFT JOIN  Companies CP on CP.Id = A.CompanyId    
  LEFT JOIN  AircraftMakes AM on AM.Id = A.AircraftMakeId    
  LEFT JOIN  AircraftModels AMD on AMD.Id = A.AircraftModelId    
  LEFT JOIN  AircraftCategories AC on AC.Id = A.AircraftCategoryId    
  LEFT JOIN  AircraftStatuses ACS on ACS.Id = A.AircraftStatusId  
        WHERE    
    CP.IsDeleted = 0 AND    
    1 = 1 AND     
         (    
     ((ISNULL(@CompanyId,0)=0)    
     OR (A.CompanyId = @CompanyId))    
         
         )    
    AND     
    A.IsDeleted = 0 AND    
    (@SearchValue= '' OR  (       
               TailNo LIKE '%' + @SearchValue + '%' OR    
      AM.Name LIKE '%' + @SearchValue + '%' OR    
      AMD.Name LIKE '%' + @SearchValue + '%' OR    
      AC.Name LIKE '%' + @SearchValue + '%' OR    
      CP.Name LIKE '%' + @SearchValue + '%'    
             ))      
        
     )      
    SELECT TotalRecords,A.*, CP.Name CompanyName, AM.Name MakeName,  
 AMD.Name ModelName, AC.Name Category, ACS.CanFly,   
 ACS.Indicator, ACS.Status AS AircraftStatus from Aircrafts A    
 LEFT JOIN  Companies CP on CP.Id = A.CompanyId    
 LEFT JOIN  AircraftMakes AM on AM.Id = A.AircraftMakeId    
 LEFT JOIN  AircraftModels AMD on AMD.Id = A.AircraftModelId    
 LEFT JOIN  AircraftCategories AC on AC.Id = A.AircraftCategoryId    
 LEFT JOIN  AircraftStatuses ACS on ACS.Id = A.AircraftStatusId  
 , CTE_TotalRows       
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = A.ID)      
       
END