CREATE DATABASE SportShop
COLLATE Cyrillic_General_100_CI_AS;
GO

USE SportShop;
GO

--drop DATABASE SportShop;

-- Товари: назва товару, вид товару (одяг, взуття, і т.д.), кількість товару в наявності, собівартість, виробник, ціна продажу
create table Products
(
	Id int identity(1,1) NOT NULL primary key,
	Name nvarchar(100) NOT NULL,
	TypeProduct nvarchar(20) NOT NULL,
	Quantity int NOT NULL,
	CostPrice int NOT NULL,
	Producer nvarchar(50),
	Price int NOT NULL
);
GO

INSERT INTO Products(Name, TypeProduct, Quantity, CostPrice, Producer, Price)
VALUES ('Рукавиці', 'Аксесуари', 5, 85, 'Туреччина', 150),
		('Окуляри', 'Аксесуари', 5, 85, 'Китай', 150),
		('Ремінь', 'Одяг', 15, 120, 'Туреччина', 250),
		('Рюкзак', 'Аксесуари', 10, 450, 'Польща', 700),
		('Кросівки Адідас', 'Взуття', 20, 800, 'Польща', 1500)
GO



select * from Products

-- Працівники: повне ім'я, дата прийняття на роботу, стать, ЗП
CREATE TABLE Employees
(
	Id INT Identity(1, 1) NOT NULL PRIMARY KEY,
	FullName NVarchar(100) NOT NULL, Check(LEN(FullName) > 0),
	HireDate Date NOT NULL,
	Gender NVarchar(1)NOT NULL,
	Salary Money NOT NULL, Check(Salary > 0)
);
Go

INSERT INTO Employees(FullName, HireDate, Gender, Salary)
VALUES ('Ярощук Іван Петрович', '2020-05-30', 'M', 8500),
('Михальчук Руслана Романівна', '2020-05-06', 'F', 8500),
('Левчук Тетяна Степанівна', '2020-05-07', 'F', 8500),
('Волос Ігор Іванович', '2020-05-15', 'M', 8500);
GO

-- Клієнти: повне ім'я, пошта, телефон, стать, знижка, підписка
CREATE TABLE Clients
(
	Id INT Identity(1, 1) NOT NULL PRIMARY KEY,
	FullName NVarchar(100) NOT NULL,
	Email NVarchar(100) NOT NULL,
	Phone NVarchar(15) NOT NULL,
	Gender NVarchar(1)NOT NULL,
	PercentSale INT NOT NULL CHECK(PercentSale >=0 AND PercentSale <=100) Default 0,
	Subscribe BIT Default 1
);
GO

INSERT INTO Clients(FullName, Email, Phone, Gender, PercentSale, Subscribe)
VALUES ('Петрук Степан Романович', 'ss@c.com', '0971234567', 'M', 10, 0),
('Романчук Людмила Степанівна', 'rls@rr.org', '0989876543', 'F', 15, 1)
GO


-- Продажі: ціна продажі, кількість одениць товару, товар, клієнт (який виконав покупку), працівник (який виконав продажу)
CREATE TABLE Salles
(
	Id INT Identity(1, 1) NOT NULL PRIMARY KEY,
	ProductId INT References Products(Id) NOT NULL,
	Price Money NOT NULL,
	Quantity INT NOT NULL,
	EmployeeId INT References Employees(Id) NOT NULL,
	ClientId INT References Clients(Id) NOT NULL,
);
GO

INSERT INTO Salles(ProductId, Price, Quantity, EmployeeId, ClientId)
VALUES  (1, 10000, 1, 1, 1),
		(1, 100, 1, 1, 1),
		(4, 1800, 1, 2, 2),
		(2, 10000, 3, 4, 2)
GO
insert into Products (Name, TypeProduct, Quantity, CostPrice, Producer, Price)
values 
(N'Куртка зимова', N'Одяг', 8, 1200, N'Україна', 2200),
(N'Шапка спортивна', N'Аксесуари', 12, 150, N'Китай', 300),
(N'Футболка Nike', N'Одяг', 20, 250, N'Вʼєтнам', 500),
(N'Термобілизна', N'Одяг', 10, 400, N'Польща', 750),
(N'Кросівки Puma', N'Взуття', 15, 900, N'Індія', 1600),
(N'Сандалі', N'Взуття', 10, 350, N'Китай', 700),
(N'Спортивна сумка', N'Аксесуари', 6, 600, N'Туреччина', 950),
(N'Шкарпетки', N'Одяг', 50, 30, N'Україна', 80),
(N'Кепка', N'Аксесуари', 25, 100, N'Китай', 200),
(N'Куртка вітровка', N'Одяг', 7, 800, N'Польща', 1400);
GO

insert into Clients (FullName, Email, Phone, Gender, PercentSale, Subscribe)
values 
(N'Коваль Олександр Ігорович', 'koval@ukr.net', '0671112233', 'M', 5, 1),
(N'Гуменюк Наталія Сергіївна', 'natali.g@ukr.net', '0632223344', 'F', 10, 1),
(N'Іваненко Андрій Миколайович', 'andriy.i@gmail.com', '0503334455', 'M', 0, 0),
(N'Савчук Ольга Василівна', 'olga.s@meta.ua', '0974445566', 'F', 20, 1),
(N'Бондаренко Тарас Олексійович', 'taras.b@ukr.net', '0935556677', 'M', 15, 0),
(N'Захарченко Ірина Володимирівна', 'irina.z@ukr.net', '0666667788', 'F', 5, 1),
(N'Мельник Віталій Степанович', 'vitaliy.m@ukr.net', '0687778899', 'M', 10, 1),
(N'Шевчук Марія Анатоліївна', 'maria.s@ukr.net', '0998889900', 'F', 0, 0),
(N'Ткаченко Дмитро Юрійович', 'dmytro.t@ukr.net', '0969990011', 'M', 25, 1),
(N'Остапчук Оксана Іванівна', 'oksana.o@ukr.net', '0950001122', 'F', 30, 1);
go

insert into Salles (ProductId, Price, Quantity, EmployeeId, ClientId)
values 
(3, 250, 2, 1, 3),
(5, 1500, 1, 2, 4),
(2, 150, 1, 3, 5),
(4, 700, 1, 4, 6),
(1, 150, 3, 1, 7),
(3, 250, 1, 2, 8),
(5, 1500, 2, 3, 9),
(2, 150, 2, 4, 10),
(4, 700, 1, 1, 2),
(1, 150, 1, 2, 5);
go

ALTER TABLE Salles
ADD SaleDate DATE;

UPDATE Salles SET SaleDate = GETDATE();



-- вивід значень з таблиць
SELECT * FROM Products;
SELECT * FROM Employees;
SELECT * FROM Clients;
SELECT * FROM Salles;


