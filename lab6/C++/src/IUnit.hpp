#pragma once

class SupplyResponse;
class SupplyRequest;

class IUnit {
public:
    IUnit(const IUnit&) = delete;
    IUnit() = default;
    virtual SupplyResponse MakeSupplyRequest(std::shared_ptr<SupplyRequest>&) = 0;
    ~IUnit() = default;
};
