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
    eGasoline = 0,
    eDiesel = 1
};

enum class MeasureUnit {
    eItem = 0,
    eLiter = 1,
    eKilogram = 2
};

enum class VehicleType
{
    eTank = 0,
    eArmoredVehicle = 1,
    eMotorbike = 2,
    eSelfPropelledLauncher = 3
};
enum class MaterialType
{
    eAmmunition = 0,
    eFuel = 1,
    eWeapon = 2,
    eVehicle = 3
};
enum SupplyResponseStatus
{
    eSuccess = 0,
    ePartial = 1,
    eDenied = 2
};

enum class WarehouseType {
    eRear = 0,
    eField = 1
};

enum class LogLevel {
    eInfo = 0,
    eWarn = 1,
    eDebug = 2,
    eError = 3,
};