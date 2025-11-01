#include "Weapon.hpp"
#include "SupplyRequestDetail.hpp"


Weapon::Weapon(const Weapon& other) :
    Equipment(rand(), other.GetName(), other.GetCount()), m_caliber(other.m_caliber) {
}

Weapon::Weapon(int id, const std::string& name, Caliber caliber, int count)
    : Equipment(id, name, count), m_caliber(caliber) { }

bool Weapon::IsMatches(const SupplyRequestDetail& detail) const {
    return detail.GetSupplyType() == SupplyType::Weapon && detail.GetCaliber() == m_caliber;
}

Caliber Weapon::GetCaliber() const {
    return m_caliber;
}
