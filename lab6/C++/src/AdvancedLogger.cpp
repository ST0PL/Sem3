#include <chrono>
#include <iostream>
#include <sstream>
#include <iomanip>
#include "AdvancedLogger.hpp"

void AdvancedLogger::Log(const std::string& text, LogLevel level) const {
    std::cout << "[" << GetNowTime() << "]" << "[" << m_levels.at(level) << "] " << text << std::endl;
}

std::string AdvancedLogger::GetNowTimeVirtual() const {
    auto now = std::chrono::system_clock::now();
    auto time_t = std::chrono::system_clock::to_time_t(now);
    std::tm tm;
    std::stringstream sstream;
    localtime_s(&tm, &time_t);
    sstream << std::put_time(&tm, "%Y-%m-%d %H:%M:%S");
    return sstream.str();
}
const std::map<LogLevel, std::string> AdvancedLogger::m_levels
{
    {LogLevel::eInfo, "Информация"},
    {LogLevel::eWarn, "Предупреждение"},
    {LogLevel::eDebug, "Отладка"},
    {LogLevel::eError, "Ошибка"}
};

AdvancedLogger::~AdvancedLogger() {
    std::cout << "Вызов деструктора AdvancedLogger" << std::endl;
}