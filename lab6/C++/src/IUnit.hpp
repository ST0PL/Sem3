#pragma once

class SupplyResponse;
class SupplyRequest;

class IUnit {
public:
    virtual SupplyResponse MakeSupplyRequest(std::shared_ptr<SupplyRequest>&) = 0;
    ~IUnit() = default;
};
