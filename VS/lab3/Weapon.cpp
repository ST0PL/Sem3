#include "Weapon.hpp"
#include "SupplyRequestDetail.hpp"

Weapon::Weapon(int id, std::string name, Caliber caliber, int count)
    : Equipment(id, name, count), m_caliber(caliber)
{
    m_id = id;
    m_name = name;
    m_count = count;
}

bool Weapon::IsMatches(SupplyRequestDetail* detail) const {
    return detail->GetSupplyType() == SupplyType::Weapon && detail->GetCaliber() == m_caliber;
}

Caliber Weapon::GetCaliber() const {
    return m_caliber;
}
