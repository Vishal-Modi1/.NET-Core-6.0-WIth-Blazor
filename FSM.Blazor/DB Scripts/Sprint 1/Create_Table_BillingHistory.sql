/****** Object:  Table [dbo].[BillingHistories]    Script Date: 31-03-2022 17:07:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BillingHistories](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[SubscriptionPlanName] [varchar](50) NOT NULL,
	[ModuleIds] [varchar](50) NOT NULL,
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

ALTER TABLE [dbo].[BillingHistories]  WITH CHECK ADD  CONSTRAINT [FK_BillingHistories_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[BillingHistories] CHECK CONSTRAINT [FK_BillingHistories_Users]
GO


