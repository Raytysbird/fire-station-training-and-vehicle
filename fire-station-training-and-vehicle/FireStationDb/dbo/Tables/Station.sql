CREATE TABLE [dbo].[Station]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NULL, 
    [Address] VARCHAR(500) NULL, 
    [Email] VARCHAR(50) NULL, 
    [PhoneNumber] VARCHAR(50) NULL, 
    [IsDeleted] BIT NULL DEFAULT 0
    
)
