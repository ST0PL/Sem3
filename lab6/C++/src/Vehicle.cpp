#include "Vehicle.hpp"
#include "SupplyRequestDetail.hpp"

Vehicle::Vehicle(const Vehicle& other) : 
    Equipment(rand(), other.GetMaterialType(), other.GetName(), other.GetCount()), m_type(other.m_type), m_fuelType(other.m_fuelType) {
    AssignWarehouse(other.GetWarehouse());
}

Vehicle::Vehicle(int id, const std::string& name, VehicleType type, FuelType fuelType, int count)
    : Equipment(id, MaterialType::eVehicle, name, count), m_type(type), m_fuelType(fuelType) { }


bool Vehicle::IsMatches(const SupplyRequestDetail& detail) const {
    return detail.GetSupplyType() == MaterialType::eVehicle &&
        detail.GetVehicleType() == m_type &&
        detail.GetFuelType() == m_fuelType;
}

VehicleType Vehicle::GetType() const {
    return m_type;
}

FuelType Vehicle::GetFuelType() const {
    return m_fuelType;
}

Vehicle& Vehicle::operator = (const Equipment& base) {
    if (base.GetMaterialType() != GetMaterialType())
        throw std::invalid_argument("Тип ресурса не соответствует типу экземпляра.");
    SetId(base.GetId());
    SetName(base.GetName());
    AssignWarehouse(base.GetWarehouse());
    m_count = base.GetCount();
    return *this;
}