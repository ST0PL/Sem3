#include "SupplyRequest.hpp"
#include "SupplyRequestDetail.hpp"
#include "Unit.hpp"


SupplyRequest::SupplyRequest(int id, const Unit* requestUnit, std::vector<std::unique_ptr<SupplyRequestDetail>> details)
    : m_id(id), m_details(std::move(details))
{
    m_createTime = std::chrono::system_clock::now();
    if (requestUnit) {
        m_requestUnitId = requestUnit->GetId();
        m_requestUnit = requestUnit;
    }
}

int SupplyRequest::GetId() const {
    return m_id;
}

int SupplyRequest::GetRequestUnitId() const {
    return m_requestUnitId;
}

const Unit* SupplyRequest::GetRequestUnit() const {
    return m_requestUnit;
}

void SupplyRequest::SetRequestUnit(const Unit* unit) {
    if (unit) {
        m_requestUnitId = unit->GetId();
        m_requestUnit = unit;
    }
}

std::vector<std::unique_ptr<SupplyRequestDetail>>& SupplyRequest::GetDetails() {
    return m_details;
}

std::chrono::system_clock::time_point SupplyRequest::GetCreateTime() const {
    return m_createTime;
}