#include <iostream>
#include <memory>
#include <vector>
#include "Unit.hpp"
#include "Staff.hpp"
#include "SupplyRequestDetail.hpp"
#include "SupplyResponse.hpp"
#include "Ammunition.hpp"
#include "Fuel.hpp"
#include "Vehicle.hpp"
#include "Weapon.hpp"

using namespace std;

int main()
{
    setlocale(LC_ALL, "Rus");
    srand(time(0)); // для генерации идентификаторов   

    cout << "\nДемонстрация алгоритма снабжения\n" << endl;

    shared_ptr<Unit> army = make_shared<Unit>(100, "1-я Армия", UnitType::eArmy);
    shared_ptr<Unit> division = make_shared<Unit>(101, "2-я Мотострелковая Дивизия", UnitType::eDivision);

    // Склады для армии и дивизии
    shared_ptr<Warehouse> armyWarehouse = make_shared<Warehouse>(100, "Армейский склад", WarehouseType::eRear);
    shared_ptr<Warehouse> divisionWarehouse = make_shared<Warehouse>(101, "Дивизионный склад", WarehouseType::eField);

    // Наполнение склада дивизии
    vector<unique_ptr<Resource>> divisionResources;
    divisionResources.emplace_back(make_unique<Ammunition>(200, "Патроны 5.45мм", Caliber::e545mm, 100));
    divisionWarehouse->AddResources(divisionResources);

    // Наполнение склада дивизии
    vector<unique_ptr<Resource>> armyResources;
    armyResources.emplace_back(make_unique<Ammunition>(100, "Патроны 5.45мм", Caliber::e545mm, 2000));
    armyWarehouse->AddResources(armyResources);

    // Связываем подразделения со складами
    army->AssignWarehouse(armyWarehouse);
    division->AssignWarehouse(divisionWarehouse);

    // Связываем сами подразделения
    army->AddChildUnit(division);

    cout << "Создана иерархия: " << army->GetName() << " -> " << division->GetName() << endl;

    // Дивизия запрашивает больше патронов, чем есть на её складе
    cout << "\nДивизия запрашивает 2200 патронов (на её складе только 100, на армейском 2000):" << endl;
    vector<unique_ptr<SupplyRequestDetail>> requestDetails;
    unique_ptr<SupplyRequestDetail> detail = make_unique<SupplyRequestDetail>(1, SupplyType::Ammunition, 2200);
    detail->WithCaliber(Caliber::e545mm);
    requestDetails.emplace_back(move(detail));

    SupplyRequest request = division->CreateRequest(requestDetails);
    SupplyRequest request2 = SupplyRequest(request);
    SupplyResponse response = division->MakeSupplyRequest(request);

    //cout << "Результат:" << endl;
    //cout << "  Статус: " << SupplyResponse::StatusToString(response.GetStatus()) << endl;
    //cout << "  Комментарий: " << response.GetComment() << endl;

}

