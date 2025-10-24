#pragma once
#include "Equipment.hpp"
#include "Enums.hpp"

class Weapon : public Equipment
{
public:
    Weapon(int, std::string, Caliber, int);
    Caliber GetCaliber() const;
    bool IsMatches(SupplyRequestDetail* detail) const override;
private:
    Caliber m_caliber;
};

