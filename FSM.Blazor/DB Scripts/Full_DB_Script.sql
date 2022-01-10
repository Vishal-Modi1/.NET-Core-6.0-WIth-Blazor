/****** Object:  User [sa1]    Script Date: 03-12-2021 16:50:57 ******/

/****** Object:  Table [dbo].[AircraftCategories]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AircraftCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AircraftClasses]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AircraftClasses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AirCraftEquipments]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AirCraftEquipments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StatusId] [int] NOT NULL,
	[AirCraftId] [int] NOT NULL,
	[ClassificationId] [int] NOT NULL,
	[Item] [nvarchar](500) NOT NULL,
	[Model] [nvarchar](500) NULL,
	[Make] [nvarchar](500) NULL,
	[Manufacturer] [nvarchar](500) NULL,
	[PartNumber] [nvarchar](500) NULL,
	[SerialNumber] [nvarchar](500) NULL,
	[ManufacturerDate] [datetime2](7) NULL,
	[LogEntryDate] [datetime2](7) NULL,
	[AircraftTTInstall] [int] NULL,
	[PartTTInstall] [int] NULL,
	[Notes] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedOn] [datetime2](7) NULL,
	[CreatedBy] [int] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedBy] [int] NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
 CONSTRAINT [PK_AirCraftEquipments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AircraftEquipmentTimes]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AircraftEquipmentTimes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EquipmentName] [varchar](200) NOT NULL,
	[Hours] [int] NOT NULL,
	[AircraftId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
 CONSTRAINT [PK_AircraftEquipementTimes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AircraftMakes]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AircraftMakes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AircraftModels]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AircraftModels](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Aircrafts]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Aircrafts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ImageName] [varchar](200) NULL,
	[TailNo] [varchar](30) NOT NULL,
	[Year] [varchar](4) NULL,
	[AircraftMakeId] [int] NOT NULL,
	[AircraftModelId] [int] NOT NULL,
	[AircraftCategoryId] [int] NOT NULL,
	[AircraftClassId] [int] NULL,
	[FlightSimulatorClassId] [int] NULL,
	[NoofEngines] [int] NULL,
	[NoofPropellers] [int] NULL,
	[IsEngineshavePropellers] [bit] NOT NULL,
	[IsEnginesareTurbines] [bit] NOT NULL,
	[TrackOilandFuel] [bit] NOT NULL,
	[IsIdentifyMeterMismatch] [bit] NOT NULL,
	[CompanyId] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
 CONSTRAINT [PK__Aircraft__3214EC07C1A8802D] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Address] [varchar](300) NOT NULL,
	[ContactNo] [varchar](20) NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[TwoCharCountryCode] [char](2) NULL,
	[ThreeCharCountryCode] [char](3) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailTokens]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailTokens](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Token] [varchar](500) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ExpireOn] [datetime] NULL,
	[UserId] [int] NULL,
	[EmailType] [varchar](25) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EquipmentClassifications]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquipmentClassifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Classifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EquipmentStatuses]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquipmentStatuses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Statuses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FlightSimulatorClasses]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlightSimulatorClasses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InstructorTypes]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InstructorTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModuleDetails]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModuleDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[ControllerName] [varchar](100) NULL,
	[ActionName] [varchar](100) NULL,
	[DisplayName] [varchar](100) NULL,
	[Icon] [varchar](50) NULL,
	[OrderNo] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PermissionType] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRolePermissions]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRolePermissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NULL,
	[PermissionId] [int] NULL,
	[ModuleId] [int] NULL,
	[CompanyId] [int] NULL,
	[IsAllowed] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_RoleId_PermissionId_ModuleId] UNIQUE NONCLUSTERED 
(
	[RoleId] ASC,
	[PermissionId] ASC,
	[ModuleId] ASC,
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Priority] int not null,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](150) NOT NULL,
	[LastName] [varchar](150) NOT NULL,
	[Email] [varchar](150) NOT NULL,
	[IsSendEmailInvite] [bit] NOT NULL,
	[Phone] [varchar](20) NULL,
	[RoleId] [int] NOT NULL,
	[IsInstructor] [bit] NULL,
	[InstructorTypeId] [int] NULL,
	[CompanyId] [int] NULL,
	[ExternalId] [varchar](50) NULL,
	[DateofBirth] [datetime] NULL,
	[Gender] [varchar](6) NULL,
	[CountryId] [int] NULL,
	[Password] [varchar](100) NULL,
	[IsSendTextMessage] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [bit] NULL,
 CONSTRAINT [PK__Users__3214EC0734F10733] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Users__A9D1053466EBF258] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AirCraftEquipments] ADD  CONSTRAINT [DF_AirCraftEquipments_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Aircrafts] ADD  CONSTRAINT [DF__Aircrafts__IsEng__47DBAE45]  DEFAULT ((0)) FOR [IsEngineshavePropellers]
GO
ALTER TABLE [dbo].[Aircrafts] ADD  CONSTRAINT [DF__Aircrafts__IsEng__48CFD27E]  DEFAULT ((0)) FOR [IsEnginesareTurbines]
GO
ALTER TABLE [dbo].[Aircrafts] ADD  CONSTRAINT [DF__Aircrafts__Track__49C3F6B7]  DEFAULT ((0)) FOR [TrackOilandFuel]
GO
ALTER TABLE [dbo].[Aircrafts] ADD  CONSTRAINT [DF__Aircrafts__IsIde__4AB81AF0]  DEFAULT ((0)) FOR [IsIdentifyMeterMismatch]
GO
ALTER TABLE [dbo].[Aircrafts] ADD  CONSTRAINT [DF__Aircrafts__IsAct__4BAC3F29]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Aircrafts] ADD  CONSTRAINT [DF__Aircrafts__IsDel__4CA06362]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Companies] ADD  CONSTRAINT [DF_Companies_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Companies] ADD  CONSTRAINT [DF_Companies_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ModuleDetails] ADD  CONSTRAINT [DF_ModuleDetails_OrderNo]  DEFAULT ((0)) FOR [OrderNo]
GO
ALTER TABLE [dbo].[ModuleDetails] ADD  CONSTRAINT [DF_ModuleDetails_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UserRolePermissions] ADD  DEFAULT ((0)) FOR [IsAllowed]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [df_IsSendEmailInvite]  DEFAULT ((0)) FOR [IsSendEmailInvite]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [df_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [df_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__Users__IsSendTex__52593CB8]  DEFAULT ((0)) FOR [DeletedOn]
GO
ALTER TABLE [dbo].[AirCraftEquipments]  WITH CHECK ADD  CONSTRAINT [FK_AirCraftEquipments_Classifications] FOREIGN KEY([ClassificationId])
REFERENCES [dbo].[EquipmentClassifications] ([Id])
GO
ALTER TABLE [dbo].[AirCraftEquipments] CHECK CONSTRAINT [FK_AirCraftEquipments_Classifications]
GO
ALTER TABLE [dbo].[AirCraftEquipments]  WITH CHECK ADD  CONSTRAINT [FK_AirCraftEquipments_Statuses] FOREIGN KEY([StatusId])
REFERENCES [dbo].[EquipmentStatuses] ([Id])
GO
ALTER TABLE [dbo].[AirCraftEquipments] CHECK CONSTRAINT [FK_AirCraftEquipments_Statuses]
GO
ALTER TABLE [dbo].[AircraftEquipmentTimes]  WITH CHECK ADD  CONSTRAINT [FK_AircraftEquipmentTimes_AircraftId] FOREIGN KEY([AircraftId])
REFERENCES [dbo].[Aircrafts] ([Id])
GO
ALTER TABLE [dbo].[AircraftEquipmentTimes] CHECK CONSTRAINT [FK_AircraftEquipmentTimes_AircraftId]
GO
ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_AircraftCategory_Aircraft] FOREIGN KEY([AircraftCategoryId])
REFERENCES [dbo].[AircraftCategories] ([Id])
GO
ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_AircraftCategory_Aircraft]
GO
ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_AircraftClass_Aircraft] FOREIGN KEY([AircraftClassId])
REFERENCES [dbo].[AircraftClasses] ([Id])
GO
ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_AircraftClass_Aircraft]
GO
ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_AircraftMake_Aircraft] FOREIGN KEY([AircraftMakeId])
REFERENCES [dbo].[AircraftMakes] ([Id])
GO
ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_AircraftMake_Aircraft]
GO
ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_AircraftModel_Aircraft] FOREIGN KEY([AircraftModelId])
REFERENCES [dbo].[AircraftModels] ([Id])
GO
ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_AircraftModel_Aircraft]
GO
ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_Aircrafts_Companies] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_Aircrafts_Companies]
GO
ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_FlightSimulatorClass_Aircraft] FOREIGN KEY([FlightSimulatorClassId])
REFERENCES [dbo].[FlightSimulatorClasses] ([Id])
GO
ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_FlightSimulatorClass_Aircraft]
GO
ALTER TABLE [dbo].[EmailTokens]  WITH CHECK ADD  CONSTRAINT [FK_User_EmailTokens] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[EmailTokens] CHECK CONSTRAINT [FK_User_EmailTokens]
GO
ALTER TABLE [dbo].[UserRolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_Companies_UserRolePermission] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[UserRolePermissions] CHECK CONSTRAINT [FK_Companies_UserRolePermission]
GO
ALTER TABLE [dbo].[UserRolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_ModuleDetail_UserRolePermission] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[ModuleDetails] ([Id])
GO
ALTER TABLE [dbo].[UserRolePermissions] CHECK CONSTRAINT [FK_ModuleDetail_UserRolePermission]
GO
ALTER TABLE [dbo].[UserRolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_Permission_UserRolePermission] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permissions] ([Id])
GO
ALTER TABLE [dbo].[UserRolePermissions] CHECK CONSTRAINT [FK_Permission_UserRolePermission]
GO
ALTER TABLE [dbo].[UserRolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_UserRolePermission] FOREIGN KEY([RoleId])
REFERENCES [dbo].[UserRoles] ([Id])
GO
ALTER TABLE [dbo].[UserRolePermissions] CHECK CONSTRAINT [FK_UserRole_UserRolePermission]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Country_User] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Country_User]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_InstructorType_User] FOREIGN KEY([InstructorTypeId])
REFERENCES [dbo].[InstructorTypes] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_InstructorType_User]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_User] FOREIGN KEY([RoleId])
REFERENCES [dbo].[UserRoles] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_UserRole_User]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Companies] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Companies]
GO
/****** Object:  StoredProcedure [dbo].[GetAircraftEquipmentList]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [dbo].[GetCompanyList]    Script Date: 04-01-2022 12:36:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCompanyList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'Name',  
    @SortOrder NVARCHAR(20) = 'ASC'  
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
        SELECT * from Companies 
		
        WHERE  IsDeleted = 0 and ( @SearchValue= '' OR  (   
              Name LIKE '%' + @SearchValue + '%' 
            )  )
  
            ORDER BY  
            CASE WHEN (@SortColumn = 'Name' AND @SortOrder='ASC')  
                        THEN [Name]  
            END ASC,
			CASE WHEN (@SortColumn = 'Name' AND @SortOrder='DESC')  
                        THEN [Name]  
            END DESC

            OFFSET @PageSize * (@PageNo - 1) ROWS  
            FETCH NEXT @PageSize ROWS ONLY  
    ),  
    CTE_TotalRows AS   
    (  
        select count(ID) as TotalRecords from Companies 
		
        WHERE IsDeleted = 0 and ( @SearchValue= '' OR  (   
              Name LIKE '%' + @SearchValue + '%' 
            )  )
    )  
    Select  * from CTE_Results 
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = ID)  
   
END

/****** Object:  StoredProcedure [dbo].[GetInstructorTypeList]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetInstructorTypeList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'Name',  
    @SortOrder NVARCHAR(20) = 'ASC'  
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
        SELECT * from InstructorTypes 
  
        WHERE  @SearchValue= '' OR  (   
              Name LIKE '%' + @SearchValue + '%' 
            )  
  
            ORDER BY  
            CASE WHEN (@SortColumn = 'Name' AND @SortOrder='ASC')  
                        THEN [Name]  
            END ASC,
			CASE WHEN (@SortColumn = 'Name' AND @SortOrder='DESC')  
                        THEN [Name]  
            END DESC

            OFFSET @PageSize * (@PageNo - 1) ROWS  
            FETCH NEXT @PageSize ROWS ONLY  
    ),  
    CTE_TotalRows AS   
    (  
        select count(ID) as TotalRecords from InstructorTypes 
		
        WHERE  (@SearchValue= '' OR  (   
              Name LIKE '%' + @SearchValue + '%' 
            ))   
    )  
    Select TotalRecords, Id, Name from CTE_Results 
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = ID)  
   
END
GO

/****** Object:  StoredProcedure [dbo].[GetUserList]    Script Date: 07-12-2021 16:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'FirstName',  
    @SortOrder NVARCHAR(20) = 'ASC',
	@CompanyId INT = NULL,
	@RoleId INT = NULL
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
        SELECT U.*,UR.Name, CP.Name CompanyName from Users U
		LEFT JOIN  UserRoles UR on UR.Id= U.RoleId
		LEFT JOIN  Companies CP on CP.Id = U.CompanyId

        WHERE
			(CP.IsDeleted = 0 OR  CP.IsDeleted IS NULL) AND
			1 = 1 AND 
		      (
				((ISNULL(@CompanyId,0)=0)
				OR (U.CompanyId = @CompanyId))
				AND ((ISNULL(@RoleId,0)=0) 
				OR (UR.Id = @RoleId))
		      )
			AND 
			U.IsDeleted = 0 AND
			(@SearchValue= '' OR  (   
              FirstName LIKE '%' + @SearchValue + '%' OR
			  LastName LIKE '%' + @SearchValue + '%' OR
			  Email LIKE '%' + @SearchValue + '%' OR
			  UR.Name LIKE '%' + @SearchValue + '%' OR
			  CP.Name LIKE '%' + @SearchValue + '%'
            ))  
  
            ORDER BY  
            CASE WHEN (@SortColumn = 'FirstName' AND @SortOrder='ASC')  
                        THEN FirstName  
            END ASC,  
            CASE WHEN (@SortColumn = 'FirstName' AND @SortOrder='DESC')  
                        THEN FirstName  
            END DESC, 
			CASE WHEN (@SortColumn = 'LastName' AND @SortOrder='ASC')  
                        THEN LastName  
            END ASC,  
            CASE WHEN (@SortColumn = 'LastName' AND @SortOrder='DESC')  
                        THEN LastName  
            END DESC, 
			CASE WHEN (@SortColumn = 'Email' AND @SortOrder='ASC')  
                        THEN Email  
            END ASC,  
            CASE WHEN (@SortColumn = 'Email' AND @SortOrder='DESC')  
                        THEN Email  
            END DESC, 
            CASE WHEN (@SortColumn = 'UserRole' AND @SortOrder='ASC')  
                        THEN UR.Name  
            END ASC,  
            CASE WHEN (@SortColumn = 'UserRole' AND @SortOrder='DESC')  
                        THEN UR.Name  
            END DESC,
			CASE WHEN (@SortColumn = 'IsInstructor' AND @SortOrder='ASC')  
                        THEN U.IsInstructor 
            END ASC,  
            CASE WHEN (@SortColumn = 'IsInstructor' AND @SortOrder='DESC')  
                        THEN U.IsInstructor  
            END DESC, 
			CASE WHEN (@SortColumn = 'IsActive' AND @SortOrder='ASC')  
                        THEN U.IsActive  
            END ASC,  
            CASE WHEN (@SortColumn = 'IsActive' AND @SortOrder='DESC')  
                        THEN U.IsActive 
            END DESC,
			CASE WHEN (@SortColumn = 'CompanyName' AND @SortOrder='ASC')  
                        THEN CP.Name
            END ASC,  
            CASE WHEN (@SortColumn = 'CompanyName' AND @SortOrder='DESC')  
                        THEN CP.Name 
            END DESC
            OFFSET @PageSize * (@PageNo - 1) ROWS  
            FETCH NEXT @PageSize ROWS ONLY  
    ),  
    CTE_TotalRows AS   
    (  
        select count(U.ID) as TotalRecords from Users U
		LEFT JOIN  UserRoles UR on UR.Id =U.RoleId
		LEFT JOIN  Companies CP on CP.Id = U.CompanyId
       WHERE
		(CP.IsDeleted = 0 OR  CP.IsDeleted IS NULL) AND
			1 = 1 AND 
		      (
				((ISNULL(@CompanyId,0)=0)
				OR (U.CompanyId = @CompanyId))
				AND ((ISNULL(@RoleId,0)=0) 
				OR (UR.Id = @RoleId))
		      )
			AND 
			U.IsDeleted = 0 AND
			(@SearchValue= '' OR  (   
              FirstName LIKE '%' + @SearchValue + '%' OR
			  LastName LIKE '%' + @SearchValue + '%' OR
			  Email LIKE '%' + @SearchValue + '%' OR
			  UR.Name LIKE '%' + @SearchValue + '%' OR
			  CP.Name LIKE '%' + @SearchValue + '%'
            ))  
   
    )  
    Select TotalRecords, U.Id, U.FirstName, U.LastName,U.Email,ISNULL(U.IsInstructor, 0 ) 
	AS IsInstructor,ISNULL(U.IsActive, 0 ) AS IsActive,Ur.Name as UserRole, CP.Name AS CompanyName from Users U
	LEFT JOIN  UserRoles UR on UR.Id=U.RoleId
	LEFT JOIN  Companies CP on CP.Id = U.CompanyId
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = U.ID)  
   
END



/****** Object:  StoredProcedure [dbo].[GetUserRolePermissionList]    Script Date: 07-12-2021 15:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserRolePermissionList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'UserRole',  
    @SortOrder NVARCHAR(20) = 'ASC'  ,
	@ModuleId INT = NULL,
	@RoleId INT = NULL,
	@CompanyId INT = NULL
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
        SELECT URP.*,UR.Name,PR.PermissionType,MD.Name AS ModuleName,
		MD.OrderNo from UserRolePermissions URP
		LEFT JOIN  UserRoles UR on UR.Id = URP.RoleId
		LEFT JOIN  Permissions PR on PR.Id = URP.PermissionId 
		LEFT JOIN  ModuleDetails MD on MD.Id = URP.ModuleId
		LEFT JOIN  Companies CP on CP.Id = URP.CompanyId
		
        WHERE 
			(CP.IsDeleted = 0 OR CP.IsDeleted IS NULL) AND
			1 = 1 AND 
		      (
				((ISNULL(@ModuleId,0)=0)
				OR (URP.ModuleId = @ModuleId))
				AND ((ISNULL(@RoleId,0)=0) 
				OR (URP.RoleId = @RoleId))
				AND ((ISNULL(@CompanyId,0)=0)
				OR (URP.CompanyId = @CompanyId))
		      )
			  
			  AND
			  (@SearchValue= '' OR  (   
              UR.Name LIKE '%' + @SearchValue + '%' OR
			  MD.Name LIKE '%' + @SearchValue + '%' OR
			  PR.PermissionType LIKE '%' + @SearchValue + '%' OR
			  URP.IsAllowed  LIKE '%' + @SearchValue + '%'
            ) )
            ORDER BY  
            CASE WHEN (@SortColumn = 'RoleName' AND @SortOrder='ASC')  
                        THEN UR.Name  
            END ASC,  
            CASE WHEN (@SortColumn = 'RoleName' AND @SortOrder='DESC')  
                        THEN UR.Name  
            END DESC, 
			CASE WHEN (@SortColumn = 'DisplayName' AND @SortOrder='ASC')  
                        THEN MD.Name  
            END ASC,  
            CASE WHEN (@SortColumn = 'DisplayName' AND @SortOrder='DESC')  
                        THEN MD.Name  
            END DESC, 
			CASE WHEN (@SortColumn = 'PermissionType' AND @SortOrder='ASC')  
                        THEN PR.PermissionType  
            END ASC,  
            CASE WHEN (@SortColumn = 'PermissionType' AND @SortOrder='DESC')  
                        THEN PR.PermissionType  
            END DESC, 
			CASE WHEN (@SortColumn = 'IsAllowed' AND @SortOrder='ASC')  
                        THEN URP.IsAllowed
            END ASC,  
            CASE WHEN (@SortColumn = 'IsAllowed' AND @SortOrder='DESC')  
                         THEN URP.IsAllowed
            END DESC 
            OFFSET @PageSize * (@PageNo - 1) ROWS  
            FETCH NEXT @PageSize ROWS ONLY  
    ),  
    CTE_TotalRows AS   
    (  
        select count(URP.ID) as TotalRecords  from UserRolePermissions URP
		LEFT JOIN  UserRoles UR on UR.Id = URP.RoleId
		LEFT JOIN  Permissions PR on PR.Id = URP.PermissionId 
		LEFT JOIN  ModuleDetails MD on MD.Id = URp.ModuleId
		LEFT JOIN  Companies CP on CP.Id = URP.CompanyId

        WHERE 
			(CP.IsDeleted = 0 OR CP.IsDeleted IS NULL) AND
			1 = 1 AND 
		      (
				((ISNULL(@ModuleId,0)=0)
				OR (URP.ModuleId = @ModuleId))
				AND ((ISNULL(@RoleId,0)=0) 
				OR (URP.RoleId = @RoleId))
				AND ((ISNULL(@CompanyId,0)=0)
				OR (URP.CompanyId = @CompanyId))
		      ) AND
			  (@SearchValue= '' OR  (   
              UR.Name LIKE '%' + @SearchValue + '%' OR
			  MD.Name LIKE '%' + @SearchValue + '%' OR
			  PR.PermissionType LIKE '%' + @SearchValue + '%' OR
			  URP.IsAllowed  LIKE '%' + @SearchValue + '%'
            ))   
    )  

    Select TotalRecords, URP.*,UR.Name  AS RoleName,PR.PermissionType,
		   MD.Name ModuleName,MD.ControllerName, MD.ActionName, MD.Icon, MD.DisplayName,
		   MD.OrderNo from UserRolePermissions URP
		LEFT JOIN  UserRoles UR on UR.Id = URP.RoleId
		LEFT JOIN  Permissions PR on PR.Id = URP.PermissionId 
		LEFT JOIN  ModuleDetails MD on MD.Id = URp.ModuleId
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = URP.ID)  
   
END



ALTER DATABASE [FlyManagerDevDB] SET  READ_WRITE 
GO

/****** Object:  Trigger [dbo].[Trg_Company_InsertUserRolePermission]    Script Date: 03-12-2021 16:40:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Trigger [dbo].[Trg_Company_InsertUserRolePermission]
On [dbo].[Companies]
AFTER INSERT
AS
SET NOCOUNT ON;
BEGIN

		DECLARE @companyId INT = 0
		SELECT @companyId = i.id FROM inserted i
	
		DECLARE @moduleDetailsCursor CURSOR, @permissionsCursor CURSOR, @rolesCursor CURSOR
		DECLARE @moduleId INT, @permissionId INT, @roleId INT
		
		SET @moduleDetailsCursor = CURSOR FOR
	    SELECT Id FROM dbo.ModuleDetails
		WHERE IsActive = 1
	    
		OPEN @moduleDetailsCursor 
	    FETCH NEXT FROM @moduleDetailsCursor 
	    INTO @moduleId
		

	    WHILE @@FETCH_STATUS = 0
	    BEGIN
	    
					SET @permissionsCursor = CURSOR FOR
					SELECT Id FROM dbo.Permissions
					
					OPEN @permissionsCursor 
					FETCH NEXT FROM @permissionsCursor 
					INTO @permissionId
	
					WHILE @@FETCH_STATUS = 0
					BEGIN
					  
							SET @rolesCursor = CURSOR FOR
							SELECT Id FROM dbo.UserRoles
							WHERE Name != 'Super Admin'
							
							OPEN @rolesCursor 
							FETCH NEXT FROM @rolesCursor 
							INTO @roleId
	
							WHILE @@FETCH_STATUS = 0
							BEGIN
							  
									IF NOT EXISTS (Select Id from UserRolePermissions where RoleId = @roleId and
												    ModuleId = @moduleId and CompanyId = @companyId and 
											     	PermissionId = @permissionId)

									BEGIN

									INSERT INTO UserRolePermissions (RoleId, PermissionId, ModuleId, CompanyId, IsAllowed)
															 VALUES (@roleId,@permissionId, @moduleId, @companyId,0 )

										FETCH NEXT FROM @rolesCursor INTO @roleId 

									END
	
							END; 
	
							CLOSE @rolesCursor ;
							DEALLOCATE @rolesCursor;

							FETCH NEXT FROM @permissionsCursor 
							INTO @permissionId 
	
					END; 
	
					CLOSE @permissionsCursor;
					DEALLOCATE @permissionsCursor;
		
			FETCH NEXT FROM @moduleDetailsCursor 
			INTO @moduleId 
	
	    END; 
	
	    CLOSE @moduleDetailsCursor ;
	    DEALLOCATE @moduleDetailsCursor;
	
END
GO

CREATE Trigger [dbo].[Trg_ModuleDetails_InsertUserRolePermission]
On [dbo].[ModuleDetails]
AFTER INSERT
AS
SET NOCOUNT ON;
BEGIN

		DECLARE @moduleDetailId INT = 0
		SELECT @moduleDetailId = i.id FROM inserted i
	
		DECLARE @companyCursor CURSOR, @permissionsCursor CURSOR, @rolesCursor CURSOR
		DECLARE @companyId INT, @permissionId INT, @roleId INT
		
		SET @companyCursor = CURSOR FOR
	    SELECT Id FROM dbo.Companies
		--WHERE IsActive = 1 and IsDeleted = 0
	    
		OPEN @companyCursor 
	    FETCH NEXT FROM @companyCursor 
	    INTO @companyId
		
	    WHILE @@FETCH_STATUS = 0
	    BEGIN
	    
					SET @permissionsCursor = CURSOR FOR
					SELECT Id FROM dbo.Permissions
					
					OPEN @permissionsCursor 
					FETCH NEXT FROM @permissionsCursor 
					INTO @permissionId
	
					WHILE @@FETCH_STATUS = 0
					BEGIN
					  
							SET @rolesCursor = CURSOR FOR
							SELECT Id FROM dbo.UserRoles
							WHERE Name != 'Super Admin'
							
							OPEN @rolesCursor 
							FETCH NEXT FROM @rolesCursor 
							INTO @roleId
	
							WHILE @@FETCH_STATUS = 0
							BEGIN
									IF NOT EXISTS (Select Id from UserRolePermissions where RoleId = @roleId and
												    ModuleId = @moduleDetailId and CompanyId = @companyId and 
											     	PermissionId = @permissionId)

									BEGIN

									INSERT INTO UserRolePermissions (RoleId, PermissionId, ModuleId, CompanyId, IsAllowed)
															 VALUES (@roleId,@permissionId, @moduleDetailId, @companyId,0 )

										FETCH NEXT FROM @rolesCursor 
									INTO @roleId 

									END
	
							END; 
	
							CLOSE @rolesCursor ;
							DEALLOCATE @rolesCursor;

							FETCH NEXT FROM @permissionsCursor 
							INTO @permissionId 
	
					END; 
	
					CLOSE @permissionsCursor;
					DEALLOCATE @permissionsCursor;
		
			FETCH NEXT FROM @companyCursor 
			INTO @companyId 
	
	    END; 
	
	    CLOSE @companyCursor ;
	    DEALLOCATE @companyCursor;

		-- For Super Admin

		SET @permissionsCursor = CURSOR FOR
		SELECT Id FROM dbo.Permissions
		
		OPEN @permissionsCursor 
		FETCH NEXT FROM @permissionsCursor 
		INTO @permissionId
	
		WHILE @@FETCH_STATUS = 0
		BEGIN
		  
				SET @rolesCursor = CURSOR FOR
				SELECT Id FROM dbo.UserRoles
				WHERE Name = 'Super Admin'
				
				OPEN @rolesCursor 
				FETCH NEXT FROM @rolesCursor 
				INTO @roleId
	
				WHILE @@FETCH_STATUS = 0
				BEGIN
				  
						IF NOT EXISTS (Select Id from UserRolePermissions where RoleId = @roleId and
												    ModuleId = @moduleDetailId and CompanyId = null and 
											     	PermissionId = @permissionId)

						BEGIN
						INSERT INTO UserRolePermissions (RoleId, PermissionId, ModuleId, CompanyId, IsAllowed)
												 VALUES (@roleId,@permissionId, @moduleDetailId, null, 1)

						END 

						FETCH NEXT FROM @rolesCursor 
						INTO @roleId 
	
				END; 
	
				CLOSE @rolesCursor ;
				DEALLOCATE @rolesCursor;

				FETCH NEXT FROM @permissionsCursor 
				INTO @permissionId 
	
		END; 
	
		CLOSE @permissionsCursor;
		DEALLOCATE @permissionsCursor;
	
END
GO

CREATE Trigger [dbo].[Trg_Permissions_InsertUserRolePermission]
On [dbo].[Permissions]
AFTER INSERT
AS
SET NOCOUNT ON;
BEGIN

		DECLARE @permissionId INT = 0
		SELECT @permissionId = i.Id FROM inserted i
	
		DECLARE @companyCursor CURSOR, @moduleDetaiilsCursor CURSOR, @rolesCursor CURSOR
		DECLARE @companyId INT, @moduleDetailsId INT, @roleId INT
		
		SET @companyCursor = CURSOR FOR
	    SELECT Id FROM dbo.Companies
		--WHERE IsActive = 1 and IsDeleted = 0
	    
		OPEN @companyCursor 
	    FETCH NEXT FROM @companyCursor 
	    INTO @companyId
		

	    WHILE @@FETCH_STATUS = 0
	    BEGIN
	    
					SET @moduleDetaiilsCursor = CURSOR FOR
					SELECT Id FROM dbo.ModuleDetails
					
					OPEN @moduleDetaiilsCursor 
					FETCH NEXT FROM @moduleDetaiilsCursor 
					INTO @moduleDetailsId
	
					WHILE @@FETCH_STATUS = 0
					BEGIN
					  
					  print(@permissionId)

							SET @rolesCursor = CURSOR FOR
							SELECT Id FROM dbo.UserRoles
							WHERE Name != 'Super Admin'
							
							OPEN @rolesCursor 
							FETCH NEXT FROM @rolesCursor 
							INTO @roleId
	
							WHILE @@FETCH_STATUS = 0
							BEGIN
							  
								    IF NOT EXISTS (Select Id from UserRolePermissions where RoleId = @roleId and
												    ModuleId = @moduleDetailsId and CompanyId = @companyId and 
											     	PermissionId = @permissionId)

									BEGIN
									INSERT INTO UserRolePermissions (RoleId, PermissionId, ModuleId, CompanyId, IsAllowed)
															 VALUES (@roleId,@permissionId, @moduleDetailsId, @companyId,0 )

									END

										FETCH NEXT FROM @rolesCursor INTO @roleId 
	
							END; 
	
							CLOSE @rolesCursor ;
							DEALLOCATE @rolesCursor;

							FETCH NEXT FROM @moduleDetaiilsCursor 
							INTO @moduleDetailsId 
	
					END; 
	
					CLOSE @moduleDetaiilsCursor;
					DEALLOCATE @moduleDetaiilsCursor;
		
			FETCH NEXT FROM @companyCursor 
			INTO @companyId 
	
	    END; 
	
	    CLOSE @companyCursor ;
	    DEALLOCATE @companyCursor;

		-- For Super Admin

		SET @moduleDetaiilsCursor = CURSOR FOR
		SELECT Id FROM dbo.ModuleDetails
		
		OPEN @moduleDetaiilsCursor 
		FETCH NEXT FROM @moduleDetaiilsCursor 
		INTO @moduleDetailsId
	
		WHILE @@FETCH_STATUS = 0
		BEGIN
		  
				SET @rolesCursor = CURSOR FOR
				SELECT Id FROM dbo.UserRoles
				WHERE Name = 'Super Admin'
				
				OPEN @rolesCursor 
				FETCH NEXT FROM @rolesCursor 
				INTO @roleId
	
				WHILE @@FETCH_STATUS = 0
				BEGIN
				  
						IF NOT EXISTS (Select Id from UserRolePermissions where RoleId = @roleId and
												    ModuleId = @moduleDetailsId and CompanyId = null and 
											     	PermissionId = @permissionId)

						BEGIN

						INSERT INTO UserRolePermissions (RoleId, PermissionId, ModuleId, CompanyId, IsAllowed)
												 VALUES (@roleId,@permissionId, @moduleDetailsId, null ,1 )

						END

							FETCH NEXT FROM @rolesCursor 
						INTO @roleId 
	
				END; 
	
				CLOSE @rolesCursor ;
				DEALLOCATE @rolesCursor;

				FETCH NEXT FROM @moduleDetaiilsCursor 
				INTO @moduleDetailsId 
	
		END; 
	
		CLOSE @moduleDetaiilsCursor;
		DEALLOCATE @moduleDetaiilsCursor;
	
END
GO

CREATE Trigger [dbo].[Trg_UserRoles_InsertUserRolePermission]
On [dbo].[UserRoles]
AFTER INSERT
AS
SET NOCOUNT ON;
BEGIN

		DECLARE @roleId INT = 0
		SELECT @roleId = i.Id FROM inserted i
	
		DECLARE @companyCursor CURSOR, @moduleDetaiilsCursor CURSOR, @permissionsCursor CURSOR
		DECLARE @companyId INT, @moduleDetailsId INT, @permissionId INT
		
		SET @companyCursor = CURSOR FOR
	    SELECT Id FROM dbo.Companies
		--WHERE IsActive = 1 and IsDeleted = 0
	    
		OPEN @companyCursor 
	    FETCH NEXT FROM @companyCursor 
	    INTO @companyId
		

	    WHILE @@FETCH_STATUS = 0
	    BEGIN
	    
					SET @moduleDetaiilsCursor = CURSOR FOR
					SELECT Id FROM dbo.ModuleDetails
					
					OPEN @moduleDetaiilsCursor 
					FETCH NEXT FROM @moduleDetaiilsCursor 
					INTO @moduleDetailsId
	
					WHILE @@FETCH_STATUS = 0
					BEGIN
					  
					  print(@permissionId)

							SET @permissionsCursor = CURSOR FOR
							SELECT Id FROM dbo.Permissions
							
							OPEN @permissionsCursor 
							FETCH NEXT FROM @permissionsCursor 
							INTO @permissionId
	
							WHILE @@FETCH_STATUS = 0
							BEGIN
							  
								   IF NOT EXISTS (Select Id from UserRolePermissions where RoleId = @roleId and
												    ModuleId = @moduleDetailsId and CompanyId = @companyId and 
											     	PermissionId = @permissionId)

									BEGIN
									INSERT INTO UserRolePermissions (RoleId, PermissionId, ModuleId, CompanyId, IsAllowed)
															 VALUES (@roleId,@permissionId, @moduleDetailsId, @companyId,0 )

									End

										FETCH NEXT FROM @permissionsCursor 
									INTO @permissionId 
	
							END; 
	
							CLOSE @permissionsCursor ;
							DEALLOCATE @permissionsCursor;

							FETCH NEXT FROM @moduleDetaiilsCursor 
							INTO @moduleDetailsId 
	
					END; 
	
					CLOSE @moduleDetaiilsCursor;
					DEALLOCATE @moduleDetaiilsCursor;
		
			FETCH NEXT FROM @companyCursor 
			INTO @companyId 
	
	    END; 
	
	    CLOSE @companyCursor ;
	    DEALLOCATE @companyCursor;
	
END
GO

CREATE PROCEDURE [dbo].[GetAircraftsList]  
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
        SELECT A.*, CP.Name CompanyName, AM.Name MakeName,AMD.Name ModelName, AC.Name Category from Aircrafts A
		LEFT JOIN  Companies CP on CP.Id = A.CompanyId
		LEFT JOIN  AircraftMakes AM on AM.Id = A.AircraftMakeId
		LEFT JOIN  AircraftModels AMD on AMD.Id = A.AircraftModelId
		LEFT JOIN  AircraftCategories AC on AC.Id = A.AircraftCategoryId

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
    SELECT TotalRecords,A.*, CP.Name CompanyName, AM.Name MakeName,AMD.Name ModelName, AC.Name Category from Aircrafts A
		LEFT JOIN  Companies CP on CP.Id = A.CompanyId
		LEFT JOIN  AircraftMakes AM on AM.Id = A.AircraftMakeId
		LEFT JOIN  AircraftModels AMD on AMD.Id = A.AircraftModelId
		LEFT JOIN  AircraftCategories AC on AC.Id = A.AircraftCategoryId
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = A.ID)  
   
END