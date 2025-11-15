#pragma once
#include <string>
#include "Enums.hpp"

class ILogger {
public:
    virtual void Log(const std::string&, LogLevel) const = 0;
    virtual ~ILogger() = default;
};