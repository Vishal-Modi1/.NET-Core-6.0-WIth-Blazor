ALTER TABLE ModuleDetails ADD IsAdministrationModule
BIT NOT NULL DEFAULT(0)
GO

UPDATE ModuleDetails SET IsAdministrationModule = 1 WHERE Name IN 
('Company','User','InstructorType','UserRolePermission')
GO
