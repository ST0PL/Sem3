#pragma once
#include "WarehouseEntry.hpp" 

class Equipment : public WarehouseEntry {
protected:
    int m_count;
public:
    Equipment(int, std::string, int);
    void Increase(int);
    int Decrease(int);
    int GetCount() const;
    ~Equipment() = default;
};