#pragma once
#include "Enums.hpp"

class SupplyRequestDetail {

public:
    SupplyRequestDetail(int, SupplyType, float);
    SupplyRequestDetail(int, SupplyType, float, Caliber);
    SupplyRequestDetail(int, SupplyType, float, FuelType);
    SupplyRequestDetail WithCaliber(Caliber);
    SupplyRequestDetail WithFuelType(FuelType);
    int GetId() const;
    SupplyType GetSupplyType() const;
    Caliber GetCaliber() const;
    VehicleType GetVehicleType() const;
    FuelType GetFuelType() const;
    float GetCount() const;
    void SetCount(float);
private:
    int m_id;
    SupplyType m_supplyType;
    Caliber m_caliber;
    VehicleType m_vehicleType;
    FuelType m_fuelType;
    float m_count;
};
