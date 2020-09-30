CREATE TABLE UserType (
	Id int NOT NULL,
	[Name] varchar(40) NOT NULL,

	CONSTRAINT PK_UserType PRIMARY KEY CLUSTERED (Id ASC)
);

CREATE TABLE dbo.[User] (
	Id int NOT NULL IDENTITY(1,1),
	Username varchar(80) NOT NULL,
	PasswordHash varchar(80) NULL,
	[Name] varchar(80) NOT NULL,
	UserTypeId int NOT NULL,
	CreatedOn DateTime2(3) NOT NULL,

	CONSTRAINT PK_User PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT FK_User_UserType FOREIGN KEY (UserTypeId) REFERENCES dbo.UserType(Id)
);

CREATE TABLE dbo.UserSession (
	Id int NOT NULL IDENTITY(1,1),
	UserId int NOT NULL,
	CreatedOn DateTime2(3) NOT NULL,
	LastSeenOn DateTime2(3) NOT NULL,
	AbsoluteExpirationDate DateTime2(3) NOT NULL,
	IsForcedExpiration bit NOT NULL,

	CONSTRAINT PK_UserSession PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT FK_UserSession_User FOREIGN KEY (UserId) REFERENCES dbo.[User](Id)
);

-- System Data

INSERT INTO dbo.UserType(Id, [Name]) 
VALUES (1, 'Interactive User'),
	(2, 'System User');

SET IDENTITY_INSERT dbo.[User] ON
INSERT INTO dbo.[User] (Id, Username, CreatedOn, UserTypeId, [Name])
VALUES(-1, 'System', GetUtcDate(), 2, 'System');
SET IDENTITY_INSERT dbo.[User] OFF
