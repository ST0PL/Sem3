#pragma once
#include <string>
#include <memory>
#include "Enums.hpp"


class SupplyRequestDetail;
class Warehouse;

template<typename T>
class WarehouseEntry {
public:
    WarehouseEntry(const WarehouseEntry&) = delete;
    WarehouseEntry(int, const std::string&, MaterialType);
    const std::string& GetName() const;
    void SetName(const std::string&);
    int GetId() const;
    MaterialType GetMaterialType() const;
    void SetMaterialType(MaterialType);
    void SetId(int id);
    void AssignWarehouse(std::weak_ptr<const Warehouse>&);
    virtual bool IsMatches(const SupplyRequestDetail&) const = 0;
    virtual bool IsEmpty() const = 0;
    virtual void Increase(T) = 0;
    virtual T Decrease(T) = 0;
    virtual ~WarehouseEntry() = default;

private:
    int m_id;
    std::string m_name;
    MaterialType m_materialType;
    int m_assignedWarehouseId;
    std::weak_ptr<const Warehouse> m_assignedWarehouse;
};
template class WarehouseEntry<int>;
template class WarehouseEntry<float>;