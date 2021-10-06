CREATE DATABASE Almacen
GO

USE Almacen
GO

CREATE TABLE Usuarios
(
	Id INT PRIMARY KEY IDENTITY,
	Nombre VARCHAR(100) NOT NULL,
	Usuario VARCHAR(100) NOT NULL,
	Email VARCHAR(100) NOT NULL,
	Contraseņa VARCHAR(100) NOT NULL,
	FechaBaja DATETIME 
)
GO

CREATE TABLE Proveedores
(
	Id INT PRIMARY KEY IDENTITY,
	Nombre VARCHAR(100) NOT NULL,
	CUIL BIGINT NOT NULL,
	Direccion VARCHAR(100) NOT NULL,
	Telefono BIGINT NOT NULL,
	FechaBaja DATETIME 
)
GO

CREATE TABLE Clientes
(
	Id INT PRIMARY KEY IDENTITY,
	Nombre VARCHAR(100) NOT NULL,
	DNI BIGINT NOT NULL,
	Direccion VARCHAR(100) NOT NULL,
	Telefono VARCHAR(100) NOT NULL,
	FechaBaja DATETIME 
)
GO

CREATE TABLE Ventas
(
	Id INT PRIMARY KEY IDENTITY,
	Fecha DATETIME NOT NULL,
	Cliente_Id INT NOT NULL,
	Empleado_Id INT NOT NULL,
	Total FLOAT NOT NULL,
	Saldo FLOAT,
	FechaBaja DATETIME,
	FOREIGN KEY (Cliente_Id) REFERENCES Clientes(Id),
	FOREIGN KEY (Empleado_Id) REFERENCES Usuarios(Id)
)
GO


CREATE TABLE Articulos
(
	Id INT PRIMARY KEY IDENTITY,
	Nombre VARCHAR(100) NOT NULL,
	Codigo_Art BIGINT NOT NULL,
	Precio_Unit FLOAT NOT NULL,
	Precio_Mayor FLOAT NOT NULL,
	Stock_Act INT NOT NULL,
	Ultima_Modif DATETIME NOT NULL,
	FechaBaja DATETIME 
)
GO

CREATE TABLE DetalleVentas
(
	Id INT PRIMARY KEY IDENTITY, 
	Articulo_Id INT NOT NULL,
	Precio INT NOT NULL,
	Cantidad INT NOT NULL,
	Venta_Id INT NOT NULL,
	SubTotal FLOAT,
	FOREIGN KEY (Venta_Id) REFERENCES Ventas(Id),
	FOREIGN KEY (Articulo_Id) REFERENCES Articulos(Id)
)
GO

CREATE TABLE Compras
(
	Id INT PRIMARY KEY IDENTITY,
	Fecha DATETIME NOT NULL,
	Proveedor_Id INT NOT NULL,
	NroRecibo BIGINT NOT NULL,
	Empleado_Id INT NOT NULL,
	Total FLOAT NOT NULL,
	FechaBaja DATETIME,
	FOREIGN KEY (Proveedor_Id) REFERENCES Proveedores(Id),
	FOREIGN KEY (Empleado_Id) REFERENCES Usuarios(Id)
)
GO

CREATE TABLE DetalleCompras
(
	Id INT PRIMARY KEY IDENTITY,
	Articulo_Id INT NOT NULL,
	Cantidad INT NOT NULL,
	Precio_Mayor FLOAT NOT NULL,
	Precio_Unit FLOAT NOT NULL,
	SubTotal FLOAT,
	Compra_Id INT NOT NULL,
	FOREIGN KEY (Compra_Id) REFERENCES Compras(Id),
	FOREIGN KEY (Articulo_Id) REFERENCES Articulos(Id)
)
GO

CREATE TABLE Caja
(
	Id INT PRIMARY KEY IDENTITY,
	Fecha DATETIME NOT NULL,
	Empleado_Id INT NOT NULL,
	Apertura FLOAT NOT NULL,
	Cierre FLOAT NOT NULL,
	FechaBaja DATETIME,
	FOREIGN KEY (Empleado_Id) REFERENCES Usuarios(Id)
)
GO

CREATE TABLE MovimientosCaja
(
	Id INT PRIMARY KEY IDENTITY,
	Caja_Id INT NOT NULL,
	Descripcion VARCHAR(100) NULL,
	Ingreso FLOAT NOT NULL,
	Egreso FLOAT NOT NULL,
	Total FLOAT NOT NULL,
	FechaBaja DATETIME NULL,
	Venta_Id INT NULL, 
	Compra_Id INT NULL,
	FOREIGN KEY (Venta_Id) REFERENCES Ventas(Id),
	FOREIGN KEY (Compra_Id) REFERENCES Compras(Id),
	FOREIGN KEY (Caja_Id) REFERENCES Caja(Id)
)
GO

CREATE TABLE FormasPago
(
	Id INT PRIMARY KEY IDENTITY,
	Descripcion VARCHAR(100) NULL,
)
GO


CREATE TABLE FormasPagoVentas
(
	Id INT PRIMARY KEY IDENTITY,
	Venta_Id INT NOT NULL, 
	FormaPago_Id INT NOT NULL,
	Importe FLOAT NOT NULL,
	Fecha DATETIME NOT NULL
)
GO

INSERT INTO FormasPago VALUES('Efectivo')
GO
INSERT INTO FormasPago VALUES('Tarjeta Debito')
GO
INSERT INTO FormasPago VALUES('Tarjeta Credito')
GO