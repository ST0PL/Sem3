#pragma once
#include <vector>
#include <memory>
#include <string>
#include "IUnit.hpp"
#include "Warehouse.hpp"

class Staff;

class Unit : public IUnit, public std::enable_shared_from_this<Unit> {
public:
    Unit(int, const std::string&, UnitType);
    int GetId() const;
    const std::string& GetName() const;
    UnitType GetType() const;
    void SetId(int);
    void SetParent(const std::weak_ptr<Unit>);
    SupplyResponse MakeSupplyRequest(SupplyRequest&) override;
    void AddChildUnit(const std::weak_ptr<Unit>);
    void AddChildUnits(const std::vector<std::weak_ptr<Unit>>&);
    void AddSoldier(const std::weak_ptr<Staff>);
    void AddSoldiers(const std::vector<std::weak_ptr<Staff>>&);
    bool RemoveSoldier(int);
    void AssignWarehouse(std::weak_ptr<Warehouse>);
    void AssignCommander(const std::weak_ptr<Staff>);
    bool RemoveChildUnit(int);
    SupplyRequest CreateRequest(std::vector<std::unique_ptr<SupplyRequestDetail>>&) const;

private:
    int m_id;
    std::string m_name;
    UnitType m_type;
    int m_commanderId;
    std::weak_ptr<Staff> m_commander;
    int m_parentId;
    std::weak_ptr<Unit> m_parent;
    std::vector<std::weak_ptr<Unit>> m_children;
    std::vector<std::weak_ptr<Staff>> m_personnel;
    int m_assignedWarehouseId;
    std::weak_ptr<Warehouse> m_assignedWarehouse;
};
