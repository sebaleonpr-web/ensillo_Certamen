/* =========================================================
   CREATE DATABASE + SCHEMA (SQL Server)
   Proyecto: boletinLayon
   ========================================================= */

-- 1) Crea la base si no existe
IF DB_ID(N'boletinLayon') IS NULL
BEGIN
    CREATE DATABASE [boletinLayon];
END
GO

USE [boletinLayon];
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET XACT_ABORT ON;
GO

BEGIN TRANSACTION;

-- 2) Tabla de historial de migraciones (EF)
IF OBJECT_ID(N'[dbo].[__EFMigrationsHistory]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[__EFMigrationsHistory] (
        [MigrationId]    NVARCHAR(150) NOT NULL,
        [ProductVersion] NVARCHAR(32)  NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

-- 3) Tablas (orden de dependencias)

-- rolGeneral
IF OBJECT_ID(N'[dbo].[rolGeneral]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[rolGeneral] (
        [GuidRol]    UNIQUEIDENTIFIER NOT NULL DEFAULT (newsequentialid()),
        [nombreRol]  VARCHAR(50)  NOT NULL,
        [descripRol] VARCHAR(200) NULL,
        CONSTRAINT [PK_rolGeneral] PRIMARY KEY ([GuidRol])
    );
END;

-- usuariosGeneral (GuidRol NULL)
IF OBJECT_ID(N'[dbo].[usuariosGeneral]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[usuariosGeneral] (
        [GuidUsuario]  UNIQUEIDENTIFIER NOT NULL DEFAULT (newsequentialid()),
        [contraUser]   VARCHAR(100) NOT NULL,
        [nombreUser]   VARCHAR(50)  NOT NULL,
        [apellidoUser] VARCHAR(50)  NOT NULL,
        [emailUser]    VARCHAR(100) NOT NULL,
        [GuidRol]      UNIQUEIDENTIFIER NULL,
        CONSTRAINT [PK_usuariosGeneral] PRIMARY KEY ([GuidUsuario])
    );
END;

-- FK usuariosGeneral -> rolGeneral
IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE name = N'FK_usuariosGeneral_Rol'
      AND parent_object_id = OBJECT_ID(N'[dbo].[usuariosGeneral]')
)
BEGIN
    ALTER TABLE [dbo].[usuariosGeneral]
    ADD CONSTRAINT [FK_usuariosGeneral_Rol]
        FOREIGN KEY ([GuidRol]) REFERENCES [dbo].[rolGeneral]([GuidRol]);
END;

-- noticiaGeneral (GuidUsuario NULL)
IF OBJECT_ID(N'[dbo].[noticiaGeneral]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[noticiaGeneral] (
        [GuidNoticia]      UNIQUEIDENTIFIER NOT NULL DEFAULT (newsequentialid()),
        [tituloNoticia]    VARCHAR(200) NOT NULL,
        [resumenNoticia]   VARCHAR(500) NULL,
        [contenidoNoticia] TEXT NULL,
        [fechaNoticia]     DATETIME NOT NULL DEFAULT (getdate()),
        [GuidUsuario]      UNIQUEIDENTIFIER NULL,
        CONSTRAINT [PK_noticiaGeneral] PRIMARY KEY ([GuidNoticia])
    );
END;

-- FK noticiaGeneral -> usuariosGeneral
IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE name = N'FK_noticiaGeneral_Usuario'
      AND parent_object_id = OBJECT_ID(N'[dbo].[noticiaGeneral]')
)
BEGIN
    ALTER TABLE [dbo].[noticiaGeneral]
    ADD CONSTRAINT [FK_noticiaGeneral_Usuario]
        FOREIGN KEY ([GuidUsuario]) REFERENCES [dbo].[usuariosGeneral]([GuidUsuario]);
END;

-- boletinGeneral (GuidNoticia NULL)
IF OBJECT_ID(N'[dbo].[boletinGeneral]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[boletinGeneral] (
        [GuidBoletin]       UNIQUEIDENTIFIER NOT NULL DEFAULT (newsequentialid()),
        [TituloBoletin]     VARCHAR(200) NOT NULL,
        [DescripcionBoletin] VARCHAR(500) NULL,
        [FechaBoletin]      DATE NOT NULL DEFAULT (CONVERT(date, getdate())),
        [GuidNoticia]       UNIQUEIDENTIFIER NULL,
        CONSTRAINT [PK_boletinGeneral] PRIMARY KEY ([GuidBoletin])
    );
END;

-- FK boletinGeneral -> noticiaGeneral
IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE name = N'FK_boletinGeneral_Noticia'
      AND parent_object_id = OBJECT_ID(N'[dbo].[boletinGeneral]')
)
BEGIN
    ALTER TABLE [dbo].[boletinGeneral]
    ADD CONSTRAINT [FK_boletinGeneral_Noticia]
        FOREIGN KEY ([GuidNoticia]) REFERENCES [dbo].[noticiaGeneral]([GuidNoticia]);
END;

-- comentarioGeneral (GuidNoticia NULL)
IF OBJECT_ID(N'[dbo].[comentarioGeneral]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[comentarioGeneral] (
        [GuidComentario]        UNIQUEIDENTIFIER NOT NULL DEFAULT (newsequentialid()),
        [nombrelectorComentario] VARCHAR(50) NOT NULL,
        [emailLectorComentario]  VARCHAR(50) NOT NULL,
        [contenidoComentario]    TEXT NOT NULL,
        [fechaComentario]        DATETIME NOT NULL DEFAULT (getdate()),
        [GuidNoticia]            UNIQUEIDENTIFIER NULL,
        CONSTRAINT [PK_comentarioGeneral] PRIMARY KEY ([GuidComentario])
    );
END;

-- FK comentarioGeneral -> noticiaGeneral
IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE name = N'FK_comentarioGeneral_Noticia'
      AND parent_object_id = OBJECT_ID(N'[dbo].[comentarioGeneral]')
)
BEGIN
    ALTER TABLE [dbo].[comentarioGeneral]
    ADD CONSTRAINT [FK_comentarioGeneral_Noticia]
        FOREIGN KEY ([GuidNoticia]) REFERENCES [dbo].[noticiaGeneral]([GuidNoticia]);
END;

-- suscripcionGeneral (GuidBoletin NULL)
IF OBJECT_ID(N'[dbo].[suscripcionGeneral]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[suscripcionGeneral] (
        [GuidSuscripcion]   UNIQUEIDENTIFIER NOT NULL DEFAULT (newsequentialid()),
        [nombreSuscripcion] NVARCHAR(MAX) NOT NULL,
        [emailSuscripcion]  NVARCHAR(MAX) NOT NULL,
        [fechaSuscripcion]  DATETIME NOT NULL DEFAULT (getdate()),
        [GuidBoletin]       UNIQUEIDENTIFIER NULL,
        CONSTRAINT [PK_suscripcionGeneral] PRIMARY KEY ([GuidSuscripcion])
    );
END;

-- FK suscripcionGeneral -> boletinGeneral
IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE name = N'FK_suscripcionGeneral_Boletin'
      AND parent_object_id = OBJECT_ID(N'[dbo].[suscripcionGeneral]')
)
BEGIN
    ALTER TABLE [dbo].[suscripcionGeneral]
    ADD CONSTRAINT [FK_suscripcionGeneral_Boletin]
        FOREIGN KEY ([GuidBoletin]) REFERENCES [dbo].[boletinGeneral]([GuidBoletin]);
END;

-- 4) Índices
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_boletinGeneral_GuidNoticia' AND object_id = OBJECT_ID(N'[dbo].[boletinGeneral]'))
    CREATE INDEX [IX_boletinGeneral_GuidNoticia] ON [dbo].[boletinGeneral] ([GuidNoticia]);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_comentarioGeneral_GuidNoticia' AND object_id = OBJECT_ID(N'[dbo].[comentarioGeneral]'))
    CREATE INDEX [IX_comentarioGeneral_GuidNoticia] ON [dbo].[comentarioGeneral] ([GuidNoticia]);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_noticiaGeneral_GuidUsuario' AND object_id = OBJECT_ID(N'[dbo].[noticiaGeneral]'))
    CREATE INDEX [IX_noticiaGeneral_GuidUsuario] ON [dbo].[noticiaGeneral] ([GuidUsuario]);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_suscripcionGeneral_GuidBoletin' AND object_id = OBJECT_ID(N'[dbo].[suscripcionGeneral]'))
    CREATE INDEX [IX_suscripcionGeneral_GuidBoletin] ON [dbo].[suscripcionGeneral] ([GuidBoletin]);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_usuariosGeneral_GuidRol' AND object_id = OBJECT_ID(N'[dbo].[usuariosGeneral]'))
    CREATE INDEX [IX_usuariosGeneral_GuidRol] ON [dbo].[usuariosGeneral] ([GuidRol]);

-- 5) Marcar migraciones aplicadas (solo si faltan)
IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = N'20251020215923_MakeGuidRolNullable')
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251020215923_MakeGuidRolNullable', N'9.0.10');

IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = N'20251020231734_MakeSubsAndCommentsNullable')
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251020231734_MakeSubsAndCommentsNullable', N'9.0.10');

IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = N'20251020232451_MakeSubsAndCommentsNullable_v2')
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251020232451_MakeSubsAndCommentsNullable_v2', N'9.0.10');

IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = N'20251020235250_MakeGuidNoticiaNullable_en_comentario')
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251020235250_MakeGuidNoticiaNullable_en_comentario', N'9.0.10');

COMMIT TRANSACTION;
GO

PRINT '✅ Base [boletinLayon] creada/actualizada con esquema, FKs, índices y migraciones.';
