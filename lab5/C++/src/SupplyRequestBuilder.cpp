#include "SupplyRequestBuilder.hpp"
#include "SupplyRequest.hpp"
#include "SupplyRequestDetail.hpp"
#include "Unit.hpp"


SupplyRequestBuilder& SupplyRequestBuilder::WithVehicle(VehicleType type, FuelType fuel, float count) {
    std::unique_ptr<SupplyRequestDetail> detail = std::make_unique<SupplyRequestDetail>(rand(), SupplyType::Vehicle, count);
    detail->WithVehicleType(type);
    detail->WithFuelType(fuel);
    m_details.push_back(std::move(detail));
    return *this;
}

SupplyRequestBuilder& SupplyRequestBuilder::WithFuel(FuelType fuel, float count) {
    std::unique_ptr<SupplyRequestDetail> detail = std::make_unique<SupplyRequestDetail>(rand(), SupplyType::Fuel, count);
    detail->WithFuelType(fuel);
    m_details.push_back(std::move(detail));
    return *this;
}

SupplyRequestBuilder& SupplyRequestBuilder::WithAmmunition(Caliber caliber, float count) {
    std::unique_ptr<SupplyRequestDetail> detail = std::make_unique<SupplyRequestDetail>(rand(), SupplyType::Ammunition, count);
    detail->WithCaliber(caliber);
    m_details.push_back(std::move(detail));
    return *this;
}

SupplyRequestBuilder& SupplyRequestBuilder::WithWeapon(Caliber caliber, float count) {
    std::unique_ptr<SupplyRequestDetail> detail = std::make_unique<SupplyRequestDetail>(rand(), SupplyType::Weapon, count);
    detail->WithCaliber(caliber);
    m_details.push_back(std::move(detail));
    return *this;
}

std::unique_ptr<SupplyRequest> SupplyRequestBuilder::Create() {
    return std::unique_ptr<SupplyRequest>(new SupplyRequest(rand(), m_requestUnit, m_details));
}

