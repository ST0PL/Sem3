#pragma once
#include "Resource.hpp"

class SupplyRequestDetail;

class Ammunition : public Resource {

public:
    Ammunition(const Ammunition&);
    Ammunition(int, const std::string&, Caliber, float);
    Caliber GetCaliber() const;
    bool IsMatches(const SupplyRequestDetail&) const override;
    Ammunition& operator = (const Resource&);
private:
    Caliber m_caliber;
};