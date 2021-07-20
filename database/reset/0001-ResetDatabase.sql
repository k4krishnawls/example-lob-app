
-- Clear non-system defined data: used for test databases
--	Make sure the order is correct and ignores system-defined data

DELETE FROM dbo.Review;
DELETE FROM dbo.Invoice;
DELETE FROM dbo.OrderLine;
DELETE FROM dbo.[Order];
DELETE FROM dbo.Product;
DELETE FROM dbo.Category;
DELETE FROM dbo.CustomerAddress;
DELETE FROM dbo.Customer_CustomerGroup_Xref;
DELETE FROM dbo.Customer;
DELETE FROM dbo.CustomerGroup;
DELETE FROM dbo.[User] WHERE Id >= 0;	-- ignore the built-in system user
