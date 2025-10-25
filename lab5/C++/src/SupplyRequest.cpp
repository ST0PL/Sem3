#include "SupplyRequest.hpp"
#include "SupplyRequestDetail.hpp"
#include "Unit.hpp"


SupplyRequest::SupplyRequest(int id, const std::weak_ptr<const Unit> unit, std::vector<std::unique_ptr<SupplyRequestDetail>>& details)
    : m_id(id), m_details(std::move(details))
{
    m_createTime = std::chrono::system_clock::now();
    if (auto requestUnit = unit.lock()) {
        m_requestUnitId = requestUnit->GetId();
        m_requestUnit = unit;
    }
}

int SupplyRequest::GetId() const {
    return m_id;
}

int SupplyRequest::GetRequestUnitId() const {
    return m_requestUnitId;
}

const std::weak_ptr<const Unit> SupplyRequest::GetRequestUnit() const {
    return m_requestUnit;
}

void SupplyRequest::SetRequestUnit(const std::weak_ptr<const Unit> unit) {
    if (auto requestUnit = unit.lock()) {
        m_requestUnitId = requestUnit->GetId();
        m_requestUnit = unit;
    }
}

std::vector<std::unique_ptr<SupplyRequestDetail>>& SupplyRequest::GetDetails() {
    return m_details;
}

std::chrono::system_clock::time_point SupplyRequest::GetCreateTime() const {
    return m_createTime;
}