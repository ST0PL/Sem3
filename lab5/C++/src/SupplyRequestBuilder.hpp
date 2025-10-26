#pragma once
#include <memory>
#include <vector>
#include "Enums.hpp"

class Unit;
class SupplyRequestDetail;
class SupplyRequest;

class SupplyRequestBuilder {
public:
    SupplyRequestBuilder& WithUnit(std::weak_ptr<const Unit> unit);
    SupplyRequestBuilder& WithWeapon(Caliber caliber, float count);
    SupplyRequestBuilder& WithAmmunition(Caliber caliber, float count);
    SupplyRequestBuilder& WithFuel(FuelType type, float liters);
    SupplyRequestBuilder& WithVehicle(VehicleType type, FuelType fuel, float count);
    std::unique_ptr<SupplyRequest> Create();

private:
    std::weak_ptr<const Unit> m_requestUnit;
    std::vector<std::unique_ptr<SupplyRequestDetail>> m_details;
};