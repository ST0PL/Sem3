#include "Ammunition.hpp"
#include "SupplyRequestDetail.hpp"

Ammunition::Ammunition(int id, const std::string& name, Caliber caliber, float quantity)
    : Resource(id, name, MeasureUnit::eItem, quantity), m_caliber(caliber) {
}

bool Ammunition::IsMatches(SupplyRequestDetail* detail) const {
    return detail->GetSupplyType() == SupplyType::Ammunition && detail->GetCaliber() == m_caliber;
}

Caliber Ammunition::GetCaliber() const {
    return m_caliber;
}