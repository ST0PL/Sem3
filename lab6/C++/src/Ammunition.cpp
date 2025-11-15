#include <stdexcept>
#include "Ammunition.hpp"
#include "SupplyRequestDetail.hpp"

Ammunition::Ammunition(const Ammunition& other) :
    Resource(rand(), other.GetMaterialType(), other.GetName(), MeasureUnit::eItem, other.GetQuantity()), m_caliber(other.m_caliber) {
    AssignWarehouse(other.GetWarehouse());
}

Ammunition::Ammunition(int id, const std::string& name, Caliber caliber, float quantity)
    : Resource(id, MaterialType::eAmmunition, name, MeasureUnit::eItem, quantity), m_caliber(caliber) {
}

Ammunition& Ammunition::operator = (const Resource& base){
    if (base.GetMaterialType() != GetMaterialType())
        throw std::invalid_argument("Тип ресурса не соответствует типу экземпляра.");
    SetId(base.GetId());
    SetName(base.GetName());

    AssignWarehouse(base.GetWarehouse());
    m_measureUnit = base.GetMeasureUnit();
    m_quantity = base.GetQuantity();
    return *this;
}

bool Ammunition::IsMatches(const SupplyRequestDetail& detail) const {
    return detail.GetSupplyType() == MaterialType::eAmmunition && detail.GetCaliber() == m_caliber;
}

Caliber Ammunition::GetCaliber() const {
    return m_caliber;
}