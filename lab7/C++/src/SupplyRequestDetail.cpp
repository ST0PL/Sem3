#include <string>
#include <algorithm>
#include "SupplyRequestDetail.hpp"

SupplyRequestDetail::SupplyRequestDetail(const SupplyRequestDetail& other) : 
    m_id(rand()), m_materialType(other.m_materialType), m_count(other.m_count),
    m_caliber(other.m_caliber), m_fuelType(other.m_fuelType), m_vehicleType(other.m_vehicleType){
}

SupplyRequestDetail::SupplyRequestDetail(int id, MaterialType type, float count)
    : m_id(id), m_materialType(type), m_count(count) {
}

SupplyRequestDetail& SupplyRequestDetail::WithCaliber(Caliber caliber) {
    m_caliber = caliber;
    return *this;
}

SupplyRequestDetail& SupplyRequestDetail::WithFuelType(FuelType fuelType) {
    m_fuelType = fuelType;
    return *this;
}

SupplyRequestDetail& SupplyRequestDetail::WithVehicleType(VehicleType vehicleType) {
    m_vehicleType = vehicleType;
    return *this;
}

int SupplyRequestDetail::GetId() const {
    return m_id;
}

MaterialType SupplyRequestDetail::GetSupplyType() const {
    return m_materialType;
}

Caliber SupplyRequestDetail::GetCaliber() const {
    return m_caliber;
}

VehicleType SupplyRequestDetail::GetVehicleType() const {
    return m_vehicleType;
}

FuelType SupplyRequestDetail::GetFuelType() const {
    return m_fuelType;
}

float SupplyRequestDetail::GetCount() const {
    return m_count;
}

void SupplyRequestDetail::SetCount(float count) {
    m_count = count;
}

SupplyRequestDetail& SupplyRequestDetail::operator +=(float count) {
    m_count += count;
    return *this;
}

SupplyRequestDetail& SupplyRequestDetail::operator -=(float count) {
    m_count -= std::min(m_count, count);
    return *this;
}

std::string SupplyRequestDetail::ToString() const {

    std::string result = MaterialTypeToString(m_materialType) + ": ";

    switch (m_materialType) {
    case MaterialType::eAmmunition:
        result += CaliberToString(m_caliber) + ", " + std::to_string(m_count) + " шт";
        break;
    case MaterialType::eFuel:
        result += FuelTypeToString(m_fuelType) + ", " + std::to_string(m_count) + " л";
        break;
    case MaterialType::eVehicle:
        result += VehicleTypeToString(m_vehicleType) + ", " +
            FuelTypeToString(m_fuelType) + ", " +
            std::to_string(m_count) + " шт";
        break;
    case MaterialType::eWeapon:
        result += CaliberToString(m_caliber) + ", " + std::to_string(m_count) + " шт";
        break;
    }

    return result;
}

std::string SupplyRequestDetail::MaterialTypeToString(MaterialType mtype) {
    return m_materialTypes.at(mtype);
}
std::string SupplyRequestDetail::CaliberToString(Caliber caliber) {
    return m_calibers.at(caliber);
}
std::string SupplyRequestDetail::FuelTypeToString(FuelType ftype) {
    return m_fuelTypes.at(ftype);
}

std::string SupplyRequestDetail::VehicleTypeToString(VehicleType vtype) {
    return m_vehicleTypes.at(vtype);
}


const std::map<MaterialType, std::string> SupplyRequestDetail::m_materialTypes {
    {MaterialType::eAmmunition, "Боеприпасы"},
    {MaterialType::eFuel, "Топливо"},
    {MaterialType::eVehicle, "Транспорт"},
    {MaterialType::eWeapon, "Вооружение"}
};

// Инициализация словаря калибров
const std::map<Caliber, std::string> SupplyRequestDetail::m_calibers = {
    {Caliber::e545mm, "5.45мм"},
    {Caliber::e122mm, "122мм"}
};

// Инициализация словаря типов топлива
const std::map<FuelType, std::string> SupplyRequestDetail::m_fuelTypes {
    {FuelType::eGasoline, "Бензин"},
    {FuelType::eDiesel, "Дизель"}
};

// Инициализация словаря типов транспорта
const std::map<VehicleType, std::string> SupplyRequestDetail::m_vehicleTypes = {
    {VehicleType::eTank, "Танк"},
    {VehicleType::eArmoredVehicle, "Бронемашина"},
    {VehicleType::eMotorbike, "Мотоцикл"},
    {VehicleType::eSelfPropelledLauncher, "Самоходная установка"}
};
