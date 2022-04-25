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

INSERT INTO CompanyServices VALUES('Aircraft Maintenance')
INSERT INTO CompanyServices VALUES('Aircraft Management')
INSERT INTO CompanyServices VALUES('Aircraft Partnership')
INSERT INTO CompanyServices VALUES('Aircraft Sales')
INSERT INTO CompanyServices VALUES('Charter')
INSERT INTO CompanyServices VALUES('Corporate Flight Dept.')
INSERT INTO CompanyServices VALUES('FBO')
INSERT INTO CompanyServices VALUES('Flight Training')
INSERT INTO CompanyServices VALUES('Military')
INSERT INTO CompanyServices VALUES('Photography/Surveying')
INSERT INTO CompanyServices VALUES('Tours')
INSERT INTO CompanyServices VALUES('Other')




