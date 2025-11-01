#include <algorithm>
#include "Warehouse.hpp"
#include "SupplyRequestDetail.hpp"
#include "Ammunition.hpp"
#include "Fuel.hpp"


Warehouse::Warehouse(int id, const std::string& name, WarehouseType type) :
    m_id(id), m_name(name), m_type(type) {
}

int Warehouse::GetId() const {
    return m_id;
}

WarehouseType Warehouse::GetType() const {
    return m_type;
}

const std::string& Warehouse::GetName() const {
    return m_name;
}

void Warehouse::SetName(const std::string& name) {
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

bool Warehouse::ProcessSupplyRequestDetails(std::vector<std::unique_ptr<SupplyRequestDetail>>& details) {

    if (details.empty())
        return false;

    bool corrected = false;

    for (auto& detail : details) {
        switch (detail->GetSupplyType()) {

        case SupplyType::Ammunition:
        case SupplyType::Fuel:
            WriteOff<Resource>(m_resources, *detail, corrected);
            break;

        case SupplyType::Weapon:
        case SupplyType::Vehicle:
            WriteOff<Equipment>(m_equipments, *detail, corrected);
            break;
        }
    }

    RemoveEmptyEntries(m_resources);
    RemoveEmptyEntries(m_equipments);
    RemoveEmptyRequestDetails(details);

    return corrected;
}

template<typename T>
void Warehouse::WriteOff(const std::vector<std::unique_ptr<T>>& collection, SupplyRequestDetail& detail, bool& corrected) {

    float remaining = detail.GetCount();

    for (auto& item : collection) {

        if (item->IsMatches(detail)) {

            corrected = true;

            remaining -= item->Decrease(remaining);

            if (remaining == 0.0f)
                break;
        }
    }
    detail.SetCount(remaining);
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

    auto newEnd = std::remove_if(pointers.begin(), pointers.end(), []( std::unique_ptr<SupplyRequestDetail>& pointer)
        {
            return pointer == nullptr || pointer->GetCount() == 0.0f;
        });

    pointers.erase(newEnd, pointers.end());
}