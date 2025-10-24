#include "Vehicle.hpp"
#include "SupplyRequestDetail.hpp"

Vehicle::Vehicle(int id, const std::string& name, VehicleType type, FuelType fuelType, int count)
    : Equipment(id, name, count), m_type(type), m_fuelType(fuelType) {
}


bool Vehicle::IsMatches(SupplyRequestDetail* detail) const {
    return detail->GetSupplyType() == SupplyType::Vehicle &&
        detail->GetVehicleType() == m_type &&
        detail->GetFuelType() == m_fuelType;
}

VehicleType Vehicle::GetType() const {
    return m_type;
}

FuelType Vehicle::GetFuelType() const {
    return m_fuelType;
}