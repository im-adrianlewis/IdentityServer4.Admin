IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Roles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Users] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [RoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RoleClaims_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [UserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserClaims_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [UserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_UserLogins_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [UserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [UserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_RoleClaims_RoleId] ON [RoleClaims] ([RoleId]);

GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [Roles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

GO

CREATE INDEX [IX_UserClaims_UserId] ON [UserClaims] ([UserId]);

GO

CREATE INDEX [IX_UserLogins_UserId] ON [UserLogins] ([UserId]);

GO

CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);

GO

CREATE INDEX [EmailIndex] ON [Users] ([NormalizedEmail]);

GO

CREATE UNIQUE INDEX [UserNameIndex] ON [Users] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191119163918_DbInit', N'3.0.1');

GO

DROP INDEX [UserNameIndex] ON [Users];

GO

ALTER TABLE [Users] ADD [Address1] nvarchar(255) NULL;

GO

ALTER TABLE [Users] ADD [Address2] nvarchar(255) NULL;

GO

ALTER TABLE [Users] ADD [AuthenticationType] nvarchar(max) NULL;

GO

ALTER TABLE [Users] ADD [City] nvarchar(50) NULL;

GO

ALTER TABLE [Users] ADD [Country] nvarchar(100) NULL;

GO

ALTER TABLE [Users] ADD [County] nvarchar(50) NULL;

GO

ALTER TABLE [Users] ADD [CreateDate] datetimeoffset NOT NULL DEFAULT '0001-01-01T00:00:00.0000000+00:00';

GO

ALTER TABLE [Users] ADD [FirstName] nvarchar(max) NULL;

GO

ALTER TABLE [Users] ADD [FirstPartyIm] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

ALTER TABLE [Users] ADD [FirstPartyImUpdatedDate] datetimeoffset NOT NULL DEFAULT '0001-01-01T00:00:00.0000000+00:00';

GO

ALTER TABLE [Users] ADD [LastLoggedInDate] datetimeoffset NULL;

GO

ALTER TABLE [Users] ADD [LastLoggedInIpAddress] nvarchar(50) NULL;

GO

ALTER TABLE [Users] ADD [LastName] nvarchar(max) NULL;

GO

ALTER TABLE [Users] ADD [LastUpdatedDate] datetimeoffset NOT NULL DEFAULT '0001-01-01T00:00:00.0000000+00:00';

GO

ALTER TABLE [Users] ADD [Postcode] nvarchar(20) NULL;

GO

ALTER TABLE [Users] ADD [RegistrationDate] datetimeoffset NULL;

GO

ALTER TABLE [Users] ADD [RegistrationIpAddress] nvarchar(50) NULL;

GO

ALTER TABLE [Users] ADD [ScreenName] nvarchar(100) NOT NULL DEFAULT N'';

GO

ALTER TABLE [Users] ADD [TenantId] nvarchar(128) NOT NULL DEFAULT N'';

GO

ALTER TABLE [Users] ADD [UserBiography] nvarchar(4000) NULL;

GO

ALTER TABLE [Users] ADD [UserType] nvarchar(50) NULL;

GO

CREATE INDEX [UserNameIndex] ON [Users] ([NormalizedUserName]);

GO

CREATE UNIQUE INDEX [Tenant_EmailIndex] ON [Users] ([TenantId], [NormalizedEmail]) WHERE [NormalizedEmail] IS NOT NULL;

GO

CREATE UNIQUE INDEX [Tenant_UserNameIndex] ON [Users] ([TenantId], [NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

GO

CREATE UNIQUE INDEX [Tenant_ScreenNameIndex] ON [Users] ([TenantId], [ScreenName]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191204194320_ImAccessMultiTenant', N'3.0.1');

GO

