CREATE TABLE dbo.ReviewStatus (
	Id int NOT NULL,
	[Name] varchar(8)

	CONSTRAINT PK_ReviewStatus PRIMARY KEY CLUSTERED (Id ASC)
);

INSERT INTO dbo.ReviewStatus(Id, [Name])
VALUES (1, 'Pending'),
		(2, 'Accepted'),
		(3, 'Rejected');

-- should be safe to go directly to NOT NULL since the prior changes haven't been merged yet
ALTER TABLE dbo.Review ADD ReviewStatusId int NOT NULL;
ALTER TABLE dbo.Review ADD CONSTRAINT FK_Review_ReviewStatus FOREIGN KEY (ReviewStatusId) REFERENCES dbo.ReviewStatus(Id);

ALTER TABLE dbo.Review ADD EntryDate DateTime2(3) NOT NULL;
ALTER TABLE dbo.Review ADD ReviewDate DateTime2(3) NULL;
