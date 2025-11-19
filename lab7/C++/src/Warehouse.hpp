#pragma once
#include <string>
#include <vector>
#include <memory>
#include "Resource.hpp"
#include "Equipment.hpp"
#include "SupplyRequest.hpp"
#include "Enums.hpp"

class Warehouse {

private:
    int m_id;
    WarehouseType m_type;
    std::string m_name;
    std::vector<std::unique_ptr<Resource>> m_resources;
    std::vector<std::unique_ptr<Equipment>> m_equipments;
    template<typename T> void WriteOff(const std::vector<std::unique_ptr<T>>&, SupplyRequestDetail&, bool&);
    template<typename T> void RemoveEmptyEntries(std::vector<std::unique_ptr<T>>&);
    void RemoveEmptyRequestDetails(std::vector<std::unique_ptr<SupplyRequestDetail>>&);
public:
    Warehouse(const Warehouse&) = delete;
    Warehouse(int, const std::string&, WarehouseType);
    int GetId() const;
    WarehouseType GetType() const;
    const std::string& GetName() const;
    void SetName(const std::string&);
    void AddResource(std::unique_ptr<Resource>);
    void AddResources(std::vector<std::unique_ptr<Resource>>&);
    void AddEquipment(std::unique_ptr<Equipment>);
    void AddEquipment(std::vector<std::unique_ptr<Equipment>>&);
    bool ProcessSupplyRequestDetails(std::vector<std::unique_ptr<SupplyRequestDetail>>&);
};