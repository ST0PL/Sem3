#pragma once
#include <string>
#include <memory>
#include <type_traits>
#include "Enums.hpp"


class SupplyRequestDetail;
class Warehouse;

template<typename T>
class WarehouseEntry {
    static_assert(std::is_arithmetic_v<T>);
public:
    WarehouseEntry(const WarehouseEntry&) = delete;
    WarehouseEntry(int, const std::string&, MaterialType);
    const std::string& GetName() const;
    void SetName(const std::string&);
    int GetId() const;
    MaterialType GetMaterialType() const;
    void SetMaterialType(MaterialType);
    void SetId(int id);
    void AssignWarehouse(const std::weak_ptr<const Warehouse>&);
    std::weak_ptr<const Warehouse> GetWarehouse() const;
    virtual bool IsMatches(const SupplyRequestDetail&) const = 0;
    virtual bool IsEmpty() const = 0;
    virtual void Increase(T) = 0;
    virtual T Decrease(T) = 0;
    virtual ~WarehouseEntry() = default;

protected:
    int m_assignedWarehouseId;
    std::weak_ptr<const Warehouse> m_assignedWarehouse;
private:
    int m_id;
    std::string m_name;
    MaterialType m_materialType;
};
template class WarehouseEntry<int>;
template class WarehouseEntry<float>;