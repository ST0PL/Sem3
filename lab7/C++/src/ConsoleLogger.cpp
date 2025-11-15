#include <chrono>
#include <iostream>
#include <sstream>
#include <iomanip>
#include "AdvancedLogger.hpp"

void ConsoleLogger::Log(const std::string& text, LogLevel level) const {
    std::cout << "[" << GetNowTime() << "]" << "[" << m_levels.at(level) << "] " << text << std::endl;
}

std::string ConsoleLogger::GetNowTime() const {
    auto now = std::chrono::system_clock::now();
    auto time_t = std::chrono::system_clock::to_time_t(now);
    std::tm tm;
    std::stringstream sstream;
    localtime_s(&tm, &time_t);
    sstream << std::put_time(&tm, "%Y-%m-%d %H:%M:%S");
    return sstream.str();
}
const std::map<LogLevel, std::string> ConsoleLogger::m_levels
{
    {LogLevel::eInfo, "Информация"},
    {LogLevel::eWarn, "Предупреждение"},
    {LogLevel::eDebug, "Отладка"},
    {LogLevel::eError, "Ошибка"}
};