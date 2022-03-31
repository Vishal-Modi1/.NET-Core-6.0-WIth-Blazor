INSERT INTO ModuleDetails (Name, ControllerName, ActionName, DisplayName, Icon, OrderNo, IsActive)
Values ('Reservation', 'Reservation', 'Index', 'Reservation', 'book_online',7,1)

--SET IDENTITY_INSERT [dbo].ModuleDetails OFF 
INSERT INTO ModuleDetails(Id,Name,ControllerName,ActionName,DisplayName,Icon,OrderNo,IsActive)
Values (8,'Document', 'Document', 'Index', 'Document', 'description',8,1)

INSERT INTO ModuleDetails(Id,Name,ControllerName,ActionName,DisplayName,Icon,OrderNo,IsActive)
Values (9,'SubscriptionPlan', 'SubscriptionPlan', 'Index', 'Subscription Plan', 'monetization_on',9,1)

--SET IDENTITY_INSERT [dbo].ModuleDetails ON 
INSERT INTO ModuleDetails(Id,Name,ControllerName,ActionName,DisplayName,Icon,OrderNo,IsActive)
Values (10,'BillingHistory', 'BillingHistory', 'Index', 'Billing History', 'receipt',10,1)


