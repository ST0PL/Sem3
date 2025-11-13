#include <iostream>
#include <memory>
#include "BaseLogger.hpp"
#include "AdvancedLogger.hpp"
#include "FileLogger.hpp"
#include "Resource.hpp"
#include "Ammunition.hpp"
#include "Fuel.hpp"
#include "Equipment.hpp"
#include "Weapon.hpp"
#include "Vehicle.hpp"
#include "Warehouse.hpp"
#include "Unit.hpp"
#include "SupplyRequest.hpp"
#include "SupplyRequestBuilder.hpp"
#include "SupplyResponse.hpp"
#include "SupplyRequestDetail.hpp"

using namespace std;

int main()
{
    setlocale(LC_ALL, "Rus");
    srand(time(0)); // для генерации идентификаторов
    // Производные от WarehouseEntry<T> классы
    Ammunition ammo(1, "Патроны 5.45мм", Caliber::e545mm, 100);
    Fuel fuel(2, "Дизель", FuelType::eDiesel, 500.0f);
    Weapon weapon(3, "АК-12", Caliber::e545mm, 10);
    Vehicle vehicle(4, "БТР-82А", VehicleType::eArmoredVehicle, FuelType::eDiesel, 2);

    // Перегрузка метода базового класса (без вызова базового и с вызовом базового)
    AdvancedLogger advLogger;
    FileLogger fileLogger("log.txt");

    advLogger.Log("AdvancedLogger — без вызова базового метода", LogLevel::eInfo);
    fileLogger.Log("FileLogger — вызывает метод базового класса", LogLevel::eWarn, 1, true);

    // Виртуальные функции
    BaseLogger baseLogger;
    auto basePtr = make_unique<AdvancedLogger>();

    cout << "\nBaseLogger::GetNowTimeVirtual(): " << baseLogger.GetNowTimeVirtual() << endl;
    cout << "\nAdvancedLogger через указатель на BaseLogger:" << endl;
    cout << "\nGetNowTime() (невиртуальная): " << basePtr->GetNowTime() << endl;
    cout << "\nGetNowTimeVirtual() (виртуальная): " << basePtr->GetNowTimeVirtual() << endl << endl;;

    // Вызов виртуальной функции из невиртуальной
    advLogger.BaseLogger::Log("Вызов базовой функии, вызывающей виртуальный и не виртуальный метод GetNowTime()");


    // Клонирование
    auto unit = std::make_shared<Unit>(1, "1-я рота", UnitType::eBattalion);
    SupplyRequestBuilder builder;
    auto req = builder.WithAmmunition(Caliber::e545mm, 100)
        .Create(unit);

    auto shallowClone = req->Clone(true);  // поверхностная копия
    auto deepClone = req->Clone(false);    // глубокая копия

    cout << "\nОригинал ID: " << req->GetId() << endl;
    for (auto& d : req->GetDetails()) cout << "  " << d->ToString() << endl;

    cout << "\nПоверхностная копия. ID: " << shallowClone->GetId() << endl;
    for (auto& d : shallowClone->GetDetails()) cout << "  " << d->ToString() << endl;

    cout << "\nГлубокая копия. ID: " << deepClone->GetId() << endl;
    for (auto& d : deepClone->GetDetails()) cout << "  " << d->ToString() << endl;


    // Вызов конструктора базового класса из производного
    Ammunition ammo2(11, "Патроны 7.62мм", Caliber::e545mm, 200);

    // Абстрактный класс и интерфейс

    auto warehouse = make_shared<Warehouse>(5, "Тыловой склад", WarehouseType::eRear);
    unit->AssignWarehouse(warehouse);
    shared_ptr<SupplyRequest> request; // создаем пустой
    auto response = unit->MakeSupplyRequest(request);

    // Перегрузка оператора присваивания объекта базового класса производному типу

    Fuel fuel2(12, "Бензин", FuelType::eGasoline, 1000.0f);
    fuel2.AssignWarehouse(warehouse);
    Resource& baseRef = fuel2;

    Fuel fuel3(13, "Дизель", FuelType::eDiesel, 75.0f);

    cout << "До присваивания:" << endl;
    cout << "fuel3 ID: " << fuel3.GetId() << endl;
    cout << "fuel3 Name: " << fuel3.GetName() << endl;
    cout << "fuel3 Quantity: " << fuel3.GetQuantity() << endl;
    cout << "fuel3 MeasureUnit: " << static_cast<int>(fuel3.GetMeasureUnit()) << endl;

    fuel3 = baseRef;

    cout << "\nПосле присваивания:" << endl;
    cout << "fuel3 ID: " << fuel3.GetId() << endl;
    cout << "fuel3 Name: " << fuel3.GetName() << endl;
    cout << "fuel3 Quantity: " << fuel3.GetQuantity() << endl;
    cout << "fuel3 MeasureUnit: " << static_cast<int>(fuel3.GetMeasureUnit()) << endl;
    cout << "fuel3 Warehouse ID: "
        << fuel3.GetWarehouse().lock()->GetId() << endl;

    // Виртуальные деструкторы

    {
        cout << "\nДемонстрация вызова цепочки деструкторов" << endl;
        unique_ptr<BaseLogger> virtLogger = make_unique<FileLogger>("file.txt");
    }

    cout << "\nОстаточные деструкторы вызываемые при выходе из main\n" << endl;
    return 0;
}
