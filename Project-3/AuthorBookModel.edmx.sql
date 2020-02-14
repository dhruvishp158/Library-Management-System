
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/25/2019 23:39:05
-- Generated from EDMX file: C:\Users\owner\OneDrive\Desktop\Lasalle\SEM3\Multi-tier\Project\HiTechOrderSyatem\Project-3\AuthorBookModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [HiTechDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AuthorBooks_Authors]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AuthorBooks] DROP CONSTRAINT [FK_AuthorBooks_Authors];
GO
IF OBJECT_ID(N'[dbo].[FK_AuthorBooks_Books]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AuthorBooks] DROP CONSTRAINT [FK_AuthorBooks_Books];
GO
IF OBJECT_ID(N'[dbo].[FK_Books_Publishers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Books] DROP CONSTRAINT [FK_Books_Publishers];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AuthorBooks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AuthorBooks];
GO
IF OBJECT_ID(N'[dbo].[Authors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Authors];
GO
IF OBJECT_ID(N'[dbo].[Books]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Books];
GO
IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[HiTechEmployee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HiTechEmployee];
GO
IF OBJECT_ID(N'[dbo].[HiTechUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HiTechUser];
GO
IF OBJECT_ID(N'[dbo].[Publishers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Publishers];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Authors'
CREATE TABLE [dbo].[Authors] (
    [AuthorId] int  NOT NULL,
    [FirstName] nvarchar(50)  NULL,
    [LastName] nvarchar(50)  NULL,
    [Email] nvarchar(50)  NULL
);
GO

-- Creating table 'Books'
CREATE TABLE [dbo].[Books] (
    [ISBN] nvarchar(50)  NOT NULL,
    [Title] nvarchar(50)  NULL,
    [UnitPrice] int  NULL,
    [YearPlublished] datetime  NULL,
    [QOH] int  NULL,
    [PublisherId] int  NOT NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [CustomerId] int  NOT NULL,
    [Name] nvarchar(50)  NULL,
    [Street] nvarchar(50)  NULL,
    [City] nvarchar(50)  NULL,
    [PostalCode] nvarchar(50)  NULL,
    [CreditLimit] int  NULL,
    [PhoneNumber] int  NULL,
    [FaxNumber] nvarchar(50)  NULL
);
GO

-- Creating table 'HiTechEmployees'
CREATE TABLE [dbo].[HiTechEmployees] (
    [EmployeeId] int  NOT NULL,
    [FirstName] nvarchar(50)  NULL,
    [LastName] nvarchar(50)  NULL
);
GO

-- Creating table 'HiTechUsers'
CREATE TABLE [dbo].[HiTechUsers] (
    [UserId] int  NOT NULL,
    [FirstName] nvarchar(50)  NULL,
    [LastName] nvarchar(50)  NULL,
    [Password] nvarchar(50)  NULL,
    [JobTitle] nvarchar(50)  NULL
);
GO

-- Creating table 'Publishers'
CREATE TABLE [dbo].[Publishers] (
    [PublisherId] int  NOT NULL,
    [Name] nvarchar(50)  NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'AuthorBooks'
CREATE TABLE [dbo].[AuthorBooks] (
    [Authors_AuthorId] int  NOT NULL,
    [Books_ISBN] nvarchar(50)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [AuthorId] in table 'Authors'
ALTER TABLE [dbo].[Authors]
ADD CONSTRAINT [PK_Authors]
    PRIMARY KEY CLUSTERED ([AuthorId] ASC);
GO

-- Creating primary key on [ISBN] in table 'Books'
ALTER TABLE [dbo].[Books]
ADD CONSTRAINT [PK_Books]
    PRIMARY KEY CLUSTERED ([ISBN] ASC);
GO

-- Creating primary key on [CustomerId] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [EmployeeId] in table 'HiTechEmployees'
ALTER TABLE [dbo].[HiTechEmployees]
ADD CONSTRAINT [PK_HiTechEmployees]
    PRIMARY KEY CLUSTERED ([EmployeeId] ASC);
GO

-- Creating primary key on [UserId] in table 'HiTechUsers'
ALTER TABLE [dbo].[HiTechUsers]
ADD CONSTRAINT [PK_HiTechUsers]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [PublisherId] in table 'Publishers'
ALTER TABLE [dbo].[Publishers]
ADD CONSTRAINT [PK_Publishers]
    PRIMARY KEY CLUSTERED ([PublisherId] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [Authors_AuthorId], [Books_ISBN] in table 'AuthorBooks'
ALTER TABLE [dbo].[AuthorBooks]
ADD CONSTRAINT [PK_AuthorBooks]
    PRIMARY KEY CLUSTERED ([Authors_AuthorId], [Books_ISBN] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [PublisherId] in table 'Books'
ALTER TABLE [dbo].[Books]
ADD CONSTRAINT [FK_Books_Publishers]
    FOREIGN KEY ([PublisherId])
    REFERENCES [dbo].[Publishers]
        ([PublisherId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Books_Publishers'
CREATE INDEX [IX_FK_Books_Publishers]
ON [dbo].[Books]
    ([PublisherId]);
GO

-- Creating foreign key on [Authors_AuthorId] in table 'AuthorBooks'
ALTER TABLE [dbo].[AuthorBooks]
ADD CONSTRAINT [FK_AuthorBooks_Authors]
    FOREIGN KEY ([Authors_AuthorId])
    REFERENCES [dbo].[Authors]
        ([AuthorId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Books_ISBN] in table 'AuthorBooks'
ALTER TABLE [dbo].[AuthorBooks]
ADD CONSTRAINT [FK_AuthorBooks_Books]
    FOREIGN KEY ([Books_ISBN])
    REFERENCES [dbo].[Books]
        ([ISBN])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AuthorBooks_Books'
CREATE INDEX [IX_FK_AuthorBooks_Books]
ON [dbo].[AuthorBooks]
    ([Books_ISBN]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------