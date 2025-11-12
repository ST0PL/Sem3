#pragma once
#include <map>
#include "Enums.hpp"
#include "BaseLogger.hpp"
class AdvancedLogger : public BaseLogger {
public:
    AdvancedLogger(const AdvancedLogger&) = delete;
    void Log(const std::string&, LogLevel) const;
    ~AdvancedLogger() = default;
protected:
    static const std::map<LogLevel, std::string> m_levels;
};