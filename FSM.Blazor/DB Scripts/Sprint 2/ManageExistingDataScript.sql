INSERT INTO USERSVSCOMPANIES (UserId, CompanyId, roleid ,IsActive, IsDeleted,CreatedBy,CreatedOn)
select id, CompanyId , RoleId,1,0,1,sysdatetime() from users
where CompanyId is not null
GO

ALTER TABLE Users DROP Column CompanyId
ALTER TABLE Users DROP  cONSTRAINT FK_UserRole_User
ALTER TABLE Users DROP Column RoleId


