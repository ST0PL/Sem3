#include "WarehouseEntry.hpp"
#include "Warehouse.hpp"

WarehouseEntry::WarehouseEntry(int id, std::string name)
    : m_id(id), m_name(name) {
}

const std::string& WarehouseEntry::GetName() const {
    return m_name;
}

void WarehouseEntry::SetName(std::string name) {
    m_name = name;
}

int WarehouseEntry::GetId() const {
    return m_id;
}

void WarehouseEntry::SetId(int id) {
    m_id = id;
}

void WarehouseEntry::AssignWarehouse(const Warehouse* warehouse) {
    if (warehouse) {
        m_assignedWarehouseId = warehouse->GetId();
        m_assignedWarehouse = warehouse;
    }
}