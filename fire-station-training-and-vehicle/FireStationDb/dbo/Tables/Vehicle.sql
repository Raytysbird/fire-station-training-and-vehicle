CREATE TABLE Vehicle 
(Vehicle_Id int IDENTITY NOT NULL, 
VehicleTypeId int NULL,
StationId int NULL, name varchar(255) NULL,
LicencePlate varchar(255) NULL, 
LicenceExpiry date NULL, 
VehicleStatus varchar(255) NULL,
Make varchar(255) NULL, 
Model varchar(255) NULL,
Year int NULL,
[IsDeleted] BIT NULL, 
    PRIMARY KEY (Vehicle_Id),
CONSTRAINT [FK_Vehicle_1] FOREIGN KEY ([VehicleTypeId]) REFERENCES [VehicleType]([VehicleTypeId]),
CONSTRAINT [FK_Vehicle_2] FOREIGN KEY ([StationId]) REFERENCES [Station]([Id]));
