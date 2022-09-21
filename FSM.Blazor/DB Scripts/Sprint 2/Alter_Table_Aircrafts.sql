ALTER TABLE Aircrafts ADD AircraftStatusId tinyint not null default 1,
FOREIGN KEY(AircraftStatusId) REFERENCES AircraftStatuses(Id)
GO

UPDATE Aircrafts SET AircraftStatusId = 1
GO