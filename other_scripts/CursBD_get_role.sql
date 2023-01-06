USE [CursBD]
GO

SELECT Name, Role FROM Roles
WHERE Roles.Role_Code IN 
(
    SELECT Role_Code
	FROM Logins
	/*WHERE Login = 'admin' and Password = '12345'*/
);


