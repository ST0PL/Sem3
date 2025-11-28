#include <iostream>
#include <string>



consteval int Power(int z, int x) {
    int result = 1;
    for (int i = 0; i < x; i++)
        result *= z;
    return result;
}


int main()
{
    constexpr int a = Power(2,10);
    std::cout << "Результат вычисления power(2,10) во время компиляции: " << a << std::endl;
}