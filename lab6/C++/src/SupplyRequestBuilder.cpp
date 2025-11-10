#include "SupplyRequestBuilder.hpp"
#include "SupplyRequest.hpp"
#include "SupplyRequestDetail.hpp"
#include "Unit.hpp"


SupplyRequestBuilder& SupplyRequestBuilder::WithVehicle(VehicleType type, FuelType fuel, float count) {
    std::unique_ptr<SupplyRequestDetail> detail = std::make_unique<SupplyRequestDetail>(rand(), MaterialType::Vehicle, count);
    detail->WithVehicleType(type);
    detail->WithFuelType(fuel);
    m_details.push_back(std::move(detail));
    return *this;
}

SupplyRequestBuilder& SupplyRequestBuilder::WithFuel(FuelType fuel, float count) {
    std::unique_ptr<SupplyRequestDetail> detail = std::make_unique<SupplyRequestDetail>(rand(), MaterialType::Fuel, count);
    detail->WithFuelType(fuel);
    m_details.push_back(std::move(detail));
    return *this;
}

SupplyRequestBuilder& SupplyRequestBuilder::WithAmmunition(Caliber caliber, float count) {
    std::unique_ptr<SupplyRequestDetail> detail = std::make_unique<SupplyRequestDetail>(rand(), MaterialType::Ammunition, count);
    detail->WithCaliber(caliber);
    m_details.push_back(std::move(detail));
    return *this;
}

SupplyRequestBuilder& SupplyRequestBuilder::WithWeapon(Caliber caliber, float count) {
    std::unique_ptr<SupplyRequestDetail> detail = std::make_unique<SupplyRequestDetail>(rand(), MaterialType::Weapon, count);
    detail->WithCaliber(caliber);
    m_details.push_back(std::move(detail));
    return *this;
}

std::shared_ptr<SupplyRequest> SupplyRequestBuilder::Create(std::weak_ptr<const Unit> unit) {
    return std::make_shared<SupplyRequest>(rand(), unit, m_details);
}

