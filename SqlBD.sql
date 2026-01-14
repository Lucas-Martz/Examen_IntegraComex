IF DB_ID(N'IntegraComexChallenge') IS NULL
BEGIN
    CREATE DATABASE IntegraComexChallenge;
END
GO

USE IntegraComexChallenge;
GO

IF OBJECT_ID(N'dbo.Clientes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clientes
    (
        IdCliente   INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Clientes PRIMARY KEY,
        Cuit        VARCHAR(11)       NOT NULL,
        RazonSocial VARCHAR(200)      NULL,
        Telefono    VARCHAR(20)       NULL,
        Direccion   VARCHAR(200)      NULL,
        Activo      BIT               NOT NULL CONSTRAINT DF_Clientes_Activo DEFAULT (1),
        FechaAlta   DATETIME          NOT NULL CONSTRAINT DF_Clientes_FechaAlta DEFAULT (GETDATE()),
        FechaModif  DATETIME          NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE [name] = N'UQ_Clientes_Cuit')
BEGIN
    ALTER TABLE dbo.Clientes
    ADD CONSTRAINT UQ_Clientes_Cuit UNIQUE (Cuit);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE [name] = N'CK_Clientes_Cuit_11Digitos')
BEGIN
    ALTER TABLE dbo.Clientes
    ADD CONSTRAINT CK_Clientes_Cuit_11Digitos
    CHECK (LEN(Cuit) = 11 AND Cuit NOT LIKE '%[^0-9]%');
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE [name] = N'CK_Clientes_Telefono_Numerico')
BEGIN
    ALTER TABLE dbo.Clientes
    ADD CONSTRAINT CK_Clientes_Telefono_Numerico
    CHECK (Telefono IS NULL OR Telefono = '' OR Telefono NOT LIKE '%[^0-9]%');
END
GO

IF OBJECT_ID(N'dbo.TR_Clientes_SetFechaModif', N'TR') IS NULL
BEGIN
    EXEC(N'
    CREATE TRIGGER dbo.TR_Clientes_SetFechaModif
    ON dbo.Clientes
    AFTER UPDATE
    AS
    BEGIN
        SET NOCOUNT ON;

        UPDATE c
           SET FechaModif = GETDATE()
          FROM dbo.Clientes c
          INNER JOIN inserted i ON i.IdCliente = c.IdCliente;
    END
    ');
END
GO
