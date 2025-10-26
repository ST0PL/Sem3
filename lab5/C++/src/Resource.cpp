#include "stdexcept"
#include "Resource.hpp"
#include "WarehouseEntry.hpp"

Resource::Resource(int id, const std::string& name, MeasureUnit measureUnit, float quantity)
    : WarehouseEntry<float>(id, name), m_measureUnit(measureUnit){
    if (quantity < Resource::MIN_QUANTITY)
        throw std::invalid_argument("Недопустимое количество");
    m_quantity = quantity;
}

MeasureUnit Resource::GetMeasureUnit() const {
    return m_measureUnit;
}

float Resource::GetQuantity() const {
    return m_quantity;
}

bool Resource::IsEmpty() const {
    return m_quantity <= 0;
}

void Resource::Increase(float quantity) {
    m_quantity += quantity;
}

float Resource::Decrease(float quantity) {
    float taken = std::min(m_quantity, quantity);
    m_quantity -= taken;
    return taken;
}