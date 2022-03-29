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


