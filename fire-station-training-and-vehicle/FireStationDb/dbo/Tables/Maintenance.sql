CREATE TABLE Maintenance (RepairId int IDENTITY NOT NULL,
VehicleId int NOT NULL,
DateOfRepair DATE NULL,
Description varchar(255) NULL,
DateCompleted DATE NULL, 
Milage int NULL, Notes varchar(255) NULL, [Status] VARCHAR(50) NULL, 
    PRIMARY KEY (RepairId),
CONSTRAINT [FK_VehicleMain] FOREIGN KEY ([VehicleId]) REFERENCES [Vehicle]([Vehicle_Id]));
