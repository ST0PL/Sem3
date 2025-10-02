#pragma once
enum class Rank {
    ePrivate = 0, //
    eCorporal = 1, // Ефрейтор
    eJuniorSergeant = 2, // Младший сержант
    eSergeant = 3, // Cержант
    eSeniorSergeant = 4, // Старший сержант
    eSergeantMajor = 5, // Старшина
    eWarrantOfficer = 6, // Прапорщик
    eSeniorWarrantOfficer = 7, // Старший прапорщик
    eJuniorLieutenant = 8, // Младший лейтенант
    eLieutenant = 9, // Лейтенант
    eSeniorLieutenant = 10, // Старший лейтенант
    eCaptain = 11, // Капитан
    eMajor = 12, // Майор
    eLieutenantColonel = 13, // Подполковник
    eColonel = 14, // Полковник
    eMajorGeneral = 15, // Генерал майор
    eLieutenantGeneral = 16, // Генерал лейтенант
    eColonelGeneral = 17, // Генерал полковник
    eArmyGeneral = 18 // Генерал армии
};

enum class Speciality {
    eNone = 0,
    eInfantry = 1,
    eMachineGunner = 2,
    eGrenadeLauncher = 3,
    eReconnaissance = 4,
    eSpecialForces = 5,
    eSniper = 6,
    eTanker = 7,
    eAntiAircraft = 8,
    eEngineer = 9,
    eMedic = 10
};

enum class UnitType {
    eBattalion = 0,
    eRegiment = 1,
    eBrigade = 2,
    eDivision = 3,
    eArmy = 4,
};

enum class Caliber {
    e545mm = 0,
    e122mm = 1,
};

enum class FuelType {
    Gasoline = 0,
    Diesel = 1
};

enum class MeasureUnit {
    eItem = 0,
    eLiter = 1,
    eKilogram = 2
};

enum class VehicleType
{
    Tank = 0,
    ArmoredVehicle = 1,
    Motorbike = 2
};
enum class SupplyType
{
    Ammunition = 0,
    Fuel = 1,
    Weapon = 2,
    Vehicle = 3
};
enum SupplyResponseStatus
{
    Success = 0,
    Partial = 1,
    Denied = 2
};

enum class WarehouseType {
    eRear = 0,
    eField = 1
};