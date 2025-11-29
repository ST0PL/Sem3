#include <iostream>
#include "Vector2D.hpp"


static constexpr int Power(int z, int x) {
    int result = 1;
    for (int i = 0; i < x; i++)
        result *= z;
    return result;
}

constinit Vector2D globalVector(1.0, 1.0);

int main()
{
    setlocale(LC_ALL, "Rus");

    constexpr int a = Power(2, 10);
    std::cout << "Результат вычисления Power(2,10) на этапе компиляции: " << a << std::endl;

    int z = 3;
    int x = 2;
    int b = Power(z, x);
    std::cout << "Результат вычисления Power(z,x), где z = 3, x = 2 на этапе выполнения: " << b << std::endl;


    std::cout << "Constinit вектор ";
    globalVector.Print();

    globalVector.SetX(10.0);
    globalVector.SetY(20.0);

    std::cout << "Constinit вектор после изменения:";
    globalVector.Print();

    // Создание константного объекта Vector2D на этапе компиляции
    constexpr Vector2D vCompileTime(3.0, 4.0);

    // Вызов constexpr метода GetLength() на этапе компиляции
    constexpr double lengthCt = vCompileTime.GetLength();

    // Вызов consteval метода GetNormalized() на этапе компиляции.
    constexpr Vector2D normalized = vCompileTime.GetNormalized();

    std::cout << "Исходный вектор vCompileTime: ";
    vCompileTime.Print();
    std::cout << "Длина вектора: " << lengthCt << std::endl;
    std::cout << "Нормализованный вектор normalized: ";
    normalized.Print();

    // Создание обычного рантайм Vector2D
    Vector2D vRuntimeTime(10.0, 0.0);
    std::cout << "Исходный вектор vRuntimeTime: ";
    vRuntimeTime.Print();

    vRuntimeTime.SetX(5.0);
    vRuntimeTime.SetY(5.0);

    double length_rt = vRuntimeTime.GetLength();

    std::cout << "Вектор после SetX/SetY: ";
    vRuntimeTime.Print();
    std::cout << "Длина вектора: " << length_rt << std::endl;


    return 0;
}
