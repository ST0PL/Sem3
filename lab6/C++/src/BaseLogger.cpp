#include <chrono>
#include <iostream>
#include <sstream>
#include <iomanip>
#include <format>
#include <ctime>
#include "BaseLogger.hpp"

void BaseLogger::Log(const std::string& text) const {
    std::cout << "[" << GetNowTime("%H:%M:%S") << "]" << "[LOG] " << text << std::endl;
}
std::string BaseLogger::GetNowTime(const std::string format) const {
    auto now = std::chrono::system_clock::now();
    auto time_t = std::chrono::system_clock::to_time_t(now);
    std::tm tm;
    std::stringstream sstream;
    localtime_s(&tm, &time_t);
    sstream << std::put_time(&tm, format.c_str());
    return sstream.str();
}