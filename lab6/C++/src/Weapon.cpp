#include "Weapon.hpp"
#include "SupplyRequestDetail.hpp"


Weapon::Weapon(const Weapon& other) :
    Equipment(rand(), other.GetMaterialType(), other.GetName(), other.GetCount()), m_caliber(other.m_caliber) {
    AssignWarehouse(other.GetWarehouse());
}

Weapon::Weapon(int id, const std::string& name, Caliber caliber, int count)
    : Equipment(id, MaterialType::Weapon, name, count), m_caliber(caliber) { }

bool Weapon::IsMatches(const SupplyRequestDetail& detail) const {
    return detail.GetSupplyType() == MaterialType::Weapon && detail.GetCaliber() == m_caliber;
}

Caliber Weapon::GetCaliber() const {
    return m_caliber;
}

Weapon& Weapon::operator = (const Equipment& base) {
    if (base.GetMaterialType() != GetMaterialType())
        throw std::invalid_argument("“ип ресурса не соответствует типу экземпл€ра.");
    SetId(base.GetId());
    SetName(base.GetName());
    AssignWarehouse(base.GetWarehouse());
    m_count = base.GetCount();
    return *this;
}
