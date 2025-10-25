#pragma once
#include "Equipment.hpp"
#include "Enums.hpp"

class Weapon : public Equipment
{
public:
    Weapon(int, const std::string&, Caliber, int);
    Caliber GetCaliber() const;
    bool IsMatches(std::unique_ptr<SupplyRequestDetail>&) const override;
private:
    Caliber m_caliber;
};

