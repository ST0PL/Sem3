#include "SupplyRequestDetail.hpp"

SupplyRequestDetail::SupplyRequestDetail(int id, SupplyType type, float count)
    : m_id(id), m_supplyType(type), m_count(count) {
}

SupplyRequestDetail::SupplyRequestDetail(int id, SupplyType type, float count, Caliber caliber)
    : SupplyRequestDetail(id, type, count)
{
    m_caliber = caliber;
}

SupplyRequestDetail::SupplyRequestDetail(int id, SupplyType type, float count, FuelType fuelType)
    : SupplyRequestDetail(id, type, count) {
    m_fuelType = fuelType;
}

SupplyRequestDetail SupplyRequestDetail::WithCaliber(Caliber caliber) {
    return SupplyRequestDetail(this->m_id, this->m_supplyType, this->m_count, caliber);
}

SupplyRequestDetail SupplyRequestDetail::WithFuelType(FuelType fuelType) {
    return SupplyRequestDetail(this->m_id, this->m_supplyType, this->m_count, fuelType);
}

int SupplyRequestDetail::GetId() const {
    return m_id;
}

SupplyType SupplyRequestDetail::GetSupplyType() const {
    return m_supplyType;
}

Caliber SupplyRequestDetail::GetCaliber() const {
    return m_caliber;
}

VehicleType SupplyRequestDetail::GetVehicleType() const {
    return m_vehicleType;
}

FuelType SupplyRequestDetail::GetFuelType() const {
    return m_fuelType;
}

float SupplyRequestDetail::GetCount() const {
    return m_count;
}

void SupplyRequestDetail::SetCount(float count) {
    m_count = count;
}