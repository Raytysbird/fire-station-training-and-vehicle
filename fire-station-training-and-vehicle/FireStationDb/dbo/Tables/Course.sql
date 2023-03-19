CREATE TABLE Courses (CourseId int IDENTITY NOT NULL,
Name varchar(255) NULL,
Details varchar(255),
RenewalPeriod VARCHAR(255) NULL, 
CONSTRAINT [PK_Course] PRIMARY KEY ([CourseId]));
