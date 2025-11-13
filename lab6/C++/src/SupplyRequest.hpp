#pragma once
#include <memory>
#include <vector>
#include <chrono>
#include "IClonable.hpp"

class Unit;
class SupplyRequestDetail;

class SupplyRequest : public IClonable<SupplyRequest> {
public:
    SupplyRequest(const SupplyRequest&);
    SupplyRequest(int, const std::weak_ptr<const Unit>, std::vector<std::unique_ptr<SupplyRequestDetail>>&);
    SupplyRequest(int id, const std::weak_ptr<const Unit> unit, std::chrono::system_clock::time_point);
    int GetId() const;
    int GetRequestUnitId() const;
    const std::weak_ptr<const Unit> GetRequestUnit() const;
    void SetRequestUnit(const std::weak_ptr<const Unit>);
    std::vector<std::unique_ptr<SupplyRequestDetail>>& GetDetails();
    std::chrono::system_clock::time_point GetCreateTime() const;
    std::unique_ptr<SupplyRequest> Clone(bool = false) const override;
private:
    int m_id;
    int m_requestUnitId;
    std::weak_ptr<const Unit> m_requestUnit;
    std::vector<std::unique_ptr<SupplyRequestDetail>> m_details;
    std::chrono::system_clock::time_point m_createTime;
};
