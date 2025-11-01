#include "WarehouseEntry.hpp"
#include "Warehouse.hpp"

template<typename T>
WarehouseEntry<T>::WarehouseEntry(int id, const std::string& name)
    : m_id(id), m_name(name) {
}

template<typename T>
const std::string& WarehouseEntry<T>::GetName() const {
    return m_name;
}

template<typename T>
void WarehouseEntry<T>::SetName(const std::string& name) {
    m_name = name;
}

template<typename T>
int WarehouseEntry<T>::GetId() const {
    return m_id;
}

template<typename T>
void WarehouseEntry<T>::SetId(int id) {
    m_id = id;
}

template<typename T>
void WarehouseEntry<T>::AssignWarehouse(std::weak_ptr<const Warehouse>& warehouse) {
    if (auto w = warehouse.lock()) {
        m_assignedWarehouseId = w->GetId();
        m_assignedWarehouse = warehouse;
    }
}