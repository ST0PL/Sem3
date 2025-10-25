#pragma once
#include <memory>
#include <vector>
#include <chrono>

class Unit;
class SupplyRequestDetail;

class SupplyRequest {
public:
    SupplyRequest(const SupplyRequest&);
    SupplyRequest(int, const std::weak_ptr<const Unit>, std::vector<std::unique_ptr<SupplyRequestDetail>>&);
    int GetId() const;
    int GetRequestUnitId() const;
    const std::weak_ptr<const Unit> GetRequestUnit() const;
    void SetRequestUnit(const std::weak_ptr<const Unit>);
    std::vector<std::unique_ptr<SupplyRequestDetail>>& GetDetails();
    std::chrono::system_clock::time_point GetCreateTime() const;
private:
    int m_id;
    int m_requestUnitId;
    std::weak_ptr<const Unit> m_requestUnit;
    std::vector<std::unique_ptr<SupplyRequestDetail>> m_details;
    std::chrono::system_clock::time_point m_createTime;
};
