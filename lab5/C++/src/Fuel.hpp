#pragma once
#include "Resource.hpp"

class SupplyRequestDetail;

class Fuel : public Resource {
public:
    Fuel(int, std::string, FuelType, float);
    FuelType GetType() const;
    bool IsMatches(SupplyRequestDetail*) const override;
private:
    FuelType m_type;
};