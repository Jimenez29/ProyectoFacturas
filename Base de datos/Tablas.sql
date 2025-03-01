
CREATE DATABASE SistemaFacturas;

USE SistemaFacturas;

-- Tabla Clientes
CREATE TABLE Clientes (
    ID_Cliente INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) UNIQUE NOT NULL,
    Telefono NVARCHAR(20) NOT NULL
);
GO

-- Tabla Facturas
CREATE TABLE Facturas (
    ID_Factura INT IDENTITY(1,1) PRIMARY KEY,
    ClienteID INT NOT NULL,
    NumeroFacturaElectronica NVARCHAR(50) UNIQUE NOT NULL,
    FechaEmision DATE NOT NULL,
    MontoTotal DECIMAL(10,2) NOT NULL,
    Estado NVARCHAR(10) DEFAULT 'Pendiente' CHECK (Estado IN ('Pendiente', 'Pagada', 'Cancelada')),
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ID_Cliente) ON DELETE CASCADE
);
GO

INSERT INTO Clientes (Nombre, Correo, Telefono)
VALUES ('Juan Pérez', 'juan.perez@email.com', '8888-5555');

INSERT INTO Facturas (ClienteID, NumeroFacturaElectronica, FechaEmision, MontoTotal, Estado)
VALUES (1, 'FAC-20240228-001', '2024-02-28', 50000.00, 'Pendiente');

Select * from facturas

Select * from clientes

select correo from clientes where nombre='Juan Pérez'