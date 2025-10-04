#pragma once
enum class Rank {
    ePrivate = 0, //
    eCorporal = 1, // ��������
    eJuniorSergeant = 2, // ������� �������
    eSergeant = 3, // C������
    eSeniorSergeant = 4, // ������� �������
    eSergeantMajor = 5, // ��������
    eWarrantOfficer = 6, // ���������
    eSeniorWarrantOfficer = 7, // ������� ���������
    eJuniorLieutenant = 8, // ������� ���������
    eLieutenant = 9, // ���������
    eSeniorLieutenant = 10, // ������� ���������
    eCaptain = 11, // �������
    eMajor = 12, // �����
    eLieutenantColonel = 13, // ������������
    eColonel = 14, // ���������
    eMajorGeneral = 15, // ������� �����
    eLieutenantGeneral = 16, // ������� ���������
    eColonelGeneral = 17, // ������� ���������
    eArmyGeneral = 18 // ������� �����
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