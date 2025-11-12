#pragma once
#include "Equipment.hpp"
#include "Enums.hpp"

class Weapon : public Equipment
{
public:
    Weapon(const Weapon&);
    Weapon(int, const std::string&, Caliber, int);
    Caliber GetCaliber() const;
    bool IsMatches(const SupplyRequestDetail&) const override;
    Weapon& operator = (const Equipment& base);
private:
    Caliber m_caliber;
};

