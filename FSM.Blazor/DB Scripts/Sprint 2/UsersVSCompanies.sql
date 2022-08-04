/****** Object:  Table [dbo].[UsersVSCompanies]    Script Date: 23-07-2022 17:21:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UsersVSCompanies](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[CompanyId] [int] NULL,
	[RoleId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [bigint] NULL,
	[DeletedOn] [datetime] NULL,
 CONSTRAINT [PK_UsersVSCompanies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UsersVSCompanies] ADD  CONSTRAINT [DF_UsersVSCompanies_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO

ALTER TABLE [dbo].[UsersVSCompanies] ADD  CONSTRAINT [DF_UsersVSCompanies_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[UsersVSCompanies]  WITH CHECK ADD  CONSTRAINT [FK_UsersVSCompanies_Companies] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO

ALTER TABLE [dbo].[UsersVSCompanies] CHECK CONSTRAINT [FK_UsersVSCompanies_Companies]
GO

ALTER TABLE [dbo].[UsersVSCompanies]  WITH CHECK ADD  CONSTRAINT [FK_UsersVSCompanies_UserRoles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[UserRoles] ([Id])
GO

ALTER TABLE [dbo].[UsersVSCompanies] CHECK CONSTRAINT [FK_UsersVSCompanies_UserRoles]
GO

ALTER TABLE [dbo].[UsersVSCompanies]  WITH CHECK ADD  CONSTRAINT [FK_UsersVSCompanies_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[UsersVSCompanies] CHECK CONSTRAINT [FK_UsersVSCompanies_Users]
GO


