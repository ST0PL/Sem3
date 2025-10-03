#include "SupplyRequestDetail.hpp"
#include <string>

SupplyRequestDetail::SupplyRequestDetail(int id, SupplyType type, float count)
    : m_id(id), m_supplyType(type), m_count(count) {
}

SupplyRequestDetail::SupplyRequestDetail(int id, SupplyType type, float count, Caliber caliber)
    : SupplyRequestDetail(id, type, count)
{
    m_caliber = caliber;
}

SupplyRequestDetail::SupplyRequestDetail(int id, SupplyType type, float count, FuelType fuelType)
    : SupplyRequestDetail(id, type, count) {
    m_fuelType = fuelType;
}

SupplyRequestDetail SupplyRequestDetail::WithCaliber(Caliber caliber) {
    return SupplyRequestDetail(this->m_id, this->m_supplyType, this->m_count, caliber);
}

SupplyRequestDetail SupplyRequestDetail::WithFuelType(FuelType fuelType) {
    return SupplyRequestDetail(this->m_id, this->m_supplyType, this->m_count, fuelType);
}

int SupplyRequestDetail::GetId() const {
    return m_id;
}

SupplyType SupplyRequestDetail::GetSupplyType() const {
    return m_supplyType;
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

std::string SupplyRequestDetail::ToString(std::vector<std::unique_ptr<SupplyRequestDetail>>& details) {

    std::string result = "";

    for (const auto& detail : details) {
        result.append(detail->ToString()).append("\n");
    }

    return result;
}


std::string SupplyRequestDetail::ToString() const {

    std::string result = SupplyTypeToString(m_supplyType) + ": ";

    switch (m_supplyType) {
    case SupplyType::Ammunition:
        result += CaliberToString(m_caliber) + ", " + std::to_string(m_count) + "шт";
        break;
    case SupplyType::Fuel:
        result += FuelTypeToString(m_fuelType) + ", " + std::to_string(m_count) + "л";
        break;
    case SupplyType::Vehicle:
        result += VehicleTypeToString(m_vehicleType) + ", " +
            FuelTypeToString(m_fuelType) + ", " +
            std::to_string(m_count) + "шт";
        break;
    case SupplyType::Weapon:
        result += CaliberToString(m_caliber) + ", " + std::to_string(m_count) + "шт";
        break;
    }

    return result;
}

std::string SupplyRequestDetail::SupplyTypeToString(SupplyType stype) {
    switch (stype) {
    case SupplyType::Ammunition:
        return "Боеприпасы";
    case SupplyType::Fuel:
        return "Топливо";
    case SupplyType::Vehicle:
        return "Транспорт";
    case SupplyType::Weapon:
        return "Вооружение";
    default:
        return "Неизвестно";
    }
}
std::string SupplyRequestDetail::CaliberToString(Caliber caliber) {
    switch (caliber) {
    case Caliber::e545mm:
        return "5.25мм";
    case Caliber::e122mm:
        return "122мм";
    default:
        return "Неизвестно";
    }
}
std::string SupplyRequestDetail::FuelTypeToString(FuelType ftype) {
    switch (ftype) {
    case FuelType::Gasoline:
        return "Бензин";
    case FuelType::Diesel:
        return "Дизель";
    default:
        return "Неизвестно";
    }
}

std::string SupplyRequestDetail::VehicleTypeToString(VehicleType vtype) {
    switch (vtype) {
    case VehicleType::ArmoredVehicle:
        return "Бронемашина";
    case VehicleType::Tank:
        return "Танк";
    case VehicleType::Motorbike:
        return "Мотоцикл";
    default:
        return "Неизвестно";
    }
}