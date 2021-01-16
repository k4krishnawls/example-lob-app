
-- Clear non-system defined data: used for test databases
--	Make sure the order is correct and ignores system-defined data

DELETE FROM dbo.ProductType;
DELETE FROM dbo.Customer;
DELETE FROM dbo.[User] WHERE Id >= 0;	-- ignore the built-in system user
