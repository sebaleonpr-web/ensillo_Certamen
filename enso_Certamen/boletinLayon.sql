IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [rolGeneral] (
    [GuidRol] uniqueidentifier NOT NULL DEFAULT ((newsequentialid())),
    [nombreRol] varchar(50) NOT NULL,
    [descripRol] varchar(200) NULL,
    CONSTRAINT [PK_rolGeneral] PRIMARY KEY ([GuidRol])
);

CREATE TABLE [usuariosGeneral] (
    [GuidUsuario] uniqueidentifier NOT NULL DEFAULT ((newsequentialid())),
    [contraUser] varchar(100) NOT NULL,
    [nombreUser] varchar(50) NOT NULL,
    [apellidoUser] varchar(50) NOT NULL,
    [emailUser] varchar(100) NOT NULL,
    [GuidRol] uniqueidentifier NULL,
    CONSTRAINT [PK_usuariosGeneral] PRIMARY KEY ([GuidUsuario]),
    CONSTRAINT [FK_usuariosGeneral_Rol] FOREIGN KEY ([GuidRol]) REFERENCES [rolGeneral] ([GuidRol])
);

CREATE TABLE [noticiaGeneral] (
    [GuidNoticia] uniqueidentifier NOT NULL DEFAULT ((newsequentialid())),
    [tituloNoticia] varchar(200) NOT NULL,
    [resumenNoticia] varchar(500) NULL,
    [contenidoNoticia] text NULL,
    [fechaNoticia] datetime NOT NULL DEFAULT ((getdate())),
    [GuidUsuario] uniqueidentifier NULL,
    CONSTRAINT [PK_noticiaGeneral] PRIMARY KEY ([GuidNoticia]),
    CONSTRAINT [FK_noticiaGeneral_Usuario] FOREIGN KEY ([GuidUsuario]) REFERENCES [usuariosGeneral] ([GuidUsuario])
);

CREATE TABLE [boletinGeneral] (
    [GuidBoletin] uniqueidentifier NOT NULL DEFAULT ((newsequentialid())),
    [TituloBoletin] varchar(200) NOT NULL,
    [DescripcionBoletin] varchar(500) NULL,
    [FechaBoletin] date NOT NULL DEFAULT ((CONVERT([date],getdate()))),
    [GuidNoticia] uniqueidentifier NULL,
    CONSTRAINT [PK_boletinGeneral] PRIMARY KEY ([GuidBoletin]),
    CONSTRAINT [FK_boletinGeneral_Noticia] FOREIGN KEY ([GuidNoticia]) REFERENCES [noticiaGeneral] ([GuidNoticia])
);

CREATE TABLE [comentarioGeneral] (
    [GuidComentario] uniqueidentifier NOT NULL DEFAULT ((newsequentialid())),
    [nombrelectorComentario] varchar(50) NOT NULL,
    [emailLectorComentario] varchar(50) NOT NULL,
    [contenidoComentario] text NOT NULL,
    [fechaComentario] datetime NOT NULL DEFAULT ((getdate())),
    [GuidNoticia] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_comentarioGeneral] PRIMARY KEY ([GuidComentario]),
    CONSTRAINT [FK_comentarioGeneral_Noticia] FOREIGN KEY ([GuidNoticia]) REFERENCES [noticiaGeneral] ([GuidNoticia])
);

CREATE TABLE [suscripcionGeneral] (
    [GuidSuscripcion] uniqueidentifier NOT NULL DEFAULT ((newsequentialid())),
    [nombreSuscripcion] nvarchar(max) NOT NULL,
    [emailSuscripcion] nvarchar(max) NOT NULL,
    [fechaSuscripcion] datetime NOT NULL DEFAULT ((getdate())),
    [GuidBoletin] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_suscripcionGeneral] PRIMARY KEY ([GuidSuscripcion]),
    CONSTRAINT [FK_suscripcionGeneral_Boletin] FOREIGN KEY ([GuidBoletin]) REFERENCES [boletinGeneral] ([GuidBoletin])
);

CREATE INDEX [IX_boletinGeneral_GuidNoticia] ON [boletinGeneral] ([GuidNoticia]);

CREATE INDEX [IX_comentarioGeneral_GuidNoticia] ON [comentarioGeneral] ([GuidNoticia]);

CREATE INDEX [IX_noticiaGeneral_GuidUsuario] ON [noticiaGeneral] ([GuidUsuario]);

CREATE INDEX [IX_suscripcionGeneral_GuidBoletin] ON [suscripcionGeneral] ([GuidBoletin]);

CREATE INDEX [IX_usuariosGeneral_GuidRol] ON [usuariosGeneral] ([GuidRol]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251020215923_MakeGuidRolNullable', N'9.0.10');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[suscripcionGeneral]') AND [c].[name] = N'GuidBoletin');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [suscripcionGeneral] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [suscripcionGeneral] ALTER COLUMN [GuidBoletin] uniqueidentifier NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251020231734_MakeSubsAndCommentsNullable', N'9.0.10');

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[comentarioGeneral]') AND [c].[name] = N'GuidNoticia');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [comentarioGeneral] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [comentarioGeneral] ALTER COLUMN [GuidNoticia] uniqueidentifier NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251020232451_MakeSubsAndCommentsNullable_v2', N'9.0.10');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251020235250_MakeGuidNoticiaNullable_en_comentario', N'9.0.10');

COMMIT;
GO

