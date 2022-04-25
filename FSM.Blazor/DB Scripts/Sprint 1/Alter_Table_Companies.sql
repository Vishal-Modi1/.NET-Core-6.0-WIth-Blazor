ALTER TABLE Companies ADD Website VARCHAR(50),
PrimaryAirport VARCHAR(200), 
PrimaryServiceId smallint,
FOREIGN KEY(PrimaryServiceId) REFERENCES CompanyServices(Id);