CREATE TABLE VehicleReports (ReportId int IDENTITY NOT NULL,
UserId  NVARCHAR (450) NULL, 
VehicleId int NULL, 
DateReported date NULL, 
Status varchar(255) NULL, 
IssueType varchar(255) NULL,
Description varchar(255) NULL,
CONSTRAINT ReportId PRIMARY KEY (ReportId),
CONSTRAINT [FK_VehicleReport] FOREIGN KEY ([VehicleId]) REFERENCES [Vehicle]([Vehicle_Id]),
CONSTRAINT [FK_ReportUser] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers]([Id]));


