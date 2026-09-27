/*
    Migracion de Loguin.accdb -> SQL Server
    Rama: migracion-sqlserver

    Esta primera seccion reproduce las tablas de datos detectadas en el
    archivo Access entregado, sin depender de OleDb.

    IMPORTANTE:
    - La estructura del ACCDB no contiene las tablas Menu, Pedido, Producto
      ni UsuariosGrupos que el codigo actual tambien utiliza.
    - Por eso esas tablas NO se inventan en esta migracion fiel.
    - La carga de datos fila por fila requiere una exportacion de Access o
      una herramienta Jet/ACE. El entorno de desarrollo actual no dispone
      del proveedor ACE/OleDb necesario para leer los registros directamente.
*/

IF DB_ID(N'SistemaLoguin') IS NULL
    CREATE DATABASE SistemaLoguin;
GO

USE SistemaLoguin;
GO

/* ===== Tablas base ===== */

IF OBJECT_ID(N'dbo.Provincias', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Provincias
    (
        IdProvincia INT NOT NULL,
        Provincia NVARCHAR(255) NULL,
        CONSTRAINT PK_Provincias PRIMARY KEY (IdProvincia)
    );
END
GO

IF OBJECT_ID(N'dbo.Partidos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Partidos
    (
        IdPartido INT NOT NULL,
        Partido NVARCHAR(255) NULL,
        IdProvincia INT NULL,
        CONSTRAINT PK_Partidos PRIMARY KEY (IdPartido)
    );
END
GO

IF OBJECT_ID(N'dbo.Localidades', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Localidades
    (
        idLocalidad INT NOT NULL,
        Localidades NVARCHAR(255) NULL,
        idPartido INT NULL,
        CONSTRAINT PK_Localidades PRIMARY KEY (idLocalidad)
    );
END
GO

IF OBJECT_ID(N'dbo.TipoDoc', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TipoDoc
    (
        Id INT NOT NULL,
        Tipo NVARCHAR(100) NULL,
        CONSTRAINT PK_TipoDoc PRIMARY KEY (Id)
    );
END
GO

IF OBJECT_ID(N'dbo.Cargos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Cargos
    (
        IdCargo INT NOT NULL,
        Cargo NVARCHAR(255) NULL,
        IdGerencia INT NULL,
        CONSTRAINT PK_Cargos PRIMARY KEY (IdCargo)
    );
END
GO

IF OBJECT_ID(N'dbo.Grupos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Grupos
    (
        IdGrupo INT NOT NULL,
        Grupo NVARCHAR(255) NULL,
        CONSTRAINT PK_Grupos PRIMARY KEY (IdGrupo)
    );
END
GO

IF OBJECT_ID(N'dbo.Permisos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Permisos
    (
        IdPermiso INT NOT NULL,
        Funcionalidad NVARCHAR(255) NULL,
        CONSTRAINT PK_Permisos PRIMARY KEY (IdPermiso)
    );
END
GO

IF OBJECT_ID(N'dbo.Personal', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Personal
    (
        IdPersona INT NOT NULL,
        Apellido NVARCHAR(255) NULL,
        Nombres NVARCHAR(255) NULL,
        IdTDoc INT NULL,
        NroDoc INT NULL,
        CuitCuil NVARCHAR(50) NULL,
        Telefono NVARCHAR(100) NULL,
        Correo NVARCHAR(255) NULL,
        Calle NVARCHAR(255) NULL,
        Piso NVARCHAR(50) NULL,
        IdLocalidad INT NULL,
        IdCargo INT NULL,
        CONSTRAINT PK_Personal PRIMARY KEY (IdPersona)
    );
END
GO

IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios
    (
        IdUsuario INT NOT NULL,
        [Usuario] NVARCHAR(100) NULL,
        [Password] NVARCHAR(255) NULL,
        IdPersona INT NULL,
        FechaAlta DATE NULL,
        FechaBaja DATE NULL,
        CambiaCada INT NULL,
        FechaUltimoCambio DATE NULL,
        UsuarioDesactivado BIT NULL,
        FechaDesactivacion DATE NULL,
        CONSTRAINT PK_Usuarios PRIMARY KEY (IdUsuario)
    );
END
GO

IF OBJECT_ID(N'dbo.PermisosUsuarios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PermisosUsuarios
    (
        IdUsuario INT NOT NULL,
        IdPermiso INT NOT NULL,
        FechaAlta DATE NULL,
        FechaBaja DATE NULL,
        AltaProvisoria DATE NULL,
        CONSTRAINT PK_PermisosUsuarios PRIMARY KEY (IdUsuario, IdPermiso)
    );
END
GO

IF OBJECT_ID(N'dbo.PermisosGrupos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PermisosGrupos
    (
        IdPermiso INT NOT NULL,
        IdGrupo INT NOT NULL,
        CONSTRAINT PK_PermisosGrupos PRIMARY KEY (IdPermiso, IdGrupo)
    );
END
GO

IF OBJECT_ID(N'dbo.Bitacora', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Bitacora
    (
        IdEvento INT NOT NULL,
        Fecha DATE NULL,
        Hora CHAR(5) NULL,
        IdUsuario INT NULL,
        [Usuario] NVARCHAR(100) NULL,
        Evento NVARCHAR(255) NULL,
        Detalle NVARCHAR(MAX) NULL,
        Origen NVARCHAR(255) NULL,
        CONSTRAINT PK_Bitacora PRIMARY KEY (IdEvento)
    );
END
GO

IF OBJECT_ID(N'dbo.Proveedor', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Proveedor
    (
        idProveedor INT NOT NULL,
        Nombre NVARCHAR(255) NULL,
        Direccion NVARCHAR(255) NULL,
        idLocalidad INT NULL,
        Telefono NVARCHAR(100) NULL,
        CUIT NVARCHAR(50) NULL,
        CONSTRAINT PK_Proveedor PRIMARY KEY (idProveedor)
    );
END
GO

IF OBJECT_ID(N'dbo.Stock', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Stock
    (
        idProducto INT NOT NULL,
        nombre NVARCHAR(255) NULL,
        descripcion NVARCHAR(MAX) NULL,
        stock INT NULL,
        precio DECIMAL(18,2) NULL,
        idProveedor INT NULL,
        CONSTRAINT PK_Stock PRIMARY KEY (idProducto)
    );
END
GO

/* ===== Relaciones detectadas en Access ===== */

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Partidos_Provincias')
    ALTER TABLE dbo.Partidos ADD CONSTRAINT FK_Partidos_Provincias
        FOREIGN KEY (IdProvincia) REFERENCES dbo.Provincias(IdProvincia);
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Localidades_Partidos')
    ALTER TABLE dbo.Localidades ADD CONSTRAINT FK_Localidades_Partidos
        FOREIGN KEY (idPartido) REFERENCES dbo.Partidos(IdPartido);
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Personal_TipoDoc')
    ALTER TABLE dbo.Personal ADD CONSTRAINT FK_Personal_TipoDoc
        FOREIGN KEY (IdTDoc) REFERENCES dbo.TipoDoc(Id);
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Personal_Localidades')
    ALTER TABLE dbo.Personal ADD CONSTRAINT FK_Personal_Localidades
        FOREIGN KEY (IdLocalidad) REFERENCES dbo.Localidades(idLocalidad);
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Personal_Cargos')
    ALTER TABLE dbo.Personal ADD CONSTRAINT FK_Personal_Cargos
        FOREIGN KEY (IdCargo) REFERENCES dbo.Cargos(IdCargo);
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Usuarios_Personal')
    ALTER TABLE dbo.Usuarios ADD CONSTRAINT FK_Usuarios_Personal
        FOREIGN KEY (IdPersona) REFERENCES dbo.Personal(IdPersona);
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_PermisosUsuarios_Usuarios')
    ALTER TABLE dbo.PermisosUsuarios ADD CONSTRAINT FK_PermisosUsuarios_Usuarios
        FOREIGN KEY (IdUsuario) REFERENCES dbo.Usuarios(IdUsuario);
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_PermisosUsuarios_Permisos')
    ALTER TABLE dbo.PermisosUsuarios ADD CONSTRAINT FK_PermisosUsuarios_Permisos
        FOREIGN KEY (IdPermiso) REFERENCES dbo.Permisos(IdPermiso);
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_PermisosGrupos_Permisos')
    ALTER TABLE dbo.PermisosGrupos ADD CONSTRAINT FK_PermisosGrupos_Permisos
        FOREIGN KEY (IdPermiso) REFERENCES dbo.Permisos(IdPermiso);
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_PermisosGrupos_Grupos')
    ALTER TABLE dbo.PermisosGrupos ADD CONSTRAINT FK_PermisosGrupos_Grupos
        FOREIGN KEY (IdGrupo) REFERENCES dbo.Grupos(IdGrupo);
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Bitacora_Usuarios')
    ALTER TABLE dbo.Bitacora ADD CONSTRAINT FK_Bitacora_Usuarios
        FOREIGN KEY (IdUsuario) REFERENCES dbo.Usuarios(IdUsuario);
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Proveedor_Localidades')
    ALTER TABLE dbo.Proveedor ADD CONSTRAINT FK_Proveedor_Localidades
        FOREIGN KEY (idLocalidad) REFERENCES dbo.Localidades(idLocalidad);
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Stock_Proveedor')
    ALTER TABLE dbo.Stock ADD CONSTRAINT FK_Stock_Proveedor
        FOREIGN KEY (idProveedor) REFERENCES dbo.Proveedor(idProveedor);
GO
