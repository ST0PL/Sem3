#include "Unit.hpp"
#include "Staff.hpp"
#include "SupplyRequestDetail.hpp"
#include "SupplyResponse.hpp"
#include <algorithm>

Unit::Unit(int id, const std::string& name, UnitType unitType) :
    m_id(id), m_name(name), m_type(unitType) {
}

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

SupplyResponse Unit::MakeSupplyRequest(SupplyRequest& request) {
    bool detailsCorreted = m_assignedWarehouse != nullptr ?
        m_assignedWarehouse->ProcessSupplyRequestDetails(request.GetDetails()) : false;

    auto& remainingDetails = request.GetDetails();

    if (remainingDetails.empty())
        return SupplyResponse(SupplyResponseStatus::Success, "");


    if (m_parent != nullptr) {
        auto parentResponse = m_parent->MakeSupplyRequest(request);

        if (parentResponse.GetStatus() == SupplyResponseStatus::Success)
            return SupplyResponse(SupplyResponseStatus::Success, "");

        return detailsCorreted ? SupplyResponse(SupplyResponseStatus::Partial, SupplyRequestDetail::ToString(remainingDetails)) :
            SupplyResponse(SupplyResponseStatus::Denied, "Ни один из складов высших порядков не смог удовлетворить запрос");
    }

    return detailsCorreted ? SupplyResponse(SupplyResponseStatus::Partial, SupplyRequestDetail::ToString(remainingDetails)) :
        SupplyResponse(SupplyResponseStatus::Denied, m_assignedWarehouse == nullptr ? "Нет прикрепленного склада" : "Склад подразделения не смог удовлетворить запрос");
}

void Unit::AddChildUnit(Unit* unit) {
    if (unit && (unit->GetType() < m_type)) {
        unit->SetParent(this);
        m_children.push_back(unit);
    }
}

void Unit::AddChildUnits(std::vector<Unit*>& units) {
    for (Unit* unit : units)
        m_children.push_back(unit);
}

void Unit::AddSoldier(Staff* soldier) {
    m_personnel.push_back(soldier);
}
void Unit::AddSoldiers(std::vector<Staff*>& soldiers) {
    for (auto& soldier : soldiers)
        m_personnel.push_back(soldier);
}
bool Unit::RemoveSoldier(int id) {
    int startSize = m_personnel.size();
    auto newEnd = std::remove_if(m_personnel.begin(), m_personnel.end(), [&id](const Staff* soldier)
        {
            return soldier == nullptr || soldier->GetId() == id;
        });
    m_personnel.erase(newEnd, m_personnel.end());
    return m_personnel.size() < startSize;
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

    auto newEnd = std::remove_if(m_children.begin(), m_children.end(), [&id](const Unit* unit)
        {
            if (unit)
                return unit->GetId() == id;

            return true;
        });

    m_children.erase(newEnd, m_children.end());

    return m_children.size() <= prevSize;
}

SupplyRequest Unit::CreateRequest(std::vector<std::unique_ptr<SupplyRequestDetail>>& details) const {
    return SupplyRequest(rand(), this, std::move(details));
}