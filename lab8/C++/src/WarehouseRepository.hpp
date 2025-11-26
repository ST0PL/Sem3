#pragma once
#include <type_traits>
#include <vector>
#include <memory>
#include <unordered_map>
#include "WarehouseEntry.hpp"
#include "Ammunition.hpp"
#include "Fuel.hpp"
#include "Weapon.hpp"
#include "Vehicle.hpp"

template<typename T>
class WarehouseRepository {
    static_assert(std::is_base_of_v<WarehouseEntry<int>, T> || std::is_base_of_v<WarehouseEntry<float>, T>);
private:
    std::vector<std::shared_ptr<T>> m_items;

public:
    WarehouseRepository() = default;
    WarehouseRepository(const WarehouseRepository&) = delete;
    WarehouseRepository& operator=(const WarehouseRepository&) = delete;

    size_t GetSize() const;
    bool IsEmpty() const;
    void Clear();   
    void Add(std::unique_ptr<T>);
    bool Remove(int);
    bool ContainsId(int) const;

    std::weak_ptr<T> FindById(int) const;
    std::vector<std::weak_ptr<T>> FindByMaterialType(MaterialType) const;
    std::vector<std::weak_ptr<T>> FindNonEmpty() const;
    std::vector<std::weak_ptr<T>> FindEmpty() const;

    size_t CountByMaterialType(MaterialType) const;

    void AddRange(std::vector<std::unique_ptr<T>>);
    bool RemoveRange(const std::vector<int>&);

    template<typename Predicate>
    std::vector<std::weak_ptr<T>> Find(Predicate predicate) const {
        static_assert(std::is_invocable_r_v<bool, Predicate, const T*>);
        std::vector<std::weak_ptr<T>> result;
        for (const auto& item : m_items) {
            if (predicate(item.get())) {
                result.emplace_back(item);
            }
        }
        return result;
    }
};

template class WarehouseRepository<Ammunition>;
template class WarehouseRepository<Fuel>;
template class WarehouseRepository<Weapon>;
template class WarehouseRepository<Vehicle>;