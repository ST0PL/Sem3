#include <iostream>
#include <memory>
#include "WarehouseRepository.hpp"
#include "Ammunition.hpp"
#include "Fuel.hpp"
#include "Weapon.hpp"
#include "Vehicle.hpp"

using namespace std;

int main() {
    setlocale(LC_ALL, "Rus");
    return 0;
}

template<typename T>
T CalculateRequirement(T count, T perUnit) {
    static_assert(std::is_arithmetic_v<T>);
    return count * perUnit;
}
