CREATE TABLE Task_FireFighter (TaskId int NOT NULL,
UserId NVARCHAR(450) NOT NULL,
PRIMARY KEY (TaskId, UserId),
 CONSTRAINT [FKUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FKTask] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Task] ([TaskId]));
