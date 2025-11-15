#pragma once
#include <map>
#include "Enums.hpp"
#include "ILogger.hpp"
class ConsoleLogger : public ILogger {
public:
    ConsoleLogger() = default;
    ConsoleLogger(const ConsoleLogger&) = delete;
    void Log(const std::string&, LogLevel) const override;
    virtual std::string GetNowTime() const;
    virtual ~ConsoleLogger() = default;
protected:
    static const std::map<LogLevel, std::string> m_levels;
};