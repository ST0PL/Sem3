#include "Unit.hpp"
#include "Staff.hpp"
#include "SupplyRequestDetail.hpp"
#include "SupplyResponse.hpp"

Unit::Unit(int id, const std::string& name, UnitType unitType) :
    m_id(id), m_name(name), m_type(unitType) { }

UnitType Unit::GetType() const {
    return m_type;
}

void Unit::SetId(int id) {
    m_id = id;
}
int Unit::GetId() const {
    return m_id;
}

std::string Unit::GetName() const {
    return m_name;
}

void Unit::SetParent(Unit* parent) {
    if (parent) {
        m_parentId = parent->m_id;
        m_parent = parent;
    }
}

SupplyResponse Unit::MakeSupplyRequest(SupplyRequest request) {

    // Заглушка
    return SupplyResponse(SupplyResponseStatus::Success, "");
}


void Unit::AddChildUnit(const Unit* unit) {
    if (unit && (unit->GetType() < m_type))
        m_children.push_back(unit);
}

void Unit::AddChildUnits(std::vector<const Unit*> units) {
    for (const Unit* unit : units)
        m_children.push_back(unit);
}

void Unit::AssignWarehouse(Warehouse* warehouse) {
    if (warehouse) {
        m_assignedWarehouseId = warehouse->GetId();
        m_assignedWarehouse = warehouse;
    }
}

void Unit::AssignCommander(const Staff* commander) {
    if (commander) {
        m_commanderId = commander->GetId();
        m_commander = commander;
    }
}

bool Unit::RemoveChildUnit(int id) {
    int prevSize = m_children.size();

    auto new_end = std::remove_if(m_children.begin(), m_children.end(), [&id](const Unit* unit)
        {
            if (unit)
                return unit->GetId() == id;

            return true;
        });
    m_children.erase(new_end, m_children.end());

    return m_children.size() <= prevSize;
}

SupplyRequest Unit::CreateRequest(std::vector<std::unique_ptr<SupplyRequestDetail>>& details) const {
    return SupplyRequest(rand(), this, std::move(details));
}