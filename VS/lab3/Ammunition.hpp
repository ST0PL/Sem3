#pragma once
#include "Resource.hpp"
#include "Enums.hpp"

class SupplyRequestDetail;

class Ammunition : public Resource {

public:
    Ammunition(int, std::string, Caliber caliber, float);
    Caliber GetCaliber() const;
    bool IsMatches(SupplyRequestDetail*) const override;
private:
    Caliber m_caliber;
};