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

    // Статические константыs

    cout << "MIN_COUNT: " << Equipment::MIN_COUNT << endl;
    cout << "MIN_QUANTITY: " << Resource::MIN_QUANTITY << endl;
    cout << "MIN_COMMANDER_RANK: " << (int)Unit::MIN_COMMANDER_RANK << endl;

    // Обработка исключений

    try {
        // Попытка создать невалидное снаряжение
        Weapon corrupted(rand(), "Оружие", Caliber::e545mm, -1);
    }
    catch (const std::invalid_argument& e) {
        cout << "Перехвачено исключение: " << e.what() << endl;
    }

    auto soldier1 = make_shared<Staff>(1, "Иванов Иван Иванович", Rank::ePrivate, Speciality::eInfantry);
    auto soldier2 = make_shared<Staff>(2, "Петров Петр Сергеевич", Rank::eSergeant, Speciality::eMachineGunner);
    auto soldier3 = make_shared<Staff>(3, "Иванов Алексей Владимирович", Rank::eCaptain, Speciality::eTanker);

    division->AddSoldier(soldier1);
    division->AddSoldier(soldier2);
    division->AddSoldier(soldier3);

    // Поиск по имени
    string searchQuery = "Иванов";
    auto foundSoldiers = division->FindByName(searchQuery);
    for (auto& weakSoldier : foundSoldiers) {
        if (auto soldier = weakSoldier.lock()) {
            cout << "Найден: " << *soldier << endl;
        }
    }

    // Перегрузка операторов сравнения

    cout << "солдат1 < солдат2: " << (*soldier1 < *soldier2) << endl;
    cout << "солдат2 > солдат1: " << (*soldier2 > *soldier3) << endl;

    // Перегрузка арифметических операторов

    SupplyRequestDetail testDetail(999, SupplyType::Ammunition, 100);
    testDetail.WithCaliber(Caliber::e545mm);
    cout << "Исходно: " << testDetail << endl;
    testDetail += 50;
    cout << "После += 50: " << testDetail << endl;
    testDetail -= 30;
    cout << "После -= 30: " << testDetail << endl;

    // Логика снабжения
    cout << "Дивизия запрашивает 2200 патронов (на её складе только 100, на армейском 2000):" << endl;

    SupplyRequestBuilder requestBuilder;
    unique_ptr<SupplyRequest> request = requestBuilder.WithAmmunition(Caliber::e545mm, 2200).Create();
    SupplyResponse response = division->MakeSupplyRequest(*request);

    cout << "Результат:" << endl;
    cout << "  Статус: " << SupplyResponse::StatusToString(response.GetStatus()) << endl;
    cout << "  Комментарий: " << response.GetComment() << endl;

    // Статические методы

    cout << "Ранг капитана: " << Staff::RankToString(Rank::eCaptain) << endl;
    cout << "Специальность танкиста: " << Staff::SpecialityToString(Speciality::eTanker) << endl;
}

// Дружественная функция перегрузки оператора << для SupplyRequestDetail

std::ostream& operator << (std::ostream& stream, const SupplyRequestDetail& detail) {
    stream << detail.ToString();
    return stream;
}

// Дружественная функция перегрузки оператора << для Staff
std::ostream& operator <<(std::ostream& stream, const Staff& staff) {
    stream << staff.ToString();
    return stream;
}
