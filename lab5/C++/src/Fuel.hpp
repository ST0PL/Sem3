#pragma once
#include "Resource.hpp"

class SupplyRequestDetail;

class Fuel : public Resource {
public:
    Fuel(const Fuel&);
    Fuel(int, const std::string&, FuelType, float);
    FuelType GetType() const;
    bool IsMatches(const SupplyRequestDetail&) const override;
private:
    FuelType m_type;
};