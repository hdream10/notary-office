use CursBD

GO
IF OBJECT_ID(N'dbo.Customers', N'U') IS NULL
 BEGIN
create table dbo.Customers(
Client_Code INT IDENTITY(1,1) PRIMARY KEY,
Name VARCHAR(100) NOT NULL,
Addres VARCHAR(50) NULL,
Phone VARCHAR(16) NOT NULL,
Type_of_activity VARCHAR(50) NULL
)
END
ELSE
 PRINT 'Таблица Customers уже существует!'


IF OBJECT_ID(N'dbo.Deal', N'U') IS NULL
 BEGIN
create table dbo.Deal(
Deal_Code INT IDENTITY(1,1) PRIMARY KEY,
Client_Code INT FOREIGN KEY REFERENCES Customers(Client_Code) NOT NULL,
Descriptions VARCHAR(100) NULL
)
END
ELSE
 PRINT 'Таблица Deal уже существует!'

IF OBJECT_ID(N'dbo.Customer_Services', N'U') IS NULL
BEGIN
create table dbo.Customer_Services(
Service_Code INT IDENTITY(1,1) PRIMARY KEY,
Name VARCHAR(100) NOT NULL,
Commission DECIMAL(5,2) NOT NULL,
Discount DECIMAL(5,2) NULL,
Descriptions VARCHAR(100) NULL,
Cost MONEY NOT NULL
)
END
ELSE
 PRINT 'Таблица Customer_Services уже существует!'

IF OBJECT_ID(N'dbo.Structure_Deal', N'U') IS NULL
 BEGIN
create table dbo.Structure_Deal(
Structure_Deal_Code INT IDENTITY(1,1) PRIMARY KEY,
Deal_Code INT FOREIGN KEY REFERENCES Deal(Deal_Code) NOT NULL,
Service_Code INT FOREIGN KEY REFERENCES Customer_Services(Service_Code) NOT NULL,
Descriptions VARCHAR(100) NULL
)
END
ELSE
 PRINT 'Таблица Structure_Deal уже существует!'
 
 
 
IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL
 BEGIN
create table dbo.Roles(
Role_Code INT IDENTITY(1,1) PRIMARY KEY,
Name VARCHAR(50) NOT NULL,
Role VARCHAR(50) NOT NULL
)
END
ELSE
 PRINT 'Таблица Roles уже существует!'

IF OBJECT_ID(N'dbo.Logins', N'U') IS NULL
 BEGIN
CREATE TABLE dbo.Logins(
Login_Code INT IDENTITY(1,1) PRIMARY KEY,
Login VARCHAR(20) NOT NULL,
Password VARCHAR(50) NOT NULL,
Role_Code INT FOREIGN KEY REFERENCES Roles(Role_Code) NOT NULL
)
END
ELSE
 PRINT 'Таблица Logins уже существует!'