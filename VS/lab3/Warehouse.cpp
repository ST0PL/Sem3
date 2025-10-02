#include "Warehouse.hpp"
#include "SupplyRequestDetail.hpp"
#include "Ammunition.hpp"
#include "Fuel.hpp"
#include <algorithm>


Warehouse::Warehouse(int id, std::string name, WarehouseType type) :
    m_id(id), m_name(name), m_type(type) {
}

int Warehouse::GetId() const {
    return m_id;
}

WarehouseType Warehouse::GetType() const {
    return m_type;
}

const std::string Warehouse::GetName() const {
    return m_name;
}

void Warehouse::SetName(std::string name) {
    m_name = name;
}

void Warehouse::AddResource(std::unique_ptr<Resource> resource) {
    m_resources.push_back(std::move(resource));
}

void Warehouse::AddResources(std::vector<std::unique_ptr<Resource>>& resources) {
    for (auto& resource : resources) {
        AddResource(std::move(resource));
    }
}

void Warehouse::AddEquipment(std::unique_ptr<Equipment> equipment) {
    m_equipments.push_back(std::move(equipment));
}

void Warehouse::AddEquipment(std::vector<std::unique_ptr<Equipment>>& equipment) {
    for (auto& eq : equipment) {
        AddEquipment(std::move(eq));
    }
}

void Warehouse::ProcessSupplyRequestDetails(std::vector<std::unique_ptr<SupplyRequestDetail>>& details) {

    if (details.empty())
        return;

    for (auto& detail : details) {
        switch (detail->GetSupplyType()) {

        case SupplyType::Ammunition:
        case SupplyType::Fuel:
            WriteOff<Resource>(m_resources, detail.get());
            break;

        case SupplyType::Weapon:
        case SupplyType::Vehicle:
            WriteOff<Equipment>(m_equipments, detail.get());
            break;
        }
    }
    RemoveEmptyEntries(m_resources);
    RemoveEmptyEntries(m_equipments);
    RemoveEmptyRequestDetails(details);
}

template<typename T>
void Warehouse::WriteOff(std::vector<std::unique_ptr<T>>& collection, SupplyRequestDetail* detail) {

    float remaining = detail->GetCount();

    for (auto& item : collection) {

        if (item->IsMatches(detail)) {

            remaining -= item->Decrease(remaining);

            if (remaining == 0.0f)
                break;
        }
    }
    detail->SetCount(remaining);
}

template<typename T>
void Warehouse::RemoveEmptyEntries(std::vector<std::unique_ptr<T>>& pointers) {

    auto newEnd = std::remove_if(pointers.begin(), pointers.end(), [](const std::unique_ptr<T>& pointer)
        {
            return pointer == nullptr || pointer->IsEmpty();
        });

    pointers.erase(newEnd, pointers.end());
}

void Warehouse::RemoveEmptyRequestDetails(std::vector<std::unique_ptr<SupplyRequestDetail>>& pointers) {

    auto newEnd = std::remove_if(pointers.begin(), pointers.end(), [](const std::unique_ptr<SupplyRequestDetail>& pointer)
        {
            return pointer == nullptr || pointer->GetCount() == 0.0f;
        });

    pointers.erase(newEnd, pointers.end());
}