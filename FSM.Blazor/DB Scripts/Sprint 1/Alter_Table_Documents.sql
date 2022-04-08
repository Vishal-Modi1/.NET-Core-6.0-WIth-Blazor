ALTER TABLE Documents 
Add TotalDownloads bigint, TotalShares bigint,
LastShareDate datetime, IsShareable BIT NOT NULL DEFAULT 1