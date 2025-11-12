#include "AdvancedLogger.hpp"
#include "iostream"


void AdvancedLogger::Log(const std::string& text, LogLevel level) const {
    std::cout << "[" << GetNowTime("%H:%M:%S") << "]" << "[" << m_levels.at(level) << "] " << text << std::endl;
}
const std::map<LogLevel, std::string> AdvancedLogger::m_levels
{
    {LogLevel::eInfo, "Информация"},
    {LogLevel::eWarn, "Предупреждение"},
    {LogLevel::eDebug, "Отладка"},
    {LogLevel::eError, "Ошибка"}
};