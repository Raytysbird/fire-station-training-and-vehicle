CREATE TABLE AssignedTask ([Task_Id] int NOT NULL,
[User_Id] NVARCHAR(450) NOT NULL,
PRIMARY KEY ([Task_Id], [User_Id]),
 CONSTRAINT [FKUser] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FKTask] FOREIGN KEY ([Task_Id]) REFERENCES [dbo].[UserTask] ([TaskId]));
