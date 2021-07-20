
CREATE TABLE dbo.Category (
	Id int NOT NULL IDENTITY(1,1),
	[Name] varchar(20) NOT NULL,

	CONSTRAINT PK_Category PRIMARY KEY CLUSTERED (Id ASC)
);

-- Remove older version, replace with newer one
DROP TABLE dbo.Customer;
CREATE TABLE dbo.Customer (
	Id int NOT NULL IDENTITY(1,1),
	-- not separating name, generally not advised unless necessary. Names are hard.
	[Name] varchar(200) NOT NULL,
	Email varchar(200) NOT NULL,
	-- not storing address on customer record, addresses also are not easy or 1-1 w/ customer
	Avatar varchar(200) NULL,
	-- not storing birthday, don't ask for PII if you don't actually need it
	NewsletterOptIn DateTime2(0) NULL,

	-- not adding fields for auth: the source system handled this poorly (enterable password)
	-- and it needs to be a separate optional record since not all customers sign up for perm. accounts
	-- this is the backend system, the customer wouldn't be logging into this anyway

	CONSTRAINT PK_Customer PRIMARY KEY CLUSTERED (Id ASC)
);

-- Very simplified address record duplicated from source, don't use this in a real system unless you only ship to US/CA + a couple others
--	this will be linked to User record so they can manage some defaults, but copied to order records at time of order (cheap and dirty solution)
CREATE TABLE dbo.CustomerAddress (
	Id int NOT NULL IDENTITY(1,1),
	CustomerId int NOT NULL,
	AddressLine1 varchar(200) NOT NULL,
	AddressLine2 varchar(200) NULL,
	City varchar(200) NOT NULL,
	StateAbbr char(2) NOT NULL,
	ZipCode varchar(10) NOT NULL,
	-- no country, so must be US only?
	ArchivedOn datetime2(3) NULL,
	
	CONSTRAINT PK_CustomerAddress PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT FK_CustomerAddress_Customer FOREIGN KEY (CustomerId) REFERENCES dbo.Customer(Id)
);

CREATE TABLE dbo.CustomerGroup (
	Id int NOT NULL IDENTITY(1,1),
	[Name] varchar(30) NOT NULL,

	CONSTRAINT PK_CustomerGroup PRIMARY KEY CLUSTERED (Id ASC)
);

CREATE TABLE dbo.Customer_CustomerGroup_Xref (
	CustomerId int NOT NULL,
	CustomerGroupId int NOT NULL,

	CONSTRAINT FK_CustomerCustomerGroupXref_Customer FOREIGN KEY (CustomerId) REFERENCES dbo.Customer(Id),
	CONSTRAINT FK_CustomerCustomerGroupXref_CustomerGroup FOREIGN KEY (CustomerGroupId) REFERENCES dbo.CustomerGroup(Id)
)

CREATE TABLE dbo.Product (
	Id int NOT NULL IDENTITY(1,1),
	CategoryId int NOT NULL,
	[Name] varchar(200) NOT NULL,
	Width decimal(18,3) NOT NULL,
	Height decimal(18,3) NOT NULL,
	Price decimal(18,2) NOT NULL,
	Thumbnail varchar(200) NOT NULL,
	[Image] varchar(200) NOT NULL,
	[Description] varchar(MAX) NOT NULL,
	-- this is questionable, would not combine product and inventory in a real system
	[Stock] int NOT NULL,
	-- not including "Sales" field, that's a projected value or something that should be aggregated from Orders
	
	CONSTRAINT PK_Product PRIMARY KEY CLUSTERED (Id ASC),
);

CREATE TABLE dbo.OrderStatus (
	Id int NOT NULL,
	[Name] varchar(10) NOT NULL,
	
	CONSTRAINT PK_OrderStatus PRIMARY KEY CLUSTERED (Id ASC),
);

INSERT INTO dbo.OrderStatus(Id, [Name])
VALUES  (1, 'Ordered'),
		(2, 'Processing'),
		(3, 'Delivering'),
		(4, 'Delivered'),
		(5, 'Cancelled');

CREATE TABLE dbo.[Order] (
	Id int NOT NULL IDENTITY(1,1),
	Reference varchar(6) NOT NULL,
	OrderDate DateTime2(3) NOT NULL,
	CustomerId int NOT NULL,
	SubTotal decimal(18,2) NOT NULL,
	DeliveryFeeTotal decimal(18,2) NOT NULL,
	-- yeeeaahhh...sales tax is a bit more complicated than this, plus customer orders <> invoices
	TaxRate decimal(10,4) NOT NULL,
	SalesTax decimal(18,2) NOT NULL,
	Total decimal(18,2) NOT NULL,
	-- projected values for sure
	OrderStatusId int NOT NULL,
	Returned bit NOT NULL,
	-- actual shipping address
	ShipToName varchar(200) NOT NULL,
	AddressLine1 varchar(200) NOT NULL,
	AddressLine2 varchar(200) NULL,
	City varchar(200) NOT NULL,
	StateAbbr char(2) NOT NULL,
	ZipCode varchar(10) NOT NULL,
	
	CONSTRAINT PK_Order PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT FK_Order_Customer FOREIGN KEY (CustomerId) REFERENCES dbo.Customer(Id),
	CONSTRAINT FK_Order_OrderStatus FOREIGN KEY (OrderStatusId) REFERENCES dbo.OrderStatus(Id)
);

CREATE TABLE dbo.OrderLine (
	Id int NOT NULL IDENTITY(1,1),
	OrderId int NOT NULL,
	ProductId int NOT NULL,
	[Quantity] int NOT NULL,
	-- note that we don't have versioning on the products and haven't captured the price at time of purchase...?
	
	CONSTRAINT PK_OrderLine PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT FK_OrderLine_Order FOREIGN KEY (OrderId) REFERENCES dbo.[Order](Id),
	CONSTRAINT FK_OrderLine_Product FOREIGN KEY (ProductId) REFERENCES dbo.Product(Id)
);

CREATE TABLE dbo.Invoice (
	Id int NOT NULL IDENTITY(1,1),
	InvoiceDate DateTime2(3) NOT NULL,
	OrderId int NOT NULL,
	-- dupe?
	CustomerId int NOT NULL,
	-- same fields and potential issues as order
	SubTotal decimal(18,2) NOT NULL,
	DeliveryFeeTotal decimal(18,2) NOT NULL,
	TaxRate decimal(10,4) NOT NULL,
	SalesTax decimal(18,2) NOT NULL,
	Total decimal(18,2) NOT NULL,

	CONSTRAINT PK_Invoice PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT FK_Invoice_Order FOREIGN KEY (OrderId) REFERENCES dbo.[Order](Id),
	CONSTRAINT FK_Invoice_Customer FOREIGN KEY (CustomerId) REFERENCES dbo.Customer(Id),
);

CREATE TABLE dbo.Review (
	Id int NOT NULL IDENTITY(1,1),
	CustomerId int NOT NULL,
	ProductId int NOT NULL,
	OrderId int NOT NULL,
	Rating smallint NOT NULL,
	Comment varchar(MAX) NOT NULL,
	
	CONSTRAINT PK_Review PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT FK_Review_Customer FOREIGN KEY (CustomerId) REFERENCES dbo.Customer(Id),
	CONSTRAINT FK_Review_Product FOREIGN KEY (ProductId) REFERENCES dbo.Product(Id),
	CONSTRAINT FK_Review_Order FOREIGN KEY (OrderId) REFERENCES dbo.[Order](Id)
);
