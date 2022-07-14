
/****** Object:  Table [dbo].[AircraftCategories]    Script Date: 01-02-2022 09:37:22 ******/
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
/****** Object:  Table [dbo].[AircraftClasses]    Script Date: 01-02-2022 09:37:22 ******/
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

/****** Object:  Table [dbo].[CompanyServices]    Script Date: 22-04-2022 11:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CompanyServices](
	[Id] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NULL,
 CONSTRAINT [PK_CompanyServices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[AirCraftEquipments]    Script Date: 01-02-2022 09:37:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AirCraftEquipments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[StatusId] [int] NOT NULL,
	[AirCraftId] [bigint] NOT NULL,
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
	[CreatedBy] [bigint] NOT NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedBy] [bigint] NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[UpdatedBy] [bigint] NULL,
 CONSTRAINT [PK_AirCraftEquipments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AircraftEquipmentTimes]    Script Date: 01-02-2022 09:37:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AircraftEquipmentTimes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[EquipmentName] [varchar](200) NOT NULL,
	[Hours] [decimal](18, 2) NOT NULL,
	[AircraftId] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [bigint] NULL,
 CONSTRAINT [PK_AircraftEquipementTimes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AircraftMakes]    Script Date: 01-02-2022 09:37:22 ******/
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
/****** Object:  Table [dbo].[AircraftModels]    Script Date: 01-02-2022 09:37:22 ******/
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
/****** Object:  Table [dbo].[Aircrafts]    Script Date: 01-02-2022 09:37:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Aircrafts](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
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
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [bigint] NULL,
 CONSTRAINT [PK__Aircraft__3214EC07C1A8802D] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[AircraftScheduleDetails]    Script Date: 01-02-2022 09:37:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AircraftScheduleDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AircraftScheduleId] [bigint] NOT NULL,
	[FlightStatus] [varchar](15) NULL,
	[CheckInTime] [datetime] NULL,
	[CheckOutTime] [datetime] NULL,
	[CheckInBy] [bigint] NULL,
	[CheckOutBy] [bigint] NULL,
	[IsCheckOut] [bit] NOT NULL,
 CONSTRAINT [PK_AircraftScheduleDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AircraftScheduleHobbsTimes]    Script Date: 01-02-2022 09:37:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AircraftScheduleHobbsTimes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AircraftScheduleId] [bigint] NOT NULL,
	[AircraftEquipmentTimeId] [bigint] NULL,
	[OutTime] [decimal](8, 2) NULL,
	[InTime] [decimal](8, 2) NULL,
	[TotalTime] [decimal](8, 2) NULL,
 CONSTRAINT [PK_AircraftScheduleHobbsTimes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AircraftSchedules]    Script Date: 01-02-2022 09:37:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AircraftSchedules](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SchedulActivityTypeId] [int] NULL,
	[ReservationId] [uniqueidentifier] NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[EndDateTime] [datetime] NOT NULL,
	[IsRecurring] [bit] NOT NULL,
	[Member1Id] [bigint] NULL,
	[Member2Id] [bigint] NULL,
	[InstructorId] [bigint] NULL,
	[ScheduleTitle] [varchar](100) NULL,
	[AircraftId] [bigint] NULL,
	[FlightType] [varchar](15) NULL,
	[FlightRules] [varchar](15) NULL,
	[EstFlightHours] [decimal](5, 2) NULL,
	[FlightRoutes] [varchar](max) NULL,
	[Comments] [varchar](max) NULL,
	[PrivateComments] [varchar](max) NULL,
	[StandBy] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [bigint] NULL,
	[DeletedOn] [datetime] NULL,
 CONSTRAINT [PK_AircraftSchedules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_AircraftSchedules] UNIQUE NONCLUSTERED 
(
	[ReservationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 01-02-2022 09:37:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Address] [varchar](300) NOT NULL,
	[ContactNo] [varchar](20) NULL,
	[TimeZone] [varchar](100) NOT NULL,
	[Website] [varchar](50) NULL,
	[PrimaryAirport] [varchar](200) NULL,
	[PrimaryServiceId] [smallint] NULL,
	[Logo] [varchar](200) NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [bigint] NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 01-02-2022 09:37:22 ******/
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
/****** Object:  Table [dbo].[EmailTokens]    Script Date: 01-02-2022 09:37:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailTokens](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Token] [varchar](500) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ExpireOn] [datetime] NULL,
	[UserId] [bigint] NULL,
	[EmailType] [varchar](25) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EquipmentClassifications]    Script Date: 01-02-2022 09:37:22 ******/
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
/****** Object:  Table [dbo].[EquipmentStatuses]    Script Date: 01-02-2022 09:37:22 ******/
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
/****** Object:  Table [dbo].[FlightSimulatorClasses]    Script Date: 01-02-2022 09:37:22 ******/
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
/****** Object:  Table [dbo].[InstructorTypes]    Script Date: 01-02-2022 09:37:22 ******/
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
/****** Object:  Table [dbo].[ModuleDetails]    Script Date: 01-02-2022 09:37:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  Table [dbo].[ModuleDetails]    Script Date: 28-02-2022 13:23:56 ******/
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
	[IsAdministrationModule] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Permissions]    Script Date: 01-02-2022 09:37:22 ******/
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
/****** Object:  Table [dbo].[ScheduleActivityTypes]    Script Date: 01-02-2022 09:37:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduleActivityTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ScheduleActivityTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[UserRolePermissions]    Script Date: 01-02-2022 09:37:22 ******/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRolePermissions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NULL,
	[PermissionId] [int] NULL,
	[ModuleId] [int] NULL,
	[CompanyId] [int] NULL,
	[IsAllowed] [bit] NOT NULL,
	[IsAllowedForMobileApp] [bit] NOT NULL,
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

/****** Object:  Table [dbo].[UserRoles]    Script Date: 01-02-2022 09:37:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Priority] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoleVsScheduleActivityType]    Script Date: 01-02-2022 09:37:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoleVsScheduleActivityType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserRoleId] [int] NOT NULL,
	[ActivityTypeIds] [varchar](50) NULL,
 CONSTRAINT [PK_UserRoleVsScheduleActivityType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 01-02-2022 09:37:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ImageName] [varchar](200) NULL,
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
	[CreatedBy] [bigint] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [bigint] NULL,
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

/****** Object:  Table [dbo].[Documents]    Script Date: 16-03-2022 09:04:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Documents](
	[Id] [uniqueidentifier] NOT NULL,
	[DisplayName] [varchar](200) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[TagIds] [varchar](200) NULL,
	[Size(InKB)] [bigint] NULL,
	[TotalDownloads] [bigint] NULL,
	[TotalShares] [bigint] NULL,
	[LastShareDate] [datetime] NULL,
	[IsShareable] BIT NOT NULL DEFAULT 1,
	[ExpirationDate] [datetime] NULL,
	[CompanyId] [int] NULL,
	[ModuleId] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [bigint] NULL,
	[DeletedOn] [datetime] NULL,
 CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[DocumentTags]    Script Date: 22-03-2022 09:49:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DocumentTags](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TagName] [varchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [bigint] NULL,
	[DeletedOn] [datetime] NULL,
 CONSTRAINT [PK_DocumentTags] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[UserPreferences]    Script Date: 18-02-2022 11:59:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserPreferences](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[PreferenceType] [varchar](100) NOT NULL,
	[PreferencesIds] [varchar](500) NULL,
 CONSTRAINT [PK_UserPreference] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[SubscriptionPlans]    Script Date: 29-03-2022 16:14:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SubscriptionPlans](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[ModuleIds] [varchar](50) NULL,
	[IsCombo] [bit] NOT NULL,
	[Price] [numeric](6, 2) NOT NULL,
	[Duration(InMonths)] [smallint] NOT NULL,
	[Description] VARCHAR (500),
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [bigint] NULL,
	[DeletedOn] [datetime] NULL,
 CONSTRAINT [PK_SubscriptionPlans] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[BillingHistories]    Script Date: 31-03-2022 17:01:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BillingHistories](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[SubscriptionPlanName] [varchar](50) NOT NULL,
	[ModuleIds] [varchar](50) NOT NULL,
	IsActive bit NOT NULL,
	[IsCombo] [bit] NOT NULL,
	[Price] [numeric](8, 2) NOT NULL,
	[Duration] [smallint] NOT NULL,
	[PlanStartDate] [datetime] NOT NULL,
	[PlanEndDate] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_BillingHistories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Timezones]    Script Date: 06-05-2022 15:23:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Timezones](
	[Id] [smallint] IDENTITY(1,1) NOT NULL,
	[Timezone] [varchar](100) NOT NULL,
	[Offset] [varchar](10) NOT NULL,
 CONSTRAINT [PK_Timezones] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Locations]    Script Date: 06-05-2022 15:38:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Locations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PhysicalAddress] [varchar](max) NOT NULL,
	[TimezoneId] [smallint] NOT NULL,
	[AirportCode] [varchar](100) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [bigint] NULL,
	[DeletedOn] [datetime] NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
---------------------------------------------------------------------------------------------------------------
--					CONSTRAINT
---------------------------------------------------------------------------------------------------------------

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
ALTER TABLE [dbo].[AircraftScheduleDetails] ADD  CONSTRAINT [DF_AircraftScheduleDetails_IsCheckout]  DEFAULT ((0)) FOR [IsCheckOut]
GO
ALTER TABLE [dbo].[AircraftSchedules] ADD  CONSTRAINT [DF_AircraftSchedules_IsRecurring]  DEFAULT ((0)) FOR [IsRecurring]
GO
ALTER TABLE [dbo].[AircraftSchedules] ADD  CONSTRAINT [DF_AircraftSchedules_StandBy]  DEFAULT ((0)) FOR [StandBy]
GO
ALTER TABLE [dbo].[AircraftSchedules] ADD  CONSTRAINT [DF_AircraftSchedules_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AircraftSchedules] ADD  CONSTRAINT [DF_AircraftSchedules_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Companies] ADD  CONSTRAINT [DF_Companies_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Companies] ADD  CONSTRAINT [DF_Companies_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ModuleDetails] ADD  CONSTRAINT [DF_ModuleDetails_OrderNo]  DEFAULT ((0)) FOR [OrderNo]
GO
ALTER TABLE [dbo].[ModuleDetails] ADD  CONSTRAINT [DF_ModuleDetails_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ModuleDetails] ADD  DEFAULT ((0)) FOR [IsAdministrationModule]
GO
ALTER TABLE [dbo].[ScheduleActivityTypes] ADD  CONSTRAINT [DF_ScheduleActivityTypes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UserRolePermissions] ADD  DEFAULT ((0)) FOR [IsAllowed]
GO
ALTER TABLE [dbo].[UserRolePermissions] ADD  DEFAULT ((0)) FOR [IsAllowedForMobileApp]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [df_IsSendEmailInvite]  DEFAULT ((0)) FOR [IsSendEmailInvite]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [df_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [df_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__Users__IsSendTex__52593CB8]  DEFAULT ((0)) FOR [DeletedOn]
GO
ALTER TABLE [dbo].[AirCraftEquipments]  WITH CHECK ADD  CONSTRAINT [FK_AirCraftEquipments_Classifications] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AirCraftEquipments] CHECK CONSTRAINT [FK_AirCraftEquipments_Classifications]
GO
ALTER TABLE [dbo].[AirCraftEquipments]  WITH CHECK ADD  CONSTRAINT [FK_AirCraftEquipments_Statuses] FOREIGN KEY([StatusId])
REFERENCES [dbo].[EquipmentStatuses] ([Id])
GO
ALTER TABLE [dbo].[AirCraftEquipments] CHECK CONSTRAINT [FK_AirCraftEquipments_Statuses]
GO
ALTER TABLE [dbo].[AirCraftEquipments]  WITH CHECK ADD  CONSTRAINT [FK_AirCraftEquipments_Users] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AirCraftEquipments] CHECK CONSTRAINT [FK_AirCraftEquipments_Users]
GO
ALTER TABLE [dbo].[AirCraftEquipments]  WITH CHECK ADD  CONSTRAINT [FK_AirCraftEquipments_Users1] FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AirCraftEquipments] CHECK CONSTRAINT [FK_AirCraftEquipments_Users1]
GO
ALTER TABLE [dbo].[AircraftEquipmentTimes]  WITH CHECK ADD  CONSTRAINT [FK_AircraftEquipmentTimes_AircraftId] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AircraftEquipmentTimes] CHECK CONSTRAINT [FK_AircraftEquipmentTimes_AircraftId]
GO
ALTER TABLE [dbo].[AircraftEquipmentTimes]  WITH CHECK ADD  CONSTRAINT [FK_AircraftEquipmentTimes_Users] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AircraftEquipmentTimes] CHECK CONSTRAINT [FK_AircraftEquipmentTimes_Users]
GO
ALTER TABLE [dbo].[AircraftEquipmentTimes]  WITH CHECK ADD  CONSTRAINT [FK_AircraftEquipmentTimes_Users1] FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AircraftEquipmentTimes] CHECK CONSTRAINT [FK_AircraftEquipmentTimes_Users1]
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
ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_Aircrafts_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_Aircrafts_Users]
GO
ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_Aircrafts_Users1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_Aircrafts_Users1]
GO
ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_Aircrafts_Users2] FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_Aircrafts_Users2]
GO
ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_FlightSimulatorClass_Aircraft] FOREIGN KEY([FlightSimulatorClassId])
REFERENCES [dbo].[FlightSimulatorClasses] ([Id])
GO
ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_FlightSimulatorClass_Aircraft]
GO
ALTER TABLE [dbo].[AircraftScheduleDetails]  WITH CHECK ADD  CONSTRAINT [FK_AircraftScheduleDetails_AircraftSchedules] FOREIGN KEY([AircraftScheduleId])
REFERENCES [dbo].[AircraftSchedules] ([Id])
GO
ALTER TABLE [dbo].[AircraftScheduleDetails] CHECK CONSTRAINT [FK_AircraftScheduleDetails_AircraftSchedules]
GO
ALTER TABLE [dbo].[AircraftScheduleDetails]  WITH CHECK ADD  CONSTRAINT [FK_AircraftScheduleDetails_Users] FOREIGN KEY([CheckInBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AircraftScheduleDetails] CHECK CONSTRAINT [FK_AircraftScheduleDetails_Users]
GO
ALTER TABLE [dbo].[AircraftScheduleDetails]  WITH CHECK ADD  CONSTRAINT [FK_AircraftScheduleDetails_Users1] FOREIGN KEY([CheckOutBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AircraftScheduleDetails] CHECK CONSTRAINT [FK_AircraftScheduleDetails_Users1]
GO
ALTER TABLE [dbo].[AircraftScheduleHobbsTimes]  WITH CHECK ADD  CONSTRAINT [FK_AircraftScheduleHobbsTimes_AircraftEquipmentTimes] FOREIGN KEY([AircraftEquipmentTimeId])
REFERENCES [dbo].[AircraftEquipmentTimes] ([Id])
GO
ALTER TABLE [dbo].[AircraftScheduleHobbsTimes] CHECK CONSTRAINT [FK_AircraftScheduleHobbsTimes_AircraftEquipmentTimes]
GO
ALTER TABLE [dbo].[AircraftScheduleHobbsTimes]  WITH CHECK ADD  CONSTRAINT [FK_AircraftScheduleHobbsTimes_AircraftSchedules] FOREIGN KEY([AircraftScheduleId])
REFERENCES [dbo].[AircraftSchedules] ([Id])
GO
ALTER TABLE [dbo].[AircraftScheduleHobbsTimes] CHECK CONSTRAINT [FK_AircraftScheduleHobbsTimes_AircraftSchedules]
GO
ALTER TABLE [dbo].[AircraftSchedules]  WITH CHECK ADD  CONSTRAINT [FK_AircraftSchedules_Aircrafts] FOREIGN KEY([AircraftId])
REFERENCES [dbo].[Aircrafts] ([Id])
GO
ALTER TABLE [dbo].[AircraftSchedules] CHECK CONSTRAINT [FK_AircraftSchedules_Aircrafts]
GO
ALTER TABLE [dbo].[AircraftSchedules]  WITH CHECK ADD  CONSTRAINT [FK_AircraftSchedules_ScheduleActivityTypes] FOREIGN KEY([SchedulActivityTypeId])
REFERENCES [dbo].[ScheduleActivityTypes] ([Id])
GO
ALTER TABLE [dbo].[AircraftSchedules] CHECK CONSTRAINT [FK_AircraftSchedules_ScheduleActivityTypes]
GO
ALTER TABLE [dbo].[AircraftSchedules]  WITH CHECK ADD  CONSTRAINT [FK_AircraftSchedules_UsersCreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AircraftSchedules] CHECK CONSTRAINT [FK_AircraftSchedules_UsersCreatedBy]
GO
ALTER TABLE [dbo].[AircraftSchedules]  WITH CHECK ADD  CONSTRAINT [FK_AircraftSchedules_UsersDeletedBy] FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AircraftSchedules] CHECK CONSTRAINT [FK_AircraftSchedules_UsersDeletedBy]
GO
ALTER TABLE [dbo].[AircraftSchedules]  WITH CHECK ADD  CONSTRAINT [FK_AircraftSchedules_UsersInstructorId] FOREIGN KEY([InstructorId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AircraftSchedules] CHECK CONSTRAINT [FK_AircraftSchedules_UsersInstructorId]
GO
ALTER TABLE [dbo].[AircraftSchedules]  WITH CHECK ADD  CONSTRAINT [FK_AircraftSchedules_UsersMember1Id] FOREIGN KEY([Member1Id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AircraftSchedules] CHECK CONSTRAINT [FK_AircraftSchedules_UsersMember1Id]
GO
ALTER TABLE [dbo].[AircraftSchedules]  WITH CHECK ADD  CONSTRAINT [FK_AircraftSchedules_UsersMember2Id] FOREIGN KEY([Member2Id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AircraftSchedules] CHECK CONSTRAINT [FK_AircraftSchedules_UsersMember2Id]
GO
ALTER TABLE [dbo].[AircraftSchedules]  WITH CHECK ADD  CONSTRAINT [FK_AircraftSchedules_UsersUpdatedBy] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AircraftSchedules] CHECK CONSTRAINT [FK_AircraftSchedules_UsersUpdatedBy]
GO
ALTER TABLE [dbo].[Companies]  WITH CHECK ADD FOREIGN KEY([PrimaryServiceId])
REFERENCES [dbo].[CompanyServices] ([Id])
GO
ALTER TABLE [dbo].[Companies]  WITH CHECK ADD  CONSTRAINT [FK_Companies_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Companies] CHECK CONSTRAINT [FK_Companies_Users]
GO
ALTER TABLE [dbo].[Companies]  WITH CHECK ADD  CONSTRAINT [FK_Companies_Users1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Companies] CHECK CONSTRAINT [FK_Companies_Users1]
GO
ALTER TABLE [dbo].[Companies]  WITH CHECK ADD  CONSTRAINT [FK_Companies_Users2] FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Companies] CHECK CONSTRAINT [FK_Companies_Users2]
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
ALTER TABLE [dbo].[UserRoleVsScheduleActivityType]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleVsScheduleActivityType_UserRoles] FOREIGN KEY([UserRoleId])
REFERENCES [dbo].[UserRoles] ([Id])
GO
ALTER TABLE [dbo].[UserRoleVsScheduleActivityType] CHECK CONSTRAINT [FK_UserRoleVsScheduleActivityType_UserRoles]
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
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Users1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Users1]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Users2] FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Users2]
GO
ALTER TABLE [dbo].[Documents]  WITH CHECK ADD  CONSTRAINT [FK_Documents_Users] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Documents] CHECK CONSTRAINT [FK_Documents_Users]
GO

ALTER TABLE [dbo].[Documents]  WITH CHECK ADD  CONSTRAINT [FK_Documents_Users1] FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Documents] CHECK CONSTRAINT [FK_Documents_Users1]
GO

ALTER TABLE [dbo].[Documents]  WITH CHECK ADD  CONSTRAINT [FK_Documents_Users2] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Documents] CHECK CONSTRAINT [FK_Documents_Users2]
GO

ALTER TABLE [dbo].[DocumentTags] ADD  CONSTRAINT [DF_DocumentTags_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[DocumentTags] ADD  CONSTRAINT [DF_DocumentTags_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[DocumentTags]  WITH CHECK ADD  CONSTRAINT [FK_DocumentTags_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[DocumentTags] CHECK CONSTRAINT [FK_DocumentTags_Users]
GO

ALTER TABLE [dbo].[DocumentTags]  WITH CHECK ADD  CONSTRAINT [FK_DocumentTags_Users1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[DocumentTags] CHECK CONSTRAINT [FK_DocumentTags_Users1]
GO

ALTER TABLE [dbo].[DocumentTags]  WITH CHECK ADD  CONSTRAINT [FK_DocumentTags_Users2] FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[DocumentTags] CHECK CONSTRAINT [FK_DocumentTags_Users2]
GO

ALTER TABLE [dbo].[UserPreferences]  WITH CHECK ADD  CONSTRAINT [FK_UserPreference_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[UserPreferences] CHECK CONSTRAINT [FK_UserPreference_Users]
GO

ALTER TABLE [dbo].[SubscriptionPlans] ADD  CONSTRAINT [DF_SubscriptionPlans_IsCombo]  DEFAULT ((0)) FOR [IsCombo]
GO

ALTER TABLE [dbo].[SubscriptionPlans]  WITH CHECK ADD  CONSTRAINT [FK_SubscriptionPlans_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[SubscriptionPlans] CHECK CONSTRAINT [FK_SubscriptionPlans_Users]
GO

ALTER TABLE [dbo].[SubscriptionPlans]  WITH CHECK ADD  CONSTRAINT [FK_SubscriptionPlans_Users1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[SubscriptionPlans] CHECK CONSTRAINT [FK_SubscriptionPlans_Users1]
GO

ALTER TABLE [dbo].[SubscriptionPlans]  WITH CHECK ADD  CONSTRAINT [FK_SubscriptionPlans_Users2] FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[SubscriptionPlans] CHECK CONSTRAINT [FK_SubscriptionPlans_Users2]
GO

ALTER TABLE [dbo].[BillingHistories]  WITH CHECK ADD  CONSTRAINT [FK_BillingHistories_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[BillingHistories] CHECK CONSTRAINT [FK_BillingHistories_Users]
GO

ALTER TABLE [dbo].[Locations]  WITH CHECK ADD  CONSTRAINT [FK_Locations_Timezones] FOREIGN KEY([TimezoneId])
REFERENCES [dbo].[Timezones] ([Id])
GO

ALTER TABLE [dbo].[Locations] CHECK CONSTRAINT [FK_Locations_Timezones]
GO

ALTER TABLE [dbo].[Locations]  WITH CHECK ADD  CONSTRAINT [FK_Locations_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Locations] CHECK CONSTRAINT [FK_Locations_Users]
GO

ALTER TABLE [dbo].[Locations]  WITH CHECK ADD  CONSTRAINT [FK_Locations_Users1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Locations] CHECK CONSTRAINT [FK_Locations_Users1]
GO

ALTER TABLE [dbo].[Locations]  WITH CHECK ADD  CONSTRAINT [FK_Locations_Users2] FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Locations] CHECK CONSTRAINT [FK_Locations_Users2]
GO


---------------------------------------------------------------------------------------------------------------------
						--PROCEDURE
---------------------------------------------------------------------------------------------------------------------
/****** Object:  StoredProcedure [dbo].[GetAircraftEquipmentList]    Script Date: 01-02-2022 09:37:22 ******/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAircraftEquipmentList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'Status',  
    @SortOrder NVARCHAR(20) = 'ASC',
	@AircraftId INT
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
        SELECT AE.*,AES.Name [Status], EC.Name [Classification] from AirCraftEquipments AE
		LEFT JOIN  EquipmentStatuses AES on AE.StatusId = AES.Id
		LEFT JOIN  EquipmentClassifications EC on AE.ClassificationId = EC.Id

        WHERE AE.AirCraftId = @AircraftId AND AE.IsDeleted = 0 AND (@SearchValue= '' OR  (   
              AES.Name LIKE '%' + @SearchValue + '%' OR
			  EC.Name LIKE '%' + @SearchValue + '%' OR
			  AE.Item LIKE '%' + @SearchValue + '%' OR
			  AE.Model LIKE '%' + @SearchValue + '%' OR
			  AE.Make LIKE '%' + @SearchValue + '%'
            ) ) 
  
            ORDER BY  
            CASE WHEN (@SortColumn = 'Status' AND @SortOrder='ASC')  
                        THEN AES.Name  
            END ASC,  
            CASE WHEN (@SortColumn = 'Status' AND @SortOrder='DESC')  
                        THEN AES.Name  
            END DESC, 
			CASE WHEN (@SortColumn = 'Classification' AND @SortOrder='ASC')  
                        THEN EC.Name  
            END ASC,  
            CASE WHEN (@SortColumn = 'Classification' AND @SortOrder='DESC')  
                        THEN EC.Name  
            END DESC, 
			CASE WHEN (@SortColumn = 'Item' AND @SortOrder='ASC')  
                        THEN AE.Item  
            END ASC,  
            CASE WHEN (@SortColumn = 'Item' AND @SortOrder='DESC')  
                        THEN AE.Item  
            END DESC, 
            CASE WHEN (@SortColumn = 'Model' AND @SortOrder='ASC')  
                        THEN AE.Model  
            END ASC,  
            CASE WHEN (@SortColumn = 'Model' AND @SortOrder='DESC')  
                        THEN AE.Model  
            END DESC,
			CASE WHEN (@SortColumn = 'Make' AND @SortOrder='ASC')  
                        THEN AE.Make 
            END ASC,  
            CASE WHEN (@SortColumn = 'Make' AND @SortOrder='DESC')  
                        THEN AE.Make  
            END DESC
			
            OFFSET @PageSize * (@PageNo - 1) ROWS  
            FETCH NEXT @PageSize ROWS ONLY  
    ),  
    CTE_TotalRows AS   
    (  
        select count(AE.ID) as TotalRecords from AirCraftEquipments AE
		LEFT JOIN  EquipmentStatuses AES on AE.StatusId = AES.Id
		LEFT JOIN  EquipmentClassifications EC on AE.ClassificationId = EC.Id

       
         WHERE AE.AirCraftId = @AircraftId AND AE.IsDeleted = 0 AND (@SearchValue= '' OR  (   
              AES.Name LIKE '%' + @SearchValue + '%' OR
			  EC.Name LIKE '%' + @SearchValue + '%' OR
			  AE.Item LIKE '%' + @SearchValue + '%' OR
			  AE.Model LIKE '%' + @SearchValue + '%' OR
			  AE.Make LIKE '%' + @SearchValue + '%'
            ) ) 
    )  

    Select TotalRecords, AE.*,AES.Name [Status], EC.Name [Classification] from AirCraftEquipments AE
		LEFT JOIN  EquipmentStatuses AES on AE.StatusId = AES.Id
		LEFT JOIN  EquipmentClassifications EC on AE.ClassificationId = EC.Id
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = AE.ID)  
   
END
GO
/****** Object:  StoredProcedure [dbo].[GetAircraftsList]    Script Date: 01-02-2022 09:37:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
GO


/****** Object:  StoredProcedure [dbo].[GetCompanyList]    Script Date: 28-04-2022 15:57:20 ******/
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
    @SortOrder NVARCHAR(20) = 'ASC',
	@CompanyId INT = NULL
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
        SELECT * from Companies 
		
        WHERE  IsDeleted = 0 and 
		((ISNULL(@CompanyId,0)=0)
				OR (Id = @CompanyId))
				and (@SearchValue= '' OR  (   
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
		
        WHERE  IsDeleted = 0 and 
		((ISNULL(@CompanyId,0)=0)
				OR (Id = @CompanyId))
				and (@SearchValue= '' OR  (   
              Name LIKE '%' + @SearchValue + '%' 
            )  )
    )  
    Select  * from CTE_Results 
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = ID)  
   
END

/****** Object:  StoredProcedure [dbo].[GetInstructorTypeList]    Script Date: 03-12-2021 16:50:57 ******/
SET ANSI_NULLS ON


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

/****** Object:  StoredProcedure [dbo].[GetUsersList]    Script Date: 10-05-2022 17:53:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUsersList]  
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
	AS IsInstructor,ISNULL(U.IsActive, 0 ) AS IsActive,Ur.Name as UserRole, CP.Id AS CompanyId,
	CP.Name AS CompanyName,	U.ImageName as ProfileImage from Users U
	LEFT JOIN  UserRoles UR on UR.Id=U.RoleId
	LEFT JOIN  Companies CP on CP.Id = U.CompanyId
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = U.ID)  
   
END
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
        SELECT URP.*,UR.Name,PR.PermissionType,MD.Name AS ModuleName, MD.IsAdministrationModule, 
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
            END DESC,   
   CASE WHEN (@SortColumn = 'IsAllowedForMobileApp' AND @SortOrder='ASC')    
                        THEN URP.IsAllowed  
            END ASC,    
            CASE WHEN (@SortColumn = 'IsAllowedForMobileApp' AND @SortOrder='DESC')    
                         THEN URP.IsAllowed  
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
  
    Select TotalRecords, URP.*,UR.Name  AS RoleName,PR.PermissionType, CP.Name AS CompanyName,MD.IsAdministrationModule, 
    MD.Name ModuleName,MD.ControllerName, MD.ActionName, MD.Icon, MD.DisplayName,  
    MD.OrderNo from UserRolePermissions URP  
  LEFT JOIN  UserRoles UR on UR.Id = URP.RoleId  
  LEFT JOIN  Permissions PR on PR.Id = URP.PermissionId   
  LEFT JOIN  ModuleDetails MD on MD.Id = URp.ModuleId  
  LEFT JOIN  Companies CP on CP.Id = URP.CompanyId  
 , CTE_TotalRows     
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = URP.ID)    
     
END  

/****** Object:  StoredProcedure [dbo].[GetReservationList]    Script Date: 28-04-2022 16:08:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetReservationList]  
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
	@AircraftId BIGINT = NULL,
	@ReservationType VARCHAR(10) = NULL
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
				AND
				(((ISNULL(@ReservationType,'')='')
				OR ((@ReservationType = 'Past') AND EndDateTime < SYSDATETIME()))
				OR
				((ISNULL(@ReservationType,'')='')
				OR ((@ReservationType = 'Future') AND StartDateTime > SYSDATETIME())))
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
				OR (acs.EndDateTime  <= @EndDate ))
				AND
				(((ISNULL(@ReservationType,'')='')
				OR ((@ReservationType = 'Past') AND EndDateTime < SYSDATETIME()))
				OR
				((ISNULL(@ReservationType,'')='')
				OR ((@ReservationType = 'Future') AND StartDateTime > SYSDATETIME())))
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
GO

/****** Object:  StoredProcedure [dbo].[GetLocationsList]    Script Date: 10-05-2022 11:10:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetLocationsList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'AirportCode',  
    @SortOrder NVARCHAR(20) = 'ASC'  
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
        SELECT l.*, tz.Offset, tz.Timezone from Locations l
		INNER JOIN Timezones  tz
		ON l.TimezoneId = tz.Id

  
        WHERE  (@SearchValue= '' OR  (   
              AirportCode LIKE '%' + @SearchValue + '%' 
            )) AND l.DeletedBy Is  null  
  
            ORDER BY  
            CASE WHEN (@SortColumn = 'AirportCode' AND @SortOrder='ASC')  
                        THEN [AirportCode]  
            END ASC,
			CASE WHEN (@SortColumn = 'AirportCode' AND @SortOrder='DESC')  
                        THEN [AirportCode]  
            END DESC

            OFFSET @PageSize * (@PageNo - 1) ROWS  
            FETCH NEXT @PageSize ROWS ONLY  
    ),  
    CTE_TotalRows AS   
    (  
        select count(ID) as TotalRecords from Locations 
		
        WHERE  (@SearchValue= '' OR  (   
              AirportCode LIKE '%' + @SearchValue + '%' 
            ))   AND DeletedBy Is  null
    )  
    Select   * from CTE_Results 
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = ID)  
   
END
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

/****** Object:  StoredProcedure [dbo].[GetDocumentList]    Script Date: 07-04-2022 15:44:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetDocumentList]      
(           
    @SearchValue NVARCHAR(50) = NULL,      
    @PageNo INT = 1,      
    @PageSize INT = 10,      
    @SortColumn NVARCHAR(20) = 'DisplayName',      
    @SortOrder NVARCHAR(20) = 'ASC',    
	@CompanyId INT = NULL,    
	@ModuleId INT = NULL,
	@UserId bigint = NULL
)      
AS BEGIN      
    SET NOCOUNT ON;      
      
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))      
      
    ; WITH CTE_Results AS       
    (      
        SELECT d.Id, d.Name,d.UserId ,d.DisplayName, d.ExpirationDate, d.IsShareable , d.CompanyId,
		 ISNULL(d.TotalDownloads, 0) as TotalDownloads , ISNULL(d.TotalShares, 0) as TotalShares ,
		 d.LastShareDate,
		d.Type,ISNULL(d.[Size(InKB)], 0) as Size, m.Name as ModuleName,
		CP.Name as CompanyName, u.FirstName + ' ' + u.LastName as UserName   from Documents d    
		LEFT JOIN  Companies CP on CP.Id = d.CompanyId    
		LEFT JOIN ModuleDetails m on d.ModuleId = m.Id
		LEFT JOIN Users u on d.UserId = u.Id
    
        WHERE    
   (CP.IsDeleted = 0 OR  CP.IsDeleted IS NULL) AND    
   1 = 1 AND     
        (    
    ((ISNULL(@CompanyId,0)=0)    
    OR (d.CompanyId = @CompanyId))    
        
        ) 
		AND     
        (    
    ((ISNULL(@ModuleId,0)=0)    
    OR (d.ModuleId = @ModuleId))    
        
        ) 

		AND     
        (    
    ((ISNULL(@UserId,0)=0)    
    OR (d.UserId = @UserId))    
        
        ) 
   AND     
   d.IsDeleted = 0 AND    
   (@SearchValue= '' OR  (       
              d.DisplayName LIKE '%' + @SearchValue + '%' OR    
     Type LIKE '%' + @SearchValue + '%' OR    
     CP.Name LIKE '%' + @SearchValue + '%'    
            ))      
      
            ORDER BY      
            CASE WHEN (@SortColumn = 'DisplayName' AND @SortOrder='ASC')      
                        THEN d.DisplayName      
            END ASC,      
            CASE WHEN (@SortColumn = 'DisplayName' AND @SortOrder='DESC')      
                        THEN d.DisplayName      
            END DESC,     
   CASE WHEN (@SortColumn = 'Type' AND @SortOrder='ASC')      
                        THEN [Type]    
            END ASC,      
            CASE WHEN (@SortColumn = 'Type' AND @SortOrder='DESC')      
                        THEN [Type]     
            END DESC,     
   CASE WHEN (@SortColumn = 'Size' AND @SortOrder='ASC')      
                        THEN [Size(InKB)]      
            END ASC,      
            CASE WHEN (@SortColumn = 'Size' AND @SortOrder='DESC')      
                        THEN [Size(InKB)]      
            END DESC,  
	CASE WHEN (@SortColumn = 'ModuleName' AND @SortOrder='ASC')      
                        THEN m.Name    
            END ASC,      
            CASE WHEN (@SortColumn = 'ModuleName' AND @SortOrder='DESC')      
                        THEN m.Name     
            END DESC,
   CASE WHEN (@SortColumn = 'CompanyName' AND @SortOrder='ASC')      
                        THEN CP.Name    
            END ASC,      
            CASE WHEN (@SortColumn = 'CompanyName' AND @SortOrder='DESC')      
                        THEN CP.Name     
            END DESC,
	CASE WHEN (@SortColumn = 'UserName' AND @SortOrder='ASC')      
                        THEN u.FirstName    
            END ASC,      
            CASE WHEN (@SortColumn = 'UserName' AND @SortOrder='DESC')      
                        THEN u.FirstName     
            END DESC
            OFFSET @PageSize * (@PageNo - 1) ROWS      
            FETCH NEXT @PageSize ROWS ONLY      
    ),      
    CTE_TotalRows AS       
    (      
        select count(CTE_Results.Id) as TotalRecords from CTE_Results      
       
    )      
    Select TotalRecords, cr.Id,cr.UserId, cr.Name, cr.DisplayName,cr.IsShareable , cr.ExpirationDate, cr.CompanyId,
	 ISNULL(cr.TotalDownloads, 0) as TotalDownloads , ISNULL(cr.TotalShares, 0) as TotalShares ,
	 cr.LastShareDate,
	cr.Type, CONCAT(ISNULL(cr.Size, 0), ' KB') as Size, cr.ModuleName, 
	cr.CompanyName, cr.UserName from CTE_Results cr
	, CTE_TotalRows       
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = cr.Id)      
       
END

/****** Object:  StoredProcedure [dbo].[GetSubscriptionPlanList]    Script Date: 06-04-2022 16:05:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetSubscriptionPlanList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'CreatedOn',  
    @SortOrder NVARCHAR(20) = 'DESC',
	@IsActive BIT = NULL
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
  print(@sortColumn)
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
      select * from ( select SP.*,temp.ModulesName from (select sp.Id,STRING_AGG(md.displayname,',') 
		as ModulesName from (SELECT SP.id, value  
		FROM SubscriptionPlans sp
		CROSS APPLY STRING_SPLIT(sp.ModuleIds, ',')) as sp
		join ModuleDetails md on SP.value = md.Id group by sp.Id)as temp
		join SubscriptionPlans SP on SP.Id = temp.id
) as tempData
        WHERE
			((ISNULL(@IsActive,0)=0)
				OR (tempData.IsActive = @IsActive))
			AND
			(tempData.IsDeleted = 0 OR  tempData.IsDeleted IS NULL) AND
			
			(@SearchValue= '' OR  (   
              tempData.Name LIKE '%' + @SearchValue + '%' OR
			  tempData.Price LIKE '%' + @SearchValue + '%' OR
			  tempData.[Duration(InMonths)] LIKE '%' + @SearchValue + '%' 
            ))  
  
            ORDER BY  
            CASE WHEN (@SortColumn = 'Name' AND @SortOrder='ASC')  
                        THEN [Name]  
            END ASC,  
            CASE WHEN (@SortColumn = 'Name' AND @SortOrder='DESC')  
                        THEN [Name]  
            END DESC, 
			CASE WHEN (@SortColumn = 'Price' AND @SortOrder='ASC')  
					
                        THEN Price  
            END ASC,  
            CASE WHEN (@SortColumn = 'Price' AND @SortOrder='DESC')  
                        THEN Price  
            END DESC, 
			CASE WHEN (@SortColumn = 'Duration' AND @SortOrder='ASC')  
                        THEN [Duration(InMonths)]  
            END ASC,  
            CASE WHEN (@SortColumn = 'Duration' AND @SortOrder='DESC')  
                        THEN [Duration(InMonths)]  
            END DESC,
			   
            CASE WHEN (@SortColumn = 'CreatedOn' AND @SortOrder='ASC')  
                        THEN [CreatedOn]  
            END ASC,  
            CASE WHEN (@SortColumn = 'CreatedOn' AND @SortOrder='DESC')  
                        THEN [CreatedOn]  
            END DESC
           
            OFFSET @PageSize * (@PageNo - 1) ROWS  
            FETCH NEXT @PageSize ROWS ONLY  
    ),  
    CTE_TotalRows AS   
    (  
        select count(sp.Id)  as TotalRecords from SubscriptionPlans SP
       WHERE
	    ((ISNULL(@IsActive,0)=0)
				OR (SP.IsActive = @IsActive))
			AND
		(SP.IsDeleted = 0 OR  SP.IsDeleted IS NULL) AND
			
			(@SearchValue= '' OR  (   
              SP.Name LIKE '%' + @SearchValue + '%' OR
			  SP.Price LIKE '%' + @SearchValue + '%' OR
			  SP.[Duration(InMonths)] LIKE '%' + @SearchValue + '%' 
            ))  
   
    )  
    Select  TotalRecords, SP.Id, SP.Name, SP.ModuleIds, SP.ModulesName, SP.[Duration(InMonths)], sp.Description
	, SP.IsActive, SP.IsCombo, SP.Price from CTE_Results SP
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = SP.ID)  
   
END

/****** Object:  StoredProcedure [dbo].[GetBillingHistoryList]    Script Date: 06-04-2022 15:33:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetBillingHistoryList]  
(       
    @SearchValue NVARCHAR(50) = NULL,  
    @PageNo INT = 1,  
    @PageSize INT = 10,  
    @SortColumn NVARCHAR(20) = 'CreatedOn',  
    @SortOrder NVARCHAR(20) = 'DESC',
	@CompanyId INT = NULL,
	@UserId BIGINT = NULL
)  
AS BEGIN  
    SET NOCOUNT ON;  
  
  print(@sortColumn)
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))  
  
    ; WITH CTE_Results AS   
    (  
      select * from ( select BH.*, u.CompanyId ,temp.ModulesName from (select bh.Id,STRING_AGG(md.displayname,',') 
		as ModulesName from (SELECT BH.id, value  
		FROM BillingHistories bh
		CROSS APPLY STRING_SPLIT(bh.ModuleIds, ',')) as bh
		join ModuleDetails md on BH.value = md.Id group by bh.Id)as temp
		join BillingHistories BH on BH.Id = temp.id
		join Users u on BH.UserId =u.Id
) as tempData
        WHERE
			((ISNULL(@UserId,0)=0)
				OR (tempData.UserId = @UserId))
				AND
			((ISNULL(@CompanyId,0)=0)
				OR (tempData.CompanyId = @CompanyId))
				AND
			(@SearchValue= '' OR  (   
              tempData.SubscriptionPlanName LIKE '%' + @SearchValue + '%' OR
			  tempData.Price LIKE '%' + @SearchValue + '%' OR
			  tempData.[Duration] LIKE '%' + @SearchValue + '%' 
            ))  
  
            ORDER BY  
            CASE WHEN (@SortColumn = 'SubscriptionPlanName' AND @SortOrder='ASC')  
                        THEN [SubscriptionPlanName]  
            END ASC,  
            CASE WHEN (@SortColumn = 'SubscriptionPlanName' AND @SortOrder='DESC')  
                        THEN [SubscriptionPlanName]  
            END DESC, 
			CASE WHEN (@SortColumn = 'Price' AND @SortOrder='ASC')  
                        THEN Price  
            END ASC,  
            CASE WHEN (@SortColumn = 'Price' AND @SortOrder='DESC')  
                        THEN Price  
            END DESC, 
			CASE WHEN (@SortColumn = 'Duration' AND @SortOrder='ASC')  
                        THEN [Duration]  
            END ASC,  
            CASE WHEN (@SortColumn = 'Duration' AND @SortOrder='DESC')  
                        THEN [Duration]  
            END DESC,
			   
            CASE WHEN (@SortColumn = 'CreatedOn' AND @SortOrder='ASC')  
                        THEN [CreatedOn]  
            END ASC,  
            CASE WHEN (@SortColumn = 'CreatedOn' AND @SortOrder='DESC')  
                        THEN [CreatedOn]  
            END DESC
           
            OFFSET @PageSize * (@PageNo - 1) ROWS  
            FETCH NEXT @PageSize ROWS ONLY  
    ),  
    CTE_TotalRows AS   
    (  
        select count(bh.Id)  as TotalRecords from CTE_Results BH
       WHERE
			((ISNULL(@UserId,0)=0)
				OR (bh.UserId = @UserId))
				AND
			((ISNULL(@CompanyId,0)=0)
				OR (bh.CompanyId = @CompanyId))
				AND
			(@SearchValue= '' OR  (   
              BH.SubscriptionPlanName LIKE '%' + @SearchValue + '%' OR
			  BH.Price LIKE '%' + @SearchValue + '%' OR
			  BH.[Duration] LIKE '%' + @SearchValue + '%' 
            ))  
   
    )  
    Select  TotalRecords, BH.Id, BH.UserId, BH.SubscriptionPlanName, BH.ModuleIds, BH.ModulesName, BH.[Duration]
	,BH.PlanStartDate,BH.IsActive, BH.PlanEndDate, BH.CreatedOn, BH.Iscombo , BH.Price from CTE_Results BH
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = BH.ID)  
   
END

/****** Object:  StoredProcedure [dbo].[GetAircraftModelsList]    Script Date: 08-07-2022 10:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAircraftModelsList]  
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
        SELECT * from AircraftModels
  
        WHERE  (@SearchValue= '' OR  (   
              [Name] LIKE '%' + @SearchValue + '%' 
            ))
  
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
        select count(ID) as TotalRecords from AircraftModels 
		
        WHERE  (@SearchValue= '' OR  (   
              [Name] LIKE '%' + @SearchValue + '%' 
            ))
    )  

    Select   * from CTE_Results 
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = ID)  
   
END

/****** Object:  StoredProcedure [dbo].[GetAircraftMakesList]    Script Date: 08-07-2022 10:54:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAircraftMakesList]  
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
        SELECT * from AircraftMakes
  
        WHERE  (@SearchValue= '' OR  (   
              [Name] LIKE '%' + @SearchValue + '%' 
            ))
  
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
        select count(ID) as TotalRecords from AircraftMakes 
		
        WHERE  (@SearchValue= '' OR  (   
              [Name] LIKE '%' + @SearchValue + '%' 
            ))
    )  

    Select   * from CTE_Results 
	, CTE_TotalRows   
    WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = ID)  
   
END
