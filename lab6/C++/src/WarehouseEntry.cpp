#include "WarehouseEntry.hpp"
#include "Warehouse.hpp"

template<typename T>
WarehouseEntry<T>::WarehouseEntry(int id, const std::string& name, MaterialType materialType)
    : m_id(id), m_name(name), m_materialType(materialType)  {
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
MaterialType WarehouseEntry<T>::GetMaterialType() const {
    return m_materialType;
}

template<typename T>
void WarehouseEntry<T>::SetMaterialType(MaterialType materialType) {
    m_materialType = materialType;
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
void WarehouseEntry<T>::AssignWarehouse(const std::weak_ptr<const Warehouse>& warehouse) {
    if (auto w = warehouse.lock()) {
        m_assignedWarehouseId = w->GetId();
        m_assignedWarehouse = warehouse;
    }
}
template<typename T>
std::weak_ptr<const Warehouse> WarehouseEntry<T>::GetWarehouse() const {
    return m_assignedWarehouse;
}