#pragma once
#include "Enums.hpp"
#include <memory>
#include <vector>
#include <string>

class SupplyRequestDetail {

public:
    SupplyRequestDetail(int, SupplyType, float);
    SupplyRequestDetail& WithCaliber(Caliber);
    SupplyRequestDetail& WithFuelType(FuelType);
    SupplyRequestDetail& WithVehicleType(VehicleType);
    int GetId() const;
    SupplyType GetSupplyType() const;
    Caliber GetCaliber() const;
    VehicleType GetVehicleType() const;
    FuelType GetFuelType() const;
    float GetCount() const;
    void SetCount(float);
    static std::string ToString(std::vector<std::unique_ptr<SupplyRequestDetail>>&);
    std::string ToString() const;
    static std::string SupplyTypeToString(SupplyType);
    static std::string CaliberToString(Caliber);
    static std::string FuelTypeToString(FuelType);
    static std::string VehicleTypeToString(VehicleType);
private:
    int m_id;
    SupplyType m_supplyType;
    Caliber m_caliber;
    VehicleType m_vehicleType;
    FuelType m_fuelType;
    float m_count;
};
