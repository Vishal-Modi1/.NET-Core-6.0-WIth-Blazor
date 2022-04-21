
/****** Object:  Table [dbo].[DocumentTags]    Script Date: 22-03-2022 10:20:33 ******/
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


