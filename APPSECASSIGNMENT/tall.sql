drop table if exists Account;

CREATE TABLE [dbo].[Account] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [First Name]   VARCHAR (50)     NULL,
    [Last Name]    VARCHAR (50)     NULL,
    [Credit Card]  NVARCHAR (MAX)   NULL,
    [Email]        VARCHAR (50)     NULL,
    [DoB]          VARCHAR (50)     NULL,
    [Photo]        VARBINARY (MAX)  NULL,
    [PasswordHash] NVARCHAR (MAX)   NULL,
    [PasswordSalt] NVARCHAR (MAX)   NULL,
    [IV]           NVARCHAR (MAX)   NULL,
    [Key]          NVARCHAR (MAX)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
