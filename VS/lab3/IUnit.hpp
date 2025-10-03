#pragma once

class SupplyResponse;
class SupplyRequest;

class IUnit {
public:
    virtual SupplyResponse MakeSupplyRequest(SupplyRequest) = 0;
    ~IUnit() = default;
};
