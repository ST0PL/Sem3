#pragma once
#include "WarehouseEntry.hpp" 

class Equipment : public WarehouseEntry<int> {
protected:
    int m_count;
public:
    Equipment(int, const std::string&, int);
    void Increase(int) override;
    int Decrease(int) override;
    int GetCount() const;
    bool IsEmpty() const override;
    ~Equipment() = default;
};