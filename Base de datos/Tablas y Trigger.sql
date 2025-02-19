CREATE DATABASE IF NOT EXISTS SistemaFacturas;
USE SistemaFacturas;

-- Tabla Clientes
CREATE TABLE Clientes (
    ID_Cliente INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Correo VARCHAR(100) UNIQUE NOT NULL,
    Telefono VARCHAR(20) NOT NULL
);

-- Tabla Facturas
CREATE TABLE Facturas (
    ID_Factura INT AUTO_INCREMENT PRIMARY KEY,
    ClienteID INT NOT NULL,
    NumeroFacturaElectronica VARCHAR(50) UNIQUE NOT NULL,
    FechaEmision DATE NOT NULL,
    MontoTotal DECIMAL(10,2) NOT NULL,
    MontoFinal DECIMAL(10,2) NOT NULL,  
    Estado ENUM('Pendiente', 'Pagada', 'Cancelada') DEFAULT 'Pendiente',
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ID_Cliente) ON DELETE CASCADE
);

-- Tabla Notas de Credito
CREATE TABLE NotasCredito (
    ID_NotaCredito INT AUTO_INCREMENT PRIMARY KEY,
    FacturaID INT NOT NULL,
    MontoAplicado DECIMAL(10,2) NOT NULL,
    FechaEmision DATE NOT NULL,
    Descripcion TEXT NULL,
    FOREIGN KEY (FacturaID) REFERENCES Facturas(ID_Factura) ON DELETE CASCADE
);

-- Trigger para actualizar el monto cuando se ingrese una nota de credito en una factura en especifico
DELIMITER //

CREATE TRIGGER actualizar_monto_final
AFTER INSERT ON NotasCredito
FOR EACH ROW
BEGIN
    UPDATE Facturas
    SET MontoFinal = MontoTotal - (SELECT IFNULL(SUM(MontoAplicado), 0) 
                                   FROM NotasCredito 
                                   WHERE FacturaID = NEW.FacturaID)
    WHERE ID_Factura = NEW.FacturaID;
END;

//
--Hola
