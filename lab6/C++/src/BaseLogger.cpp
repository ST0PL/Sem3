#include <chrono>
#include <iostream>
#include <sstream>
#include <iomanip>
#include <format>
#include <ctime>
#include "BaseLogger.hpp"

void BaseLogger::Log(const std::string& text) const {
    std::cout << "[VirtualTime][" << GetNowTimeVirtual() << "]" << "[LOG] " << text << std::endl;
    std::cout << "[NonVirtualTime][" << GetNowTime() << "]" << "[LOG] " << text << std::endl;
}
std::string BaseLogger::GetNowTime() const {
    auto now = std::chrono::system_clock::now();
    auto time_t = std::chrono::system_clock::to_time_t(now);
    std::tm tm;
    std::stringstream sstream;
    localtime_s(&tm, &time_t);
    sstream << std::put_time(&tm, "%H:%M:%S");
    return sstream.str();
}

std::string BaseLogger::GetNowTimeVirtual() const {
    auto now = std::chrono::system_clock::now();
    auto time_t = std::chrono::system_clock::to_time_t(now);
    std::tm tm;
    std::stringstream sstream;
    localtime_s(&tm, &time_t);
    sstream << std::put_time(&tm, "%H:%M:%S");
    return sstream.str();
}
BaseLogger::~BaseLogger() {
    std::cout << "Вызов деструктора BaseLogger" << std::endl;
}