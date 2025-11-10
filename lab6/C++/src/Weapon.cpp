#include "Weapon.hpp"
#include "SupplyRequestDetail.hpp"


Weapon::Weapon(const Weapon& other) :
    Equipment(rand(), other.GetMaterialType(), other.GetName(), other.GetCount()), m_caliber(other.m_caliber) {
}

Weapon::Weapon(int id, const std::string& name, Caliber caliber, int count)
    : Equipment(id, MaterialType::Weapon, name, count), m_caliber(caliber) { }

bool Weapon::IsMatches(const SupplyRequestDetail& detail) const {
    return detail.GetSupplyType() == MaterialType::Weapon && detail.GetCaliber() == m_caliber;
}

Caliber Weapon::GetCaliber() const {
    return m_caliber;
}
