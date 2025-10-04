#pragma once
#include <string>
#include <memory>


class SupplyRequestDetail;
class Warehouse;

template<typename T>
class WarehouseEntry {
protected:
    int m_id;
    std::string m_name;
    int m_assignedWarehouseId;
    const Warehouse* m_assignedWarehouse;
public:
    WarehouseEntry(int, std::string);
    const std::string& GetName() const;
    void SetName(std::string);
    int GetId() const;
    void SetId(int id);
    void AssignWarehouse(const Warehouse*);
    virtual bool IsMatches(SupplyRequestDetail*) const = 0;
    virtual bool IsEmpty() const = 0;
    virtual void Increase(T) = 0;
    virtual T Decrease(T) = 0;
    virtual ~WarehouseEntry() = default;
};
template class WarehouseEntry<int>;
template class WarehouseEntry<float>;