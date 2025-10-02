#pragma once
#include "WarehouseEntry.hpp"
#include "Enums.hpp"

class Resource : public WarehouseEntry  {
public:
    Resource(int, std::string, MeasureUnit, float);
    virtual ~Resource() = default;
    MeasureUnit GetMeasureUnit() const;
    float GetQuantity() const;
    void Increase(float);
    float Decrease(float);
protected:
    MeasureUnit m_measureUnit;
    float m_quantity;
};
