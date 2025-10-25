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

const std::string& Unit::GetName() const {
    return m_name;
}

void Unit::SetParent(const std::weak_ptr<Unit> unit) {
    auto parent = unit.lock();

    if (parent)
        m_parentId = parent->m_id;

    m_parent = unit;
}

SupplyResponse Unit::MakeSupplyRequest(SupplyRequest& request) {
    auto warehouse = m_assignedWarehouse.lock();
    auto parent = m_parent.lock();
    bool detailsCorreted = warehouse != nullptr ?
        warehouse->ProcessSupplyRequestDetails(request.GetDetails()) : false;

    auto& remainingDetails = request.GetDetails();

    if (remainingDetails.empty())
        return SupplyResponse(SupplyResponseStatus::Success, "");


    if (parent != nullptr) {
        auto parentResponse = parent->MakeSupplyRequest(request);

        if (parentResponse.GetStatus() == SupplyResponseStatus::Success)
            return SupplyResponse(SupplyResponseStatus::Success, "");

        return detailsCorreted ? SupplyResponse(SupplyResponseStatus::Partial, SupplyRequestDetail::ToString(remainingDetails)) :
            SupplyResponse(SupplyResponseStatus::Denied, "Ни один из складов высших порядков не смог удовлетворить запрос");
    }

    return detailsCorreted ? SupplyResponse(SupplyResponseStatus::Partial, SupplyRequestDetail::ToString(remainingDetails)) :
        SupplyResponse(SupplyResponseStatus::Denied, warehouse == nullptr ? "Нет прикрепленного склада" : "Склад подразделения не смог удовлетворить запрос");
}

void Unit::AddChildUnit(const std::weak_ptr<Unit> unit) {
    if (auto child = unit.lock()) {
        if (child->GetType() >= this->m_type)
            return;
        child->SetParent(weak_from_this());
        m_children.push_back(unit);
    }
}

void Unit::AddChildUnits(const std::vector<std::weak_ptr<Unit>>& units) {
    for (const auto& unit : units)
        AddChildUnit(unit);
}

void Unit::AddSoldier(const std::weak_ptr<Staff> staff) {
    if (auto soldier = staff.lock()) {
        soldier->SetUnit(weak_from_this());
        m_personnel.push_back(staff);
    }
}
void Unit::AddSoldiers(const std::vector<std::weak_ptr<Staff>>& soldiers) {
    for (const auto& soldier : soldiers)
        AddSoldier(soldier);
}
bool Unit::RemoveSoldier(int id) {
    int startSize = m_personnel.size();
    auto newEnd = std::remove_if(m_personnel.begin(), m_personnel.end(), [&id](const std::weak_ptr<Staff>& staff)
        {
            
            if (auto soldier = staff.lock()) {
                if (soldier->GetId() == id) {
                    soldier->SetUnit(std::weak_ptr<Unit>());
                    return true;
                }
                return false;
            }
            return true;
        });
    m_personnel.erase(newEnd, m_personnel.end());
    return m_personnel.size() < startSize;
}

void Unit::AssignWarehouse(std::weak_ptr<Warehouse> warehouse) {
    if (auto w = warehouse.lock()) {
        m_assignedWarehouseId = w->GetId();
        m_assignedWarehouse = warehouse;
    }
}

void Unit::AssignCommander(const std::weak_ptr<Staff> staff) {
    if (auto commander = staff.lock()) {
        m_commanderId = commander->GetId();
        m_commander = staff;
    }
}

bool Unit::RemoveChildUnit(int id) {

    int prevSize = m_children.size();

    auto newEnd = std::remove_if(m_children.begin(), m_children.end(), [&id](const std::weak_ptr<Unit>& unit)
        {
            if (auto u = unit.lock()) {
                if (u->GetId() == id) {
                    u->SetParent(std::weak_ptr<Unit>());
                    return true;
                }
                return false;
            }

            return true;
        });

    m_children.erase(newEnd, m_children.end());

    return m_children.size() < prevSize;
}

SupplyRequest Unit::CreateRequest(std::vector<std::unique_ptr<SupplyRequestDetail>>& details) const {
    return SupplyRequest(rand(), weak_from_this(), details);
}