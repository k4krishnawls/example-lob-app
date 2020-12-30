
DECLARE @Admin int = -1;

UPDATE dbo.[User]
SET CreatedBy = @Admin,
	UpdatedBy = @Admin,
	UpdatedOn = CreatedOn;

ALTER TABLE dbo.[User] ALTER COLUMN CreatedBy int NOT NULL;
ALTER TABLE dbo.[User] ALTER COLUMN UpdatedBy int NOT NULL;
ALTER TABLE dbo.[User] ALTER COLUMN UpdatedOn DateTime2(3) NOT NULL;
