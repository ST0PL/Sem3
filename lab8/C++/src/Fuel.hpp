#pragma once
#include "Resource.hpp"

class SupplyRequestDetail;

class Fuel : public Resource {
public:
    Fuel(const Fuel&);
    Fuel(int, const std::string&, FuelType, float);
    Fuel& operator = (const Resource& base);
    FuelType GetType() const;
    bool IsMatches(const SupplyRequestDetail&) const override;
private:
    FuelType m_type;
};