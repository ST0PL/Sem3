#pragma once
#include "WarehouseEntry.hpp"
#include "Enums.hpp"

class Resource : public WarehouseEntry<float> {
public:
    static constexpr float MIN_QUANTITY = 0.1f;
    Resource(int id, const std::string& name, MeasureUnit measureUnit, float quantity);
    virtual ~Resource() = default;
    MeasureUnit GetMeasureUnit() const;
    float GetQuantity() const;
    bool IsEmpty() const override;
    void Increase(float);
    float Decrease(float);
private:
    MeasureUnit m_measureUnit;
    float m_quantity;
};
