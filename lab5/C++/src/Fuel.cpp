#include "Fuel.hpp"
#include "SupplyRequestDetail.hpp"

Fuel::Fuel(const Fuel& other) :
    Resource(rand(), other.GetName(), MeasureUnit::eLiter, other.GetQuantity()), m_type(other.m_type) { }

Fuel::Fuel(int id, const std::string& name, FuelType type, float quantity)
    : Resource(id, name, MeasureUnit::eLiter, quantity), m_type(type) {
}

bool Fuel::IsMatches(const SupplyRequestDetail& detail) const {
    return detail.GetSupplyType() == SupplyType::Fuel && detail.GetFuelType() == m_type;
}

FuelType Fuel::GetType() const {
    return m_type;
}