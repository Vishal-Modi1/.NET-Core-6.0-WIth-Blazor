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


INSERT INTO Timezones (Timezone, Offset)
SELECT NAME,CURRENT_UTC_OFFSET FROM SYS.TIME_ZONE_INFO
GO