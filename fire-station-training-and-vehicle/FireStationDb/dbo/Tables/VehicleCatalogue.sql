
CREATE TABLE [dbo].[VehicleCatalogue] 
(DefaultTypeId int IDENTITY NOT NULL, 
TypeName varchar(50) NULL, 
Description varchar(255) NULL, 
TankMinimumCapacity int NULL,
HeavyRescue BIT NULL, 
HoseOneHalfInch int NULL,
HoseOneInch int NULL,
HoseTwoHalfInch int NULL,
Ladders bit NULL,
MasterStream bit NULL,
[MaximumGVR] int NULL, 
[MinPersonel] int NULL,
[PumpAndRoll] bit NULL,
[PumpMinimumFlow] int NULL,
[RatedPressure] int NULL, 
[Turntable] bit NULL, 
[TypicalUse] varchar(255) NULL,
[WildLandRescue] bit NULL, 
[Structure] bit NULL, 
PRIMARY KEY (DefaultTypeId));

