#pragma once
#include <vector>
#include <memory>
#include <string>
#include "IUnit.hpp"
#include "Warehouse.hpp"

class Staff;

class Unit : public IUnit {
public:
    Unit(int, const std::string&, UnitType);
    int GetId() const;
    const std::string& GetName() const;
    UnitType GetType() const;
    void SetId(int);
    void SetParent(Unit*);
    SupplyResponse MakeSupplyRequest(SupplyRequest&) override;
    void AddChildUnit(Unit*);
    void AddChildUnits(std::vector<Unit*>&);
    void AddSoldier(Staff*);
    void AddSoldiers(std::vector<Staff*>&);
    bool RemoveSoldier(int);
    void AssignWarehouse(Warehouse*);
    void AssignCommander(const Staff*);
    bool RemoveChildUnit(int);
    SupplyRequest CreateRequest(std::vector<std::unique_ptr<SupplyRequestDetail>>&) const;

private:
    int m_id;
    std::string m_name;
    UnitType m_type;
    int m_commanderId;
    const Staff* m_commander;
    int m_parentId;
    Unit* m_parent;
    std::vector<Unit*> m_children;
    std::vector<const Staff*> m_personnel;
    int m_assignedWarehouseId;
    Warehouse* m_assignedWarehouse;
};
