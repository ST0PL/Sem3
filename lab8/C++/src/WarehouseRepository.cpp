#include "WarehouseRepository.hpp"

template<typename T>
size_t WarehouseRepository<T>::GetSize() const {
    return m_items.size();
}

template<typename T>
bool WarehouseRepository<T>::IsEmpty() const {
    return m_items.empty();
}
template<typename T>
void WarehouseRepository<T>::Clear() {
    m_items.clear();
}

template<typename T>
void WarehouseRepository<T>::Add(std::unique_ptr<T> item) {
    if (!item)
        return;
    m_items.push_back(std::move(item));
}
template<typename T>
bool WarehouseRepository<T>::Remove(int id) {
    size_t oldSize = m_items.size();
    m_items.erase(std::remove_if(m_items.begin(), m_items.end(),
        [id](const auto& item) { return item->GetId() == id; }), m_items.end());

    return m_items.size() < oldSize;
}

template<typename T>
bool WarehouseRepository<T>::ContainsId(int id) const {
    return !FindById(id).expired();
}

template<typename T>
std::weak_ptr<T> WarehouseRepository<T>::FindById(int id) const {
    for (const auto& item : m_items) {
        if (item->GetId() == id) {
            return item;
        }
    }
    return std::weak_ptr<T>();
}

template<typename T>
std::vector<std::weak_ptr<T>> WarehouseRepository<T>::FindByMaterialType(MaterialType type) const {
    return Find([type](const T* item)
        {
            return item->GetMaterialType() == type;
        });
}
template<typename T>
std::vector<std::weak_ptr<T>> WarehouseRepository<T>::FindNonEmpty() const {
    return Find([](const T* item) {
        return !item->IsEmpty();
        });
}
template<typename T>
std::vector<std::weak_ptr<T>> WarehouseRepository<T>::FindEmpty() const {
    return Find([](const T* item) {
        return item->IsEmpty();
        });
}

template<typename T>
size_t WarehouseRepository<T>::CountByMaterialType(MaterialType type) const {
    return std::count_if(m_items.begin(), m_items.end(),
        [type](const auto& item) {
            return item->GetMaterialType() == type;
        });
}
template<typename T>
void WarehouseRepository<T>::AddRange(std::vector<std::unique_ptr<T>> items) {
    for (auto& item : items) {
        Add(std::move(item));
    }
}
template<typename T>
bool WarehouseRepository<T>::RemoveRange(const std::vector<int>& ids) {
    bool removed = false;
    for (int id : ids)
        if (Remove(id))
            removed = true;
    return removed;
}