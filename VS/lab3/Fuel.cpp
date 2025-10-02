#include "Fuel.hpp"
#include "SupplyRequestDetail.hpp"

Fuel::Fuel(int id, std::string name, FuelType type, float quantity)
    : Resource(id, name, MeasureUnit::eLiter, m_quantity), m_type(type) {
}

bool Fuel::IsMatches(SupplyRequestDetail* detail) const {
    return detail->GetSupplyType() == SupplyType::Fuel && detail->GetFuelType() == m_type;
}

FuelType Fuel::GetType() const {
    return m_type;
}