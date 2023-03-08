CREATE TABLE Document (Id int IDENTITY NOT NULL,
UserId NVARCHAR (450)  NULL,
Description varchar(255) NULL, 
[RequestTypeId] int NULL, 
Status bit NULL, 
[Name] VARCHAR(MAX) NULL, 
    [Path] VARCHAR(MAX) NULL, 
    CONSTRAINT [PK_Document] PRIMARY KEY ([Id]),
CONSTRAINT [FK_Document_ToTable] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers]([Id]),
CONSTRAINT [FK_Document_ToTYpe] FOREIGN KEY ([RequestTypeId]) REFERENCES [RequestType]([Id]));

