CREATE TABLE Task ([TaskId] int IDENTITY NOT NULL,
CourseId int NULL,
LastDate date NULL, 
IsCompleted bit NULL,
CONSTRAINT TaskId PRIMARY KEY ([TaskId]));
