INSERT INTO ModuleDetails (Name, ControllerName, ActionName, DisplayName, Icon, OrderNo, IsActive)
Values ('Reservation', 'Reservation', 'Index', 'Reservation', 'book_online',7,1)

SET IDENTITY_INSERT [dbo].ModuleDetails ON 
INSERT INTO ModuleDetails(Id,Name,ControllerName,ActionName,DisplayName,Icon,OrderNo,IsActive)
Values (8,'Document', 'Document', 'Index', 'Document', 'description',8,1)



