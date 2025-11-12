#pragma once
#include "AdvancedLogger.hpp"
class FileLogger : public AdvancedLogger {
    using AdvancedLogger::Log;
public:
    FileLogger(const FileLogger&) = delete;
    void Log(const std::string&, const std::string&, LogLevel, bool = false) const;
};