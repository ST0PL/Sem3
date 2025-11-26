#include <iostream>
#include <memory>
#include "WarehouseRepository.hpp"
#include "Ammunition.hpp"
#include "Fuel.hpp"
#include "Weapon.hpp"
#include "Vehicle.hpp"

using namespace std;


template<typename T>
T CalculateRequirement(T count, T perOne) {
    static_assert(std::is_arithmetic_v<T>);
    return count * perOne;
}

int main() {
    setlocale(LC_ALL, "Rus");
    WarehouseRepository<Ammunition> ammoRepo;

    ammoRepo.Add(make_unique<Ammunition>(1, "5.45mm", Caliber::e545mm, 1000));
    ammoRepo.Add(make_unique<Ammunition>(2, "122mm", Caliber::e122mm, 50));
    ammoRepo.Add(make_unique<Ammunition>(3, "5.45mm", Caliber::e545mm, 500));

    cout << "Размер репозитория боеприпасов: " << ammoRepo.GetSize() << endl;
    cout << "Репозиторий пуст?: " << boolalpha << ammoRepo.IsEmpty() << endl;

    // Поиск по ID
    auto found = ammoRepo.FindById(1);
    if (auto ammo = found.lock()) {
        cout << "Элемент с идентификатором 1: " << ammo->GetName() << endl;
    }

    // Поиск через предикат
    auto result1 = ammoRepo.Find([](const Ammunition* a) {
        return a->GetQuantity() > 100;
        });
    cout << "Боеприпасы в количестве более 100: " << result1.size() << endl;

    // Поиск по типу МТО
    auto result2 = ammoRepo.FindByMaterialType(MaterialType::eAmmunition);
    cout << "Количество через поиск по типу: " << result2.size() << endl;

    // Подсчет количества записей МТО определенного типа
    auto count = ammoRepo.CountByMaterialType(MaterialType::eAmmunition);
    cout << "Количество записей боеприпасов: " << count << endl;

    cout << endl;

    // Удаление по идентификатору
    bool removed = ammoRepo.Remove(2);
    cout << "Удаление элемента с ID 2: " << removed << endl;
    cout << "Размер после удаления: " << ammoRepo.GetSize() << endl;

    cout << "Содержит элемент с ID 1?: " << ammoRepo.ContainsId(1) << endl;
    cout << "Содержит элемент с ID 2? " << ammoRepo.ContainsId(2) << endl;


    WarehouseRepository<Fuel> fuelRepo;
    fuelRepo.Add(make_unique<Fuel>(4, "Дизель", FuelType::eDiesel, 2000));
    fuelRepo.Add(make_unique<Fuel>(5, "Бензин", FuelType::eGasoline, 1500));



    auto allFuel = fuelRepo.FindByMaterialType(MaterialType::eFuel);
    cout << "Количество записей с топливом: " << allFuel.size() << endl;

    for (const auto& weakFuel : allFuel) {
        if (auto fuel = weakFuel.lock()) {
            cout << "Название: " << fuel->GetName()
                << ", Литры: " << fuel->GetQuantity()
                << ", Проверка на пустоту: " << fuel->IsEmpty() << endl;
        }
    }


    // Поиск по наполняемости

    WarehouseRepository<Weapon> weaponRepo;
    weaponRepo.Add(make_unique<Weapon>(6, "AK-74", Caliber::e545mm, 10));
    weaponRepo.Add(make_unique<Weapon>(7, "РСЗО Град", Caliber::e122mm, 5));

    auto nonEmpty = weaponRepo.FindNonEmpty();
    cout << "Не пустое вооружения: " << nonEmpty.size() << endl;

    auto empty = weaponRepo.FindEmpty();
    cout << "Пустое вооружения: " << empty.size() << endl;

    // Массовые операции

    WarehouseRepository<Ammunition> repo;

    vector<unique_ptr<Ammunition>> ammos;
    ammos.push_back(make_unique<Ammunition>(8, "5.45mm", Caliber::e545mm, 100));
    ammos.push_back(make_unique<Ammunition>(9, "122mm", Caliber::e122mm, 20));

    repo.AddRange(move(ammos));
    cout << "После массового добавления: " << repo.GetSize() << endl;

    vector<int> idsToRemove = { 8, 9 };
    removed = repo.RemoveRange(idsToRemove);
    cout << "Удален хотя бы один элемент?: " << removed << endl;
    cout << "Размер после массового удаления: " << repo.GetSize() << endl;

    // Внешняя шаблонная функция

    float distanceKm = 45.3f;
    float fuelPerKm = 0.08f;
    float total_fuel = CalculateRequirement(distanceKm, fuelPerKm);
    std::cout << "Общий расход топлива: " << total_fuel << std::endl;


    return 0;
}
