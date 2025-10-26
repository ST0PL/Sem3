#pragma once
#include "Resource.hpp"
#include "Enums.hpp"

class SupplyRequestDetail;

class Ammunition : public Resource {

public:
    Ammunition(const Ammunition&);
    Ammunition(int, const std::string&, Caliber caliber, float);
    Caliber GetCaliber() const;
    bool IsMatches(const SupplyRequestDetail&) const override;
private:
    Caliber m_caliber;
};