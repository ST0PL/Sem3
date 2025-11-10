#include "Vehicle.hpp"
#include "SupplyRequestDetail.hpp"

Vehicle::Vehicle(const Vehicle& other) : 
    Equipment(rand(), other.GetMaterialType(), other.GetName(), other.GetCount()), m_type(other.m_type), m_fuelType(other.m_fuelType) { }

Vehicle::Vehicle(int id, const std::string& name, VehicleType type, FuelType fuelType, int count)
    : Equipment(id, MaterialType::Vehicle, name, count), m_type(type), m_fuelType(fuelType) { }


bool Vehicle::IsMatches(const SupplyRequestDetail& detail) const {
    return detail.GetSupplyType() == MaterialType::Vehicle &&
        detail.GetVehicleType() == m_type &&
        detail.GetFuelType() == m_fuelType;
}

VehicleType Vehicle::GetType() const {
    return m_type;
}

FuelType Vehicle::GetFuelType() const {
    return m_fuelType;
}