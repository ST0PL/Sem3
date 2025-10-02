#pragma once
#include <memory>
#include <vector>
#include <chrono>

class Unit;
class SupplyRequestDetail;

class SupplyRequest {
public:
    SupplyRequest(int id, const Unit* requestUnit, std::vector<std::unique_ptr<SupplyRequestDetail>> details);
    int GetId() const;
    int GetRequestUnitId() const;
    const Unit* GetRequestUnit() const;
    void SetRequestUnit(const Unit*);
    std::vector<std::unique_ptr<SupplyRequestDetail>>& GetDetails();
    std::chrono::system_clock::time_point GetCreateTime() const;
private:
    int m_id;
    int m_requestUnitId;
    const Unit* m_requestUnit;
    std::vector<std::unique_ptr<SupplyRequestDetail>> m_details;
    std::chrono::system_clock::time_point m_createTime;
};
