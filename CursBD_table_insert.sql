USE CursBD

GO
INSERT INTO Customers(Name, Addres, Phone, Type_of_activity)
	VALUES
		('ISpring','Yoshkar-Ola','+79234356523','Online curs'),
		('Yandex','Moscow','+79831235623','Internet service'),
		('Google','Boston','+79234323433','Internet service'),
		('Gazprom','Kazan','+79234359173','Gaz'),
		('Travel-Line','Yoshkar-Ola','+79235126523','Travel prog')

INSERT INTO Deal(Client_Code, Descriptions)
	VALUES 
		(1,'Descriptions'),
		(2,'Descriptions'),
		(3,'Descriptions'),
		(4,'Descriptions'),
		(5,'Descriptions')
		
INSERT INTO Customer_Services(Name, Commission,Discount,Descriptions,Cost)
	VALUES
		('Pen',12.4,5,'Description',1300),
		('item1',76.1,1,'Description',21222),
		('item2',48.1,2,'Description',41232),
		('item3',11.9,4,'Description',12351),
		('item4',15.3,5,'Description',1252423)

INSERT INTO Structure_Deal(Deal_Code,Service_Code,Descriptions)
	VALUES 
		(2,3,'descrip'),
		(3,5,'descrip'),
		(4,1,'descrip'),
		(2,3,'descrip'),
		(5,2,'descrip')

INSERT INTO Roles(Name,Role)
	VALUES 
		('Admin', 'w,w,w'),
		('ReadOnly', 'r,r,r'),
		('ClientAdder', 'r,n,n'),
		('ClientServiceAdder', 'n,r,n'),
		('None', 'n,n,n')

INSERT INTO Logins(Login,Password,Role_Code)
	VALUES 
		('admin', '12345', 1),
		('reader', '12345', 2),
		('client_worker', '12345', 3),
		('service_worker', '12345', 4),
		('nobody', '12345', 5)