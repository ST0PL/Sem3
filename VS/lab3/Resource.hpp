#pragma once
#include "WarehouseEntry.hpp"
#include "Enums.hpp"

class Resource : public WarehouseEntry<float> {
public:
    Resource(int id, std::string name, MeasureUnit measureUnit, float quantity);
    virtual ~Resource() = default;
    MeasureUnit GetMeasureUnit() const;
    float GetQuantity() const;
    bool IsEmpty() const override;
    void Increase(float);
    float Decrease(float);
protected:
    MeasureUnit m_measureUnit;
    float m_quantity;
};
