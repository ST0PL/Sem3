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
    srand(time(0)); // для генерации идентификаторов запросов

    cout << "Статическая инициализация объектов\n" << endl;

    Unit static_army(1, "1-ая армия", UnitType::eArmy);
    Unit static_division(2, "1-ая стрелковая дивизия", UnitType::eDivision);

    // Статические объекты Staff
    Staff staff1(1, "Иванов Иван Иванович", Rank::eArmyGeneral, Speciality::eTanker);
    Staff staff2(2, "Петров Павел Павлович", Rank::eColonel, Speciality::eInfantry);

    // Статический вектор
    vector<Staff*> soldiers;
    soldiers.push_back(&staff1);
    soldiers.push_back(&staff2);
    static_division.AddSoldiers(soldiers);

    Warehouse static_warehouse(1, "1-ый полевой склад", WarehouseType::eField);

    cout << "Созданы статические объекты:" << endl;
    cout << "  " << static_army.GetName() << endl;
    cout << "  " << static_division.GetName() << " с " << soldiers.size() << " солдатами" << endl;
    cout << "  " << static_warehouse.GetName() << endl;


    cout << "\nДинамическая инициализация объектов\n" << endl;


    Unit* dynamic_army = new Unit(3, "2-ая динамическая армия", UnitType::eArmy);
    Unit* dynamic_division = new Unit(4, "2-ая динамическая дивизия", UnitType::eDivision);

    Staff* dynamic_staff1 = new Staff(3, "Сидоров Сергей Сергеевич", Rank::eMajor, Speciality::eSniper);
    Staff* dynamic_staff2 = new Staff(4, "Козлов Константин Константинович", Rank::eCaptain, Speciality::eEngineer);

    cout << "Созданы динамические объекты:" << endl;
    cout << "  " << dynamic_army->GetName() << endl;
    cout << "  " << dynamic_division->GetName() << endl;
    cout << "  " << dynamic_staff1->GetFullName() << endl;
    cout << "  " << dynamic_staff2->GetFullName() << endl;

    // Освобождение памяти
    delete dynamic_army;
    delete dynamic_division;
    delete dynamic_staff1;
    delete dynamic_staff2;
    cout << "Динамические объекты удалены" << endl;


    cout << "\nОператоры работы по ссылкам и указателям\n" << endl;


    // Создание ссылки
    Unit& divisionLink = static_division;
    divisionLink.SetId(100);
    cout << "Работа через ссылку:" << endl;
    cout << "  Название: " << divisionLink.GetName() << endl;
    cout << "  ID: " << divisionLink.GetId() << endl;

    // Создание указателя
    Unit* divisionPtr = &static_division;
    divisionPtr->SetId(101);
    divisionPtr->AssignCommander(&staff1);

    cout << "Работа через указатель:" << endl;
    cout << "  Название: " << divisionPtr->GetName() << endl;
    cout << "  ID: " << divisionPtr->GetId() << endl;
    cout << "  Командир: " << staff1.GetFullName() << endl;

    cout << "\nДинамический массив объектов\n" << endl;

    const int warehouseCount = 3;
    Warehouse* warehouseArray = new Warehouse[warehouseCount]{
        Warehouse(10, "Склад А", WarehouseType::eRear),
        Warehouse(11, "Склад Б", WarehouseType::eField),
        Warehouse(12, "Склад В", WarehouseType::eField)
    };

    cout << "Динамический массив складов:" << endl;
    for (int i = 0; i < warehouseCount; i++) {
        cout << "  " << warehouseArray[i].GetName() << " (ID: " << warehouseArray[i].GetId() << ")" << endl;
    }

    delete[] warehouseArray;
    cout << "Массив удален" << endl;


    cout << "\nМассив динамических объектов:\n" << endl;

    const int unitCount = 2;
    Unit* unitPtrs[unitCount];

    unitPtrs[0] = new Unit(20, "Танковая бригада", UnitType::eBrigade);
    unitPtrs[1] = new Unit(21, "Артиллерийский полк", UnitType::eRegiment);

    Staff* staffPtrs[unitCount];
    staffPtrs[0] = new Staff(30, "Николаев Александр Васильевич", Rank::eColonel, Speciality::eTanker);
    staffPtrs[1] = new Staff(31, "Иванов Иван Иванович", Rank::eLieutenantColonel, Speciality::eAntiAircraft);


    for (int i = 0; i < unitCount; i++) {
        cout << "  " << unitPtrs[i]->GetName() << " - " << staffPtrs[i]->GetFullName() << endl;
    }

    // Освобождение памяти
    for (int i = 0; i < unitCount; i++) {
        delete unitPtrs[i];
        delete staffPtrs[i];
    }
    cout << "Динамические объекты удалены" << endl;


    cout << "\nДемонстрация алгоритма снабжения\n" << endl;

    Unit army(100, "1-я Армия", UnitType::eArmy);
    Unit division(101, "2-я Мотострелковая Дивизия", UnitType::eDivision);

    // Склады для армии и дивизии
    Warehouse armyWarehouse(100, "Армейский склад", WarehouseType::eRear);
    Warehouse divisionWarehouse(101, "Дивизионный склад", WarehouseType::eField);

    // Наполнение склада дивизии
    vector<unique_ptr<Resource>> divisionResources;
    divisionResources.emplace_back(make_unique<Ammunition>(200, "Патроны 5.45мм", Caliber::e545mm, 100));
    divisionWarehouse.AddResources(divisionResources);

    // Наполнение склада дивизии
    vector<unique_ptr<Resource>> armyResources;
    armyResources.emplace_back(make_unique<Ammunition>(100, "Патроны 5.45мм", Caliber::e545mm, 2000));
    armyWarehouse.AddResources(armyResources);

    // Связываем подразделения со складами
    army.AssignWarehouse(&armyWarehouse);
    division.AssignWarehouse(&divisionWarehouse);

    // Связываем сами подразделения
    army.AddChildUnit(&division);

    cout << "Создана иерархия: " << army.GetName() << " -> " << division.GetName() << endl;

    // Дивизия запрашивает больше патронов, чем есть на её складе
    cout << "\nДивизия запрашивает 2200 патронов (на её складе только 100, на армейском 2000):" << endl;
    vector<unique_ptr<SupplyRequestDetail>> requestDetails;
    unique_ptr<SupplyRequestDetail> detail = make_unique<SupplyRequestDetail>(1, SupplyType::Ammunition, 2200);
    detail->WithCaliber(Caliber::e545mm);
    requestDetails.emplace_back(move(detail));

    SupplyRequest request = division.CreateRequest(requestDetails);
    SupplyResponse response = division.MakeSupplyRequest(request);

    cout << "Результат:" << endl;
    cout << "  Статус: " << SupplyResponse::StatusToString(response.GetStatus()) << endl;
    cout << "  Комментарий: " << response.GetComment() << endl;

}

