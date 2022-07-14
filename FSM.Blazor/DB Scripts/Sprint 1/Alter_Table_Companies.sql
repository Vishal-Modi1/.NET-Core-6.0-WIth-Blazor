ALTER TABLE Companies ADD Website VARCHAR(50),
PrimaryAirport VARCHAR(200), 
PrimaryServiceId smallint,[Logo] [varchar](200) NULL,
FOREIGN KEY(PrimaryServiceId) REFERENCES CompanyServices(Id);
