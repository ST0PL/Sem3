#pragma once
#include "WarehouseEntry.hpp" 

class Equipment : public WarehouseEntry<int> {
public:
    static constexpr int MIN_COUNT = 1;
    Equipment(const Equipment&) = delete;
    Equipment(int, MaterialType, const std::string&, int);
    void Increase(int) override;
    int Decrease(int) override;
    int GetCount() const;
    bool IsEmpty() const override;
    ~Equipment() = default;
protected:
    int m_count;
};