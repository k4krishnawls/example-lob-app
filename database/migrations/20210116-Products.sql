

CREATE TABLE dbo.ProductType (
	Id int NOT NULL IDENTITY(1,1),
	DisplayName varchar(80) NOT NULL,
	UpdatedOn DateTime2(3) NOT NULL,
	UpdatedBy int NOT NULL,

	CONSTRAINT PK_ProductType PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT FK_ProductType_User FOREIGN KEY (UpdatedBy) REFERENCES dbo.[User](Id)
);
