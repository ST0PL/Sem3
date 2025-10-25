#pragma once
#include <string>
#include "Equipment.hpp"
#include "Enums.hpp"

class Vehicle : public Equipment {
public:
    Vehicle(int, const std::string&, VehicleType, FuelType, int);
    VehicleType GetType() const;
    FuelType GetFuelType() const;
    bool IsMatches(std::unique_ptr<SupplyRequestDetail>&) const override;
private:
    VehicleType m_type;
    FuelType m_fuelType;
};