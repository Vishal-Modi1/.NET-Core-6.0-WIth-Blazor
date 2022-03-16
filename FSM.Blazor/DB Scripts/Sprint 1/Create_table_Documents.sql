
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
	[Size(InKB)] [bigint] NULL,
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


