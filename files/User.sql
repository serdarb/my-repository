CREATE TABLE [dbo].[User]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[PublicId] CHAR(32) NOT NULL, 

	[IsActive] BIT NOT NULL DEFAULT 1,
	[IsAdmin] BIT NULL DEFAULT 0,

	[Name] VARCHAR(100) NOT NULL,
	[Email] VARCHAR(300) NOT NULL,	
	[ImageUrl] VARCHAR(1000) NULL, 

	[PasswordHash] VARCHAR(100) NOT NULL,
	[PasswordResetToken] CHAR(32) NULL,
	[PasswordResetRequestedAt] DATETIME NULL,	 
	[LastLoginAt] DATETIME NOT NULL DEFAULT GETDATE(),	 
	[LoginTryCount] INT NOT NULL DEFAULT 0,	

    [CreatedBy] BIGINT NOT NULL,
	[CreatedAt] DATETIME NOT NULL,	 
    [UpdatedBy] BIGINT NOT NULL,     
    [UpdatedAt] DATETIME NOT NULL  
)