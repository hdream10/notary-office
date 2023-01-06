USE [CursBD]
GO

SELECT Deal.Deal_Code, Structure_Deal.Deal_Code, Customers.Name, Deal.Descriptions, Customers.Name,
	   SUM(Customer_Services.Cost) as SUMMA,
	   SUM(Customer_Services.Cost+Customer_Services.Commission) as SUMMA_WITH_COMMISIONS,
	   SUM((Customer_Services.Cost+Customer_Services.Commission)-((Customer_Services.Cost+Customer_Services.Commission)*Customer_Services.Discount/100)) as ITOGO_POSLE_SKIDOK
FROM Structure_Deal, Customer_Services, Deal, Customers
WHERE Structure_Deal.Service_Code = Customer_Services.Service_Code
	  and (Deal.Deal_Code = Structure_Deal.Deal_Code)
	  and Deal.Client_Code = Customers.Client_Code
GROUP BY Deal.Deal_Code, Customers.Client_Code, Structure_Deal.Deal_Code, Deal.Descriptions, Customers.Name




GO


