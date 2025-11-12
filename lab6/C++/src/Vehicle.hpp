#pragma once
#include <string>
#include "Equipment.hpp"
#include "Enums.hpp"

class Vehicle : public Equipment {
public:
    Vehicle(const Vehicle&);
    Vehicle(int, const std::string&, VehicleType, FuelType, int);
    VehicleType GetType() const;
    FuelType GetFuelType() const;
    bool IsMatches(const SupplyRequestDetail&) const override;
    Vehicle& operator = (const Equipment& base);
private:
    VehicleType m_type;
    FuelType m_fuelType;
};