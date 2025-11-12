#include <stdexcept>
#include "Ammunition.hpp"
#include "SupplyRequestDetail.hpp"

Ammunition::Ammunition(const Ammunition& other) :
    Resource(rand(), other.GetMaterialType(), other.GetName(), MeasureUnit::eItem, other.GetQuantity()), m_caliber(other.m_caliber) { }

Ammunition::Ammunition(int id, const std::string& name, Caliber caliber, float quantity)
    : Resource(id, MaterialType::Ammunition, name, MeasureUnit::eItem, quantity), m_caliber(caliber) {
}

Ammunition& Ammunition::operator = (const Resource& base){
    if (base.GetMaterialType() != GetMaterialType())
        throw std::invalid_argument("“ип ресурса не соответствует типу экземпл€ра.");
    SetId(base.GetId());
    SetName(base.GetName());

    AssignWarehouse(base.GetWarehouse());
    m_measureUnit = base.GetMeasureUnit();
    m_quantity = base.GetQuantity();
    return *this;
}

bool Ammunition::IsMatches(const SupplyRequestDetail& detail) const {
    return detail.GetSupplyType() == MaterialType::Ammunition && detail.GetCaliber() == m_caliber;
}

Caliber Ammunition::GetCaliber() const {
    return m_caliber;
}