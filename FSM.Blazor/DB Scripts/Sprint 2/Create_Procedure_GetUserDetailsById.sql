ALTER PROCEDURE [dbo].[GetUserDetailsById]        
(             
    @Id BIGINT,  
	@IsSuperAdmin BIT,
	@CompanyId INT = NULL
)        
AS BEGIN        
    SET NOCOUNT ON;        
        
   Select top 1 u.*,ur.Id as RoleId, ur.Name AS Role,c.Name AS Country,      
   uv.UserId, cp.Id AS CompanyId, cp.Name AS CompanyName from Users u      
   inner join Countries c on u.CountryId = c.Id      
   left join UsersVSCompanies uv on u.Id = uv.UserId  
   left join InvitedUsers iv on u.Email = iv.Email  
   left join Companies cp on cp.Id = uv.CompanyId       
   inner join UserRoles ur on uv.RoleId = ur.Id           
   where u.id = @Id   AND (uv.CompanyId = @CompanyId or iv.CompanyId = @CompanyId OR @IsSuperAdmin = 1)  
         
END 