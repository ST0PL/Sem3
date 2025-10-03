#include "Equipment.hpp"
#include "WarehouseEntry.hpp"

Equipment::Equipment(int id, std::string name, int count)
    : WarehouseEntry<int>(id, name), m_count(count) {}

int Equipment::GetCount() const {
    return m_count;
}

bool Equipment::IsEmpty() const {
    return m_count <= 0;
}

void Equipment::Increase(int count) {
    m_count += count;
}

int Equipment::Decrease(int count) {
    int taken = std::min(m_count, count);
    m_count -= taken;
    return taken;
}